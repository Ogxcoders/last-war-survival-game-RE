namespace System.Linq.Parallel;

internal class Shared<T>
{
	internal T Value;

	internal Shared(T value)
	{
		Value = value;
	}
}
