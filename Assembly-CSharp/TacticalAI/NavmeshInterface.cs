using UnityEngine;
using UnityEngine.AI;

namespace TacticalAI
{
	public class NavmeshInterface : MonoBehaviour
	{
		private NavMeshAgent agent;

		private Vector3 lastPos;

		private Vector3 returnVel;

		private Transform myTransform;

		public virtual void Initialize(GameObject gameObject)
		{
			myTransform = gameObject.GetComponent<AnimationScript>().myAIBodyTransform;
			myTransform = base.transform;
			lastPos = myTransform.position;
			if (gameObject.GetComponent<NavMeshAgent>() != null)
			{
				agent = gameObject.GetComponent<NavMeshAgent>();
			}
			else
			{
				Debug.Log("No Agent Found!");
			}
		}

		private void Update()
		{
			if (Time.timeScale > 0f)
			{
				returnVel = (myTransform.position - lastPos) / Time.deltaTime;
				lastPos = myTransform.position;
			}
		}

		public virtual void SetDestination(Vector3 v)
		{
			if (agent.enabled)
			{
				agent.SetDestination(v);
			}
		}

		public virtual bool ReachedDestination()
		{
			if (agent.enabled && agent.remainingDistance != float.PositiveInfinity && agent.remainingDistance <= 0f)
			{
				return !agent.pathPending;
			}
			return false;
		}

		public virtual bool PathPartial()
		{
			return agent.pathStatus == NavMeshPathStatus.PathPartial;
		}

		public virtual Vector3 GetDesiredVelocity()
		{
			return returnVel;
		}

		public virtual bool PathPending()
		{
			return agent.pathPending;
		}

		public virtual bool HasPath()
		{
			return agent.hasPath;
		}

		public virtual Vector3[] GetNavmeshVertices()
		{
			return NavMesh.CalculateTriangulation().vertices;
		}

		public virtual void SetSpeed(float f)
		{
			agent.speed = f;
		}

		public virtual float GetSpeed()
		{
			return agent.speed;
		}

		public virtual void SetAcceleration(float f)
		{
			agent.acceleration = f;
		}

		public virtual float GetAcceleration()
		{
			return agent.acceleration;
		}

		public virtual void SetStoppingDistance(float f)
		{
			agent.stoppingDistance = f;
		}

		public virtual float GetStoppingDistance()
		{
			return agent.stoppingDistance;
		}

		public virtual float GetRemainingDistance()
		{
			if (agent.enabled)
			{
				return agent.remainingDistance;
			}
			return 0f;
		}

		public virtual bool OnNavmeshLink()
		{
			return agent.isOnOffMeshLink;
		}

		public virtual void CompleteOffMeshLink()
		{
			agent.CompleteOffMeshLink();
		}

		public virtual void DisableAgent()
		{
			agent.enabled = false;
		}

		public virtual void EnableAgent()
		{
			agent.enabled = true;
		}

		public virtual bool HaveOffMeshLinkInPath()
		{
			return agent.nextOffMeshLinkData.valid;
		}
	}
}
