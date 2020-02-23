using UnityEngine;

public class TimeStopObject : MonoBehaviour
{
	[Header("Object Type")]
	public bool IsBullet;

	public bool IsRicochetRound;

	public bool IsGrenade;

	public bool IsInfantry;

	public bool IsSwordsman;

	public bool IsPhysicsObject;

	public bool IsDrone;

	public bool IsTurretCrab;

	private GameManager GM;

	private InventoryScript Inventory;

	private void Start()
	{
		GM = GameManager.GM;
		Inventory = GM.Player.GetComponent<InventoryScript>();
		if (IsInfantry || IsSwordsman || IsDrone || IsTurretCrab || IsPhysicsObject)
		{
			GM.Objects.Add(this);
		}
		else if (Inventory.EquipmentID == 8 && GM.TimeStopped)
		{
			GM.Objects.Add(this);
		}
		if (GM.TimeStopped)
		{
			Freeze();
		}
	}

	private void OnDestroy()
	{
		if (GM != null && GM.Objects.Contains(this))
		{
			GM.Objects.RemoveAt(GameManager.GM.Objects.IndexOf(this));
		}
	}

	public void Freeze()
	{
		if (IsBullet)
		{
			base.gameObject.GetComponent<BulletScript>().Freeze();
		}
		if (IsRicochetRound)
		{
			base.gameObject.GetComponent<RicochetRound>().Freeze();
		}
		if (IsGrenade)
		{
			base.gameObject.GetComponent<GrenadeScript>().Freeze();
		}
		if (IsInfantry)
		{
			base.gameObject.GetComponent<Infantry>().Freeze();
		}
		if (IsSwordsman)
		{
			base.gameObject.GetComponent<Swordsman>().Freeze();
		}
		if (IsPhysicsObject)
		{
			base.gameObject.GetComponent<RigidbodyObject>().Freeze();
		}
		if (IsDrone)
		{
			base.gameObject.GetComponent<Drone>().Freeze();
		}
		if (IsTurretCrab)
		{
			base.gameObject.GetComponent<TurretCrab>().Freeze();
		}
	}

	public void UnFreeze()
	{
		if (IsBullet)
		{
			base.gameObject.GetComponent<BulletScript>().UnFreeze();
		}
		if (IsRicochetRound)
		{
			base.gameObject.GetComponent<RicochetRound>().UnFreeze();
		}
		if (IsGrenade)
		{
			base.gameObject.GetComponent<GrenadeScript>().UnFreeze();
		}
		if (IsInfantry)
		{
			base.gameObject.GetComponent<Infantry>().UnFreeze();
		}
		if (IsSwordsman)
		{
			base.gameObject.GetComponent<Swordsman>().UnFreeze();
		}
		if (IsPhysicsObject)
		{
			base.gameObject.GetComponent<RigidbodyObject>().UnFreeze();
		}
		if (IsDrone)
		{
			base.gameObject.GetComponent<Drone>().UnFreeze();
		}
		if (IsTurretCrab)
		{
			base.gameObject.GetComponent<TurretCrab>().UnFreeze();
		}
	}
}
