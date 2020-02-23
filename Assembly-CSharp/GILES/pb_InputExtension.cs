using UnityEngine;

namespace GILES
{
	public static class pb_InputExtension
	{
		public static bool Shift()
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{
				return Input.GetKey(KeyCode.RightShift);
			}
			return true;
		}

		public static bool Control()
		{
			if (!Input.GetKey(KeyCode.LeftControl))
			{
				return Input.GetKey(KeyCode.RightControl);
			}
			return true;
		}
	}
}
