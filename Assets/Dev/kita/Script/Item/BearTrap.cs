using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject _trapPrefab;//トラップのプレハブ

    private int _spawnTurn;//設置したときのターン

    public bool _isCanTrapSet = false;//トラップが設置できる

    private GameObject _card;//選択したカード

    private GameObject _trapInstance;//生成したトラップのインスタンス
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTrapFlag();
    }

    // Update is called once per frame
    void Update()
    {
        //設置してからターンが経過したらトラップを解除する
        if (TurnManager.instance.TurnCount > _spawnTurn)
        {
            _card.GetComponent<SetSoldier>().IsTrap = false;//トラップフラグを下げる
            Debug.Log("罠破棄");
            EndTask();
        }

        if (_isCanTrapSet)//トラップの設置処理
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

            _trapInstance= Instantiate(_trapPrefab, trapPosition, Quaternion.identity);//トラップの生成

            _isCanTrapSet = false;//トラップ設置完了
        }
    }

    GameObject ClickObject() 
    {
        Vector3 mousePos = Input.mousePosition;
        Ray selectRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        
        {
            if (Physics.Raycast(selectRay, out hit, 100.0f))
            {
                Debug.Log("hit");
                GameObject hitObj = hit.collider.gameObject;

                if (hitObj.CompareTag("Card"))
                {
                    return hitObj;//なんのカードを押したかチェック
                }
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
}
