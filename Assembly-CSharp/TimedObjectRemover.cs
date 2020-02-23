using UnityEngine;

public class TimedObjectRemover : MonoBehaviour
{
	public float Time;

	private void Start()
	{
		Object.Destroy(base.gameObject, Time);
	}
}
