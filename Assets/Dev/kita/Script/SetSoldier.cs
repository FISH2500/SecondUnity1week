using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetSoldier : MonoBehaviour
{
    [SerializeField]
    int soldierIndex;//セットしたい兵士の添え字を入力

    
    private int soldierAtk;//セットしたい兵士のレベルを入力

    [SerializeField]
    SoldierData soldierData;//兵士のデータベース

    [SerializeField]
    Image image;//兵士の画像を表示するUI 
    void Start()
    {
        if (soldierIndex > 12) 
        {
            Debug.LogError("兵士の添え字が大きすぎます。");
        }

        soldierAtk=soldierData.SoldierList[soldierIndex].SolNum;//兵士のレベルをセット

        Debug.Log("兵士の攻撃力は" + soldierAtk + "です。");

    }

    // Update is called once per frame
    void Update()
    {
        SetSprite();
    }

    void SetSprite() 
    {
        

        if(!soldierData.SoldierList[soldierIndex].SolBack) image.sprite = soldierData.SoldierList[soldierIndex].SolSprite;//兵士の画像をセット
        else image.sprite = soldierData.SoldierBack;//兵士の裏の画像をセット

    }

    //void 

}
