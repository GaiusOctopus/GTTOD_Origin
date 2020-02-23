using UnityEngine;

public class CircleRail : MonoBehaviour
{
	public Transform Spinner;

	public Transform Lock;

	public Transform Angle;

	public GameObject Effects;

	public float Speed;

	private float Direction;

	private float SpeedSave;

	private bool Locked;

	private GameObject Player;

	private Rigidbody PlayerPhysics;

	private ac_CharacterController CharacterController;

	private Vector3 LookPosition;

	private Quaternion LookRotation;

	private void Start()
	{
		Player = GameManager.GM.Player;
		PlayerPhysics = Player.GetComponent<Rigidbody>();
		CharacterController = Player.GetComponent<ac_CharacterController>();
		SpeedSave = Speed;
	}

	private void Update()
	{
		if (!Locked)
		{
			LookPosition = Player.transform.position - Spinner.transform.position;
			LookPosition.y = 0f;
			LookRotation = Quaternion.LookRotation(LookPosition);
			Spinner.transform.rotation = LookRotation;
			return;
		}
		Spinner.transform.Rotate(Vector3.up, Speed * Time.deltaTime);
		if (KeyBindingManager.GetKey(KeyAction.Forward) || Input.GetAxis("Vertical") > 0f)
		{
			Speed = Mathf.Lerp(Speed, SpeedSave * Direction, 0.01f);
			if (KeyBindingManager.GetKeyDown(KeyAction.Jump) || Input.GetButton("A"))
			{
				EndRail();
			}
			Angle.LookAt(Player.transform);
			float num = Vector3.Angle(Player.transform.forward, Angle.forward);
			CharacterController.PreventDashing();
			if (num >= 75f && num <= 125f)
			{
				Direction = 0f;
			}
			else if (num >= 125f)
			{
				Direction = -1f;
			}
			else if (num <= 75f)
			{
				Direction = 1f;
			}
		}
		else
		{
			Speed = Mathf.Lerp(Speed, 0f, 0.01f);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !Locked)
		{
			BeginRail();
		}
	}

	public void BeginRail()
	{
		Player.transform.position = Lock.transform.position;
		Player.transform.parent = Lock;
		PlayerPhysics.isKinematic = true;
		Effects.SetActive(value: true);
		CharacterController.ResetAbilities();
		Angle.LookAt(Player.transform);
		if (Vector3.Angle(Player.transform.forward, Angle.forward) >= 100f)
		{
			Direction = -1f;
		}
		else
		{
			Direction = 1f;
		}
		Speed = SpeedSave * Direction;
		Locked = true;
	}

	public void EndRail()
	{
		Player.transform.parent = CharacterController.Parent;
		PlayerPhysics.isKinematic = false;
		PlayerPhysics.AddForce(Player.transform.forward * 750f);
		Effects.SetActive(value: false);
		Locked = false;
		CharacterController.ResetAbilities();
	}
}
