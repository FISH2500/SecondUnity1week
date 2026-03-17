using UnityEngine;

public class Area : MonoBehaviour
{
	[SerializeField] public Vector3[] CardPosition;
	[SerializeField] private float _setDistance = 1.0f; // セットを許容する最大距離

	public void SetAria(GameObject card)
	{
		float nearestPos = Mathf.Infinity; // 一番近い座標までの距離
		int nearestIndex = -1; // 近い座標の配列番号
		float maxDistance = _setDistance * _setDistance; // どこまで近づけたらセットされるかの距離

		// CardPositionの中から引数のカードに一番近い座標を探す
		for (int i = 0; i < CardPosition.Length; i++)
		{
			// カードと現在のCardPosition[i]の距離を計算
			Vector3 diff = card.transform.position - CardPosition[i];
			diff.y = 0; // yの差分を0に

			float dis = diff.sqrMagnitude;

			if (dis < nearestPos) // 近かったならば
			{
				nearestPos = dis; // 変数の更新
				nearestIndex = i; // 変数の更新
			}
		}

		if (nearestIndex != -1 && nearestPos <= maxDistance) // 最も近い場所が_setDistance以内なら
		{
			// 一番近かったインデックスのワールド座標を取得
			Vector3 setPos = CardPosition[nearestIndex];

			// Y座標は変更しない
			setPos.y = card.transform.position.y;

			// カードの位置をセット
			card.transform.position = setPos;

			Debug.Log($"{card.name} をポジション {nearestIndex} にセットしました");
		}
		else
		{
			Debug.Log("有効な範囲内にポジションが見つかりませんでした");
		}
	}
}