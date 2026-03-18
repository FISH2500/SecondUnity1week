using TMPro;
using UnityEngine;
using static CardManegar;

public class Area : MonoBehaviour
{
	[SerializeField] private Transform[] _cardPosition; // カードをセットする位置
	[SerializeField] private float _setDistance = 1.0f; // セットを許容する最大距離

	[SerializeField] private bool[] _isSet; // カードがセットされているかどうか 見るためにSerializeFieldに
	[SerializeField] private GameObject _decision; // 決定ボタン

	[SerializeField] private SoldierData _soldierData;//兵士のデータベース
	[SerializeField] private int _generalIndex;

	[SerializeField] public GameObject[] CardObj;

	public bool AllSet = false;

	void Start()
	{
		// _cardPositionの数に合わせて配列を初期化
		_isSet = new bool[_cardPosition.Length];

		CardObj = new GameObject[_cardPosition.Length];

		for (int i = 0; i < CardObj.Length; i++)
		{
			CardObj[i] = null;
		}
	}

	public void RemoveAria(GameObject card)
	{
		Vector3 cardPos = card.transform.position;
		cardPos.y = 0;

		for (int i = 0; i < _cardPosition.Length; i++)
		{
			// カードと現在の_cardPosition[i]の距離を計算
			Vector3 areaPos = _cardPosition[i].position;
			areaPos.y = 0;

			if (cardPos == areaPos) // セットされているなら
			{
				_isSet[i] = false;
				AllSet = false;
				CardObj[i] = null;
				_decision.SetActive(false);
				card.GetComponent<SetSoldier>().IsGeneral = false;
				break;
			}
		}
	}

	public bool SetAria(GameObject card)
	{
		float nearestPos = Mathf.Infinity; // 一番近い座標までの距離
		int nearestIndex = -1; // 近い座標の配列番号
		float maxDistance = _setDistance * _setDistance; // どこまで近づけたらセットされるかの距離

		// _cardPositionの中から引数のカードに一番近い座標を探す
		for (int i = 0; i < _cardPosition.Length; i++)
		{
			// カードと現在の_cardPosition[i]の距離を計算
			Vector3 diff = card.transform.position - _cardPosition[i].position;
			diff.y = 0; // yの差分を0に

			float dis = diff.sqrMagnitude;

			if (dis < nearestPos) // 近かったならば
			{
				nearestPos = dis; // 変数の更新
				nearestIndex = i; // 変数の更新
			}
		}

		if (nearestIndex != -1 && nearestPos <= maxDistance && !_isSet[nearestIndex]) // 最も近い場所が_setDistance以内でまだセットされていないなら
		{
			// 一番近かったインデックスのワールド座標を取得
			Vector3 setPos = _cardPosition[nearestIndex].position;

			// Y座標は変更しない
			setPos.y = card.transform.position.y;

			// カードの位置をセット
			card.transform.position = setPos;

			// フラグを立てる
			_isSet[nearestIndex] = true;

			// 将軍にする
			if (nearestIndex == _generalIndex)
			{
				card.GetComponent<SetSoldier>().IsGeneral = true;

				Debug.Log($"{card.name} が大将になりました");
			}

			// 全てセットしたか確かめるループ
			bool checkAll = true;
			for (int i = 0; i < _cardPosition.Length; i++)
			{
				if (!_isSet[i]) // falseなら
				{
					checkAll = false;
					break;
				}
			}

			CardObj[nearestIndex] = card; // どの場所にどのカードがあるか保存

			AllSet = checkAll;

			if (AllSet)
			{
				_decision.SetActive(true);
			}

			Debug.Log($"{card.name} を {nearestIndex} にセットしました");
			if (AllSet) Debug.Log("すべてのカードがセットされました");

			return true;
		}
		
		return false;
	}
}