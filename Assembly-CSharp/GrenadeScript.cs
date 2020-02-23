using System.Collections;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
	public Rigidbody Grenade;

	public GameObject Explosion;

	public bool InputBomb;

	public bool TimeBomb = true;

	[ConditionalField("TimeBomb", null)]
	public float ExploadTime;

	private bool Frozen;

	private float TimeToInput = 0.25f;

	private Vector3 VelocitySave;

	private void Start()
	{
		Grenade.velocity = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity;
		if (TimeBomb)
		{
			StartCoroutine(Expload());
		}
	}

	private void Update()
	{
		TimeToInput -= Time.deltaTime;
		if (InputBomb && TimeToInput <= 0f && (KeyBindingManager.GetKey(KeyAction.SwitchFireMode) || Input.GetButton("RightBumper")))
		{
			ExploadGrenade();
		}
	}

	private IEnumerator Expload()
	{
		yield return new WaitForSeconds(ExploadTime);
		if (!Frozen)
		{
			ExploadGrenade();
		}
	}

	public void ExploadGrenade()
	{
		if (!Frozen)
		{
			Object.Instantiate(Explosion, Grenade.transform.position, Grenade.transform.rotation);
			Object.Destroy(Grenade.gameObject);
			Object.Destroy(base.gameObject);
		}
	}

	public void Freeze()
	{
		Frozen = true;
		StopCoroutine(Expload());
		VelocitySave = Grenade.velocity;
		Grenade.isKinematic = true;
	}

	public void UnFreeze()
	{
		Frozen = false;
		if (TimeBomb)
		{
			StartCoroutine(Expload());
		}
		Grenade.isKinematic = false;
		Grenade.velocity = VelocitySave;
	}
}
