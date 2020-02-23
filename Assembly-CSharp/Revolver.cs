using EZCameraShake;
using System.Collections;
using UnityEngine;

public class Revolver : MonoBehaviour
{
	[Header("Upgrades")]
	public bool StandardAction = true;

	public bool StandardReload = true;

	[Header("Settings Stuff")]
	public KeyCode primary = KeyCode.Mouse0;

	public KeyCode secondary = KeyCode.Mouse1;

	public bool canReload = true;

	public Transform MainPosition;

	public Transform ZoomPosition;

	public Transform CrouchPosition;

	public float ZoomTime = 1f;

	private float MainFOV = 90f;

	private float zoomFOV = 60f;

	private float normalFOV = 90f;

	public float thrust = 90f;

	public float RateOfFire = 0.1f;

	public float ReloadDuration = 1.5f;

	public float VaultDuration = 1.5f;

	public float ShellDuration;

	[Header("Audio")]
	public AudioClip WieldSFX;

	public AudioClip FireSFX;

	public AudioClip ReloadSFX;

	public AudioClip OpenChamberSFX;

	public AudioClip InsertShellSFX;

	public AudioClip CloseChamberSFX;

	[Header("Shells")]
	public GameObject Shell1;

	public GameObject Shell2;

	public GameObject Shell3;

	public GameObject Shell4;

	public GameObject Shell5;

	public GameObject Shell6;

	private bool CanDumpShell1 = true;

	private bool CanDumpShell2 = true;

	private bool CanDumpShell3 = true;

	private bool CanDumpShell4 = true;

	private bool CanDumpShell5 = true;

	private bool CanDumpShell6 = true;

	[Header("Camera Shake Stuff")]
	public float Magnitude = 2f;

	public float Roughness = 10f;

	public float FadeOutTime = 5f;

	public WeaponSway Base;

	public Animator WeaponParent;

	[Header("Ammo Stuff")]
	public int currentAmmo;

	public int ammo;

	[Header("Prefab Stuff")]
	public GameObject SoundPrefab;

	public GameObject Bullet;

	public GameObject Casing;

	[Header("Spawnpoint Stuff")]
	public Transform bulletSpawnPoint;

	private AudioSource Audio;

	private Animation LegacyAnim;

	private bool outOfAmmo;

	private bool isCrouching;

	private bool isVaulting;

	private bool isReloading;

	private bool canFire = true;

	private bool Full;

	private bool isSlowReloading;

	private bool isHolstered;

	private bool canHolster = true;

	private bool ZoomingIn;

	private void Awake()
	{
		canFire = true;
		isVaulting = false;
		isReloading = false;
		isSlowReloading = false;
		Audio = base.gameObject.GetComponent<AudioSource>();
		Audio.clip = WieldSFX;
		LegacyAnim = GetComponent<Animation>();
		currentAmmo = ammo;
		Audio.Play();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse1))
		{
			ZoomingIn = true;
		}
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			ZoomingIn = false;
		}
		if (ZoomingIn)
		{
			ZoomIn();
		}
		else if (!isCrouching)
		{
			ReturnPosition();
		}
		if (currentAmmo == ammo)
		{
			Full = true;
		}
		else
		{
			Full = false;
		}
		if (Input.GetKeyDown(primary) && canFire && !outOfAmmo && !isReloading && !isVaulting)
		{
			StartCoroutine("Shoot");
		}
		if (Input.GetKeyDown(KeyCode.R) && canReload)
		{
			StartCoroutine("Reload");
		}
		if (currentAmmo == 0)
		{
			outOfAmmo = true;
		}
		else if (currentAmmo > 0)
		{
			outOfAmmo = false;
		}
		if (isSlowReloading)
		{
			if (Input.GetKeyDown(primary))
			{
				StartCoroutine("InsertBullet");
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				StartCoroutine("CancelSlowReload");
			}
		}
	}

	private IEnumerator Shoot()
	{
		if (!StandardAction)
		{
			canFire = false;
			LegacyAnim.Stop();
			LegacyAnim.Play("Fire");
			currentAmmo--;
			Object.Instantiate(Bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
			CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0f, FadeOutTime);
			StartCoroutine("FireSound");
			yield return new WaitForSeconds(RateOfFire);
			canFire = true;
		}
		if (StandardAction)
		{
			Audio.clip = FireSFX;
			Audio.Play();
			canFire = false;
			LegacyAnim.Stop();
			LegacyAnim.Play("SingleFire");
			currentAmmo--;
			Object.Instantiate(Bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
			CameraShaker.Instance.ShakeOnce(Magnitude, Roughness, 0f, FadeOutTime);
			StartCoroutine("FireSound");
			yield return new WaitForSeconds(0.75f);
			canFire = true;
		}
	}

	private IEnumerator Reload()
	{
		if (!StandardReload && !Full && !isReloading && canFire)
		{
			StartCoroutine("DumpShells");
			Audio.clip = ReloadSFX;
			Audio.Play();
			isReloading = true;
			canFire = false;
			LegacyAnim.Stop();
			LegacyAnim.Play("SpeedReload");
			StartCoroutine("CameraShake");
			yield return new WaitForSeconds(ReloadDuration);
			isReloading = false;
			canFire = true;
			currentAmmo = 6;
			Shell1.SetActive(value: true);
			Shell2.SetActive(value: true);
			Shell3.SetActive(value: true);
			Shell4.SetActive(value: true);
			Shell5.SetActive(value: true);
			Shell6.SetActive(value: true);
			CanDumpShell1 = true;
			CanDumpShell2 = true;
			CanDumpShell3 = true;
			CanDumpShell4 = true;
			CanDumpShell5 = true;
			CanDumpShell6 = true;
		}
		if (StandardReload && !Full && !isReloading && canFire)
		{
			StartCoroutine("DumpShells");
			currentAmmo = 0;
			Audio.clip = OpenChamberSFX;
			Audio.Play();
			isReloading = true;
			canFire = false;
			LegacyAnim.Stop();
			LegacyAnim.Play("OpenChamber");
			StartCoroutine("CameraShake");
			yield return new WaitForSeconds(1.5f);
			isSlowReloading = true;
		}
	}

	private IEnumerator HolsterWeapon()
	{
		canHolster = false;
		canFire = false;
		LegacyAnim.Stop();
		LegacyAnim.Play("HolsterWeapon");
		yield return new WaitForSeconds(1f);
		isHolstered = true;
		canHolster = true;
	}

	private IEnumerator UnholsterWeapon()
	{
		canHolster = false;
		canFire = false;
		LegacyAnim.Stop();
		LegacyAnim.Play("Wield");
		yield return new WaitForSeconds(1f);
		canFire = true;
		isHolstered = false;
		canHolster = true;
	}

	private IEnumerator DumpShells()
	{
		yield return new WaitForSeconds(1f);
		if (CanDumpShell1)
		{
			Shell1.SetActive(value: false);
			Object.Instantiate(Casing, Shell1.transform.position, Shell1.transform.rotation);
			CanDumpShell1 = false;
		}
		if (CanDumpShell2)
		{
			Shell2.SetActive(value: false);
			Object.Instantiate(Casing, Shell2.transform.position, Shell2.transform.rotation);
			CanDumpShell2 = false;
		}
		if (CanDumpShell3)
		{
			Shell3.SetActive(value: false);
			Object.Instantiate(Casing, Shell3.transform.position, Shell3.transform.rotation);
			CanDumpShell3 = false;
		}
		if (CanDumpShell4)
		{
			Shell4.SetActive(value: false);
			Object.Instantiate(Casing, Shell4.transform.position, Shell4.transform.rotation);
			CanDumpShell4 = false;
		}
		if (CanDumpShell5)
		{
			Shell5.SetActive(value: false);
			Object.Instantiate(Casing, Shell5.transform.position, Shell5.transform.rotation);
			CanDumpShell5 = false;
		}
		if (CanDumpShell6)
		{
			Shell6.SetActive(value: false);
			Object.Instantiate(Casing, Shell6.transform.position, Shell6.transform.rotation);
			CanDumpShell6 = false;
		}
	}

	private IEnumerator InsertBullet()
	{
		if (currentAmmo < 6)
		{
			LegacyAnim.Stop();
			LegacyAnim.Play("InsertRound");
			isSlowReloading = false;
			CameraShaker.Instance.ShakeOnce(1f, 1f, 0.2f, 2f);
			Audio.clip = InsertShellSFX;
			Audio.Play();
			currentAmmo++;
			yield return new WaitForSeconds(0.3f);
			isSlowReloading = true;
			EnableRound();
		}
		if (currentAmmo == 6)
		{
			isSlowReloading = false;
			Audio.clip = CloseChamberSFX;
			Audio.Play();
			LegacyAnim.Stop();
			LegacyAnim.Play("CloseChamber");
			CameraShaker.Instance.ShakeOnce(1f, 2f, 0.4f, 2f);
			yield return new WaitForSeconds(0.35f);
			CameraShaker.Instance.ShakeOnce(1f, 2f, 0.4f, 2f);
			yield return new WaitForSeconds(0.5f);
			isReloading = false;
			canFire = true;
			currentAmmo = 6;
		}
	}

	private IEnumerator CancelSlowReload()
	{
		isSlowReloading = false;
		Audio.clip = CloseChamberSFX;
		Audio.Play();
		LegacyAnim.Stop();
		LegacyAnim.Play("CloseChamber");
		CameraShaker.Instance.ShakeOnce(1f, 2f, 0.4f, 2f);
		yield return new WaitForSeconds(0.35f);
		CameraShaker.Instance.ShakeOnce(1f, 2f, 0.4f, 2f);
		yield return new WaitForSeconds(1.25f);
		isReloading = false;
		canFire = true;
	}

	private void EnableRound()
	{
		if (currentAmmo == 1)
		{
			Shell1.SetActive(value: true);
			CanDumpShell1 = true;
		}
		if (currentAmmo == 2)
		{
			Shell2.SetActive(value: true);
			CanDumpShell2 = true;
		}
		if (currentAmmo == 3)
		{
			Shell3.SetActive(value: true);
			CanDumpShell3 = true;
		}
		if (currentAmmo == 4)
		{
			Shell4.SetActive(value: true);
			CanDumpShell4 = true;
		}
		if (currentAmmo == 5)
		{
			Shell5.SetActive(value: true);
			CanDumpShell5 = true;
		}
		if (currentAmmo == 6)
		{
			Shell6.SetActive(value: true);
			CanDumpShell6 = true;
		}
	}

	private IEnumerator CameraShake()
	{
		if (!StandardReload)
		{
			CameraShaker.Instance.ShakeOnce(2f, 2f, 0.4f, 4f);
			yield return new WaitForSeconds(0.75f);
			CameraShaker.Instance.ShakeOnce(1.5f, 2f, 0.4f, 2f);
			yield return new WaitForSeconds(1.35f);
			CameraShaker.Instance.ShakeOnce(1f, 2f, 0.4f, 2f);
			yield return new WaitForSeconds(0.35f);
			CameraShaker.Instance.ShakeOnce(1f, 2f, 0.4f, 2f);
		}
		if (StandardReload)
		{
			CameraShaker.Instance.ShakeOnce(2f, 2f, 0.4f, 4f);
			yield return new WaitForSeconds(0.75f);
			CameraShaker.Instance.ShakeOnce(1.5f, 2f, 0.4f, 2f);
		}
	}

	private IEnumerator FireSound()
	{
		yield return new WaitForSeconds(ShellDuration);
		Object.Instantiate(SoundPrefab);
	}

	public void StartVault()
	{
		StartCoroutine(Vault());
	}

	public IEnumerator Vault()
	{
		isVaulting = true;
		LegacyAnim.Stop();
		LegacyAnim.Play("Vault");
		yield return new WaitForSeconds(VaultDuration);
		isVaulting = false;
	}

	public void ZoomIn()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, ZoomPosition.transform.position, Time.deltaTime * ZoomTime);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, ZoomPosition.transform.rotation, Time.deltaTime * ZoomTime);
		Base.swayAmount = 0f;
		WeaponParent.transform.localPosition = new Vector3(0f, 0f, 0f);
		WeaponParent.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
	}

	public void ReturnPosition()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, MainPosition.transform.position, Time.deltaTime * ZoomTime);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, MainPosition.transform.rotation, Time.deltaTime * ZoomTime);
		Base.swayAmount = 0.006f;
	}

	public void Run()
	{
		WeaponParent.SetTrigger("Run");
		canFire = false;
	}

	public void Walk()
	{
		canFire = true;
		WeaponParent.SetTrigger("Idle");
	}

	public void Crouch()
	{
		isCrouching = true;
		base.transform.position = Vector3.Lerp(base.transform.position, CrouchPosition.transform.position, Time.deltaTime * 5f);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, CrouchPosition.transform.rotation, Time.deltaTime * 5f);
		WeaponParent.transform.localPosition = new Vector3(0f, 0f, 0f);
		WeaponParent.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
	}

	public void Stand()
	{
		isCrouching = false;
		base.transform.position = Vector3.Lerp(base.transform.position, MainPosition.transform.position, Time.deltaTime * 5f);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, MainPosition.transform.rotation, Time.deltaTime * 5f);
		Base.swayAmount = 0.006f;
	}
}
