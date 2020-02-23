using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GILES
{
	public class Undo : pb_ScriptableObjectSingleton<Undo>
	{
		protected class UndoState
		{
			public string message;

			public IUndo target;

			public Hashtable values;

			public UndoState(IUndo target, string msg)
			{
				this.target = target;
				message = msg;
				values = target.RecordState();
			}

			public void Apply()
			{
				target.ApplyState(values);
			}

			public override string ToString()
			{
				return message;
			}
		}

		public Callback undoPerformed;

		public Callback redoPerformed;

		public Callback undoStackModified;

		public Callback redoStackModified;

		[SerializeField]
		private Stack<List<UndoState>> undoStack = new Stack<List<UndoState>>();

		[SerializeField]
		private Stack<List<UndoState>> redoStack = new Stack<List<UndoState>>();

		[SerializeField]
		private UndoState currentUndo;

		[SerializeField]
		private UndoState currentRedo;

		public static void AddUndoPerformedListener(Callback callback)
		{
			if (pb_ScriptableObjectSingleton<Undo>.instance.undoPerformed != null)
			{
				Undo instance = pb_ScriptableObjectSingleton<Undo>.instance;
				instance.undoPerformed = (Callback)Delegate.Combine(instance.undoPerformed, callback);
			}
			else
			{
				pb_ScriptableObjectSingleton<Undo>.instance.undoPerformed = callback;
			}
		}

		public static void AddRedoPerformedListener(Callback callback)
		{
			if (pb_ScriptableObjectSingleton<Undo>.instance.redoPerformed != null)
			{
				Undo instance = pb_ScriptableObjectSingleton<Undo>.instance;
				instance.redoPerformed = (Callback)Delegate.Combine(instance.redoPerformed, callback);
			}
			else
			{
				pb_ScriptableObjectSingleton<Undo>.instance.redoPerformed = callback;
			}
		}

		public static string PrintUndoStack()
		{
			return pb_ScriptableObjectSingleton<Undo>.instance.PrintStack(pb_ScriptableObjectSingleton<Undo>.instance.undoStack);
		}

		public static string PrintRedoStack()
		{
			return pb_ScriptableObjectSingleton<Undo>.instance.PrintStack(pb_ScriptableObjectSingleton<Undo>.instance.redoStack);
		}

		private string PrintStack(Stack<List<UndoState>> stack)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (List<UndoState> item in stack)
			{
				using (List<UndoState>.Enumerator enumerator2 = item.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						UndoState current = enumerator2.Current;
						stringBuilder.AppendLine(current.ToString());
					}
				}
				stringBuilder.AppendLine("-----");
			}
			return stringBuilder.ToString();
		}

		private void PushUndo(List<UndoState> state)
		{
			currentUndo = state[0];
			undoStack.Push(state);
			if (undoStackModified != null)
			{
				undoStackModified();
			}
		}

		private void PushRedo(List<UndoState> state)
		{
			currentRedo = state[0];
			redoStack.Push(state);
			if (redoStackModified != null)
			{
				redoStackModified();
			}
		}

		private List<UndoState> PopUndo()
		{
			List<UndoState> list = Pop(undoStack);
			if (list == null || undoStack.Count < 1)
			{
				currentUndo = null;
			}
			else
			{
				currentUndo = undoStack.Peek()[0];
			}
			if (undoStackModified != null)
			{
				undoStackModified();
			}
			return list;
		}

		private List<UndoState> PopRedo()
		{
			List<UndoState> list = Pop(redoStack);
			if (list == null || redoStack.Count < 1)
			{
				currentRedo = null;
			}
			else
			{
				currentRedo = redoStack.Peek()[0];
			}
			if (redoStackModified != null)
			{
				redoStackModified();
			}
			return list;
		}

		private List<UndoState> Pop(Stack<List<UndoState>> stack)
		{
			if (stack.Count > 0)
			{
				return stack.Pop();
			}
			return null;
		}

		private static void ClearStack(Stack<List<UndoState>> stack)
		{
			foreach (List<UndoState> item in stack)
			{
				foreach (UndoState item2 in item)
				{
					item2.target.OnExitScope();
				}
			}
			stack.Clear();
		}

		public static string GetCurrentUndo()
		{
			if (pb_ScriptableObjectSingleton<Undo>.instance.currentUndo != null)
			{
				return pb_ScriptableObjectSingleton<Undo>.instance.currentUndo.message;
			}
			return "";
		}

		public static string GetCurrentRedo()
		{
			if (pb_ScriptableObjectSingleton<Undo>.instance.currentRedo != null)
			{
				return pb_ScriptableObjectSingleton<Undo>.instance.currentRedo.message;
			}
			return "";
		}

		public static void RegisterState(IUndo target, string message)
		{
			ClearStack(pb_ScriptableObjectSingleton<Undo>.instance.redoStack);
			pb_ScriptableObjectSingleton<Undo>.instance.currentRedo = null;
			pb_ScriptableObjectSingleton<Undo>.instance.PushUndo(new List<UndoState>
			{
				new UndoState(target, message)
			});
		}

		public static void RegisterStates(IEnumerable<IUndo> targets, string message)
		{
			ClearStack(pb_ScriptableObjectSingleton<Undo>.instance.redoStack);
			pb_ScriptableObjectSingleton<Undo>.instance.currentRedo = null;
			List<UndoState> state = targets.Select((IUndo x) => new UndoState(x, message)).ToList();
			pb_ScriptableObjectSingleton<Undo>.instance.PushUndo(state);
		}

		public static void PerformUndo()
		{
			List<UndoState> list = pb_ScriptableObjectSingleton<Undo>.instance.PopUndo();
			if (list != null)
			{
				pb_ScriptableObjectSingleton<Undo>.instance.PushRedo(list.Select((UndoState x) => new UndoState(x.target, x.message)).ToList());
				foreach (UndoState item in list)
				{
					item.Apply();
				}
				if (pb_ScriptableObjectSingleton<Undo>.instance.undoPerformed != null)
				{
					pb_ScriptableObjectSingleton<Undo>.instance.undoPerformed();
				}
			}
		}

		public static void PerformRedo()
		{
			List<UndoState> list = pb_ScriptableObjectSingleton<Undo>.instance.PopRedo();
			if (list != null)
			{
				pb_ScriptableObjectSingleton<Undo>.instance.PushUndo(list.Select((UndoState x) => new UndoState(x.target, x.message)).ToList());
				foreach (UndoState item in list)
				{
					item.Apply();
				}
				if (pb_ScriptableObjectSingleton<Undo>.instance.redoPerformed != null)
				{
					pb_ScriptableObjectSingleton<Undo>.instance.redoPerformed();
				}
			}
		}
	}
}
