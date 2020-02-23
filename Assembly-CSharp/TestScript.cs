using UnityEngine;

public class TestScript : MonoBehaviour
{
	public Transform DashTransform;

	public Transform DashDestination;

	public bool KeyboardMouse;

	public float Distance = 35f;

	private RaycastHit DashCheck;

	private ac_CharacterController Player;

	private Vector3 SavePosition;

	private bool DashLocked;

	private float ForwardBack;

	private float LeftRight;

	private void Start()
	{
		SavePosition = DashTransform.transform.localPosition;
	}

	private void Update()
	{
		if (KeyboardMouse)
		{
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{
				DashLocked = true;
				if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
				{
					DashTransform.localPosition += base.transform.forward * Distance;
				}
				if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
				{
					DashTransform.localPosition += base.transform.forward * (0f - Distance);
				}
				if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
				{
					DashTransform.localPosition += base.transform.right * Distance;
				}
				if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
				{
					DashTransform.localPosition += base.transform.right * (0f - Distance);
				}
			}
			else
			{
				DashLocked = false;
				DashTransform.localPosition = SavePosition;
				DashDestination.position = SavePosition;
			}
			if (DashLocked)
			{
				if (Physics.Raycast(base.transform.position, DashTransform.position, out DashCheck, 35f))
				{
					Debug.DrawRay(base.transform.position, DashTransform.position, Color.red);
					DashDestination.position = DashCheck.point + DashCheck.normal * 0.35f;
				}
				else
				{
					Debug.DrawRay(base.transform.position, DashTransform.position, Color.green);
					DashDestination.position = DashTransform.position;
				}
			}
			return;
		}
		ForwardBack = Input.GetAxis("Vertical");
		LeftRight = Input.GetAxis("Horizontal");
		if (ForwardBack != 0f || LeftRight != 0f)
		{
			DashLocked = true;
			DashTransform.localPosition = new Vector3(Distance * LeftRight, base.transform.position.y, Distance * (0f - ForwardBack));
		}
		else
		{
			DashLocked = false;
			DashTransform.localPosition = SavePosition;
			DashDestination.position = SavePosition;
		}
		if (DashLocked)
		{
			if (Physics.Raycast(base.transform.position, DashTransform.position, out DashCheck, 35f))
			{
				Debug.DrawRay(base.transform.position, DashTransform.position, Color.red);
				DashDestination.position = DashCheck.point + DashCheck.normal * 0.35f;
			}
			else
			{
				Debug.DrawRay(base.transform.position, DashTransform.position, Color.green);
				DashDestination.position = DashTransform.position;
			}
		}
	}
}
