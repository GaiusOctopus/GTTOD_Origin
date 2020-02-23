using UnityEngine;

public class ADS : MonoBehaviour
{
	public Transform AimPostion;

	public Animator Idle;

	public Animator WeaponMovement;

	private float LastMovementSpeed;

	private void Start()
	{
		LastMovementSpeed = GetComponent<WeaponScript>().WeaponMovementSpeed;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.Mouse1))
		{
			GetComponent<WeaponScript>().WeaponMovementSpeed = 0f;
			base.transform.position = Vector3.Lerp(base.transform.position, AimPostion.position, 0.5f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, AimPostion.rotation, 0.5f);
			Idle.speed = 0f;
			Idle.transform.localPosition = new Vector3(0f, 0f, 0f);
			Idle.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
			WeaponMovement.speed = 0f;
			WeaponMovement.transform.localPosition = new Vector3(0f, 0f, 0f);
			WeaponMovement.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
		}
		else
		{
			Idle.speed = 1f;
			WeaponMovement.speed = 1f;
			GetComponent<WeaponScript>().WeaponMovementSpeed = LastMovementSpeed;
		}
	}
}
