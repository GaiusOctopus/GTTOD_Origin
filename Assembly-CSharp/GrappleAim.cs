using UnityEngine;

public class GrappleAim : MonoBehaviour
{
	public Transform AimPoint;

	private void Update()
	{
		base.transform.LookAt(AimPoint.position);
	}
}
