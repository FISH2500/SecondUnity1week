using UnityEngine;

public class GameJudge : MonoBehaviour
{
    [SerializeField]
    int _gameWinCount;

    public static GameJudge Instance;
    static int PlayerWinCount;
    static int CPUWinCount;
    static bool _oneTime;
    private void Awake()
    {
        if (_oneTime)
            Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(this);
        _oneTime = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
