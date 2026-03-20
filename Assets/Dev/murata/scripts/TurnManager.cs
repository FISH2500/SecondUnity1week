using UnityEngine;

public class TurnManager : MonoBehaviour
{
	[SerializeField]
	private CPUBase _cpuBase;

	public int CurrentPlayer;

	public bool UseItem = false;
	public bool IsAction = false;

<<<<<<< Updated upstream
=======
	public bool UnlimitedItem = false;
	public bool Revolution = false;
	public bool DoubleAttack = false;

	public bool IsDraw = false;

>>>>>>> Stashed changes
	public static TurnManager instance;

	private void Awake()
	{
		instance = this;

		CurrentPlayer = -1;
	}

	public void ChangeTurn()
	{
<<<<<<< Updated upstream
		CurrentPlayer ^= 1;
		UseItem = false;
		IsAction = false;
=======

        CurrentPlayer ^= 1;

        if (CurrentPlayer == 1)//自分のターンが来たらカウントする
        {
            TurnCount++;
            Debug.Log($"ターン数 {TurnCount}");
        }
		else
		{
			StartCoroutine(_cpuBase.SetAction());
		}

		UseItem = false;
		IsAction = false;
		UnlimitedItem = false;
		Revolution = false;
		DoubleAttack = false;
		IsDraw = false;
>>>>>>> Stashed changes

		DispUI.instance.Disp(CurrentPlayer == 0);

		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}

	public void SetTurn(int player)
	{
		CurrentPlayer = player;
		UseItem = false;
		IsAction = false;
<<<<<<< Updated upstream
=======
		UnlimitedItem = false;
		Revolution = false;
		DoubleAttack = false;
		IsDraw = false;

		if (CurrentPlayer == 1)
			StartCoroutine(_cpuBase.SetAction());
>>>>>>> Stashed changes

		DispUI.instance.Disp(CurrentPlayer == 0);

		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}
<<<<<<< Updated upstream
=======

	public void SetUnlimitedItem()
	{
		UnlimitedItem = true;

		UseItem = false;
	}

	public void UseItemFlag()
	{
		if (!UnlimitedItem) UseItem = true;
	}

	public void SetRevolution()
	{
		Revolution = true;
	}

	public void SetDoubleAttack()
	{
		DoubleAttack = true;
	}

	public bool DoubleAttackSimasuka()
	{
		bool tmp = DoubleAttack;

		DoubleAttack = false;

		return tmp;
	}
>>>>>>> Stashed changes
}