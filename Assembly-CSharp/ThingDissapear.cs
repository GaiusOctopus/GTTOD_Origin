using UnityEngine;

public class ThingDissapear : MonoBehaviour
{
	public float timetodie = 5f;

	private void Start()
	{
		Object.Destroy(base.gameObject, timetodie);
	}
}
