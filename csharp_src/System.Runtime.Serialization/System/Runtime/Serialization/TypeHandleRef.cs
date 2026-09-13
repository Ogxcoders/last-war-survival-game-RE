namespace System.Runtime.Serialization;

internal class TypeHandleRef
{
	private RuntimeTypeHandle value;

	public RuntimeTypeHandle Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public TypeHandleRef()
	{
	}

	public TypeHandleRef(RuntimeTypeHandle value)
	{
		this.value = value;
	}
}
