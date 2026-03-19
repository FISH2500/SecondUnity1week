using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/ItemData")]
public class ItemDataBase : ScriptableObject
{
    public Sprite Back;//アイテムの裏の画像

    public List<ItemData> itemDatas;//アイテムのデータを格納するリスト
}
