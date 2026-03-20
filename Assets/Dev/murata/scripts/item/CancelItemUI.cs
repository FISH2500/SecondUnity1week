using UnityEngine;
using UnityEngine.EventSystems;

public class CancelItemUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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
		GetComponent<ActionButton>().ActionButtonDown();
        _playerItem.SelectItem(false);
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
