using System.Runtime.Interop;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Diagnostics;

internal sealed class EtwProvider : DiagnosticsEventProvider
{
	private Action invokeControllerCallback;

	private bool end2EndActivityTracingEnabled;

	internal Action ControllerCallBack
	{
		get
		{
			return invokeControllerCallback;
		}
		set
		{
			invokeControllerCallback = value;
		}
	}

	internal bool IsEnd2EndActivityTracingEnabled => end2EndActivityTracingEnabled;

	[SecurityCritical]
	[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
	internal EtwProvider(Guid id)
		: base(id)
	{
	}

	protected override void OnControllerCommand()
	{
		end2EndActivityTracingEnabled = false;
		if (invokeControllerCallback != null)
		{
			invokeControllerCallback();
		}
	}

	internal void SetEnd2EndActivityTracingEnabled(bool isEnd2EndActivityTracingEnabled)
	{
		end2EndActivityTracingEnabled = isEnd2EndActivityTracingEnabled;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, string value2, string value3)
	{
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		bool result;
		fixed (char* ptr = value2)
		{
			fixed (char* ptr2 = value3)
			{
				byte* ptr3 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 3)];
				UnsafeNativeMethods.EventData* ptr4 = (UnsafeNativeMethods.EventData*)ptr3;
				ptr4->DataPointer = (ulong)(&value1);
				ptr4->Size = (uint)sizeof(Guid);
				ptr4[1].DataPointer = (ulong)ptr;
				ptr4[1].Size = (uint)((value2.Length + 1) * 2);
				ptr4[2].DataPointer = (ulong)ptr2;
				ptr4[2].Size = (uint)((value3.Length + 1) * 2);
				result = WriteEvent(ref eventDescriptor, eventTraceActivity, 3, (IntPtr)ptr3);
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteTransferEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid relatedActivityId, string value1, string value2)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				byte* ptr3 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 2)];
				UnsafeNativeMethods.EventData* ptr4 = (UnsafeNativeMethods.EventData*)ptr3;
				ptr4->DataPointer = (ulong)ptr;
				ptr4->Size = (uint)((value1.Length + 1) * 2);
				ptr4[1].DataPointer = (ulong)ptr2;
				ptr4[1].Size = (uint)((value2.Length + 1) * 2);
				result = WriteTransferEvent(ref eventDescriptor, eventTraceActivity, relatedActivityId, 2, (IntPtr)ptr3);
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				byte* ptr3 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 2)];
				UnsafeNativeMethods.EventData* ptr4 = (UnsafeNativeMethods.EventData*)ptr3;
				ptr4->DataPointer = (ulong)ptr;
				ptr4->Size = (uint)((value1.Length + 1) * 2);
				ptr4[1].DataPointer = (ulong)ptr2;
				ptr4[1].Size = (uint)((value2.Length + 1) * 2);
				result = WriteEvent(ref eventDescriptor, eventTraceActivity, 2, (IntPtr)ptr3);
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					byte* ptr4 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 3)];
					UnsafeNativeMethods.EventData* ptr5 = (UnsafeNativeMethods.EventData*)ptr4;
					ptr5->DataPointer = (ulong)ptr;
					ptr5->Size = (uint)((value1.Length + 1) * 2);
					ptr5[1].DataPointer = (ulong)ptr2;
					ptr5[1].Size = (uint)((value2.Length + 1) * 2);
					ptr5[2].DataPointer = (ulong)ptr3;
					ptr5[2].Size = (uint)((value3.Length + 1) * 2);
					result = WriteEvent(ref eventDescriptor, eventTraceActivity, 3, (IntPtr)ptr4);
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						byte* ptr5 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 4)];
						UnsafeNativeMethods.EventData* ptr6 = (UnsafeNativeMethods.EventData*)ptr5;
						ptr6->DataPointer = (ulong)ptr;
						ptr6->Size = (uint)((value1.Length + 1) * 2);
						ptr6[1].DataPointer = (ulong)ptr2;
						ptr6[1].Size = (uint)((value2.Length + 1) * 2);
						ptr6[2].DataPointer = (ulong)ptr3;
						ptr6[2].Size = (uint)((value3.Length + 1) * 2);
						ptr6[3].DataPointer = (ulong)ptr4;
						ptr6[3].Size = (uint)((value4.Length + 1) * 2);
						result = WriteEvent(ref eventDescriptor, eventTraceActivity, 4, (IntPtr)ptr5);
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							byte* ptr6 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 5)];
							UnsafeNativeMethods.EventData* ptr7 = (UnsafeNativeMethods.EventData*)ptr6;
							ptr7->DataPointer = (ulong)ptr;
							ptr7->Size = (uint)((value1.Length + 1) * 2);
							ptr7[1].DataPointer = (ulong)ptr2;
							ptr7[1].Size = (uint)((value2.Length + 1) * 2);
							ptr7[2].DataPointer = (ulong)ptr3;
							ptr7[2].Size = (uint)((value3.Length + 1) * 2);
							ptr7[3].DataPointer = (ulong)ptr4;
							ptr7[3].Size = (uint)((value4.Length + 1) * 2);
							ptr7[4].DataPointer = (ulong)ptr5;
							ptr7[4].Size = (uint)((value5.Length + 1) * 2);
							result = WriteEvent(ref eventDescriptor, eventTraceActivity, 5, (IntPtr)ptr6);
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								byte* ptr7 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 6)];
								UnsafeNativeMethods.EventData* ptr8 = (UnsafeNativeMethods.EventData*)ptr7;
								ptr8->DataPointer = (ulong)ptr;
								ptr8->Size = (uint)((value1.Length + 1) * 2);
								ptr8[1].DataPointer = (ulong)ptr2;
								ptr8[1].Size = (uint)((value2.Length + 1) * 2);
								ptr8[2].DataPointer = (ulong)ptr3;
								ptr8[2].Size = (uint)((value3.Length + 1) * 2);
								ptr8[3].DataPointer = (ulong)ptr4;
								ptr8[3].Size = (uint)((value4.Length + 1) * 2);
								ptr8[4].DataPointer = (ulong)ptr5;
								ptr8[4].Size = (uint)((value5.Length + 1) * 2);
								ptr8[5].DataPointer = (ulong)ptr6;
								ptr8[5].Size = (uint)((value6.Length + 1) * 2);
								result = WriteEvent(ref eventDescriptor, eventTraceActivity, 6, (IntPtr)ptr7);
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									byte* ptr8 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 7)];
									UnsafeNativeMethods.EventData* ptr9 = (UnsafeNativeMethods.EventData*)ptr8;
									ptr9->DataPointer = (ulong)ptr;
									ptr9->Size = (uint)((value1.Length + 1) * 2);
									ptr9[1].DataPointer = (ulong)ptr2;
									ptr9[1].Size = (uint)((value2.Length + 1) * 2);
									ptr9[2].DataPointer = (ulong)ptr3;
									ptr9[2].Size = (uint)((value3.Length + 1) * 2);
									ptr9[3].DataPointer = (ulong)ptr4;
									ptr9[3].Size = (uint)((value4.Length + 1) * 2);
									ptr9[4].DataPointer = (ulong)ptr5;
									ptr9[4].Size = (uint)((value5.Length + 1) * 2);
									ptr9[5].DataPointer = (ulong)ptr6;
									ptr9[5].Size = (uint)((value6.Length + 1) * 2);
									ptr9[6].DataPointer = (ulong)ptr7;
									ptr9[6].Size = (uint)((value7.Length + 1) * 2);
									result = WriteEvent(ref eventDescriptor, eventTraceActivity, 7, (IntPtr)ptr8);
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									fixed (char* ptr8 = value8)
									{
										byte* ptr9 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 8)];
										UnsafeNativeMethods.EventData* ptr10 = (UnsafeNativeMethods.EventData*)ptr9;
										ptr10->DataPointer = (ulong)ptr;
										ptr10->Size = (uint)((value1.Length + 1) * 2);
										ptr10[1].DataPointer = (ulong)ptr2;
										ptr10[1].Size = (uint)((value2.Length + 1) * 2);
										ptr10[2].DataPointer = (ulong)ptr3;
										ptr10[2].Size = (uint)((value3.Length + 1) * 2);
										ptr10[3].DataPointer = (ulong)ptr4;
										ptr10[3].Size = (uint)((value4.Length + 1) * 2);
										ptr10[4].DataPointer = (ulong)ptr5;
										ptr10[4].Size = (uint)((value5.Length + 1) * 2);
										ptr10[5].DataPointer = (ulong)ptr6;
										ptr10[5].Size = (uint)((value6.Length + 1) * 2);
										ptr10[6].DataPointer = (ulong)ptr7;
										ptr10[6].Size = (uint)((value7.Length + 1) * 2);
										ptr10[7].DataPointer = (ulong)ptr8;
										ptr10[7].Size = (uint)((value8.Length + 1) * 2);
										result = WriteEvent(ref eventDescriptor, eventTraceActivity, 8, (IntPtr)ptr9);
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									fixed (char* ptr8 = value8)
									{
										fixed (char* ptr9 = value9)
										{
											byte* ptr10 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 9)];
											UnsafeNativeMethods.EventData* ptr11 = (UnsafeNativeMethods.EventData*)ptr10;
											ptr11->DataPointer = (ulong)ptr;
											ptr11->Size = (uint)((value1.Length + 1) * 2);
											ptr11[1].DataPointer = (ulong)ptr2;
											ptr11[1].Size = (uint)((value2.Length + 1) * 2);
											ptr11[2].DataPointer = (ulong)ptr3;
											ptr11[2].Size = (uint)((value3.Length + 1) * 2);
											ptr11[3].DataPointer = (ulong)ptr4;
											ptr11[3].Size = (uint)((value4.Length + 1) * 2);
											ptr11[4].DataPointer = (ulong)ptr5;
											ptr11[4].Size = (uint)((value5.Length + 1) * 2);
											ptr11[5].DataPointer = (ulong)ptr6;
											ptr11[5].Size = (uint)((value6.Length + 1) * 2);
											ptr11[6].DataPointer = (ulong)ptr7;
											ptr11[6].Size = (uint)((value7.Length + 1) * 2);
											ptr11[7].DataPointer = (ulong)ptr8;
											ptr11[7].Size = (uint)((value8.Length + 1) * 2);
											ptr11[8].DataPointer = (ulong)ptr9;
											ptr11[8].Size = (uint)((value9.Length + 1) * 2);
											result = WriteEvent(ref eventDescriptor, eventTraceActivity, 9, (IntPtr)ptr10);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									fixed (char* ptr8 = value8)
									{
										fixed (char* ptr9 = value9)
										{
											fixed (char* ptr10 = value10)
											{
												byte* ptr11 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 10)];
												UnsafeNativeMethods.EventData* ptr12 = (UnsafeNativeMethods.EventData*)ptr11;
												ptr12->DataPointer = (ulong)ptr;
												ptr12->Size = (uint)((value1.Length + 1) * 2);
												ptr12[1].DataPointer = (ulong)ptr2;
												ptr12[1].Size = (uint)((value2.Length + 1) * 2);
												ptr12[2].DataPointer = (ulong)ptr3;
												ptr12[2].Size = (uint)((value3.Length + 1) * 2);
												ptr12[3].DataPointer = (ulong)ptr4;
												ptr12[3].Size = (uint)((value4.Length + 1) * 2);
												ptr12[4].DataPointer = (ulong)ptr5;
												ptr12[4].Size = (uint)((value5.Length + 1) * 2);
												ptr12[5].DataPointer = (ulong)ptr6;
												ptr12[5].Size = (uint)((value6.Length + 1) * 2);
												ptr12[6].DataPointer = (ulong)ptr7;
												ptr12[6].Size = (uint)((value7.Length + 1) * 2);
												ptr12[7].DataPointer = (ulong)ptr8;
												ptr12[7].Size = (uint)((value8.Length + 1) * 2);
												ptr12[8].DataPointer = (ulong)ptr9;
												ptr12[8].Size = (uint)((value9.Length + 1) * 2);
												ptr12[9].DataPointer = (ulong)ptr10;
												ptr12[9].Size = (uint)((value10.Length + 1) * 2);
												result = WriteEvent(ref eventDescriptor, eventTraceActivity, 10, (IntPtr)ptr11);
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
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									fixed (char* ptr8 = value8)
									{
										fixed (char* ptr9 = value9)
										{
											fixed (char* ptr10 = value10)
											{
												fixed (char* ptr11 = value11)
												{
													byte* ptr12 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 11)];
													UnsafeNativeMethods.EventData* ptr13 = (UnsafeNativeMethods.EventData*)ptr12;
													ptr13->DataPointer = (ulong)ptr;
													ptr13->Size = (uint)((value1.Length + 1) * 2);
													ptr13[1].DataPointer = (ulong)ptr2;
													ptr13[1].Size = (uint)((value2.Length + 1) * 2);
													ptr13[2].DataPointer = (ulong)ptr3;
													ptr13[2].Size = (uint)((value3.Length + 1) * 2);
													ptr13[3].DataPointer = (ulong)ptr4;
													ptr13[3].Size = (uint)((value4.Length + 1) * 2);
													ptr13[4].DataPointer = (ulong)ptr5;
													ptr13[4].Size = (uint)((value5.Length + 1) * 2);
													ptr13[5].DataPointer = (ulong)ptr6;
													ptr13[5].Size = (uint)((value6.Length + 1) * 2);
													ptr13[6].DataPointer = (ulong)ptr7;
													ptr13[6].Size = (uint)((value7.Length + 1) * 2);
													ptr13[7].DataPointer = (ulong)ptr8;
													ptr13[7].Size = (uint)((value8.Length + 1) * 2);
													ptr13[8].DataPointer = (ulong)ptr9;
													ptr13[8].Size = (uint)((value9.Length + 1) * 2);
													ptr13[9].DataPointer = (ulong)ptr10;
													ptr13[9].Size = (uint)((value10.Length + 1) * 2);
													ptr13[10].DataPointer = (ulong)ptr11;
													ptr13[10].Size = (uint)((value11.Length + 1) * 2);
													result = WriteEvent(ref eventDescriptor, eventTraceActivity, 11, (IntPtr)ptr12);
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
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									fixed (char* ptr8 = value8)
									{
										fixed (char* ptr9 = value9)
										{
											fixed (char* ptr10 = value10)
											{
												fixed (char* ptr11 = value11)
												{
													fixed (char* ptr12 = value12)
													{
														byte* ptr13 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 12)];
														UnsafeNativeMethods.EventData* ptr14 = (UnsafeNativeMethods.EventData*)ptr13;
														ptr14->DataPointer = (ulong)ptr;
														ptr14->Size = (uint)((value1.Length + 1) * 2);
														ptr14[1].DataPointer = (ulong)ptr2;
														ptr14[1].Size = (uint)((value2.Length + 1) * 2);
														ptr14[2].DataPointer = (ulong)ptr3;
														ptr14[2].Size = (uint)((value3.Length + 1) * 2);
														ptr14[3].DataPointer = (ulong)ptr4;
														ptr14[3].Size = (uint)((value4.Length + 1) * 2);
														ptr14[4].DataPointer = (ulong)ptr5;
														ptr14[4].Size = (uint)((value5.Length + 1) * 2);
														ptr14[5].DataPointer = (ulong)ptr6;
														ptr14[5].Size = (uint)((value6.Length + 1) * 2);
														ptr14[6].DataPointer = (ulong)ptr7;
														ptr14[6].Size = (uint)((value7.Length + 1) * 2);
														ptr14[7].DataPointer = (ulong)ptr8;
														ptr14[7].Size = (uint)((value8.Length + 1) * 2);
														ptr14[8].DataPointer = (ulong)ptr9;
														ptr14[8].Size = (uint)((value9.Length + 1) * 2);
														ptr14[9].DataPointer = (ulong)ptr10;
														ptr14[9].Size = (uint)((value10.Length + 1) * 2);
														ptr14[10].DataPointer = (ulong)ptr11;
														ptr14[10].Size = (uint)((value11.Length + 1) * 2);
														ptr14[11].DataPointer = (ulong)ptr12;
														ptr14[11].Size = (uint)((value12.Length + 1) * 2);
														result = WriteEvent(ref eventDescriptor, eventTraceActivity, 12, (IntPtr)ptr13);
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
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13)
	{
		value1 = value1 ?? string.Empty;
		value2 = value2 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		value13 = value13 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value2)
			{
				fixed (char* ptr3 = value3)
				{
					fixed (char* ptr4 = value4)
					{
						fixed (char* ptr5 = value5)
						{
							fixed (char* ptr6 = value6)
							{
								fixed (char* ptr7 = value7)
								{
									fixed (char* ptr8 = value8)
									{
										fixed (char* ptr9 = value9)
										{
											fixed (char* ptr10 = value10)
											{
												fixed (char* ptr11 = value11)
												{
													fixed (char* ptr12 = value12)
													{
														fixed (char* ptr13 = value13)
														{
															byte* ptr14 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 13)];
															UnsafeNativeMethods.EventData* ptr15 = (UnsafeNativeMethods.EventData*)ptr14;
															ptr15->DataPointer = (ulong)ptr;
															ptr15->Size = (uint)((value1.Length + 1) * 2);
															ptr15[1].DataPointer = (ulong)ptr2;
															ptr15[1].Size = (uint)((value2.Length + 1) * 2);
															ptr15[2].DataPointer = (ulong)ptr3;
															ptr15[2].Size = (uint)((value3.Length + 1) * 2);
															ptr15[3].DataPointer = (ulong)ptr4;
															ptr15[3].Size = (uint)((value4.Length + 1) * 2);
															ptr15[4].DataPointer = (ulong)ptr5;
															ptr15[4].Size = (uint)((value5.Length + 1) * 2);
															ptr15[5].DataPointer = (ulong)ptr6;
															ptr15[5].Size = (uint)((value6.Length + 1) * 2);
															ptr15[6].DataPointer = (ulong)ptr7;
															ptr15[6].Size = (uint)((value7.Length + 1) * 2);
															ptr15[7].DataPointer = (ulong)ptr8;
															ptr15[7].Size = (uint)((value8.Length + 1) * 2);
															ptr15[8].DataPointer = (ulong)ptr9;
															ptr15[8].Size = (uint)((value9.Length + 1) * 2);
															ptr15[9].DataPointer = (ulong)ptr10;
															ptr15[9].Size = (uint)((value10.Length + 1) * 2);
															ptr15[10].DataPointer = (ulong)ptr11;
															ptr15[10].Size = (uint)((value11.Length + 1) * 2);
															ptr15[11].DataPointer = (ulong)ptr12;
															ptr15[11].Size = (uint)((value12.Length + 1) * 2);
															ptr15[12].DataPointer = (ulong)ptr13;
															ptr15[12].Size = (uint)((value13.Length + 1) * 2);
															result = WriteEvent(ref eventDescriptor, eventTraceActivity, 13, (IntPtr)ptr14);
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
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, int value1)
	{
		byte* ptr = stackalloc byte[(int)(uint)sizeof(UnsafeNativeMethods.EventData)];
		UnsafeNativeMethods.EventData* ptr2 = (UnsafeNativeMethods.EventData*)ptr;
		ptr2->DataPointer = (ulong)(&value1);
		ptr2->Size = 4u;
		return WriteEvent(ref eventDescriptor, eventTraceActivity, 1, (IntPtr)ptr);
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, int value1, int value2)
	{
		byte* ptr = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 2)];
		UnsafeNativeMethods.EventData* ptr2 = (UnsafeNativeMethods.EventData*)ptr;
		ptr2->DataPointer = (ulong)(&value1);
		ptr2->Size = 4u;
		ptr2[1].DataPointer = (ulong)(&value2);
		ptr2[1].Size = 4u;
		return WriteEvent(ref eventDescriptor, eventTraceActivity, 2, (IntPtr)ptr);
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, int value1, int value2, int value3)
	{
		byte* ptr = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 3)];
		UnsafeNativeMethods.EventData* ptr2 = (UnsafeNativeMethods.EventData*)ptr;
		ptr2->DataPointer = (ulong)(&value1);
		ptr2->Size = 4u;
		ptr2[1].DataPointer = (ulong)(&value2);
		ptr2[1].Size = 4u;
		ptr2[2].DataPointer = (ulong)(&value3);
		ptr2[2].Size = 4u;
		return WriteEvent(ref eventDescriptor, eventTraceActivity, 3, (IntPtr)ptr);
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, long value1)
	{
		byte* ptr = stackalloc byte[(int)(uint)sizeof(UnsafeNativeMethods.EventData)];
		UnsafeNativeMethods.EventData* ptr2 = (UnsafeNativeMethods.EventData*)ptr;
		ptr2->DataPointer = (ulong)(&value1);
		ptr2->Size = 8u;
		return WriteEvent(ref eventDescriptor, eventTraceActivity, 1, (IntPtr)ptr);
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, long value1, long value2)
	{
		byte* ptr = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 2)];
		UnsafeNativeMethods.EventData* ptr2 = (UnsafeNativeMethods.EventData*)ptr;
		ptr2->DataPointer = (ulong)(&value1);
		ptr2->Size = 8u;
		ptr2[1].DataPointer = (ulong)(&value2);
		ptr2[1].Size = 8u;
		return WriteEvent(ref eventDescriptor, eventTraceActivity, 2, (IntPtr)ptr);
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, long value1, long value2, long value3)
	{
		byte* ptr = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 3)];
		UnsafeNativeMethods.EventData* ptr2 = (UnsafeNativeMethods.EventData*)ptr;
		ptr2->DataPointer = (ulong)(&value1);
		ptr2->Size = 8u;
		ptr2[1].DataPointer = (ulong)(&value2);
		ptr2[1].Size = 8u;
		ptr2[2].DataPointer = (ulong)(&value3);
		ptr2[2].Size = 8u;
		return WriteEvent(ref eventDescriptor, eventTraceActivity, 3, (IntPtr)ptr);
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13, string value14, string value15)
	{
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		value13 = value13 ?? string.Empty;
		value14 = value14 ?? string.Empty;
		value15 = value15 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value5)
			{
				fixed (char* ptr3 = value6)
				{
					fixed (char* ptr4 = value7)
					{
						fixed (char* ptr5 = value8)
						{
							fixed (char* ptr6 = value9)
							{
								fixed (char* ptr7 = value10)
								{
									fixed (char* ptr8 = value11)
									{
										fixed (char* ptr9 = value12)
										{
											fixed (char* ptr10 = value13)
											{
												fixed (char* ptr11 = value14)
												{
													fixed (char* ptr12 = value15)
													{
														byte* ptr13 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 15)];
														UnsafeNativeMethods.EventData* ptr14 = (UnsafeNativeMethods.EventData*)ptr13;
														ptr14->DataPointer = (ulong)(&value1);
														ptr14->Size = (uint)sizeof(Guid);
														ptr14[1].DataPointer = (ulong)(&value2);
														ptr14[1].Size = 8u;
														ptr14[2].DataPointer = (ulong)(&value3);
														ptr14[2].Size = 8u;
														ptr14[3].DataPointer = (ulong)ptr;
														ptr14[3].Size = (uint)((value4.Length + 1) * 2);
														ptr14[4].DataPointer = (ulong)ptr2;
														ptr14[4].Size = (uint)((value5.Length + 1) * 2);
														ptr14[5].DataPointer = (ulong)ptr3;
														ptr14[5].Size = (uint)((value6.Length + 1) * 2);
														ptr14[6].DataPointer = (ulong)ptr4;
														ptr14[6].Size = (uint)((value7.Length + 1) * 2);
														ptr14[7].DataPointer = (ulong)ptr5;
														ptr14[7].Size = (uint)((value8.Length + 1) * 2);
														ptr14[8].DataPointer = (ulong)ptr6;
														ptr14[8].Size = (uint)((value9.Length + 1) * 2);
														ptr14[9].DataPointer = (ulong)ptr7;
														ptr14[9].Size = (uint)((value10.Length + 1) * 2);
														ptr14[10].DataPointer = (ulong)ptr8;
														ptr14[10].Size = (uint)((value11.Length + 1) * 2);
														ptr14[11].DataPointer = (ulong)ptr9;
														ptr14[11].Size = (uint)((value12.Length + 1) * 2);
														ptr14[12].DataPointer = (ulong)ptr10;
														ptr14[12].Size = (uint)((value13.Length + 1) * 2);
														ptr14[13].DataPointer = (ulong)ptr11;
														ptr14[13].Size = (uint)((value14.Length + 1) * 2);
														ptr14[14].DataPointer = (ulong)ptr12;
														ptr14[14].Size = (uint)((value15.Length + 1) * 2);
														result = WriteEvent(ref eventDescriptor, eventTraceActivity, 15, (IntPtr)ptr13);
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
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, bool value13, string value14, string value15, string value16, string value17)
	{
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		value14 = value14 ?? string.Empty;
		value15 = value15 ?? string.Empty;
		value16 = value16 ?? string.Empty;
		value17 = value17 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value5)
			{
				fixed (char* ptr3 = value6)
				{
					fixed (char* ptr4 = value7)
					{
						fixed (char* ptr5 = value8)
						{
							fixed (char* ptr6 = value9)
							{
								fixed (char* ptr7 = value10)
								{
									fixed (char* ptr8 = value11)
									{
										fixed (char* ptr9 = value12)
										{
											fixed (char* ptr10 = value14)
											{
												fixed (char* ptr11 = value15)
												{
													fixed (char* ptr12 = value16)
													{
														fixed (char* ptr13 = value17)
														{
															byte* ptr14 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 17)];
															UnsafeNativeMethods.EventData* ptr15 = (UnsafeNativeMethods.EventData*)ptr14;
															ptr15->DataPointer = (ulong)(&value1);
															ptr15->Size = (uint)sizeof(Guid);
															ptr15[1].DataPointer = (ulong)(&value2);
															ptr15[1].Size = 8u;
															ptr15[2].DataPointer = (ulong)(&value3);
															ptr15[2].Size = 8u;
															ptr15[3].DataPointer = (ulong)ptr;
															ptr15[3].Size = (uint)((value4.Length + 1) * 2);
															ptr15[4].DataPointer = (ulong)ptr2;
															ptr15[4].Size = (uint)((value5.Length + 1) * 2);
															ptr15[5].DataPointer = (ulong)ptr3;
															ptr15[5].Size = (uint)((value6.Length + 1) * 2);
															ptr15[6].DataPointer = (ulong)ptr4;
															ptr15[6].Size = (uint)((value7.Length + 1) * 2);
															ptr15[7].DataPointer = (ulong)ptr5;
															ptr15[7].Size = (uint)((value8.Length + 1) * 2);
															ptr15[8].DataPointer = (ulong)ptr6;
															ptr15[8].Size = (uint)((value9.Length + 1) * 2);
															ptr15[9].DataPointer = (ulong)ptr7;
															ptr15[9].Size = (uint)((value10.Length + 1) * 2);
															ptr15[10].DataPointer = (ulong)ptr8;
															ptr15[10].Size = (uint)((value11.Length + 1) * 2);
															ptr15[11].DataPointer = (ulong)ptr9;
															ptr15[11].Size = (uint)((value12.Length + 1) * 2);
															ptr15[12].DataPointer = (ulong)(&value13);
															ptr15[12].Size = 1u;
															ptr15[13].DataPointer = (ulong)ptr10;
															ptr15[13].Size = (uint)((value14.Length + 1) * 2);
															ptr15[14].DataPointer = (ulong)ptr11;
															ptr15[14].Size = (uint)((value15.Length + 1) * 2);
															ptr15[15].DataPointer = (ulong)ptr12;
															ptr15[15].Size = (uint)((value16.Length + 1) * 2);
															ptr15[16].DataPointer = (ulong)ptr13;
															ptr15[16].Size = (uint)((value17.Length + 1) * 2);
															result = WriteEvent(ref eventDescriptor, eventTraceActivity, 17, (IntPtr)ptr14);
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
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, string value5, string value6, string value7, string value8, string value9)
	{
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value5)
			{
				fixed (char* ptr3 = value6)
				{
					fixed (char* ptr4 = value7)
					{
						fixed (char* ptr5 = value8)
						{
							fixed (char* ptr6 = value9)
							{
								byte* ptr7 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 9)];
								UnsafeNativeMethods.EventData* ptr8 = (UnsafeNativeMethods.EventData*)ptr7;
								ptr8->DataPointer = (ulong)(&value1);
								ptr8->Size = (uint)sizeof(Guid);
								ptr8[1].DataPointer = (ulong)(&value2);
								ptr8[1].Size = 8u;
								ptr8[2].DataPointer = (ulong)(&value3);
								ptr8[2].Size = 8u;
								ptr8[3].DataPointer = (ulong)ptr;
								ptr8[3].Size = (uint)((value4.Length + 1) * 2);
								ptr8[4].DataPointer = (ulong)ptr2;
								ptr8[4].Size = (uint)((value5.Length + 1) * 2);
								ptr8[5].DataPointer = (ulong)ptr3;
								ptr8[5].Size = (uint)((value6.Length + 1) * 2);
								ptr8[6].DataPointer = (ulong)ptr4;
								ptr8[6].Size = (uint)((value7.Length + 1) * 2);
								ptr8[7].DataPointer = (ulong)ptr5;
								ptr8[7].Size = (uint)((value8.Length + 1) * 2);
								ptr8[8].DataPointer = (ulong)ptr6;
								ptr8[8].Size = (uint)((value9.Length + 1) * 2);
								result = WriteEvent(ref eventDescriptor, eventTraceActivity, 9, (IntPtr)ptr7);
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11)
	{
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value5)
			{
				fixed (char* ptr3 = value6)
				{
					fixed (char* ptr4 = value7)
					{
						fixed (char* ptr5 = value8)
						{
							fixed (char* ptr6 = value9)
							{
								fixed (char* ptr7 = value10)
								{
									fixed (char* ptr8 = value11)
									{
										byte* ptr9 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 11)];
										UnsafeNativeMethods.EventData* ptr10 = (UnsafeNativeMethods.EventData*)ptr9;
										ptr10->DataPointer = (ulong)(&value1);
										ptr10->Size = (uint)sizeof(Guid);
										ptr10[1].DataPointer = (ulong)(&value2);
										ptr10[1].Size = 8u;
										ptr10[2].DataPointer = (ulong)(&value3);
										ptr10[2].Size = 8u;
										ptr10[3].DataPointer = (ulong)ptr;
										ptr10[3].Size = (uint)((value4.Length + 1) * 2);
										ptr10[4].DataPointer = (ulong)ptr2;
										ptr10[4].Size = (uint)((value5.Length + 1) * 2);
										ptr10[5].DataPointer = (ulong)ptr3;
										ptr10[5].Size = (uint)((value6.Length + 1) * 2);
										ptr10[6].DataPointer = (ulong)ptr4;
										ptr10[6].Size = (uint)((value7.Length + 1) * 2);
										ptr10[7].DataPointer = (ulong)ptr5;
										ptr10[7].Size = (uint)((value8.Length + 1) * 2);
										ptr10[8].DataPointer = (ulong)ptr6;
										ptr10[8].Size = (uint)((value9.Length + 1) * 2);
										ptr10[9].DataPointer = (ulong)ptr7;
										ptr10[9].Size = (uint)((value10.Length + 1) * 2);
										ptr10[10].DataPointer = (ulong)ptr8;
										ptr10[10].Size = (uint)((value11.Length + 1) * 2);
										result = WriteEvent(ref eventDescriptor, eventTraceActivity, 11, (IntPtr)ptr9);
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13)
	{
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		value13 = value13 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value5)
			{
				fixed (char* ptr3 = value6)
				{
					fixed (char* ptr4 = value7)
					{
						fixed (char* ptr5 = value8)
						{
							fixed (char* ptr6 = value9)
							{
								fixed (char* ptr7 = value10)
								{
									fixed (char* ptr8 = value11)
									{
										fixed (char* ptr9 = value12)
										{
											fixed (char* ptr10 = value13)
											{
												byte* ptr11 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 13)];
												UnsafeNativeMethods.EventData* ptr12 = (UnsafeNativeMethods.EventData*)ptr11;
												ptr12->DataPointer = (ulong)(&value1);
												ptr12->Size = (uint)sizeof(Guid);
												ptr12[1].DataPointer = (ulong)(&value2);
												ptr12[1].Size = 8u;
												ptr12[2].DataPointer = (ulong)(&value3);
												ptr12[2].Size = 8u;
												ptr12[3].DataPointer = (ulong)ptr;
												ptr12[3].Size = (uint)((value4.Length + 1) * 2);
												ptr12[4].DataPointer = (ulong)ptr2;
												ptr12[4].Size = (uint)((value5.Length + 1) * 2);
												ptr12[5].DataPointer = (ulong)ptr3;
												ptr12[5].Size = (uint)((value6.Length + 1) * 2);
												ptr12[6].DataPointer = (ulong)ptr4;
												ptr12[6].Size = (uint)((value7.Length + 1) * 2);
												ptr12[7].DataPointer = (ulong)ptr5;
												ptr12[7].Size = (uint)((value8.Length + 1) * 2);
												ptr12[8].DataPointer = (ulong)ptr6;
												ptr12[8].Size = (uint)((value9.Length + 1) * 2);
												ptr12[9].DataPointer = (ulong)ptr7;
												ptr12[9].Size = (uint)((value10.Length + 1) * 2);
												ptr12[10].DataPointer = (ulong)ptr8;
												ptr12[10].Size = (uint)((value11.Length + 1) * 2);
												ptr12[11].DataPointer = (ulong)ptr9;
												ptr12[11].Size = (uint)((value12.Length + 1) * 2);
												ptr12[12].DataPointer = (ulong)ptr10;
												ptr12[12].Size = (uint)((value13.Length + 1) * 2);
												result = WriteEvent(ref eventDescriptor, eventTraceActivity, 13, (IntPtr)ptr11);
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
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13, string value14)
	{
		value4 = value4 ?? string.Empty;
		value5 = value5 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		value13 = value13 ?? string.Empty;
		value14 = value14 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value5)
			{
				fixed (char* ptr3 = value6)
				{
					fixed (char* ptr4 = value7)
					{
						fixed (char* ptr5 = value8)
						{
							fixed (char* ptr6 = value9)
							{
								fixed (char* ptr7 = value10)
								{
									fixed (char* ptr8 = value11)
									{
										fixed (char* ptr9 = value12)
										{
											fixed (char* ptr10 = value13)
											{
												fixed (char* ptr11 = value14)
												{
													byte* ptr12 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 14)];
													UnsafeNativeMethods.EventData* ptr13 = (UnsafeNativeMethods.EventData*)ptr12;
													ptr13->DataPointer = (ulong)(&value1);
													ptr13->Size = (uint)sizeof(Guid);
													ptr13[1].DataPointer = (ulong)(&value2);
													ptr13[1].Size = 8u;
													ptr13[2].DataPointer = (ulong)(&value3);
													ptr13[2].Size = 8u;
													ptr13[3].DataPointer = (ulong)ptr;
													ptr13[3].Size = (uint)((value4.Length + 1) * 2);
													ptr13[4].DataPointer = (ulong)ptr2;
													ptr13[4].Size = (uint)((value5.Length + 1) * 2);
													ptr13[5].DataPointer = (ulong)ptr3;
													ptr13[5].Size = (uint)((value6.Length + 1) * 2);
													ptr13[6].DataPointer = (ulong)ptr4;
													ptr13[6].Size = (uint)((value7.Length + 1) * 2);
													ptr13[7].DataPointer = (ulong)ptr5;
													ptr13[7].Size = (uint)((value8.Length + 1) * 2);
													ptr13[8].DataPointer = (ulong)ptr6;
													ptr13[8].Size = (uint)((value9.Length + 1) * 2);
													ptr13[9].DataPointer = (ulong)ptr7;
													ptr13[9].Size = (uint)((value10.Length + 1) * 2);
													ptr13[10].DataPointer = (ulong)ptr8;
													ptr13[10].Size = (uint)((value11.Length + 1) * 2);
													ptr13[11].DataPointer = (ulong)ptr9;
													ptr13[11].Size = (uint)((value12.Length + 1) * 2);
													ptr13[12].DataPointer = (ulong)ptr10;
													ptr13[12].Size = (uint)((value13.Length + 1) * 2);
													ptr13[13].DataPointer = (ulong)ptr11;
													ptr13[13].Size = (uint)((value14.Length + 1) * 2);
													result = WriteEvent(ref eventDescriptor, eventTraceActivity, 14, (IntPtr)ptr12);
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
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, Guid value1, long value2, long value3, string value4, Guid value5, string value6, string value7, string value8, string value9, string value10, string value11, string value12, string value13)
	{
		value4 = value4 ?? string.Empty;
		value6 = value6 ?? string.Empty;
		value7 = value7 ?? string.Empty;
		value8 = value8 ?? string.Empty;
		value9 = value9 ?? string.Empty;
		value10 = value10 ?? string.Empty;
		value11 = value11 ?? string.Empty;
		value12 = value12 ?? string.Empty;
		value13 = value13 ?? string.Empty;
		bool result;
		fixed (char* ptr = value4)
		{
			fixed (char* ptr2 = value6)
			{
				fixed (char* ptr3 = value7)
				{
					fixed (char* ptr4 = value8)
					{
						fixed (char* ptr5 = value9)
						{
							fixed (char* ptr6 = value10)
							{
								fixed (char* ptr7 = value11)
								{
									fixed (char* ptr8 = value12)
									{
										fixed (char* ptr9 = value13)
										{
											byte* ptr10 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 13)];
											UnsafeNativeMethods.EventData* ptr11 = (UnsafeNativeMethods.EventData*)ptr10;
											ptr11->DataPointer = (ulong)(&value1);
											ptr11->Size = (uint)sizeof(Guid);
											ptr11[1].DataPointer = (ulong)(&value2);
											ptr11[1].Size = 8u;
											ptr11[2].DataPointer = (ulong)(&value3);
											ptr11[2].Size = 8u;
											ptr11[3].DataPointer = (ulong)ptr;
											ptr11[3].Size = (uint)((value4.Length + 1) * 2);
											ptr11[4].DataPointer = (ulong)(&value5);
											ptr11[4].Size = (uint)sizeof(Guid);
											ptr11[5].DataPointer = (ulong)ptr2;
											ptr11[5].Size = (uint)((value6.Length + 1) * 2);
											ptr11[6].DataPointer = (ulong)ptr3;
											ptr11[6].Size = (uint)((value7.Length + 1) * 2);
											ptr11[7].DataPointer = (ulong)ptr4;
											ptr11[7].Size = (uint)((value8.Length + 1) * 2);
											ptr11[8].DataPointer = (ulong)ptr5;
											ptr11[8].Size = (uint)((value9.Length + 1) * 2);
											ptr11[9].DataPointer = (ulong)ptr6;
											ptr11[9].Size = (uint)((value10.Length + 1) * 2);
											ptr11[10].DataPointer = (ulong)ptr7;
											ptr11[10].Size = (uint)((value11.Length + 1) * 2);
											ptr11[11].DataPointer = (ulong)ptr8;
											ptr11[11].Size = (uint)((value12.Length + 1) * 2);
											ptr11[12].DataPointer = (ulong)ptr9;
											ptr11[12].Size = (uint)((value13.Length + 1) * 2);
											result = WriteEvent(ref eventDescriptor, eventTraceActivity, 13, (IntPtr)ptr10);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	[SecurityCritical]
	internal unsafe bool WriteEvent(ref EventDescriptor eventDescriptor, EventTraceActivity eventTraceActivity, string value1, long value2, string value3, string value4)
	{
		value1 = value1 ?? string.Empty;
		value3 = value3 ?? string.Empty;
		value4 = value4 ?? string.Empty;
		bool result;
		fixed (char* ptr = value1)
		{
			fixed (char* ptr2 = value3)
			{
				fixed (char* ptr3 = value4)
				{
					byte* ptr4 = stackalloc byte[(int)(uint)(sizeof(UnsafeNativeMethods.EventData) * 4)];
					UnsafeNativeMethods.EventData* ptr5 = (UnsafeNativeMethods.EventData*)ptr4;
					ptr5->DataPointer = (ulong)ptr;
					ptr5->Size = (uint)((value1.Length + 1) * 2);
					ptr5[1].DataPointer = (ulong)(&value2);
					ptr5[1].Size = 8u;
					ptr5[2].DataPointer = (ulong)ptr2;
					ptr5[2].Size = (uint)((value3.Length + 1) * 2);
					ptr5[3].DataPointer = (ulong)ptr3;
					ptr5[3].Size = (uint)((value4.Length + 1) * 2);
					result = WriteEvent(ref eventDescriptor, eventTraceActivity, 4, (IntPtr)ptr4);
				}
			}
		}
		return result;
	}
}
