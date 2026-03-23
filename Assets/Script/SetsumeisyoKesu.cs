using UnityEngine;
using UnityEngine.EventSystems;

public class SetsumeisyoKesu : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	Canvas _canvas;

	private Vector3 _scale;

	private void Awake()
	{
		_scale = transform.localScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		DispUI.instance.Disp(true);
		_canvas.enabled = false;
		SoundManager.Instance.PlaySE("Command");
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