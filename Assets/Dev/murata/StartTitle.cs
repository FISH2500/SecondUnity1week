using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTitle : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[Header("シーン遷移の設定")]
	[SerializeField] private string _nextSceneName;   // 遷移先シーン名
	[SerializeField] private GameObject _loadCanvas;

	private Vector3 _scale;

	private void Awake()
	{
		_scale = transform.localScale;
		GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		Instantiate( _loadCanvas );

		SceneManager.LoadSceneAsync(_nextSceneName);
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
