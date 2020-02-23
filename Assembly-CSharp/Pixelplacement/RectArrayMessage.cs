using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class RectArrayMessage : MessageBase
	{
		public Rect[] value;

		public string id;

		public RectArrayMessage()
		{
		}

		public RectArrayMessage(Rect[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayRect_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayRect_None(reader);
			id = reader.ReadString();
		}
	}
}
