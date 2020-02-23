using EZCameraShake;
using UnityEngine;

public class SpeedEffects : MonoBehaviour
{
	public Transform SpeedLines;

	public Animator WeaponAnimator;

	public FOVManager FOVManager;

	public GameObject SlideEffects;

	public GameObject SuddenStop;

	public GameObject GroundSlide;

	private Transform Player;

	private Rigidbody PlayerPhysics;

	private ac_CharacterController CharacterController;

	private AudioSource Wind;

	private AudioSource Slide;

	private ParticleSystem SlideParticles;

	private AudioSource SlideSounds;

	private Vector3 LastPos;

	private Vector3 Direction;

	private Vector3 Velocity;

	private RaycastHit Hit;

	private LayerMask Layers;

	private float CheckTime = 0.1f;

	private float TimeToShake = 0.1f;

	private bool isPlaying;

	private bool SoundOn = true;

	private bool ParticlesOn = true;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
		PlayerPhysics = Player.GetComponent<Rigidbody>();
		CharacterController = Player.GetComponent<ac_CharacterController>();
		Wind = GetComponent<AudioSource>();
		Slide = SlideEffects.GetComponent<AudioSource>();
		LastPos = Player.position;
		Layers = CharacterController.AvailableLayers;
		SlideParticles = SlideEffects.GetComponent<ParticleSystem>();
		SlideSounds = SlideEffects.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Player.gameObject.activeInHierarchy)
		{
			if (!CharacterController.OnWall)
			{
				Velocity = PlayerPhysics.velocity;
			}
			else
			{
				Velocity = CharacterController.ParkourPlayer.PlayerPhysics.velocity;
			}
			if (Velocity.magnitude >= 25f)
			{
				if (!isPlaying)
				{
					SpeedLines.gameObject.SetActive(value: true);
					isPlaying = true;
				}
				base.transform.position = Player.position;
				Direction = Player.position - LastPos;
				if (Direction != Vector3.zero)
				{
					base.transform.forward = Direction;
				}
				if (Wind.volume < 1f)
				{
					Wind.volume += Time.deltaTime;
				}
				LastPos = Player.position;
			}
			else
			{
				if (isPlaying)
				{
					SpeedLines.gameObject.SetActive(value: false);
					isPlaying = false;
				}
				if (Wind.volume > 0f)
				{
					Wind.volume -= Time.deltaTime;
				}
			}
			if (Velocity.magnitude > 50f)
			{
				FOVManager.SafeZoom(35f);
				WeaponAnimator.speed = 10f;
				TimeToShake -= Time.deltaTime;
				if (TimeToShake <= 0f)
				{
					Shake();
				}
			}
			else
			{
				FOVManager.SafeUnZoom();
				WeaponAnimator.speed = 1f;
			}
			if (CharacterController.isSliding && Physics.Raycast(new Vector3(Player.position.x, Player.position.y + 1f, Player.position.z + 1f), Player.transform.up * -1f, out Hit, 3f, Layers))
			{
				SlideEffects.transform.position = new Vector3(Player.position.x, Player.position.y - 0.5f, Player.position.z);
				if (!ParticlesOn)
				{
					ParticlesOn = true;
				}
				if (!SoundOn)
				{
					SoundOn = true;
					SlideSounds.volume = 1f;
					Object.Instantiate(GroundSlide, new Vector3(Player.position.x, Player.position.y - 0.5f, Player.position.z), Player.transform.rotation);
					CharacterController.StartGroundSlide();
				}
			}
			else
			{
				SlideEffects.transform.position = new Vector3(-1000f, -1000f, -1000f);
				if (ParticlesOn)
				{
					ParticlesOn = false;
				}
				if (SoundOn)
				{
					SoundOn = false;
					SlideSounds.volume = 0f;
					if (CharacterController.isCrouching && Physics.Raycast(new Vector3(Player.position.x, Player.position.y + 0.25f, Player.position.z), Player.transform.up * -1f, out Hit, 1f, Layers))
					{
						Object.Instantiate(SuddenStop, new Vector3(Player.position.x, Player.position.y - 0.5f, Player.position.z), Player.transform.rotation);
						CharacterController.MovementShake();
					}
				}
			}
		}
		else
		{
			SlideSounds.volume = 0f;
			SlideEffects.transform.position = new Vector3(-1000f, -1000f, -1000f);
		}
		if (Time.timeScale < 1f)
		{
			Wind.pitch = Time.timeScale;
			Slide.pitch = Time.timeScale;
		}
		else if (Wind.pitch != 1f || Slide.pitch != 1f)
		{
			Wind.pitch = 1f;
			Slide.pitch = 1f;
		}
	}

	private void Shake()
	{
		TimeToShake = Random.Range(0.1f, 0.35f);
		float magnitude = Random.Range(8f, 16f);
		CameraShaker.Instance.DefaultPosInfluence = new Vector3(0.5f, 0.1f, 0.5f);
		CameraShaker.Instance.DefaultRotInfluence = new Vector3(1f, 1f, 1f);
		CameraShaker.Instance.ShakeOnce(magnitude, 2f, TimeToShake, TimeToShake);
		CameraShaker.Instance.ResetCamera();
	}
}
