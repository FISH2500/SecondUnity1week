using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;

public class AttackImage : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private CardSelect _cardSelect;

	private ActionButton _actionButton;

    private Vector3 _scale;


	private void Awake()
	{
		_scale = transform.localScale;
        _actionButton = GetComponent<ActionButton>();

    }

	public void OnPointerDown(PointerEventData eventData)
	{
		TextManegar.instance.SetText("攻撃に使う札を選択してください");

		_actionButton.ActionButtonDown();//アクションボタンの関数を呼び出す

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
