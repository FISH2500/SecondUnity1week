using UnityEngine;

public class AttackTurnJudge : MonoBehaviour
{

    [SerializeField]
    SpawnSoldierScr _spawnSoldierScr;//カードのスポーンシステム

    [SerializeField]
    GameObject _backGround;

    GameObject[] player = new GameObject[2];//PlayerとCPUのカードを格納する配列

    private bool _isRoate;//カードの回転フラグ

    float _t = 0.0f;//時間経過

    private Quaternion _startRot;//開始回転
    private Quaternion _endRot= Quaternion.Euler(0.0f, 0.0f, 0.0f);//最終角度

    [SerializeField]
    float _rotateSpeed;//回転速度

    int[] card = new int[2];//攻撃ターンの兵士のインデックスを格納する配列
    void Start()
    {
        //TurnJudge();
        //TurnJudge();//攻撃ターンの判定を行う関数

        _startRot = Quaternion.Euler(0.0f, 0.0f, 180.0f); ;//回転の始まりの角度
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRoate) 
        {
            _t += Time.deltaTime * _rotateSpeed;//時間経過を加算
            

            //z値を0から180度に回転させる
            for (int i = 0; i < 2; i++)
                player[i].transform.rotation = Quaternion.Lerp(_startRot, _endRot, _t);


            if (_t > 1.0f)
            {
                _isRoate = false;//回転終了

                //数秒後に背景、カードを消す
                Invoke("TurnJudgeEnd", 2.0f);


            }
        }
    }
    //攻撃ターンの判定を行う関数
    public void TurnJudge() 
    {
        _backGround.SetActive(true);//背景を消す

        
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

        

        OutPutAttack[] outPutAttack = new OutPutAttack[2];//PlayerとCPU用の3Dテキストスクリプトの配列を格納

        SetSoldier[] setSoldier =new SetSoldier[2];//PlayerとCPU用のSetSoldierスクリプトの配列を格納

        for (int i = 0; i < 2; i++)
        {
            player[i] = _spawnSoldierScr.Spawn(card[i], i);//Player,CPUカードをスポーンさせる

            outPutAttack[i] = player[i].GetComponent<OutPutAttack>();//カードから3Dテキストスクリプトを取得する

            outPutAttack[i].IsShowText = false;//先攻、後攻のカードのため3D数字を非表示

            setSoldier[i]= player[i].GetComponent<SetSoldier>();

            setSoldier[i].SetBack(1);//カードを裏にする
        }

        //カードを裏から徐々に回転
        
        _isRoate = true;

        Debug.Log("Player : " + (int)(card[0] + 1) +" CPU : " + (int)(card[1] + 1));



    }

    void TurnJudgeEnd()//ターン終了時の関数 
    {
        _backGround.SetActive(false);//背景を消す
        for (int i = 0; i < 2; i++)
        {
            Destroy(player[i]);//カードを消す
        }

        if (card[0] > card[1])
        {
            TurnManager.instance.SetTurn(0);//先攻をプレイヤーに設定
            Debug.Log("あなたは先攻です。");
        }
        else
        {
            TurnManager.instance.SetTurn(1);
            Debug.Log("あなたは後攻です。");
        }

        //DispUI.instance.Disp(true);//UIを表示する
    }
}
