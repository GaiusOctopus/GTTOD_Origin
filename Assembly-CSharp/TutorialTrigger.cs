using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
	public bool TutorialMessage = true;

	[ConditionalField("TutorialMessage", null)]
	public string KeyboardMouseString;

	[ConditionalField("TutorialMessage", null)]
	public string ControllerString;

	public bool ObjectiveMessage;

	[ConditionalField("ObjectiveMessage", null)]
	public string ObjectiveString;

	public bool InputTutorial;

	[ConditionalField("InputTutorial", null)]
	public KeyAction KeyboardInput;

	[ConditionalField("InputTutorial", null)]
	public string ControllerInput;

	[ConditionalField("InputTutorial", null)]
	public GameObject Effect;

	public bool GiveWeapon;

	[ConditionalField("GiveWeapon", null)]
	public WeaponScript Weapon;

	public bool GrantPoints;

	[ConditionalField("GrantPoints", null)]
	public int PointsToGrant;

	private ac_CharacterController CharacterController;

	private InventoryScript Inventory;

	private TimeSlow TimeManager;

	private GameObject EffectSave;

	private bool hasTriggered;

	private bool HasPaused;

	private void Start()
	{
		CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Inventory = GameManager.GM.Player.GetComponent<InventoryScript>();
		TimeManager = CharacterController.gameObject.GetComponent<TimeSlow>();
	}

	private void Update()
	{
		if (HasPaused && (KeyBindingManager.GetKeyDown(KeyboardInput) || Input.GetButtonDown(ControllerInput) || Input.GetAxis(ControllerInput) > 0.25f))
		{
			HasPaused = false;
			TimeManager.StartGameTime(Instant: true);
			if (EffectSave != null)
			{
				Object.Destroy(EffectSave);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !hasTriggered)
		{
			hasTriggered = true;
			if (TutorialMessage)
			{
				PrintMessage();
			}
			else if (ObjectiveMessage)
			{
				PrintObjective();
			}
			if (GiveWeapon)
			{
				Inventory.GrabWeapon(Weapon.MyWeaponID);
			}
			if (GrantPoints)
			{
				GameManager.GM.GetComponent<GTTODManager>().Points += PointsToGrant;
				GameManager.GM.GetComponent<GTTODManager>().CheckPointsUI();
			}
		}
	}

	public void PrintMessage()
	{
		if (CharacterController.KeyboardMouse)
		{
			Inventory.PrintTutorialMessage(KeyboardMouseString);
		}
		else
		{
			Inventory.PrintTutorialMessage(ControllerString);
		}
		if (InputTutorial)
		{
			HasPaused = true;
			TimeManager.StopGameTime(CutOffFilter: false, Instant: true);
			EffectSave = Object.Instantiate(Effect);
		}
	}

	public void PrintObjective()
	{
		Inventory.PrintObjective(ObjectiveString);
	}
}
