using System;
using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public GameObject CollectEffect;

	public GameObject CollectObject;

	public int Points;

	public int Health;

	public bool Animate = true;

	public bool Respawning;

	[ConditionalField("Respawning", null)]
	public bool TimedRespawn;

	private Transform Player;

	private Transform Effect;

	private Vector3 StartPos;

	private RaycastHit RaycastDown;

	private Vector3 posOffset;

	private Vector3 tempPos;

	private bool canCollect;

	private GameManager GM;

	private void Start()
	{
		GM = GameManager.GM;
		StartPos = base.transform.position;
		posOffset = base.transform.position;
		StartCoroutine(CollectTimer());
	}

	private IEnumerator CollectTimer()
	{
		yield return new WaitForSeconds(2f);
		canCollect = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Player = other.transform;
			Collect();
		}
	}

	private void Update()
	{
		if (Animate)
		{
			base.transform.Rotate(new Vector3(0f, Time.deltaTime * 90f, 0f), Space.World);
			tempPos = posOffset;
			tempPos.y += Mathf.Sin(Time.fixedTime * (float)Math.PI * 1f) * 0.15f;
			base.transform.position = tempPos;
		}
	}

	private void Collect()
	{
		Player.GetComponent<HealthScript>().Heal(Health);
		Effect = UnityEngine.Object.Instantiate(CollectEffect, base.transform.position, base.transform.rotation).transform;
		if (Health != 0)
		{
			GameManager.GM.RandomStats[4].IncreaseStat();
		}
		if (Points != 0)
		{
			GM.GetComponent<GTTODManager>().Points += Points;
			GM.GetComponent<GTTODManager>().CheckPointsUI();
		}
		if (Respawning)
		{
			if (TimedRespawn)
			{
				GetComponent<SphereCollider>().enabled = false;
				CollectObject.SetActive(value: false);
				StartCoroutine(Respawn());
			}
			else
			{
				GetComponent<SphereCollider>().enabled = false;
				CollectObject.SetActive(value: false);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject, 0.1f);
		}
	}

	private IEnumerator Respawn()
	{
		canCollect = false;
		base.transform.position = StartPos;
		yield return new WaitForSeconds(20f);
		GetComponent<SphereCollider>().enabled = true;
		CollectObject.SetActive(value: false);
		canCollect = true;
		base.transform.position = StartPos;
	}

	public void ResetFuntionality()
	{
		GetComponent<SphereCollider>().enabled = true;
		CollectObject.SetActive(value: true);
	}
}
