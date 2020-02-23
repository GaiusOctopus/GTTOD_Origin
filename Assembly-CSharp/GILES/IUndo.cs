using System.Collections;

namespace GILES
{
	public interface IUndo
	{
		Hashtable RecordState();

		void ApplyState(Hashtable values);

		void OnExitScope();
	}
}
