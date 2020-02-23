using UnityEngine;

namespace GILES
{
	[pb_Gizmo(typeof(Camera))]
	public class pb_Gizmo_Camera : pb_Gizmo
	{
		private void Start()
		{
			icon = pb_BuiltinResource.GetMaterial("Gizmos/Camera");
		}
	}
}
