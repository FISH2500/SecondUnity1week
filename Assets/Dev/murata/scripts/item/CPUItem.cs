using System.Collections.Generic;
using UnityEngine;

public class CPUItem : MonoBehaviour
{
	[SerializeField] private ItemDeck _itemDeck;

	[Header("確認用(設定不要)")]
	[SerializeField] private List<int> _myItems = new List<int>();

	private ItemBase _itemBase;

	public static CPUItem Instance;
	
	private void Start()
	{
		Instance = this;

		_itemBase = GetComponent<ItemBase>();

		for (int i = 0; i < 3; i++)
		{
			_myItems.Add(_itemDeck.CPUDrawItem());
		}
	}

	public void CPUUseItem()
	{
		int id = -1;
		int itemIndex = 0;
        for (int i = 0; i < _myItems.Count; i++)
		{
			itemIndex = i;
			if (CPUItemSelect.ItemSelect(_myItems[i]))
			{
				id = _myItems[i];
				break;
            }
		}
		if (id == -1) return;

        Debug.Log($"CPUがアイテム使用 : {id}");
        _myItems.RemoveAt(itemIndex);
		_itemBase.ItemID = id;
		_itemBase.Use();
	}

	public int GetFirstItemID()
	{
		return _myItems.Count > 0 ? _myItems[0] : -1;
	}

	public void ReplaceFirstItem(int newID)
	{
		if (_myItems.Count > 0) _myItems[0] = newID;
	}

	public int GetItemCount()
	{
		return _myItems.Count;
	}
}
