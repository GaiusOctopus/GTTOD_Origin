using GILES.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace GILES
{
	public class pb_FullInspectorButton : pb_ToolbarButton
	{
		public pb_Inspector inspector;

		private Color onColor = new Color(1f, 0.68f, 11f / 51f, 1f);

		private Color offColor = new Color(0.26f, 0.26f, 0.26f, 1f);

		public override string tooltip => "Show Full Inspector";

		protected override void Start()
		{
			base.Start();
			onColor = selectable.colors.normalColor;
			offColor = selectable.colors.disabledColor;
			UpdateColors();
		}

		public void DoToggle()
		{
			inspector.showUnityComponents = !inspector.showUnityComponents;
			inspector.RebuildInspector(pb_Selection.activeGameObject);
			UpdateColors();
		}

		private void UpdateColors()
		{
			ColorBlock colors = selectable.colors;
			colors.normalColor = (inspector.showUnityComponents ? onColor : offColor);
			selectable.colors = colors;
		}
	}
}
