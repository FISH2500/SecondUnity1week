using UnityEngine;
using System.Collections.Generic;

public class OpenCard : MonoBehaviour
{
	private bool _isChoosingSelfCard = false;

	public bool RunOpenCard()
	{
		Area playerArea = Area.Instance;
		CPUArea cpuArea = CPUArea.Instance;

		if (TurnManager.instance.CurrentPlayer == 0) // プレイヤーのターン
		{
			// CPUに自分のカード（相手から見て）を選ばせる
			GameObject target = null;
			int pattern = Random.Range(0, 2);

			if (pattern == 0) // ランダム
			{
				List<GameObject> backs = new List<GameObject>();
				foreach (var obj in cpuArea.CardObject)
				{
					if (obj != null && obj.GetComponent<SetSoldier>().IsBack) backs.Add(obj);
				}
				if (backs.Count > 0) target = backs[Random.Range(0, backs.Count)];
			}
			else // 最弱
			{
				int minAtk = 9999;
				foreach (var obj in cpuArea.CardObject)
				{
					if (obj == null) continue;
					SetSoldier s = obj.GetComponent<SetSoldier>();
					if (s.IsBack && s.SoldierAtk < minAtk)
					{
						minAtk = s.SoldierAtk;
						target = obj;
					}
				}
			}

			if (target != null)
			{
				target.GetComponent<SetSoldier>().SetFront();
				return true;
			}
			return false;
		}
		else // CPUのターン
		{
			// プレイヤーに「自分の裏カード」を選ばせるモードへ
			_isChoosingSelfCard = true;
			Debug.Log("表にする自分のカードを選んでください");
			return true;
		}
	}

	private void Update()
	{
		if (!_isChoosingSelfCard) return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				GameObject hitObj = hit.collider.gameObject;
				SetSoldier s = hitObj.GetComponent<SetSoldier>();

				// 自分のカードかつ裏向きならOK
				if (hitObj.CompareTag("Card") && s != null && s.IsBack)
				{
					s.SetFront();
					_isChoosingSelfCard = false;
					DispUI.instance.Disp(true);
					// アイテム処理終了
				}
			}
		}
	}
}