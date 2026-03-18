using UnityEngine;

public enum TurnState
{
	Player1Turn,
	Player2Turn,
}

public class TurnManager : MonoBehaviour
{
	public static int CurrentTurn;

	public bool useItem = false;

	public void TurnChange()
	{
		CurrentTurn ^= 1;
		useItem = false;
		Debug.Log($"プレイヤー{CurrentTurn} のターン");
	}
}