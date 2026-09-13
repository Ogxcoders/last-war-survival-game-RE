namespace System.Diagnostics.Tracing;

[AttributeUsage(AttributeTargets.Property)]
public class EventFieldAttribute : Attribute
{
	[MonoTODO]
	public EventFieldFormat Format
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

	[MonoTODO]
	public EventFieldTags Tags
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
