using UnityEngine;

public class SetCPUCard : MonoBehaviour
{
    [SerializeField]
    Deck _deck;

    [SerializeField]
    CPUArea _cpuArea;
    void Start()
    {
        for(int i = 0; i < 6; i++) 
        {
            GameObject _card= _deck.DrawCard(1);//CPU‚ÌƒJ[ƒh‚ð6–‡ˆø‚­

            _card.transform.position = _cpuArea.CardPosition[i].position;

            _card.transform.rotation = _cpuArea.CardPosition[i].rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
