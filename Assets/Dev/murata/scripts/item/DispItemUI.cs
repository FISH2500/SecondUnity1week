using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DispItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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

		if (!TurnManager.instance.UseItem) _playerItem.SelectItem(true);
<<<<<<< Updated upstream
=======

		//gameObject.GetComponent<ActionButton>().ActionButtonDown();
>>>>>>> Stashed changes
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
