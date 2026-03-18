using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBase : MonoBehaviour
{
	[SerializeField] private CPUArea _cpuArea;
	[SerializeField] private DrawCard _drawCard;

	private bool _hasAction = false;

	private void Update()
	{
		if (TurnManager.instance.CurrentPlayer == 1) return;
		if (TurnManager.instance.IsAction) return;
		if (_hasAction) return;

		_hasAction = true;
		StartCoroutine(SetAction());
	}

	private IEnumerator SetAction()
	{
		yield return new WaitForSeconds(1.0f);

		// 有効なアクション番号を入れるリスト
		List<int> validActions = new List<int>();

		// 攻撃はいつでもできるので追加
		validActions.Add(0);

		// カードが6枚未満ならドロー
		if (_cpuArea.CardNum < 6)
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

				Draw();

				TurnManager.instance.IsAction = true;
				break;
			case 2:
				Debug.Log("CPUがアイテム使用");
				Item();

				TurnManager.instance.UseItem = true;
				StartCoroutine(SetAction());
				yield break;
				break;
		}

		yield return new WaitForSeconds(1.0f);

		TurnManager.instance.ChangeTurn();

		_hasAction = false;
	}

	private void Attack()
	{

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

	}
}