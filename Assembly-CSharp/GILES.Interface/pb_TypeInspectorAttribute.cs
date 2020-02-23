using System;

namespace GILES.Interface
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class pb_TypeInspectorAttribute : Attribute
	{
		public Type type;

		public pb_TypeInspectorAttribute(Type type)
		{
			this.type = type;
		}

		public virtual bool CanEditType(Type rhs)
		{
			return type.IsAssignableFrom(rhs);
		}
	}
}
