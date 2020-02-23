using System;
using UnityEngine;

[Serializable]
public class EquipmentItem
{
	public string EquipmentName = "EquipmentName";

	public string Name = "EquipmentName";

	public GameObject EquipmentObject;

	public string ObjectPoint;

	public string AnimationName;

	public float ShakeMagnitude;

	public float ActivationTime;

	public float DeactivationTime;

	public float Cooldown;

	public AudioClip SoundEffect;

	public int Price;

	public bool CanUse = true;

	public void SetEquipment()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>().SetUpEquipment(EquipmentObject, ObjectPoint, AnimationName, ShakeMagnitude, ActivationTime, DeactivationTime, Cooldown, SoundEffect, GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>().Equipment.IndexOf(this));
		GameManager.GM.Player.GetComponent<InventoryScript>().PrintMessage(Name + " EQUIPPED");
	}

	public void SetEquipmentName()
	{
		EquipmentName = GameObject.Find("Player").GetComponent<InventoryScript>().Equipment.IndexOf(this) + ". " + Name.ToString();
	}
}
