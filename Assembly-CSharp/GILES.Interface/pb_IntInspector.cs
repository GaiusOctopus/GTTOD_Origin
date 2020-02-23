using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(int))]
	public class pb_IntInspector : pb_TypeInspector
	{
		private int value;

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
			value = GetValue<int>();
			input.text = value.ToString();
		}

		public void OnValueChange(string val)
		{
			if (int.TryParse(val, out int result))
			{
				value = result;
				OnGUIChanged();
			}
		}
	}
}
