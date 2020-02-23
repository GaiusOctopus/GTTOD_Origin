using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Color32ArrayMessage : MessageBase
	{
		public Color32[] value;

		public string id;

		public Color32ArrayMessage()
		{
		}

		public Color32ArrayMessage(Color32[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayColor32_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayColor32_None(reader);
			id = reader.ReadString();
		}
	}
}
