using System.Collections;
using UnityEngine;

public class DashReset : MonoBehaviour
{
	public GameObject ActiveEffect;

	public GameObject SpawnEffect;

	private Rigidbody PlayerPhysics;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<ac_CharacterController>().ResetAbilities();
			if (other.GetComponent<ac_CharacterController>().isFalling)
			{
				PlayerPhysics = other.GetComponent<Rigidbody>();
				PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x, 10f, PlayerPhysics.velocity.z);
			}
			StartCoroutine(Reset());
		}
	}

	private IEnumerator Reset()
	{
		base.gameObject.GetComponent<SphereCollider>().enabled = false;
		ActiveEffect.SetActive(value: false);
		Object.Instantiate(SpawnEffect, base.transform.position, SpawnEffect.transform.rotation);
		yield return new WaitForSeconds(5f);
		base.gameObject.GetComponent<SphereCollider>().enabled = true;
		ActiveEffect.SetActive(value: true);
	}
}
