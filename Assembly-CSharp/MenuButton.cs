using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
	[Header("BUTTON OBJECTS")]
	public Image Button;

	public Text ButtonText;

	public AudioSource ButtonAudio;

	[Header("BUTTON ASSETS")]
	public Sprite IdleImage;

	public Sprite HoverImage;

	public AudioClip HoverSound;

	public AudioClip ClickSound;

	private void OnEnable()
	{
		Idle();
	}

	public void Idle()
	{
		Button.sprite = IdleImage;
		ButtonText.color = Color.white;
	}

	public void OnHover()
	{
		ButtonAudio.PlayOneShot(HoverSound);
		Button.sprite = HoverImage;
		ButtonText.color = Color.black;
	}

	public void OnClick()
	{
		ButtonAudio.PlayOneShot(ClickSound);
	}
}
