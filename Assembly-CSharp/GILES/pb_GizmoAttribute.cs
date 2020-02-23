using System;

namespace GILES
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class pb_GizmoAttribute : Attribute
	{
		public Type type;

		public pb_GizmoAttribute(Type type)
		{
			this.type = type;
		}

		public virtual bool CanEditType(Type rhs)
		{
			return type.IsAssignableFrom(rhs);
		}
	}
}
