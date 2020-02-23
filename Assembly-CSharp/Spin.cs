using UnityEngine;

public class Spin : MonoBehaviour
{
	public float speed = 10f;

	public bool X;

	public bool Y = true;

	public bool Z;

	private void Update()
	{
		if (X)
		{
			base.transform.Rotate(Vector3.right, speed * Time.deltaTime);
		}
		if (Y)
		{
			base.transform.Rotate(Vector3.up, speed * Time.deltaTime);
		}
		if (Z)
		{
			base.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
		}
	}
}
