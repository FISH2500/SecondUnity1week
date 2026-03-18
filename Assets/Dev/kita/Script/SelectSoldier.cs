using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SelectSoldier : MonoBehaviour
{
    
    [SerializeField]
    Deck _deck;//カードのスポーンシステム

    [SerializeField]
    Area _area;

    Vector3 _startCard;//カードスポーンの開始位置

    GameObject _card;

    void Start()
    {

        MeshRenderer _mesh=GetComponent<MeshRenderer>();

        _mesh.enabled = false;//カードスポーンの開始位置をオブジェクトの位置に設定

        _startCard = transform.position;//カードスポーンの開始位置をオブジェクトの位置に設定

        for (int i = 0; i < 6; i++) 
        {
            _card= _deck.DrawCard(0);

            _card.GetComponent<SetSoldier>().SetFront();//カードを表にする

            _startCard.x +=2.0f;//カードを横に並べるための位置

            _card.transform.position = _startCard;//カードを横に並べる

        }

        
        //spawnSoldierScr.SpawnSelectSoldier(SetSoldier());//選ばれた兵士をリストを基にスポーンさせる
        
    }

    void Update()
    {
       
    }

    public void CardSet()//カードの向きをセットする関数
    {
        for(int i = 0; i < 6; i++) 
        {
            if (_area.CardObj[i].GetComponent<SetSoldier>().IsGeneral) continue;//将軍なら表

            _area.CardObj[i].GetComponent<SetSoldier>().SetBack(0);//将軍以外裏面にする
        }
        
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
