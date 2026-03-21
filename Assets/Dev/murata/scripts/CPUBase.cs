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

		bool canDraw = _cpuArea.CardNum < 6 && _drawCard.IsDraw() && !TurnManager.instance.IsDraw;
		bool canUseItem = !TurnManager.instance.UseItem && _cpuItem.GetItemCount() > 0;

		// 1. まずは「確実に勝てる攻撃」があるか
		bool hasSafeAttack = FindSafeAttack(out GameObject bestAttacker, out GameObject bestTarget);

		int action = -1;

		if (hasSafeAttack)
		{
			// 倒せるなら倒す（最優先）
			action = 0;
		}
		else
		{
			// 2. 倒せないなら「効果的なアイテム」があるか聞く
			if (canUseItem && _cpuItem.ShouldUseItem())
			{
				action = 2;
			}
			// 3. アイテムも微妙ならドローする
			else if (canDraw)
			{
				action = 1;
			}
			// 4. 何もできなければ、一番弱いカードで偵察攻撃
			else
			{
				action = 3;
			}
		}

		// --- 実行処理 (switch) ---
		switch (action)
		{
			case 0:
			case 3:
				Attack(action == 0, bestAttacker, bestTarget);
				TurnManager.instance.IsAction = true;
				break;
			case 1:
				Draw();
				TurnManager.instance.ChangeTurn();
				break;
			case 2:
				Debug.Log("CPU：アイテム使用");
				TextManegar.instance.SetText("CPUがアイテムを使用");

				// アイテム実行
				Item();

				// もし使ったアイテムが OpenCard なら、プレイヤーが選び終わるまで待つ
				OpenCard openCardScript = _cpuItem.GetComponent<OpenCard>();
				if (openCardScript != null)
				{
					// プレイヤーがカードを選び終わるまでループ
					while (openCardScript.IsProcessing)
					{
						yield return null; // 1フレーム待機してループ継続
					}
				}

				TurnManager.instance.UseItemFlag();
				yield return new WaitForSeconds(1.0f);

				// 再度アクション評価
				StartCoroutine(SetAction());
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

		// スコア制を導入して、最も「効率の良い」ペアを探す
		float bestEfficiencyScore = float.MinValue;

		GameObject[] myCards = _cpuArea.CardObject;
		GameObject[] enemyCards = _playerArea.CardObj;

		foreach (GameObject eCard in enemyCards)
		{
			if (eCard == null) continue;
			SetSoldier eSol = eCard.GetComponent<SetSoldier>();

			// 攻撃対象の条件：表向き、かつ大将制限クリア、かつ罠でない
			if (eSol.IsBack || (eSol.IsGeneral && _playerArea.CardNum > 1) || eSol.IsTrap) continue;

			foreach (GameObject mCard in myCards)
			{
				if (mCard == null) continue;
				SetSoldier mSol = mCard.GetComponent<SetSoldier>();

				// 自分の大将制限
				if (mSol.IsGeneral && _cpuArea.CardNum > 1) continue;

				// 勝利判定（革命考慮）
				bool wins = isRev ? (mSol.SoldierAtk < eSol.SoldierAtk) : (mSol.SoldierAtk > eSol.SoldierAtk);

				if (wins)
				{
					// --- 効率スコアの計算 ---
					// 1. 基本は「戦力差」が小さいほど良い（オーバーキル防止）
					int powerDiff = Mathf.Abs(mSol.SoldierAtk - eSol.SoldierAtk);
					float efficiency = 100f - powerDiff;

					// 2. 🌟 ここが重要：自分のカードがすでに「表向き」なら評価を大幅アップ
					// すでにバレているカードを再利用するのは「情報の節約」になる
					if (!mSol.IsBack)
					{
						efficiency += 50f;
					}

					if (efficiency > bestEfficiencyScore)
					{
						bestEfficiencyScore = efficiency;
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
		cpuCard.GetComponent<SetSoldier>().RotateSetFront();

		yield return new WaitForSeconds(1.0f);

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

		//攻撃を受けたカードを表にする処理

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