using System;
using UnityEngine;

public class DrawMouse : MonoBehaviour
{
	[SerializeField] private Camera _camera; // レイを飛ばすためのカメラ
	[SerializeField] private Area _area; // セットするためのエリア
	[SerializeField] private Canvas _selectDestroyCardUI; // 破壊選択UI

	private GameObject _dragObj = null; // 現在ドラッグしているオブジェクト
	private float _zDistance = 0; // 選択した時の奥行き位置

	[NonSerialized] public GameObject DrawObject;

	private float _yPos; // カードのY軸固定位置

	private void Awake()
	{
		if (_area.AllSet) SetDestroySelectCardWindow();
	}

	void Update()
	{
		// クリックされた瞬間
		if (Input.GetMouseButtonDown(0))
		{
			// マウスの画面上の位置から、カメラの奥に向かうレイを作成
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) // 何かに当たった場合
			{
				// Cardタグ以外ならこれ以上実行しない
				if (!hit.collider.CompareTag("Card")) return;

				// 大将の場合はカードを選択できないようにする
				var soldier = hit.collider.gameObject.GetComponent<SetSoldier>();
				if (soldier != null && soldier.IsGeneral) return;

				GameObject obj = hit.collider.gameObject;

				if (DrawObject == obj)
				{
					// 選択したオブジェクトを保存
					_dragObj = obj;
					// Zの位置を保存
					_zDistance = _camera.WorldToScreenPoint(_dragObj.transform.position).z;
					_yPos = obj.transform.position.y;
				}
				else if (_area.AllSet)
				{
					// 全てセットされている状態で別のカードを選んだら、そのカードを削除
					_area.RemoveArea(obj);
					Destroy(obj);

					// 削除したのでUIを閉じる
					ReSetDestroySelectCardWindow();
				}

				Debug.Log("クリックされたオブジェクト: " + hit.collider.gameObject.name);
			}
		}

		if (_dragObj == null) return;

		// ドラッグ中
		if (Input.GetMouseButton(0))
		{
			// 平面を作成してマウス位置をワールド座標に変換
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
			// エリアにセットを試みる
			if (_area.SetAria(_dragObj))
			{
				TurnManager.instance.ChangeTurn();
				enabled = false;
			}

			_dragObj = null; // ドラッグ対象をリセット
		}
	}

	// カードを捨てる選択ウィンドウを表示する
	void SetDestroySelectCardWindow()
	{
		for (int i = 0; i < _area.CardObj.Length; i++)
		{
			if (_area.CardObj[i] == null) continue;

			// 大将かどうかを判定
			bool isGeneral = _area.CardObj[i].GetComponent<SetSoldier>().IsGeneral;

			// 大将の場合はアウトラインをつけない
			if (isGeneral) continue;

			// セットされているすべてのカードにアウトラインをつける
			var outline = _area.CardObj[i].GetComponent<SetOutLine>();
			if (outline != null) outline.SetOutline(0.03f);
		}

		_selectDestroyCardUI.enabled = true;
	}

	// 削除選択ウィンドウを閉じてリセットする
	void ReSetDestroySelectCardWindow()
	{
		for (int i = 0; i < _area.CardObj.Length; i++)
		{
			if (_area.CardObj[i] == null) continue;

			bool isGeneral = _area.CardObj[i].GetComponent<SetSoldier>().IsGeneral;

			if (isGeneral) continue;

			// アウトラインを消去
			var outline = _area.CardObj[i].GetComponent<SetOutLine>();
			if (outline != null) outline.ReSetOutline();
		}

		_selectDestroyCardUI.enabled = false;
	}
}