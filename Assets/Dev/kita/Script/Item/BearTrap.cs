using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject _trapPrefab;//トラップのプレハブ

    private int _spawnTurn;//設置したときのターン

    public bool _isCanTrapSet = false;//トラップが設置できる

    private GameObject _card;//選択したカード

    private GameObject _trapInstance;//生成したトラップのインスタンス

    private bool _isCPUTrap = false;//CPUの罠かどうか

    void Start()
    {
		if (TurnManager.instance.CurrentPlayer == 0)
		{
			SetTrapFlag();
			DispUI.instance.Disp(false);
		}
		else
		{
            _isCPUTrap = true;//CPUの罠
            _isCanTrapSet = false;
            _spawnTurn = TurnManager.instance.CPUTurnCount;//CPUのターン数を取得
            ExecuteCPUTrap();
		}
	}

    // Update is called once per frame
    void Update()
    {

        if (!_isCPUTrap)//Playerの罠が消滅する条件と敵の罠が消滅する条件は違うため分ける
        {
            //設置してからプレイヤーターンが経過したらトラップを解除する
            if (TurnManager.instance.TurnCount > _spawnTurn)
            {
                if (_card != null)
                    _card.GetComponent<SetSoldier>().IsTrap = false;//トラップフラグを下げる
                Debug.Log("罠破棄");
                EndTask();
            }
        }
        else 
        {
            //設置してからプレイヤーターンが経過したらトラップを解除する
            if (TurnManager.instance.CPUTurnCount > _spawnTurn)
            {
                _card.GetComponent<SetSoldier>().IsTrap = false;//トラップフラグを下げる
                Debug.Log("CPU罠破棄");
                EndTask();
            }
        }

        if (_isCanTrapSet)//罠が配置できる状態ならば、罠を設置する場所を決める関数を呼び出す
            SetTrapCard();
    }

    //罠の設置フラグを立てる
    public void SetTrapFlag() 
    {
        TextManegar.instance.SetText("罠を設置する場所を決めてください");
        _isCanTrapSet = true;//トラップが設置できる
        _spawnTurn = TurnManager.instance.TurnCount;//設置時のターン
    }

    //罠を設置する場所を決める
    void SetTrapCard() 
    {
        if (Input.GetMouseButton(0))
        {
            _card = ClickObject();//選択したカードを取得

            if (_card == null) return;//押していない場合処理しない

            bool isGeneral= _card.GetComponent<SetSoldier>().IsGeneral;

            if (isGeneral) return;//大将の場合設置できない

            _card.GetComponent<SetSoldier>().IsTrap = true;//トラップフラグをたてる

            Vector3 trapPosition = _card.transform.position;//クリックしたカードの位置を取得

            trapPosition.y += 0.2f;//トラップをカードの上に配置

            _trapInstance=Instantiate(_trapPrefab, trapPosition, Quaternion.identity);//トラップを生成

            _isCanTrapSet = false;//トラップ設置完了
			DispUI.instance.Disp(true);
      }
    }

    GameObject ClickObject() 
    {
        Vector3 mousePos = Input.mousePosition;
        Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(selectRay, out hit, 100.0f))
        {
            Debug.Log("hit");
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj.CompareTag("Card"))
            {
                return hitObj;//なんのカードを押したかチェック
            }
        }

        return null;//カードをクリックしていない

    }

    //トラップ機能の終了
    void EndTask()
    {
        Destroy(_trapInstance);
        Destroy(gameObject);
    }

	private void ExecuteCPUTrap()//CPUの罠設置
    {
		GameObject[] cpuCards = CPUArea.Instance.CardObject;
		System.Collections.Generic.List<GameObject> candidates = new System.Collections.Generic.List<GameObject>();

		foreach (var obj in cpuCards)
		{
			if (obj != null)// && !obj.GetComponent<SetSoldier>().IsGeneral) 大将にも罠を配置してよいためコメントアウト
				candidates.Add(obj);
		}

		if (candidates.Count > 0)
		{
			_card = candidates[Random.Range(0, candidates.Count)];
			_card.GetComponent<SetSoldier>().IsTrap = true;

			Debug.Log($"{_card.name}に罠を設置しました");
		}
	}
}
