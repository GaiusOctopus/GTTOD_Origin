using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class RectByteArrayMessage : MessageBase
	{
		public Rect rect;

		public byte[] bytes;

		public string id;

		public RectByteArrayMessage()
		{
		}

		public RectByteArrayMessage(Rect rect, byte[] bytes, string id)
		{
			this.rect = rect;
			this.bytes = bytes;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(rect);
			writer.WriteBytesFull(bytes);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			rect = reader.ReadRect();
			bytes = reader.ReadBytesAndSize();
			id = reader.ReadString();
		}
	}
}
