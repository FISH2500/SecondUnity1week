using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetSoldier : MonoBehaviour
{
    public int CardIndex;  //セットしたい兵士の添え字を入力

    public int SoldierAtk; //セットしたい兵士のレベルを入力

	public int OwnerPlayer; // どちらのプレイヤーが持っているか

	public bool IsGeneral; // 大将かどうか

    public bool IsBack; // 裏か表か

    public bool IsTrap; // 罠カードかどうか

    [SerializeField]
    private SoldierData _soldierData;//兵士のデータベース

    [SerializeField]
	private Image _frontImage;//兵士の表の画像を表示するUI 

    [SerializeField]
	private Image _backImage;//兵士の裏の画像を表示するUI

    void Start()
    {
        if (CardIndex > 12) 
        {
            Debug.LogError("兵士の添え字が大きすぎます。");
        }

        SoldierAtk = _soldierData.SoldierList[CardIndex].CardNum;//兵士のレベルをセット

        //Debug.Log("兵士の攻撃力は" + SoldierAtk + "です。");

        SetSprite();
    }

    void SetSprite() 
    {
        if(!_soldierData.SoldierList[CardIndex].CardBack) _frontImage.sprite = _soldierData.SoldierList[CardIndex].CardSprite;//兵士の画像をセット
        else _frontImage.sprite = _soldierData.SoldierBack;//兵士の裏の画像をセット

        _backImage.sprite = _soldierData.SoldierBack;//兵士の裏の画像をセット
    }

    public void SetBack(int OwnerPlayer)//裏面にする
    {
        transform.rotation=Quaternion.Euler(0, 0, 180);

		IsBack = true;

		if (OwnerPlayer == 1)//敵が裏面にした場合攻撃力を非表示
        {
            GetComponent<OutPutAttack>().IsShowText = false;
        }
        else 
        {
            GetComponent<OutPutAttack>().IsShowText = true;
        }
    }

    public void SetFront() //表にする
	{
		IsBack = false;

		transform.rotation = Quaternion.Euler(0, 0, 0);

        GetComponent<OutPutAttack>().IsShowText = false;
    }
}