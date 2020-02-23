using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Material))]
	public class pb_MaterialInspector : pb_TypeInspector
	{
		private Material value;

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
			value = GetValue<Material>();
			dropbox.text = ((value == null) ? "null" : value.ToString());
		}

		public void OnValueChange(Material val)
		{
			value = val;
			OnGUIChanged();
		}
	}
}
