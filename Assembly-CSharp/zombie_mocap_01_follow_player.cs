using UnityEngine;

public class zombie_mocap_01_follow_player : MonoBehaviour
{
	public GameObject Player;

	public float cameraHeight = 10f;

	public float cameraDistance = 5f;

	private void Update()
	{
		Vector3 position = Player.transform.position;
		Debug.Log(position.x);
		position.y = cameraHeight;
		position.x = cameraDistance + position.x;
		base.transform.position = position;
	}
}
