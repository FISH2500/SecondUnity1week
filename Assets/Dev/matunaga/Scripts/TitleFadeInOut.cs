using UnityEngine;
using UnityEngine.UI;

public class TitleFadeInOut : MonoBehaviour
{
    private Image _targetImage;

    // インスペクターで点滅の速さを調整できるようにする
    [SerializeField] private float _blinkSpeed = 1.5f;

    void Start()
    {
        // アタッチされているオブジェクトのImageコンポーネントを取得
        _targetImage = GetComponent<Image>();
    }

    void Update()
    {
        Color color = _targetImage.color;
        
        color.a = Mathf.PingPong(Time.time * _blinkSpeed, 1.0f);

        // 計算した色をImageに戻す
        _targetImage.color = color;
    }
}
