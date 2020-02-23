using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class QuaternionMessage : MessageBase
	{
		public Quaternion value;

		public string id;

		public QuaternionMessage()
		{
		}

		public QuaternionMessage(Quaternion value, string id)
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
			value = reader.ReadQuaternion();
			id = reader.ReadString();
		}
	}
}
