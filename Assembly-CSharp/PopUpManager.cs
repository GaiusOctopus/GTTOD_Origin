using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviour
{
	[Header("POP UP OBJECTS")]
	public GameObject PopUpObject;

	public Text QuestionText;

	public GameObject YesButton;

	public GameObject NoButton;

	public GameObject OkayButton;

	[HideInInspector]
	public bool PopUpOpen;

	private ac_CharacterController CharacterController;

	private GameObject ReplyObject;

	private Animation Anim;

	private void Start()
	{
		CharacterController = GetComponent<ac_CharacterController>();
		Anim = PopUpObject.GetComponent<Animation>();
	}

	public void AskQuestion(string QuestionToAsk, GameObject Sender)
	{
		QuestionText.text = QuestionToAsk;
		ReplyObject = Sender;
		CharacterController.FreezePlayer();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PopUpObject.SetActive(value: true);
		Anim.Play("PopUpIn");
		PopUpOpen = true;
		YesButton.SetActive(value: true);
		NoButton.SetActive(value: true);
		OkayButton.SetActive(value: false);
	}

	public void AnswerQuestion(bool Yes)
	{
		if (Yes)
		{
			ReplyObject.SendMessage("Yes");
		}
		else
		{
			ReplyObject.SendMessage("No");
		}
		CharacterController.UnFreezePlayer();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Anim.Play("PopUpOut");
		PopUpOpen = false;
	}

	public void InformPlayer(string QuestionToAsk)
	{
		QuestionText.text = QuestionToAsk;
		CharacterController.FreezePlayer();
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PopUpObject.SetActive(value: true);
		Anim.Play("PopUpIn");
		PopUpOpen = true;
		YesButton.SetActive(value: false);
		NoButton.SetActive(value: false);
		OkayButton.SetActive(value: true);
	}

	public void Okay()
	{
		CharacterController.UnFreezePlayer();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Anim.Play("PopUpOut");
		PopUpOpen = false;
	}
}
