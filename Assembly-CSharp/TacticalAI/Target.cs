using UnityEngine;

namespace TacticalAI
{
	public class Target
	{
		public int uniqueIdentifier;

		public int teamID;

		public Transform transform;

		public TargetScript targetScript;

		public Target(int identity, int id, Transform transformToAdd, TargetScript script)
		{
			uniqueIdentifier = identity;
			teamID = id;
			transform = transformToAdd;
			targetScript = script;
		}
	}
}
