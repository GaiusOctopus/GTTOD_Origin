using System.Collections;
using System.Reflection;

namespace GILES
{
	public class UndoReflection : IUndo
	{
		public object target;

		public string memberName;

		public UndoReflection(object target, string member)
		{
			this.target = target;
			memberName = member;
		}

		public UndoReflection(object target, MemberInfo info)
		{
			this.target = target;
			memberName = info.Name;
		}

		public Hashtable RecordState()
		{
			return new Hashtable
			{
				{
					"value",
					pb_Reflection.GetValue<object>(target, memberName)
				}
			};
		}

		public void ApplyState(Hashtable hash)
		{
			pb_Reflection.SetValue(target, memberName, hash["value"]);
		}

		public void OnExitScope()
		{
		}
	}
}
