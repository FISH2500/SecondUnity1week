using UnityEngine;

public class SwapCard : MonoBehaviour
{
	private Camera _camera;
	private Area _playerArea;
	private CPUArea _cpuArea;

	private GameObject _firstSelected = null;
	private bool _isSwapping = false;

	private void Awake()
	{
		_camera = Camera.main;
		_playerArea = Area.Instance;
		_cpuArea = CPUArea.Instance;
	}

	public void StartSwapMode()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			_isSwapping = true;
			_firstSelected = null;
			Debug.Log("交換モード開始");
		}
		else
		{
			ExecuteCPUSwap();
		}
	}

	void Update()
	{
		if (!_isSwapping) return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				GameObject hitObj = hit.collider.gameObject;

				// 大将は交換不可
				SetSoldier s = hitObj.GetComponent<SetSoldier>();
				if (s == null || s.IsGeneral) return;

				// 1枚目の選択
				if (_firstSelected == null)
				{
					_firstSelected = hitObj;
					// 選択中エフェクト（あれば）
					_firstSelected.transform.localScale *= 1.1f;
				}
				// 2枚目の選択（1枚目と違うタグ＝自分と相手のペアであること）
				else if (hitObj != _firstSelected && hitObj.tag != _firstSelected.tag)
				{
					ExecuteSwap(_firstSelected, hitObj);
				}
				else
				{
					// 同じ陣営を選んだらリセット
					_firstSelected.transform.localScale /= 1.1f;
					_firstSelected = null;
				}
			}
		}
	}

	private void ExecuteSwap(GameObject a, GameObject b)
	{
		SetSoldier sA = a.GetComponent<SetSoldier>();
		SetSoldier sB = b.GetComponent<SetSoldier>();

		// データ(IndexとAtk)の入れ替え
		int tmpIdx = sA.CardIndex;
		int tmpAtk = sA.SoldierAtk;

		sA.CardIndex = sB.CardIndex;
		sA.SoldierAtk = sB.SoldierAtk;

		sB.CardIndex = tmpIdx;
		sB.SoldierAtk = tmpAtk;

		// 見た目のリセットと終了処理
		_firstSelected.transform.localScale /= 1.1f;
		_isSwapping = false;
		_firstSelected = null;

		Debug.Log("交換完了");

		// アイテム使用後の後片付け（UI再表示など）
		DispUI.instance.Disp(true);
	}

	private void ExecuteCPUSwap()
	{
		GameObject myWeakest = null;
		GameObject targetStrongest = null;

		int minAtk = 9999;
		int maxAtk = -1;

		// 1. CPU(自分)の最も弱いカードを探す（大将以外）
		foreach (GameObject obj in _cpuArea.CardObject)
		{
			if (obj == null) continue;
			SetSoldier s = obj.GetComponent<SetSoldier>();
			if (s.IsGeneral) continue;

			if (s.SoldierAtk < minAtk)
			{
				minAtk = s.SoldierAtk;
				myWeakest = obj;
			}
		}

		// 2. プレイヤー(相手)の最も強いカードを探す（大将以外、かつ表向きのもの優先）
		foreach (GameObject obj in _playerArea.CardObj)
		{
			if (obj == null) continue;
			SetSoldier s = obj.GetComponent<SetSoldier>();
			if (s.IsGeneral) continue;

			// 表向きのカードから最強を探す
			if (!s.IsBack && s.SoldierAtk > maxAtk)
			{
				maxAtk = s.SoldierAtk;
				targetStrongest = obj;
			}
		}

		// 3. もしプレイヤーの表向きカードがなければ、裏向きからランダムに選ぶ
		if (targetStrongest == null)
		{
			System.Collections.Generic.List<GameObject> backCards = new System.Collections.Generic.List<GameObject>();
			foreach (GameObject obj in _playerArea.CardObj)
			{
				if (obj != null && !obj.GetComponent<SetSoldier>().IsGeneral) backCards.Add(obj);
			}
			if (backCards.Count > 0) targetStrongest = backCards[Random.Range(0, backCards.Count)];
		}

		// 4. 交換実行
		if (myWeakest != null && targetStrongest != null)
		{
			Debug.Log($"CPUが交換を実行: {myWeakest.name} <-> {targetStrongest.name}");
			ExecuteSwap(myWeakest, targetStrongest);
		}
	}
}