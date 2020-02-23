using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommandTerminal
{
	public static class BuiltinCommands
	{
		[RegisterCommand(null, Help = "Lists Commands", MaxArgCount = 1)]
		private static void CommandHelp(CommandArg[] args)
		{
			if (args.Length == 0)
			{
				foreach (KeyValuePair<string, CommandInfo> command in Terminal.Shell.Commands)
				{
					Terminal.Log("{0}: {1}", command.Key.PadRight(16), command.Value.help);
				}
				return;
			}
			string text = args[0].String.ToUpper();
			if (!Terminal.Shell.Commands.ContainsKey(text))
			{
				Terminal.Shell.IssueErrorMessage("Command {0} could not be found.", text);
				return;
			}
			string help = Terminal.Shell.Commands[text].help;
			if (help == null)
			{
				Terminal.Log("{0} does not provide any help documentation.", text);
			}
			else
			{
				Terminal.Log(help);
			}
		}

		private static string JoinArguments(CommandArg[] args)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = args.Length;
			for (int i = 0; i < num; i++)
			{
				stringBuilder.Append(args[i].String);
				if (i < num - 1)
				{
					stringBuilder.Append(" ");
				}
			}
			return stringBuilder.ToString();
		}

		[RegisterCommand(null, Help = "Resets Your Position")]
		private static void Reset_pos(CommandArg[] args)
		{
			Debug.Log("Resetting Position");
			GameManager.GM.Player.transform.position = new Vector3(0f, 0f, 0f);
		}

		[RegisterCommand(null, Help = "NoClip")]
		private static void NoClip(CommandArg[] args)
		{
			Debug.Log("NoClip enabled");
			GameManager.GM.NoClip();
		}

		[RegisterCommand(null, Help = "Toggles your HUD")]
		private static void Toggle(CommandArg[] args)
		{
			Debug.Log("TOGGLING");
			MenuScript component = GameObject.FindGameObjectWithTag("Manager").GetComponent<MenuScript>();
			if (component.HUDElements.activeInHierarchy)
			{
				component.HUDElements.SetActive(value: false);
			}
			else
			{
				component.HUDElements.SetActive(value: true);
			}
		}

		[RegisterCommand(null, Help = "Kills the player")]
		private static void Suicide(CommandArg[] args)
		{
			Debug.Log("If you need help, just come to my Discord and talk!");
			GameManager.GM.Player.GetComponent<HealthScript>().Die();
		}

		[RegisterCommand(null, Help = "Deletes save data")]
		private static void DeleteData(CommandArg[] args)
		{
			Debug.Log("DATA DELETED");
			PlayerPrefs.DeleteAll();
		}

		[RegisterCommand(null, Help = "Gives Weapon")]
		private static void CommandGive(int item)
		{
			Debug.Log("WEAPON GIVEN");
			GameManager.GM.Player.GetComponent<InventoryScript>().GrabWeapon(item);
		}

		private static void FrontCommandGive(CommandArg[] args)
		{
			CommandGive(args[0].Int);
		}

		[RegisterCommand(null, Help = "Equips Equipment")]
		private static void CommandEquip(int item)
		{
			Debug.Log("EQUIPMENT EQUIPPED");
			GameManager.GM.Player.GetComponent<InventoryScript>().AquireEquipment(item);
		}

		private static void FrontCommandEquip(CommandArg[] args)
		{
			CommandEquip(args[0].Int);
		}

		[RegisterCommand(null, Help = "Loads Scene")]
		private static void CommandLoad(int item)
		{
			Debug.Log("LOADING...");
			if (item > 1)
			{
				SceneManager.LoadScene(item);
			}
			GameManager.GM.Player.transform.position = new Vector3(0f, 0f, 0f);
		}

		private static void FrontCommandLoad(CommandArg[] args)
		{
			CommandLoad(args[0].Int);
		}

		[RegisterCommand(null, Help = "Plays Song")]
		private static void CommandPlay(int item)
		{
			Debug.Log("SONG PLAYED");
			GameManager.GM.GetComponent<GTTODManager>().ChooseSong(item);
		}

		private static void FrontCommandPlay(CommandArg[] args)
		{
			CommandPlay(args[0].Int);
		}

		[RegisterCommand(null, Help = "Grants Points")]
		private static void CommandGrant(int item)
		{
			Debug.Log("POINTS GRANTED");
			GameManager.GM.GetComponent<GTTODManager>().Points += item;
			GameManager.GM.GetComponent<GTTODManager>().CheckPointsUI();
		}

		private static void FrontCommandGrant(CommandArg[] args)
		{
			CommandGrant(args[0].Int);
		}

		[RegisterCommand(null, Help = "Grants Starter Card Unlocks")]
		private static void CommandGrantDoor(int item)
		{
			Debug.Log("DOORS GRANTED");
			int @int = PlayerPrefs.GetInt("DoorsOpened", 0);
			@int += item;
			PlayerPrefs.SetInt("DoorsOpened", @int);
		}

		private static void FrontCommandGrantDoor(CommandArg[] args)
		{
			CommandGrantDoor(args[0].Int);
		}
	}
}
