using System.Collections;
using UnityEngine;

public class RigidbodyObject : MonoBehaviour
{
	public bool CanInteract;

	[ConditionalField("CanInteract", null)]
	public GameObject InteractionObject;

	[ConditionalField("CanInteract", null)]
	public GameObject HitEffect;

	[ConditionalField("CanInteract", null)]
	public float Health = 3f;

	[ConditionalField("CanInteract", null)]
	public float RequiredForce;

	public AudioClip[] Clips;

	private bool Active;

	private bool Grabbed;

	private bool Broken;

	private bool Frozen;

	private float Distance;

	private float HealthSave;

	private Rigidbody MyPhysics;

	private MeshRenderer MyRenderer;

	private BoxCollider MyCollider;

	private AudioSource MyAudio;

	private Transform Player;

	private Transform PhysicsHook;

	private Rigidbody PlayerPhysics;

	private ac_CharacterController CharacterController;

	private InventoryScript Inventory;

	private RaycastAim Raycaster;

	private GameManager GM;

	public void Start()
	{
		MyPhysics = GetComponent<Rigidbody>();
		MyRenderer = GetComponent<MeshRenderer>();
		MyCollider = GetComponent<BoxCollider>();
		MyAudio = GetComponent<AudioSource>();
		HealthSave = Health;
		Player = GameManager.GM.Player.transform;
		CharacterController = Player.GetComponent<ac_CharacterController>();
		Inventory = Player.GetComponent<InventoryScript>();
		PlayerPhysics = Player.GetComponent<Rigidbody>();
		PhysicsHook = CharacterController.PhysicsHook;
		Raycaster = CharacterController.AimScript;
		GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 2f && CanInteract)
		{
			int num = Random.Range(0, Clips.Length);
			MyAudio.clip = Clips[num];
			MyAudio.Play();
			if (CanInteract && collision.relativeVelocity.magnitude > RequiredForce && !Broken)
			{
				Break();
			}
		}
	}

	private void Update()
	{
		if (Active && CanInteract)
		{
			if (Raycaster.MyLookedAtObject != base.gameObject)
			{
				Stable();
			}
			Distance = Vector3.Distance(base.transform.position, Player.position);
			if (Distance < 3f && KeyBindingManager.GetKeyDown(KeyAction.Interact))
			{
				MyPhysics.useGravity = false;
				MyPhysics.drag = 1f;
				MyPhysics.constraints = RigidbodyConstraints.FreezeRotation;
				base.gameObject.layer = 2;
				base.transform.parent = PhysicsHook;
				Grabbed = true;
			}
		}
		if (Grabbed)
		{
			if (Inventory.CurrentWeapon != null)
			{
				Inventory.CurrentWeapon.LowerWeapon();
			}
			Inventory.IsHolding = true;
			base.transform.position = Vector3.Lerp(base.transform.position, PhysicsHook.position, 0.1f);
			if (Input.GetKey(KeyCode.Mouse0))
			{
				StartCoroutine(Throw());
			}
			if (Input.GetKey(KeyCode.Mouse1))
			{
				StartCoroutine(Drop());
			}
		}
	}

	public IEnumerator Throw()
	{
		Grabbed = false;
		MyPhysics.useGravity = true;
		MyPhysics.drag = 0f;
		MyPhysics.constraints = RigidbodyConstraints.None;
		base.gameObject.layer = 0;
		base.transform.parent = null;
		MyPhysics.velocity = PlayerPhysics.velocity;
		MyPhysics.AddForce(PhysicsHook.forward * 750f);
		yield return new WaitForSeconds(0.15f);
		if (Inventory.CurrentWeapon != null)
		{
			Inventory.CurrentWeapon.RaiseWeapon();
		}
		yield return new WaitForSeconds(0.5f);
		Inventory.IsHolding = false;
	}

	public IEnumerator Drop()
	{
		Grabbed = false;
		MyPhysics.useGravity = true;
		MyPhysics.drag = 0f;
		MyPhysics.constraints = RigidbodyConstraints.None;
		base.gameObject.layer = 0;
		base.transform.parent = null;
		MyPhysics.velocity = PlayerPhysics.velocity;
		yield return new WaitForSeconds(0.15f);
		if (Inventory.CurrentWeapon != null)
		{
			Inventory.CurrentWeapon.RaiseWeapon();
		}
		yield return new WaitForSeconds(0.5f);
		Inventory.IsHolding = false;
	}

	public void Interact()
	{
		if (!Frozen && CanInteract)
		{
			Health -= 1f;
			Object.Instantiate(HitEffect, base.transform.position, base.transform.rotation);
			if (Health <= 0f && !Broken)
			{
				Break();
				Broken = true;
			}
		}
	}

	public void Interacting()
	{
		if (!Frozen && CanInteract)
		{
			Active = true;
		}
	}

	public void Stable()
	{
		Active = false;
	}

	public void Damage(float Damage)
	{
		if (!Frozen && CanInteract)
		{
			Health -= 1f;
			Object.Instantiate(HitEffect, base.transform.position, base.transform.rotation);
			if (Health <= 0f && !Broken)
			{
				Break();
				Broken = true;
			}
		}
	}

	public void Break()
	{
		if (!Frozen && CanInteract)
		{
			Object.Instantiate(InteractionObject, base.transform.position, base.transform.rotation);
			Inventory.IsHolding = false;
			CanInteract = false;
			MyRenderer.enabled = false;
			MyCollider.enabled = false;
			MyPhysics.isKinematic = true;
			base.transform.GetChild(0).gameObject.SetActive(value: false);
		}
	}

	public void Freeze()
	{
		if (CanInteract)
		{
			Frozen = true;
			MyPhysics.isKinematic = true;
			Active = false;
		}
	}

	public void UnFreeze()
	{
		if (CanInteract)
		{
			Frozen = false;
			MyPhysics.isKinematic = false;
		}
	}

	public void ResetFuntionality()
	{
		Health = HealthSave;
		Broken = false;
		CanInteract = true;
		MyRenderer.enabled = true;
		MyCollider.enabled = true;
		MyPhysics.isKinematic = false;
		base.transform.GetChild(0).gameObject.SetActive(value: true);
	}
}
