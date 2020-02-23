using UnityEngine.Networking;

namespace Pixelplacement
{
	public class ByteMessage : MessageBase
	{
		public byte value;

		public string id;

		public ByteMessage()
		{
		}

		public ByteMessage(byte value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32(value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = (byte)reader.ReadPackedUInt32();
			id = reader.ReadString();
		}
	}
}
