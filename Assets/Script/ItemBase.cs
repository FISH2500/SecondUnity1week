using UnityEngine;

// abstractを付けることで「これ単体では実体化できない」親クラスになります
public abstract class ItemBase : MonoBehaviour
{
	[Header("アイテム設定")]
	public string ItemName;    // アイテム名
	public int ItemID;        // 識別ID
	[TextArea] public string Description; // 説明文

	public abstract void Use();

	// 共通の処理はここに書く
	public virtual void Discard()
	{
		
	}
}
