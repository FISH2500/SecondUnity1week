using UnityEngine;

public class CardSetCheck : MonoBehaviour
{
    public static CardSetCheck Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        Instance = this;
    }

    public bool SetCheck()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Card");
        int CardCount = objs.Length;
        if (CardCount == Area.Instance.CardNum)
            return true;
        else return false;
    }
}
