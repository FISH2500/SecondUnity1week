using System.Collections.Generic;
using UnityEngine;
using System.Linq; // 便利機能用

public class CPUItem : MonoBehaviour
{
	[SerializeField] private ItemDeck _itemDeck;
	[SerializeField] private List<int> _myItems = new List<int>();
	[SerializeField] private CPUArea _cpuArea;
	[SerializeField] private Area _playerArea;
	private ItemBase _itemBase;

	public static CPUItem Instance;

	private void Awake() // Startより早く初期化
	{
		Instance = this;
		_itemBase = GetComponent<ItemBase>();
	}

	private void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			_myItems.Add(_itemDeck.CPUDrawItem());
		}
	}

	public void CPUUseItem()
	{
		if (_myItems.Count == 0) return;

		// 最適なアイテムのインデックスを選択する
		int bestItemIndex = EvaluateBestItem();
		int id = _myItems[bestItemIndex];

		_myItems.RemoveAt(bestItemIndex);

		_itemBase.ItemID = id;
		_itemBase.Use();
	}

	private int EvaluateBestItem()
	{
		// アイテムが1つしかないならそれを返す
		if (_myItems.Count == 1) return 0;

		int bestIdx = 0;
		float maxScore = -100;

		for (int i = 0; i < _myItems.Count; i++)
		{
			float score = CalculateItemScore(_myItems[i]);
			if (score > maxScore)
			{
				maxScore = score;
				bestIdx = i;
			}
		}
		return bestIdx;
	}

	private float CalculateItemScore(int id)
	{
		float score = 0;

		int revealedPlayerCards = 0;
		if (_playerArea != null && _playerArea.CardObj != null)
		{
			foreach (var c in _playerArea.CardObj)
			{
				if (c != null)
				{
					var s = c.GetComponent<SetSoldier>();
					if (s != null && !s.IsBack) revealedPlayerCards++;
				}
			}
		}

		int cpuCardCount = _cpuArea != null ? _cpuArea.CardNum : 0;

		switch (id)
		{
			case 0: // シャッフル：相手に見られているカードが多いほど価値が高い
				score = revealedPlayerCards * 20;
				break;
			case 1: // 罠：自分のカードが少ない（守りたい）時に価値アップ
				score = (6 - cpuCardCount) * 15;
				break;
			case 2: // 相手を表に：相手が裏向きのカードを多く持っているなら強い
				score = (6 - revealedPlayerCards) * 15;
				break;
			case 4: // 革命：自分が弱いカードばかり持っているなら超強力
				score = CheckRevolutionValue();
				break;
			case 5: // カード交換：相手の強カードが見えていて、自分の弱カードと替えられるなら神
				score = 10; // 暫定
				break;
			case 9: // 引き直し：相手のめちゃくちゃ強いカードが見えている時に使う
				score = CheckRedrawValue();
				break;
			default: // その他（ドロー追加など）は腐らないので中程度の評価
				score = 30;
				break;
		}
		return score;
	}

	private float CheckRevolutionValue()
	{
		// 自分の平均攻撃力が低いなら「革命」の価値を高める
		int totalAtk = 0;
		int count = 0;

		if (_cpuArea == null || _cpuArea.CardObject == null) return 0;

		foreach (var c in _cpuArea.CardObject)
		{
			// ここで null チェックを追加
			if (c == null) continue;

			var s = c.GetComponent<SetSoldier>();
			if (s != null)
			{
				totalAtk += s.SoldierAtk;
				count++;
			}
		}

		// カードが1枚もない場合は革命の価値なし
		if (count == 0) return 0;

		float avg = (float)totalAtk / count;

		// 平均攻撃力が低い（弱い札ばかり）ほど、スコアが高くなる計算
		// 例: 平均 3 なら (12 - 3) * 10 = 90点（超使いたい）
		// 例: 平均 10 なら (12 - 10) * 10 = 20点（あまり使いたくない）
		return Mathf.Max(0, (12 - avg) * 10);
	}

	private float CheckRedrawValue()
	{
		// 相手の場に7以上の強いカードが見えていたらスコアアップ
		if (_playerArea == null || _playerArea.CardObj == null) return 10;

		foreach (var c in _playerArea.CardObj)
		{
			if (c == null) continue;
			var s = c.GetComponent<SetSoldier>();
			if (s != null && !s.IsBack && s.SoldierAtk >= 8) return 80;
		}
		return 10;
	}

	public int GetItemCount() => _myItems.Count;

	public bool ShouldUseItem()
	{
		if (_myItems.Count == 0) return false;

		// 全所持アイテムの中で最高スコアを算出
		float bestScore = -1;
		foreach (int id in _myItems)
		{
			float s = CalculateItemScore(id);
			if (s > bestScore) bestScore = s;
		}

		// スコアが 40 以上なら「使う価値あり」と判断
		// (例: 相手のカードが2枚以上バレている、または自分がピンチなど)
		return bestScore >= 40f;
	}

	public int GetFirstItemID()
	{
		return _myItems.Count > 0 ? _myItems[0] : -1;
	}

	public void ReplaceFirstItem(int newID)
	{
		if (_myItems.Count > 0)
		{
			_myItems[0] = newID;
		}
		else
		{
			// もしリストが空なら新しく追加
			_myItems.Add(newID);
		}
	}
}