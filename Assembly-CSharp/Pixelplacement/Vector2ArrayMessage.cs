using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Pixelplacement
{
	public class Vector2ArrayMessage : MessageBase
	{
		public Vector2[] value;

		public string id;

		public Vector2ArrayMessage()
		{
		}

		public Vector2ArrayMessage(Vector2[] value, string id)
		{
			this.value = value;
			this.id = id;
		}

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteArrayVector2_None(writer, value);
			writer.Write(id);
		}

		public override void Deserialize(NetworkReader reader)
		{
			value = GeneratedNetworkCode._ReadArrayVector2_None(reader);
			id = reader.ReadString();
		}
	}
}
