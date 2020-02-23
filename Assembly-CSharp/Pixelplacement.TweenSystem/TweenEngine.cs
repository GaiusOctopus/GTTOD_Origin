using System.Collections;
using UnityEngine;

namespace Pixelplacement.TweenSystem
{
	public class TweenEngine : MonoBehaviour
	{
		public void ExecuteTween(TweenBase tween)
		{
			StartCoroutine(RunTween(tween));
		}

		private static IEnumerator RunTween(TweenBase tween)
		{
			Tween.activeTweens.Add(tween);
			while (tween.Tick())
			{
				yield return null;
			}
			Tween.activeTweens.Remove(tween);
		}
	}
}
