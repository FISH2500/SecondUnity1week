using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTitle : MonoBehaviour
{
	[Header("シーン遷移の設定")]
	[SerializeField] private float _waitTime = 2.0f;  // 入力有効化までの秒数
	[SerializeField] private string _nextSceneName;   // 遷移先シーン名

	private bool _canInput = false;

	void Start()
	{
		// 一定時間後に入力を許可する
		StartCoroutine(ReadyToStart());
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
			SceneManager.LoadSceneAsync(_nextSceneName);
		}
	}
}
