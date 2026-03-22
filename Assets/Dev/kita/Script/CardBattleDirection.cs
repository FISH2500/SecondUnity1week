using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardBattleDirection : MonoBehaviour
{
    [SerializeField] private float _atkPosXOffset = 1.5f; // 攻撃側の位置を守備側からどれだけ左にずらすか

    [SerializeField] private float _atkPosYOffset = 1.5f;//攻撃側の位置を守備側からどれだけ上にずらすか

    [SerializeField] private Image _fadeOutImage;//フェードアウトさせる画像

    [SerializeField] private float _outSetTime;//フェードアウトがセットされるまで

    [SerializeField] private float _fadeOutSpeed;

    [SerializeField] private float _fadeInSpeed;

    [SerializeField] private BattleManegar _battleManegar;

    bool _fadeOut = false;//フェードアウトのフラグ

    bool _fadeIn = false;//フェードインのフラグ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_fadeOut) 
        {
            StartCoroutine(SetFadeOut());
        }
        else if (_fadeIn) 
        {

            StartCoroutine(SetFadeIn());
        }
    }

    //攻撃演出をする際のオブジェクトの位置をセット
    public void SetBattleDirection(GameObject attacker,GameObject defender)//攻撃者と防御者の位置を受け取って、攻撃演出をする際のオブジェクトをセットする 
    {
        Vector3 defPos=defender.transform.position;//守備側の位置を取得

        defPos.x -= _atkPosXOffset;//守備側の少し左の位置を取得

        defPos.y += _atkPosYOffset;//守備側の少し左の位置を取得

        Vector3 atkPos=defPos;//攻撃側の位置を取得

        attacker.transform.position=atkPos;//攻撃側の位置をセット

        defender.GetComponent<SetSoldier>().RotateSetFront();//守備側のカードを表にする

        //敗北カード位置にヒットエフェクトを出す


        //暗転させる
        _fadeOut = true;

        //暗転している間にカードを壊れた状態に
        StartCoroutine(SetBreakSprite());
        //オブジェクトを削除

        //Destroy(defeatCard,10.0f);

    }

    private IEnumerator SetFadeOut()
    {
        yield return new WaitForSeconds(_outSetTime);

        Color c = _fadeOutImage.color;

        while (c.a < 1.0f)
        {
            c.a += _fadeOutSpeed * Time.deltaTime;
            c.a = Mathf.Clamp01(c.a);
            _fadeOutImage.color = c;

            yield return null;
        }

        _fadeOut = false;
        _fadeIn = true;

    }

    private IEnumerator SetFadeIn()
    {
        yield return new WaitForSeconds(0.5f);

        Color c = _fadeOutImage.color;

        while (c.a > 0.0f)
        {
            c.a -= _fadeInSpeed * Time.deltaTime;
            c.a = Mathf.Clamp01(c.a);
            _fadeOutImage.color = c;

            yield return null;
        }

        _fadeIn = false;
    }

    private IEnumerator SetBreakSprite() 
    {

        yield return new WaitForSeconds(3.5f);

        GameObject defeatCard = _battleManegar.DefeatCrad;

        if (defeatCard != null ) 
        defeatCard.GetComponent<SetSoldier>().SetBreakSprite();
    }

}
