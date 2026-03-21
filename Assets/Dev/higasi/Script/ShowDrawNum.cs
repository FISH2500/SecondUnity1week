using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class ShowDrawNum : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _drawCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int remainingDrawNum;
        remainingDrawNum = DrawCard.instance._drawCardNum - DrawCard.instance._drawCountPlayer1;
        _drawCount.text = "Žc‚è" + remainingDrawNum.ToString() + "‰ñ";
    }
}
