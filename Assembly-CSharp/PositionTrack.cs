using UnityEngine;

public class PositionTrack : MonoBehaviour
{
	public Transform Object;

	public GameObject HealEffect;

	private float TimeToHeal = 1f;

	private HealthScript PlayerHealth;

	private void Start()
	{
		base.transform.parent = null;
		PlayerHealth = GameManager.GM.Player.GetComponent<HealthScript>();
		if (!PlayerHealth.CanRegen)
		{
			PlayerHealth.CanRegen = true;
		}
	}

	private void Update()
	{
		if (Object != null)
		{
			base.transform.position = Object.position;
		}
		base.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		if ((Vector3.Distance(base.transform.position, PlayerHealth.transform.position) <= 5f && PlayerHealth.Health < 100f) || (Vector3.Distance(base.transform.position, PlayerHealth.transform.position) <= 5f && PlayerHealth.Shield < 100f))
		{
			TimeToHeal -= Time.deltaTime;
			if (TimeToHeal <= 0f)
			{
				TimeToHeal = 1f;
				PlayerHealth.Heal(10);
				UnityEngine.Object.Instantiate(HealEffect, PlayerHealth.transform.position, Quaternion.identity);
			}
		}
	}
}
