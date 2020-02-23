using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_GUIStyleApplier : MonoBehaviour
	{
		public bool ignoreStyle;

		public pb_GUIStyle style;

		private void Awake()
		{
			if (!ignoreStyle)
			{
				ApplyStyle();
			}
		}

		public void ApplyStyle()
		{
			if (!(style == null))
			{
				ApplyRecursive(base.gameObject);
			}
		}

		private void ApplyRecursive(GameObject go)
		{
			Graphic[] components = go.GetComponents<Graphic>();
			foreach (Graphic element in components)
			{
				style.Apply(element);
			}
			Selectable[] components2 = go.GetComponents<Selectable>();
			foreach (Selectable element2 in components2)
			{
				style.Apply(element2);
			}
			foreach (Transform item in go.transform)
			{
				if (!(item.gameObject.GetComponent<pb_GUIStyleApplier>() != null))
				{
					ApplyRecursive(item.gameObject);
				}
			}
		}
	}
}
