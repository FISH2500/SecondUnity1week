using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
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

    int PlayerCardPower;
    int EnemyCardPower;

    public enum BattleResult
    {
        Win,
        Lose,
        Draw
    }
    public static BattleResult Result;
    public static bool EndGame = false;
    bool _playerWin = false;

    void Update()
    {
		int gameJudgeIndex = 0; // 0:続行 1:プレイヤーの勝利 2:CPUの勝利
		if (EndGame)
		{
			_winImage.gameObject.SetActive(_playerWin);
			_loseImage.gameObject.SetActive(!_playerWin);
			gameJudgeIndex = GameJudge.Instance.Judge(_playerWin);
			if (gameJudgeIndex == 0)
			{
				EndGame = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
            Debug.Log("ゲーム終了");
		}
		else
		{
			_winImage.gameObject.SetActive(false);
			_loseImage.gameObject.SetActive(false);
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
			}
			else // CPUが罠を踏んだ
			{
				ProcessVictory(enemyCard, isEnemyGeneral);
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
		Destroy(enemyCard,3.0f);
		if (isEnemyGeneral)
		{
			EndGame = true;
			_playerWin = true;
		}
	}

	// 敗北時の共通処理
	private void ProcessDefeat(GameObject playerCard, bool isPlayerGeneral, bool playerWinsGame)
	{
		Result = BattleResult.Lose;
		_playerArea.RemoveArea(playerCard);
		Destroy(playerCard,3.0f);
		if (isPlayerGeneral)
		{
			EndGame = true;
			_playerWin = playerWinsGame;
		}
	}

	// 引き分け時の共通処理
	private void ProcessDraw(GameObject pCard, GameObject eCard, bool pGen, bool eGen)
	{
		Result = BattleResult.Draw;
		_cpuArea.RemoveCPUArea(eCard);
		_playerArea.RemoveArea(pCard);
		Destroy(pCard, 3.0f);
		Destroy(eCard, 3.0f);

		if (pGen || eGen)
		{
			EndGame = true;
			_playerWin = eGen; // 両方落ちた場合や大将が落ちた場合の判定
		}
	}

	// ターン終了の管理
	private void HandleTurnEnd()
	{
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
		yield return new WaitForSeconds(5.0f);


        HandleTurnEnd();
    }

}
