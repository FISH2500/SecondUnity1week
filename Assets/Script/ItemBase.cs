using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// abstractを付けることで「これ単体では実体化できない」親クラスになります
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

	public bool IsBack;

	private void Start()
	{
		SetSprite();
		Highlight(false);
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
			case 0:
				Shuffle();
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				AddDraw();
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
		}

		DispUI.instance.Disp(true);
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
	}

	private void AddDraw()
	{
		DrawCard.instance.AddDrawNum(1);
	}
}
