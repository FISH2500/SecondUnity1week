using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
	[SerializeField] private Transform[] _Position;
	[SerializeField] private ItemDeck _itemDeck;
	[SerializeField] private Camera _camera;

	[SerializeField] private Canvas _itemCanvas;

	[Header("確認用(設定不要)")]
	[SerializeField] private List<GameObject> _myItems = new List<GameObject>();

	private bool _selectItem = false;

	private GameObject UsingItem = null;

	public static PlayerItem Instance;

	private void Start()
	{
		Instance = this;

		for (int i = 0; i < 3; i++)
		{
			GameObject obj = _itemDeck.DrawItem();

			obj.SetActive(false);
			obj.transform.position = _Position[i].position;
			obj.transform.rotation = _Position[i].rotation;

			AddItem(obj);
		}
	}


	void Update()
	{
		if (!_selectItem) return;

		// クリックされた瞬間
		if (Input.GetMouseButtonDown(0))
		{
			// マウスの画面上の位置から、カメラの奥に向かう光線を作成
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) // 光線を飛ばして当たって
			{
				if (!hit.collider.CompareTag("Item")) return; // Item以外ならこれ以上行わない

				if(UsingItem != hit.collider.gameObject)
				{
					// ここに強調表示を辞める処理を書く
					if (UsingItem != null) UsingItem.GetComponent<ItemBase>().Highlight(false);
				}

				UsingItem = hit.collider.gameObject;

				// ここに強調表示させる処理を書く
				UsingItem.GetComponent<ItemBase>().Highlight(true);

				// ログを流す
				Debug.Log("クリックしたオブジェクト: " + hit.collider.gameObject.name);
			}
		}
	}

	public void AddItem(GameObject item)
	{
		_myItems.Add(item);
	}

	public void UseItem()
	{
		GameObject item = UsingItem;

		_itemCanvas.enabled = false;

		item.GetComponent<ItemBase>().Use();

		_myItems.Remove(item);

		item.GetComponent<ItemBase>().Highlight(false);

		Destroy(item);

		UsingItem = null;

		_selectItem = false;

		TurnManager.instance.UseItemFlag();

		HideAllItems();
	}

	public void SelectItem(bool select)
	{
		_selectItem = select;
		_itemCanvas.enabled = select;
		DispUI.instance.Disp(!select);

		if (UsingItem != null) UsingItem.GetComponent<ItemBase>().Highlight(false);
		UsingItem = null;

		foreach (var item in _myItems)
		{
			item.SetActive(select);
		}
	}

	public void DispItem(bool disp)
	{
		foreach (var item in _myItems)
		{
			item.SetActive(disp);
		}
	}

	private void HideAllItems()
	{
		foreach (var item in _myItems)
		{
			if (item != null) item.SetActive(false);
		}
	}

	public List<GameObject> GetMyItems()
	{
		return _myItems;
	}
}