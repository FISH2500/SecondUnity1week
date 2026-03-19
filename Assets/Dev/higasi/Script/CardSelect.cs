using UnityEngine;

public class CardSelect : MonoBehaviour
{
    bool _click = false;
    bool _player1Selected = false; 
    bool _player2Selected = false;
    GameObject _player1Card;
    GameObject _player2Card;

	[SerializeField]
	BattleManegar _battleManegar;

	[SerializeField]
	CPUArea _cpuArea;

	[SerializeField]
	Area _area;

    void Update()
    {
        // カードの選択
        Select();

        // カードが2枚選択されるとバトル開始
        if (_player1Selected && _player2Selected)
        {
            BattleStart();
			enabled = false;
        }

        //====== テスト用 ======
        //if (Input.GetMouseButton(1))
        //{
        //    Vector3 mousePos = Input.mousePosition;
        //    Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
        //    RaycastHit hit;

        //    if (Physics.Raycast(selectRay, out hit, 100.0f))
        //    {
        //        GameObject hitObj = hit.collider.gameObject;
        //        hitObj.GetComponent<SetSoldier>().IsGeneral = true;
        //        Debug.Log("右クリックで将軍に設定(テスト用)");
        //    }
        //}
        // ===================
    }

    void Select()
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
                Debug.Log("hit");
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.CompareTag("Card"))
                {
                    if (!_player1Selected)
                        _player1Selected = true;
                    _player1Card = hitObj;
                }
                if (hitObj.CompareTag("Player2Card") && _player1Selected)
                {
                    if (!_player2Selected)
                        _player2Selected = true;
                    _player2Card = hitObj;
                }
            }
        }
        else if (!Input.GetMouseButton(0) && _click)
        {
            _click = false;
        }

    }

    void BattleStart()
    {
		if (_player1Card.GetComponent<SetSoldier>().IsGeneral)
		{
			Debug.Log("大将が選択されました。残り枚数: " + _area.CardNum);
			if (_area.CardNum >= 2) // 大将以外のカードが残っていたら無効
			{
				_player1Selected = false;
				Debug.Log("大将は最後の1枚でなければ選択できません。");
				return;
			}
			Debug.Log("大将が選択されました。バトル開始");
		}

		if (_player2Card.GetComponent<SetSoldier>().IsGeneral)
		{
			Debug.Log("大将が選択されました。残り枚数: " + _cpuArea.CardNum);
			if (_cpuArea.CardNum >= 2) // 大将以外のカードが残っていたら無効
			{
				_player2Selected = false;
				Debug.Log("大将は最後の1枚でなければ選択できません。");
				return;
			}
			Debug.Log("大将が選択されました。バトル開始");
		}

		_battleManegar.Battle(_player1Card, _player2Card);

		_player1Selected = false;
        _player2Selected = false;
        Debug.Log(BattleManegar.Result);
    }
}
