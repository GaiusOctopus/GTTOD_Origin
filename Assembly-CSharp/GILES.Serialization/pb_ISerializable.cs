using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GILES.Serialization
{
	public interface pb_ISerializable : ISerializable
	{
		Type type
		{
			get;
			set;
		}

		void ApplyProperties(object obj);

		Dictionary<string, object> PopulateSerializableDictionary();
	}
}
