using UnityEngine;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(GameObject))]
	public class pb_GameObjectInspector : pb_TypeInspector
	{
		private GameObject value;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			value = GetValue<GameObject>();
		}

		protected override void OnUpdateGUI()
		{
		}

		public void OnValueChange(object val)
		{
		}
	}
}
