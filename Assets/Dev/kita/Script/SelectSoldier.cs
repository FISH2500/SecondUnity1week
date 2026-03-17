using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SelectSoldier : MonoBehaviour
{
    [SerializeField]
    SoldierData soldierData;//兵士のデータベース
    
    [SerializeField]
    SpawnSoldierScr spawnSoldierScr;//カードのスポーンシステム


    void Start()
    {
        //カードの所持状態をリセットする
        for (int i = 0; i < soldierData.SoldierList.Count; i++) 
        {
            soldierData.SoldierList[i].CardPossession = false;
        }
        
        spawnSoldierScr.SpawnSelectSoldier(SetSoldier());//選ばれた兵士をリストを基にスポーンさせる
        
    }

    void Update()
    {
       
    }

    //選ばれた6枚をリストに格納する関数
    List<int> SetSoldier()
    {
        List<int> solArray = new List<int>();//選択された兵士のリスト

        int selectCnt = 0;//選択された回数

        //13枚の中から6枚をランダムで並べる
        while (selectCnt < 6)
        {
            int randomIdx = Random.Range(0, soldierData.SoldierList.Count);//0～兵士のリスト分の範囲でランダムな値を生成

            if (!soldierData.SoldierList[randomIdx].CardPossession)//手札に加える処理
            {
                selectCnt++;//選択された回数を増やす
                solArray.Add(randomIdx);//選ばれた兵士のインデックスをリストに格納

                soldierData.SoldierList[randomIdx].CardPossession = true;//所持している
            }

        }

        return solArray;
    }
    
}
