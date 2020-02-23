using System.Collections;
using UnityEngine;

public class TutorialStarter : MonoBehaviour
{
	[Header("Abyss Obejcts")]
	public GameObject Tutorials;

	public GameObject SFX;

	[Header("Game Obejcts")]
	public Camera MainCam;

	public Animation CameraEffects;

	public AudioClip SuccessfulLandingSFX;

	public AudioClip FailedLandingSFX;

	public Transform EndPosition;

	public FOVManager FOV;

	public GameObject ScienceClubPickup;

	private ac_CharacterController Player;

	private AudioSource Audio;

	private Animation Anim;

	private void Update()
	{
		if (Time.timeScale < 1f)
		{
			Audio.pitch = Time.timeScale;
		}
	}

	public void SetLandingType(bool Successful)
	{
		Player = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		Audio = GetComponent<AudioSource>();
		Anim = GetComponent<Animation>();
		FixCamera();
		base.transform.position = new Vector3(0f, 0.35f, 0f);
		base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		if (Successful)
		{
			Audio.clip = SuccessfulLandingSFX;
			Audio.Play();
			Anim.Play("SuccessfulLanding");
			Tutorials.SetActive(value: false);
			SFX.SetActive(value: true);
			ScienceClubPickup.SetActive(value: false);
			StartCoroutine(StartAbyss(4f, RemoveAbilities: false));
		}
		else
		{
			Audio.clip = FailedLandingSFX;
			Audio.Play();
			Anim.Play("FailedLanding");
			Tutorials.SetActive(value: true);
			ScienceClubPickup.SetActive(value: true);
			StartCoroutine(StartAbyss(27f, RemoveAbilities: true));
		}
	}

	public IEnumerator StartAbyss(float TimeToStart, bool RemoveAbilities)
	{
		GameManager.GM.GetComponent<MenuScript>().HUDElements.SetActive(value: false);
		yield return new WaitForSeconds(TimeToStart);
		SFX.SetActive(value: true);
		Player.CutsceneEnd(EndPosition);
		CameraEffects.Play("CameraOn");
		CameraEffects.gameObject.GetComponent<AudioSource>().Play();
		base.gameObject.SetActive(value: false);
		GameManager.GM.GetComponent<MenuScript>().HUDElements.SetActive(value: true);
		if (RemoveAbilities)
		{
			Player.RemoveAbilities();
		}
	}

	public void FixCamera()
	{
		MainCam.fieldOfView = FOV.NormalFOV;
	}
}
