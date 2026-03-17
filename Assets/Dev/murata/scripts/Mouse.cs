using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
	[SerializeField] private Camera _camera; // rayを飛ばすためのカメラ

	private GameObject _dragObj = null; // 今ドラッグしているオブジェクト
	private float _zDistance = 0; // 選択した時のZ軸の位置

	void Update()
	{
		// クリックされた瞬間
		if (Input.GetMouseButtonDown(0))
		{
			// マウスの画面上の位置から、カメラの奥に向かう光線を作成
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) // 光線を飛ばして当たったならば
			{
				// 選択したオブジェクトを保存
				_dragObj = hit.collider.gameObject;

				// Zの位置を保存
				_zDistance = _camera.WorldToScreenPoint(_dragObj.transform.position).z;

				// ログを流す
				Debug.Log("クリックしたオブジェクト: " + hit.collider.gameObject.name);
			}
		}

		// ドラッグ中
		if (Input.GetMouseButton(0) && _dragObj != null)
		{
			// マウスの座標を取得
			Vector3 mousePos = Input.mousePosition;

			// オブジェクトのZの位置と合わせる
			mousePos.z = _zDistance;

			// ワールド座標に変換
			Vector3 objPos = _camera.ScreenToWorldPoint(mousePos);

			// 位置を移動させる
			_dragObj.transform.position = objPos;
		}

		// 指を離した時
		if (Input.GetMouseButtonUp(0))
		{
			_dragObj = null; // ドラッグしているオブジェクトをNULLに
		}
	}
}
