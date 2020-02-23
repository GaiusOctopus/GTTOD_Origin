using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GILES.Interface
{
	[pb_TypeInspector(typeof(ICollection))]
	public class pb_ICollectionInspector : pb_TypeInspector
	{
		private const int MAX_COLLECTION_LENGTH = 32;

		private ICollection value;

		private object[] array;

		public Text title;

		public Transform collection;

		private void OnGUIChanged()
		{
			SetValue(value);
		}

		public override void InitializeGUI()
		{
			title.text = GetName().SplitCamelCase();
			value = GetValue<ICollection>();
			if (value == null)
			{
				return;
			}
			array = value.Cast<object>().ToArray();
			if (array.Length >= 1 && array.Length <= 32 && !(base.declaringType == null) && !(base.declaringType.GetElementType() == null))
			{
				Type elementType = base.declaringType.GetElementType();
				string name = elementType.ToString().Substring(elementType.ToString().LastIndexOf('.') + 1);
				for (int i = 0; i < array.Length; i++)
				{
					pb_TypeInspector inspector = pb_InspectorResolver.GetInspector(elementType);
					inspector.SetIndexInCollection(i);
					inspector.Initialize(name, (int index) => array[index], SetValueAtIndex);
					inspector.transform.SetParent(collection);
				}
			}
		}

		private void SetValueAtIndex(int index, object val)
		{
			Debug.LogWarning("Setting values in a collection is not supported yet!");
			array[index] = val;
		}

		protected override void OnUpdateGUI()
		{
			value = GetValue<ICollection>();
			if (value != null)
			{
				int num = array.Length;
				array = value.Cast<object>().ToArray();
				if (array.Length >= 1 && array.Length <= 32 && num != array.Length)
				{
					foreach (Transform item in base.transform)
					{
						pb_ObjectUtility.Destroy(item.gameObject);
					}
					InitializeGUI();
				}
			}
		}
	}
}
