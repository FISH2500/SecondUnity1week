using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
	[SerializeField] private GameObject _cardPrefab;

	[SerializeField] private List<int> _deck;

	void Start()
	{
		_deck.Clear(); // 一旦空にする

		// 0から12までをループ
		for (int i = 0; i <= 12; i++)
		{
			// 同じ数字を2枚ずつ入れる
			_deck.Add(i);
			_deck.Add(i);
		}

		// シャッフル
		for (int i = 0; i < _deck.Count * 2; i++)
		{
			int idx1 = Random.Range(i, _deck.Count);
			int idx2 = Random.Range(i, _deck.Count);

			int tmp = _deck[idx1];
			_deck[idx1] = _deck[idx2];
			_deck[idx2] = tmp;
		}
	}

	public GameObject DrawCard(int ownerPlayer)
	{
		if (_deck.Count == 0) return null;

		GameObject cardObj = Instantiate(_cardPrefab);

		// 生成したカードのスクリプトに数字を渡す
		SetSoldier sc = cardObj.GetComponent<SetSoldier>();
		sc.CardIndex = _deck[0];

		_deck.RemoveAt(0); // 山札から消す

		return cardObj;
	}
}