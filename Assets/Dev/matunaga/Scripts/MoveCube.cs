using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newpos = transform.position;
        newpos.x += 0.05f;
        transform.position = newpos;
    }
}
