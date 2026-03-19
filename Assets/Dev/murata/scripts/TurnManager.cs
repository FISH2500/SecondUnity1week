using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public int CurrentPlayer;

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