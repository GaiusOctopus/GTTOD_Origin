using UnityEngine;
using UnityEngine.AI;

public class ReturnNavmeshInfo : MonoBehaviour
{
	private NavMeshAgent agent;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	public bool IsOnNavMesh()
	{
		if (agent.isOnNavMesh)
		{
			return true;
		}
		return false;
	}

	public Vector3 ReturnClosestPointBackToAgent(Vector3 agentPosition)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		agent.CalculatePath(agentPosition, navMeshPath);
		int num = navMeshPath.corners.Length - 1;
		return navMeshPath.corners[num];
	}
}
