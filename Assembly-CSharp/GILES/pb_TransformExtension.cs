using UnityEngine;

namespace GILES
{
	public static class pb_TransformExtension
	{
		public static void SetTRS(this Transform transform, pb_Transform pbTransform)
		{
			transform.position = pbTransform.position;
			transform.localRotation = pbTransform.rotation;
			transform.localScale = pbTransform.scale;
		}
	}
}
