using UnityEngine;

namespace GILES.Interface
{
	public interface pb_ICustomEditor
	{
		pb_ComponentEditor InstantiateInspector(Component component);
	}
}
