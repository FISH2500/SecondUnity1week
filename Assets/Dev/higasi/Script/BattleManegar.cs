using System;
using Unity.VisualScripting;
using UnityEngine;
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
       if (EndGame)
       {
            _winImage.gameObject.SetActive(_playerWin);
            _loseImage.gameObject.SetActive(!_playerWin);
            Debug.Log("ÉQÅ[ÉÄèIóπ");
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
			Debug.Log($"BattleíÜé~");
			TurnManager.instance.ChangeTurn();
			return;
		}

		Debug.Log($"BattleäJén: PlayerCard={playerCard.name}, EnemyCard={enemyCard.name}");

		SetSoldier solPlayer = playerCard.GetComponent<SetSoldier>();
		SetSoldier solEnemy = enemyCard.GetComponent<SetSoldier>();

		bool isPlayerGeneral = solPlayer.IsGeneral;
        bool isEnemyGeneral = solEnemy.IsGeneral;
        PlayerCardPower = solPlayer.SoldierAtk;
        EnemyCardPower  = solEnemy.SoldierAtk;

		solPlayer.SetFront();
		solEnemy.SetFront();

        bool isPlayerTrap = solPlayer.IsTrap;

        

        if (TurnManager.instance.CurrentPlayer == 1&&isPlayerTrap) //ìGÇÃÉ^Å[Éì,çUåÇÇµÇΩÉJÅ[ÉhÇ™„©ÇæÇ¡ÇΩèÍçá
        {
            Debug.Log("„©Ç…ÇÊÇËPlayerèüóò");
            Result = BattleResult.Win;
            _cpuArea.RemoveCPUArea(enemyCard);
            Destroy(enemyCard);
            if (isEnemyGeneral)
            {
                EndGame = true;
                _playerWin = true;
            }
        }
        else if (PlayerCardPower > EnemyCardPower)
        {
			Debug.Log("Playerèüóò");
			Result = BattleResult.Win;
			_cpuArea.RemoveCPUArea(enemyCard);
			Destroy(enemyCard);
            if (isEnemyGeneral)
            {
                EndGame = true;
                _playerWin = true;
            }
        }
        else if (PlayerCardPower < EnemyCardPower)
		{
			Debug.Log("Playerîsñk");
			Result = BattleResult.Lose;
			_playerArea.RemoveArea(playerCard);
            Destroy(playerCard);
            if (isPlayerGeneral)
            {
                EndGame = true;
                _playerWin = false;
            }
        }
        else
		{
			Debug.Log("à¯Ç´ï™ÇØ");
			Result = BattleResult.Draw;
			_cpuArea.RemoveCPUArea(enemyCard);
			_playerArea.RemoveArea(playerCard);
			Destroy(playerCard);
			Destroy(enemyCard);

			if (isPlayerGeneral && isEnemyGeneral)
			{
				EndGame = true;
				_playerWin = true;
			}
			else if (isPlayerGeneral)
			{
				EndGame = true;
				_playerWin = false;
			}
			else if (isEnemyGeneral)
			{
				EndGame = true;
				_playerWin = true;
			}
        }

		if (TurnManager.instance.DoubleAttackSimasuka())
		{
			TurnManager.instance.IsDraw = true;
			if (TurnManager.instance.CurrentPlayer == 1) StartCoroutine(_cpuBase.SetAction());
		}
		else
		{
			TurnManager.instance.IsAction = true;
			TurnManager.instance.ChangeTurn();
		}
    }
}
