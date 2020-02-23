using EZCameraShake;
using System.Collections;
using UnityEngine;

public class Katana : MonoBehaviour
{
	[Header("Sword Set-Up")]
	public Transform Camera;

	public Rigidbody Player;

	public GameObject SwingObject;

	public GameObject DashObject;

	private Transform MyLastSwing;

	private Animator Anim;

	public int SwingNumber;

	private bool canSwing = true;

	private bool canDash = true;

	private float SwingTime = 1f;

	private WeaponScript MyWeapon;

	private AudioSource Audio;

	private void OnEnable()
	{
		Anim = GetComponent<Animator>();
		MyWeapon = GetComponent<WeaponScript>();
		canSwing = true;
		SwingNumber = 0;
		SwingTime = 0f;
		Audio = GetComponent<AudioSource>();
		canDash = true;
	}

	private void Update()
	{
		if ((Input.GetKey(KeyCode.Mouse0) && canSwing) || (Input.GetAxis("RightTrigger") == 1f && canSwing))
		{
			SwingNumber++;
			if (SwingNumber == 1)
			{
				Anim.SetTrigger("Swing1");
				SwingTime = 1f;
				StartCoroutine(SwingCooldown(0.275f));
			}
			if (SwingNumber == 2)
			{
				Anim.SetTrigger("Swing2");
				SwingTime = 1f;
				StartCoroutine(SwingCooldown(0.275f));
			}
			if (SwingNumber == 3)
			{
				Anim.SetTrigger("Swing3");
				SwingTime = 1f;
				StartCoroutine(SwingCooldown(0.275f));
			}
		}
		if (SwingTime > 0f)
		{
			SwingTime -= Time.deltaTime;
		}
		else
		{
			Anim.SetTrigger("Return");
			SwingTime = 0f;
			SwingNumber = 0;
		}
		if ((Input.GetKey(KeyCode.Mouse1) && canDash) || (Input.GetAxis("RightBumper") == 1f && canDash))
		{
			canDash = false;
			StartCoroutine(Dash());
		}
		if (canDash)
		{
			MyWeapon.PrimaryOn = true;
		}
		else
		{
			MyWeapon.PrimaryOn = false;
		}
	}

	private IEnumerator Dash()
	{
		Anim.SetTrigger("Swing3");
		SwingTime = 1f;
		Audio.Play();
		yield return new WaitForSeconds(0.15f);
		Object.Instantiate(DashObject, Camera.transform.position, Camera.transform.rotation);
		MyLastSwing = Object.Instantiate(SwingObject, Camera.transform.position, Camera.transform.rotation).transform;
		MyLastSwing.parent = Camera;
		yield return new WaitForSeconds(5f);
		canDash = true;
	}

	private IEnumerator SwingCooldown(float CooldownTime)
	{
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.15f, 0.075f, 0.15f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
		CameraShaker.Instance.ShakeOnce(10f, 1f, 0.35f, 1.5f);
		Player.AddForce(Player.transform.forward * 150f);
		Player.AddForce(Player.transform.up * 50f);
		if (SwingNumber >= 3)
		{
			SwingNumber = 1;
		}
		canSwing = false;
		yield return new WaitForSeconds(0.3f);
		Player.AddForce(Player.transform.forward * 100f);
		MyLastSwing = Object.Instantiate(SwingObject, Camera.transform.position, Camera.transform.rotation).transform;
		MyLastSwing.parent = Camera;
		CameraShaker.Instance.ShakeOnce(10f, 1.5f, 0.25f, 2f);
		yield return new WaitForSeconds(CooldownTime);
		canSwing = true;
		CameraShaker.Instance.ResetCamera();
	}
}
