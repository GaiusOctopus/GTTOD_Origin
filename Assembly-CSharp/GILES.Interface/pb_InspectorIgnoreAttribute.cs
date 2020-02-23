using System;

namespace GILES.Interface
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class pb_InspectorIgnoreAttribute : Attribute
	{
	}
}
