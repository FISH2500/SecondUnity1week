using UnityEngine;
using UnityEngine.EventSystems;

public class UseItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	PlayerItem _playerItem;

	private Vector3 _scale;

	private void Awake()
	{
		_scale = transform.localScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");
<<<<<<< Updated upstream:Assets/Dev/murata/scripts/item/UseIteuUI.cs

=======
        //GetComponent<ActionButton>().ActionButtonDown();
		//DispUI.instance.Disp(false);
>>>>>>> Stashed changes:Assets/Dev/murata/scripts/item/UseItemUI.cs
		_playerItem.UseItem();
	}

	// マウスが乗った時
	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.localScale = _scale * 1.1f;
	}

	// マウスが離れた時
	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = _scale;
	}
}
