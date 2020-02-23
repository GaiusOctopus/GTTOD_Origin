using Unity;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class StringArrayMessage : MessageBase
	{
		public string[] value;

		public string id;

		public StringArrayMessage()
		{
		}

		public StringArrayMessage(string[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayString_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayString_None(reader);
			id = reader.ReadString();
		}
	}
}
