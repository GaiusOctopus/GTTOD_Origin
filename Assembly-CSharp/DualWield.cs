using UnityEngine;

public class DualWield : MonoBehaviour
{
	private void Start()
	{
		GameManager.GM.Player.GetComponent<InventoryScript>().TriggerDualWield();
		Object.Destroy(base.gameObject);
	}
}
