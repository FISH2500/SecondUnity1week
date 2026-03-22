using UnityEngine;
using UnityEngine.EventSystems;

public class CancelImages : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CardSelect _cardSelect;
    [SerializeField] private GameObject _drawButton;
    [SerializeField] private GameObject _attackButton;

    private ActionButton _actionButton;

    private Vector3 _scale;


    private void Awake()
    {
        _scale = transform.localScale;
        _actionButton = GetComponent<ActionButton>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TextManegar.instance.SetText("攻撃をキャンセルした");

        _actionButton.ActionButtonDown();//アクションボタンのキャンセル関数を呼び出す

        _cardSelect.enabled = false;//カードを選択するスクリプトを無効に
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
