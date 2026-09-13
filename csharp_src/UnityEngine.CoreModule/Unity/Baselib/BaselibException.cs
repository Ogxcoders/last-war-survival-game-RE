using System;
using Unity.Baselib.LowLevel;

namespace Unity.Baselib;

internal class BaselibException : Exception
{
	private readonly ErrorState errorState;

	public Binding.Baselib_ErrorCode ErrorCode => errorState.ErrorCode;

	internal BaselibException(ErrorState errorState)
		: base(errorState.Explain())
	{
		this.errorState = errorState;
	}
}
