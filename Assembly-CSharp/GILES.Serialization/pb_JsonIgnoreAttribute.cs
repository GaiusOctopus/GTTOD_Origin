using System;

namespace GILES.Serialization
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public class pb_JsonIgnoreAttribute : Attribute
	{
	}
}
