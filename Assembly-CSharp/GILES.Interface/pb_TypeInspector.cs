using GILES.Serialization;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	public abstract class pb_TypeInspector : MonoBehaviour
	{
		public delegate object UpdateValueWithIndex(int index);

		public delegate object UpdateValue();

		public string memberName;

		protected object target;

		protected PropertyInfo propertyInfo;

		protected FieldInfo fieldInfo;

		private bool ignoreSetValue;

		private int index = -1;

		public pb_TypeInspector parent;

		public static readonly Color InputField_BackgroundColor = new Color(0.32f, 0.32f, 0.32f, 0.8f);

		public static readonly Color InputField_TextColor = Color.white;

		public const int InputField_MinHeight = 30;

		public UpdateValue updateValue;

		public UpdateValueWithIndex updateValueWithIndex;

		public Callback onValueBeginChange;

		public Callback<int> onValueBeginChangeAtIndex;

		public Callback<object> onValueChanged;

		public Callback<int, object> onValueChangedAtIndex;

		public Callback onTypeInspectorSetValue;

		private int onValueSetCount;

		public Type declaringType
		{
			get;
			private set;
		}

		public virtual bool useDefaultSkin => true;

		internal void SetDeclaringType(Type type)
		{
			declaringType = type;
		}

		public void Initialize(object target, PropertyInfo prop)
		{
			Initialize(null, target, prop);
		}

		public void Initialize(string name, object target, PropertyInfo prop)
		{
			if (!string.IsNullOrEmpty(name))
			{
				memberName = name;
			}
			this.target = target;
			propertyInfo = prop;
			fieldInfo = null;
			declaringType = propertyInfo.PropertyType;
			Initialize_INTERNAL();
		}

		public void Initialize(object target, FieldInfo field)
		{
			Initialize(null, target, field);
		}

		public void Initialize(string name, object target, FieldInfo field)
		{
			if (!string.IsNullOrEmpty(name))
			{
				memberName = name;
			}
			this.target = target;
			propertyInfo = null;
			fieldInfo = field;
			declaringType = fieldInfo.FieldType;
			Initialize_INTERNAL();
		}

		public void Initialize(string name, UpdateValue getStoredValueDelegate, Callback<object> onValueChangedDelegate)
		{
			Initialize(name, getStoredValueDelegate, onValueChangedDelegate, null, null);
		}

		public void Initialize(string name, UpdateValueWithIndex getStoredValueDelegate, Callback<int, object> onValueChangedDelegate)
		{
			Initialize(name, null, null, getStoredValueDelegate, onValueChangedDelegate);
		}

		public void Initialize(string name, UpdateValue getStoredValueDelegate, Callback<object> onValueChangedDelegate, UpdateValueWithIndex getStoredValueDelegateIndexed, Callback<int, object> onValueChangedDelegateIndexed)
		{
			if (!string.IsNullOrEmpty(name))
			{
				memberName = name;
			}
			updateValue = getStoredValueDelegate;
			onValueChanged = onValueChangedDelegate;
			updateValueWithIndex = getStoredValueDelegateIndexed;
			onValueChangedAtIndex = onValueChangedDelegateIndexed;
			if (declaringType == null)
			{
				object obj = (updateValue != null) ? updateValue() : ((updateValueWithIndex != null) ? updateValueWithIndex(index) : null);
				if (obj != null)
				{
					declaringType = obj.GetType();
				}
				else
				{
					declaringType = null;
				}
			}
			Initialize_INTERNAL();
		}

		private void Initialize_INTERNAL()
		{
			base.gameObject.name = GetName();
			InitializeGUI();
			if (useDefaultSkin)
			{
				ApplyDefaultSkin();
			}
			UpdateGUI();
		}

		public void SetIndexInCollection(int index)
		{
			this.index = index;
		}

		public void ApplyDefaultSkin()
		{
			InputField[] componentsInChildren = base.gameObject.GetComponentsInChildren<InputField>();
			foreach (InputField obj in componentsInChildren)
			{
				obj.gameObject.DemandComponent<LayoutElement>().minHeight = 30f;
				obj.textComponent.color = InputField_TextColor;
			}
			Button[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Button>();
			foreach (Button obj2 in componentsInChildren2)
			{
				obj2.gameObject.DemandComponent<LayoutElement>().minHeight = 30f;
				obj2.GetComponentInChildren<Text>().color = InputField_TextColor;
			}
		}

		public abstract void InitializeGUI();

		public void UpdateGUI()
		{
			ignoreSetValue = true;
			OnUpdateGUI();
			ignoreSetValue = false;
		}

		protected abstract void OnUpdateGUI();

		protected void SetValue(object value)
		{
			if (ignoreSetValue)
			{
				return;
			}
			if (++onValueSetCount == 1 && !GetValue<object>().Equals(value))
			{
				if (onValueBeginChange != null)
				{
					onValueBeginChange();
				}
				if (onValueBeginChangeAtIndex != null)
				{
					onValueBeginChangeAtIndex(index);
				}
				if (propertyInfo != null)
				{
					Undo.RegisterState(new UndoReflection(target, propertyInfo), "Set " + propertyInfo.Name);
				}
				if (fieldInfo != null)
				{
					Undo.RegisterState(new UndoReflection(target, fieldInfo), "Set " + fieldInfo.Name);
				}
			}
			if (onValueChanged != null)
			{
				onValueChanged(value);
			}
			else if (onValueChangedAtIndex != null)
			{
				onValueChangedAtIndex(index, value);
			}
			else if (propertyInfo != null)
			{
				try
				{
					if (target == null)
					{
						target = Activator.CreateInstance(propertyInfo.PropertyType);
					}
					propertyInfo.SetValue(target, value, null);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
			else if (fieldInfo != null && target != null)
			{
				try
				{
					if (target == null)
					{
						target = Activator.CreateInstance(fieldInfo.FieldType);
					}
					fieldInfo.SetValue(target, value);
				}
				catch (Exception ex2)
				{
					Debug.LogError(ex2.ToString());
				}
			}
			OnInspectedValueSet();
		}

		protected virtual void Update()
		{
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2) || Input.GetKeyUp(KeyCode.Tab) || Input.GetKeyUp(KeyCode.Return))
			{
				onValueSetCount = 0;
			}
		}

		protected virtual void OnInspectedValueSet()
		{
			if (onTypeInspectorSetValue != null)
			{
				onTypeInspectorSetValue();
			}
			if (parent != null)
			{
				parent.OnInspectedValueSet();
			}
			else if (target is Component)
			{
				if (propertyInfo != null)
				{
					pb_ComponentDiff.AddDiff((Component)target, propertyInfo.Name, GetValue<object>());
				}
				else if (fieldInfo != null)
				{
					pb_ComponentDiff.AddDiff((Component)target, fieldInfo.Name, GetValue<object>());
				}
			}
		}

		public T GetValue<T>()
		{
			if (updateValue != null)
			{
				return (T)updateValue();
			}
			if (updateValueWithIndex != null)
			{
				return (T)updateValueWithIndex(index);
			}
			if (propertyInfo != null && target != null)
			{
				return (T)propertyInfo.GetValue(target, null);
			}
			if (fieldInfo != null && target != null)
			{
				return (T)fieldInfo.GetValue(target);
			}
			return default(T);
		}

		public virtual string GetName()
		{
			if (!string.IsNullOrEmpty(memberName))
			{
				return memberName;
			}
			if (propertyInfo != null)
			{
				return propertyInfo.Name;
			}
			if (fieldInfo != null)
			{
				return fieldInfo.Name;
			}
			return "Generic Type Inspector";
		}
	}
}
