using UnityEngine;

namespace TacticalAI
{
	public class TeamChanger : MonoBehaviour
	{
		public TargetScript targetScriptToChange;

		public float timeToWait = 5f;

		public int newTeam = 10;

		private void Update()
		{
			timeToWait -= Time.deltaTime;
			if (timeToWait < 0f)
			{
				targetScriptToChange.SetNewTeam(newTeam);
				base.enabled = false;
			}
		}
	}
}
