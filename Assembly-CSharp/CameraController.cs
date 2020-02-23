using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject cameraTarget;

	public float rotateSpeed;

	private float rotate;

	public float offsetDistance;

	public float offsetHeight;

	public float smoothing;

	private Vector3 offset;

	private bool following = true;

	private Vector3 lastPosition;

	private void Start()
	{
		lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
		offset = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			if (following)
			{
				following = false;
			}
			else
			{
				following = true;
			}
		}
		if (Input.GetKey(KeyCode.Q))
		{
			rotate = -1f;
		}
		else if (Input.GetKey(KeyCode.E))
		{
			rotate = 1f;
		}
		else
		{
			rotate = 0f;
		}
		if (following)
		{
			offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
			base.transform.position = cameraTarget.transform.position + offset;
			base.transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime), Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime), Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));
		}
		else
		{
			base.transform.position = lastPosition;
		}
		base.transform.LookAt(cameraTarget.transform.position);
	}

	private void LateUpdate()
	{
		lastPosition = base.transform.position;
	}
}
