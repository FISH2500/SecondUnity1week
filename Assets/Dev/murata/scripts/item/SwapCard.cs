using UnityEngine;

public class SwapCard : MonoBehaviour
{
	private Camera _camera;

	[Header("確認用")]
	[SerializeField]
	private GameObject _firstSelected = null;
	private bool _isSwapping = false;

	private void Awake()
	{
		_camera = Camera.main;
	}

	public void StartSwapMode()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			if (Area.Instance.CardNum == 1 || CPUArea.Instance.CardNum == 1)
			{
				DispUI.instance.Disp(true);
				Debug.Log("交換するものがありません");
				return;
			}
			_isSwapping = true;
			_firstSelected = null;
			DispUI.instance.Disp(false);
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
		// 1. 座標を物理的に入れ替える
		Vector3 posA = a.transform.position;
		Vector3 posB = b.transform.position;

		a.transform.position = posB;
		b.transform.position = posA;

		(b.tag, a.tag) = (a.tag, b.tag);

		// 2. 各エリアの配列(CardObjなど)の中身を書き換える
		UpdateAreaArrays(a, b);

		SetSoldier sA = a.GetComponent<SetSoldier>();
		SetSoldier sB = b.GetComponent<SetSoldier>();

		(sA.OwnerPlayer, sB.OwnerPlayer) = (sB.OwnerPlayer, sA.OwnerPlayer);

		if (sA.IsBack) sA.SetBack(sA.OwnerPlayer);
		else sA.SetFront();

		if (sB.IsBack) sB.SetBack(sB.OwnerPlayer);
		else sB.SetFront();

		// 4. 見た目のリセットと終了処理
		if (TurnManager.instance.CurrentPlayer == 0 && _firstSelected != null)
			_firstSelected.transform.localScale /= 1.1f;

		_isSwapping = false;
		_firstSelected = null;

		Debug.Log($"{a.name} と {b.name} を物理的に入れ替えました");
		DispUI.instance.Disp(true);
	}

	private void UpdateAreaArrays(GameObject a, GameObject b)
	{
		// プレイヤー側の配列からaまたはbを探して入れ替える
		GameObject[] pArray = Area.Instance.CardObj;
		GameObject[] cArray = CPUArea.Instance.CardObject;

		int idxA_inP = -1, idxB_inP = -1;
		int idxA_inC = -1, idxB_inC = -1;

		// どこに誰がいるかインデックスを特定
		for (int i = 0; i < 6; i++)
		{
			if (pArray[i] == a) idxA_inP = i;
			if (pArray[i] == b) idxB_inP = i;
			if (cArray[i] == a) idxA_inC = i;
			if (cArray[i] == b) idxB_inC = i;
		}

		// 配列の中身を入れ替え (aがいた場所にbを、bがいた場所にaを入れる)
		if (idxA_inP != -1) pArray[idxA_inP] = b;
		if (idxB_inP != -1) pArray[idxB_inP] = a;
		if (idxA_inC != -1) cArray[idxA_inC] = b;
		if (idxB_inC != -1) cArray[idxB_inC] = a;
	}

	private void ExecuteCPUSwap()
	{
		if(CPUArea.Instance.CardNum == 1 || Area.Instance.CardNum == 1)
		{
			return;
		}
		GameObject myWeakest = null;
		GameObject targetStrongest = null;

		int minAtk = 9999;
		int maxAtk = -1;

		// 1. CPU(自分)の最も弱いカードを探す（大将以外）
		foreach (GameObject obj in CPUArea.Instance.CardObject)
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
		foreach (GameObject obj in Area.Instance.CardObj)
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
			foreach (GameObject obj in Area.Instance.CardObj)
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