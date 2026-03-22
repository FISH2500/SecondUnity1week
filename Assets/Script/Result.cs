using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
	[SerializeField]
	Image _win; 

	[SerializeField]
	Image _lose;


	void Start()
    {
		GameObject judge = GameObject.Find("GameJudge");
		bool b = judge.GetComponent<GameJudge>().PlayerWin();
		if (judge != null) Destroy(judge);

		_win.enabled = b;
		_lose.enabled = !b;
	}
}
