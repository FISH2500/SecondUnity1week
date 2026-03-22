using TMPro;
using UnityEngine;

using UnityEngine.UI;
public class TextManegar : MonoBehaviour
{

    public static TextManegar instance { private set; get; }

    [SerializeField] private Color _playerColor;

    [SerializeField]private Color _cpuColor;

    private TextMeshProUGUI _actionText;

    void Awake() 
    {
        instance = this;
        _actionText = GetComponent<TextMeshProUGUI>();

		_actionText.text = "";
	}

    //•¶Žš‚Ì•\Ž¦
    public void SetText(string text) 
    {
        if (TurnManager.instance.CurrentPlayer == 0) 
        {
            _actionText.color = _playerColor;
        }
        else
        {
            _actionText.color = _cpuColor;
        }

        _actionText.text = text;
    }
}
