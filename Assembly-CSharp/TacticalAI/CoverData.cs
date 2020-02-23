using UnityEngine;

namespace TacticalAI
{
	public class CoverData
	{
		public bool foundCover;

		public Vector3 hidingPosition;

		public Vector3 firingPosition;

		public bool isDynamicCover;

		public CoverNodeScript coverNodeScript;

		public CoverData(bool f, Vector3 hp, Vector3 fP, bool d, CoverNodeScript cns)
		{
			foundCover = f;
			hidingPosition = hp;
			firingPosition = fP;
			isDynamicCover = d;
			coverNodeScript = cns;
		}

		public CoverData()
		{
			foundCover = false;
		}
	}
}
