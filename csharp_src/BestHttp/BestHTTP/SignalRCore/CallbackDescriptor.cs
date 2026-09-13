using System;

namespace BestHTTP.SignalRCore;

internal struct CallbackDescriptor
{
	public readonly Type[] ParamTypes;

	public readonly Action<object[]> Callback;

	public CallbackDescriptor(Type[] paramTypes, Action<object[]> callback)
	{
		ParamTypes = paramTypes;
		Callback = callback;
	}
}
