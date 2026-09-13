namespace System.Threading.Tasks;

internal class Box<T>
{
	internal T Value;

	internal Box(T value)
	{
		Value = value;
	}
}
