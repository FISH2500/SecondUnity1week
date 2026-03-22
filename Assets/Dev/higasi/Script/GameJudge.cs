using Unity.VisualScripting;
using UnityEngine;

public class GameJudge : MonoBehaviour
{
    [SerializeField]
    int _gameWinCount;

    public static GameJudge Instance;
    static int PlayerWinCount;
    static int CPUWinCount;
    static bool _oneTime = false;
    private void Awake()
    {
		if (_oneTime)
		{
			Destroy(gameObject);
			return;
		}
		else
		{
			PlayerWinCount = 0;
			CPUWinCount = 0;
			Instance = this;
			DontDestroyOnLoad(this);
			_oneTime = true;
		}
    }

    public int Judge(bool playerWin)
    {
        if (playerWin)
        {
            PlayerWinCount++;
        }
        else
        {
            CPUWinCount++;
        }
        if (PlayerWinCount >= _gameWinCount)
            return 1;
        if (CPUWinCount >= _gameWinCount)
            return 2;
        return 0;
    }

	private void OnDestroy()
	{
		if (Instance == this)
		{
			_oneTime = false;
			Instance = null;
		}
	}

	public int ReturnMatchPoint(bool IsPlayer)
	{
		if (IsPlayer)
		{
			return PlayerWinCount;
		}
		else
		{
			return CPUWinCount;
		}
	}

	public bool PlayerWin()
	{
		return PlayerWinCount >= _gameWinCount;
	}
}
