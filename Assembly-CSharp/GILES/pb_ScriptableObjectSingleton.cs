using UnityEngine;

namespace GILES
{
	public class pb_ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		private static ScriptableObject _instance;

		public static T instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = ScriptableObject.CreateInstance<T>();
				}
				return (T)_instance;
			}
		}

		public static T nullableInstance => (T)_instance;

		protected virtual void OnEnable()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			else
			{
				pb_ObjectUtility.Destroy(this);
			}
		}
	}
}
