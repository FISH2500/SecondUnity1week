using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardBattleDirection : MonoBehaviour
{
    [SerializeField] private float _atkPosXOffset = 1.5f; // 攻撃側の位置を守備側からどれだけ左にずらすか

    [SerializeField] private float _atkPosYOffset = 1.5f;//攻撃側の位置を守備側からどれだけ上にずらすか

    [SerializeField] private Image _fadeOutImage;//フェードアウトさせる画像

	[SerializeField] private BattleManegar _battleManegar;

	[Header("時間設定")]
	[SerializeField] private float _outSetTime = 1.0f;     // 開始までの待ち時間
	[SerializeField] private float _fadeOutDuration = 0.5f; // 何秒かけて暗くするか
	[SerializeField] private float _blackoutHoldTime = 0.2f; // 真っ暗なまま維持する時間
	[SerializeField] private float _fadeInDuration = 0.5f;  // 何秒かけて明るくするか


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
		StartCoroutine(PlayBattleSequence());

		
	}

	private IEnumerator PlayBattleSequence()
	{
		yield return new WaitForSeconds(_outSetTime);

		yield return StartCoroutine(FadeRoutine(0f, 1f, _fadeOutDuration));


		GameObject defeatCard = _battleManegar.DefeatCrad;
		if (defeatCard != null)
		{
			defeatCard.GetComponent<SetSoldier>().SetBreakSprite();
		}

		SoundManager.Instance.PlaySE("tear");

		yield return new WaitForSeconds(_blackoutHoldTime);

		yield return StartCoroutine(FadeRoutine(1f, 0f, _fadeInDuration));

		if (BattleManegar.Result == BattleManegar.BattleResult.Win)
		{
			SoundManager.Instance.PlaySE("battleWin");
		}
		else
		{
			SoundManager.Instance.PlaySE("battleLose");
		}
	}

	private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
	{
		float elapsed = 0f;
		Color c = _fadeOutImage.color;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / duration);

			c.a = Mathf.Lerp(startAlpha, endAlpha, t);
			_fadeOutImage.color = c;

			yield return null;
		}

		c.a = endAlpha;
		_fadeOutImage.color = c;
	}
}
