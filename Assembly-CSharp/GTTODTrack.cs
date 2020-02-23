using System;
using UnityEngine;

[Serializable]
public class GTTODTrack
{
	public string Name = "SongName";

	public string Artist = "Beasuce";

	public AudioClip MyTrack;

	public int MyTrackID;

	private AudioSource Audio;

	public void PlayAudio(int ID)
	{
		if (MyTrackID == ID)
		{
			Audio = GameManager.GM.GetComponent<AudioSource>();
			Audio.Stop();
			Audio.gameObject.GetComponent<GTTODManager>().SongName.text = Name.ToString();
			Audio.gameObject.GetComponent<GTTODManager>().ArtistCard.SetTrigger(Artist);
			Audio.clip = MyTrack;
			Audio.Play();
		}
	}
}
