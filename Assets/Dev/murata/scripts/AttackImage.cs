using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
//using UnityEngine.UIElements;

public class AttackImage : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private CardSelect _cardSelect;

<<<<<<< Updated upstream
=======
	[SerializeField] private Canvas _attackCanvas;
>>>>>>> Stashed changes

	private Vector3 _scale;


	private void Awake()
	{
		_scale = transform.localScale;
<<<<<<< Updated upstream
	}
=======
    }
>>>>>>> Stashed changes

	public void OnPointerDown(PointerEventData eventData)
	{
		TextManegar.instance.SetText("攻撃に使う札を選択してください");

<<<<<<< Updated upstream
        _cardSelect.enabled = true;//カードを選択するスクリプトを有効に
=======
		//_actionButton.ActionButtonDown();//アクションボタンの関数を呼び出す

		DispUI.instance.Disp(false);

		_attackCanvas.enabled = true;

		_cardSelect.enabled = true;//カードを選択するスクリプトを有効に
>>>>>>> Stashed changes
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
