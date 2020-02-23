using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
	public AudioSource Audio;

	public Rigidbody ShellPhysics;

	public float MinPitch = 0.9f;

	public float MaxPitch = 1.25f;

	[Header("Shell Properties")]
	public bool RandomRotation = true;

	public float UpMin = 1.5f;

	public float UpMax = 2.5f;

	private float SoundPitch;

	private float CountTime = 4f;

	private ac_CharacterController CharacterController;

	private void Awake()
	{
		SoundPitch = Random.Range(MinPitch, MaxPitch);
	}

	public void Fire(AudioClip SoundEffect, Transform Ejector, Vector3 NewVelocity)
	{
		if (CharacterController == null)
		{
			CharacterController = GameManager.GM.Player.GetComponent<ac_CharacterController>();
		}
		ShellPhysics.isKinematic = false;
		CountTime = 4f;
		base.transform.position = Ejector.position;
		base.transform.rotation = Ejector.rotation;
		ShellPhysics.transform.localPosition = new Vector3(0f, 0f, 0f);
		if (RandomRotation)
		{
			ShellPhysics.transform.rotation = ShellPhysics.transform.rotation * new Quaternion(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
		}
		else
		{
			ShellPhysics.transform.rotation = Ejector.rotation;
		}
		ShellPhysics.velocity = NewVelocity + Ejector.transform.right * Random.Range(3f, 5f) + Ejector.transform.up * Random.Range(UpMin, UpMax);
		ShellPhysics.AddTorque(base.transform.right * -5000f);
		Audio.clip = SoundEffect;
		Audio.Play();
		if (CharacterController.Swimming)
		{
			ShellPhysics.useGravity = false;
			ShellPhysics.drag = 1f;
			ShellPhysics.angularDrag = 1f;
		}
		else
		{
			ShellPhysics.useGravity = true;
			ShellPhysics.drag = 0f;
			ShellPhysics.angularDrag = 0f;
		}
	}

	private void Update()
	{
		if (!ShellPhysics.isKinematic)
		{
			CountTime -= Time.deltaTime;
			if (CountTime <= 0f)
			{
				ShellPhysics.isKinematic = true;
			}
		}
		if (Time.timeScale != 1f)
		{
			Audio.pitch = Time.timeScale;
		}
		else
		{
			Audio.pitch = SoundPitch;
		}
	}
}
