using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentJumpToTarget : MonoBehaviour
{
	public NavMeshAgent NavMeshAgent;

	public Rigidbody Rigidbody;

	public GameObject Target;

	public float ReachedStartPointDistance = 0.5f;

	public Transform DummyAgent;

	public Vector3 EndJumpPosition;

	public float MaxJumpableDistance = 80f;

	public float JumpTime = 0.6f;

	public float AddToJumpHeight;

	private Transform _dummyAgent;

	public Vector3 JumpStartPoint;

	private Vector3 JumpMidPoint;

	private Vector3 JumpEndPoint;

	private bool checkForStartPointReached;

	private Transform _transform;

	private List<Vector3> Path = new List<Vector3>();

	private float JumpDistance;

	private Vector3[] _jumpPath;

	private bool previousRigidBodyState;

	public void GetStartPointAndMoveToPosition()
	{
		JumpStartPoint = GetJumpStartPoint();
		MoveToStartPoint();
	}

	public void PerformJump()
	{
		SpawnAgentAndGetPoint();
	}

	private void OnEnable()
	{
		checkForStartPointReached = false;
		_transform = base.transform;
	}

	private Vector3 GetJumpStartPoint()
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		NavMeshAgent.CalculatePath(Target.transform.position, navMeshPath);
		int num = navMeshPath.corners.Length - 1;
		return navMeshPath.corners[num];
	}

	private void MoveToStartPoint()
	{
		checkForStartPointReached = true;
		NavMeshAgent.isStopped = false;
		NavMeshAgent.SetDestination(JumpStartPoint);
	}

	private void ReadyToJump()
	{
	}

	private void SpawnAgentAndGetPoint()
	{
		_dummyAgent = Object.Instantiate(DummyAgent, Target.transform.position, Quaternion.identity);
		ReturnNavmeshInfo component = _dummyAgent.GetComponent<ReturnNavmeshInfo>();
		EndJumpPosition = component.ReturnClosestPointBackToAgent(base.transform.position);
		JumpEndPoint = EndJumpPosition;
		MakeJumpPath();
	}

	private void MakeJumpPath()
	{
		Path.Add(JumpStartPoint);
		Vector3 item = Vector3.Lerp(JumpStartPoint, JumpEndPoint, 0.5f);
		item.y = item.y + NavMeshAgent.height + AddToJumpHeight;
		Path.Add(item);
		Path.Add(JumpEndPoint);
		JumpDistance = Vector3.Distance(JumpStartPoint, JumpEndPoint);
		if (JumpDistance <= MaxJumpableDistance)
		{
			DoJump();
		}
		else
		{
			Debug.Log("Too far to jump");
		}
	}

	private void DoJump()
	{
		previousRigidBodyState = Rigidbody.isKinematic;
		NavMeshAgent.enabled = false;
		Rigidbody.isKinematic = true;
		_jumpPath = Path.ToArray();
		Rigidbody.DOLocalPath(_jumpPath, JumpTime, PathType.CatmullRom).OnComplete(JumpFinished);
	}

	private void JumpFinished()
	{
		NavMeshAgent.enabled = true;
		Rigidbody.isKinematic = previousRigidBodyState;
		Object.Destroy(_dummyAgent.gameObject);
	}

	private void Update()
	{
		if (checkForStartPointReached && (_transform.position - JumpStartPoint).sqrMagnitude <= ReachedStartPointDistance * ReachedStartPointDistance)
		{
			ReadyToJump();
			if (NavMeshAgent.isOnNavMesh)
			{
				NavMeshAgent.isStopped = true;
			}
			checkForStartPointReached = false;
		}
	}
}
