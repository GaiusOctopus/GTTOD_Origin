using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public LayerMask AvailableLayers;

	public float Damage;

	public Transform Emitter;

	public Transform EndPoint;

	private LineRenderer LR;

	private AudioSource Audio;

	private RaycastHit LaserCast;

	private RaycastHit DamageCast;

	private bool canDamage = true;

	private void Start()
	{
		LR = GetComponent<LineRenderer>();
		Audio = GetComponent<AudioSource>();
	}

	private void Update()
	{
		LR.SetPosition(0, Emitter.position);
		LR.SetPosition(1, EndPoint.position);
		if (Physics.Raycast(Emitter.transform.position, Emitter.transform.forward, out LaserCast, 100000f, AvailableLayers))
		{
			EndPoint.position = LaserCast.point;
		}
		else
		{
			EndPoint.position = base.transform.forward * 1000f;
		}
		if (Physics.Raycast(Emitter.transform.position, Emitter.transform.forward, out DamageCast, 100000f, AvailableLayers) && DamageCast.collider.tag == "Player" && canDamage)
		{
			canDamage = false;
			DamageCast.collider.gameObject.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
			Audio.Play();
			StartCoroutine(DamageCooldown());
		}
	}

	public IEnumerator DamageCooldown()
	{
		canDamage = false;
		yield return new WaitForSeconds(0.5f);
		canDamage = true;
	}
}
