using UnityEngine;

public class Mouse : MonoBehaviour
{
	[SerializeField] private Camera _camera; // rayを飛ばすためのカメラ
	[SerializeField] private Area _area; // セットするためのエリア

	private GameObject _dragObj = null; // 今ドラッグしているオブジェクト
	private float _zDistance = 0; // 選択した時のZ軸の位置

	private float _yPos;//カードのYの固定位置


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

				// 選択したオブジェクトを保存
				_dragObj = hit.collider.gameObject;

				_area.RemoveArea(_dragObj);

				// Zの位置を保存
				_zDistance = _camera.WorldToScreenPoint(_dragObj.transform.position).z;

				_yPos = _dragObj.transform.position.y;

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
			_area.SetAria(_dragObj);
			_dragObj = null; // ドラッグしているオブジェクトをNULLに
		}
    }
}