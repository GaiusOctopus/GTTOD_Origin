using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(object))]
	public class pb_ObjectInspector : pb_TypeInspector
	{
		private object value;

		private const int VERTICAL_LAYOUT_SPACING = 0;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			value = GetValue<object>();
			pb_GUIUtility.AddVerticalLayoutGroup(base.gameObject, new RectOffset(0, 0, 0, 0), 0, childForceExpandWidth: true, childForceExpandHeight: false);
			BuildInspectorTree();
		}

		protected override void OnUpdateGUI()
		{
		}

		public void OnValueChange(object val)
		{
		}

		private void BuildInspectorTree()
		{
			if (base.declaringType == null)
			{
				Debug.LogWarning("Inspector is targeting a null or primitive type with no available pb_TypeInspector override, or target is null and using delegates in the parent inspector.");
				return;
			}
			string name = GetName();
			GameObject gameObject = pb_GUIUtility.CreateLabeledVerticalPanel(name.Substring(name.LastIndexOf(".") + 1));
			gameObject.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(2, 2, 2, 2);
			gameObject.transform.SetParent(base.transform);
			foreach (PropertyInfo serializableProperty in pb_Reflection.GetSerializableProperties(base.declaringType, BindingFlags.Instance | BindingFlags.Public))
			{
				pb_InspectorResolver.AddTypeInspector(value, gameObject.transform, serializableProperty).parent = this;
			}
			foreach (FieldInfo serializableField in pb_Reflection.GetSerializableFields(base.declaringType, BindingFlags.Instance | BindingFlags.Public))
			{
				pb_InspectorResolver.AddTypeInspector(value, gameObject.transform, null, serializableField).parent = this;
			}
		}
	}
}
