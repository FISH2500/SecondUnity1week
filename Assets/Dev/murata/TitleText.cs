using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class TitleText : MonoBehaviour
{
	[Header("設定")]
	[SerializeField] private float _cycleTime = 2.0f; // フェードイン→アウトの1周期の秒数
	[SerializeField] private bool _startVisible = true; // 開始時に表示されているか

	private Image _fadingImage;
	private float _timer;
	private Color _baseColor;

	private void Awake()
	{
		_fadingImage = GetComponent<Image>();
		_baseColor = _fadingImage.color; // 元の色（RGB）を保存

		// 初期状態の設定
		SetAlpha(_startVisible ? 1.0f : 0.0f);

		// サイン波の開始位置を調整（可視状態なら波の頂点から始める）
		if (_startVisible)
		{
			_timer = Mathf.PI / 2f;
		}
	}

	private void Update()
	{
		if (_cycleTime <= 0) return; // エラー回避

		// 時間経過
		_timer += Time.deltaTime;

		// 周期を秒数に変換する公式： (2 * PI) / 周期秒数
		float frequency = (2f * Mathf.PI) / _cycleTime;

		// Sinは -1.0 ～ 1.0 の値を返す
		float sinValue = Mathf.Sin(_timer * frequency);

		// (Sin + 1) で 0.0 ～ 2.0 になり、それを 0.5倍する
		float alpha = (sinValue + 1f) * 0.5f;

		// Imageの色を更新
		SetAlpha(alpha);
	}

	// アルファ値だけを書き換えるヘルパーメソッド
	private void SetAlpha(float alpha)
	{
		_baseColor.a = alpha;
		_fadingImage.color = _baseColor;
	}
}
