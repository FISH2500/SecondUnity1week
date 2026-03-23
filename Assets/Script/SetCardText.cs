using UnityEngine;
using System.Collections; // コルーチンに必要

public class SetCardText : MonoBehaviour
{
	[SerializeField] private float _delaySeconds = 1.3f;

	void Start()
	{
		StartCoroutine(DelayedProcess());
	}

	private IEnumerator DelayedProcess()
	{
		yield return new WaitForSeconds(_delaySeconds);

		TextManegar.instance.SetText("札を配置してください");
	}
}