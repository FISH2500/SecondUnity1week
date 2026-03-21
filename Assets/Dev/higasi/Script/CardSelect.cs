using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CardSelect : MonoBehaviour
{
    bool _click = false;
    bool _player1Selected = false; 
    bool _player2Selected = false;
    GameObject _player1Card = null;
    GameObject _player2Card = null;
    Vector3 _cardOriginPos;

	[SerializeField]
	BattleManegar _battleManegar;

	[SerializeField]
	CPUArea _cpuArea;

	[SerializeField]
	Area _area;

    [SerializeField]
    Camera cam;

	[SerializeField] private Canvas _attackCanvas;

    [SerializeField] private CardBattleDirection _cardBattleDirection;

	private void Awake()
	{
		_player1Card = null;
		_player2Card = null;
	}

	void Update()
    {
        // カードの選択
        Select();
        Drag();
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
				SetSoldier soldier = hitObj.GetComponent<SetSoldier>();

				if (hitObj.CompareTag("Card"))
                {
					if (soldier.IsGeneral && _area.CardNum > 1)
					{
						TextManegar.instance.SetText("大将は最後にしか出せません");
						return;
					}

					_player1Selected = true;
                    _player1Card = hitObj;
                    _cardOriginPos = hitObj.transform.position;

					soldier.RotateSetFront();

					TextManegar.instance.SetText("攻撃対象の札を選択してください");

                }
            }
        }
        else if (!Input.GetMouseButton(0) && _click)
        {
            _click = false;
        }

    }

    void Drag()//攻撃対象の選択
    {
		if (!_player1Selected) return;//攻撃するカードが選択されていない場合は攻撃対象を選択できない

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 cardPos = Area.Instance.GetCardPositions()[0].position;
            cardPos = new Vector3(cardPos.x, cardPos.y + 1.0f, cardPos.z);
            Plane dragPlane = new Plane(Vector3.up, cardPos);
            float enter;

            // 平面とRayの交点を取得
            if (dragPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                _player1Card.transform.position = hitPoint;
            }

            
            GameObject targetObj = null;

            float minDistance = float.MaxValue;

            for (int i = 0; i < _cpuArea.CardObject.Length; i++) 
            {

                if (_cpuArea.CardObject[i] == null) continue;
                float distance = Vector3.Distance(_player1Card.transform.position, _cpuArea.CardObject[i].transform.position);//攻撃カードとCPUのカードの距離を取得
                _cpuArea.CardObject[i].GetComponent<SetOutLine>().ReSetOutline();//アウトライン非表示

                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetObj = _cpuArea.CardObject[i];
                }

            }

            if (targetObj != null && minDistance <= 5.0f)
            {
                targetObj.GetComponent<SetOutLine>().SetOutline(0.02f);//アウトライン表示
            }
            
            //ドラッグ中に攻撃対象と攻撃カードの距離を取得して一定の距離内に入ったオブジェクトをアウトラインにする


        }
        

        // 離したら解除
        if (Input.GetMouseButtonUp(0))
        {
            
            GameObject targetObj = null;
            float minDistance = float.MaxValue;
            for (int i = 0; i < _cpuArea.CardObject.Length; i++)
            {
                Debug.Log(_player1Card);
                Debug.Log(_cpuArea);
                Debug.Log(_cpuArea.CardObject[i]);
                if (_cpuArea.CardObject[i] == null) continue;

                _cpuArea.CardObject[i].GetComponent<SetOutLine>().ReSetOutline();

                float distance = Vector3.Distance(_player1Card.transform.position, _cpuArea.CardObject[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetObj = _cpuArea.CardObject[i];
                }
            }
            if (targetObj != null && minDistance <= 5.0f)
            {
                _player2Card = targetObj;
                _player2Selected = true;
            }
            // カードが2枚選択されるとバトル開始
            if (_player1Selected && _player2Selected)
            {
                //カードの演出
                _cardBattleDirection.SetBattleDirection(_player1Card, _player2Card);
                if (BattleStart())
                {
                    if (_player2Card.GetComponent<SetSoldier>().IsGeneral)
                    {
                        // 大将を選択した場合はすぐ戻す
                        StartCoroutine(ReturnCard(_player1Card, false));
                    }
                    else
                    {
                        // 通常バトルは演出後に戻す
                        StartCoroutine(ReturnCard(_player1Card, true));
                    }


                    if (BattleManegar.Result == BattleManegar.BattleResult.Lose)
                    {
                        StopCoroutine(ReturnCard(_player1Card,false));//
                    }
                    _attackCanvas.enabled = false;
                    enabled = false;
                }
            }
            else
            {
                StartCoroutine(ReturnCard(_player1Card,false));
                _player1Card.GetComponent<SetSoldier>().RotateSetBack(TurnManager.instance.CurrentPlayer);

                _player1Selected = false;
                _player2Selected = false;
                _player1Card = null;
                _player2Card = null;
            }
        }
    }

    bool BattleStart()
    {
		if (_player1Card.GetComponent<SetSoldier>().IsGeneral)
		{
			Debug.Log("大将が選択されました。バトル開始");
		}

		if (_player2Card.GetComponent<SetSoldier>().IsGeneral)
		{
			Debug.Log("大将が選択されました。残り枚数: " + _cpuArea.CardNum);
			if (_cpuArea.CardNum >= 2) // 大将以外のカードが残っていたら無効
			{
				_player1Card.GetComponent<SetSoldier>().RotateSetBack(TurnManager.instance.CurrentPlayer);

				_player1Selected = false;
				_player2Selected = false;
				_player1Card = null;
				_player2Card = null;
				Debug.Log("大将は最後の1枚でなければ選択できません。");
			}
            else
                Debug.Log("大将が選択されました。バトル開始");
		}

        if (!_player1Selected || !_player2Selected) return false;
        _battleManegar.Battle(_player1Card, _player2Card);

		_player1Selected = false;
        _player2Selected = false;
        Debug.Log(BattleManegar.Result);

        
        return true;
    }

    private IEnumerator ReturnCard(GameObject moveCard ,bool isBattle)//バトルが開始されなかったときにカードを元の位置に戻すためのコルーチン
    {
        if (isBattle) 
        {
            yield return new WaitForSeconds(3.0f);//演出のため戻るのに3秒間待たせる
        }

        while (true)
        {
            yield return null;
            if (moveCard == null) yield break;
            float speed = 40.0f;
            moveCard.transform.position = Vector3.MoveTowards(
                moveCard.transform.position,
                _cardOriginPos,
                speed * Time.deltaTime);
            if (moveCard.transform.position == _cardOriginPos)
            {
                yield break;
            }
        }
    }

}
