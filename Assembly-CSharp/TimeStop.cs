using System.Collections;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
	public AudioClip StopTime;

	public AudioClip StartTime;

	public float Limit;

	private AudioSource Audio;

	private void Start()
	{
		Audio = GetComponent<AudioSource>();
		StartCoroutine(EndFreeze());
		Object.Destroy(base.gameObject, Limit + 2f);
	}

	public IEnumerator EndFreeze()
	{
		GameManager.GM.FreezeGame();
		Audio.clip = StopTime;
		Audio.Play();
		yield return new WaitForSeconds(Limit);
		Audio.clip = StartTime;
		Audio.Play();
		GameManager.GM.UnfreezeGame();
	}
}
