using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SelectSoldier : MonoBehaviour
{
    
    [SerializeField]
    Deck _deck;//カードのスポーンシステム

    [SerializeField]
    Vector3 startCard;//カードスポーンの開始位置


    void Start()
    {


        for(int i = 0; i < 6; i++) 
        {
            GameObject card= _deck.DrawCard();

            startCard.x +=2.0f;//カードを横に並べるための位置

            card.transform.position = startCard;//カードを横に並べる

        }

        
        //spawnSoldierScr.SpawnSelectSoldier(SetSoldier());//選ばれた兵士をリストを基にスポーンさせる
        
    }

    void Update()
    {
       
    }

    //選ばれた6枚をリストに格納する関数
    //List<int> SetSoldier()
    //{
    //    List<int> solArray = new List<int>();//選択された兵士のリスト

    //    int selectCnt = 0;//選択された回数

    //    //13枚の中から6枚をランダムで並べる
    //    while (selectCnt < 6)
    //    {
    //        int randomIdx = Random.Range(0, soldierData.SoldierList.Count);//0～兵士のリスト分の範囲でランダムな値を生成

    //        if (!soldierData.SoldierList[randomIdx].CardPossession)//手札に加える処理
    //        {
    //            selectCnt++;//選択された回数を増やす
    //            solArray.Add(randomIdx);//選ばれた兵士のインデックスをリストに格納

    //            soldierData.SoldierList[randomIdx].CardPossession = true;//所持している
    //        }

    //    }

    //    return solArray;
    //}
    
}
