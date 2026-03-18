using TMPro;
using UnityEngine;

public class OutPutAttack : MonoBehaviour
{
    public bool IsShowText;//テキストを表示するかどうか

    int soldierAtk;

    Vector3 _cardPos;

    [SerializeField]
    GameObject[] _numberObj=new GameObject[10];//3Dテキストを格納する配列

    [SerializeField]
    float _rotateSpeed;//回転速度

    GameObject _parent;//10,1の位をまとめる親オブジェクト

    float parentY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        SetSoldier setSoldier = GetComponent<SetSoldier>();//SetCardスクリプトを取得する

        if (setSoldier == null)
        {
            Debug.LogError("SetSoldierスクリプトが見つかりません。");
        }

        soldierAtk =setSoldier.SoldierAtk;//SetCardスクリプトから兵士の攻撃力を取得する

        _cardPos=setSoldier.transform.position;//カードの位置を取得する
        // textMesh = GetComponent<TextMeshProUGUI>();//TextMeshProUGUIコンポーネントを取得する



        _parent = new GameObject("Number");//テキストをまとめる親オブジェクトを生成する

        

        //if (IsShowText)
        OutPutText();//テキストに兵士の攻撃力を表示する関数

    }

    // Update is called once per frame
    void Update()
    {
        if (IsShowText)
        {
            _parent.SetActive(true);

            parentY = _rotateSpeed * Time.deltaTime;//回転速度をフレームレートに依存させないようにする
                                                    //10の位がある場合、ない場合で回転させるオブジェクトを変える

            _parent.transform.Rotate(0.0f, parentY, 0.0f);//親オブジェクトをカードと同じ向きにするために回転させる

            Vector3 pos = _cardPos;



            _parent.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
        else 
        {
            _parent.SetActive(false);
        }

    }


    void OutPutText() 
    {
        //textMesh.text = soldierAtk.ToString();//兵士の攻撃力をテキストに表示する

        int tenNum =soldierAtk/10;//10の位を取得する

        int oneNum = soldierAtk % 10;//1の位を取得する

        Vector3 pos = _cardPos;

        pos.y += 0.5f;//テキストをカードの上に表示するためにy座標を少し上げる

        Quaternion rot = Quaternion.Euler(-90, 0, 180);//テキストをカードと同じ向きにするために回転させる

        if (tenNum > 0)//10の位があるならば
        {


            _cardPos.y += 0.5f;//テキストをカードの上に表示するためにy座標を少し上げる

            _parent.transform.position = pos;//親オブジェクトの位置をテキストの位置にする


            pos.x -= 0.25f;//10の位のテキストを1の位の左側に表示するためにx座標を少し左にずらす
            Instantiate(_numberObj[tenNum],pos,rot,_parent.transform);//10の位のテキストを表示する

            pos.x += 0.5f;//1の位のテキストを10の位の右側に表示するためにx座標を少し右にずらす
            Instantiate(_numberObj[oneNum], pos, rot, _parent.transform);//1の位のテキストを表示する
        }
        else //10の位がないならば
        {
            _parent.transform.position = pos;

            Instantiate(_numberObj[oneNum], pos, rot, _parent.transform);
        }
    }

	private void OnDestroy()
	{
		Debug.Log("カードが破壊されました");

		Destroy(_parent);
	}
}
