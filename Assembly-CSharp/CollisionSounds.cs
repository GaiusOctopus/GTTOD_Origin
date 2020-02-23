using UnityEngine;

public class CollisionSounds : MonoBehaviour
{
	private AudioSource Audio;

	private AudioClip Sound;

	public AudioClip[] Clips;

	private void Start()
	{
		Audio = GetComponent<AudioSource>();
		Audio.priority = 256;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 2f)
		{
			int num = Random.Range(0, Clips.Length);
			Sound = Clips[num];
			Audio.clip = Sound;
			Audio.Play();
		}
	}

	private void FixedUpdate()
	{
		Audio.pitch = Time.timeScale;
	}
}
