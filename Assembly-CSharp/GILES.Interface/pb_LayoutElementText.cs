using UnityEngine.UI;

namespace GILES.Interface
{
	public class pb_LayoutElementText : LayoutElement
	{
		public bool expandWidth = true;

		public bool expandHeight;

		public Text text;

		public float paddingWidth = 4f;

		public float paddingHeight = 4f;

		public override float minWidth => GetTextWidth();

		public override float preferredWidth => GetTextWidth();

		public override float minHeight => GetTextHeight();

		public override float preferredHeight => GetTextHeight();

		private float GetTextWidth()
		{
			if (text != null && expandWidth)
			{
				return text.preferredWidth + paddingWidth * 2f;
			}
			return -1f;
		}

		private float GetTextHeight()
		{
			if (text != null && expandHeight)
			{
				return text.preferredHeight + paddingHeight * 2f;
			}
			return -1f;
		}
	}
}
