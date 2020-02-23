using UnityEngine;

public class ExplosionTransfer : MonoBehaviour
{
	public GameObject Base;

	public void Damage(float Damage)
	{
		Base.SendMessage("Damage", Damage, SendMessageOptions.DontRequireReceiver);
	}

	public void Stagger()
	{
		Base.SendMessage("Stagger", SendMessageOptions.DontRequireReceiver);
	}

	public void RagdollAgent()
	{
		Base.SendMessage("Ragdoll", SendMessageOptions.DontRequireReceiver);
	}

	public void DodgeRoll()
	{
		Base.SendMessage("DodgeRoll", SendMessageOptions.DontRequireReceiver);
	}

	public void Die()
	{
	}
}
