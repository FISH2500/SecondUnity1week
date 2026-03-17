using TMPro;
using UnityEngine;

public class Area : MonoBehaviour
{
	[SerializeField] private Vector3[] _cardPosition; // カードをセットする位置
	[SerializeField] private float _setDistance = 1.0f; // セットを許容する最大距離

	[SerializeField] private bool[] _isSet; // カードがセットされているかどうか 見るためにSerializeFieldに

	public bool AllSet = false;

	void Start()
	{
		// _cardPositionの数に合わせて配列を初期化
		_isSet = new bool[_cardPosition.Length];
	}

	public void RemoveAria(GameObject card)
	{
		Vector3 cardPos = card.transform.position;
		cardPos.y = 0;

		for (int i = 0; i < _cardPosition.Length; i++)
		{
			// カードと現在の_cardPosition[i]の距離を計算
			Vector3 areaPos = _cardPosition[i];
			areaPos.y = 0;

			if (cardPos == areaPos) // セットされているなら
			{
				_isSet[i] = false;
				AllSet = false;
				break;
			}
		}
	}

	public void SetAria(GameObject card)
	{
		float nearestPos = Mathf.Infinity; // 一番近い座標までの距離
		int nearestIndex = -1; // 近い座標の配列番号
		float maxDistance = _setDistance * _setDistance; // どこまで近づけたらセットされるかの距離

		// _cardPositionの中から引数のカードに一番近い座標を探す
		for (int i = 0; i < _cardPosition.Length; i++)
		{
			// カードと現在の_cardPosition[i]の距離を計算
			Vector3 diff = card.transform.position - _cardPosition[i];
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
			Vector3 setPos = _cardPosition[nearestIndex];

			// Y座標は変更しない
			setPos.y = card.transform.position.y;

			// カードの位置をセット
			card.transform.position = setPos;

			// フラグを立てる
			_isSet[nearestIndex] = true;

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

			AllSet = checkAll;

			Debug.Log($"{card.name} を {nearestIndex} にセットしました");
			if (AllSet) Debug.Log("すべてのカードがセットされました");
		}
	}
}