using System.Collections.Generic;

namespace GILES.Serialization
{
	public interface pb_ISerializableComponent
	{
		Dictionary<string, object> PopulateSerializableDictionary();

		void ApplyDictionaryValues(Dictionary<string, object> values);
	}
}
