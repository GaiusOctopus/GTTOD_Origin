using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Vector3))]
	public class pb_Vector3Inspector : pb_TypeInspector
	{
		private Vector3 vector;

		public Text title;

		public InputField input_x;

		public InputField input_y;

		public InputField input_z;

		private void OnGUIChanged()
		{
			SetValue(vector);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input_x.onValueChanged.AddListener(OnValueChange_X);
			input_y.onValueChanged.AddListener(OnValueChange_Y);
			input_z.onValueChanged.AddListener(OnValueChange_Z);
		}

		protected override void OnUpdateGUI()
		{
			vector = GetValue<Vector3>();
			input_x.text = vector.x.ToString();
			input_y.text = vector.y.ToString();
			input_z.text = vector.z.ToString();
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

		public void OnValueChange_Z(string val)
		{
			if (float.TryParse(val, out float result))
			{
				vector.z = result;
				OnGUIChanged();
			}
		}
	}
}
