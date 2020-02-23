using UnityEngine;

namespace GILES
{
	public class pb_MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static MonoBehaviour _instance;

		public virtual bool dontDestroyOnLoad => false;

		public static T instance
		{
			get
			{
				if (_instance == null)
				{
					T[] array = Object.FindObjectsOfType<T>();
					if (array != null && array.Length != 0)
					{
						_instance = array[0];
						for (int i = 1; i < array.Length; i++)
						{
							pb_ObjectUtility.Destroy(array[i]);
						}
					}
					else
					{
						GameObject gameObject = new GameObject();
						string text = typeof(T).ToString();
						int num = text.LastIndexOf('.') + 1;
						gameObject.name = ((num > 0) ? text.Substring(num) : text) + " Singleton";
						T val = gameObject.AddComponent<T>();
						pb_MonoBehaviourSingleton<T> pb_MonoBehaviourSingleton = val as pb_MonoBehaviourSingleton<T>;
						if (pb_MonoBehaviourSingleton != null)
						{
							pb_MonoBehaviourSingleton.Initialize();
						}
						_instance = val;
					}
					if (((pb_MonoBehaviourSingleton<T>)_instance).dontDestroyOnLoad)
					{
						Object.DontDestroyOnLoad(_instance.gameObject);
					}
				}
				return (T)_instance;
			}
		}

		public static T nullableInstance => (T)_instance;

		protected virtual void Initialize()
		{
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				if (((pb_MonoBehaviourSingleton<T>)_instance).dontDestroyOnLoad)
				{
					Object.DontDestroyOnLoad(_instance.gameObject);
				}
			}
			else
			{
				pb_ObjectUtility.Destroy(base.gameObject);
			}
		}
	}
}
