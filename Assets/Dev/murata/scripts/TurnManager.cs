using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public int CurrentPlayer;
	public int TurnCount = 1;//ターン経過数

    public bool UseItem = false;
	public bool IsAction = false;

	public static TurnManager instance;

	private void Awake()
	{
		instance = this;

		CurrentPlayer = -1;
	}

	public void ChangeTurn()
	{

        CurrentPlayer ^= 1;

        if (CurrentPlayer == 1)//自分のターンが来たらカウントする
        {
            TurnCount++;
            Debug.Log($"ターン数 {TurnCount}");
            
        }
        UseItem = false;
		IsAction = false;

		DispUI.instance.Disp(CurrentPlayer == 0);

		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}

	public void SetTurn(int player)
	{
		CurrentPlayer = player;
		UseItem = false;
		IsAction = false;

		DispUI.instance.Disp(CurrentPlayer == 0);

		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}
}