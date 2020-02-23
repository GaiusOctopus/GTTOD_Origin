using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClicks : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public AudioSource clickSource;

	public AudioClip hoverSound;

	public AudioClip clickSound;

	public void OnDrag(PointerEventData eventData)
	{
		clickSource.PlayOneShot(hoverSound);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		clickSource.PlayOneShot(clickSound);
	}
}
