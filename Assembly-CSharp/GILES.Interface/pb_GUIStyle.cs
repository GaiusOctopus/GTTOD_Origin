using System;
using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[Serializable]
	[CreateAssetMenu(menuName = "Level Editor GUI Style", fileName = "RT GUI Style", order = 800)]
	public class pb_GUIStyle : ScriptableObject
	{
		[SerializeField]
		private Font _font;

		public Color color = Color.white;

		public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.7f);

		public Color highlightedColor = new Color(0.27f, 0.27f, 0.27f, 1f);

		public Color pressedColor = new Color(0.37f, 0.37f, 0.37f, 1f);

		public Color disabledColor = new Color(0.7f, 0.7f, 0.7f, 1f);

		public Texture2D image;

		public Sprite sprite;

		public Color fontColor = Color.white;

		public Font font
		{
			get
			{
				if (!(_font == null))
				{
					return _font;
				}
				return (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			}
			set
			{
				_font = value;
			}
		}

		public static pb_GUIStyle Create(Color color, Color? normalColor = null, Color? highlightedColor = null, Color? pressedColor = null, Color? disabledColor = null, Texture2D image = null, Sprite sprite = null, Font font = null, Color? fontColor = null)
		{
			pb_GUIStyle pb_GUIStyle = ScriptableObject.CreateInstance<pb_GUIStyle>();
			pb_GUIStyle.color = color;
			pb_GUIStyle.image = image;
			pb_GUIStyle.sprite = sprite;
			pb_GUIStyle.font = font;
			if (normalColor.HasValue)
			{
				pb_GUIStyle.normalColor = normalColor.Value;
			}
			if (highlightedColor.HasValue)
			{
				pb_GUIStyle.highlightedColor = highlightedColor.Value;
			}
			if (pressedColor.HasValue)
			{
				pb_GUIStyle.pressedColor = pressedColor.Value;
			}
			if (disabledColor.HasValue)
			{
				pb_GUIStyle.disabledColor = disabledColor.Value;
			}
			if (fontColor.HasValue)
			{
				pb_GUIStyle.fontColor = fontColor.Value;
			}
			return pb_GUIStyle;
		}

		public virtual void Apply(Graphic element)
		{
			element.color = ((element is Text) ? fontColor : color);
			pb_Reflection.SetValue(element, "font", font);
			pb_Reflection.SetValue(element, "image", image);
			pb_Reflection.SetValue(element, "sprite", sprite);
		}

		public virtual void Apply(Selectable element)
		{
			ColorBlock colors = element.colors;
			colors.disabledColor = disabledColor;
			colors.highlightedColor = highlightedColor;
			colors.normalColor = normalColor;
			colors.pressedColor = pressedColor;
			element.colors = colors;
		}
	}
}
