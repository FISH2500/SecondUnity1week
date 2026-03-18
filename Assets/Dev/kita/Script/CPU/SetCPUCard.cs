using UnityEngine;

public class SetCPUCard : MonoBehaviour
{
    [SerializeField]
    Deck _deck;

    [SerializeField]
    CPUArea _cpuArea;
    void Start()
    {
		_cpuArea.CardObject = new GameObject[6];
		_cpuArea.CardNum = 6;

		for (int i = 0; i < 6; i++) 
        {
            GameObject card= _deck.DrawCard(1);//CPU‚ÌƒJ[ƒh‚ð6–‡ˆø‚­

            card.transform.position = _cpuArea.CardPosition[i].position;
			_cpuArea.CardObject[i] = card;

		}
    }
}
