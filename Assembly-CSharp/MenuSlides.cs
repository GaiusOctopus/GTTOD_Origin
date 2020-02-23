using UnityEngine;
using UnityEngine.UI;

public class MenuSlides : MonoBehaviour
{
	[Header("Set-Up")]
	public bool FullGame = true;

	[ConditionalField("FullGame", null)]
	public Camera IntroCam;

	[ConditionalField("FullGame", null)]
	public Animator Anim;

	[ConditionalField("FullGame", null)]
	public VerticalLayoutGroup Layout1;

	[ConditionalField("FullGame", null)]
	public VerticalLayoutGroup Layout2;

	[ConditionalField("FullGame", null)]
	public VerticalLayoutGroup Layout3;

	[ConditionalField("FullGame", null)]
	public Slider SpacingSlider;

	[ConditionalField("FullGame", null)]
	public GameObject TextBox;

	[ConditionalField("FullGame", null)]
	public Toggle TextBoxToggle;

	private MenuScript Menu;

	private AudioSource Audio;

	private bool inMenu;

	private bool canTransition = true;

	public void Start()
	{
		Menu = GameManager.GM.GetComponent<MenuScript>();
		Audio = GetComponent<AudioSource>();
		SpacingSlider.value = PlayerPrefs.GetFloat("MenuSpacing", 50f);
		UpdateLayout(SpacingSlider.value);
		TextBoxToggle.isOn = PlayerPrefsPlus.GetBool("MenuToggle", defaultValue: true);
		UpdateTextBox(TextBoxToggle.isOn);
	}

	public void Update()
	{
		if (!inMenu && canTransition && Input.anyKeyDown)
		{
			canTransition = false;
			if (FullGame)
			{
				if (PlayerPrefsPlus.GetBool("HasBooted", defaultValue: false))
				{
					inMenu = true;
					Anim.SetTrigger("Position");
					Audio.Play();
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
					base.gameObject.GetComponent<MenuScript>().CanPauseGame = true;
				}
				else
				{
					Anim.SetTrigger("FadeMusic");
					IntroCam.GetComponent<Animation>().Play("AbyssFall");
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
			}
			else
			{
				Anim.SetTrigger("FadeMusic");
				IntroCam.GetComponent<Animation>().Play("AbyssFall");
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}
		Audio.volume = Menu.MusicVolume / 100f;
	}

	public void UpdateLayout(float Spacing)
	{
		Layout1.spacing = Spacing;
		Layout2.spacing = Spacing;
		Layout3.spacing = Spacing;
		PlayerPrefs.SetFloat("MenuSpacing", Spacing);
	}

	public void UpdateTextBox(bool State)
	{
		TextBox.SetActive(State);
		PlayerPrefsPlus.SetBool("MenuToggle", State);
	}
}
