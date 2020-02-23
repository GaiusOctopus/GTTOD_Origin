using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
	private Transform player;

	public float playerHealth;

	private float enemyHealth;

	private NavMeshAgent nav;

	private Animator controller;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		nav = GetComponent<NavMeshAgent>();
		controller = GetComponentInParent<Animator>();
	}

	private void Update()
	{
		if (playerHealth > 0f)
		{
			nav.SetDestination(player.position);
		}
		else
		{
			nav.enabled = false;
		}
		controller.SetFloat("speed", Mathf.Abs(nav.velocity.x) + Mathf.Abs(nav.velocity.z));
	}
}
