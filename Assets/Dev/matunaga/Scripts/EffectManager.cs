using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct EffectData
{
    public string Name;
    public GameObject EffectPrefab;
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    public EffectData[] EffectList;

    // アクティブなエフェクトのリスト
    private List<GameObject> _activeEffects = new List<GameObject>();

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
    public void PlayEffect(string effectName, Vector3 targetPos)
    {   // EffectListから名前が一致するエフェクトを探して生成
        foreach (EffectData data in EffectList)
        {
            if (data.Name == effectName)
            {
                GameObject obj = Instantiate(data.EffectPrefab, targetPos, Quaternion.identity);

                // エフェクトの名前を設定（後で停止するため）
                obj.name = data.Name;

                _activeEffects.Add(obj);
                return;
            }
        }

        Debug.LogWarning("Effectが見つかりませんでした： " + effectName);
    }

    public void StopEffect(string effectName)
    {   //アクティブなエフェクトのリストの中から名前が一致するエフェクトを探して停止
        for (int i = _activeEffects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _activeEffects[i];

            //オブジェクトが既に消えていないかのチェック
            if (obj == null)
            {
                _activeEffects.RemoveAt(i);
                continue;
            }
            // 名前が一致するエフェクトを停止
            if (obj.name.StartsWith(effectName))
            {
                Destroy(obj);
                _activeEffects.RemoveAt(i);
            }
        }
    }
}