using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementDecision : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	// インスペクターから実行したいメソッドを指定できるようにする
	//[SerializeField] private UnityEvent _onClicked;

	private Vector3 _scale;

	void Awake()
	{
		_scale = transform.localScale;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		// インスペクターで設定した処理を実行
		//_onClicked?.Invoke();
		GetComponent<Image>().color = new Color(Random.value, Random.value, Random.value);
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
