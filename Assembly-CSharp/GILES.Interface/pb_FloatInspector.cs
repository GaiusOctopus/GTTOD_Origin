using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(float))]
	public class pb_FloatInspector : pb_TypeInspector
	{
		private float value;

		public Text title;

		public InputField input;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input.onValueChanged.AddListener(OnValueChange);
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<float>();
			input.text = value.ToString();
		}

		public void OnValueChange(string val)
		{
			if (float.TryParse(val, out float result))
			{
				value = result;
				OnGUIChanged();
			}
		}
	}
}
