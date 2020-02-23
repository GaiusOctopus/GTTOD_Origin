using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_PanelToggle : pb_ToolbarButton
	{
		public GameObject panel;

		private Color onColor = new Color(1f, 0.68f, 11f / 51f, 1f);

		private Color offColor = new Color(0.26f, 0.26f, 0.26f, 1f);

		protected override void Start()
		{
			base.Start();
			onColor = selectable.colors.normalColor;
			offColor = selectable.colors.disabledColor;
		}

		public void DoToggle()
		{
			panel.SetActive(!panel.activeInHierarchy);
			ColorBlock colors = selectable.colors;
			colors.normalColor = (panel.activeInHierarchy ? onColor : offColor);
			selectable.colors = colors;
		}
	}
}
