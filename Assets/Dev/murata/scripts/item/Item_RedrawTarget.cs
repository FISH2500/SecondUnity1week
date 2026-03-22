using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_RedrawTarget : MonoBehaviour
{
	private Camera _camera;
	private const float _time = 0.5f; // 片道の移動時間
	private bool _isChoosingEnemyCard = false;
	private bool _isAnimating = false;

	private void Awake() => _camera = Camera.main;

	public void StartRedraw()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			_isChoosingEnemyCard = true;
			DispUI.instance.Disp(false);
			Debug.Log("引き直させる相手のカードを選択してください");
		}
		else
		{
			ExecuteCPURedraw();
		}
	}

	void Update()
	{
		if (!_isChoosingEnemyCard || _isAnimating) return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				GameObject hitObj = hit.collider.gameObject;
				if (hitObj.CompareTag("Player2Card"))
				{
					SetSoldier s = hitObj.GetComponent<SetSoldier>();
					if (s != null && !s.IsGeneral)
					{
						StartCoroutine(AnimateRedraw(hitObj));
						_isChoosingEnemyCard = false;
					}
				}
			}
		}
	}

	private void ExecuteCPURedraw()
	{
		GameObject target = null;
		GameObject strongestOpen = null;
		List<GameObject> backCards = new List<GameObject>();
		int maxAtk = -1;

		foreach (GameObject obj in Area.Instance.CardObj)
		{
			if (obj == null) continue;
			SetSoldier s = obj.GetComponent<SetSoldier>();
			if (s.IsGeneral) continue;

			if (!s.IsBack)
			{
				if (s.SoldierAtk > maxAtk)
				{
					maxAtk = s.SoldierAtk;
					strongestOpen = obj;
				}
			}
			else
			{
				backCards.Add(obj);
			}
		}

		if (strongestOpen != null && maxAtk > 6) target = strongestOpen;
		else if (backCards.Count > 0) target = backCards[Random.Range(0, backCards.Count)];
		else if (strongestOpen != null) target = strongestOpen;

		if (target != null) StartCoroutine(AnimateRedraw(target));
	}

	private IEnumerator AnimateRedraw(GameObject oldCard)
	{
		_isAnimating = true;
		Vector3 originPos = oldCard.transform.position;
		Vector3 deckPos = Deck.Instance.transform.position;
		deckPos.y = originPos.y; // 高さを合わせる

		// 1. デッキへ移動
		float elapsed = 0;
		while (elapsed < _time)
		{
			elapsed += Time.deltaTime;
			oldCard.transform.position = Vector3.Lerp(originPos, deckPos, elapsed / _time);
			yield return null;
		}
		oldCard.transform.position = deckPos;

		// 2. 中身の書き換え (Deckからカードを引く)
		GameObject newCardTemp = Deck.Instance.DrawCard(TurnManager.instance.CurrentPlayer);
		if (newCardTemp != null)
		{
			SetSoldier oldS = oldCard.GetComponent<SetSoldier>();
			SetSoldier newS = newCardTemp.GetComponent<SetSoldier>();

			oldS.CardIndex = newS.CardIndex;
			oldS.SoldierAtk = newS.SoldierAtk;

			// 相手のカードとして裏向きにする
			oldS.SetBack(oldS.OwnerPlayer);

			oldS.SetSprite();

			Destroy(newCardTemp);
			Debug.Log($"{oldCard.name} の中身を書き換えました");
		}

		// ちょっと待機（演出用）
		yield return new WaitForSeconds(0.2f);

		// 3. 元の場所へ戻る
		elapsed = 0;
		while (elapsed < _time)
		{
			elapsed += Time.deltaTime;
			oldCard.transform.position = Vector3.Lerp(deckPos, originPos, elapsed / _time);
			yield return null;
		}
		oldCard.transform.position = originPos;

		_isAnimating = false;
		DispUI.instance.Disp(true);
	}
}