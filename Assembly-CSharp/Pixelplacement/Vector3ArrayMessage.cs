using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Vector3ArrayMessage : MessageBase
	{
		public Vector3[] value;

		public string id;

		public Vector3ArrayMessage()
		{
		}

		public Vector3ArrayMessage(Vector3[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayVector3_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayVector3_None(reader);
			id = reader.ReadString();
		}
	}
}
