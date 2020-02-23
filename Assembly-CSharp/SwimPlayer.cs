using System.Collections;
using UnityEngine;

public class SwimPlayer : MonoBehaviour
{
	private Rigidbody PlayerPhysics;

	private bool canThrust = true;

	private float rotationX;

	private float rotationY;

	[Header("Inputs")]
	public GameObject Cam;

	public float Sensitivity = 2f;

	public float MaxYRotation = 90f;

	public float MaxXRotation = 90f;

	private void Start()
	{
		PlayerPhysics = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		rotationX += Input.GetAxis("Mouse X") * Sensitivity;
		rotationY += (0f - Input.GetAxis("Mouse Y")) * Sensitivity;
		float num = 0f - MaxYRotation;
		if (rotationY >= MaxYRotation)
		{
			rotationY = MaxYRotation;
		}
		if (num >= rotationY)
		{
			rotationY = num;
		}
		base.gameObject.transform.rotation = Quaternion.Euler(base.gameObject.transform.rotation.x, rotationX, base.gameObject.transform.rotation.z);
		Cam.transform.localRotation = Quaternion.Euler(rotationY, Cam.transform.localRotation.y, 0f);
		if (Input.GetKey(KeyCode.W) && canThrust)
		{
			PlayerPhysics.velocity = Vector3.zero;
			PlayerPhysics.AddForce(Cam.transform.forward * 200f);
			canThrust = false;
			StartCoroutine(Thrust());
		}
	}

	private void FixedUpdate()
	{
		if (canThrust)
		{
			PlayerPhysics.velocity = new Vector3(PlayerPhysics.velocity.x * 0.975f, PlayerPhysics.velocity.y * 0.975f, PlayerPhysics.velocity.z * 0.975f);
		}
	}

	private IEnumerator Thrust()
	{
		yield return new WaitForSeconds(1f);
		canThrust = true;
	}
}
