using UnityEngine;

namespace Pixelplacement
{
	public class LayerMaskHelper
	{
		public static int OnlyIncluding(params int[] layers)
		{
			return MakeMask(layers);
		}

		public static int Everything()
		{
			return -1;
		}

		public static int Default()
		{
			return 1;
		}

		public static int Nothing()
		{
			return 0;
		}

		public static int EverythingBut(params int[] layers)
		{
			return ~MakeMask(layers);
		}

		public static bool ContainsLayer(LayerMask layerMask, int layer)
		{
			return (layerMask.value & (1 << layer)) != 0;
		}

		private static int MakeMask(params int[] layers)
		{
			int num = 0;
			foreach (int num2 in layers)
			{
				num |= 1 << num2;
			}
			return num;
		}
	}
}
