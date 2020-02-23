using UnityEngine;

public class AudioRange : MonoBehaviour
{
	public bool PlayOnStart = true;

	private AudioSource audioSource;

	public float lowestPitch;

	public float highestPitch;

	public AudioClip[] Sounds;

	public bool TimePitch;

	private void Start()
	{
		audioSource = base.gameObject.GetComponent<AudioSource>();
		if (PlayOnStart)
		{
			PlayAudio();
		}
	}

	public void PlayAudio()
	{
		float pitch = Random.Range(lowestPitch, highestPitch);
		audioSource.pitch = pitch;
		int num = Random.Range(0, Sounds.Length);
		audioSource.clip = Sounds[num];
		audioSource.Play();
	}

	private void Update()
	{
		if (TimePitch && Time.timeScale < 1f)
		{
			audioSource.pitch = Time.timeScale;
		}
	}
}
