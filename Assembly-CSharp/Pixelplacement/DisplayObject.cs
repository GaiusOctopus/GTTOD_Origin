using UnityEngine;

namespace Pixelplacement
{
	[RequireComponent(typeof(Initialization))]
	public class DisplayObject : MonoBehaviour
	{
		private bool _activated;

		public void Register()
		{
			if (!_activated)
			{
				_activated = true;
				base.gameObject.SetActive(value: false);
			}
		}

		public void SetActive(bool value)
		{
			_activated = true;
			base.gameObject.SetActive(value);
		}

		public void Solo()
		{
			if (base.transform.parent != null)
			{
				foreach (Transform item in base.transform.parent)
				{
					if (!(item == base.transform))
					{
						DisplayObject component = item.GetComponent<DisplayObject>();
						if (component != null)
						{
							component.SetActive(value: false);
						}
					}
				}
				base.gameObject.SetActive(value: true);
				return;
			}
			DisplayObject[] array = Resources.FindObjectsOfTypeAll<DisplayObject>();
			foreach (DisplayObject displayObject in array)
			{
				if (displayObject.transform.parent == null)
				{
					if (displayObject == this)
					{
						displayObject.SetActive(value: true);
					}
					else
					{
						displayObject.SetActive(value: false);
					}
				}
			}
		}

		public void HideAll()
		{
			if (base.transform.parent != null)
			{
				foreach (Transform item in base.transform.parent)
				{
					if (item.GetComponent<DisplayObject>() != null)
					{
						item.gameObject.SetActive(value: false);
					}
				}
				return;
			}
			DisplayObject[] array = Resources.FindObjectsOfTypeAll<DisplayObject>();
			foreach (DisplayObject displayObject in array)
			{
				if (displayObject.transform.parent == null)
				{
					displayObject.gameObject.SetActive(value: false);
				}
			}
		}
	}
}
