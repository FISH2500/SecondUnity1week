using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Deck _deck;
	[SerializeField] private TurnManager _turnManager;

	[SerializeField] private int _drawCardNum;

	[SerializeField] private Vector3 _drawCardPosition;

	private Vector3 _scale;

	private int _drawCountPlayer1 = 0;
	private int _drawCountPlayer2 = 0;

	private Image _image;

	private void Awake()
	{
		_scale = transform.localScale;
		_image = GetComponent<Image>();
	}

	private void Update()
	{
		bool isMyTurn = _turnManager.CurrentPlayer == 0;

		if (_image.enabled != isMyTurn)
		{
			_image.enabled = isMyTurn;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		if (_turnManager.CurrentPlayer == 0)
		{
			if (_drawCountPlayer1 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(_turnManager.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer1++;

				Debug.Log($"ドローしました");

				_turnManager.ChangeTurn();
			}
		}
		else
		{
			if (_drawCountPlayer2 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(_turnManager.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer2++;

				Debug.Log($"ドローしました");

				_turnManager.ChangeTurn();
			}
		}
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

	public void DrawCardCPU()
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		if (_turnManager.CurrentPlayer == 0)
		{
			if (_drawCountPlayer1 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(_turnManager.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer1++;

				Debug.Log($"ドローしました");

				_turnManager.ChangeTurn();
			}
		}
		else
		{
			if (_drawCountPlayer2 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(_turnManager.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer2++;

				Debug.Log($"ドローしました");

				_turnManager.ChangeTurn();
			}
		}
	}
}
