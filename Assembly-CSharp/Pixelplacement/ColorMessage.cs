using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class ColorMessage : MessageBase
	{
		public Color value;

		public string id;

		public ColorMessage()
		{
		}

		public ColorMessage(Color value, string id)
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
			value = reader.ReadColor();
			id = reader.ReadString();
		}
	}
}
