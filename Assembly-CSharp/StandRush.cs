using EZCameraShake;
using UnityEngine;

public class StandRush : MonoBehaviour
{
	public bool StopTime;

	[ConditionalField("StopTime", null)]
	public GameObject TimeStopper;

	public GameObject Damage;

	public GameObject Effects;

	private float DamageTime;

	private Transform Player;

	private Transform Camera;

	private void Start()
	{
		Player = GameManager.GM.Player.transform;
		Camera = Player.gameObject.GetComponent<ac_CharacterController>().MainCamera.transform;
		Effects.transform.parent = Player.transform;
		Effects.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - 1f, Player.transform.position.z);
		base.gameObject.transform.parent = Camera;
		DamageTime = 0.1f;
		if (StopTime)
		{
			Object.Instantiate(TimeStopper);
		}
		Object.Destroy(Effects, 6f);
		Object.Destroy(base.gameObject, 6f);
	}

	private void Update()
	{
		DamageTime -= Time.deltaTime;
		if (DamageTime <= 0f)
		{
			DamageTime = 0.1f;
			Object.Instantiate(Damage, base.transform.position, base.transform.rotation);
			CameraShaker.Instance.DefaultRotInfluence = new Vector3(2f, 2f, 2f);
			CameraShaker.Instance.ShakeOnce(12f, 1.5f, 0.1f, 1f);
			CameraShaker.Instance.ResetCamera();
		}
	}
}
