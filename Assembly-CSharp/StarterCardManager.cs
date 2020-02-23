using UnityEngine;
using UnityEngine.UI;

public class StarterCardManager : MonoBehaviour
{
	[Header("Scroll Settings")]
	public RectTransform Content;

	public Slider MenuSlider;

	public float StepAmount;

	public float MaximumStepAmount;

	[Header("Button Settings")]
	public GameObject SelectionScreen;

	public Text CardName;

	public Image CardWeapon;

	public Image CardEquipment;

	public Text CardDescription;

	[Header("Card Settings")]
	private int WeaponID;

	private int EquipmentID;

	private GameObject CustomSetting;

	private StarterCard CurrentStarterCard;

	private GTTODManager Manager;

	private float CurrentArea;

	private float AreaLerp;

	private void Start()
	{
		Manager = GameManager.GM.GetComponent<GTTODManager>();
	}

	private void Update()
	{
		AreaLerp = Mathf.Lerp(AreaLerp, CurrentArea, 0.1f);
		Content.SetLeft(0f - AreaLerp);
		Content.SetRight(AreaLerp);
		if (KeyBindingManager.GetKeyDown(KeyAction.Left))
		{
			Left();
		}
		if (KeyBindingManager.GetKeyDown(KeyAction.Right))
		{
			Right();
		}
	}

	public void Left()
	{
		CurrentArea -= StepAmount;
		MenuSlider.value = CurrentArea;
		SetContentPosition();
	}

	public void Right()
	{
		CurrentArea += StepAmount;
		MenuSlider.value = CurrentArea;
		SetContentPosition();
	}

	public void Back()
	{
		SelectionScreen.SetActive(value: false);
		CurrentArea = 0f;
		MenuSlider.value = CurrentArea;
		SetContentPosition();
	}

	public void AdjustSlider(float Value)
	{
		CurrentArea = Value;
	}

	public void SetCardSettings(int SentWeaponID, int SentEquipmentID, GameObject SentCustomSetting, StarterCard Card)
	{
		WeaponID = SentWeaponID;
		EquipmentID = SentEquipmentID;
		CustomSetting = SentCustomSetting;
		CurrentStarterCard = Card;
		CardName.text = CurrentStarterCard.NameText.text.ToString();
		CardWeapon.sprite = CurrentStarterCard.WeaponImage.sprite;
		CardWeapon.rectTransform.sizeDelta = CurrentStarterCard.WeaponImage.rectTransform.sizeDelta;
		CardEquipment.sprite = CurrentStarterCard.EquipmentImage.sprite;
		CardEquipment.rectTransform.sizeDelta = CurrentStarterCard.EquipmentImage.rectTransform.sizeDelta;
		CardDescription.text = CurrentStarterCard.DescriptionText.text.ToString();
		SelectionScreen.SetActive(value: true);
	}

	public void PlayGame()
	{
		Manager.StartCardMode(WeaponID, EquipmentID, CustomSetting);
		CurrentStarterCard.PlayCard();
	}

	public void SetContentPosition()
	{
		if (CurrentArea > MaximumStepAmount)
		{
			CurrentArea = MaximumStepAmount;
		}
		if (CurrentArea < 0f)
		{
			CurrentArea = 0f;
		}
	}
}
