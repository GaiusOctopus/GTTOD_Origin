using UnityEngine;

public class AlignWithGround : MonoBehaviour
{
	public Transform backLeft;

	public Transform backRight;

	public Transform frontLeft;

	public Transform frontRight;

	public RaycastHit lr;

	public RaycastHit rr;

	public RaycastHit lf;

	public RaycastHit rf;

	public Vector3 upDir;

	public bool isSliding;

	private Transform SetTransform;

	private void Start()
	{
		SetTransform = new GameObject("FollowTransform").transform;
		SetTransform.parent = base.transform.parent;
	}

	private void Update()
	{
		if (isSliding)
		{
			Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr);
			Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr);
			Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf);
			Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf);
			Vector3 vector = rr.point - lr.point;
			Vector3 vector2 = rf.point - rr.point;
			Vector3 vector3 = lf.point - rf.point;
			Vector3 vector4 = rr.point - lf.point;
			Vector3 a = Vector3.Cross(vector2, vector);
			Vector3 b = Vector3.Cross(vector3, vector2);
			Vector3 b2 = Vector3.Cross(vector4, vector3);
			Vector3 b3 = Vector3.Cross(vector, vector4);
			SetTransform.up = (a + b + b2 + b3).normalized;
			base.transform.position = Vector3.Lerp(base.transform.position, SetTransform.position, 0.035f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, SetTransform.rotation, 0.035f);
		}
	}
}
