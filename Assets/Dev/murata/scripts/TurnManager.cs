using UnityEngine;

public class TurnManager : MonoBehaviour
{
	[SerializeField]
	private CPUBase _cpuBase;

	public int CurrentPlayer;
	public int TurnCount = 1;//ターン経過数

    public bool UseItem = false;
	public bool IsAction = false;

	public bool UnlimitedItem = false;
	public bool Revolution = false;
	public bool DoubleAttack = false;

	public bool IsDraw = false;

	public bool UnlimitedItem = false;

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

        CurrentPlayer ^= 1;

        if (CurrentPlayer == 1)//自分のターンが来たらカウントする
        {
            TurnCount++;
            Debug.Log($"ターン数 {TurnCount}");
            
        }
        UseItem = false;
		IsAction = false;
		UnlimitedItem = false;

		DispUI.instance.Disp(CurrentPlayer == 0);

		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}

	public void SetTurn(int player)
	{
		CurrentPlayer = player;
		UseItem = false;
		IsAction = false;
		UnlimitedItem = false;
		Revolution = false;
		DoubleAttack = false;
		IsDraw = false;

		if (CurrentPlayer == 1)
			StartCoroutine(_cpuBase.SetAction());

		DispUI.instance.Disp(CurrentPlayer == 0);

		Debug.Log($"プレイヤー{CurrentPlayer} のターン");
	}

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
}