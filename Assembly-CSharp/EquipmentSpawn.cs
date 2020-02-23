using UnityEngine;

public class EquipmentSpawn : MonoBehaviour
{
	public Material ActiveMaterial;

	[Header("Equipment Stats")]
	public GameObject EquipmentObject;

	public string ObjectPoint;

	public string AnimationName;

	public float ShakeMagnitude;

	public float ActivationTime;

	public float DeactivationTime;

	public float Cooldown;

	public AudioClip SoundEffect;

	[Header("Private Variables")]
	private GameManager GM;

	private GameObject Player;

	private MeshRenderer Renderer;

	private Material BaseMaterial;

	private bool Active;

	private bool WithinRange;

	private void Start()
	{
		GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
		Renderer = GetComponent<MeshRenderer>();
		BaseMaterial = Renderer.material;
	}

	private void Update()
	{
		if (Active)
		{
			Renderer.material = ActiveMaterial;
		}
		else
		{
			Renderer.material = BaseMaterial;
		}
		if (WithinRange && Active && KeyBindingManager.GetKey(KeyAction.Interact))
		{
			GiveWeapon();
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			WithinRange = true;
			Player = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			WithinRange = false;
		}
	}

	public void Interacting()
	{
		Active = true;
	}

	public void Stable()
	{
		Active = false;
	}

	private void GiveWeapon()
	{
	}
}
