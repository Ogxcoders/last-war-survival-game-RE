using System.IO;

namespace VEngine;

public class CRC32
{
	private const uint _initialResidueValue = uint.MaxValue;

	private static readonly object _globalSync = new object();

	private static uint[] _crc32Table;

	private static readonly byte[][] _maskingBitTable = new byte[32][]
	{
		new byte[1] { 2 },
		new byte[2] { 0, 3 },
		new byte[3] { 0, 1, 4 },
		new byte[3] { 1, 2, 5 },
		new byte[4] { 0, 2, 3, 6 },
		new byte[4] { 1, 3, 4, 7 },
		new byte[2] { 4, 5 },
		new byte[3] { 0, 5, 6 },
		new byte[3] { 1, 6, 7 },
		new byte[1] { 7 },
		new byte[1] { 2 },
		new byte[1] { 3 },
		new byte[2] { 0, 4 },
		new byte[3] { 0, 1, 5 },
		new byte[3] { 1, 2, 6 },
		new byte[3] { 2, 3, 7 },
		new byte[4] { 0, 2, 3, 4 },
		new byte[5] { 0, 1, 3, 4, 5 },
		new byte[6] { 0, 1, 2, 4, 5, 6 },
		new byte[6] { 1, 2, 3, 5, 6, 7 },
		new byte[4] { 3, 4, 6, 7 },
		new byte[4] { 2, 4, 5, 7 },
		new byte[4] { 2, 3, 5, 6 },
		new byte[4] { 3, 4, 6, 7 },
		new byte[5] { 0, 2, 4, 5, 7 },
		new byte[6] { 0, 1, 2, 3, 5, 6 },
		new byte[7] { 0, 1, 2, 3, 4, 6, 7 },
		new byte[5] { 1, 3, 4, 5, 7 },
		new byte[4] { 0, 4, 5, 6 },
		new byte[5] { 0, 1, 5, 6, 7 },
		new byte[4] { 0, 1, 6, 7 },
		new byte[2] { 1, 7 }
	};

	private uint _residue = uint.MaxValue;

	internal uint crc => ~_residue;

	internal CRC32()
	{
		lock (_globalSync)
		{
			if (_crc32Table == null)
			{
				PrepareTable();
			}
		}
	}

	internal uint Compute(Stream stream)
	{
		byte[] array = new byte[4096];
		while (true)
		{
			int num = stream.Read(array, 0, array.Length);
			if (num <= 0)
			{
				break;
			}
			Accumulate(array, 0, num);
		}
		return crc;
	}

	internal void Accumulate(byte[] buffer, int offset, int count)
	{
		for (int i = offset; i < count + offset; i++)
		{
			_residue = ((_residue >> 8) & 0xFFFFFF) ^ _crc32Table[(_residue ^ buffer[i]) & 0xFF];
		}
	}

	internal void ClearCrc()
	{
		_residue = uint.MaxValue;
	}

	private static void PrepareTable()
	{
		_crc32Table = new uint[256];
		for (uint num = 0u; num < _crc32Table.Length; num++)
		{
			for (byte b = 0; b < 32; b++)
			{
				bool flag = false;
				byte[] array = _maskingBitTable[b];
				foreach (byte bitOrdinal in array)
				{
					flag ^= GetBit(bitOrdinal, num);
				}
				SetBit(b, ref _crc32Table[num], flag);
			}
		}
	}

	private static bool GetBit(byte bitOrdinal, uint data)
	{
		return ((data >> (int)bitOrdinal) & 1) == 1;
	}

	private static void SetBit(byte bitOrdinal, ref uint data, bool value)
	{
		if (value)
		{
			data |= (uint)(1 << (int)bitOrdinal);
		}
	}
}
