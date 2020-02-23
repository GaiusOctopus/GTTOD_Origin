using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Color32Message : MessageBase
	{
		public Color32 value;

		public string id;

		public Color32Message()
		{
		}

		public Color32Message(Color32 value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = reader.ReadColor32();
			id = reader.ReadString();
		}
	}
}
