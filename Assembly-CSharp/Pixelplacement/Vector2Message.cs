using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Vector2Message : MessageBase
	{
		public Vector2 value;

		public string id;

		public Vector2Message()
		{
		}

		public Vector2Message(Vector2 value, string id)
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
			value = reader.ReadVector2();
			id = reader.ReadString();
		}
	}
}
