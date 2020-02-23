using UnityEngine;

[ExecuteInEditMode]
public class LimbIK : MonoBehaviour
{
	public Vector3 elbowForward = Vector3.back;

	public Transform upperLimb;

	public Transform lowerLimb;

	public Transform endLimb;

	public Transform target;

	private void LateUpdate()
	{
		IKSolver.Solve(debug: false, 1f, 1f, 0f, -1f, -1f, upperLimb, lowerLimb, endLimb, elbowForward, target.position, Vector3.zero, target.rotation);
	}
}
