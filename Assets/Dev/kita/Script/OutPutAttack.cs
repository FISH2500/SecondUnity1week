using TMPro;
using UnityEngine;

public class OutPutAttack : MonoBehaviour
{
    int soldierAtk;

    Vector3 cardPos;

    [SerializeField]
    GameObject[] numberObj=new GameObject[10];//3Dテキストを格納する配列

    [SerializeField]
    float rotateSpeed;//回転速度

    GameObject parent;//10,1の位をまとめる親オブジェクト

    float y;

    Vector3 textPos;//テキストの位置
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        SetSoldier setSoldier = GetComponent<SetSoldier>();//SetCardスクリプトを取得する

        if (setSoldier == null)
        {
            Debug.LogError("SetSoldierスクリプトが見つかりません。");
        }

        soldierAtk =setSoldier.SoldierAtk;//SetCardスクリプトから兵士の攻撃力を取得する

        cardPos=setSoldier.transform.position;//カードの位置を取得する
        // textMesh = GetComponent<TextMeshProUGUI>();//TextMeshProUGUIコンポーネントを取得する

        cardPos.y += 0.5f;//テキストをカードの上に表示するためにy座標を少し上げる

        textPos = cardPos;


        OutPutText();//テキストに兵士の攻撃力を表示する関数

    }

    // Update is called once per frame
    void Update()
    {
        
        y = rotateSpeed*Time.deltaTime;//回転速度をフレームレートに依存させないようにする
        //10の位がある場合、ない場合で回転させるオブジェクトを変える

        parent.transform.Rotate(0.0f,y,0.0f);//親オブジェクトをカードと同じ向きにするために回転させる

        Vector3 pos = cardPos;

        

        parent.transform.position=new Vector3(transform.position.x,transform.position.y+0.5f,transform.position.z);


    }


    void OutPutText() 
    {
        //textMesh.text = soldierAtk.ToString();//兵士の攻撃力をテキストに表示する

        int tenNum =soldierAtk/10;//10の位を取得する

        int oneNum = soldierAtk % 10;//1の位を取得する

        Vector3 pos = cardPos;

        pos.y += 0.5f;//テキストをカードの上に表示するためにy座標を少し上げる

        Quaternion rot = Quaternion.Euler(-90, 0, 180);//テキストをカードと同じ向きにするために回転させる

        if (tenNum > 0)//10の位があるならば
        {

            parent=new GameObject("Number");//テキストをまとめる親オブジェクトを生成する

            parent.transform.position = pos;//親オブジェクトの位置をテキストの位置にする

            pos.x -= 0.25f;//10の位のテキストを1の位の左側に表示するためにx座標を少し左にずらす
            Instantiate(numberObj[tenNum],pos,rot,parent.transform);//10の位のテキストを表示する

            pos.x += 0.5f;//1の位のテキストを10の位の右側に表示するためにx座標を少し右にずらす
            Instantiate(numberObj[oneNum], pos, rot, parent.transform);//1の位のテキストを表示する
        }
        else //10の位がないならば
        {
            parent = new GameObject("Number");
            parent.transform.position = pos;

            Instantiate(numberObj[oneNum], pos, rot, parent.transform);
        }

        

    }

}
