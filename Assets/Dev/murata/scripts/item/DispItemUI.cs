using UnityEngine;
using UnityEngine.EventSystems;

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

		if (!TurnManager.instance.UseItem && PlayerItem.Instance.GetMyItems().Count > 0)
		{
			SoundManager.Instance.PlaySE("Command");
			_playerItem.SelectItem(true);
		}
	}

	// マウスが乗った時
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!TurnManager.instance.UseItem && PlayerItem.Instance.GetMyItems().Count > 0)
		{
			SoundManager.Instance.PlaySE("Shot");
		}
		transform.localScale = _scale * 1.1f;
	}

	// マウスが離れた時
	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = _scale;
	}
}
