using UnityEngine;

public class ObjectResetter : MonoBehaviour
{
	[Header("Reset Parameters")]
	public bool ResetPosition;

	public bool ResetRotation;

	public bool ResetScript;

	[Header("Private Parameters")]
	private Vector3 StartingPosition;

	private Quaternion StartingRotation;

	public void Awake()
	{
		StartingPosition = base.transform.position;
		StartingRotation = base.transform.rotation;
	}

	public void ResetMe()
	{
		if (ResetPosition)
		{
			base.transform.position = StartingPosition;
		}
		if (ResetRotation)
		{
			base.transform.rotation = StartingRotation;
		}
		if (ResetScript)
		{
			base.gameObject.SendMessage("ResetFuntionality");
		}
	}
}
