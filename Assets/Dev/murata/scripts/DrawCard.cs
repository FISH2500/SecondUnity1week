using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Deck _deck;

	[SerializeField] private int _drawCardNum;

	[SerializeField] private Vector3 _drawCardPosition;
	[SerializeField] private DrawMouse _drawMouse;

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
		bool isMyTurn = TurnManager.instance.CurrentPlayer == 0;

		if (_image.enabled != isMyTurn)
		{
			_image.enabled = isMyTurn;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log($"{gameObject.name} がクリックされました");

		if (TurnManager.instance.CurrentPlayer == 0)
		{
			if (_drawCountPlayer1 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer1++;

				Debug.Log($"ドローしました");

				TurnManager.instance.IsAction = true;
				_drawMouse.enabled = true;
				_drawMouse.DrawObject = obj;
				//TurnManager.instance.ChangeTurn();
			}
		}
		else
		{
			if (_drawCountPlayer2 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer2++;

				Debug.Log($"ドローしました");

				TurnManager.instance.IsAction = true;
				_drawMouse.enabled = true;
				_drawMouse.DrawObject = obj;
				//TurnManager.instance.ChangeTurn();
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

	public GameObject DrawCardCPU()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			if (_drawCountPlayer1 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer1++;

				Debug.Log($"ドローしました");

				TurnManager.instance.IsAction = true;

				return obj;
			}
		}
		else
		{
			if (_drawCountPlayer2 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);
				obj.transform.position = _drawCardPosition;
				_drawCountPlayer2++;

				Debug.Log($"ドローしました");

				TurnManager.instance.IsAction = true;

				return obj;
			}
		}

		return null;
	}

	public bool IsDraw()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			return _drawCountPlayer1 < _drawCardNum;
		}
		else
		{
			return _drawCountPlayer2 < _drawCardNum;
		}
	}
}
