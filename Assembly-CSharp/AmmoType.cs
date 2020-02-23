using System;

[Serializable]
public class AmmoType
{
	public string AmmoName = "AmmoName";

	public int AmmoAmount = 15;

	public int MaxAmmoLimit = 15;

	private int StartingAmmo = 15;

	public void AddAmmo(int Amount)
	{
		AmmoAmount += Amount;
		if (AmmoAmount > MaxAmmoLimit)
		{
			AmmoAmount = MaxAmmoLimit;
		}
	}

	public void SetStartingAmmo()
	{
		StartingAmmo = AmmoAmount;
	}

	public void ResetAmmo()
	{
		AmmoAmount = StartingAmmo;
	}
}
