using UnityEngine;
using System.Collections;

public class SwapCard : MonoBehaviour
{
	private Camera _camera;
	private const float _time = 0.8f; // 演出時間

	[Header("確認用")]
	[SerializeField] private GameObject _firstSelected = null;
	private bool _isSwapping = false;
	private bool _isAnimating = false; // アニメーション中フラグ

	private void Awake() => _camera = Camera.main;

	public void StartSwapMode()
	{
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			if (Area.Instance.CardNum <= 1 || CPUArea.Instance.CardNum <= 1)
			{
				DispUI.instance.Disp(true);
				return;
			}
			_isSwapping = true;
			_firstSelected = null;
			DispUI.instance.Disp(false);
		}
		else
		{
			ExecuteCPUSwap();
		}
	}

	void Update()
	{
		if (!_isSwapping || _isAnimating) return;

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				GameObject hitObj = hit.collider.gameObject;
				SetSoldier s = hitObj.GetComponent<SetSoldier>();
				if (s == null || s.IsGeneral) return;

				if (_firstSelected == null)
				{
					_firstSelected = hitObj;
					_firstSelected.transform.localScale *= 1.1f;
				}
				else if (hitObj != _firstSelected && hitObj.tag != _firstSelected.tag)
				{
					StartCoroutine(AnimateSwap(_firstSelected, hitObj));
				}
				else
				{
					_firstSelected.transform.localScale /= 1.1f;
					_firstSelected = null;
				}
			}
		}
	}

	private IEnumerator AnimateSwap(GameObject a, GameObject b)
	{
		_isAnimating = true;

		// 1. スケールを戻す
		if (_firstSelected != null) _firstSelected.transform.localScale /= 1.1f;

		// 2. 移動開始座標と終了座標
		Vector3 startPosA = a.transform.position;
		Vector3 startPosB = b.transform.position;

		// カードが重ならないように少し高さを出す演出を入れるなら
		Vector3 peakA = (startPosA + startPosB) / 2 + Vector3.up * 1.0f;

		float elapsed = 0;
		while (elapsed < _time)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / _time;

			// 滑らかな移動 (Lerp)
			a.transform.position = Vector3.Lerp(startPosA, startPosB, t);
			b.transform.position = Vector3.Lerp(startPosB, startPosA, t);

			yield return null;
		}

		// 3. 座標を完全に固定
		a.transform.position = startPosB;
		b.transform.position = startPosA;

		// 4. 内部データの入れ替え
		FinalizeSwap(a, b);

		_isAnimating = false;
		_isSwapping = false;
		_firstSelected = null;
		DispUI.instance.Disp(true);
	}

	private void FinalizeSwap(GameObject a, GameObject b)
	{
		// タグの入れ替え
		(b.tag, a.tag) = (a.tag, b.tag);

		// 配列の入れ替え
		UpdateAreaArrays(a, b);

		SetSoldier sA = a.GetComponent<SetSoldier>();
		SetSoldier sB = b.GetComponent<SetSoldier>();

		// 所有権の入れ替え
		(sA.OwnerPlayer, sB.OwnerPlayer) = (sB.OwnerPlayer, sA.OwnerPlayer);

		// 表裏の再設定（新しい所有者の色にする）
		if (sA.IsBack) sA.SetBack(sA.OwnerPlayer); else sA.SetFront();
		if (sB.IsBack) sB.SetBack(sB.OwnerPlayer); else sB.SetFront();

		Debug.Log("物理交換・演出完了");
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
			StartCoroutine(AnimateSwap(myWeakest, targetStrongest));
		}
	}
}