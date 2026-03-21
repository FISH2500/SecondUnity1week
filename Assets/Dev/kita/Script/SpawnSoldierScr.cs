using System.Collections.Generic;
using UnityEngine;

public class SpawnSoldierScr : MonoBehaviour
{
    [SerializeField]
    GameObject soldierPrefab;

    [SerializeField]
    Vector3 spawPos;//どの位置から生成し始めるか

    [SerializeField]
    float spawVel;//どれくらいの間隔で生成するか

    [SerializeField]
    Transform[] turnPos;//先攻、後攻を決めるためのカードの配置

    public void SpawnSelectSoldier(List<int> array)//6枚を選択してスポーンする関数 
    {

        for (int i = 0; i < array.Count; i++)
        {
            float posX = spawPos.x+spawVel*i;//一定間隔で並べる

            Vector3 pos= new Vector3(posX, spawPos.y, spawPos.z);//生成する位置を決める

            GameObject soldier = Instantiate(soldierPrefab, pos, Quaternion.identity);

            SetSoldier setSoldier = soldier.GetComponent<SetSoldier>();

            if (setSoldier == null) 
            {
                Debug.LogError("SetCardスクリプトが見つかりません。");
            }

            setSoldier.CardIndex = array[i];//生成したい値をセット
        }
    }

    public GameObject Spawn(int idx,int num)//インデックスを基にスポーン 
    {
        GameObject soldier = Instantiate(soldierPrefab, turnPos[num].position, Quaternion.identity);

        SetSoldier setSoldier = soldier.GetComponent<SetSoldier>();

        if (setSoldier == null)
        {
            Debug.LogError("SetCardスクリプトが見つかりません。");
        }

        setSoldier.CardIndex = idx;//生成したい値をセット

        return soldier;

    }
}
