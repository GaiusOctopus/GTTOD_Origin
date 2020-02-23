using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(string))]
	public class pb_StringInspector : pb_TypeInspector
	{
		private string value;

		public Text title;

		public InputField input;

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			input.onValueChanged.AddListener(OnValueChange);
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<string>();
			input.text = ((value != null) ? value.ToString() : "null");
		}

		public void OnValueChange(string val)
		{
			SetValue(val);
		}
	}
}
