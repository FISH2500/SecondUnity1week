using UnityEngine;

public class RemoveMeshRenderer : MonoBehaviour
{
    void Start()
    {
        MeshRenderer _mesh=GetComponent<MeshRenderer>();

        _mesh.enabled = false;
    }
}
