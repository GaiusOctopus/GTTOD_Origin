using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Quaternion))]
	public class pb_QuaternionInspector : pb_TypeInspector
	{
		private Quaternion quaternion;

		public Text title;

		public InputField input_x;

		public InputField input_y;

		public InputField input_z;

		public InputField input_w;

		private void OnGUIChanged()
		{
			SetValue(quaternion);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input_x.onValueChanged.AddListener(OnValueChange_X);
			input_y.onValueChanged.AddListener(OnValueChange_Y);
			input_z.onValueChanged.AddListener(OnValueChange_Z);
			input_w.onValueChanged.AddListener(OnValueChange_W);
		}

		protected override void OnUpdateGUI()
		{
			quaternion = GetValue<Quaternion>();
			input_x.text = quaternion.x.ToString();
			input_y.text = quaternion.y.ToString();
			input_z.text = quaternion.z.ToString();
			input_w.text = quaternion.w.ToString();
		}

		public void OnValueChange_X(string val)
		{
			if (float.TryParse(val, out float result))
			{
				quaternion.x = result;
				OnGUIChanged();
			}
		}

		public void OnValueChange_Y(string val)
		{
			if (float.TryParse(val, out float result))
			{
				quaternion.y = result;
				OnGUIChanged();
			}
		}

		public void OnValueChange_Z(string val)
		{
			if (float.TryParse(val, out float result))
			{
				quaternion.z = result;
				OnGUIChanged();
			}
		}

		public void OnValueChange_W(string val)
		{
			if (float.TryParse(val, out float result))
			{
				quaternion.w = result;
				OnGUIChanged();
			}
		}
	}
}
