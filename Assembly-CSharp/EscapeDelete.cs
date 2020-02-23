using UnityEngine;

public class EscapeDelete : MonoBehaviour
{
	public bool ShouldResetPosition = true;

	private Transform Player;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && ShouldResetPosition)
		{
			Player.transform.position = new Vector3(0f, 0f, 0f);
			Object.Destroy(base.gameObject);
		}
	}
}
