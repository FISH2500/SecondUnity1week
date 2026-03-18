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
    public static int TurnCount = 0;

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
        TurnCount++;
        PlayerCardPower = playerCard.GetComponent<Soldier>().CardNum;
        EnemyCardPower = enemyCard.GetComponent<Soldier>().CardNum;
        if (PlayerCardPower > EnemyCardPower)
        {
            Result = BattleResult.Win;
            //playerCard.GetComponent<Soldier>().CardBack = false;
            //enemyCard.GetComponent<Soldier>().CardDie = true;
        }
        else if (PlayerCardPower < EnemyCardPower)
        {
            Result = BattleResult.Lose;
            //playerCard.GetComponent<Soldier>().CardDie = true;
            //enemyCard.GetComponent<Soldier>().CardBack = false;
        }
        else
        {
            Result = BattleResult.Draw; 
            //playerCard.GetComponent<Soldier>().CardDie = true;
            //enemyCard.GetComponent<Soldier>().CardDie = true;
        }
    }

    public void BattleVsGeneral(GameObject playerCard, GameObject enemyCard, bool player)
    {
        TurnCount++;
        // テスト用にカードの強さを固定する
        PlayerCardPower = 6;
        EnemyCardPower = 5;
        //PlayerCardPower = playerCard.GetComponent<Soldier>().CardNum;
        //EnemyCardPower = enemyCard.GetComponent<Soldier>().CardNum;
        if (PlayerCardPower >= EnemyCardPower)
        {
            Result = BattleResult.Win;
            //playerCard.GetComponent<Soldier>().CardBack = false;
            //enemyCard.GetComponent<Soldier>().CardDie = true;
            EndGame = true;
            _playerWin = player;
        }
        else if (PlayerCardPower < EnemyCardPower)
        {
            Result = BattleResult.Lose;
            //playerCard.GetComponent<Soldier>().CardDie = true;
            //enemyCard.GetComponent<Soldier>().CardNum -= playerCard.GetComponent<Soldier>().CardNum;
        }
    }
}
