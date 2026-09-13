using System.Threading;

namespace System.Linq.Parallel;

internal class IntValueEvent : ManualResetEventSlim
{
	internal int Value;

	internal IntValueEvent()
		: base(initialState: false)
	{
		Value = 0;
	}

	internal void Set(int index)
	{
		Value = index;
		Set();
	}
}
