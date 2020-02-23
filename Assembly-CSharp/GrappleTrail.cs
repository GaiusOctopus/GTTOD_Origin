using UnityEngine;

public class GrappleTrail : MonoBehaviour
{
	private Transform ReturnPoint;

	private LineRenderer LR;

	private void Start()
	{
		LR = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		LR.SetPosition(0, base.transform.position);
		LR.SetPosition(1, ReturnPoint.transform.position);
	}
}
