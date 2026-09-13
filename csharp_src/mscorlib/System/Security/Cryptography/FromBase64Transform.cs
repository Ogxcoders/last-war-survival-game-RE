using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class FromBase64Transform : ICryptoTransform, IDisposable
{
	private byte[] _inputBuffer = new byte[4];

	private int _inputIndex;

	private FromBase64TransformMode _whitespaces;

	public int InputBlockSize => 1;

	public int OutputBlockSize => 3;

	public bool CanTransformMultipleBlocks => false;

	public virtual bool CanReuseTransform => true;

	public FromBase64Transform()
		: this(FromBase64TransformMode.IgnoreWhiteSpaces)
	{
	}

	public FromBase64Transform(FromBase64TransformMode whitespaces)
	{
		_whitespaces = whitespaces;
		_inputIndex = 0;
	}

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
		if (_inputBuffer == null)
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a disposed object."));
		}
		byte[] array = new byte[inputCount];
		int num;
		if (_whitespaces == FromBase64TransformMode.IgnoreWhiteSpaces)
		{
			array = DiscardWhiteSpaces(inputBuffer, inputOffset, inputCount);
			num = array.Length;
		}
		else
		{
			Buffer.InternalBlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			num = inputCount;
		}
		if (num + _inputIndex < 4)
		{
			Buffer.InternalBlockCopy(array, 0, _inputBuffer, _inputIndex, num);
			_inputIndex += num;
			return 0;
		}
		int num2 = (num + _inputIndex) / 4;
		byte[] array2 = new byte[_inputIndex + num];
		Buffer.InternalBlockCopy(_inputBuffer, 0, array2, 0, _inputIndex);
		Buffer.InternalBlockCopy(array, 0, array2, _inputIndex, num);
		_inputIndex = (num + _inputIndex) % 4;
		Buffer.InternalBlockCopy(array, num - _inputIndex, _inputBuffer, 0, _inputIndex);
		byte[] array3 = Convert.FromBase64CharArray(Encoding.ASCII.GetChars(array2, 0, 4 * num2), 0, 4 * num2);
		Buffer.BlockCopy(array3, 0, outputBuffer, outputOffset, array3.Length);
		return array3.Length;
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
		if (_inputBuffer == null)
		{
			throw new ObjectDisposedException(null, Environment.GetResourceString("Cannot access a disposed object."));
		}
		byte[] array = new byte[inputCount];
		int num;
		if (_whitespaces == FromBase64TransformMode.IgnoreWhiteSpaces)
		{
			array = DiscardWhiteSpaces(inputBuffer, inputOffset, inputCount);
			num = array.Length;
		}
		else
		{
			Buffer.InternalBlockCopy(inputBuffer, inputOffset, array, 0, inputCount);
			num = inputCount;
		}
		if (num + _inputIndex < 4)
		{
			Reset();
			return EmptyArray<byte>.Value;
		}
		int num2 = (num + _inputIndex) / 4;
		byte[] array2 = new byte[_inputIndex + num];
		Buffer.InternalBlockCopy(_inputBuffer, 0, array2, 0, _inputIndex);
		Buffer.InternalBlockCopy(array, 0, array2, _inputIndex, num);
		_inputIndex = (num + _inputIndex) % 4;
		Buffer.InternalBlockCopy(array, num - _inputIndex, _inputBuffer, 0, _inputIndex);
		byte[] result = Convert.FromBase64CharArray(Encoding.ASCII.GetChars(array2, 0, 4 * num2), 0, 4 * num2);
		Reset();
		return result;
	}

	private byte[] DiscardWhiteSpaces(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		int num = 0;
		for (int i = 0; i < inputCount; i++)
		{
			if (char.IsWhiteSpace((char)inputBuffer[inputOffset + i]))
			{
				num++;
			}
		}
		byte[] array = new byte[inputCount - num];
		num = 0;
		for (int i = 0; i < inputCount; i++)
		{
			if (!char.IsWhiteSpace((char)inputBuffer[inputOffset + i]))
			{
				array[num++] = inputBuffer[inputOffset + i];
			}
		}
		return array;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Reset()
	{
		_inputIndex = 0;
	}

	public void Clear()
	{
		Dispose();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_inputBuffer != null)
			{
				Array.Clear(_inputBuffer, 0, _inputBuffer.Length);
			}
			_inputBuffer = null;
			_inputIndex = 0;
		}
	}

	~FromBase64Transform()
	{
		Dispose(disposing: false);
	}
}
