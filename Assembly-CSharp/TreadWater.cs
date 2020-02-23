using UnityEngine;

public class TreadWater : MonoBehaviour
{
	public InventoryScript Inventory;

	public void Tread()
	{
		Inventory.Tread();
	}

	public void FinishTread()
	{
		Inventory.EndTread();
	}
}
