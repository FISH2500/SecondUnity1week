using UnityEngine;

public class GameAudio : MonoBehaviour
{
    bool okSE = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            SoundManager.Instance.PlayBGM("GameBGM");
    }
    private void Update()
    {
        if (Area.Instance.HasOK == true && okSE == false)
        {
            SoundManager.Instance.PlaySE("ok");
            okSE = true;
        }
    }
}