using UnityEngine;

public class buff_Effect_jikken : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            EffectManager.Instance.PlayEffect("Buff", transform.position);
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            EffectManager.Instance.StopEffect("Buff");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            EffectManager.Instance.PlayEffect("DeBuff", transform.position);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            EffectManager.Instance.StopEffect("DeBuff");
        }
    }
}
