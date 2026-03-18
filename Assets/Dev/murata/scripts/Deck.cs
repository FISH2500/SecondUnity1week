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
		for (int i = 0; i < _deck.Count; i++)
		{
			int randomIndex = Random.Range(i, _deck.Count);
			int tmp = _deck[i];
			_deck[i] = _deck[randomIndex];
			_deck[randomIndex] = tmp;
		}
	}

	public GameObject DrawCard()
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