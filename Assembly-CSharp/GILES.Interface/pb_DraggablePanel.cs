using UnityEngine;
using UnityEngine.EventSystems;

namespace GILES.Interface
{
	public class pb_DraggablePanel : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
	{
		private Rect screenRect = new Rect(0f, 0f, 0f, 0f);

		public RectTransform windowParent;

		public void OnBeginDrag(PointerEventData eventData)
		{
			screenRect.width = Screen.width;
			screenRect.height = Screen.height;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (windowParent == null)
			{
				Debug.LogWarning("Window parent is null, cannot drag a null window.");
			}
			else if (screenRect.Contains(eventData.position))
			{
				windowParent.position += (Vector3)eventData.delta;
			}
		}
	}
}
