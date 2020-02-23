using UnityEngine;

public class QuickDirtyRotate : MonoBehaviour
{
	public Vector3 rotateVector;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(rotateVector * Time.deltaTime);
	}
}
