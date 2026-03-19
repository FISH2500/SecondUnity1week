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

	private void Update()
	{
		bool isMyTurn = TurnManager.instance.CurrentPlayer == 0;

		if (_image.enabled != isMyTurn)
		{
			_image.enabled = isMyTurn;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		TextManegar.instance.SetText("攻撃に使う札を選択してください");

        _cardSelect.enabled = true;//カードを選択するスクリプトを有効に
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
