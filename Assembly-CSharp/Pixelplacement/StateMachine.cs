using UnityEngine;
using UnityEngine.Events;

namespace Pixelplacement
{
	[RequireComponent(typeof(Initialization))]
	public class StateMachine : MonoBehaviour
	{
		public GameObject defaultState;

		public GameObject currentState;

		public bool _unityEventsFolded;

		[Tooltip("Should log messages be thrown during usage?")]
		public bool verbose = true;

		[Tooltip("Can States within this StateMachine be reentered?")]
		public bool allowReentry;

		[Tooltip("Return to default state on disable?")]
		public bool returnToDefaultOnDisable = true;

		public GameObjectEvent OnStateExited;

		public GameObjectEvent OnStateEntered;

		public UnityEvent OnFirstStateEntered;

		public UnityEvent OnFirstStateExited;

		public UnityEvent OnLastStateEntered;

		public UnityEvent OnLastStateExited;

		private bool _initialized;

		private bool _atFirst;

		private bool _atLast;

		public bool CleanSetup
		{
			get;
			private set;
		}

		public bool AtFirst
		{
			get
			{
				return _atFirst;
			}
			private set
			{
				if (_atFirst)
				{
					_atFirst = false;
					if (OnFirstStateExited != null)
					{
						OnFirstStateExited.Invoke();
					}
				}
				else
				{
					_atFirst = true;
					if (OnFirstStateEntered != null)
					{
						OnFirstStateEntered.Invoke();
					}
				}
			}
		}

		public bool AtLast
		{
			get
			{
				return _atLast;
			}
			private set
			{
				if (_atLast)
				{
					_atLast = false;
					if (OnLastStateExited != null)
					{
						OnLastStateExited.Invoke();
					}
				}
				else
				{
					_atLast = true;
					if (OnLastStateEntered != null)
					{
						OnLastStateEntered.Invoke();
					}
				}
			}
		}

		public GameObject Next()
		{
			if (currentState == null)
			{
				return ChangeState(0);
			}
			int siblingIndex = currentState.transform.GetSiblingIndex();
			if (siblingIndex == base.transform.childCount - 1)
			{
				return currentState;
			}
			return ChangeState(++siblingIndex);
		}

		public GameObject Previous()
		{
			if (currentState == null)
			{
				return ChangeState(0);
			}
			int siblingIndex = currentState.transform.GetSiblingIndex();
			if (siblingIndex == 0)
			{
				return currentState;
			}
			return ChangeState(--siblingIndex);
		}

		public void Exit()
		{
			if (!(currentState == null))
			{
				Log("(-) " + base.name + " EXITED state: " + currentState.name);
				int siblingIndex = currentState.transform.GetSiblingIndex();
				if (siblingIndex == 0)
				{
					AtFirst = false;
				}
				if (siblingIndex == base.transform.childCount - 1)
				{
					AtLast = false;
				}
				if (OnStateExited != null)
				{
					OnStateExited.Invoke(currentState);
				}
				currentState.SetActive(value: false);
				currentState = null;
			}
		}

		public GameObject ChangeState(int childIndex)
		{
			if (childIndex > base.transform.childCount - 1)
			{
				Log("Index is greater than the amount of states in the StateMachine \"" + base.gameObject.name + "\" please verify the index you are trying to change to.");
				return null;
			}
			return ChangeState(base.transform.GetChild(childIndex).gameObject);
		}

		public GameObject ChangeState(GameObject state)
		{
			if (currentState != null && !allowReentry && state == currentState)
			{
				Log("State change ignored. State machine \"" + base.name + "\" already in \"" + state.name + "\" state.");
				return null;
			}
			if (state.transform.parent != base.transform)
			{
				Log("State \"" + state.name + "\" is not a child of \"" + base.name + "\" StateMachine state change canceled.");
				return null;
			}
			Exit();
			Enter(state);
			return currentState;
		}

		public GameObject ChangeState(string state)
		{
			Transform transform = base.transform.Find(state);
			if (!transform)
			{
				Log("\"" + base.name + "\" does not contain a state by the name of \"" + state + "\" please verify the name of the state you are trying to reach.");
				return null;
			}
			return ChangeState(transform.gameObject);
		}

		public void Initialize()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
		}

		public void StartMachine()
		{
			if (Application.isPlaying && defaultState != null)
			{
				ChangeState(defaultState.name);
			}
		}

		private void Enter(GameObject state)
		{
			currentState = state;
			int siblingIndex = currentState.transform.GetSiblingIndex();
			if (siblingIndex == 0)
			{
				AtFirst = true;
			}
			if (siblingIndex == base.transform.childCount - 1)
			{
				AtLast = true;
			}
			Log("(+) " + base.name + " ENTERED state: " + state.name);
			if (OnStateEntered != null)
			{
				OnStateEntered.Invoke(currentState);
			}
			currentState.SetActive(value: true);
		}

		private void Log(string message)
		{
			if (verbose)
			{
				Debug.Log(message, base.gameObject);
			}
		}
	}
}
