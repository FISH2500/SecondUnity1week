using UnityEngine;

public class AttackTurnJudge : MonoBehaviour
{

    [SerializeField]
    SpawnSoldierScr _spawnSoldierScr;//カードのスポーンシステム

    [SerializeField]
    TurnManager _turnManager;//ターン管理システム

    void Start()
    {
        //TurnJudge();
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
        while (setCard<2) 
        {
            card[setCard]= Random.Range(0, 12);

            setCard++;

            if (card[0] == card[1])//カードが同じ場合は再度カードを引く
            {
                setCard--;
            }

        }

        _spawnSoldierScr.Spawn(card[0],0);//カードのインデックスを基に兵士をスポーンさせる
        _spawnSoldierScr.Spawn(card[1],1);//カードのインデックスを基に兵士をスポーンさせる

        Debug.Log("Player" + card[0]+1+" CPU" + card[1]+1);

        if (card[0] > card[1]) 
        {
            _turnManager.SetTurn(0);//先攻をプレイヤーに設定
            Debug.Log("あなたは先攻です。");
        }
        else
        {
            _turnManager.SetTurn(1);
            Debug.Log("あなたは後攻です。");
        }

    }
}
