using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Object))]
	public class pb_UnityObjectInspector : pb_TypeInspector
	{
		private Object value;

		public Text title;

		public InputField dropbox;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<Object>();
			dropbox.text = ((value == null) ? "null" : value.ToString());
		}

		public void OnValueChange(Object val)
		{
			value = val;
			OnGUIChanged();
		}
	}
}
