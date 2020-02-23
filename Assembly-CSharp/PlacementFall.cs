using UnityEngine;

public class PlacementFall : MonoBehaviour
{
	public int ItemMultiplyer;

	public float Speed;

	private float LiftHeight;

	private Vector3 StartPosition;

	private bool hasMoved;

	private float YPosition;

	private void Start()
	{
		LiftHeight = Random.Range(100, 500) * ItemMultiplyer;
		StartPosition = base.gameObject.transform.position;
		base.gameObject.transform.position = new Vector3(StartPosition.x, StartPosition.y + LiftHeight, StartPosition.z);
		hasMoved = true;
	}

	private void Update()
	{
		if (hasMoved)
		{
			base.gameObject.transform.position = Vector3.Lerp(base.gameObject.transform.position, StartPosition, Speed / (float)ItemMultiplyer);
		}
	}

	public void ResetFuntionality()
	{
		base.gameObject.transform.position = new Vector3(StartPosition.x, StartPosition.y + LiftHeight, StartPosition.z);
	}
}
