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

    public void OpenCard() // 相手に表にさせるカードを選ばせる
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
                        backCard[i] = objs[i];
                        backCardCount++;
                    }
                }

                if (backCardCount >= 1)
                {
                    int randomIndex = Random.Range(0, backCardCount); // ランダムに1枚選ぶ
                    backCard[randomIndex].GetComponent<SetSoldier>().IsBack = false; // 選んだカードを表にする
                }
                else
                    Debug.Log("表にするカードがありません");
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
                    lowestCard.GetComponent<SetSoldier>().IsBack = false; // 一番数字が低いカードを表にする
                else
                    Debug.Log("表にするカードがありません");
            }

        }
        else // CPUのターン
        {
            Debug.Log("プレイヤーは表にするカードを選んでください");
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
        }
    }
}
