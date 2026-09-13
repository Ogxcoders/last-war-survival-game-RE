using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class ToBase64Transform : ICryptoTransform, IDisposable
{
	public int InputBlockSize => 3;

	public int OutputBlockSize => 4;

	public bool CanTransformMultipleBlocks => false;

	public virtual bool CanReuseTransform => true;

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
	{
		if (inputBuffer == null)
		{
			throw new ArgumentNullException("inputBuffer");
		}
		if (inputOffset < 0)
		{
			throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("Non-negative number required."));
		}
		if (inputCount < 0 || inputCount > inputBuffer.Length)
		{
			throw new ArgumentException(Environment.GetResourceString("Value was invalid."));
		}
		if (inputBuffer.Length - inputCount < inputOffset)
		{
			throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
		}
		char[] array = new char[4];
		Convert.ToBase64CharArray(inputBuffer, inputOffset, 3, array, 0);
		byte[] bytes = Encoding.ASCII.GetBytes(array);
		if (bytes.Length != 4)
		{
			throw new CryptographicException(Environment.GetResourceString("Length of the data to encrypt is invalid."));
		}
		Buffer.BlockCopy(bytes, 0, outputBuffer, outputOffset, bytes.Length);
		return bytes.Length;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		if (inputBuffer == null)
		{
			throw new ArgumentNullException("inputBuffer");
		}
		if (inputOffset < 0)
		{
			throw new ArgumentOutOfRangeException("inputOffset", Environment.GetResourceString("Non-negative number required."));
		}
		if (inputCount < 0 || inputCount > inputBuffer.Length)
		{
			throw new ArgumentException(Environment.GetResourceString("Value was invalid."));
		}
		if (inputBuffer.Length - inputCount < inputOffset)
		{
			throw new ArgumentException(Environment.GetResourceString("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection."));
		}
		if (inputCount == 0)
		{
			return EmptyArray<byte>.Value;
		}
		char[] array = new char[4];
		Convert.ToBase64CharArray(inputBuffer, inputOffset, inputCount, array, 0);
		return Encoding.ASCII.GetBytes(array);
	}

	public void Dispose()
	{
		Clear();
	}

	public void Clear()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~ToBase64Transform()
	{
		Dispose(disposing: false);
	}
}
