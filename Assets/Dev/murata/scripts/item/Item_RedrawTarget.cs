using UnityEngine;
using System.Collections.Generic;

public class Item_RedrawTarget : MonoBehaviour
{
	private Camera _camera;

	private bool _isChoosingEnemyCard = false;

	private void Awake()
	{
		_camera = Camera.main;
	}

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
		if (!_isChoosingEnemyCard) return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				GameObject hitObj = hit.collider.gameObject;
				// 相手(CPU)のカードであることを確認
				if (hitObj.CompareTag("Player2Card"))
				{
					SetSoldier s = hitObj.GetComponent<SetSoldier>();
					if (s != null && !s.IsGeneral)
					{
						ReplaceCardData(hitObj);
						_isChoosingEnemyCard = false;
						DispUI.instance.Disp(true);
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

		// プレイヤーの場を解析
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

		// ロジック判定
		if (strongestOpen != null && maxAtk > 6)
		{
			target = strongestOpen;
		}
		else if (backCards.Count > 0)
		{
			target = backCards[Random.Range(0, backCards.Count)];
		}
		else if (strongestOpen != null)
		{
			target = strongestOpen;
		}

		if (target != null)
		{
			ReplaceCardData(target);
		}
	}

	private void ReplaceCardData(GameObject oldCard)
	{
		// 山札から新しいカードの「データ」だけをもらうために1枚生成
		GameObject newCardTemp = Deck.Instance.DrawCard(TurnManager.instance.CurrentPlayer);
		if (newCardTemp == null) return;

		SetSoldier oldS = oldCard.GetComponent<SetSoldier>();
		SetSoldier newS = newCardTemp.GetComponent<SetSoldier>();

		// データの書き換え
		oldS.CardIndex = newS.CardIndex;
		oldS.SoldierAtk = newS.SoldierAtk;

		oldS.SetBack(TurnManager.instance.CurrentPlayer ^ 1);

		// 一時生成したオブジェクトを破棄
		Destroy(newCardTemp);

		Debug.Log($"{oldCard.name} を引き直させました");
	}
}