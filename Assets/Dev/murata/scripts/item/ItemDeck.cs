using System.Collections.Generic;
using UnityEngine;

public class ItemDeck : MonoBehaviour
{
	[SerializeField] private List<int> _deck;

	[SerializeField] private GameObject _itemObj;

	void Awake()
	{
		_deck.Clear(); // 一旦空にする

		// 0から9までをループ
		for (int i = 0; i < 10; i++)
		{
			_deck.Add(i);
		}

		// シャッフル
		for (int i = 0; i < _deck.Count * 2; i++)
		{
			int idx1 = Random.Range(0, _deck.Count);
			int idx2 = Random.Range(0, _deck.Count);

			(_deck[idx2], _deck[idx1]) = (_deck[idx1], _deck[idx2]);
		}
	}

	public GameObject DrawItem()
	{
		if (_deck.Count == 0) return null;

		GameObject cardObj = Instantiate(_itemObj);

		// 生成したカードのスクリプトに数字を渡す
		cardObj.GetComponent<ItemBase>().ItemID = _deck[0];

		_deck.RemoveAt(0); // 山札から消す

		return cardObj;
	}

	public int CPUDrawItem()
	{
		if (_deck.Count == 0) return -1;
		
		int id = _deck[0];

		_deck.RemoveAt(0); // 山札から消す

		return id;
	}
}
