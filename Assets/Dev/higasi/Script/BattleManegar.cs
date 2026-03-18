using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleManegar : MonoBehaviour
{
    [SerializeField]
    Image _winImage;
    [SerializeField]
    Image _loseImage;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       if (EndGame)
       {
            _winImage.gameObject.SetActive(_playerWin);
            _loseImage.gameObject.SetActive(!_playerWin);
            Debug.Log("ゲーム終了");
       }
       else
       {
                _winImage.gameObject.SetActive(false);
                _loseImage.gameObject.SetActive(false);
       }
    }

    public void Battle(GameObject playerCard, GameObject enemyCard, bool player)
    {
        bool isPlayerGeneral = playerCard.GetComponent<SetSoldier>().IsGeneral;
        bool isEnemyGeneral = enemyCard.GetComponent<SetSoldier>().IsGeneral;
        PlayerCardPower = playerCard.GetComponent<SetSoldier>().SoldierAtk;
        EnemyCardPower = enemyCard.GetComponent<SetSoldier>().SoldierAtk;
        if (PlayerCardPower > EnemyCardPower)
        {
            Result = BattleResult.Win;
            playerCard.GetComponent<SetSoldier>().IsBack = false;
            Destroy(enemyCard);
            if (isEnemyGeneral)
            {
                EndGame = true;
                _playerWin = player;
            }
        }
        else if (PlayerCardPower < EnemyCardPower)
        {
            Result = BattleResult.Lose;
            Destroy(playerCard);
            enemyCard.GetComponent<SetSoldier>().IsBack = false;
            if (isPlayerGeneral)
            {
                EndGame = true;
                _playerWin = !player;
            }
        }
        else
        {
            Result = BattleResult.Draw;
            Destroy(playerCard);
            Destroy(enemyCard);
            if (isPlayerGeneral || isEnemyGeneral)
            {
                EndGame = true;
                _playerWin = true;
            }
        }
    }

    public void BattleVsGeneral(GameObject playerCard, GameObject enemyCard, bool player)
    {
        PlayerCardPower = playerCard.GetComponent<SetSoldier>().SoldierAtk;
        EnemyCardPower = enemyCard.GetComponent<SetSoldier>().SoldierAtk;
        //// テスト用にカードの強さを固定する
        //PlayerCardPower = 6;
        //EnemyCardPower = 5;
        
        if (PlayerCardPower >= EnemyCardPower)
        {
            Result = BattleResult.Win;
            playerCard.GetComponent<SetSoldier>().IsBack = false;
            Destroy(enemyCard);
            EndGame = true;
            _playerWin = player;
        }
        else if (PlayerCardPower < EnemyCardPower)
        {
            Result = BattleResult.Lose;
            Destroy(playerCard);
            enemyCard.GetComponent<SetSoldier>().SoldierAtk -= playerCard.GetComponent<SetSoldier>().SoldierAtk;
        }
    }
}
