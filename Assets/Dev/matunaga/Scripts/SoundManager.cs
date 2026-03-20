using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct SoundData
{
    public string Name;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public SoundData[] SoundList;

    
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _seSource;

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
    }
    //--------------------------------------------------------------------------------
    public void PlaySE(string soundName)
    {
        foreach (SoundData data in SoundList)
        {
            if (data.Name == soundName)
            {
                _seSource.PlayOneShot(data.Clip, data.Volume);
                return;
            }
        }
        Debug.LogWarning("Sound(SE)Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÇ≈ÇµÇΩÅF " + soundName);
    }
    //public void StopSE()
    //{
    //    foreach (SoundData data in SoundList) {
    //        if (data.Clip == _seSource.clip)
    //        {
    //            _seSource.Stop();
    //            return;
    //        }
    //    }
    //}
    //--------------------------------------------------------------------------------
    public void PlayBGM(string soundName)
    {
        foreach (SoundData data in SoundList)
        {
            if (data.Name == soundName)
            {
                _bgmSource.clip = data.Clip;
                _bgmSource.volume = data.Volume;
                _bgmSource.loop = true; 
                _bgmSource.Play();
                return;
            }
        }
        Debug.LogWarning("Sound(BGM)Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÇ≈ÇµÇΩÅF " + soundName);
    }
    public void StopBGM()
    {
        foreach (SoundData data in SoundList)
        {
            if (data.Clip == _bgmSource.clip)
            {
                _bgmSource.Stop();
                return;
            }
        }
    }
}