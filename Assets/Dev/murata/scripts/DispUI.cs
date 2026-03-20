using UnityEngine;
using UnityEngine.UIElements;

public class DispUI : MonoBehaviour
{
	private Canvas _canvas;

	public static DispUI instance;

	private void Awake()
	{
		_canvas = GetComponent<Canvas>();
		instance = this;
		Disp(false);
	}

	public void Disp(bool disp)
	{
		_canvas.enabled = disp;
	}
}
