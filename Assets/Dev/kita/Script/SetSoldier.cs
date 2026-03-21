using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetSoldier : MonoBehaviour
{
	private const float _rotateTime = 0.3f; // 回転にかかる時間
	private bool _isRotating = false;

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

    void Awake()
    {
        if (CardIndex > 12) 
        {
            Debug.LogError("兵士の添え字が大きすぎます。");
        }

        SoldierAtk = _soldierData.SoldierList[CardIndex].CardNum;//兵士のレベルをセット

        //Debug.Log("兵士の攻撃力は" + SoldierAtk + "です。");

        SetSprite();
    }

    public void SetSprite() 
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

	public void RotateSetBack(int ownerPlayer)
	{
		StopAllCoroutines(); // 連続で呼ばれた時のために現在の回転を止める
		StartCoroutine(AnimateRotate(true, ownerPlayer));
	}

	public void RotateSetFront()
	{
		StopAllCoroutines();
		StartCoroutine(AnimateRotate(false, 0));
	}

	// --- 回転アニメーションの実体 ---
	private IEnumerator AnimateRotate(bool toBack, int ownerPlayer)
	{
		_isRotating = true;

		Quaternion startRot = transform.rotation;
		// 裏面なら Z180度、表面なら Z0度
		Quaternion endRot = toBack ? Quaternion.Euler(0, 0, 180) : Quaternion.Euler(0, 0, 0);

		float elapsed = 0;
		while (elapsed < _rotateTime)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / _rotateTime;
			// 球面線形補間で滑らかに回転
			transform.rotation = Quaternion.Slerp(startRot, endRot, t);
			yield return null;
		}

		// 最終状態の確定
		transform.rotation = endRot;
		IsBack = toBack;

		// --- 表示/非表示のロジック ---
		if (toBack)
		{
			// 裏面にする時：CPU(1)なら非表示、プレイヤー(0)なら表示
			GetComponent<OutPutAttack>().IsShowText = (ownerPlayer == 0);
		}
		else
		{
			// 表にする時：全員数値を隠す（画像自体に数字がある想定、またはテキスト不要な場合）
			GetComponent<OutPutAttack>().IsShowText = false;
		}

		_isRotating = false;
	}
}