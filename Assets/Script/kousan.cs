using UnityEngine;
using UnityEngine.EventSystems;

public class kousan : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] BattleManegar _bm;
	private Vector3 _scale;

	private void Awake()
	{
		_scale = transform.localScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		_bm.kousan();
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
