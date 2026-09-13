using Unity;

namespace System.Configuration;

public sealed class TimeSpanMinutesOrInfiniteConverter : TimeSpanMinutesConverter
{
	public TimeSpanMinutesOrInfiniteConverter()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
