using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Typer : MonoBehaviour
{
	public List<TyperMessage> Messages;

	private AudioRange Audio;

	private Text _display;

	private TyperMessage _currentMessage;

	private float _timeLastTyped;

	private float _typeDelay = 0.1f;

	private float TimeToType;

	private int _currentProgress;

	private bool ShouldType;

	private string LastCharacter;

	private void Start()
	{
		_display = GetComponent<Text>();
		Audio = GetComponent<AudioRange>();
		foreach (TyperMessage message in Messages)
		{
			message.FixText();
		}
	}

	private void Update()
	{
		if (_currentMessage == null)
		{
			if (Messages.Count > 0)
			{
				_currentMessage = Messages[0];
				Messages.Remove(_currentMessage);
				_currentProgress = 0;
			}
		}
		else if (_currentProgress != _currentMessage.Text.Length)
		{
			if (TimeToType <= 0f)
			{
				TypeNextCharacter(_currentMessage, _currentProgress);
				_currentProgress++;
				_timeLastTyped = Time.time;
				TimeToType = _typeDelay;
			}
			else
			{
				TimeToType -= Time.deltaTime;
			}
		}
	}

	private void TypeNextCharacter(TyperMessage currentMessage, int currentProgress)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(currentMessage.Text.Substring(0, currentProgress));
		stringBuilder.Append("<color=#ffffff00>");
		stringBuilder.Append(currentMessage.Text.Substring(currentProgress));
		stringBuilder.Append("</color>");
		_display.text = stringBuilder.ToString();
		if (LastCharacter == "," || LastCharacter == ".")
		{
			_typeDelay = 0.35f;
		}
		else
		{
			_typeDelay = Random.Range(0.0125f, 0.05f);
			if (ShouldType)
			{
				Audio.PlayAudio();
				ShouldType = false;
			}
			else
			{
				ShouldType = true;
			}
		}
		LastCharacter = currentMessage.Text[currentProgress].ToString();
	}

	public void NextMessage()
	{
		_currentMessage = null;
	}

	public void SetMessageToPlay(TyperMessage MessageToSend)
	{
		if (base.gameObject.activeInHierarchy)
		{
			Messages.Clear();
			Messages.Add(MessageToSend);
			_currentMessage = MessageToSend;
			_currentProgress = 0;
			StartCoroutine(ClearText(5f));
		}
	}

	public IEnumerator ClearText(float TimeToClear)
	{
		yield return new WaitForSeconds(TimeToClear);
		_display.text = "";
		Messages.Clear();
		GetComponent<AudioSource>().clip = null;
	}
}
