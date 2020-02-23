using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Vector4ArrayMessage : MessageBase
	{
		public Vector4[] value;

		public string id;

		public Vector4ArrayMessage()
		{
		}

		public Vector4ArrayMessage(Vector4[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayVector4_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayVector4_None(reader);
			id = reader.ReadString();
		}
	}
}
