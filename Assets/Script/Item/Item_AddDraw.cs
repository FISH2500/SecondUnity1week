using UnityEngine;

public class Item_AddDraw : ItemBase
{
	public override void Use()
	{
		DrawCard.instance.AddDrawNum(1);
	}
}
