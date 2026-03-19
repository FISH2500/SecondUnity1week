using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;

public class AttackImage : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private CardSelect _cardSelect;

	private Vector3 _scale;
	private Image _image;

	private void Awake()
	{
		_scale = transform.localScale;
		_image = GetComponent<Image>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_cardSelect.enabled = true;
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
