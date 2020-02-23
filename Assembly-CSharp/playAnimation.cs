using UnityEngine;

public class playAnimation : MonoBehaviour
{
	public bool play;

	private ParticleSystem[] dust;

	private void Start()
	{
		dust = GetComponentsInChildren<ParticleSystem>();
	}

	private void Update()
	{
		if (play)
		{
			dust[0].Play();
			dust[1].Play();
			play = false;
		}
	}

	private void DisableBoard()
	{
		dust[0].Play();
		dust[1].Play();
		base.gameObject.SetActive(value: false);
	}
}
