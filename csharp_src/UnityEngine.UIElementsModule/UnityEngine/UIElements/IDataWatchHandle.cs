using System;

namespace UnityEngine.UIElements;

internal interface IDataWatchHandle : IDisposable
{
	Object watched { get; }

	bool disposed { get; }
}
