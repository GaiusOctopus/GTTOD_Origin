using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(bool))]
	public class pb_BoolInspector : pb_TypeInspector
	{
		private bool value;

		public Text title;

		public Toggle input;

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input.onValueChanged.AddListener(OnValueChange);
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<bool>();
			input.isOn = value;
		}

		public void OnValueChange(bool val)
		{
			value = val;
			SetValue(value);
		}
	}
}
