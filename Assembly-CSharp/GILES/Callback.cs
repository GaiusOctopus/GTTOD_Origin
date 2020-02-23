namespace GILES
{
	public delegate void Callback();
	public delegate void Callback<T>(T value);
	public delegate void Callback<T, S>(T value0, S value1);
}
