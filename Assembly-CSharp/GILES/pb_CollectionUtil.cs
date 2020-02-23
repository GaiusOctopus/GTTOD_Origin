namespace GILES
{
	public static class pb_CollectionUtil
	{
		public static T[] Fill<T>(T value, int length)
		{
			T[] array = new T[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = value;
			}
			return array;
		}
	}
}
