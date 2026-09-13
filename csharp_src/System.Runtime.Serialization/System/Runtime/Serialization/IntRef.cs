namespace System.Runtime.Serialization;

internal class IntRef
{
	private int value;

	public int Value => value;

	public IntRef(int value)
	{
		this.value = value;
	}
}
