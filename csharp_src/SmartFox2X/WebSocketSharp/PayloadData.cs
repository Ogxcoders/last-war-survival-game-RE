using System;
using System.Collections;
using System.Collections.Generic;

namespace WebSocketSharp;

internal class PayloadData : IEnumerable<byte>, IEnumerable
{
	private byte[] _data;

	private long _extDataLength;

	private long _length;

	private bool _masked;

	public const ulong MaxLength = 9223372036854775807uL;

	internal long ExtensionDataLength
	{
		get
		{
			return _extDataLength;
		}
		set
		{
			_extDataLength = value;
		}
	}

	internal bool IncludesReservedCloseStatusCode
	{
		get
		{
			if (_length > 1)
			{
				return _data.SubArray(0, 2).ToUInt16(ByteOrder.Big).IsReserved();
			}
			return false;
		}
	}

	public byte[] ApplicationData
	{
		get
		{
			if (_extDataLength <= 0)
			{
				return _data;
			}
			return _data.SubArray(_extDataLength, _length - _extDataLength);
		}
	}

	public byte[] ExtensionData
	{
		get
		{
			if (_extDataLength <= 0)
			{
				return new byte[0];
			}
			return _data.SubArray(0L, _extDataLength);
		}
	}

	public bool IsMasked => _masked;

	public ulong Length => (ulong)_length;

	internal PayloadData()
	{
		_data = new byte[0];
	}

	internal PayloadData(byte[] data)
		: this(data, masked: false)
	{
	}

	internal PayloadData(byte[] data, bool masked)
	{
		_data = data;
		_masked = masked;
		_length = data.LongLength;
	}

	internal void Mask(byte[] key)
	{
		for (long num = 0L; num < _length; num++)
		{
			_data[num] ^= key[num % 4];
		}
		_masked = !_masked;
	}

	public IEnumerator<byte> GetEnumerator()
	{
		byte[] data = _data;
		for (int i = 0; i < data.Length; i++)
		{
			yield return data[i];
		}
	}

	public byte[] ToByteArray()
	{
		return _data;
	}

	public override string ToString()
	{
		return BitConverter.ToString(_data);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
