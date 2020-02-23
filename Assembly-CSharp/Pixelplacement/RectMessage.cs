using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class RectMessage : MessageBase
	{
		public Rect value;

		public string id;

		public RectMessage()
		{
		}

		public RectMessage(Rect value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = reader.ReadRect();
			id = reader.ReadString();
		}
	}
}
