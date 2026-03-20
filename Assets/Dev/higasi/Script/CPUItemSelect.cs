using UnityEngine;

public class CPUItemSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static public bool ItemSelect(int id)
    {
        switch(id)
        {
            case 0:
                for (int i = 0; i < CPUArea.Instance.CardObject.Length; i++)
                {
                    if (CPUArea.Instance.CardObject[i] == null) continue;
                    if (!CPUArea.Instance.CardObject[i].GetComponent<SetSoldier>().IsBack)
                    {
                        return true;
                    }
                }
                return false;
            case 1:
                return true;
            case 2:
                for (int i = 0; i < Area.Instance.CardObj.Length; i++)
                {
                    if (Area.Instance.CardObj[i] == null) continue;
                    if (Area.Instance.CardObj[i].GetComponent<SetSoldier>().IsBack)
                    {
                        return true;
                    }
                }
                return false;
            case 3:
                return true;
            case 4:
                return true;
            case 5:
                if (Area.Instance.CardNum > 1 && CPUArea.Instance.CardNum > 1)
                {
                    return true;
                }
                else
                    return false;
            case 6:
                return true;
            case 7:
                if (CPUItem.Instance.GetItemCount() >= 2)
                {
                    return true;
                }
                else
                    return false;
            case 8:
                if (CPUItem.Instance.GetItemCount() >= 3)
                {
                    return true;
                }
                else
                    return false;
            case 9:
                if (Area.Instance.CardNum > 1)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
}
