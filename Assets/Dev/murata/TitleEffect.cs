using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TitleEffect : MonoBehaviour
{
	[Header("設定")]
	[SerializeField] private Sprite[] _confettiSprites;
	[SerializeField] private Canvas _targetCanvas;
	[SerializeField] private float _spawnInterval = 0.2f;

	[Header("パラメータ")]
	[SerializeField] private Vector2 _speedRange = new Vector2(100f, 300f);
	[SerializeField] private Vector2 _scaleRange = new Vector2(0.5f, 1.5f);
	[SerializeField] private Color[] _colors = { Color.white, Color.red, Color.yellow, Color.blue, Color.green };

	private float _timer;

	void Start()
	{
		// 画面が埋まるのに必要な時間を想定して一気に生成
		float preWarmTime = 5.0f;
		int initialSpawnCount = Mathf.RoundToInt(preWarmTime / _spawnInterval);

		for (int i = 0; i < initialSpawnCount; i++)
		{
			// 初期配置では「画面の上端」だけでなく「画面全体」に散らばらせたい
			Spawn(isInitial: true);
		}
	}

	void Update()
	{
		_timer += Time.deltaTime;
		if (_timer >= _spawnInterval)
		{
			Spawn(isInitial: false);
			_timer = 0;
		}
	}

	// 引数 isInitial を追加して、初期位置を画面全体にバラけさせる
	void Spawn(bool isInitial = false)
	{
		if (_confettiSprites == null || _confettiSprites.Length == 0) return;

		GameObject obj = new GameObject("Confetti");
		obj.transform.SetParent(_targetCanvas.transform, false);

		Image img = obj.AddComponent<Image>();
		img.sprite = _confettiSprites[Random.Range(0, _confettiSprites.Length)];
		img.color = _colors[Random.Range(0, _colors.Length)];
		img.raycastTarget = false;

		RectTransform rt = obj.GetComponent<RectTransform>();
		float canvasWidth = _targetCanvas.GetComponent<RectTransform>().rect.width;
		float canvasHeight = _targetCanvas.GetComponent<RectTransform>().rect.height;

		float randomX = Random.Range(-canvasWidth / 2, canvasWidth / 2);

		float startY;
		if (isInitial)
		{
			// 初回は画面内のどこかにランダム配置
			startY = Random.Range(-canvasHeight / 2, canvasHeight / 2);
		}
		else
		{
			// 通常時は画面の上から
			startY = canvasHeight / 2 + 50f;
		}

		rt.anchoredPosition = new Vector2(randomX, startY);

		float scale = Random.Range(_scaleRange.x, _scaleRange.y);
		rt.localScale = new Vector3(scale, scale, 1);

		Effect piece = obj.AddComponent<Effect>();
		piece.Setup(Random.Range(_speedRange.x, _speedRange.y), canvasHeight);
	}
}