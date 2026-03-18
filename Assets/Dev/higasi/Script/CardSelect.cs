using UnityEngine;

public class CardSelect : MonoBehaviour
{
    bool _click = false;
    bool _player1Selected = false;
    bool _player2Selected = false;
    BattleManegar BattleManegar;
    GameObject _player1Card;
    GameObject _player2Card;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // SoldierData cardData = new SoldierData();
        BattleManegar = GameObject.Find("BattleManegar").GetComponent<BattleManegar>();
        //for (int i = 0; i < CardManegar.MaxCardNum * 2; i++)
        //{
        //    SoldierData.SoldierList[i].General = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // カードの選択
        if (Input.GetMouseButton(0) && !_click)
        {
            //Debug.Log("click");
            _click = true;
            Vector3 mousePos = Input.mousePosition;
            Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(selectRay, out hit, 100.0f))
            {
                Debug.Log("hit");
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.CompareTag("Player1Card"))
                {
                    if (!_player1Selected)
                        _player1Selected = true;
<<<<<<< Updated upstream
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
=======
                    _player1Card = hitObj;
                }
                if (hitObj.CompareTag("Player2Card") && _player1Selected)
                {
                    if (!_player2Selected)
                        _player2Selected = true;
                    _player2Card = hitObj;
>>>>>>> Stashed changes
                }
            }
        }
        else if (!Input.GetMouseButton(0) && _click)
        {
            _click = false;
        }
        
        // カードが2枚選択されるとバトル開始
        if (_player1Selected && _player2Selected)
        {
            //if (_player2Card.GetComponent<Soldier>().General)
            if (true) // テスト用に常に大将判定にする
                BattleManegar.BattleVsGeneral(_player1Card, _player2Card, true);
            else
                BattleManegar.Battle(_player1Card, _player2Card, true);
            _player1Selected = false;
            _player2Selected = false;
            Debug.Log(BattleManegar.Result);
        }

        //====== テスト用 ======
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(selectRay, out hit, 100.0f))
            {
                GameObject hitObj = hit.collider.gameObject;
                hitObj.GetComponent<Soldier>().General = true;
            }
        }
        // ===================
    }
}
