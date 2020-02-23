using UnityEngine;

namespace Pixelplacement
{
	public class State : MonoBehaviour
	{
		private StateMachine _stateMachine;

		public bool IsFirst => base.transform.GetSiblingIndex() == 0;

		public bool IsLast => base.transform.GetSiblingIndex() == base.transform.parent.childCount - 1;

		public StateMachine StateMachine
		{
			get
			{
				if (_stateMachine == null)
				{
					_stateMachine = base.transform.parent.GetComponent<StateMachine>();
					if (_stateMachine == null)
					{
						Debug.LogError("States must be the child of a StateMachine to operate.");
						return null;
					}
				}
				return _stateMachine;
			}
		}

		public void ChangeState(int childIndex)
		{
			StateMachine.ChangeState(childIndex);
		}

		public void ChangeState(GameObject state)
		{
			StateMachine.ChangeState(state.name);
		}

		public void ChangeState(string state)
		{
			if (!(StateMachine == null))
			{
				StateMachine.ChangeState(state);
			}
		}

		public GameObject Next()
		{
			return StateMachine.Next();
		}

		public GameObject Previous()
		{
			return StateMachine.Previous();
		}

		public void Exit()
		{
			StateMachine.Exit();
		}
	}
}
