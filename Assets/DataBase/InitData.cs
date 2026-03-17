using UnityEngine;

public class InitData : MonoBehaviour
{
    [SerializeField]
    SoldierData soldierData;//兵士のデータベース

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var soldier in soldierData.SoldierList)//カードのデータの変更する部分をすべて初期化する
        {
            soldier.CardArrangement = false;
            soldier.CardDie = false;
            soldier.CardBack = false;
            soldier.CardPossession = false;
            soldier.General = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
