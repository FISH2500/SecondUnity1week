using UnityEngine;

public class RemoveMeshRenderer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer _mesh=GetComponent<MeshRenderer>();

        _mesh.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
