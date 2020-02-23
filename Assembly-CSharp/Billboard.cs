using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Transform Player;

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			base.transform.LookAt(Player);
		}
	}
}
