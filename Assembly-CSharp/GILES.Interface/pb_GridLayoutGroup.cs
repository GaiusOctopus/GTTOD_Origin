using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_GridLayoutGroup : GridLayoutGroup
	{
		public Vector2 elementSize = new Vector2(100f, 50f);

		public bool maintainAspectRatio = true;

		protected override void Start()
		{
			base.Start();
		}
	}
}
