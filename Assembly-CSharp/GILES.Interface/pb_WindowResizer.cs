using UnityEngine;
using UnityEngine.EventSystems;

namespace GILES.Interface
{
	public class pb_WindowResizer : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler
	{
		public RectTransform window;

		private Rect screenRect = new Rect(0f, 0f, 0f, 0f);

		public Vector2 minSize = new Vector2(100f, 100f);

		public Vector2 maxSize = new Vector2(10000f, 10000f);

		public void OnBeginDrag(PointerEventData eventData)
		{
			screenRect.width = Screen.width;
			screenRect.height = Screen.height;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (window == null)
			{
				Debug.LogWarning("Window parent is null, cannot drag a null window.");
				return;
			}
			window.position += (Vector3)eventData.delta * 0.5f;
			Vector2 vector = new Vector2(eventData.delta.x, 0f - eventData.delta.y);
			window.sizeDelta += vector;
			Rect rect = window.GetScreenRect();
			float width = window.rect.width;
			float height = window.rect.height;
			if (width < minSize.x || width > maxSize.x || rect.x + rect.width > screenRect.width)
			{
				Vector2 sizeDelta = window.sizeDelta;
				sizeDelta.x -= vector.x;
				window.sizeDelta = sizeDelta;
				Vector3 position = window.position;
				position.x -= eventData.delta.x * 0.5f;
				window.position = position;
			}
			if (height < minSize.y || height > maxSize.y || rect.y - rect.height < 0f)
			{
				Vector2 sizeDelta2 = window.sizeDelta;
				sizeDelta2.y -= vector.y;
				window.sizeDelta = sizeDelta2;
				Vector3 position2 = window.position;
				position2.y -= eventData.delta.y * 0.5f;
				window.position = position2;
			}
			pb_IOnResizeHandler[] componentsInChildren = window.transform.GetComponentsInChildren<pb_IOnResizeHandler>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].OnResize();
			}
		}
	}
}
