using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rule : MonoBehaviour
{
	[SerializeField] private float _waitTime = 2.0f;  // 入力有効化までの秒数
	[SerializeField] private Canvas _canvas;
	[SerializeField] private Image _targetImage;       // 切り替える対象のUI Image
	[SerializeField] private Sprite[] _spriteArray;   // スプライトの配列

	private int _currentIndex = 0; // 現在表示している添字

	private bool _canInput = false;

	public static Rule Instance { get; private set; }

	void Start()
	{
		_canvas.enabled = false;
		Instance = this;
		// 初期表示の設定
		UpdateImage();
		StartCoroutine(ReadyToStart());
	}

	// --- 右ボタンなどで呼ぶ関数 ---
	public void NextSprite()
	{
		if (_spriteArray.Length == 0) return;

        SoundManager.Instance.PlaySE("MenuSE");
        _currentIndex++;

		if (_currentIndex >= _spriteArray.Length)
		{
			_currentIndex = _spriteArray.Length - 1;
		}

		UpdateImage();
	}

	// --- 左ボタンなどで呼ぶ関数 ---
	public void PrevSprite()
	{
		if (_spriteArray.Length == 0) return;

        SoundManager.Instance.PlaySE("MenuSE");
        _currentIndex--;

		if (_currentIndex < 0)
		{
			_currentIndex = 0;
		}

		UpdateImage();
	}

	// 画像を実際に更新する処理
	private void UpdateImage()
	{
		if (_targetImage != null && _spriteArray.Length > 0)
		{
			_targetImage.sprite = _spriteArray[_currentIndex];
		}
	}

	private IEnumerator ReadyToStart()
	{
		yield return new WaitForSeconds(_waitTime);
		_canInput = true;
	}

	private void Update()
	{
		if (!_canInput) return;

		if (Input.GetMouseButtonDown(0))
		{
			SoundManager.Instance.PlaySE("MenuSE");
			_canvas.enabled = true;
		}
	}
}
