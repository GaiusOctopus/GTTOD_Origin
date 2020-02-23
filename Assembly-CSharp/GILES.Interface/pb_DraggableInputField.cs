using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_DraggableInputField : InputField
	{
		private bool isDraggingValue;

		private float value;

		public override void OnBeginDrag(PointerEventData eventData)
		{
			if ((base.contentType == ContentType.IntegerNumber || base.contentType == ContentType.DecimalNumber) && IsInteractable() && (eventData.button == PointerEventData.InputButton.Right || (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftAlt))))
			{
				if (!float.TryParse(m_TextComponent.text, out value))
				{
					value = 0f;
				}
				isDraggingValue = true;
			}
			else
			{
				base.OnBeginDrag(eventData);
			}
		}

		public override void OnDrag(PointerEventData eventData)
		{
			if (!isDraggingValue)
			{
				base.OnDrag(eventData);
				return;
			}
			float x = eventData.delta.x;
			float y = eventData.delta.y;
			value += ((Mathf.Abs(x) > Mathf.Abs(y)) ? x : y) / 10f;
			base.text = ((base.contentType == ContentType.DecimalNumber) ? value.ToString("g") : ((int)value).ToString());
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			isDraggingValue = false;
			base.OnEndDrag(eventData);
		}
	}
}
