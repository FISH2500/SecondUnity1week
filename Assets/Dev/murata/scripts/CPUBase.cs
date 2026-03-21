using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBase : MonoBehaviour
{
	[SerializeField] private CPUArea _cpuArea;
	[SerializeField] private Area _playerArea;
	[SerializeField] private DrawCard _drawCard;
	[SerializeField] private BattleManegar _battleManegar;
	[SerializeField] private CPUItem _cpuItem;

	public IEnumerator SetAction()
	{
		yield return new WaitForSeconds(1.0f);

		// 1. 現状で可能なアクションをチェック
		bool canDraw = _cpuArea.CardNum < 6 && _drawCard.IsDraw() && !TurnManager.instance.IsDraw;
		bool canUseItem = !TurnManager.instance.UseItem && _cpuItem.GetItemCount() > 0;

		// 2. 盤面を解析し、安全に（確実に）勝てる攻撃があるか探す
		bool hasSafeAttack = FindSafeAttack(out GameObject bestAttacker, out GameObject bestTarget);

		int action = -1; // 0:安全な攻撃, 1:ドロー, 2:アイテム, 3:偵察(ブラインド)攻撃

		if (hasSafeAttack)
		{
			// 確実に勝てる相手がいるなら迷わず攻撃
			action = 0;
		}
		else
		{
			// 勝てる相手が見えていない場合
			if (canUseItem && Random.value < 0.65f) // 65%の確率でアイテムを使って盤面を動かす
			{
				action = 2;
			}
			else if (canDraw)
			{
				// アイテムがない、使わない場合はドローして戦力を整える
				action = 1;
			}
			else
			{
				// ドローもアイテムも使えないなら、被害の少ないカードで特攻する
				action = 3;
			}
		}

		// 3. 決定したアクションの実行
		switch (action)
		{
			case 0:
			case 3:
				Debug.Log(action == 0 ? "CPU：安全な攻撃を実行" : "CPU：やむを得ずブラインド攻撃を実行");
				Attack(action == 0, bestAttacker, bestTarget);
				TurnManager.instance.IsAction = true;
				break;
			case 1:
				Debug.Log("CPU：ドロー");
				TextManegar.instance.SetText("CPUが札を引きました");
				Draw();
				TurnManager.instance.ChangeTurn();
				break;
			case 2:
				Debug.Log("CPU：アイテム使用");
				TextManegar.instance.SetText("CPUがアイテムを使用");
				Item();
				TurnManager.instance.UseItemFlag();
				yield return new WaitForSeconds(1.3f);
				StartCoroutine(SetAction()); // アイテム使用後は再度行動評価
				yield break;
		}
	}

	private void Attack(bool isSafe, GameObject attacker, GameObject target)
	{
		if (!isSafe)
		{
			// 偵察攻撃：一番どうでもいいカードで、裏向きのカードを狙う
			attacker = GetWorstCard();
			target = GetRandomFaceDownEnemy();

			// 裏向きの敵がいなければ、表向きの一番強い敵に特攻（どうせ負けるなら大物を削る期待）
			if (target == null) target = GetStrongestEnemy();
		}

		if (attacker != null && target != null)
		{
			TextManegar.instance.SetText($"CPUの攻撃");
			StartCoroutine(AtkMotion(attacker, target));
		}
		else
		{
			// 万が一ターゲットが見つからなかった場合（エラー回避）
			TurnManager.instance.ChangeTurn();
		}
	}

	private bool FindSafeAttack(out GameObject attacker, out GameObject target)
	{
		attacker = null;
		target = null;
		bool isRev = TurnManager.instance.Revolution;
		int minPowerDiff = int.MaxValue; // 無駄撃ちを防ぐための「戦力差」

		GameObject[] myCards = _cpuArea.CardObject;
		GameObject[] enemyCards = _playerArea.CardObj;

		foreach (GameObject eCard in enemyCards)
		{
			if (eCard == null) continue;
			SetSoldier eSol = eCard.GetComponent<SetSoldier>();

			// 裏向き、大将（条件未達）、または見えている罠は攻撃しない
			if (eSol.IsBack || (eSol.IsGeneral && _playerArea.CardNum > 1) || eSol.IsTrap) continue;

			foreach (GameObject mCard in myCards)
			{
				if (mCard == null) continue;
				SetSoldier mSol = mCard.GetComponent<SetSoldier>();

				if (mSol.IsGeneral && _cpuArea.CardNum > 1) continue;

				// 革命ルールも考慮して勝敗を判定
				bool wins = isRev ? (mSol.SoldierAtk < eSol.SoldierAtk) : (mSol.SoldierAtk > eSol.SoldierAtk);

				if (wins)
				{
					// 勝てるカードの中で、一番「ギリギリで勝てる」カードを選ぶ（エコな攻撃）
					int diff = Mathf.Abs(mSol.SoldierAtk - eSol.SoldierAtk);
					if (diff < minPowerDiff)
					{
						minPowerDiff = diff;
						attacker = mCard;
						target = eCard;
					}
				}
			}
		}
		return attacker != null;
	}

	private GameObject GetWorstCard()
	{
		bool isRev = TurnManager.instance.Revolution;
		GameObject worstCard = null;
		int worstValue = isRev ? -1 : 999; // 革命中は数字が大きいほど「弱い（不用）」

		foreach (GameObject mCard in _cpuArea.CardObject)
		{
			if (mCard == null) continue;
			SetSoldier mSol = mCard.GetComponent<SetSoldier>();
			if (mSol.IsGeneral && _cpuArea.CardNum > 1) continue;

			bool isWorse = isRev ? (mSol.SoldierAtk > worstValue) : (mSol.SoldierAtk < worstValue);
			if (isWorse)
			{
				worstValue = mSol.SoldierAtk;
				worstCard = mCard;
			}
		}
		return worstCard;
	}

	private GameObject GetRandomFaceDownEnemy()
	{
		List<GameObject> faceDowns = new List<GameObject>();
		foreach (GameObject eCard in _playerArea.CardObj)
		{
			if (eCard == null) continue;
			SetSoldier eSol = eCard.GetComponent<SetSoldier>();
			if (eSol.IsGeneral && _playerArea.CardNum > 1) continue;

			if (eSol.IsBack) faceDowns.Add(eCard);
		}
		if (faceDowns.Count > 0) return faceDowns[Random.Range(0, faceDowns.Count)];
		return null;
	}

	private GameObject GetStrongestEnemy()
	{
		bool isRev = TurnManager.instance.Revolution;
		GameObject strongestCard = null;
		int strongestValue = isRev ? 999 : -1;

		foreach (GameObject eCard in _playerArea.CardObj)
		{
			if (eCard == null) continue;
			SetSoldier eSol = eCard.GetComponent<SetSoldier>();
			if (eSol.IsGeneral && _playerArea.CardNum > 1) continue;

			bool isStronger = isRev ? (eSol.SoldierAtk < strongestValue) : (eSol.SoldierAtk > strongestValue);
			if (isStronger)
			{
				strongestValue = eSol.SoldierAtk;
				strongestCard = eCard;
			}
		}
		return strongestCard;
	}

	private void Draw()
	{
		GameObject cardObj = _drawCard.DrawCardCPU();
		if (cardObj != null) _cpuArea.AddCPUArea(cardObj);
	}

	private void Item()
	{
		_cpuItem.CPUUseItem();
	}

	private IEnumerator AtkMotion(GameObject cpuCard, GameObject playerCard)
	{
		Vector3 cardOriginPos = cpuCard.transform.position;
		float speed = 40.0f;
		while (true)
		{
			yield return null;
			if (cpuCard == null || playerCard == null) break; // Null回避
			cpuCard.transform.position = Vector3.MoveTowards(
											cpuCard.transform.position,
											playerCard.transform.position,
											speed * Time.deltaTime);
			if (cpuCard.transform.position == playerCard.transform.position) break;
		}
		_battleManegar.Battle(playerCard, cpuCard);
		StartCoroutine(ReturnMotion(cpuCard, cardOriginPos));
	}

	private IEnumerator ReturnMotion(GameObject cpuCard, Vector3 originPos)
	{
		yield return null;
		if (cpuCard != null)
		{
			float speed = 40.0f;
			while (true)
			{
				yield return null;
				if (cpuCard == null) break;
				cpuCard.transform.position = Vector3.MoveTowards(
												cpuCard.transform.position,
												originPos,
												speed * Time.deltaTime);
				if (cpuCard.transform.position == originPos) break;
			}
		}
	}
}