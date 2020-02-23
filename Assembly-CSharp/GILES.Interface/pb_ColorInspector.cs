using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Color))]
	public class pb_ColorInspector : pb_TypeInspector
	{
		private Color value;

		public Text title;

		public InputField input_r;

		public InputField input_g;

		public InputField input_b;

		public InputField input_a;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input_r.onValueChanged.AddListener(OnValueChange_R);
			input_g.onValueChanged.AddListener(OnValueChange_G);
			input_b.onValueChanged.AddListener(OnValueChange_B);
			input_a.onValueChanged.AddListener(OnValueChange_A);
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<Color>();
			input_r.text = value.r.ToString("g");
			input_g.text = value.g.ToString("g");
			input_b.text = value.b.ToString("g");
			input_a.text = value.a.ToString("g");
		}

		public void OnValueChange_R(string val)
		{
			if (float.TryParse(val, out float result))
			{
				value.r = Mathf.Clamp(result, 0f, 1f);
				if (result < 0f || result > 1f)
				{
					input_r.text = value.r.ToString("g");
				}
				OnGUIChanged();
			}
		}

		public void OnValueChange_G(string val)
		{
			if (float.TryParse(val, out float result))
			{
				value.g = Mathf.Clamp(result, 0f, 1f);
				if (result < 0f || result > 1f)
				{
					input_g.text = value.g.ToString("g");
				}
				OnGUIChanged();
			}
		}

		public void OnValueChange_B(string val)
		{
			if (float.TryParse(val, out float result))
			{
				value.b = Mathf.Clamp(result, 0f, 1f);
				if (result < 0f || result > 1f)
				{
					input_b.text = value.b.ToString("g");
				}
				OnGUIChanged();
			}
		}

		public void OnValueChange_A(string val)
		{
			if (float.TryParse(val, out float result))
			{
				value.a = Mathf.Clamp(result, 0f, 1f);
				if (result < 0f || result > 1f)
				{
					input_a.text = value.a.ToString("g");
				}
				OnGUIChanged();
			}
		}
	}
}
