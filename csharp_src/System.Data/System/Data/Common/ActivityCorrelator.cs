using System.Globalization;

namespace System.Data.Common;

internal static class ActivityCorrelator
{
	internal class ActivityId
	{
		internal Guid Id { get; private set; }

		internal uint Sequence { get; private set; }

		internal ActivityId()
		{
			Id = Guid.NewGuid();
			Sequence = 0u;
		}

		internal ActivityId(ActivityId activity)
		{
			Id = activity.Id;
			Sequence = activity.Sequence;
		}

		internal void Increment()
		{
			uint sequence = Sequence + 1;
			Sequence = sequence;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Id, Sequence);
		}
	}

	[ThreadStatic]
	private static ActivityId t_tlsActivity;

	internal static ActivityId Current
	{
		get
		{
			if (t_tlsActivity == null)
			{
				t_tlsActivity = new ActivityId();
			}
			return new ActivityId(t_tlsActivity);
		}
	}

	internal static ActivityId Next()
	{
		if (t_tlsActivity == null)
		{
			t_tlsActivity = new ActivityId();
		}
		t_tlsActivity.Increment();
		return new ActivityId(t_tlsActivity);
	}
}
