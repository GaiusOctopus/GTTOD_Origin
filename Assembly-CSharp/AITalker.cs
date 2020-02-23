using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITalker : MonoBehaviour
{
	public Typer Typer;

	[Header("MESSAGES")]
	public List<TyperMessage> AttackMessages;

	public List<TyperMessage> DeathMessages;

	public List<TyperMessage> EquipmentMessages;

	public List<TyperMessage> ReactionMessages;

	public List<TyperMessage> ReloadMessages;

	public List<TyperMessage> JokeMessages;

	public List<TyperMessage> AngryMessages;

	[HideInInspector]
	public bool Angry;

	private void Start()
	{
		GameManager.GM.GetComponent<AIManager>().Talkers.Add(this);
		foreach (TyperMessage attackMessage in AttackMessages)
		{
			attackMessage.Text += " ";
		}
		foreach (TyperMessage deathMessage in DeathMessages)
		{
			deathMessage.Text += " ";
		}
		foreach (TyperMessage equipmentMessage in EquipmentMessages)
		{
			equipmentMessage.Text += " ";
		}
		foreach (TyperMessage reactionMessage in ReactionMessages)
		{
			reactionMessage.Text += " ";
		}
		foreach (TyperMessage reloadMessage in ReloadMessages)
		{
			reloadMessage.Text += " ";
		}
		foreach (TyperMessage jokeMessage in JokeMessages)
		{
			jokeMessage.Text += " ";
		}
		foreach (TyperMessage angryMessage in AngryMessages)
		{
			angryMessage.Text += " ";
		}
		StartCoroutine(SayRandomMessage());
	}

	public void SayRandomAttack()
	{
		int index = Random.Range(0, AttackMessages.Count);
		Typer.SetMessageToPlay(AttackMessages[index]);
	}

	public void SayRandomDeath()
	{
		int index = Random.Range(0, DeathMessages.Count);
		Typer.SetMessageToPlay(DeathMessages[index]);
	}

	public void SayRandomEquipment()
	{
		int index = Random.Range(0, EquipmentMessages.Count);
		Typer.SetMessageToPlay(EquipmentMessages[index]);
	}

	public void SayRandomReaction()
	{
		int index = Random.Range(0, ReactionMessages.Count);
		Typer.SetMessageToPlay(ReactionMessages[index]);
	}

	public void SayRandomReload()
	{
		int index = Random.Range(0, ReloadMessages.Count);
		Typer.SetMessageToPlay(ReloadMessages[index]);
	}

	public void SayRandomJoke()
	{
		int index = Random.Range(0, JokeMessages.Count);
		Typer.SetMessageToPlay(JokeMessages[index]);
	}

	public void SayRandomAngry()
	{
		int index = Random.Range(0, AngryMessages.Count);
		Typer.SetMessageToPlay(AngryMessages[index]);
	}

	public IEnumerator SayRandomMessage()
	{
		float seconds = Random.Range(10f, 45f);
		float MessageType = Random.Range(-1f, 1f);
		yield return new WaitForSeconds(seconds);
		if (MessageType > 0f)
		{
			if (!Angry)
			{
				SayRandomAttack();
			}
			else
			{
				SayRandomAngry();
			}
		}
		else
		{
			SayRandomJoke();
		}
		StartCoroutine(SayRandomMessage());
	}

	private void OnDestroy()
	{
		if (GameManager.GM.GetComponent<AIManager>() != null)
		{
			GameManager.GM.GetComponent<AIManager>().Talkers.RemoveAt(GameManager.GM.GetComponent<AIManager>().Talkers.IndexOf(this));
		}
	}
}
