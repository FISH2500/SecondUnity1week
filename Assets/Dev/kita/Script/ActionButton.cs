using UnityEngine;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour
{

    [SerializeField] public GameObject[] _unSetButton;//アクションボタンを押したときに非表示にしたいボタンを格納する配列 
    [SerializeField] public GameObject[] _setButton;//アクションボタンを押したときに表示したいボタンを格納する配列

    public void ActionButtonDown()//このスクリプトがついているボタンのアクションをしたとき
    {
        for(int i = 0; i < _unSetButton.Length; i++) 
        {
            _unSetButton[i].SetActive(false);//配列に格納されているボタンを全て非表示にする
        }
        for (int i = 0; i < _setButton.Length; i++)
        {
            _setButton[i].SetActive(true);//配列に格納されているボタンを全て非表示にする
        }

    }


}
