using UnityEngine;
public class CanvasFalse : MonoBehaviour
{
	void Start()
	{
		Canvas _mesh = GetComponent<Canvas>();

		_mesh.enabled = false;
	}
}
