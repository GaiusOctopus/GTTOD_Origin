using UnityEngine.EventSystems;

namespace GILES.Interface
{
	public class pb_Window : UIBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
			base.transform.SetAsLastSibling();
		}
	}
}
