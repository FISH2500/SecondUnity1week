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
}
