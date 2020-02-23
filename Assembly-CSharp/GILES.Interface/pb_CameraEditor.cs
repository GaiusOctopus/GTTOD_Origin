using GILES.Serialization;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_CameraEditor : pb_ComponentEditor
	{
		private Camera _camera;

		protected override void InitializeGUI()
		{
			_camera = (Camera)target;
			pb_GUIUtility.AddVerticalLayoutGroup(base.gameObject);
			pb_TypeInspector inspector = pb_InspectorResolver.GetInspector(typeof(bool));
			inspector.Initialize("Enabled", UpdateEnabled, OnSetEnabled);
			inspector.onValueBeginChange = delegate
			{
				Undo.RegisterState(new UndoReflection(_camera, "enabled"), "Camera Enabled");
			};
			inspector.transform.SetParent(base.transform);
		}

		private object UpdateEnabled()
		{
			return _camera.enabled;
		}

		private void OnSetEnabled(object value)
		{
			_camera.enabled = (bool)value;
			pb_ComponentDiff.AddDiff(target, "enabled", _camera.enabled);
		}
	}
}
