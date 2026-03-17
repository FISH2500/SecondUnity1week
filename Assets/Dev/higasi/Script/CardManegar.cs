using UnityEngine;

public class CardManegar : MonoBehaviour
{
    public static int MaxCardNum = 6;
    public static int MaxCardPower = 13;
    public static int MinCardPower = 1;

    public struct Card
    {
        public int Power;
        public bool IsOpen;
        public bool IsLost;
        public bool IsGeneral;
    }

    public static Card[] player1pCards = new Card[MaxCardNum];
    public static Card[] player2pCards = new Card[MaxCardNum];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < MaxCardNum; i++)
        {
            player1pCards[i].Power = Random.Range(MinCardPower, MaxCardPower + 1);
            player1pCards[i].IsOpen = false;
            player1pCards[i].IsLost = false;
            player1pCards[i].IsGeneral = false;
            player2pCards[i].Power = Random.Range(MinCardPower, MaxCardPower + 1);
            player2pCards[i].IsOpen = false;
            player2pCards[i].IsLost = false;
            player2pCards[i].IsGeneral = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
