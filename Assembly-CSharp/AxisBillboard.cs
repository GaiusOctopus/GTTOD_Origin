using UnityEngine;

public class AxisBillboard : MonoBehaviour
{
	public Transform Player;

	private Vector3 LookPosition;

	private Quaternion LookRotation;

	public GameObject Hook;

	private bool canSpawn = true;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, LookRotation, 0.5f);
		LookPosition = Player.transform.position - base.transform.position;
		LookPosition.y = 0f;
		LookRotation = Quaternion.LookRotation(LookPosition);
		if (Input.GetKeyDown(KeyCode.E) && canSpawn)
		{
			Object.Instantiate(Hook, base.transform.position, base.transform.rotation);
			canSpawn = false;
		}
	}
}
