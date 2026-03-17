using System;
using UnityEngine;

public class BattleManegar : MonoBehaviour
{
    public static int PlayerCardPower;
    public static int EnemyCardPower;
    public static int TurnCount = 0;

    public enum BattleResult
    {
        Win,
        Lose,
        Draw
    }
    public static BattleResult Result;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       TurnCount++;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Battle()
    {
        if (PlayerCardPower > EnemyCardPower)
        {
            Result = BattleResult.Win;
        }
        else if (PlayerCardPower < EnemyCardPower)
        {
            Result = BattleResult.Lose;
        }
        else
        {
            Result = BattleResult.Draw; 
        }
    }
}
