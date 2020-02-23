using UnityEngine;

public class WeaponDropPhysics : MonoBehaviour
{
	public Rigidbody WeaponObject;

	private Transform Player;

	private void Start()
	{
		Object.Destroy(base.gameObject, 10f);
		Player = GameManager.GM.Player.transform;
		WeaponObject.transform.LookAt(Player.position);
		WeaponObject.velocity = new Vector3(0f, 3f, 0f);
		WeaponObject.AddTorque(base.transform.right * 5000f);
		WeaponObject.AddTorque(base.transform.forward * -5000f);
		WeaponObject.AddForce(Player.transform.position - base.transform.position);
	}
}
