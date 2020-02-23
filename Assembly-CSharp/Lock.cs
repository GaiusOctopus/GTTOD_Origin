using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lock : MonoBehaviour
{
	public Door LockedDoor;

	public Image Timer;

	public GameObject LockObject;

	private bool Unlocked;

	private bool Activated;

	private bool HasActivated;

	private float LockTime = 45f;

	private void Awake()
	{
		LockedDoor.Locked = true;
		StartCoroutine(TimeToCheck());
	}

	private void Update()
	{
		if (Activated && !Unlocked)
		{
			if (LockTime > 0f)
			{
				LockTime -= Time.deltaTime;
				Timer.fillAmount = LockTime / 45f;
			}
			else
			{
				Unlock();
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Activated = true;
			if (!HasActivated)
			{
				HasActivated = true;
				GameManager.GM.GetComponent<AIManager>().IncreaseDifficulty();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			Activated = false;
		}
	}

	public void Unlock()
	{
		LockedDoor.Locked = false;
		Unlocked = true;
		Activated = false;
		LockObject.SetActive(value: false);
		GetComponent<CapsuleCollider>().enabled = false;
	}

	public void ResetFuntionality()
	{
		if (!GameManager.GM.Speedrunner)
		{
			LockedDoor.Locked = true;
			Activated = false;
			Unlocked = false;
			LockTime = 20f;
			LockObject.SetActive(value: true);
			GetComponent<CapsuleCollider>().enabled = true;
		}
	}

	private IEnumerator TimeToCheck()
	{
		yield return new WaitForSeconds(3f);
		if (GameManager.GM.Speedrunner)
		{
			Unlock();
		}
	}
}
