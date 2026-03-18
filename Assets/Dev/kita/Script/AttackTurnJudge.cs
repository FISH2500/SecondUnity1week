using UnityEngine;

public class AttackTurnJudge : MonoBehaviour
{

    [SerializeField]
    Deck spawnSoldierScr;//カードのスポーンシステム


    void Start()
    {

        //TurnJudge();//攻撃ターンの判定を行う関数
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //攻撃ターンの判定を行う関数
    public void TurnJudge() 
    {
        int[] card = new int[2];//攻撃ターンの兵士のインデックスを格納する配列
        int setCard = 0;//セットされた兵士の数

        while (setCard < 2)
        {
            
            //int randomIdx = Random.Range(0, soldierData.SoldierList.Count);//兵士のデータベースからランダムに兵士を選ぶ

            //if (soldierData.SoldierList[randomIdx].CardPossession) continue;//すでに選ばれている兵士はスキップする

            //spawnSoldierScr.Spawn(randomIdx,setCard);//カードの生成
            //card[setCard++] = soldierData.SoldierList[randomIdx].CardNum;//カードの番号を配列に格納する
            //soldierData.SoldierList[randomIdx].CardPossession = true;
            
        }

        Debug.Log("Player" + card[0]+" CPU" + card[1]);

        if (card[0] > card[1]) 
        {
            Debug.Log("あなたは先攻です。");
        }
        else
        {
            Debug.Log("あなたは後攻です。");
        }

    }
}
