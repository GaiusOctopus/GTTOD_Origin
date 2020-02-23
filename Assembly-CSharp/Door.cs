using UnityEngine;

public class Door : MonoBehaviour
{
	public bool ExitDoor;

	public bool Fade;

	[ConditionalField("Fade", null)]
	public Transform MySpot;

	[ConditionalField("Fade", null)]
	public Material DoorMaterial;

	[ConditionalField("Fade", null)]
	public bool TriggerMode;

	[HideInInspector]
	public bool Locked;

	private GTTODManager Manager;

	private AudioSource Audio;

	private float Color = 1f;

	private Vector4 GlowColor;

	private void Start()
	{
		if (GameManager.GM.isActiveAndEnabled)
		{
			Manager = GameManager.GM.GetComponent<GTTODManager>();
		}
		if (Fade)
		{
			Audio = GetComponent<AudioSource>();
		}
	}

	public void Bump()
	{
		Color = 1f;
	}

	private void Update()
	{
		if (Fade && Color > 0f)
		{
			Color -= Time.deltaTime / 4f;
			GlowColor = new Vector4(Color * 3.5f, Color * 0.25f, 0f, 1f);
			DoorMaterial.SetColor("_Color", new Vector4(2f, 1f, 1f, 1f));
			DoorMaterial.SetColor("_EmissionColor", GlowColor);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && !Locked)
		{
			if (ExitDoor)
			{
				Manager.EnterDoor(ExitLevel: true);
			}
			else
			{
				Manager.EnterDoor(ExitLevel: false);
			}
		}
	}
}
