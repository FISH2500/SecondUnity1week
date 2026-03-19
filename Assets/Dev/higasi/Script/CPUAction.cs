using UnityEngine;

public class CPUAction : MonoBehaviour
{
    BattleManegar _bm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bm = GameObject.Find("BattleManegar").GetComponent<BattleManegar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        // 攻撃の処理
        GameObject strongestCard = null;
        int strongestAtk = -1;
        GameObject[] cpuCard = GameObject.FindGameObjectsWithTag("Player2Card");
        int count;

        GameObject targetCard = null;
        int targetAtk = -1;
        GameObject[] playerCard = GameObject.FindGameObjectsWithTag("Card");

        count = cpuCard.Length;// カードの残り枚数を取得
        for (int i = 0; i < count; i++) // 所持カードの中から、最強のカードを選択e
        {
            if (cpuCard[i].GetComponent<SetSoldier>().IsGeneral && count >= 2) // 大将は攻撃できないのでスキップ
                continue;                                                      // 大将のみの場合は攻撃可能
            if (cpuCard[i].GetComponent<SetSoldier>().SoldierAtk > strongestAtk)
            {
                strongestCard = cpuCard[i];
                strongestAtk = strongestCard.GetComponent<SetSoldier>().SoldierAtk;
            }
        }

        count = cpuCard.Length;// カードの残り枚数を取得
        bool hasTarget = false;
        // 表になっている相手のカードの中から、CPUのカードより攻撃力が低いカードの中で
        // 一番攻撃力が高いカードを選択
        // 表になっている相手のカードが全てCPUのカードより攻撃力が高い
        // もしくは表になっている相手のカードがない場合は、裏のカードをランダムに選択する
        for (int i = 0; i < count; i++)
        {
            if (playerCard[i].GetComponent<SetSoldier>().IsGeneral)
            {
                if (count >= 2) // 大将以外のカードが残っていたら無効
                    continue;
                // 大将のみの場合は攻撃可能
                else
                {
                    targetCard = playerCard[i];
                    targetAtk = targetCard.GetComponent<SetSoldier>().SoldierAtk;
                    hasTarget = true;
                    break;
                }
            }
            if (playerCard[i].GetComponent<SetSoldier>().SoldierAtk > targetAtk && // 現在ターゲットになっているカードより強い
                playerCard[i].GetComponent<SetSoldier>().SoldierAtk < strongestAtk && // CPUのカードより弱い
                !playerCard[i].GetComponent<SetSoldier>().IsBack) // 表になっているカード
            {
                targetCard = playerCard[i];
                targetAtk = targetCard.GetComponent<SetSoldier>().SoldierAtk;
                hasTarget = true;
            }
        }

        if (!hasTarget) // ターゲットがない場合のランダム処理
        {
            bool loop = true;
            while(loop)
            {
                int index = Random.Range(0, count);
                if (playerCard[index].GetComponent<SetSoldier>().IsGeneral && count >= 2) // 大将は攻撃できないのでスキップ
                    continue;
                targetCard = playerCard[index];
                loop = false;
            }
        }

        _bm.Battle(strongestCard, targetCard);
    }
}
