using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBase : MonoBehaviour
{
	public int ItemID;        // 識別ID

	[SerializeField] ItemDataBase _itemDatabase;

	[SerializeField]
	private Image _frontImage;//兵士の表の画像を表示するUI 

	[SerializeField]
	private Image _backImage;//兵士の裏の画像を表示するUI

	[SerializeField]
	private MeshRenderer _highlight;

	[SerializeField]
	private bool IsCPU;

	[SerializeField]
	SwapCard _swapCard;

	[SerializeField]
	OpenCard _openCard;

	[SerializeField]
	Item_RedrawTarget item_RedrawTarget;

	[SerializeField]
	Item_SwapItem _item_SwapItem;

	public bool IsBack;

	private void Start()
	{
		if (!IsCPU) SetSprite();
		if (!IsCPU) Highlight(false);
	}

	void SetSprite()
	{
		_frontImage.sprite = _itemDatabase.itemDatas[ItemID].Front;//兵士の画像をセット

		_backImage.sprite = _itemDatabase.Back;//兵士の裏の画像をセット
	}

	public void Highlight(bool highlight)
	{
		_highlight.enabled = highlight;
	}

	public void SetBack()//裏面にする
	{
		transform.rotation = Quaternion.Euler(0, 0, 180);

		IsBack = true;
	}

	public void SetFront() //表にする
	{
		IsBack = false;

		transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public void Use()
	{
		switch (ItemID)
		{
			case 0: // ①大将以外のカードを裏にして、シャッフルする
				Shuffle();
				break;
			case 1: // ②選んだ自分のカードに罠を張る
				Trap();
				break;
			case 2: // ③相手に表にさせるカードを選ばせる
				Reverse();
				break;
			case 3: // ④カードの引ける枚数を+1する
				AddDraw();
				break;
			case 4: // ⑤このターンのみカードの強さを逆転させる
				StrengthInversion();
				break;
			case 5: // ⑥1枚だけ相手のカードと自分のカードを交換する
				SwapCard();
				break;
			case 6: // ⑦数値勝負に勝つと、もう一回攻撃できる
				AdditionalAttack();
				break;
			case 7: // ⑧1枚だけ相手のアイテムカードと自分のを交換する
				SwapItem();
				break;
			case 8: // ⑨このターン中、アイテムの使用制限がなくなる
				UnlimitedItem();
				break;
			case 9: // ⑩相手のカードを一枚選んで、カードを引き直させる
				ReDrawTarget();
				break;
		}
	}

	public void Shuffle()
	{
		// 現在のターンから対象のAreaを取得
		bool isPlayer = (TurnManager.instance.CurrentPlayer == 0);
		GameObject[] targetCards;
		Transform[] positions;
		float cardY;

		if (isPlayer)
		{
			Area playerArea = Area.Instance;
			targetCards = playerArea.CardObj;
			positions = playerArea.GetCardPositions();
		}
		else
		{
			CPUArea cpuArea = CPUArea.Instance;
			targetCards = cpuArea.CardObject;
			positions = cpuArea.CardPosition;
		}

		List<int> shuffleIndices = new List<int>();
		for (int i = 0; i < targetCards.Length; i++)
		{
			if (targetCards[i] == null) continue;

			SetSoldier s = targetCards[i].GetComponent<SetSoldier>();
			if (s.IsGeneral) continue;

			s.SetBack(TurnManager.instance.CurrentPlayer);
			shuffleIndices.Add(i);
		}

		List<int> randomIndices = new List<int>(shuffleIndices);
		for (int i = randomIndices.Count - 1; i > 0; i--)
		{
			int j = Random.Range(0, i + 1);
			(randomIndices[i], randomIndices[j]) = (randomIndices[j], randomIndices[i]);
		}

		GameObject[] originalObjs = (GameObject[])targetCards.Clone();

		for (int i = 0; i < shuffleIndices.Count; i++)
		{
			int oldIdx = shuffleIndices[i];
			int newIdx = randomIndices[i];

			targetCards[oldIdx] = originalObjs[newIdx];

			if (targetCards[oldIdx] != null)
			{
				cardY = targetCards[oldIdx].transform.position.y;

				Vector3 newPos = positions[oldIdx].position;
				newPos.y = cardY;

				targetCards[oldIdx].transform.position = newPos;
			}
		}

		DispUI.instance.Disp(true);
	}

	private void Trap()
	{

	}

	private void Reverse()
	{
		_openCard.RunOpenCard();
	}

	private void AddDraw()
	{
		DrawCard.instance.AddDrawNum(1);
		DispUI.instance.Disp(true);
	}

	private void StrengthInversion()
	{

	}

	private void SwapCard()
	{
		_swapCard.StartSwapMode();
	}

	private void AdditionalAttack()
	{
		TurnManager.instance.SetDoubleAttack();
	}

	private void SwapItem()
	{
		_item_SwapItem.StartItemSwap();
	}

	private void UnlimitedItem()
	{
		TurnManager.instance.SetUnlimitedItem();
	}

	private void ReDrawTarget()
	{
		item_RedrawTarget.StartRedraw();
	}
}
