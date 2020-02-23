using UnityEngine;

public class WeaponOrb : MonoBehaviour
{
	private Transform Target;

	private int Item;

	private bool Ready;

	private bool CanPickUp = true;

	public void SetItem(Transform SetTarget, int SetItem)
	{
		Target = SetTarget;
		Item = SetItem;
		Ready = true;
	}

	private void Update()
	{
		if (Ready)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(Target.position.x, Target.position.y - 0.75f, Target.position.z), 0.25f);
			if (Vector3.Distance(base.transform.position, Target.position) <= 1f && CanPickUp)
			{
				GiveWeapon();
			}
		}
	}

	private void GiveWeapon()
	{
		if (CanPickUp)
		{
			GameManager.GM.Player.GetComponent<InventoryScript>().PickUpWeapon(Item, base.gameObject);
			CanPickUp = false;
		}
	}

	public void FailPickup()
	{
		CanPickUp = true;
		Object.Destroy(base.gameObject);
	}
}
