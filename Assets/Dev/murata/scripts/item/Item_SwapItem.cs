using UnityEngine;
using System.Collections.Generic;

public class Item_SwapItem : MonoBehaviour
{
	private Camera _camera;

	private bool _isSwapping = false;
	private GameObject _selectedPlayerItem = null;

	private void Start()
	{
		_camera = Camera.main;
	}

	public void StartItemSwap()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			_isSwapping = true;
			_selectedPlayerItem = null;
			PlayerItem.Instance.DispItem(true);
			DispUI.instance.Disp(false);
			Debug.Log("交換する自分のアイテムを選択してください");
		}
		else
		{
			ExecuteCPUItemSwap();
		}
	}

	void Update()
	{
		if (!_isSwapping) return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				if (hit.collider.CompareTag("Item"))
				{
					_selectedPlayerItem = hit.collider.gameObject;
					ExecutePlayerItemSwap();
				}
			}
		}
	}

	// プレイヤーが使用した場合：自分の選んだアイテムとCPUの所持リストの先頭を交換
	private void ExecutePlayerItemSwap()
	{
		ItemBase pItemBase = _selectedPlayerItem.GetComponent<ItemBase>();

		// CPU側のリスト(_myItems)にアクセスするためにCPUItemを一部修正(後述)
		int cpuItemID = CPUItem.Instance.GetFirstItemID();
		int playerItemID = pItemBase.ItemID;

		// IDの入れ替え
		pItemBase.ItemID = cpuItemID;
		CPUItem.Instance.ReplaceFirstItem(playerItemID);

		pItemBase.SetSprite();
		pItemBase.Highlight(false);

		Debug.Log($"アイテムを交換しました。新アイテムID: {cpuItemID}");

		_isSwapping = false;
		_selectedPlayerItem = null;

		// 交換後はアイテム選択画面を閉じる
		PlayerItem.Instance.DispItem(false);
		DispUI.instance.Disp(true);
	}

	// CPUが使用した場合：CPUのリストの次にあるものと、プレイヤーのランダムな所持品を交換
	private void ExecuteCPUItemSwap()
	{
		List<GameObject> pItems = PlayerItem.Instance.GetMyItems();
		if (pItems.Count == 0) return;

		int randomIndex = Random.Range(0, pItems.Count);
		ItemBase targetPItem = pItems[randomIndex].GetComponent<ItemBase>();

		int cpuItemID = CPUItem.Instance.GetFirstItemID();
		int playerItemID = targetPItem.ItemID;

		// IDの入れ替え
		targetPItem.ItemID = cpuItemID;
		CPUItem.Instance.ReplaceFirstItem(playerItemID);

		targetPItem.SetSprite();

		Debug.Log("CPUがアイテムを交換しました");
	}
}