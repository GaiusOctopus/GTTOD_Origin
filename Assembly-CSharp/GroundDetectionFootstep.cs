using UnityEngine;

public class GroundDetectionFootstep : MonoBehaviour
{
	public GameObject DefaultFootstep;

	public GameObject SoftFootstep;

	private RaycastHit Hit;

	public LayerMask PotentialLayers;

	private void Start()
	{
		if (Physics.Raycast(base.transform.position, base.transform.up * -1f, out Hit, 2f, PotentialLayers.value))
		{
			if (Hit.collider.tag != "Dirt")
			{
				Object.Instantiate(DefaultFootstep, base.transform.position, base.transform.rotation);
				Object.Destroy(base.gameObject);
			}
			else
			{
				Object.Instantiate(SoftFootstep, base.transform.position, base.transform.rotation);
				Object.Destroy(base.gameObject);
			}
		}
	}
}
