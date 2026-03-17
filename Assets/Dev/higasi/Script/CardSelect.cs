using UnityEngine;

public class CardSelect : MonoBehaviour
{
    bool _click = false;
    bool _player1Selected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !_click)
        {
            //Debug.Log("click");
            _click = true;
            Vector3 mousePos = Input.mousePosition;
            Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(selectRay, out hit, 100.0f))
            {
                //Debug.Log("hit");
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.CompareTag("Player1Card"))
                {
                    if (!_player1Selected)
                    {
                        _player1Selected = true;
                    }
                    BattleManegar.PlayerCardPower = hitObj.GetComponent<SetSoldier>().CardIndex;
                }
                if (hitObj.CompareTag("Player2Card") && _player1Selected)
                {
                    BattleManegar.EnemyCardPower = hitObj.GetComponent<SetSoldier>().CardIndex;
                    BattleManegar battleManegar = GameObject.Find("BattleManegar").GetComponent<BattleManegar>();
                    battleManegar.Battle();
                    Debug.Log(BattleManegar.Result);
                    _player1Selected = false;
                }
            }
        }
        else if (!Input.GetMouseButton(0) && _click)
        {
            _click = false;
        }
    }
}
