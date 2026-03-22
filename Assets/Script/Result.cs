using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
	[SerializeField]
	Image _win; 

	[SerializeField]
	Image _lose;

	[SerializeField] string _nextSceneName = "Title";
	[SerializeField] float _waitTime = 2.0f;

	[SerializeField] GameObject _scene;

	private bool _canSkip = false;

	void Start()
    {
		GameObject judge = GameObject.Find("GameJudge");
		bool b = judge.GetComponent<GameJudge>().PlayerWin();
		
		_win.enabled = b;
		_lose.enabled = !b;

		Destroy(judge);

		StartCoroutine(WaitBeforeClick());
	}

	private IEnumerator WaitBeforeClick()
	{
		yield return new WaitForSeconds(_waitTime);

		_canSkip = true;
		Debug.Log("クリックで遷移可能になりました");
	}

	private void Update()
	{
		if (_canSkip && Input.GetMouseButtonDown(0))
		{
			StartCoroutine(sk());
		}
	}

	private IEnumerator sk()
	{
		Instantiate(_scene);

		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nextSceneName);
		asyncLoad.allowSceneActivation = false;

		yield return new WaitForSeconds(0.7f);

		asyncLoad.allowSceneActivation = true;
	}
}
