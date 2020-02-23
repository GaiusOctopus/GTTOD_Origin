using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public static class pb_GUIUtility
	{
		public static readonly Color PANEL_COLOR = new Color(0.27f, 0.27f, 0.27f, 0.5f);

		public static readonly Color ITEM_BACKGROUND_COLOR = new Color(0.15f, 0.15f, 0.15f, 1f);

		public const int PADDING = 4;

		public static readonly RectOffset PADDING_RECT_OFFSET = new RectOffset(4, 4, 4, 4);

		private static Font _defaultFont;

		public static Vector2 ScreenToGUIPoint(Vector2 v)
		{
			v.y = (float)Screen.height - v.y;
			return v;
		}

		public static Font DefaultFont()
		{
			if (_defaultFont == null)
			{
				_defaultFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			}
			return _defaultFont;
		}

		public static Font GetFont(string fontName)
		{
			return Resources.Load<Font>("Required/Font/" + fontName);
		}

		public static GameObject CreateLabeledVerticalPanel(string label)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = label;
			gameObject.AddComponent<Image>().color = PANEL_COLOR;
			AddVerticalLayoutGroup(gameObject);
			CreateLabel(label).transform.SetParent(gameObject.transform);
			return gameObject;
		}

		public static GameObject CreateHorizontalGroup()
		{
			GameObject gameObject = new GameObject();
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.padding = new RectOffset(2, 2, 2, 2);
			horizontalLayoutGroup.childForceExpandWidth = true;
			horizontalLayoutGroup.childForceExpandHeight = false;
			return gameObject;
		}

		public static GameObject CreateLabel(string text)
		{
			GameObject obj = new GameObject
			{
				name = "Label Field"
			};
			Text text2 = obj.AddComponent<Text>();
			string text4 = text2.text = text.Replace("UnityEngine.", "");
			text2.font = DefaultFont();
			obj.AddComponent<LayoutElement>().minHeight = 24f;
			text2.alignment = TextAnchor.MiddleLeft;
			return obj;
		}

		public static void AddVerticalLayoutGroup(GameObject go)
		{
			AddVerticalLayoutGroup(go, PADDING_RECT_OFFSET, 4, childForceExpandWidth: true, childForceExpandHeight: false);
		}

		public static void AddVerticalLayoutGroup(GameObject go, RectOffset padding, int spacing, bool childForceExpandWidth, bool childForceExpandHeight)
		{
			VerticalLayoutGroup verticalLayoutGroup = go.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = padding;
			verticalLayoutGroup.spacing = spacing;
			verticalLayoutGroup.childForceExpandWidth = childForceExpandWidth;
			verticalLayoutGroup.childForceExpandHeight = childForceExpandHeight;
		}

		public static Selectable GetNextSelectable(Selectable current)
		{
			if (current == null)
			{
				return null;
			}
			Selectable selectable = current.FindSelectableOnRight();
			if (selectable != null)
			{
				return selectable;
			}
			selectable = current.FindSelectableOnDown();
			if (selectable == null)
			{
				return null;
			}
			Selectable selectable2 = selectable;
			while (selectable != null)
			{
				selectable2 = selectable2.FindSelectableOnLeft();
				if (selectable2 == null)
				{
					return selectable;
				}
				selectable = selectable2;
			}
			return null;
		}

		public static Rect GetScreenRect(this RectTransform rectTransform)
		{
			Vector3[] array = new Vector3[4];
			rectTransform.GetWorldCorners(array);
			Vector2 vector = Vector3.Min(Vector3.Min(Vector3.Min(array[0], array[1]), array[2]), array[3]);
			Vector2 vector2 = Vector3.Max(Vector3.Max(Vector3.Max(array[0], array[1]), array[2]), array[3]);
			return new Rect(vector.x, vector2.y, vector2.x - vector.x, vector2.y - vector.y);
		}
	}
}
