using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManegar : MonoBehaviour
{
    [SerializeField]
    Image _winImage;

    [SerializeField]
    Image _loseImage;

	[SerializeField]
	private Area _playerArea;

	[SerializeField]
	private CPUArea _cpuArea;

	[SerializeField]
	private CPUBase _cpuBase;

	[SerializeField]
	string _sceneName;

	[SerializeField]
	GameObject _loadCanvas;

	[SerializeField]
	private float _setGameEndUI;//ゲームの結果が出力されるまでの時間

	[SerializeField]
	private float _setTurnTime;

	[SerializeField]
	private Image _fadeImage;

	[SerializeField]
	private Color _fadeColor;

	[SerializeField]
	private float _fadeTime;

    int PlayerCardPower;
    int EnemyCardPower;

    public enum BattleResult
    {
        Win,
        Lose,
        Draw
    }
    public static BattleResult Result;
    public bool EndGame = false;

	public GameObject DefeatCrad;//敗北したカード

    bool _playerWin = false;

    IEnumerator GameEnd(bool kousan)
    {

        if (!kousan) yield return new WaitForSeconds(_setGameEndUI);

        int gameJudgeIndex = 0; // 0:続行 1:プレイヤーの勝利 2:CPUの勝利

		if (_playerWin)
			SoundManager.Instance.PlaySE("gameWin");
		else
			SoundManager.Instance.PlaySE("gameLose");

			_fadeImage.enabled = true;
		float elapsed = 0f;

		Color startColor = _fadeColor;
		startColor.a = 0f;

		Color targetColor = _fadeColor;

		while (elapsed < _fadeTime)
		{
			elapsed += Time.deltaTime;

			float t = Mathf.Clamp01(elapsed / _fadeTime);

			_fadeImage.color = Color.Lerp(startColor, targetColor, t);

			yield return null;
		}

		_fadeImage.color = targetColor;

		_winImage.gameObject.SetActive(_playerWin);
		_loseImage.gameObject.SetActive(!_playerWin);

		// 3秒待機
		yield return new WaitForSeconds(3.0f);

		gameJudgeIndex = GameJudge.Instance.Judge(_playerWin);

		// 続行の場合
		if (gameJudgeIndex == 0)
		{
			EndGame = false;
			Instantiate(_loadCanvas);
			// 現在のシーンをリロード
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("S_game");
			asyncLoad.allowSceneActivation = false;

			yield return new WaitForSeconds(0.7f);

			asyncLoad.allowSceneActivation = true;
		}
		else
		{
			if (gameJudgeIndex == 1)
			{

			}
			else
			{

			}
			Instantiate(_loadCanvas);
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
			asyncLoad.allowSceneActivation = false;
			yield return new WaitForSeconds(1.0f);
			asyncLoad.allowSceneActivation = true;
		}
	}

	public void Battle(GameObject playerCard, GameObject enemyCard)
	{
		if (playerCard == null || enemyCard == null)
		{
			Debug.Log($"Battle中止");
			TurnManager.instance.ChangeTurn();
			return;
		}

		SetSoldier solPlayer = playerCard.GetComponent<SetSoldier>();
		SetSoldier solEnemy = enemyCard.GetComponent<SetSoldier>();

		bool isPlayerGeneral = solPlayer.IsGeneral;
		bool isEnemyGeneral = solEnemy.IsGeneral;
		PlayerCardPower = solPlayer.SoldierAtk;
		EnemyCardPower = solEnemy.SoldierAtk;

		solPlayer.SetFront();
		solEnemy.SetFront();

		// --- 1. 罠(Trap)の判定 ---
		// 現在のターンプレイヤーが、罠の設置されたカードを攻撃してしまった場合
		bool isTrapped = (TurnManager.instance.CurrentPlayer == 0 && solEnemy.IsTrap) ||
						 (TurnManager.instance.CurrentPlayer == 1 && solPlayer.IsTrap);

        if (isTrapped)
        {
            Debug.Log("罠発動！攻撃側が破壊されました");
            if (TurnManager.instance.CurrentPlayer == 0) // プレイヤーが罠を踏んだ
            {
                ProcessDefeat(playerCard, isPlayerGeneral, false);
                TextManegar.instance.SetText("罠にかかってしまった...");
            }
            else // CPUが罠を踏んだ
            {
                ProcessVictory(enemyCard, isEnemyGeneral);
                TextManegar.instance.SetText("相手が罠にかかった！");
            }
        }
        // --- 2. 通常の数値バトル (革命対応) ---
        else
		{
			// 革命フラグを取得
			bool isRev = TurnManager.instance.Revolution;

			// 勝利・敗北の条件式を革命フラグで分岐
			bool winCondition = isRev ? (PlayerCardPower < EnemyCardPower) : (PlayerCardPower > EnemyCardPower);
			bool loseCondition = isRev ? (PlayerCardPower > EnemyCardPower) : (PlayerCardPower < EnemyCardPower);

			if (winCondition)
			{
				Debug.Log(isRev ? "革命中：Player勝利" : "Player勝利");
				ProcessVictory(enemyCard, isEnemyGeneral);
			}
			else if (loseCondition)
			{
				Debug.Log(isRev ? "革命中：Player敗北" : "Player敗北");
				ProcessDefeat(playerCard, isPlayerGeneral, false);
			}
			else
			{
				Debug.Log("引き分け");
				ProcessDraw(playerCard, enemyCard, isPlayerGeneral, isEnemyGeneral);
			}
		}

		// --- 3. ターン終了・再行動処理 ---
		StartCoroutine(TurnChange(playerCard));
    }

	// 勝利時の共通処理
	private void ProcessVictory(GameObject enemyCard, bool isEnemyGeneral)
	{
		Result = BattleResult.Win;
		_cpuArea.RemoveCPUArea(enemyCard);
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			Destroy(enemyCard, _setTurnTime);
		}
		else
		{
			Destroy(enemyCard);
			SoundManager.Instance.PlaySE("tear");
            StartCoroutine(SoundWait("battleWin", 0.8f));
        }
        DefeatCrad = enemyCard;
		
        if (isEnemyGeneral)
		{
			EndGame = true;
			_playerWin = true;
			StartCoroutine(GameEnd(false));
		}
	}

	// 敗北時の共通処理
	private void ProcessDefeat(GameObject playerCard, bool isPlayerGeneral, bool playerWinsGame)
	{
		Result = BattleResult.Lose;
		_playerArea.RemoveArea(playerCard);
		DefeatCrad = playerCard;
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			Destroy(playerCard, _setTurnTime);
		}
		else
		{
			Destroy(playerCard);
            SoundManager.Instance.PlaySE("tear");
			StartCoroutine(SoundWait("battleLose", 0.8f));
        }
        if (isPlayerGeneral)
		{
			EndGame = true;
			_playerWin = playerWinsGame;
			StartCoroutine(GameEnd(false));
		}
	}

	// 引き分け時の共通処理
	private void ProcessDraw(GameObject pCard, GameObject eCard, bool pGen, bool eGen)
	{
		Result = BattleResult.Draw;
		_cpuArea.RemoveCPUArea(eCard);
		_playerArea.RemoveArea(pCard);
		StartCoroutine(SoundWait("tear", 1.5f));
		Destroy(pCard, _setTurnTime);
		Destroy(eCard, _setTurnTime);

		if (pGen || eGen)
		{
			EndGame = true;
			_playerWin = eGen; // 両方落ちた場合や大将が落ちた場合の判定
			StartCoroutine(GameEnd(false));
		}
	}

	// ターン終了の管理
	private void HandleTurnEnd()
	{
		if (EndGame) return;

		if (TurnManager.instance.DoubleAttackSimasuka())
		{
			TurnManager.instance.IsDraw = true;
			DispUI.instance.Disp(true);
			if (TurnManager.instance.CurrentPlayer == 1) StartCoroutine(_cpuBase.SetAction());
		}
		else
		{
			TurnManager.instance.IsAction = true;
			TurnManager.instance.ChangeTurn();
		}
	}

    private IEnumerator TurnChange(GameObject moveCard) 
	{
		if (TurnManager.instance.CurrentPlayer == 0)//Playerなら演出がある為5秒待たせる
		{
			Debug.Log("waitooooooo");
			yield return new WaitForSeconds(_setTurnTime);
		}

        HandleTurnEnd();
    }

	private IEnumerator SoundWait(string soundName, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
        SoundManager.Instance.PlaySE(soundName);
    }

	public void kousan()
	{
		if (EndGame) return; // すでに終わってたら何もしない

		DispUI.instance.Disp(false);

		EndGame = true;
		_playerWin = false; // プレイヤーの負け確定
		StartCoroutine(GameEnd(true)); // 敗北演出開始
	}
}
