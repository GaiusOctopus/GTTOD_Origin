using UnityEngine;

namespace GILES.Interface
{
	public static class pb_EditorStyles
	{
		public static pb_GUIStyle ButtonStyle = pb_GUIStyle.Create(Color.white, new Color(0.2f, 0.2f, 0.2f, 0.7f), new Color(0.27f, 0.27f, 0.27f, 1f), new Color(0.37f, 0.37f, 0.37f, 1f), new Color(0.7f, 0.7f, 0.7f, 1f), null, null, null, Color.white);

		public static pb_GUIStyle PanelStyle = pb_GUIStyle.Create(new Color(0.17f, 0.17f, 0.17f, 0.5f), new Color(0.17f, 0.17f, 0.17f, 0.17f), new Color(0.17f, 0.17f, 0.17f, 0.17f), new Color(0.17f, 0.17f, 0.17f, 0.17f), new Color(0.17f, 0.17f, 0.17f, 0.17f), null, null, null, Color.white);

		public static pb_GUIStyle TitleBarStyle = pb_GUIStyle.Create(new Color(0.27f, 0.27f, 0.27f, 0.7f), null, null, null, null, null, null, null, Color.white);
	}
}
