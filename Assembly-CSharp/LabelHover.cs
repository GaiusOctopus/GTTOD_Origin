using UnityEngine;
using UnityEngine.EventSystems;

public class LabelHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerClickHandler
{
	public GameObject MyLabel;

	public void OnPointerEnter(PointerEventData eventData)
	{
		MyLabel.SetActive(value: true);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		MyLabel.SetActive(value: false);
	}
}
