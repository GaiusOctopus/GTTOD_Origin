using Unity;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class BoolArrayMessage : MessageBase
	{
		public bool[] value;

		public string id;

		public BoolArrayMessage()
		{
		}

		public BoolArrayMessage(bool[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayBoolean_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayBoolean_None(reader);
			id = reader.ReadString();
		}
	}
}
