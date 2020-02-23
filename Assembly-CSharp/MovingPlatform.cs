using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public bool IsOnPlatform;

	private Transform Player;

	private Vector3 offset;

	private bool HasSet;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
	}

	public void OnPlatform()
	{
		IsOnPlatform = true;
		SetOffset();
		Debug.Log("Yoyoyoy");
	}

	private void LateUpdate()
	{
		if (IsOnPlatform)
		{
			Player.transform.position = base.transform.position + offset;
		}
	}

	public void SetOffset()
	{
		if (!HasSet)
		{
			offset = Player.transform.position - base.transform.position;
			HasSet = true;
		}
	}
}
