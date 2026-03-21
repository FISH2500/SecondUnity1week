using UnityEngine;

public class GameAudio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            SoundManager.Instance.PlayBGM("GameBGM");
    }
}
