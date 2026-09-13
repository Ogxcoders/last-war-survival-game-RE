namespace System.Diagnostics.Tracing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class EventDataAttribute : Attribute
{
	[MonoTODO]
	public string Name
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}
}
