using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Vector4Message : MessageBase
	{
		public Vector4 value;

		public string id;

		public Vector4Message()
		{
		}

		public Vector4Message(Vector4 value, string id)
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
			value = reader.ReadVector4();
			id = reader.ReadString();
		}
	}
}
