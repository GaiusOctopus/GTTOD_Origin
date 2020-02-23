using GILES.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GILES
{
	public class pb_ToolbarButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		protected Selectable selectable;

		protected GameObject tooltip_label;

		public virtual string tooltip => "";

		public bool interactable
		{
			get
			{
				return selectable.interactable;
			}
			set
			{
				selectable.interactable = value;
			}
		}

		protected virtual void Start()
		{
			selectable = GetComponent<Selectable>();
		}

		public virtual void OnPointerClick(PointerEventData eventData)
		{
			UpdateTooltip();
		}

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			ShowTooltip();
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
			HideTooltip();
		}

		public void ShowTooltip()
		{
			string tooltip = this.tooltip;
			if (!string.IsNullOrEmpty(tooltip))
			{
				tooltip_label = pb_GUIUtility.CreateLabel(tooltip);
				float preferredWidth = tooltip_label.GetComponent<Text>().preferredWidth;
				tooltip_label.GetComponent<RectTransform>().sizeDelta = new Vector2(preferredWidth, 30f);
				tooltip_label.transform.SetParent(Object.FindObjectOfType<Canvas>().transform);
				if (base.transform.position.x + preferredWidth < (float)Screen.width)
				{
					tooltip_label.transform.position = new Vector3(base.transform.position.x + preferredWidth * 0.5f, base.transform.position.y - 30f, 0f);
				}
				else
				{
					tooltip_label.transform.position = new Vector3(base.transform.position.x - preferredWidth * 0.5f, base.transform.position.y - 30f, 0f);
				}
			}
		}

		public void UpdateTooltip()
		{
			if (tooltip_label != null)
			{
				if (string.IsNullOrEmpty(tooltip))
				{
					Object.Destroy(tooltip_label);
				}
				else
				{
					tooltip_label.GetComponent<Text>().text = tooltip;
				}
			}
		}

		public void HideTooltip()
		{
			if (tooltip_label != null)
			{
				Object.Destroy(tooltip_label);
			}
		}
	}
}
