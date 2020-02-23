using System.Collections;
using UnityEngine;

public class Frisbee : MonoBehaviour
{
	private Transform Player;

	private Rigidbody Physics;

	private bool returning;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
		Physics = GetComponent<Rigidbody>();
		StartCoroutine(TimeToReturn());
	}

	private void Update()
	{
		if (returning)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, new Vector3(Player.position.x, Player.position.y - 0.5f, Player.position.z), 0.35f);
			base.transform.LookAt(Player);
			if (Vector3.Distance(base.transform.position, Player.position) <= 2f)
			{
				GameManager.GM.Player.GetComponent<InventoryScript>().RefreshEquipmentCooldown();
				Object.Destroy(base.gameObject);
			}
		}
	}

	private IEnumerator TimeToReturn()
	{
		yield return new WaitForSeconds(2f);
		Return();
	}

	public void Return()
	{
		returning = true;
		Physics.velocity = Vector3.zero;
		GetComponent<SphereCollider>().enabled = false;
	}
}
