using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography;

[ComVisible(true)]
public sealed class CryptoAPITransform : ICryptoTransform, IDisposable
{
	private bool m_disposed;

	public bool CanReuseTransform => true;

	public bool CanTransformMultipleBlocks => true;

	public int InputBlockSize => 0;

	public IntPtr KeyHandle
	{
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		get
		{
			return IntPtr.Zero;
		}
	}

	public int OutputBlockSize => 0;

	internal CryptoAPITransform()
	{
		m_disposed = false;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void Clear()
	{
		Dispose(disposing: false);
	}

	private void Dispose(bool disposing)
	{
		if (!m_disposed)
		{
			m_disposed = true;
		}
	}

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		return 0;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		return null;
	}

	[ComVisible(false)]
	public void Reset()
	{
	}
}
