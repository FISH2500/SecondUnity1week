using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SoundData
{
    public string Name;
    public AudioClip Clip;  //音源データ
    [Range(0, 100)] public float Volume; //音量   0 - 100
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public SoundData[] SoundList;

    [SerializeField]
    private AudioSource _audioSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // AudioSourceコンポーネントがアタッチされていない場合は、同じGameObjectから取得
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }
    public void PlaySE(string soudName)
    {
        foreach (SoundData data in SoundList)
        {
            if (data.Name == soudName)
            {
                _audioSource.PlayOneShot(data.Clip, data.Volume);
                return;
            }
        }
        Debug.LogWarning("Soundが見つかりませんでした： " + soudName);
    }
}