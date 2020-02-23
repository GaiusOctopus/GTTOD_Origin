using UnityEngine.Networking;

namespace Pixelplacement
{
	public class ByteArrayMessage : MessageBase
	{
		public byte[] value;

		public string id;

		public ByteArrayMessage()
		{
		}

		public ByteArrayMessage(byte[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.WriteBytesFull(value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = reader.ReadBytesAndSize();
			id = reader.ReadString();
		}
	}
}
