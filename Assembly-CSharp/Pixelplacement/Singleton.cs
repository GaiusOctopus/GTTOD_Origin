using UnityEngine;

namespace Pixelplacement
{
	[RequireComponent(typeof(Initialization))]
	public abstract class Singleton<T> : MonoBehaviour
	{
		[SerializeField]
		private bool _dontDestroyOnLoad;

		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					Debug.LogError("Singleton not registered! Make sure the GameObject running your singleton is active in your scene and has a CoreInitialization component attached.");
					return default(T);
				}
				return _instance;
			}
		}

		protected virtual void OnRegistration()
		{
		}

		public void RegisterSingleton(T instance)
		{
			_instance = instance;
		}

		protected void Initialize(T instance)
		{
			if (_dontDestroyOnLoad)
			{
				base.transform.parent = null;
				Object.DontDestroyOnLoad(base.gameObject);
			}
			_instance = instance;
			OnRegistration();
		}
	}
}
