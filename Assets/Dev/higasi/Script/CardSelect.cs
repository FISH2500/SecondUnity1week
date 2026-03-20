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

    void Update()
    {
        // カードの選択
        Select();
        Drag();

        // カードが2枚選択されるとバトル開始
        if (_player1Selected && _player2Selected)
        {
            BattleStart();
			enabled = false;
        }
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
                _cardOriginPos = hitObj.transform.position;

                if (hitObj.CompareTag("Card"))
                {
                    if (!_player1Selected)
                        _player1Selected = true;
                    _player1Card = hitObj;

                    TextManegar.instance.SetText("攻撃対象の札を選択してください");

                }
                //if (hitObj.CompareTag("Player2Card") && _player1Selected)//敵のカードを選択
                //{
                //    if (!_player2Selected)
                //        _player2Selected = true;
                //    _player2Card = hitObj;
                //    TextManegar.instance.SetText("");
                //}
            }
        }
        else if (!Input.GetMouseButton(0) && _click)
        {
            _click = false;
        }

    }

    void Drag()
    {
        if (Input.GetMouseButton(0) && _player1Selected == true)
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
        }

        // 離したら解除
        if (Input.GetMouseButtonUp(0))
        {
            if (!_player1Selected) return;
            GameObject targetObj = null;
            float minDistance = float.MaxValue;
            for (int i = 0; i < _cpuArea.CardNum; i++)
            {
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
            StartCoroutine(ReturnCard());
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

    private IEnumerator ReturnCard()
    {
        while (true)
        {
            yield return null;
            float speed = 40.0f;
            _player1Card.transform.position = Vector3.MoveTowards(
                _player1Card.transform.position,
                _cardOriginPos,
                speed * Time.deltaTime);
            if (_player1Card.transform.position == _cardOriginPos)
            {
                yield break;
            }
        }
    }
}
