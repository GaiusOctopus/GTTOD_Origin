using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class ColorArrayMessage : MessageBase
	{
		public Color[] value;

		public string id;

		public ColorArrayMessage()
		{
		}

		public ColorArrayMessage(Color[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayColor_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayColor_None(reader);
			id = reader.ReadString();
		}
	}
}
