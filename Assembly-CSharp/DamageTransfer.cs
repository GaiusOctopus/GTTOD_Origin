using UnityEngine;

public class DamageTransfer : MonoBehaviour
{
	public GameObject Base;

	public float DamageModifier = 2f;

	public bool Ragdoll;

	public bool Arm;

	[ConditionalField("Arm", null)]
	public GameObject ArmObject;

	[ConditionalField("Arm", null)]
	public Transform ArmToDismember;

	public bool Leg;

	[ConditionalField("Leg", null)]
	public GameObject LegObject;

	[ConditionalField("Leg", null)]
	public Transform LegToDismember;

	private Infantry Agent;

	private void Start()
	{
		if (Ragdoll)
		{
			Agent = Base.GetComponent<Infantry>();
		}
	}

	public void Damage(float Damage)
	{
		Base.SendMessage("Damage", Damage * DamageModifier, SendMessageOptions.DontRequireReceiver);
		if (Ragdoll && Random.Range(-1f, 1f) > 0.25f)
		{
			Agent.Ragdoll();
		}
		if (Arm || Leg)
		{
			if (Arm && Random.Range(-1f, 1f) > -0.5f)
			{
				Object.Instantiate(ArmObject, ArmToDismember.position, ArmToDismember.rotation);
				ArmToDismember.localScale = Vector3.zero;
				Stagger();
			}
			if (Leg && Random.Range(-1f, 1f) > 0.35f)
			{
				Object.Instantiate(LegObject, LegToDismember.position, LegToDismember.rotation);
				LegToDismember.localScale = Vector3.zero;
				KnockDown();
			}
		}
	}

	public void Stagger()
	{
		Base.SendMessage("Stagger", SendMessageOptions.DontRequireReceiver);
	}

	public void KnockDown()
	{
		Base.SendMessage("KnockDown", SendMessageOptions.DontRequireReceiver);
	}

	public void RagdollAgent()
	{
		Base.SendMessage("Ragdoll", SendMessageOptions.DontRequireReceiver);
	}
}
