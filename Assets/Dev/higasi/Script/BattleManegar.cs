using System;
using UnityEngine;

public class BattleManegar : MonoBehaviour
{
    public static CardManegar.Card PlayerCard;
    public static CardManegar.Card EnemyCard;
    public static int TurnCount = 0;

    public enum BattleResult
    {
        Win,
        Lose,
        Draw
    }
    public BattleResult Result;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       TurnCount++;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCard.Power > EnemyCard.Power)
        {
            Result = BattleResult.Win;
            PlayerCard.IsOpen = true;
            EnemyCard.IsLost = true;
        }
        else if (PlayerCard.Power < EnemyCard.Power)
        {
            Result = BattleResult.Lose;
            PlayerCard.IsLost = true;
            EnemyCard.IsOpen = true;
        }
        else
        {
            Result = BattleResult.Draw;
            PlayerCard.IsLost = true;
            EnemyCard.IsLost = true;
        }
    }
}
