using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Vector3Message : MessageBase
	{
		public Vector3 value;

		public string id;

		public Vector3Message()
		{
		}

		public Vector3Message(Vector3 value, string id)
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
			value = reader.ReadVector3();
			id = reader.ReadString();
		}
	}
}
