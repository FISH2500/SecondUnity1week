using System;
using UnityEngine;

public class DrawMouse : MonoBehaviour
{
	[SerializeField] private Camera _camera; // rayを飛ばすためのカメラ
	[SerializeField] private Area _area; // セットするためのエリア

	[SerializeField] private GameObject _selectDestroyCardUI;//カードを破棄する時に表示されるUI

    private GameObject _dragObj = null; // 今ドラッグしているオブジェクト
	private float _zDistance = 0; // 選択した時のZ軸の位置

	[NonSerialized] public GameObject DrawObject;

	private float _yPos;//カードのYの固定位置

    private void OnEnable()//スクリプトが有効になるたびに呼ばれる
    {
		if (_area.AllSet)//カードがすべてセットされている場合
		{
			SetDestroySelectCardWindow();
        }
    }

    void Update()
	{
		// クリックされた瞬間
		if (Input.GetMouseButtonDown(0))
		{
			// マウスの画面上の位置から、カメラの奥に向かう光線を作成
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) // 光線を飛ばして当たって
			{
				if (!hit.collider.CompareTag("Card")) return; // Card以外ならこれ以上行わない

				GameObject obj = hit.collider.gameObject;

				if (DrawObject == obj)
				{
					// 選択したオブジェクトを保存
					_dragObj = obj;
					// Zの位置を保存
					_zDistance = _camera.WorldToScreenPoint(_dragObj.transform.position).z;
					_yPos = obj.transform.position.y;
				}
				else if(_area.AllSet)//破棄するカードを選択
                {
					ReSetDestroySelectCardWindow();
					_area.RemoveArea(obj);
                    _selectDestroyCardUI.SetActive(false);
                    Destroy(obj);
				}

				// ログを流す
				Debug.Log("クリックしたオブジェクト: " + hit.collider.gameObject.name);
			}
		}

		if (_dragObj == null) return;

		// ドラッグ中
		if (Input.GetMouseButton(0))
		{
			Plane plane = new Plane(Vector3.up, new Vector3(0, _yPos, 0));

			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

			if (plane.Raycast(ray, out float enter))
			{
				Vector3 hitPoint = ray.GetPoint(enter);

				_dragObj.transform.position = hitPoint;
			}
		}

		// 指を離した時
		if (Input.GetMouseButtonUp(0))
		{
			if(_area.SetAria(_dragObj))
			{
				TurnManager.instance.ChangeTurn();
				enabled = false;
			}

			_dragObj = null; // ドラッグしているオブジェクトをNULLに
		}
	}

	void SetDestroySelectCardWindow() 
	{
        for (int i = 0; i < _area.CardObj.Length; i++)
        {
            _area.CardObj[i].GetComponent<SetOutLine>().SetOutline();//セットされているすべてのカードにアウトラインをつける
        }

        _selectDestroyCardUI.SetActive(true);
    }

    void ReSetDestroySelectCardWindow()
    {
        for (int i = 0; i < _area.CardObj.Length; i++)
        {
            _area.CardObj[i].GetComponent<SetOutLine>().ReSetOutline();//アウトラインを消す
        }

        _selectDestroyCardUI.SetActive(false);
    }
}