using UnityEngine;

public class Effect : MonoBehaviour
{
	private float _speed;
	private float _limitY;
	private float _sinOffset;
	private float _rotationSpeed;
	private RectTransform _rt;

	public void Setup(float speed, float canvasHeight)
	{
		_speed = speed;
		_limitY = -canvasHeight / 2 - 100f; // 画面下に消えるライン
		_rt = GetComponent<RectTransform>();
		_sinOffset = Random.Range(0, 100f); // 左右のゆらゆら開始位置をバラバラにする
		_rotationSpeed = Random.Range(50f, 200f); // 回転速度
	}

	void Update()
	{
		// に落下
		Vector2 pos = _rt.anchoredPosition;
		pos.y -= _speed * Time.deltaTime;

		// 左右にゆらゆら
		pos.x += Mathf.Sin(Time.time * 2f + _sinOffset) * 0.5f;

		_rt.anchoredPosition = pos;

		// 回転
		_rt.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
		_rt.Rotate(Vector3.forward * (_rotationSpeed / 2f) * Time.deltaTime);

		// 画面外に出たら削除
		if (pos.y < _limitY)
		{
			Destroy(gameObject);
		}
	}
}