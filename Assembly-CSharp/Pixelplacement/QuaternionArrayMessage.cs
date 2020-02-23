using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class QuaternionArrayMessage : MessageBase
	{
		public Quaternion[] value;

		public string id;

		public QuaternionArrayMessage()
		{
		}

		public QuaternionArrayMessage(Quaternion[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayQuaternion_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayQuaternion_None(reader);
			id = reader.ReadString();
		}
	}
}
