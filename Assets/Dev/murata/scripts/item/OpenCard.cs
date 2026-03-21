using System.Collections.Generic;
using UnityEngine;

public class OpenCard : MonoBehaviour
{
	private bool _isChoosingSelfCard = false;
	// 外部から「まだ選択中か？」を確認するためのプロパティ
	public bool IsProcessing { get; private set; }

	private void Awake()
	{
		IsProcessing = false;
	}

	public bool RunOpenCard()
	{
		Area playerArea = Area.Instance;
		CPUArea cpuArea = CPUArea.Instance;

		if (TurnManager.instance.CurrentPlayer == 0) // プレイヤーのターン
		{
			bool hasBackCard = false;
			foreach (var obj in cpuArea.CardObject)
			{
				if (obj != null)
				{
					if (obj.GetComponent<SetSoldier>().IsBack)
					{
						hasBackCard = true;
						break;
					}
				}
			}

			if (!hasBackCard)
			{
				Debug.Log("CPUのカードは全て表です。スキップします。");
				return false;
			}

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
				target.GetComponent<SetSoldier>().RotateSetFront();

				return true;
			}

			return false;
		}
		else // CPUのターン
		{
			// プレイヤー側に裏向きのカードがあるかチェック
			bool hasBackCard = false;
			foreach (var obj in playerArea.CardObj)
			{
				if (obj != null)
				{
					if (obj.GetComponent<SetSoldier>().IsBack)
					{
						hasBackCard = true;
						break;
					}
				}
			}

			if (!hasBackCard)
			{
				Debug.Log("プレイヤーのカードは全て表です。スキップします。");
				IsProcessing = false;
				_isChoosingSelfCard = false;
				return false;
			}

			IsProcessing = true; // 待機開始
			_isChoosingSelfCard = true;
			Debug.Log("表にする自分のカードを選んでください");
			TextManegar.instance.SetText("表にする自分のカードを選んでください");
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

				if (hitObj.CompareTag("Card") && s != null && s.IsBack)
				{
					s.RotateSetFront();
					_isChoosingSelfCard = false;
					IsProcessing = false; // 選択完了
					DispUI.instance.Disp(true);
				}
			}
		}
	}
}