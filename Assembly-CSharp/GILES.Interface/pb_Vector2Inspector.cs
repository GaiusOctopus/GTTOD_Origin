using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Vector2))]
	public class pb_Vector2Inspector : pb_TypeInspector
	{
		private Vector2 vector;

		public Text title;

		public InputField input_x;

		public InputField input_y;

		private void OnGUIChanged()
		{
			SetValue(vector);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input_x.onValueChanged.AddListener(OnValueChange_X);
			input_y.onValueChanged.AddListener(OnValueChange_Y);
		}

		protected override void OnUpdateGUI()
		{
			vector = GetValue<Vector2>();
			input_x.text = vector.x.ToString();
			input_y.text = vector.y.ToString();
		}

		public void OnValueChange_X(string val)
		{
			if (float.TryParse(val, out float result))
			{
				vector.x = result;
				OnGUIChanged();
			}
		}

		public void OnValueChange_Y(string val)
		{
			if (float.TryParse(val, out float result))
			{
				vector.y = result;
				OnGUIChanged();
			}
		}
	}
}
