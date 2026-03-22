using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Deck _deck;

	[SerializeField] public int _drawCardNum;

	[SerializeField] private Transform _drawCardPosition;
	[SerializeField] private DrawMouse _drawMouse;

	[SerializeField] private float _moveTime = 0.5f; // 移動にかかる時間

	private Vector3 _scale;

	public int _drawCountPlayer1 = 0;
	private int _drawCountPlayer2 = 0;

	public static DrawCard instance;

	private void Awake()
	{
		_scale = transform.localScale;
		instance = this;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (TurnManager.instance.IsDraw) return;

		// ドロー可能かチェック
		bool canDraw = (TurnManager.instance.CurrentPlayer == 0 && _drawCountPlayer1 < _drawCardNum) ||
					   (TurnManager.instance.CurrentPlayer == 1 && _drawCountPlayer2 < _drawCardNum);

		if (canDraw)
		{
			TurnManager.instance.IsDraw = true;

			// カードを生成（Deckの位置で生成される想定）
			GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);

			// 音
			SoundManager.Instance.PlaySE("DrawCard");

			// 移動演出を開始
			StartCoroutine(MoveToDrawPosition(obj));

			// カウント等の更新
			if (TurnManager.instance.CurrentPlayer == 0)
			{
				_drawCountPlayer1++;
				TextManegar.instance.SetText("札を引きました");
			}
			else
			{
				_drawCountPlayer2++;
			}
		}
	}

	// カードをスライド移動させるコルーチン
	private IEnumerator MoveToDrawPosition(GameObject card)
	{
		Vector3 startPos = _deck.transform.position; // 山札の位置
		Vector3 endPos = _drawCardPosition.position; // 目的地

		float elapsed = 0;

		TurnManager.instance.IsAction = true;

		// 最初は裏側に
		SetSoldier sol = card.GetComponent<SetSoldier>();
		sol.SetBack(TurnManager.instance.CurrentPlayer);
		sol.OwnerPlayer = TurnManager.instance.CurrentPlayer;

		while (elapsed < _moveTime)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / _moveTime;

			t = Mathf.SmoothStep(0, 1, t);

			card.transform.position = Vector3.Lerp(startPos, endPos, t);
			yield return null;
		}

		// 最後に位置を完全に固定
		card.transform.position = endPos;

		// 移動が終わってから、マウス追従などの処理を有効にする
		_drawMouse.enabled = true;
		_drawMouse.DrawObject = card;

		Debug.Log("ドロー移動完了");
	}

	// マウスが乗った時
	public void OnPointerEnter(PointerEventData eventData)
	{
		bool canDraw = (TurnManager.instance.CurrentPlayer == 0 && _drawCountPlayer1 < _drawCardNum) ||
					   (TurnManager.instance.CurrentPlayer == 1 && _drawCountPlayer2 < _drawCardNum);
		if (canDraw) SoundManager.Instance.PlaySE("Shot");
		transform.localScale = _scale * 1.1f;
	}

	// マウスが離れた時
	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = _scale;
	}

	public GameObject DrawCardCPU()
	{
		TurnManager.instance.IsDraw = true;

		if (TurnManager.instance.CurrentPlayer == 0)
		{
			if (_drawCountPlayer1 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);
				obj.transform.position = _drawCardPosition.position;
				_drawCountPlayer1++;

				Debug.Log($"ドローしました");

				TurnManager.instance.IsAction = true;

				obj.tag = "Player1Card";

				SetSoldier sol = obj.GetComponent<SetSoldier>();

				sol.SetBack(0);
				sol.OwnerPlayer = 0;

				return obj;
			}
		}
		else
		{
			if (_drawCountPlayer2 < _drawCardNum)
			{
				GameObject obj = _deck.DrawCard(TurnManager.instance.CurrentPlayer);
				obj.transform.position = _drawCardPosition.position;
				_drawCountPlayer2++;

				Debug.Log($"ドローしました");

				TurnManager.instance.IsAction = true;

				obj.tag = "Player2Card";

				SetSoldier sol = obj.GetComponent<SetSoldier>();

				sol.SetBack(1);
				sol.OwnerPlayer = 1;

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

	public void AddDrawNum(int num)
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			_drawCountPlayer1 -= num;
		}
		else
		{
			_drawCountPlayer2 -= num;
		}
	}
}
