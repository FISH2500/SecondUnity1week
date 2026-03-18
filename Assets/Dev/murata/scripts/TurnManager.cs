using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public int CurrentPlayer;

	public bool useItem = false;

	public void ChangeTurn()
	{
		CurrentPlayer ^= 1;
		useItem = false;
		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}

	public void SetTurn(int player)
	{
		CurrentPlayer = player;
		useItem = false;
		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}
}