using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject _trapPrefab;//罠のプレハブ

    private int _spawnTurn;//生成されたターン

   　public bool _isCanTrapSet = false;//罠がセットができるどうかのフラグ

    private GameObject _card;//クリックしたカードのオブジェクト

    private GameObject _trapInstance;//生成された罠のオブジェクト
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SetTrap();
    }

    // Update is called once per frame
    void Update()
    {
        //罠が生成されたターンから1ターン経過したら罠を破棄する
        if (TurnManager.instance.TurnCount > _spawnTurn)
        {
            _isCanTrapSet = false;//罠のセットができないようにする
            _card.GetComponent<SetSoldier>().IsTrap = false;//クリックしたカードの罠のフラグをリセット
            Debug.Log("罠が破棄されました");
            EndTask();
        }

        if (_isCanTrapSet)//罠がセットできる
        SetTrapCard();
    }

    //トラップのセットを許可する処理
    public void SetTrapFlag() 
    {
        TextManegar.instance.SetText("罠をどこに設置しますか？");
        _isCanTrapSet = true;//罠がセットできるようになる
        _spawnTurn = TurnManager.instance.TurnCount;//生成されたターンを記録
    }

    //オブジェクトにトラップを設置する処理
    void SetTrapCard() 
    {
        if (Input.GetMouseButton(0))
        {
            _card = ClickObject();//クリックしたカードを調べる

            if (_card == null) return;

            bool isGeneral= _card.GetComponent<SetSoldier>().IsGeneral;

            if (isGeneral) return;//将軍には罠をセットできない

            _card.GetComponent<SetSoldier>().IsTrap = true;//クリックしたカードに罠のフラグを立てる

            _isCanTrapSet = true;//罠がセットされた

            Vector3 trapPosition = _card.transform.position;//クリックしたカードの位置を取得

            trapPosition.y += 0.2f;//罠がカードの上に表示されるようにY座標を少し上げる

            _trapInstance= Instantiate(_trapPrefab, trapPosition, Quaternion.identity);//罠をクリックしたカードの位置に生成

            _isCanTrapSet = false;//罠のセットが完了したのでフラグをリセット
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
                    return hitObj;//クリックしたカードのオブジェクトを返す
                }
            }
        }

        return null;//クリックしたオブジェクトがカードでない場合はnullを返す

    }

    //ターン終了時に破棄
    void EndTask()
    {
        Destroy(_trapInstance);
        Destroy(gameObject);
    }
}
