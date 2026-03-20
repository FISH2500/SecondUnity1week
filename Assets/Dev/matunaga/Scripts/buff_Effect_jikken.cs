using UnityEngine;

public class buff_Effect_jikken : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SoundManager.Instance.PlayBGM("BossBGM");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SoundManager.Instance.StopBGM();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            EffectManager.Instance.PlayEffect("Buff", transform.position);
            SoundManager.Instance.PlaySE("BuffSE");
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            EffectManager.Instance.StopEffect("Buff");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            EffectManager.Instance.PlayEffect("DeBuff", transform.position);
            SoundManager.Instance.PlaySE("DeBuffSE");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            EffectManager.Instance.StopEffect("DeBuff");
        }
    }
}
