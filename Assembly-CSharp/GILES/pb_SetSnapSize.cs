using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GILES
{
	public class pb_SetSnapSize : pb_ToolbarButton
	{
		private Dropdown dropdown;

		private bool hideTooltips;

		public override string tooltip => "Set the snap increment";

		protected override void Start()
		{
			dropdown = GetComponent<Dropdown>();
			dropdown.value = (int)Mathf.Ceil(1f / pb_SelectionHandle.positionSnapValue) - 1;
		}

		public override void OnPointerClick(PointerEventData data)
		{
			hideTooltips = true;
			HideTooltip();
		}

		public override void OnPointerEnter(PointerEventData data)
		{
			if (!hideTooltips)
			{
				base.OnPointerEnter(data);
			}
		}

		public override void OnPointerExit(PointerEventData data)
		{
			base.OnPointerExit(data);
		}

		public void SetSnapIncrement()
		{
			hideTooltips = false;
			if (dropdown.value == 0)
			{
				pb_SelectionHandle.positionSnapValue = 1f;
			}
			else
			{
				pb_SelectionHandle.positionSnapValue = 1f / Mathf.Pow(2f, dropdown.value);
			}
		}
	}
}
