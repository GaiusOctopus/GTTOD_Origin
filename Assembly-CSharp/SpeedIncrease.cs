using System.Collections;
using UnityEngine;

public class SpeedIncrease : MonoBehaviour
{
	public float SpeedMultiplier;

	public float TimeInIncrease;

	private float SpeedSave;

	private float MeleeSave;

	private float DashSave;

	private ac_CharacterController CharacterController;

	private InventoryScript Inventory;

	private void Start()
	{
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Inventory = GameManager.GM.Player.GetComponent<InventoryScript>();
		SpeedSave = CharacterController.Speed;
		MeleeSave = Inventory.MeleeSpeed;
		CharacterController.Speed *= SpeedMultiplier;
		Inventory.MeleeSpeed /= SpeedMultiplier;
		StartCoroutine(SpeedTimer());
	}

	private IEnumerator SpeedTimer()
	{
		yield return new WaitForSeconds(TimeInIncrease);
		CharacterController.Speed = SpeedSave;
		Inventory.MeleeSpeed = MeleeSave;
		Object.Destroy(base.gameObject);
	}
}
