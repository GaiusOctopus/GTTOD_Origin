using GILES.Serialization;
using UnityEngine;

namespace GILES.Interface
{
	public class pb_TransformEditor : pb_ComponentEditor
	{
		private Transform _transform;

		protected override void InitializeGUI()
		{
			pb_GUIUtility.AddVerticalLayoutGroup(base.gameObject);
			_transform = (Transform)target;
			pb_TypeInspector inspector = pb_InspectorResolver.GetInspector(typeof(Vector3));
			pb_TypeInspector inspector2 = pb_InspectorResolver.GetInspector(typeof(Vector3));
			pb_TypeInspector inspector3 = pb_InspectorResolver.GetInspector(typeof(Vector3));
			inspector.Initialize("Position", UpdatePosition, OnSetPosition);
			inspector2.Initialize("Rotation", UpdateRotation, OnSetRotation);
			inspector3.Initialize("Scale", UpdateScale, OnSetScale);
			inspector.onValueBeginChange = delegate
			{
				Undo.RegisterState(new UndoTransform(_transform), "Set Position: " + _transform.position.ToString("G"));
			};
			inspector2.onValueBeginChange = delegate
			{
				Undo.RegisterState(new UndoTransform(_transform), "Set Rotation: " + _transform.localRotation.eulerAngles.ToString("G"));
			};
			inspector3.onValueBeginChange = delegate
			{
				Undo.RegisterState(new UndoTransform(_transform), "Set Scale: " + _transform.localScale.ToString("G"));
			};
			inspector.transform.SetParent(base.transform);
			inspector2.transform.SetParent(base.transform);
			inspector3.transform.SetParent(base.transform);
		}

		private void Update()
		{
			if (Input.GetMouseButton(0) && pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance != null && pb_MonoBehaviourSingleton<pb_SelectionHandle>.instance.InUse())
			{
				UpdateGUI();
			}
		}

		private object UpdatePosition(int index)
		{
			return _transform.position;
		}

		private void OnSetPosition(int index, object value)
		{
			_transform.position = (Vector3)value;
			pb_Selection.OnExternalUpdate();
			pb_ComponentDiff.AddDiff(target, "position", _transform.position);
		}

		private object UpdateRotation(int index)
		{
			return _transform.eulerAngles;
		}

		private void OnSetRotation(int index, object value)
		{
			_transform.localRotation = Quaternion.Euler((Vector3)value);
			pb_Selection.OnExternalUpdate();
		}

		private object UpdateScale(int index)
		{
			return _transform.localScale;
		}

		private void OnSetScale(int index, object value)
		{
			_transform.localScale = (Vector3)value;
			pb_Selection.OnExternalUpdate();
		}
	}
}
