using GILES.Serialization;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_MeshRendererEditor : pb_ComponentEditor
	{
		private MeshRenderer _meshRenderer;

		protected override void InitializeGUI()
		{
			_meshRenderer = (MeshRenderer)target;
			pb_GUIUtility.AddVerticalLayoutGroup(base.gameObject);
			pb_TypeInspector inspector = pb_InspectorResolver.GetInspector(typeof(bool));
			inspector.Initialize("Enabled", UpdateEnabled, OnSetEnabled);
			inspector.onValueBeginChange = delegate
			{
				Undo.RegisterState(new UndoReflection(_meshRenderer, "enabled"), "MeshRenderer Enabled");
			};
			inspector.transform.SetParent(base.transform);
		}

		private object UpdateEnabled()
		{
			return _meshRenderer.enabled;
		}

		private void OnSetEnabled(object value)
		{
			_meshRenderer.enabled = (bool)value;
			pb_ComponentDiff.AddDiff(target, "enabled", _meshRenderer.enabled);
		}
	}
}
