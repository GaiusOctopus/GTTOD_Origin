using System.Reflection;
using UnityEngine;

namespace Pixelplacement
{
	public class Initialization : MonoBehaviour
	{
		private StateMachine _stateMachine;

		private DisplayObject _displayObject;

		private void Awake()
		{
			InitializeSingleton();
			_stateMachine = GetComponent<StateMachine>();
			_displayObject = GetComponent<DisplayObject>();
			if (_displayObject != null)
			{
				_displayObject.Register();
			}
			if (_stateMachine != null)
			{
				_stateMachine.Initialize();
			}
		}

		private void Start()
		{
			if (_stateMachine != null)
			{
				_stateMachine.StartMachine();
			}
		}

		private void OnDisable()
		{
			if (_stateMachine != null && _stateMachine.returnToDefaultOnDisable && !(_stateMachine.defaultState == null))
			{
				if (_stateMachine.currentState == null)
				{
					_stateMachine.ChangeState(_stateMachine.defaultState);
				}
				else if (_stateMachine.currentState != _stateMachine.defaultState)
				{
					_stateMachine.ChangeState(_stateMachine.defaultState);
				}
			}
		}

		private void InitializeSingleton()
		{
			Component[] components = GetComponents<Component>();
			int num = 0;
			Component component;
			while (true)
			{
				if (num < components.Length)
				{
					component = components[num];
					string text = component.GetType().BaseType.ToString();
					if (text.Contains("Singleton") && text.Contains("Pixelplacement"))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			MethodInfo method = component.GetType().BaseType.GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic);
			object[] parameters = new Component[1]
			{
				component
			};
			method.Invoke(component, parameters);
		}
	}
}
