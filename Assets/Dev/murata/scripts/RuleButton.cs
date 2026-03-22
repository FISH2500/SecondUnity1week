using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuleBUtton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private bool _hidari;

	private Vector3 _scale;

	private void Awake()
	{
		_scale = transform.localScale;
		GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		if (_hidari) Rule.Instance.PrevSprite();
		else Rule.Instance.NextSprite();
	}

	// マウスが乗った時
	public void OnPointerEnter(PointerEventData eventData)
	{
		SoundManager.Instance.PlaySE("Shot");
		transform.localScale = _scale * 1.1f;
	}

	// マウスが離れた時
	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = _scale;
	}
}
