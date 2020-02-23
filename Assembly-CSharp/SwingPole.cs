using UnityEngine;

public class SwingPole : MonoBehaviour
{
	public GameObject Spinner;

	public GameObject Lock;

	private GameObject Player;

	private Rigidbody PlayerPhysics;

	private ac_CharacterController CharacterController;

	private Vector3 LookPosition;

	private Quaternion LookRotation;

	private bool OnPole;

	private bool ForwardSwing;

	private void Start()
	{
		Player = GameManager.GM.Player;
		PlayerPhysics = Player.GetComponent<Rigidbody>();
		CharacterController = Player.GetComponent<ac_CharacterController>();
	}

	private void Update()
	{
		if (!OnPole)
		{
			if (Spinner.transform.rotation.x > 0f && Spinner.transform.rotation.x < 180f)
			{
				LookPosition = Player.transform.position - Spinner.transform.position;
				LookPosition.z = 0f;
				LookRotation = Quaternion.LookRotation(LookPosition);
				Spinner.transform.rotation = LookRotation;
			}
		}
		else
		{
			if (ForwardSwing)
			{
				Spinner.transform.Rotate(Vector3.right, 500f * Time.deltaTime);
			}
			else
			{
				Spinner.transform.Rotate(Vector3.right, -500f * Time.deltaTime);
			}
			if (KeyBindingManager.GetKey(KeyAction.Jump) || Input.GetButton("A"))
			{
				EndSwing();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !OnPole)
		{
			BeginSwing();
		}
	}

	public void BeginSwing()
	{
		OnPole = true;
		Player.transform.position = Lock.transform.position;
		Player.transform.parent = Lock.transform;
		PlayerPhysics.isKinematic = true;
		if (Spinner.transform.rotation.x < 90f && Spinner.transform.rotation.x > -90f)
		{
			ForwardSwing = true;
		}
		else
		{
			ForwardSwing = false;
		}
		CharacterController.ResetAbilities();
		Player.GetComponent<InventoryScript>().OnWall();
		Player.GetComponent<InventoryScript>().BumpWeapons();
	}

	public void EndSwing()
	{
		OnPole = false;
		Player.transform.parent = CharacterController.Parent;
		PlayerPhysics.isKinematic = false;
		PlayerPhysics.AddForce(Spinner.transform.forward * 750f);
		ForwardSwing = false;
		LookPosition = Player.transform.position - Spinner.transform.position;
		LookPosition.z = 0f;
		LookRotation = Quaternion.LookRotation(LookPosition);
		Spinner.transform.rotation = LookRotation;
		CharacterController.ResetAbilities();
		Player.GetComponent<InventoryScript>().OffWall();
		Player.GetComponent<InventoryScript>().BumpWeapons();
	}
}
