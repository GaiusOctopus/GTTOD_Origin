using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarterCard : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[Header("SET-UP")]
	public StarterCardManager Manager;

	public Text NameText;

	public Image WeaponImage;

	public Image EquipmentImage;

	public Text DescriptionText;

	public Sprite LockedSprite;

	public Sprite NoItemSprite;

	public GameObject Outline;

	public GameObject NewIndicator;

	public AudioSource Audio;

	public AudioClip HoverSFX;

	public AudioClip ClickSFX;

	[Header("CARD INFO")]
	public WeaponScript Weapon;

	public int EquipmentID = 1;

	public GameObject CustomSetting;

	public int PointsToUnlock;

	[HideInInspector]
	[Header("PRIVATE VARIABLES")]
	public int WeaponID = 1;

	private string CardName = "Card Name";

	private Sprite WeaponIcon;

	private Sprite EquipmentIcon;

	private string CardDescription = "Card Description";

	private GameManager GM;

	private InventoryScript Inventory;

	private Button MyButton;

	private bool Unlocked;

	private bool IsNew = true;

	private void Start()
	{
		GM = GameManager.GM;
		Inventory = GM.Player.GetComponent<InventoryScript>();
		MyButton = GetComponent<Button>();
		WeaponID = Weapon.MyWeaponID;
		CardName = NameText.text.ToString();
		WeaponIcon = WeaponImage.sprite;
		EquipmentIcon = EquipmentImage.sprite;
		CardDescription = DescriptionText.text;
		if (PointsToUnlock <= PlayerPrefs.GetInt("DoorsOpened", 0))
		{
			EnableCard();
			return;
		}
		MyButton.interactable = false;
		NewIndicator.SetActive(value: false);
		NameText.text = "";
		WeaponImage.sprite = LockedSprite;
		EquipmentImage.sprite = NoItemSprite;
		WeaponImage.rectTransform.sizeDelta = new Vector2(150f, 150f);
		WeaponImage.rectTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		EquipmentImage.rectTransform.sizeDelta = new Vector2(150f, 150f);
		EquipmentImage.rectTransform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		DescriptionText.text = "Unlock this card by beating more levels!";
	}

	public void CheckCardUnlocks()
	{
		if (PointsToUnlock <= PlayerPrefs.GetInt("DoorsOpened", 0) && !Unlocked)
		{
			UnlockCard();
		}
	}

	private void EnableCard()
	{
		NameText.text = CardName.ToString();
		WeaponImage.sprite = WeaponIcon;
		EquipmentImage.sprite = EquipmentIcon;
		DescriptionText.text = CardDescription.ToString();
		MyButton.interactable = true;
		Unlocked = true;
		IsNew = PlayerPrefsPlus.GetBool(CardName, defaultValue: false);
		if (IsNew)
		{
			NewIndicator.SetActive(value: true);
		}
		else
		{
			NewIndicator.SetActive(value: false);
		}
	}

	private void UnlockCard()
	{
		NameText.text = CardName.ToString();
		WeaponImage.sprite = WeaponIcon;
		EquipmentImage.sprite = EquipmentIcon;
		DescriptionText.text = CardDescription.ToString();
		MyButton.interactable = true;
		Unlocked = true;
		Inventory.PrintFancyMessage(CardName + " CARD UNLOCKED");
	}

	public void ActivateCard()
	{
		Manager.SetCardSettings(WeaponID, EquipmentID, CustomSetting, this);
	}

	public void PlayCard()
	{
		IsNew = false;
		PlayerPrefsPlus.SetBool(CardName, IsNew);
	}

	public void OnDragEnter()
	{
		Audio.PlayOneShot(HoverSFX);
		base.transform.localScale = new Vector3(0.375f, 0.375f, 0.375f);
		Outline.SetActive(value: true);
	}

	public void OnDragExit()
	{
		base.transform.localScale = new Vector3(0.325f, 0.325f, 0.325f);
		Outline.SetActive(value: false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Audio.PlayOneShot(ClickSFX);
	}
}
