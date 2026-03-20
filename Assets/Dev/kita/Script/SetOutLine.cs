using UnityEngine;

public class SetOutLine : MonoBehaviour
{
    public bool _isSetOutLine;//アウトラインをつけるかどうかのフラグ

    [SerializeField]
    Material _outLine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SetOutline();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //アウトラインをつける関数
    public void SetOutline(float thickness) 
    {
        if (!_isSetOutLine) 
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            Material material= meshRenderer.materials[1];

            material.SetFloat("_OutLineThickness", thickness);

            _isSetOutLine = true;//アウトラインフラグ
        }

    }
    //アウトラインを消す関数
    public void ReSetOutline()
    {
        if (_isSetOutLine)
        {
            Debug.Log("アウトライン消す");
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            Material material = meshRenderer.materials[1];

            material.SetFloat("_OutLineThickness", 0.00f);

            _isSetOutLine = false;//アウトラインフラグ
        }

    }
}
