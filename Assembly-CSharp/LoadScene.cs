using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
	public Animation Canvas;

	public Text ThankText;

	public AudioClip StartingClip;

	public AudioClip ClosingClip;

	public List<string> ThankMessages;

	public List<StartMessage> StartMessages;

	private AudioSource Audio;

	private void Start()
	{
		Audio = GetComponent<AudioSource>();
		StartCoroutine(WaitLoad());
	}

	private IEnumerator WaitLoad()
	{
		yield return new WaitForSeconds(0.15f);
		Canvas.Play("StartingAnim");
		Audio.clip = StartingClip;
		Audio.Play();
	}

	public void Load()
	{
		int index = Random.Range(0, StartMessages.Count);
		float percentage = Random.Range(0f, 100f);
		StartMessages[index].ChooseMessage(percentage, base.gameObject);
	}

	public void StartLoad(string MessageToPlay)
	{
		StartCoroutine(LoadLevel(MessageToPlay));
	}

	public IEnumerator LoadLevel(string MessageToPlay)
	{
		Canvas.Play("ClosingAnim");
		Audio.clip = ClosingClip;
		Audio.Play();
		ThankText.text = MessageToPlay;
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(1);
	}

	public void Discord()
	{
		Application.OpenURL("https://discord.gg/PBpYjwu");
	}

	public void Reddit()
	{
		Application.OpenURL("https://www.reddit.com/r/GTTOD/");
	}
}
