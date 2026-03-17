using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetCard : MonoBehaviour
{
    public int CardIndex;//セットしたい兵士の添え字を入力

    
    private int CarddierAtk;//セットしたい兵士のレベルを入力

    [SerializeField]
    SoldierData soldierData;//兵士のデータベース

    [SerializeField]
    Image frontImage;//兵士の表の画像を表示するUI 

    [SerializeField]
    Image backImage;//兵士の裏の画像を表示するUI 
    void Start()
    {
        if (CardIndex > 12) 
        {
            Debug.LogError("兵士の添え字が大きすぎます。");
        }

        CarddierAtk=soldierData.SoldierList[CardIndex].CardNum;//兵士のレベルをセット

        Debug.Log("兵士の攻撃力は" + CarddierAtk + "です。");

        SetSprite();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSprite() 
    {
        if(!soldierData.SoldierList[CardIndex].CardBack) frontImage.sprite = soldierData.SoldierList[CardIndex].CardSprite;//兵士の画像をセット
        else frontImage.sprite = soldierData.SoldierBack;//兵士の裏の画像をセット

        backImage.sprite = soldierData.SoldierBack;//兵士の裏の画像をセット


    }

    //void 

}
