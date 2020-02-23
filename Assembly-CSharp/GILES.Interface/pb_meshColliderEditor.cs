using UnityEngine;

namespace GILES.Interface
{
	public class pb_meshColliderEditor : pb_ComponentEditor
	{
		private MeshCollider _meshCollider;

		protected override void InitializeGUI()
		{
			_meshCollider = (MeshCollider)target;
			pb_GUIUtility.AddVerticalLayoutGroup(base.gameObject);
			pb_TypeInspector inspector = pb_InspectorResolver.GetInspector(typeof(bool));
			inspector.Initialize("Enabled", UpdateEnabled, OnSetEnabled);
			inspector.onValueBeginChange = delegate
			{
				Undo.RegisterState(new UndoReflection(_meshCollider, "enabled"), "Mesh Collider Enabled");
			};
			inspector.transform.SetParent(base.transform);
		}

		private object UpdateEnabled()
		{
			return _meshCollider.enabled;
		}

		private void OnSetEnabled(object value)
		{
			_meshCollider.enabled = (bool)value;
		}
	}
}
