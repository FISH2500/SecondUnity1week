using UnityEditor.VersionControl;
using UnityEngine;
using static CardManegar;

public class CPUArea : MonoBehaviour
{
    public Transform[] CardPosition; // カードをセットする位置
	public GameObject[] CardObject; // 

	[SerializeField] public int GeneralIndex;

	public int CardNum;

	public void RemoveCPUArea(GameObject obj)
	{
		for (int i = 0; i < 6; i++)
		{
			if (obj == CardObject[i]) // セットされているなら
			{
				CardObject[i] = null;
				CardNum--;
				break;
			}
		}
	}

	public void AddCPUArea(GameObject obj)
	{
		for (int i = 0; i < 6; i++)
		{
			if (CardObject[i] == null)
			{
				CardObject[i] = obj;
				CardObject[i].transform.position = CardPosition[i].position;
				CardNum++;
				break;
			}
		}
	}
}
