using UnityEngine;

namespace TacticalAI
{
	public class SoundMaker : MonoBehaviour
	{
		public float radius = 40f;

		public int[] teamsThatShouldHear;

		public int delayTime = 1;

		private void Start()
		{
			ControllerScript.currentController.CreateSound(base.transform.position, radius, teamsThatShouldHear);
		}
	}
}
