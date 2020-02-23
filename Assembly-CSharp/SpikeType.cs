using UnityEngine;

public class SpikeType : MonoBehaviour
{
	public GameObject GroundSpike;

	public GameObject WallSpike;

	private float Angle;

	private void Start()
	{
		Angle = Vector3.Angle(base.transform.up, GameManager.GM.transform.up);
		if (Angle < 80f || Angle > 100f)
		{
			Object.Instantiate(WallSpike, base.transform.position, base.transform.rotation);
		}
		else
		{
			Object.Instantiate(GroundSpike, base.transform.position, base.transform.rotation);
		}
	}
}
