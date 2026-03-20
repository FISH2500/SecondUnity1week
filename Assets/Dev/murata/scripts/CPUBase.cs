using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CPUBase : MonoBehaviour
{
	[SerializeField] private CPUArea _cpuArea;
	[SerializeField] private Area _playerArea;
	[SerializeField] private DrawCard _drawCard;
	[SerializeField] private BattleManegar _battleManegar;
	[SerializeField] private CPUItem _cpuItem;

	//private void Update()
	//{
	//	if (TurnManager.instance.CurrentPlayer != 1) return;
	//	if (TurnManager.instance.IsAction) return;
	//	if (_hasAction) return;

	//	_hasAction = true;
	//	StartCoroutine(SetAction());
	//}

	public IEnumerator SetAction()
	{
		yield return new WaitForSeconds(1.0f);

		// 有効なアクション番号を入れるリスト
		List<int> validActions = new List<int>();

		// 攻撃はいつでもできるので追加
		validActions.Add(0);

		// カードが6枚未満でまだドローできるなら
		if (_cpuArea.CardNum < 6 && _drawCard.IsDraw() && !TurnManager.instance.IsDraw)
		{
			validActions.Add(1);
		}

		// アイテムを持っていて、まだ使っていないなら
		if (!TurnManager.instance.UseItem /* && アイテムを持っているなら */)
		{
			validActions.Add(2);
		}

		// リストの中からランダムに1つ選ぶ
		int idx = Random.Range(0, validActions.Count);
		int action = validActions[idx];

		switch (action)
		{
			case 0:
				Debug.Log("CPUが攻撃");
                Attack();
                
                TurnManager.instance.IsAction = true;
				break;
			case 1:
				Debug.Log("CPUがドロー");
                TextManegar.instance.SetText("CPUが札を引きました");
                Draw();

				TurnManager.instance.ChangeTurn();

				break;
			case 2:
				Debug.Log("CPUがアイテム使用");
                TextManegar.instance.SetText("CPUがアイテムを使用");
                Item();

				TurnManager.instance.UseItemFlag();
				StartCoroutine(SetAction()); // 再帰
				yield break;
		}

		yield return new WaitForSeconds(1.0f);


		_hasAction = false;
	}

	private void Attack()
	{
		// 攻撃の処理
		GameObject strongestCard = null;
		int strongestAtk = -1;
		GameObject[] cpuCard = _cpuArea.CardObject;

		GameObject targetCard = null;
		int targetAtk = -1;
		GameObject[] playerCard = _playerArea.CardObj;

		for (int i = 0; i < 6; i++) // 所持カードの中から、最強のカードを選択
		{
			if (cpuCard[i] == null) continue;

			SetSoldier soldier = cpuCard[i].GetComponent<SetSoldier>();

			if (soldier.IsGeneral && _cpuArea.CardNum > 1) continue;  // 大将は攻撃できないのでスキップ、大将のみの場合は攻撃可能

			if (soldier.SoldierAtk > strongestAtk)
			{
				strongestCard = cpuCard[i];
				strongestAtk = soldier.SoldierAtk;
			}
		}

		string log = $"{strongestAtk}でCPUの攻撃 ";

        TextManegar.instance.SetText(log);
        bool hasTarget = false;
		// 表になっている相手のカードの中から、CPUのカードより攻撃力が低いカードの中で
		// 一番攻撃力が高いカードを選択
		// 表になっている相手のカードが全てCPUのカードより攻撃力が高い
		// もしくは表になっている相手のカードがない場合は、裏のカードをランダムに選択する
		for (int i = 0; i < 6; i++)
		{
			if (playerCard[i] == null) continue;

			SetSoldier playerSoldier = playerCard[i].GetComponent<SetSoldier>();

			if (playerSoldier.IsGeneral)
			{
				if (_playerArea.CardNum > 1) continue;

				targetCard = playerCard[i];
				hasTarget = true;
				break; // 大将が狙えるなら確定
			}

			if (playerSoldier.SoldierAtk > targetAtk && // 現在ターゲットになっているカードより強い
				playerSoldier.SoldierAtk < strongestAtk && // CPUのカードより弱い
				!playerSoldier.IsBack) // 表になっているカード
			{
				targetCard = playerCard[i];
				targetAtk = playerSoldier.SoldierAtk;
				hasTarget = true;
			}
		}

		if (!hasTarget) // ターゲットがない場合のランダム処理
		{
			List<GameObject> candidates = new List<GameObject>();

			for (int i = 0; i < 6; i++)
			{
				if (playerCard[i] == null) continue;

				var s = playerCard[i].GetComponent<SetSoldier>();

				if (s.IsGeneral && _playerArea.CardNum > 1) continue;

				candidates.Add(playerCard[i]);
			}

			if (candidates.Count > 0)
			{
				targetCard = candidates[UnityEngine.Random.Range(0, candidates.Count)];
			}
		}

		_battleManegar.Battle(targetCard, strongestCard);
	}

	private void Draw()
	{
		GameObject cardObj = _drawCard.DrawCardCPU();

		if (cardObj != null)
		{
			_cpuArea.AddCPUArea(cardObj);
		}
	}

	private void Item()
	{
		_cpuItem.CPUUseItem();
	}
}