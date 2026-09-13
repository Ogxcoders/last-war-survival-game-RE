using System;
using System.Runtime.InteropServices;
using Unity.Baselib.LowLevel;

namespace Unity.Baselib;

internal struct ErrorState
{
	private Binding.Baselib_ErrorState nativeErrorState;

	public Binding.Baselib_ErrorCode ErrorCode => nativeErrorState.code;

	public unsafe Binding.Baselib_ErrorState* NativeErrorStatePtr
	{
		get
		{
			fixed (Binding.Baselib_ErrorState* result = &nativeErrorState)
			{
				return result;
			}
		}
	}

	public void ThrowIfFailed()
	{
		if (ErrorCode != Binding.Baselib_ErrorCode.Success)
		{
			throw new BaselibException(this);
		}
	}

	public unsafe string Explain(Binding.Baselib_ErrorState_ExplainVerbosity verbosity = Binding.Baselib_ErrorState_ExplainVerbosity.ErrorType_SourceLocation_Explanation)
	{
		fixed (Binding.Baselib_ErrorState* errorState = &nativeErrorState)
		{
			uint num = Binding.Baselib_ErrorState_Explain(errorState, null, 0u, verbosity) + 1;
			IntPtr intPtr = Binding.Baselib_Memory_Allocate(new UIntPtr(num));
			try
			{
				Binding.Baselib_ErrorState_Explain(errorState, (byte*)(void*)intPtr, num, verbosity);
				return Marshal.PtrToStringAnsi(intPtr);
			}
			finally
			{
				Binding.Baselib_Memory_Free(intPtr);
			}
		}
	}
}
