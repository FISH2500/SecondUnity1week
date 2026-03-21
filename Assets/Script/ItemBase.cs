using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBase : MonoBehaviour
{
	public int ItemID;        // 識別ID

	private const float _time = 0.8f;

	[SerializeField] ItemDataBase _itemDatabase;

	[SerializeField]
	private Image _frontImage;//兵士の表の画像を表示するUI 

	[SerializeField]
	private Image _backImage;//兵士の裏の画像を表示するUI

	[SerializeField]
	private MeshRenderer _highlight;

	[SerializeField]
	private bool IsCPU;

	[SerializeField]
	private GameObject _trap;

	[SerializeField]
	private SwapCard _swapCard;

	[SerializeField]
	private OpenCard _openCard;

	[SerializeField]
	private Item_RedrawTarget item_RedrawTarget;

	[SerializeField]
	private Item_SwapItem _item_SwapItem;

	public bool IsBack;

	private void Start()
	{
		if (!IsCPU) SetSprite();
		if (!IsCPU) Highlight(false);
	}

	public void SetSprite()
	{
		_frontImage.sprite = _itemDatabase.itemDatas[ItemID].Front;//兵士の画像をセット

		_backImage.sprite = _itemDatabase.Back;//兵士の裏の画像をセット
	}

	public void Highlight(bool highlight)
	{
		//_highlight.enabled = highlight;
		if(highlight)
		gameObject.GetComponent<SetOutLine>().SetOutline(0.03f);

        else
			gameObject.GetComponent<SetOutLine>().ReSetOutline();

    }

	public void SetBack()//裏面にする
	{
		transform.rotation = Quaternion.Euler(0, 0, 180);

		IsBack = true;
	}

	public void SetFront() //表にする
	{
		IsBack = false;

		transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	public void Use()
	{
		switch (ItemID)
		{
			case 0: // ①大将以外のカードを裏にして、シャッフルする
				Shuffle();
				break;
			case 1: // ②選んだ自分のカードに罠を張る
				Trap();
				break;
			case 2: // ③相手に表にさせるカードを選ばせる
				Reverse();
				break;
			case 3: // ④カードの引ける枚数を+1する
				AddDraw();
				break;
			case 4: // ⑤このターンのみカードの強さを逆転させる
				StrengthInversion();
				break;
			case 5: // ⑥1枚だけ相手のカードと自分のカードを交換する
				SwapCard();
				break;
			case 6: // ⑦数値勝負に勝つと、もう一回攻撃できる
				AdditionalAttack();
				break;
			case 7: // ⑧1枚だけ相手のアイテムカードと自分のを交換する
				SwapItem();
				break;
			case 8: // ⑨このターン中、アイテムの使用制限がなくなる
				UnlimitedItem();
				break;
			case 9: // ⑩相手のカードを一枚選んで、カードを引き直させる
				ReDrawTarget();
				break;
		}
	}

	public void Shuffle()
	{
		StartCoroutine(AnimateShuffle());
	}

	private IEnumerator AnimateShuffle()
	{
		// 現在のターンから対象のAreaを取得
		bool isPlayer = (TurnManager.instance.CurrentPlayer == 0);
		GameObject[] targetCards;
		Transform[] positions;

		if (isPlayer)
		{
			Area playerArea = Area.Instance;
			targetCards = playerArea.CardObj;
			positions = playerArea.GetCardPositions();
		}
		else
		{
			CPUArea cpuArea = CPUArea.Instance;
			targetCards = cpuArea.CardObject;
			positions = cpuArea.CardPosition;
		}

		// --- 1. シャッフル対象のインデックスを抽出 ---
		List<int> shuffleIndices = new List<int>();
		for (int i = 0; i < targetCards.Length; i++)
		{
			if (targetCards[i] == null) continue;

			SetSoldier s = targetCards[i].GetComponent<SetSoldier>();
			if (s.IsGeneral) continue;

			s.RotateSetBack(TurnManager.instance.CurrentPlayer);
			shuffleIndices.Add(i);
		}

		yield return new WaitForSeconds(0.6f);

		// --- 2. ランダムな入れ替えパターンの作成 ---
		List<int> randomIndices = new List<int>(shuffleIndices);
		for (int i = randomIndices.Count - 1; i > 0; i--)
		{
			int j = Random.Range(0, i + 1);
			(randomIndices[i], randomIndices[j]) = (randomIndices[j], randomIndices[i]);
		}

		// --- 3. アニメーション用の座標保持 ---
		Vector3[] startPositions = new Vector3[targetCards.Length];
		Vector3[] endPositions = new Vector3[targetCards.Length];
		GameObject[] originalObjs = (GameObject[])targetCards.Clone();

		for (int i = 0; i < shuffleIndices.Count; i++)
		{
			int oldIdx = shuffleIndices[i];
			int newIdx = randomIndices[i];

			// 移動前の座標を記録
			startPositions[oldIdx] = originalObjs[newIdx].transform.position;

			// 移動後の座標（スロット位置）を計算
			Vector3 newPos = positions[oldIdx].position;
			newPos.y = originalObjs[newIdx].transform.position.y; // 高さは維持
			endPositions[oldIdx] = newPos;

			// 内部配列の参照を入れ替え
			targetCards[oldIdx] = originalObjs[newIdx];
		}

		// --- 4. 移動アニメーションの実行 ---
		float elapsed = 0;
		while (elapsed < _time)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / _time;
			// 滑らかにするなら t = t * t * (3f - 2f * t); (SmoothStep)

			for (int i = 0; i < shuffleIndices.Count; i++)
			{
				int idx = shuffleIndices[i];
				targetCards[idx].transform.position = Vector3.Lerp(startPositions[idx], endPositions[idx], t);
			}
			yield return null;
		}

		// --- 5. 最終位置に固定 ---
		for (int i = 0; i < shuffleIndices.Count; i++)
		{
			int idx = shuffleIndices[i];
			targetCards[idx].transform.position = endPositions[idx];
		}

		DispUI.instance.Disp(true);
	}

	private void Trap()
	{
		Instantiate(_trap);
	}

	private void Reverse()
	{
		_openCard.RunOpenCard();
	}

	private void AddDraw()
	{
		DrawCard.instance.AddDrawNum(1);
		DispUI.instance.Disp(true);
	}

	private void StrengthInversion()
	{
		TurnManager.instance.SetRevolution();
	}

	private void SwapCard()
	{
		_swapCard.StartSwapMode();
	}

	private void AdditionalAttack()
	{
		TurnManager.instance.SetDoubleAttack();
	}

	private void SwapItem()
	{
		_item_SwapItem.StartItemSwap();
	}

	private void UnlimitedItem()
	{
		TurnManager.instance.SetUnlimitedItem();
	}

	private void ReDrawTarget()
	{
		item_RedrawTarget.StartRedraw();
	}
}
