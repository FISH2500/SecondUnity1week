using UnityEngine;
using TMPro;

public class MatchPoint : MonoBehaviour
{
	[SerializeField]
	bool _isPlayer;

    void Start()
    {
		int win = GameJudge.Instance.ReturnMatchPoint(_isPlayer);

		TextMeshProUGUI tm = GetComponent<TextMeshProUGUI>();

		if (_isPlayer)
		{
			tm.text = $"ƒvƒŒƒCƒ„[Ÿ—˜”:{win}";
		}
		else
		{
			tm.text = $"“GŸ—˜”:{win}";
		}
	}
}
