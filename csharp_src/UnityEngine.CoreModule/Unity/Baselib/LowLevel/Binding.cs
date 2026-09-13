using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace Unity.Baselib.LowLevel;

[NativeHeader("External/baselib/builds/CSharp/UnityBinding/Baselib_RegisteredNetwork.gen.binding.h")]
[NativeHeader("External/baselib/builds/CSharp/UnityBinding/Baselib_ErrorState.gen.binding.h")]
[NativeHeader("External/baselib/builds/CSharp/UnityBinding/Baselib_SourceLocation.gen.binding.h")]
[NativeHeader("External/baselib/builds/CSharp/UnityBinding/Baselib_ErrorCode.gen.binding.h")]
[NativeHeader("External/baselib/builds/CSharp/UnityBinding/Baselib_NetworkAddress.gen.binding.h")]
[NativeHeader("External/baselib/builds/CSharp/UnityBinding/Baselib_Memory.gen.binding.h")]
internal static class Binding
{
	public enum Baselib_ErrorCode
	{
		Success = 0,
		OutOfMemory = 16777216,
		OutOfSystemResources = 16777217,
		InvalidAddressRange = 16777218,
		InvalidArgument = 16777219,
		InvalidBufferSize = 16777220,
		InvalidState = 16777221,
		NotSupported = 16777222,
		Timeout = 16777223,
		UnsupportedAlignment = 33554432,
		InvalidPageSize = 33554433,
		InvalidPageCount = 33554434,
		UnsupportedPageState = 33554435,
		UninitializedThreadConfig = 50331648,
		ThreadEntryPointFunctionNotSet = 50331649,
		ThreadCannotJoinSelf = 50331650,
		NetworkInitializationError = 67108864,
		AddressInUse = 67108865,
		AddressUnreachable = 67108866,
		AddressFamilyNotSupported = 67108867,
		UnexpectedError = -1
	}

	public enum Baselib_ErrorState_NativeErrorCodeType : byte
	{
		None,
		PlatformDefined
	}

	public struct Baselib_ErrorState
	{
		public Baselib_ErrorCode code;

		public Baselib_ErrorState_NativeErrorCodeType nativeErrorCodeType;

		public ulong nativeErrorCode;

		public Baselib_SourceLocation sourceLocation;
	}

	public enum Baselib_ErrorState_ExplainVerbosity
	{
		ErrorType,
		ErrorType_SourceLocation_Explanation
	}

	public struct Baselib_Memory_PageSizeInfo
	{
		public ulong defaultPageSize;

		public ulong pageSizes0;

		public ulong pageSizes1;

		public ulong pageSizes2;

		public ulong pageSizes3;

		public ulong pageSizes4;

		public ulong pageSizes5;

		public ulong pageSizesLen;
	}

	public struct Baselib_Memory_PageAllocation
	{
		public IntPtr ptr;

		public ulong pageSize;

		public ulong pageCount;
	}

	public enum Baselib_Memory_PageState
	{
		Reserved = 0,
		NoAccess = 1,
		ReadOnly = 2,
		ReadWrite = 4,
		ReadOnly_Executable = 18,
		ReadWrite_Executable = 20
	}

	public enum Baselib_NetworkAddress_Family
	{
		Invalid,
		IPv4,
		IPv6
	}

	public struct Baselib_NetworkAddress
	{
		public byte data0;

		public byte data1;

		public byte data2;

		public byte data3;

		public byte data4;

		public byte data5;

		public byte data6;

		public byte data7;

		public byte data8;

		public byte data9;

		public byte data10;

		public byte data11;

		public byte data12;

		public byte data13;

		public byte data14;

		public byte data15;

		public byte port0;

		public byte port1;

		public byte family;

		public byte _padding;
	}

	public enum Baselib_NetworkAddress_AddressReuse
	{
		DoNotAllow,
		Allow
	}

	public struct Baselib_RegisteredNetwork_Buffer
	{
		public IntPtr id;

		public Baselib_Memory_PageAllocation allocation;
	}

	public struct Baselib_RegisteredNetwork_BufferSlice
	{
		public IntPtr id;

		public IntPtr data;

		public uint size;

		public uint offset;
	}

	public struct Baselib_RegisteredNetwork_Endpoint
	{
		public Baselib_RegisteredNetwork_BufferSlice slice;
	}

	public struct Baselib_RegisteredNetwork_Request
	{
		public Baselib_RegisteredNetwork_BufferSlice payload;

		public Baselib_RegisteredNetwork_Endpoint remoteEndpoint;

		public IntPtr requestUserdata;
	}

	public enum Baselib_RegisteredNetwork_CompletionStatus
	{
		Failed,
		Success
	}

	public struct Baselib_RegisteredNetwork_CompletionResult
	{
		public Baselib_RegisteredNetwork_CompletionStatus status;

		public uint bytesTransferred;

		public IntPtr requestUserdata;
	}

	public struct Baselib_RegisteredNetwork_Socket_UDP
	{
		public IntPtr handle;
	}

	public enum Baselib_RegisteredNetwork_ProcessStatus
	{
		Done,
		Pending
	}

	public enum Baselib_RegisteredNetwork_CompletionQueueStatus
	{
		NoResultsAvailable,
		ResultsAvailable
	}

	public struct Baselib_SourceLocation
	{
		public unsafe byte* file;

		public unsafe byte* function;

		public uint lineNumber;
	}

	public static readonly UIntPtr Baselib_Memory_MaxAlignment = new UIntPtr(65536u);

	public static readonly Baselib_Memory_PageAllocation Baselib_Memory_PageAllocation_Invalid = default(Baselib_Memory_PageAllocation);

	public const uint Baselib_NetworkAddress_IpMaxStringLength = 46u;

	public static readonly IntPtr Baselib_RegisteredNetwork_Buffer_Id_Invalid = IntPtr.Zero;

	public const uint Baselib_RegisteredNetwork_Endpoint_MaxSize = 28u;

	public static readonly Baselib_RegisteredNetwork_Socket_UDP Baselib_RegisteredNetwork_Socket_UDP_Invalid = default(Baselib_RegisteredNetwork_Socket_UDP);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public unsafe static extern uint Baselib_ErrorState_Explain(Baselib_ErrorState* errorState, byte* buffer, uint bufferLen, Baselib_ErrorState_ExplainVerbosity verbosity);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public unsafe static extern void Baselib_Memory_GetPageSizeInfo(Baselib_Memory_PageSizeInfo* outPagesSizeInfo);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public static extern IntPtr Baselib_Memory_Allocate(UIntPtr size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public static extern IntPtr Baselib_Memory_Reallocate(IntPtr ptr, UIntPtr newSize);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public static extern void Baselib_Memory_Free(IntPtr ptr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public static extern IntPtr Baselib_Memory_AlignedAllocate(UIntPtr size, UIntPtr alignment);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public static extern IntPtr Baselib_Memory_AlignedReallocate(IntPtr ptr, UIntPtr newSize, UIntPtr alignment);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public static extern void Baselib_Memory_AlignedFree(IntPtr ptr);

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_Memory_PageAllocation Baselib_Memory_AllocatePages(ulong pageSize, ulong pageCount, ulong alignmentInMultipleOfPageSize, Baselib_Memory_PageState pageState, Baselib_ErrorState* errorState)
	{
		Baselib_Memory_AllocatePages_Injected(pageSize, pageCount, alignmentInMultipleOfPageSize, pageState, errorState, out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static void Baselib_Memory_ReleasePages(Baselib_Memory_PageAllocation pageAllocation, Baselib_ErrorState* errorState)
	{
		Baselib_Memory_ReleasePages_Injected(ref pageAllocation, errorState);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public unsafe static extern void Baselib_Memory_SetPageState(IntPtr addressOfFirstPage, ulong pageSize, ulong pageCount, Baselib_Memory_PageState pageState, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public unsafe static extern void Baselib_NetworkAddress_Encode(Baselib_NetworkAddress* dstAddress, Baselib_NetworkAddress_Family family, byte* ip, ushort port, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(IsThreadSafe = true)]
	public unsafe static extern void Baselib_NetworkAddress_Decode(Baselib_NetworkAddress* srcAddress, Baselib_NetworkAddress_Family* family, byte* ipAddressBuffer, uint ipAddressBufferLen, ushort* port, Baselib_ErrorState* errorState);

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_Buffer Baselib_RegisteredNetwork_Buffer_Register(Baselib_Memory_PageAllocation pageAllocation, Baselib_ErrorState* errorState)
	{
		Baselib_RegisteredNetwork_Buffer_Register_Injected(ref pageAllocation, errorState, out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public static void Baselib_RegisteredNetwork_Buffer_Deregister(Baselib_RegisteredNetwork_Buffer buffer)
	{
		Baselib_RegisteredNetwork_Buffer_Deregister_Injected(ref buffer);
	}

	[FreeFunction(IsThreadSafe = true)]
	public static Baselib_RegisteredNetwork_BufferSlice Baselib_RegisteredNetwork_BufferSlice_Create(Baselib_RegisteredNetwork_Buffer buffer, uint offset, uint size)
	{
		Baselib_RegisteredNetwork_BufferSlice_Create_Injected(ref buffer, offset, size, out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public static Baselib_RegisteredNetwork_BufferSlice Baselib_RegisteredNetwork_BufferSlice_Empty()
	{
		Baselib_RegisteredNetwork_BufferSlice_Empty_Injected(out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_Endpoint Baselib_RegisteredNetwork_Endpoint_Create(Baselib_NetworkAddress* srcAddress, Baselib_RegisteredNetwork_BufferSlice dstSlice, Baselib_ErrorState* errorState)
	{
		Baselib_RegisteredNetwork_Endpoint_Create_Injected(srcAddress, ref dstSlice, errorState, out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public static Baselib_RegisteredNetwork_Endpoint Baselib_RegisteredNetwork_Endpoint_Empty()
	{
		Baselib_RegisteredNetwork_Endpoint_Empty_Injected(out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static void Baselib_RegisteredNetwork_Endpoint_GetNetworkAddress(Baselib_RegisteredNetwork_Endpoint endpoint, Baselib_NetworkAddress* dstAddress, Baselib_ErrorState* errorState)
	{
		Baselib_RegisteredNetwork_Endpoint_GetNetworkAddress_Injected(ref endpoint, dstAddress, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_Socket_UDP Baselib_RegisteredNetwork_Socket_UDP_Create(Baselib_NetworkAddress* bindAddress, Baselib_NetworkAddress_AddressReuse endpointReuse, uint sendQueueSize, uint recvQueueSize, Baselib_ErrorState* errorState)
	{
		Baselib_RegisteredNetwork_Socket_UDP_Create_Injected(bindAddress, endpointReuse, sendQueueSize, recvQueueSize, errorState, out var ret);
		return ret;
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static uint Baselib_RegisteredNetwork_Socket_UDP_ScheduleRecv(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_Request* requests, uint requestsCount, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_ScheduleRecv_Injected(ref socket, requests, requestsCount, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static uint Baselib_RegisteredNetwork_Socket_UDP_ScheduleSend(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_Request* requests, uint requestsCount, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_ScheduleSend_Injected(ref socket, requests, requestsCount, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_ProcessStatus Baselib_RegisteredNetwork_Socket_UDP_ProcessRecv(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_ProcessRecv_Injected(ref socket, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_ProcessStatus Baselib_RegisteredNetwork_Socket_UDP_ProcessSend(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_ProcessSend_Injected(ref socket, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_CompletionQueueStatus Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedRecv(Baselib_RegisteredNetwork_Socket_UDP socket, uint timeoutInMilliseconds, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedRecv_Injected(ref socket, timeoutInMilliseconds, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static Baselib_RegisteredNetwork_CompletionQueueStatus Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedSend(Baselib_RegisteredNetwork_Socket_UDP socket, uint timeoutInMilliseconds, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedSend_Injected(ref socket, timeoutInMilliseconds, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static uint Baselib_RegisteredNetwork_Socket_UDP_DequeueRecv(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_CompletionResult* results, uint resultsCount, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_DequeueRecv_Injected(ref socket, results, resultsCount, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static uint Baselib_RegisteredNetwork_Socket_UDP_DequeueSend(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_CompletionResult* results, uint resultsCount, Baselib_ErrorState* errorState)
	{
		return Baselib_RegisteredNetwork_Socket_UDP_DequeueSend_Injected(ref socket, results, resultsCount, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public unsafe static void Baselib_RegisteredNetwork_Socket_UDP_GetNetworkAddress(Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_NetworkAddress* dstAddress, Baselib_ErrorState* errorState)
	{
		Baselib_RegisteredNetwork_Socket_UDP_GetNetworkAddress_Injected(ref socket, dstAddress, errorState);
	}

	[FreeFunction(IsThreadSafe = true)]
	public static void Baselib_RegisteredNetwork_Socket_UDP_Close(Baselib_RegisteredNetwork_Socket_UDP socket)
	{
		Baselib_RegisteredNetwork_Socket_UDP_Close_Injected(ref socket);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_Memory_AllocatePages_Injected(ulong pageSize, ulong pageCount, ulong alignmentInMultipleOfPageSize, Baselib_Memory_PageState pageState, Baselib_ErrorState* errorState, out Baselib_Memory_PageAllocation ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_Memory_ReleasePages_Injected(ref Baselib_Memory_PageAllocation pageAllocation, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_RegisteredNetwork_Buffer_Register_Injected(ref Baselib_Memory_PageAllocation pageAllocation, Baselib_ErrorState* errorState, out Baselib_RegisteredNetwork_Buffer ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Baselib_RegisteredNetwork_Buffer_Deregister_Injected(ref Baselib_RegisteredNetwork_Buffer buffer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Baselib_RegisteredNetwork_BufferSlice_Create_Injected(ref Baselib_RegisteredNetwork_Buffer buffer, uint offset, uint size, out Baselib_RegisteredNetwork_BufferSlice ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Baselib_RegisteredNetwork_BufferSlice_Empty_Injected(out Baselib_RegisteredNetwork_BufferSlice ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_RegisteredNetwork_Endpoint_Create_Injected(Baselib_NetworkAddress* srcAddress, ref Baselib_RegisteredNetwork_BufferSlice dstSlice, Baselib_ErrorState* errorState, out Baselib_RegisteredNetwork_Endpoint ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Baselib_RegisteredNetwork_Endpoint_Empty_Injected(out Baselib_RegisteredNetwork_Endpoint ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_RegisteredNetwork_Endpoint_GetNetworkAddress_Injected(ref Baselib_RegisteredNetwork_Endpoint endpoint, Baselib_NetworkAddress* dstAddress, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_RegisteredNetwork_Socket_UDP_Create_Injected(Baselib_NetworkAddress* bindAddress, Baselib_NetworkAddress_AddressReuse endpointReuse, uint sendQueueSize, uint recvQueueSize, Baselib_ErrorState* errorState, out Baselib_RegisteredNetwork_Socket_UDP ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern uint Baselib_RegisteredNetwork_Socket_UDP_ScheduleRecv_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_Request* requests, uint requestsCount, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern uint Baselib_RegisteredNetwork_Socket_UDP_ScheduleSend_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_Request* requests, uint requestsCount, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern Baselib_RegisteredNetwork_ProcessStatus Baselib_RegisteredNetwork_Socket_UDP_ProcessRecv_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern Baselib_RegisteredNetwork_ProcessStatus Baselib_RegisteredNetwork_Socket_UDP_ProcessSend_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern Baselib_RegisteredNetwork_CompletionQueueStatus Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedRecv_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, uint timeoutInMilliseconds, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern Baselib_RegisteredNetwork_CompletionQueueStatus Baselib_RegisteredNetwork_Socket_UDP_WaitForCompletedSend_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, uint timeoutInMilliseconds, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern uint Baselib_RegisteredNetwork_Socket_UDP_DequeueRecv_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_CompletionResult* results, uint resultsCount, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern uint Baselib_RegisteredNetwork_Socket_UDP_DequeueSend_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_RegisteredNetwork_CompletionResult* results, uint resultsCount, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void Baselib_RegisteredNetwork_Socket_UDP_GetNetworkAddress_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket, Baselib_NetworkAddress* dstAddress, Baselib_ErrorState* errorState);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Baselib_RegisteredNetwork_Socket_UDP_Close_Injected(ref Baselib_RegisteredNetwork_Socket_UDP socket);
}
