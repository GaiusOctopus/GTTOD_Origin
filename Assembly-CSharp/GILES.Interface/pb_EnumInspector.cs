using System;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(Enum))]
	public class pb_EnumInspector : pb_TypeInspector
	{
		private object value;

		public Text title;

		public Button button;

		public Text currentEnumValue;

		private string[] enumNames;

		private Array enumValues;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			button.onClick.AddListener(OnClick);
			enumNames = Enum.GetNames(base.declaringType);
			enumValues = Enum.GetValues(base.declaringType);
		}

		private void OnClick()
		{
			if (enumValues != null)
			{
				int length = enumValues.Length;
				int num = Array.IndexOf(enumValues, value);
				num++;
				if (num >= length)
				{
					num = 0;
				}
				value = enumValues.GetValue(num);
				RefreshText();
				OnGUIChanged();
			}
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<object>();
			RefreshText();
		}

		private void RefreshText()
		{
			if (enumNames != null)
			{
				currentEnumValue.text = value.ToString();
			}
			else
			{
				currentEnumValue.text = "Enum: " + value.ToString();
			}
		}
	}
}
