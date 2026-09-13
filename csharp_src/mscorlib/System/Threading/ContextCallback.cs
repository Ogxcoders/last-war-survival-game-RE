using System.Runtime.InteropServices;

namespace System.Threading;

[ComVisible(true)]
public delegate void ContextCallback(object state);
internal delegate void ContextCallback<TState>(ref TState state);
