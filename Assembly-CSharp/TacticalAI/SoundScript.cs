using UnityEngine;

namespace TacticalAI
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundScript : MonoBehaviour
	{
		public bool audioEnabled = true;

		private AudioSource audioSource;

		[Range(0f, 1f)]
		public float oddsToPlayDamagedAudio = 0.5f;

		public SoundClip[] damagedAudio;

		[Range(0f, 1f)]
		public float oddsToPlayDeathAudio = 0.5f;

		public SoundClip[] deathAudio;

		[Range(0f, 1f)]
		public float oddsToPlaySpottedTargetAudio = 0.5f;

		public SoundClip[] spottedTargetAudio;

		[Range(0f, 1f)]
		public float oddsToPlaySuppressedAudio = 0.5f;

		public SoundClip[] suppressedAudio;

		[Range(0f, 1f)]
		public float oddsToPlayCoverAudio = 0.5f;

		public SoundClip[] coverAudio;

		[Range(0f, 1f)]
		public float oddsToPlayReloadAudio = 0.5f;

		public SoundClip[] reloadAudio;

		public void PlayDamagedAudio()
		{
			PlayAClip(damagedAudio, oddsToPlayDamagedAudio);
		}

		public void PlaySpottedAudio()
		{
			PlayAClip(spottedTargetAudio, oddsToPlaySpottedTargetAudio);
		}

		public void PlaySuppressedAudio()
		{
			PlayAClip(suppressedAudio, oddsToPlaySuppressedAudio);
		}

		public void PlayCoverAudio()
		{
			PlayAClip(coverAudio, oddsToPlayCoverAudio);
		}

		public void PlayReloadAudio()
		{
			PlayAClip(reloadAudio, oddsToPlayReloadAudio);
		}

		public void PlayDeathAudio()
		{
			PlayAClip(deathAudio, oddsToPlayDeathAudio);
		}

		public void OnAIDeath()
		{
			PlayDeathAudio();
		}

		public void PlayAClip(SoundClip[] audios, float odds)
		{
			if (!audioEnabled || !GetComponent<AudioSource>() || GetComponent<AudioSource>().isPlaying || !(Random.value < odds) || audios == null)
			{
				return;
			}
			int num = 0;
			int i;
			for (i = 0; i < audios.Length; i++)
			{
				num += audios[i].oddsToPlay;
			}
			num = Random.Range(0, num);
			i = 0;
			while (true)
			{
				if (i < audios.Length)
				{
					num -= audios[i].oddsToPlay;
					if (num <= 0)
					{
						break;
					}
					i++;
					continue;
				}
				return;
			}
			AudioSource.PlayClipAtPoint(audios[i].audioClip, base.transform.position);
		}
	}
}
