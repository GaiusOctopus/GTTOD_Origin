using UnityEngine;

public class ScaleRelativeToCamera : MonoBehaviour
{
	private Camera cam;

	public float objectScale = 1f;

	private Vector3 initialScale;

	private void Start()
	{
		cam = GameManager.GM.Player.GetComponent<ac_CharacterController>().MainCamera.GetComponent<Camera>();
		initialScale = base.transform.localScale;
		if (cam == null)
		{
			cam = Camera.main;
		}
	}

	private void Update()
	{
		float distanceToPoint = new Plane(cam.transform.forward, cam.transform.position).GetDistanceToPoint(base.transform.position);
		base.transform.localScale = initialScale * distanceToPoint * objectScale;
	}
}
