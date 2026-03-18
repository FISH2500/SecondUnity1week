using UnityEngine;

public class AttackTurnJudge : MonoBehaviour
{

    [SerializeField]
    SpawnSoldierScr _spawnSoldierScr;//カードのスポーンシステム

    [SerializeField]
    TurnManager _turnManager;//ターン管理システム

    [SerializeField]
    GameObject _backGround;

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
        _backGround.SetActive(true);//背景を消す

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

        GameObject[] player = new GameObject[2];//PlayerとCPUのカードを格納する配列

        OutPutAttack[] outPutAttack = new OutPutAttack[2];//PlayerとCPU用の3Dテキストスクリプトの配列を格納

        for (int i = 0; i < 2; i++)
        {
            player[i] = _spawnSoldierScr.Spawn(card[i], i);//Player,CPUカードをスポーンさせる

            outPutAttack[i] = player[i].GetComponent<OutPutAttack>();//カードから3Dテキストスクリプトを取得する

            outPutAttack[i].IsShowText = false;//先攻、後攻のカードのため3D数字を非表示

            CardMove(player,i);
        }

        //カードを裏から徐々に回転


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

    void CardMove(GameObject[] player,int i) //カードに関する動作処理まとめ
    {

        //カードをズームさせる
        player[i].transform.position = new Vector3(1.5f, 8.0f, 0.0f);//カードをズームさせるための位置
    }
}
