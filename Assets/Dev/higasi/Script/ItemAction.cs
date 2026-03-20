using System.Collections;
using UnityEngine;

public class ItemAction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shuffle() // 大将以外のカードを裏にして、シャッフルする
    {
        GameObject[] objs;
        Vector3[] movePos;
        if (TurnManager.instance.CurrentPlayer == 0) // プレイヤーのターン
        {
            Debug.Log("プレイヤーのデッキをシャッフル");
            objs = GameObject.FindGameObjectsWithTag("Card");
        }
        else // CPUのターン
        {
            Debug.Log("CPUのデッキをシャッフル");
            objs = GameObject.FindGameObjectsWithTag("Card");
        }
        int count = objs.Length;// カードの残り枚数を取得
        int[] moveIndex = { -1 };
        movePos = new Vector3[count - 1];
        for (int i = 0; i < count; i++)
        {
            if (objs[i].GetComponent<SetSoldier>().IsGeneral) continue; // 大将は裏にしない
            objs[i].GetComponent<SetSoldier>().IsBack = true;// カードを裏にする
            movePos[i] = objs[i].transform.position;// カードの位置を取得
            bool loop = true;
            while (loop)
            {
                int randomIndex = Random.Range(0, count - 1);
                if (moveIndex[randomIndex] == -1)
                {
                    loop = false;
                    moveIndex[randomIndex] = i;// シャッフルするためのランダムなインデックスを生成
                }
            }
        }

        for (int i = 0; i < count - 1; i++)
        {
            objs[i].transform.position = movePos[moveIndex[i]];// カードの位置をシャッフル
        }

    }

    public bool OpenCard() // 相手に表にさせるカードを選ばせる
    {
        if (TurnManager.instance.CurrentPlayer == 0) // プレイヤーのターン
        {// CPUに表にするカードを選ばせる
            Debug.Log("CPUは表にするカードを選んでください");
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Player2Card");
            int count = objs.Length;// カードの残り枚数を取得
            // ===== 以下のCPUの行動パターンは後ほど調整したほうがいいかもです =====
            int selectPattern = Random.Range(0, 2); // 0ならランダムに1枚、1なら一番数字が低いカードを表にする
            if (selectPattern == 0)
            {
                GameObject[] backCard = new GameObject[count];
                int backCardCount = 0;
                for (int i = 0; i < count; i++) // 裏のカードを配列に格納
                {
                    if (objs[i].GetComponent<SetSoldier>().IsBack) // 裏のカードなら
                    {
                        backCard[backCardCount] = objs[i];
                        backCardCount++;
                    }
                }

                if (backCardCount >= 1)
                {
                    int randomIndex = Random.Range(0, backCardCount); // ランダムに1枚選ぶ
                    backCard[randomIndex].GetComponent<SetSoldier>().IsBack = false; // 選んだカードを表にする
                    return true;
                }
                else
                {
                    Debug.Log("表にするカードがありません");
                    return false;
                }
            }
            else if (selectPattern == 1)
            {
                GameObject lowestCard = null;
                for (int i = 0; i < count; i++)
                {
                    if (objs[i].GetComponent<SetSoldier>().IsBack) // 裏のカードなら
                    {
                        if (lowestCard == null || objs[i].GetComponent<SetSoldier>().SoldierAtk < lowestCard.GetComponent<SetSoldier>().SoldierAtk)
                        {
                            lowestCard = objs[i];
                        }
                    }
                }
                if (lowestCard != null)
                {
                    lowestCard.GetComponent<SetSoldier>().IsBack = false; // 一番数字が低いカードを表にする
                    return true;
                }
                else
                {
                    Debug.Log("表にするカードがありません");
                    return false;
                }
            }
            else
            {
                Debug.Log("不正な選択パターン");
                return false;
            }
        }
        else // CPUのターン
        { // プレイヤーに表にするカードを選ばせる
            Debug.Log("プレイヤーは表にするカードを選んでください");

            // 裏のカードがあるか確認
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");
            int count = objs.Length;
            bool hasBackCard = false;
            for (int i = 0; i < count; i++)
            {
                if (objs[i].GetComponent<SetSoldier>().IsBack == true)
                    hasBackCard = true;
            }
            if (!hasBackCard)
            {
                Debug.Log("表にするカードがありません");
                return false;
            }

            bool loop = true;
            while (loop) // カードが選ばれるまでループ
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 mousePos = Input.mousePosition;
                    Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
                    RaycastHit hit;

                    if (Physics.Raycast(selectRay, out hit, 100.0f))
                    {
                        GameObject hitObj = hit.collider.gameObject;
                        if (hitObj.CompareTag("Card") && hitObj.GetComponent<SetSoldier>().IsBack) // プレイヤーのカードで裏のカードなら
                        {
                            hitObj.GetComponent<SetSoldier>().IsBack = false;
                            loop = false;
                        }
                        else
                        {
                            if (!hitObj.CompareTag("Card"))
                                Debug.Log("自軍のカードを選んでください");
                            else if (!hitObj.GetComponent<SetSoldier>().IsBack)
                                Debug.Log("裏のカードを選んでください");
                        }
                    }
                }
            }
            return true;
        }
    }

    public bool ChangeCard() // 一枚だけ相手のカードと自分のカードを交換する
    {
        GameObject playerCard = null;
        GameObject cpuCard = null;
        bool playerCardSelected = false;
        bool cpuCardSelected = false;
        bool click = false;
        if (TurnManager.instance.CurrentPlayer == 0) // プレイヤーのターン
        {
            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Card");
            int playerCount = playerObjs.Length;
            GameObject[] cpuObjs = GameObject.FindGameObjectsWithTag("Player2Card");
            int cpuCount = cpuObjs.Length;
            if (playerCount <= 1 || cpuCount <= 1)
            {
                Debug.Log("残りが大将だけなので交換できません");
                return false;
            }
            bool loop = true;
            while (loop) // カードが選ばれるまでループ
            {
                if (Input.GetMouseButton(0) && !click)
                {
                    click = true;
                    Vector3 mousePos = Input.mousePosition;
                    Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
                    RaycastHit hit;

                    if (Physics.Raycast(selectRay, out hit, 100.0f))
                    {
                        Debug.Log("hit");
                        GameObject hitObj = hit.collider.gameObject;
                        if (hitObj.CompareTag("Card"))
                        {
                            if (!playerCardSelected)
                                playerCardSelected = true;
                            playerCard = hitObj;
                        }
                        if (hitObj.CompareTag("Player2Card"))
                        {
                            if (!cpuCardSelected)
                                cpuCardSelected = true;
                            cpuCard = hitObj;
                        }
                    }
                }
                else if (!Input.GetMouseButton(0) && click)
                {
                    click = false;
                }

                if (playerCardSelected && cpuCardSelected) // 両方のカードが選ばれたら交換
                {
                    if (playerCard != null && cpuCard != null)
                    {
                        loop = false;
                        SetSoldier tmpSetSoldeir;
                        tmpSetSoldeir = playerCard.GetComponent<SetSoldier>();
                        playerCard.GetComponent<SetSoldier>().CardIndex = cpuCard.GetComponent<SetSoldier>().CardIndex;
                        playerCard.GetComponent<SetSoldier>().SoldierAtk = cpuCard.GetComponent<SetSoldier>().SoldierAtk;
                        playerCard.GetComponent<SetSoldier>().IsGeneral = cpuCard.GetComponent<SetSoldier>().IsGeneral;
                        cpuCard.GetComponent<SetSoldier>().CardIndex = tmpSetSoldeir.CardIndex;
                        cpuCard.GetComponent<SetSoldier>().SoldierAtk = tmpSetSoldeir.SoldierAtk;
                        cpuCard.GetComponent<SetSoldier>().IsGeneral = tmpSetSoldeir.IsGeneral;
                    }
                }
            }
            return true;
        }
        else // CPUのターン
        {
            // CPUのカード選択
            // 攻撃力が最低のカードを選ぶ
            GameObject[] cpuObjs = GameObject.FindGameObjectsWithTag("Player2Card");
            int count = cpuObjs.Length;
            GameObject lowestCard = null;
            for (int i = 0; i < count; i++)
            {
                if (cpuObjs[i].GetComponent<SetSoldier>().IsGeneral) continue;
                if (lowestCard == null || cpuObjs[i].GetComponent<SetSoldier>().SoldierAtk < lowestCard.GetComponent<SetSoldier>().SoldierAtk)
                {
                    lowestCard = cpuObjs[i];
                }
            }
            if (lowestCard != null)
            {
                cpuCard = lowestCard;
            }
            else
            {
                Debug.Log("残りが大将だけなので交換できません");
                return false;
            }

            // プレイヤーのカード選択
            // 表になっているカードの中で、CPUのカードより攻撃力が高いカードを選ぶ
            // 上記に当てはまるカードがない場合、裏のカードからランダムに選ぶ
            GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Card");
            int playerCount = playerObjs.Length;
            int strongestAtk = 0;
            for (int i = 0; i < playerCount; i++)
            {
                if (playerObjs[i].GetComponent<SetSoldier>().IsBack) continue;
                if (playerObjs[i].GetComponent<SetSoldier>().SoldierAtk > cpuCard.GetComponent<SetSoldier>().SoldierAtk &&
                    playerObjs[i].GetComponent<SetSoldier>().SoldierAtk > strongestAtk)
                {
                    playerCard = playerObjs[i];
                    strongestAtk = playerObjs[i].GetComponent<SetSoldier>().SoldierAtk;
                }
            }

            if (playerCard == null)
            {// ランダムに選ぶ
                GameObject[] backCard = new GameObject[count];
                int backCardCount = 0;
                for (int i = 0; i < count; i++) // 裏のカードを配列に格納
                {
                    if (playerObjs[i].GetComponent<SetSoldier>().IsBack) // 裏のカードなら
                    {
                        backCard[backCardCount] = playerObjs[i];
                        backCardCount++;
                    }
                }

                if (backCardCount >= 1)
                {
                    int randomIndex = Random.Range(0, backCardCount); // ランダムに1枚選ぶ
                    playerCard = backCard[randomIndex];
                }
                else
                {
                    Debug.Log("残りが大将だけなので交換できません");
                    return false;
                }
            }

            if (playerCard != null && cpuCard != null)
            {
                SetSoldier tmpSetSoldeir;
                tmpSetSoldeir = playerCard.GetComponent<SetSoldier>();
                playerCard.GetComponent<SetSoldier>().CardIndex = cpuCard.GetComponent<SetSoldier>().CardIndex;
                playerCard.GetComponent<SetSoldier>().SoldierAtk = cpuCard.GetComponent<SetSoldier>().SoldierAtk;
                playerCard.GetComponent<SetSoldier>().IsGeneral = cpuCard.GetComponent<SetSoldier>().IsGeneral;
                cpuCard.GetComponent<SetSoldier>().CardIndex = tmpSetSoldeir.CardIndex;
                cpuCard.GetComponent<SetSoldier>().SoldierAtk = tmpSetSoldeir.SoldierAtk;
                cpuCard.GetComponent<SetSoldier>().IsGeneral = tmpSetSoldeir.IsGeneral;
                return true;
            }
            else
            {
                Debug.Log("カードの選択に失敗しました");
                return false;
            }
        }
    }

    public IEnumerator OneMoreTurn()
    {
        int turn = TurnManager.instance.CurrentPlayer;
        yield return new WaitUntil(() => turn != TurnManager.instance.CurrentPlayer);
        if (BattleManegar.Result == BattleManegar.BattleResult.Win)
        {
            TextManegar.instance.SetText("もう一度攻撃できます");
            TurnManager.instance.ChangeTurn();
        }
    }

    public void Trap()
    {

    }
}
