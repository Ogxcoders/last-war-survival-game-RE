using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Sequential)]
internal sealed class MonoCQItem
{
	private object[] array;

	private byte[] array_state;

	private int head;

	private int tail;
}
