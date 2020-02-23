using UnityEngine;
using UnityEngine.UI;

public class WeaponCrate : MonoBehaviour
{
	public bool ChoiceEquipment;

	[ConditionalField("ChoiceEquipment", null)]
	public int ItemID;

	[ConditionalField("ChoiceEquipment", null)]
	public int Price;

	public Text EquipmentText;

	public Text PriceText;

	[Header("Private Variables")]
	private GameManager GM;

	private GTTODManager Manager;

	private InventoryScript Inventory;

	private Animation Anim;

	private bool InRange;

	private bool Open;

	private void Start()
	{
		GM = GameManager.GM;
		Inventory = GM.Player.GetComponent<InventoryScript>();
		Manager = GM.GetComponent<GTTODManager>();
		Anim = GetComponent<Animation>();
		if (!ChoiceEquipment)
		{
			ChooseEquipment();
		}
		else
		{
			Inventory.GetEquipmentName(base.gameObject, ItemID);
		}
	}

	private void Awake()
	{
		Anim = GetComponent<Animation>();
		Anim.Play("CloseCrate");
	}

	private void ChooseEquipment()
	{
		ItemID = Random.Range(0, Inventory.Equipment.Count);
		if (Inventory.Equipment[ItemID].CanUse)
		{
			Inventory.GetEquipmentName(base.gameObject, ItemID);
		}
		else
		{
			ChooseEquipment();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			InRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			InRange = false;
		}
	}

	private void Update()
	{
		if (((KeyBindingManager.GetKeyDown(KeyAction.Interact) && InRange && !Open) || (Input.GetButtonDown("X") && InRange && !Open)) && Manager.Points >= Price)
		{
			OpenCrate();
		}
	}

	private void OpenCrate()
	{
		Anim.Play("OpenCrate");
		GetComponent<AudioSource>().Play();
		Open = true;
		Inventory.AquireEquipment(ItemID);
		EquipmentText.transform.parent.gameObject.SetActive(value: false);
		Manager.Points -= Price;
		Manager.CheckPointsUI();
		GM.RandomStats[3].IncreaseStat();
	}

	public void ResetFuntionality()
	{
		Open = false;
		Anim.Play("CloseCrate");
		if (!ChoiceEquipment)
		{
			ItemID = Random.Range(0, Inventory.Equipment.Count);
		}
		Inventory.GetEquipmentName(base.gameObject, ItemID);
		EquipmentText.transform.parent.gameObject.SetActive(value: true);
	}

	public void SetName(string Name)
	{
		EquipmentText.text = Name.ToString();
	}

	public void SetPrice(int DefaultPrice)
	{
		if (!ChoiceEquipment)
		{
			PriceText.text = "$" + DefaultPrice.ToString();
			Price = DefaultPrice;
		}
		else
		{
			PriceText.text = "$" + Price.ToString();
		}
	}
}
