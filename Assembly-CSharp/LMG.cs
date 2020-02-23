using UnityEngine;

public class LMG : MonoBehaviour
{
	public bool PrimaryFire = true;

	public bool SecondaryFire;

	public float Min;

	public float Max;

	public float Speed;

	public bool BarrelRotation;

	[ConditionalField("BarrelRotation", null)]
	public Transform Barrel;

	[ConditionalField("BarrelRotation", null)]
	public float RotationSpeed = 100f;

	[ConditionalField("BarrelRotation", null)]
	public AudioSource SFX;

	public bool Heavy;

	private float CurrentRateOfFire;

	private float SpinSpeed;

	private float MaxSpinSpeed;

	private WeaponScript Weapon;

	private void Start()
	{
		CurrentRateOfFire = Min;
		MaxSpinSpeed = 2000f;
		SpinSpeed = 0f;
		Weapon = GetComponent<WeaponScript>();
	}

	private void OnEnable()
	{
		if (Heavy)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<ac_CharacterController>().Speed /= 1.25f;
		}
	}

	private void OnDisable()
	{
		if (Heavy)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<ac_CharacterController>().Speed *= 1.25f;
		}
	}

	private void Update()
	{
		if (PrimaryFire)
		{
			Weapon.PrimaryRateOfFire = CurrentRateOfFire;
		}
		if (SecondaryFire)
		{
			Weapon.SecondaryRateOfFire = CurrentRateOfFire;
		}
		if ((Input.GetKey(Weapon.FireKey) && !Weapon.isReloading) || (Input.GetKey(Weapon.SwapKey) && !Weapon.isReloading) || (Input.GetAxis("RightTrigger") >= 0.5f && !Weapon.isReloading))
		{
			if (CurrentRateOfFire > Max)
			{
				CurrentRateOfFire -= Time.deltaTime * Speed;
			}
			if (BarrelRotation)
			{
				if (SpinSpeed < MaxSpinSpeed)
				{
					SpinSpeed += Time.deltaTime * RotationSpeed;
				}
				Barrel.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime);
				SFX.pitch = SpinSpeed / MaxSpinSpeed;
				SFX.volume = SpinSpeed / MaxSpinSpeed;
			}
		}
		else
		{
			CurrentRateOfFire = Mathf.Lerp(CurrentRateOfFire, Min, Speed);
			if (BarrelRotation)
			{
				SpinSpeed = Mathf.Lerp(SpinSpeed, 0f, 0.025f);
				Barrel.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime);
				SFX.pitch = SpinSpeed / MaxSpinSpeed;
				SFX.volume = SpinSpeed / MaxSpinSpeed;
			}
		}
	}
}
