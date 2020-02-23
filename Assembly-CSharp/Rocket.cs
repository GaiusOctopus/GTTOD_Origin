using UnityEngine;

public class Rocket : MonoBehaviour
{
	public float Speed;

	public float DetonateDistance;

	public GameObject Explosion;

	public GameObject HailFireSpawn;

	private GameObject Weapon;

	private Transform Target;

	private bool canDetonate = true;

	private void Start()
	{
		Weapon = GameObject.FindGameObjectWithTag("RocketLauncher");
		Target = Weapon.GetComponent<WeaponScript>().AimFocus;
	}

	private void Update()
	{
		float num = Vector3.Distance(Target.position, base.transform.position);
		base.transform.position = Vector3.MoveTowards(base.transform.position, Target.position, Speed * Time.deltaTime);
		base.gameObject.transform.LookAt(Target);
		if (num <= DetonateDistance && canDetonate)
		{
			Detonate();
			canDetonate = false;
		}
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			Hailfire();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag != "Player")
		{
			Detonate();
		}
	}

	private void Detonate()
	{
		Object.Instantiate(Explosion, base.transform.position, base.transform.rotation);
		Object.Destroy(base.gameObject);
	}

	private void Hailfire()
	{
		Object.Instantiate(HailFireSpawn, base.transform.position, Quaternion.Euler(0f, 0f, 0f));
		Detonate();
	}
}
