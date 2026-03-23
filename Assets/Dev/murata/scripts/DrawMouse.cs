using System;
using UnityEngine;

public class DrawMouse : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private Area _area;
	[SerializeField] private Canvas _selectDestroyCardUI;

	private GameObject _dragObj = null;
	private float _zDistance = 0;
	[NonSerialized] public GameObject DrawObject;
	private float _yPos;

	private GameObject _lastHoveredCard = null;

	void Update()
	{
		if (_area.AllSet && _dragObj == null)
		{
			_selectDestroyCardUI.enabled = true;
			HandleHoverHighlight();
		}

		// クリックされた瞬間
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (!hit.collider.CompareTag("Card")) return;

				var soldier = hit.collider.gameObject.GetComponent<SetSoldier>();
				if (soldier != null && soldier.IsGeneral) return;

				GameObject obj = hit.collider.gameObject;

				if (DrawObject == obj)
				{
					_dragObj = obj;
					_zDistance = _camera.WorldToScreenPoint(_dragObj.transform.position).z;
					_yPos = obj.transform.position.y;
				}
				else if (_area.AllSet)
				{
					_area.RemoveArea(obj);
					Destroy(obj);

					// ハイライトもリセット
					ResetLastHover();
					_selectDestroyCardUI.enabled = false;
				}
			}
		}

		if (_dragObj != null)
		{
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

			if (Input.GetMouseButtonUp(0))
			{
				if (_area.SetAria(_dragObj))
				{
					TurnManager.instance.ChangeTurn();
					enabled = false;
				}
				_dragObj = null;
			}
		}
	}

	private void HandleHoverHighlight()
	{
		Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			GameObject hitObj = hit.collider.gameObject;

			// 自分のエリアに配置済みのカードか、タグで判定（適宜調整してください）
			if (hitObj.CompareTag("Card") && hitObj != DrawObject)
			{
				if (_lastHoveredCard != hitObj)
				{
					ResetLastHover();

					SetSoldier sol = hitObj.GetComponent<SetSoldier>();
					SetOutLine outline = hitObj.GetComponent<SetOutLine>();

					// 大将以外ならハイライト
					if (sol != null && !sol.IsGeneral && outline != null)
					{
						outline.SetOutline(0.05f); // 少し太めに強調
						_lastHoveredCard = hitObj;
					}
				}
			}
			else
			{
				ResetLastHover();
			}
		}
		else
		{
			ResetLastHover();
		}
	}

	// ハイライト解除
	private void ResetLastHover()
	{
		if (_lastHoveredCard != null)
		{
			SetOutLine outline = _lastHoveredCard.GetComponent<SetOutLine>();
			if (outline != null) outline.ReSetOutline();
			_lastHoveredCard = null;
		}
	}
}