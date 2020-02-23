using UnityEngine;

public class BowStringComponent : MonoBehaviour
{
	public Transform Tip;

	public Transform End;

	private LineRenderer Trail;

	private ac_CharacterController CharacterController;

	private void Start()
	{
		Trail = GetComponent<LineRenderer>();
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
	}

	private void Update()
	{
		if (!CharacterController.OnWall)
		{
			Trail.enabled = true;
			Trail.SetPosition(0, Tip.position);
			Trail.SetPosition(1, End.position);
		}
		else
		{
			Trail.enabled = false;
		}
	}
}
