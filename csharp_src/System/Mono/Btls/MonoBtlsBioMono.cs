using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Btls;

internal class MonoBtlsBioMono : MonoBtlsBio
{
	private enum ControlCommand
	{
		Flush = 1
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int BioReadFunc(IntPtr bio, IntPtr data, int dataLength, out int wantMore);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate int BioWriteFunc(IntPtr bio, IntPtr data, int dataLength);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate long BioControlFunc(IntPtr bio, ControlCommand command, long arg);

	private class StreamBackend : IMonoBtlsBioMono
	{
		private Stream stream;

		private bool ownsStream;

		public Stream InnerStream => stream;

		public StreamBackend(Stream stream, bool ownsStream)
		{
			this.stream = stream;
			this.ownsStream = ownsStream;
		}

		public int Read(byte[] buffer, int offset, int size, out bool wantMore)
		{
			wantMore = false;
			return stream.Read(buffer, offset, size);
		}

		public bool Write(byte[] buffer, int offset, int size)
		{
			stream.Write(buffer, offset, size);
			return true;
		}

		public void Flush()
		{
			stream.Flush();
		}

		public void Close()
		{
			if (ownsStream && stream != null)
			{
				stream.Dispose();
			}
			stream = null;
		}
	}

	private class StringBackend : IMonoBtlsBioMono
	{
		private StringWriter writer;

		private Encoding encoding = new UTF8Encoding();

		public StringBackend(StringWriter writer)
		{
			this.writer = writer;
		}

		public int Read(byte[] buffer, int offset, int size, out bool wantMore)
		{
			wantMore = false;
			return -1;
		}

		public bool Write(byte[] buffer, int offset, int size)
		{
			string value = encoding.GetString(buffer, offset, size);
			writer.Write(value);
			return true;
		}

		public void Flush()
		{
		}

		public void Close()
		{
		}
	}

	private GCHandle handle;

	private IntPtr instance;

	private BioReadFunc readFunc;

	private BioWriteFunc writeFunc;

	private BioControlFunc controlFunc;

	private IntPtr readFuncPtr;

	private IntPtr writeFuncPtr;

	private IntPtr controlFuncPtr;

	private IMonoBtlsBioMono backend;

	public MonoBtlsBioMono(IMonoBtlsBioMono backend)
		: base(new BoringBioHandle(mono_btls_bio_mono_new()))
	{
		this.backend = backend;
		handle = GCHandle.Alloc(this);
		instance = GCHandle.ToIntPtr(handle);
		readFunc = OnRead;
		writeFunc = OnWrite;
		controlFunc = Control;
		readFuncPtr = Marshal.GetFunctionPointerForDelegate(readFunc);
		writeFuncPtr = Marshal.GetFunctionPointerForDelegate(writeFunc);
		controlFuncPtr = Marshal.GetFunctionPointerForDelegate(controlFunc);
		mono_btls_bio_mono_initialize(base.Handle.DangerousGetHandle(), instance, readFuncPtr, writeFuncPtr, controlFuncPtr);
	}

	public static MonoBtlsBioMono CreateStream(Stream stream, bool ownsStream)
	{
		return new MonoBtlsBioMono(new StreamBackend(stream, ownsStream));
	}

	public static MonoBtlsBioMono CreateString(StringWriter writer)
	{
		return new MonoBtlsBioMono(new StringBackend(writer));
	}

	[DllImport("libmono-btls-shared")]
	private static extern IntPtr mono_btls_bio_mono_new();

	[DllImport("libmono-btls-shared")]
	private static extern void mono_btls_bio_mono_initialize(IntPtr handle, IntPtr instance, IntPtr readFunc, IntPtr writeFunc, IntPtr controlFunc);

	private long Control(ControlCommand command, long arg)
	{
		if (command == ControlCommand.Flush)
		{
			backend.Flush();
			return 1L;
		}
		throw new NotImplementedException();
	}

	private int OnRead(IntPtr data, int dataLength, out int wantMore)
	{
		byte[] array = new byte[dataLength];
		bool wantMore2;
		int num = backend.Read(array, 0, dataLength, out wantMore2);
		wantMore = (wantMore2 ? 1 : 0);
		if (num <= 0)
		{
			return num;
		}
		Marshal.Copy(array, 0, data, num);
		return num;
	}

	private static int OnRead(IntPtr instance, IntPtr data, int dataLength, out int wantMore)
	{
		MonoBtlsBioMono monoBtlsBioMono = (MonoBtlsBioMono)GCHandle.FromIntPtr(instance).Target;
		try
		{
			return monoBtlsBioMono.OnRead(data, dataLength, out wantMore);
		}
		catch (Exception exception)
		{
			monoBtlsBioMono.SetException(exception);
			wantMore = 0;
			return -1;
		}
	}

	private int OnWrite(IntPtr data, int dataLength)
	{
		byte[] array = new byte[dataLength];
		Marshal.Copy(data, array, 0, dataLength);
		if (!backend.Write(array, 0, dataLength))
		{
			return -1;
		}
		return dataLength;
	}

	private static int OnWrite(IntPtr instance, IntPtr data, int dataLength)
	{
		MonoBtlsBioMono monoBtlsBioMono = (MonoBtlsBioMono)GCHandle.FromIntPtr(instance).Target;
		try
		{
			return monoBtlsBioMono.OnWrite(data, dataLength);
		}
		catch (Exception exception)
		{
			monoBtlsBioMono.SetException(exception);
			return -1;
		}
	}

	private static long Control(IntPtr instance, ControlCommand command, long arg)
	{
		MonoBtlsBioMono monoBtlsBioMono = (MonoBtlsBioMono)GCHandle.FromIntPtr(instance).Target;
		try
		{
			return monoBtlsBioMono.Control(command, arg);
		}
		catch (Exception exception)
		{
			monoBtlsBioMono.SetException(exception);
			return -1L;
		}
	}

	protected override void Close()
	{
		try
		{
			if (backend != null)
			{
				backend.Close();
				backend = null;
			}
			if (handle.IsAllocated)
			{
				handle.Free();
			}
		}
		finally
		{
			base.Close();
		}
	}
}
