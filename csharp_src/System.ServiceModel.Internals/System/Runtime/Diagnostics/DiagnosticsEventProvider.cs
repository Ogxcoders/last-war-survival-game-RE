using System.Globalization;
using System.Runtime.Interop;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Diagnostics;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
internal abstract class DiagnosticsEventProvider : IDisposable
{
	public enum WriteEventErrorCode
	{
		NoError,
		NoFreeBuffers,
		EventTooBig
	}

	[SecurityCritical]
	private UnsafeNativeMethods.EtwEnableCallback etwCallback;

	private long traceRegistrationHandle;

	private byte currentTraceLevel;

	private long anyKeywordMask;

	private long allKeywordMask;

	private bool isProviderEnabled;

	private Guid providerId;

	private int isDisposed;

	[ThreadStatic]
	private static WriteEventErrorCode errorCode;

	private const int basicTypeAllocationBufferSize = 16;

	private const int etwMaxNumberArguments = 32;

	private const int etwAPIMaxStringCount = 8;

	private const int maxEventDataDescriptors = 128;

	private const int traceEventMaximumSize = 65482;

	private const int traceEventMaximumStringSize = 32724;

	private const int WindowsVistaMajorNumber = 6;

	[SecurityCritical]
	[PermissionSet(SecurityAction.Demand, Unrestricted = true)]
	protected DiagnosticsEventProvider(Guid providerGuid)
	{
		providerId = providerGuid;
		int platform = (int)Environment.OSVersion.Platform;
		if (platform != 4 && platform != 128)
		{
			EtwRegister();
		}
	}

	[SecurityCritical]
	private unsafe void EtwRegister()
	{
		etwCallback = EtwEnableCallBack;
		uint num = UnsafeNativeMethods.EventRegister(ref providerId, etwCallback, null, ref traceRegistrationHandle);
		if (num != 0)
		{
			throw new InvalidOperationException(InternalSR.EtwRegistrationFailed(num.ToString("x", CultureInfo.CurrentCulture)));
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	[SecuritySafeCritical]
	protected virtual void Dispose(bool disposing)
	{
		if (isDisposed != 1 && Interlocked.Exchange(ref isDisposed, 1) == 0)
		{
			isProviderEnabled = false;
			Deregister();
		}
	}

	public virtual void Close()
	{
		Dispose();
	}

	~DiagnosticsEventProvider()
	{
		Dispose(disposing: false);
	}

	[SecurityCritical]
	private void Deregister()
	{
		if (traceRegistrationHandle != 0L)
		{
			UnsafeNativeMethods.EventUnregister(traceRegistrationHandle);
			traceRegistrationHandle = 0L;
		}
	}

	[SecurityCritical]
	private unsafe void EtwEnableCallBack([In] ref Guid sourceId, [In] int isEnabled, [In] byte setLevel, [In] long anyKeyword, [In] long allKeyword, [In] void* filterData, [In] void* callbackContext)
	{
		isProviderEnabled = isEnabled != 0;
		currentTraceLevel = setLevel;
		anyKeywordMask = anyKeyword;
		allKeywordMask = allKeyword;
		OnControllerCommand();
	}

	protected abstract void OnControllerCommand();

	public bool IsEnabled()
	{
		return isProviderEnabled;
	}

	public bool IsEnabled(byte level, long keywords)
	{
		if (isProviderEnabled && (level <= currentTraceLevel || currentTraceLevel == 0) && (keywords == 0L || ((keywords & anyKeywordMask) != 0L && (keywords & allKeywordMask) == allKeywordMask)))
		{
			return true;
		}
		return false;
	}

	[SecurityCritical]
	public bool IsEventEnabled(ref EventDescriptor eventDescriptor)
	{
		if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
		{
			return UnsafeNativeMethods.EventEnabled(traceRegistrationHandle, ref eventDescriptor);
		}
		return false;
	}

	public static WriteEventErrorCode GetLastWriteEventError()
	{
		return errorCode;
	}

	private static void SetLastError(int error)
	{
		switch (error)
		{
		case 234:
		case 534:
			errorCode = WriteEventErrorCode.EventTooBig;
			break;
		case 8:
			errorCode = WriteEventErrorCode.NoFreeBuffers;
			break;
		}
	}

	[SecurityCritical]
	private unsafe static string EncodeObject(ref object data, UnsafeNativeMethods.EventData* dataDescriptor, byte* dataBuffer)
	{
		dataDescriptor->Reserved = 0;
		if (data is string text)
		{
			dataDescriptor->Size = (uint)((text.Length + 1) * 2);
			return text;
		}
		if (data is IntPtr)
		{
			dataDescriptor->Size = (uint)sizeof(IntPtr);
			*(IntPtr*)dataBuffer = (IntPtr)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is int)
		{
			dataDescriptor->Size = 4u;
			*(int*)dataBuffer = (int)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is long)
		{
			dataDescriptor->Size = 8u;
			*(long*)dataBuffer = (long)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is uint)
		{
			dataDescriptor->Size = 4u;
			*(uint*)dataBuffer = (uint)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is ulong)
		{
			dataDescriptor->Size = 8u;
			*(ulong*)dataBuffer = (ulong)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is char)
		{
			dataDescriptor->Size = 2u;
			*(char*)dataBuffer = (char)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is byte)
		{
			dataDescriptor->Size = 1u;
			*dataBuffer = (byte)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is short)
		{
			dataDescriptor->Size = 2u;
			*(short*)dataBuffer = (short)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is sbyte)
		{
			dataDescriptor->Size = 1u;
			*dataBuffer = (byte)(sbyte)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is ushort)
		{
			dataDescriptor->Size = 2u;
			*(ushort*)dataBuffer = (ushort)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is float)
		{
			dataDescriptor->Size = 4u;
			*(float*)dataBuffer = (float)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is double)
		{
			dataDescriptor->Size = 8u;
			*(double*)dataBuffer = (double)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is bool)
		{
			dataDescriptor->Size = 1u;
			*dataBuffer = (((bool)data) ? ((byte)1) : ((byte)0));
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is Guid)
		{
			dataDescriptor->Size = (uint)sizeof(Guid);
			*(Guid*)dataBuffer = (Guid)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else if (data is decimal)
		{
			dataDescriptor->Size = 16u;
			*(decimal*)dataBuffer = (decimal)data;
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		else
		{
			if (!(data is bool))
			{
				string text2 = data.ToString();
				dataDescriptor->Size = (uint)((text2.Length + 1) * 2);
				return text2;
			}
			dataDescriptor->Size = 1u;
			*dataBuffer = (((bool)data) ? ((byte)1) : ((byte)0));
			dataDescriptor->DataPointer = (ulong)dataBuffer;
		}
		return null;
	}

	[SecurityCritical]
	public unsafe bool WriteMessageEvent(EventTraceActivity eventTraceActivity, string eventMessage, byte eventLevel, long eventKeywords)
	{
		int num = 0;
		if (eventMessage == null)
		{
			throw Fx.Exception.AsError(new ArgumentNullException("eventMessage"));
		}
		if (eventTraceActivity != null)
		{
			SetActivityId(ref eventTraceActivity.ActivityId);
		}
		if (IsEnabled(eventLevel, eventKeywords))
		{
			if (eventMessage.Length > 32724)
			{
				errorCode = WriteEventErrorCode.EventTooBig;
				return false;
			}
			fixed (char* message = eventMessage)
			{
				num = (int)UnsafeNativeMethods.EventWriteString(traceRegistrationHandle, eventLevel, eventKeywords, message);
			}
			if (num != 0)
			{
				SetLastError(num);
				return false;
			}
		}
		return true;
	}

	[SecurityCritical]
	public bool WriteMessageEvent(EventTraceActivity eventTraceActivity, string eventMessage)
	{
		return WriteMessageEvent(eventTraceActivity, eventMessage, 0, 0L);
	}

	[SecurityCritical]
	public unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, params object[] eventPayload)
	{
		uint num = 0u;
		if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
		{
			int num2 = 0;
			if (eventTraceActivity != null)
			{
				SetActivityId(ref eventTraceActivity.ActivityId);
			}
			if (eventPayload == null || eventPayload.Length == 0 || eventPayload.Length == 1)
			{
				string text = null;
				byte* dataBuffer = stackalloc byte[16];
				UnsafeNativeMethods.EventData eventData = default(UnsafeNativeMethods.EventData);
				eventData.Size = 0u;
				if (eventPayload != null && eventPayload.Length != 0)
				{
					text = EncodeObject(ref eventPayload[0], &eventData, dataBuffer);
					num2 = 1;
				}
				if (eventData.Size > 65482)
				{
					errorCode = WriteEventErrorCode.EventTooBig;
					return false;
				}
				if (text == null)
				{
					num = ((num2 != 0) ? UnsafeNativeMethods.EventWrite(traceRegistrationHandle, ref eventDescriptor, (uint)num2, &eventData) : UnsafeNativeMethods.EventWrite(traceRegistrationHandle, ref eventDescriptor, 0u, null));
				}
				else
				{
					fixed (char* ptr = text)
					{
						eventData.DataPointer = (ulong)ptr;
						num = UnsafeNativeMethods.EventWrite(traceRegistrationHandle, ref eventDescriptor, (uint)num2, &eventData);
					}
				}
			}
			else
			{
				num2 = eventPayload.Length;
				if (num2 > 32)
				{
					throw Fx.Exception.AsError(new ArgumentOutOfRangeException("eventPayload", InternalSR.EtwMaxNumberArgumentsExceeded(32)));
				}
				uint num3 = 0u;
				int num4 = 0;
				int[] array = new int[8];
				string[] array2 = new string[8];
				UnsafeNativeMethods.EventData* ptr2 = stackalloc UnsafeNativeMethods.EventData[num2];
				UnsafeNativeMethods.EventData* ptr3 = ptr2;
				byte* ptr4 = stackalloc byte[(int)(uint)(16 * num2)];
				for (int i = 0; i < eventPayload.Length; i++)
				{
					if (eventPayload[i] == null)
					{
						continue;
					}
					string text2 = EncodeObject(ref eventPayload[i], ptr3, ptr4);
					ptr4 += 16;
					num3 += ptr3->Size;
					ptr3++;
					if (text2 != null)
					{
						if (num4 >= 8)
						{
							throw Fx.Exception.AsError(new ArgumentOutOfRangeException("eventPayload", InternalSR.EtwAPIMaxStringCountExceeded(8)));
						}
						array2[num4] = text2;
						array[num4] = i;
						num4++;
					}
				}
				if (num3 > 65482)
				{
					errorCode = WriteEventErrorCode.EventTooBig;
					return false;
				}
				fixed (char* ptr5 = array2[0])
				{
					fixed (char* ptr6 = array2[1])
					{
						fixed (char* ptr7 = array2[2])
						{
							fixed (char* ptr8 = array2[3])
							{
								fixed (char* ptr9 = array2[4])
								{
									fixed (char* ptr10 = array2[5])
									{
										fixed (char* ptr11 = array2[6])
										{
											fixed (char* ptr12 = array2[7])
											{
												ptr3 = ptr2;
												if (array2[0] != null)
												{
													ptr3[array[0]].DataPointer = (ulong)ptr5;
												}
												if (array2[1] != null)
												{
													ptr3[array[1]].DataPointer = (ulong)ptr6;
												}
												if (array2[2] != null)
												{
													ptr3[array[2]].DataPointer = (ulong)ptr7;
												}
												if (array2[3] != null)
												{
													ptr3[array[3]].DataPointer = (ulong)ptr8;
												}
												if (array2[4] != null)
												{
													ptr3[array[4]].DataPointer = (ulong)ptr9;
												}
												if (array2[5] != null)
												{
													ptr3[array[5]].DataPointer = (ulong)ptr10;
												}
												if (array2[6] != null)
												{
													ptr3[array[6]].DataPointer = (ulong)ptr11;
												}
												if (array2[7] != null)
												{
													ptr3[array[7]].DataPointer = (ulong)ptr12;
												}
												num = UnsafeNativeMethods.EventWrite(traceRegistrationHandle, ref eventDescriptor, (uint)num2, ptr2);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (num != 0)
		{
			SetLastError((int)num);
			return false;
		}
		return true;
	}

	[SecurityCritical]
	public unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string data)
	{
		uint num = 0u;
		data = data ?? string.Empty;
		if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
		{
			if (data.Length > 32724)
			{
				errorCode = WriteEventErrorCode.EventTooBig;
				return false;
			}
			if (eventTraceActivity != null)
			{
				SetActivityId(ref eventTraceActivity.ActivityId);
			}
			UnsafeNativeMethods.EventData eventData = default(UnsafeNativeMethods.EventData);
			eventData.Size = (uint)((data.Length + 1) * 2);
			eventData.Reserved = 0;
			fixed (char* ptr = data)
			{
				eventData.DataPointer = (ulong)ptr;
				num = UnsafeNativeMethods.EventWrite(traceRegistrationHandle, ref eventDescriptor, 1u, &eventData);
			}
		}
		if (num != 0)
		{
			SetLastError((int)num);
			return false;
		}
		return true;
	}

	[SecurityCritical]
	protected internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, int dataCount, IntPtr data)
	{
		uint num = 0u;
		if (eventTraceActivity != null)
		{
			SetActivityId(ref eventTraceActivity.ActivityId);
		}
		num = UnsafeNativeMethods.EventWrite(traceRegistrationHandle, ref eventDescriptor, (uint)dataCount, (UnsafeNativeMethods.EventData*)(void*)data);
		if (num != 0)
		{
			SetLastError((int)num);
			return false;
		}
		return true;
	}

	[SecurityCritical]
	public unsafe bool WriteTransferEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid relatedActivityId, params object[] eventPayload)
	{
		if (eventTraceActivity == null)
		{
			eventTraceActivity = EventTraceActivity.Empty;
		}
		uint num = 0u;
		if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
		{
			if (eventPayload != null && eventPayload.Length != 0)
			{
				int num2 = eventPayload.Length;
				if (num2 > 32)
				{
					throw Fx.Exception.AsError(new ArgumentOutOfRangeException("eventPayload", InternalSR.EtwMaxNumberArgumentsExceeded(32)));
				}
				uint num3 = 0u;
				int num4 = 0;
				int[] array = new int[8];
				string[] array2 = new string[8];
				UnsafeNativeMethods.EventData* ptr = stackalloc UnsafeNativeMethods.EventData[num2];
				UnsafeNativeMethods.EventData* ptr2 = ptr;
				byte* ptr3 = stackalloc byte[(int)(uint)(16 * num2)];
				for (int i = 0; i < eventPayload.Length; i++)
				{
					if (eventPayload[i] == null)
					{
						continue;
					}
					string text = EncodeObject(ref eventPayload[i], ptr2, ptr3);
					ptr3 += 16;
					num3 += ptr2->Size;
					ptr2++;
					if (text != null)
					{
						if (num4 >= 8)
						{
							throw Fx.Exception.AsError(new ArgumentOutOfRangeException("eventPayload", InternalSR.EtwAPIMaxStringCountExceeded(8)));
						}
						array2[num4] = text;
						array[num4] = i;
						num4++;
					}
				}
				if (num3 > 65482)
				{
					errorCode = WriteEventErrorCode.EventTooBig;
					return false;
				}
				fixed (char* ptr4 = array2[0])
				{
					fixed (char* ptr5 = array2[1])
					{
						fixed (char* ptr6 = array2[2])
						{
							fixed (char* ptr7 = array2[3])
							{
								fixed (char* ptr8 = array2[4])
								{
									fixed (char* ptr9 = array2[5])
									{
										fixed (char* ptr10 = array2[6])
										{
											fixed (char* ptr11 = array2[7])
											{
												ptr2 = ptr;
												if (array2[0] != null)
												{
													ptr2[array[0]].DataPointer = (ulong)ptr4;
												}
												if (array2[1] != null)
												{
													ptr2[array[1]].DataPointer = (ulong)ptr5;
												}
												if (array2[2] != null)
												{
													ptr2[array[2]].DataPointer = (ulong)ptr6;
												}
												if (array2[3] != null)
												{
													ptr2[array[3]].DataPointer = (ulong)ptr7;
												}
												if (array2[4] != null)
												{
													ptr2[array[4]].DataPointer = (ulong)ptr8;
												}
												if (array2[5] != null)
												{
													ptr2[array[5]].DataPointer = (ulong)ptr9;
												}
												if (array2[6] != null)
												{
													ptr2[array[6]].DataPointer = (ulong)ptr10;
												}
												if (array2[7] != null)
												{
													ptr2[array[7]].DataPointer = (ulong)ptr11;
												}
												num = UnsafeNativeMethods.EventWriteTransfer(traceRegistrationHandle, ref eventDescriptor, ref eventTraceActivity.ActivityId, ref relatedActivityId, (uint)num2, ptr);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			else
			{
				num = UnsafeNativeMethods.EventWriteTransfer(traceRegistrationHandle, ref eventDescriptor, ref eventTraceActivity.ActivityId, ref relatedActivityId, 0u, null);
			}
		}
		if (num != 0)
		{
			SetLastError((int)num);
			return false;
		}
		return true;
	}

	[SecurityCritical]
	protected unsafe bool WriteTransferEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid relatedActivityId, int dataCount, IntPtr data)
	{
		if (eventTraceActivity == null)
		{
			throw Fx.Exception.ArgumentNull("eventTraceActivity");
		}
		uint num = 0u;
		num = UnsafeNativeMethods.EventWriteTransfer(traceRegistrationHandle, ref eventDescriptor, ref eventTraceActivity.ActivityId, ref relatedActivityId, (uint)dataCount, (UnsafeNativeMethods.EventData*)(void*)data);
		if (num != 0)
		{
			SetLastError((int)num);
			return false;
		}
		return true;
	}

	[SecurityCritical]
	public static void SetActivityId(ref Guid id)
	{
		UnsafeNativeMethods.EventActivityIdControl(2, ref id);
	}
}
