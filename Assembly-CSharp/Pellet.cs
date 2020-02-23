using UnityEngine;

public class Pellet : MonoBehaviour
{
	public Vector3 Min;

	public Vector3 Max;

	private void Start()
	{
		float x = Random.Range(Min.x, Max.x);
		float y = Random.Range(Min.y, Max.y);
		float z = Random.Range(Min.z, Max.z);
		base.transform.localRotation = Quaternion.Euler(x, y, z);
		GetComponent<BulletScript>().enabled = true;
	}
}
