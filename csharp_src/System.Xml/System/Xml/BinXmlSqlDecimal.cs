using System.Diagnostics;
using System.IO;

namespace System.Xml;

internal struct BinXmlSqlDecimal
{
	internal byte m_bLen;

	internal byte m_bPrec;

	internal byte m_bScale;

	internal byte m_bSign;

	internal uint m_data1;

	internal uint m_data2;

	internal uint m_data3;

	internal uint m_data4;

	private static readonly byte NUMERIC_MAX_PRECISION = 38;

	private static readonly byte MaxPrecision = NUMERIC_MAX_PRECISION;

	private static readonly byte MaxScale = NUMERIC_MAX_PRECISION;

	private static readonly int x_cNumeMax = 4;

	private static readonly long x_lInt32Base = 4294967296L;

	private static readonly ulong x_ulInt32Base = 4294967296uL;

	private static readonly ulong x_ulInt32BaseForMod = x_ulInt32Base - 1;

	internal static readonly ulong x_llMax = 9223372036854775807uL;

	private static readonly double DUINT_BASE = x_lInt32Base;

	private static readonly double DUINT_BASE2 = DUINT_BASE * DUINT_BASE;

	private static readonly double DUINT_BASE3 = DUINT_BASE2 * DUINT_BASE;

	private static readonly uint[] x_rgulShiftBase = new uint[9] { 10u, 100u, 1000u, 10000u, 100000u, 1000000u, 10000000u, 100000000u, 1000000000u };

	private static readonly byte[] rgCLenFromPrec = new byte[38]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 2,
		2, 2, 2, 2, 2, 2, 2, 2, 2, 3,
		3, 3, 3, 3, 3, 3, 3, 3, 4, 4,
		4, 4, 4, 4, 4, 4, 4, 4
	};

	public bool IsPositive => m_bSign == 0;

	public BinXmlSqlDecimal(byte[] data, int offset, bool trim)
	{
		switch (data[offset])
		{
		case 7:
			m_bLen = 1;
			break;
		case 11:
			m_bLen = 2;
			break;
		case 15:
			m_bLen = 3;
			break;
		case 19:
			m_bLen = 4;
			break;
		default:
			throw new XmlException("Unable to parse data as SQL_DECIMAL.", (string[])null);
		}
		m_bPrec = data[offset + 1];
		m_bScale = data[offset + 2];
		m_bSign = ((data[offset + 3] == 0) ? ((byte)1) : ((byte)0));
		m_data1 = UIntFromByteArray(data, offset + 4);
		m_data2 = ((m_bLen > 1) ? UIntFromByteArray(data, offset + 8) : 0u);
		m_data3 = ((m_bLen > 2) ? UIntFromByteArray(data, offset + 12) : 0u);
		m_data4 = ((m_bLen > 3) ? UIntFromByteArray(data, offset + 16) : 0u);
		if (m_bLen == 4 && m_data4 == 0)
		{
			m_bLen = 3;
		}
		if (m_bLen == 3 && m_data3 == 0)
		{
			m_bLen = 2;
		}
		if (m_bLen == 2 && m_data2 == 0)
		{
			m_bLen = 1;
		}
		if (trim)
		{
			TrimTrailingZeros();
		}
	}

	public void Write(Stream strm)
	{
		strm.WriteByte((byte)(m_bLen * 4 + 3));
		strm.WriteByte(m_bPrec);
		strm.WriteByte(m_bScale);
		strm.WriteByte((m_bSign == 0) ? ((byte)1) : ((byte)0));
		WriteUI4(m_data1, strm);
		if (m_bLen <= 1)
		{
			return;
		}
		WriteUI4(m_data2, strm);
		if (m_bLen > 2)
		{
			WriteUI4(m_data3, strm);
			if (m_bLen > 3)
			{
				WriteUI4(m_data4, strm);
			}
		}
	}

	private void WriteUI4(uint val, Stream strm)
	{
		strm.WriteByte((byte)(val & 0xFF));
		strm.WriteByte((byte)((val >> 8) & 0xFF));
		strm.WriteByte((byte)((val >> 16) & 0xFF));
		strm.WriteByte((byte)((val >> 24) & 0xFF));
	}

	private static uint UIntFromByteArray(byte[] data, int offset)
	{
		return (uint)(data[offset] | (data[offset + 1] << 8) | (data[offset + 2] << 16) | (data[offset + 3] << 24));
	}

	private bool FZero()
	{
		if (m_data1 == 0)
		{
			return m_bLen <= 1;
		}
		return false;
	}

	private void StoreFromWorkingArray(uint[] rguiData)
	{
		m_data1 = rguiData[0];
		m_data2 = rguiData[1];
		m_data3 = rguiData[2];
		m_data4 = rguiData[3];
	}

	private bool FGt10_38(uint[] rglData)
	{
		if ((long)rglData[3] >= 1262177448L)
		{
			if ((long)rglData[3] <= 1262177448L && (long)rglData[2] <= 1518781562L)
			{
				if ((ulong)rglData[2] == 1518781562)
				{
					return (long)rglData[1] >= 160047680L;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private static void MpDiv1(uint[] rgulU, ref int ciulU, uint iulD, out uint iulR)
	{
		uint num = 0u;
		ulong num2 = iulD;
		int num3 = ciulU;
		while (num3 > 0)
		{
			num3--;
			ulong num4 = ((ulong)num << 32) + rgulU[num3];
			rgulU[num3] = (uint)(num4 / num2);
			num = (uint)(num4 - rgulU[num3] * num2);
		}
		iulR = num;
		MpNormalize(rgulU, ref ciulU);
	}

	private static void MpNormalize(uint[] rgulU, ref int ciulU)
	{
		while (ciulU > 1 && rgulU[ciulU - 1] == 0)
		{
			ciulU--;
		}
	}

	internal void AdjustScale(int digits, bool fRound)
	{
		bool flag = false;
		int num = digits;
		if (num + m_bScale < 0)
		{
			throw new XmlException("Numeric arithmetic causes truncation.", (string)null);
		}
		if (num + m_bScale > NUMERIC_MAX_PRECISION)
		{
			throw new XmlException("Arithmetic Overflow.", (string)null);
		}
		byte bScale = (byte)(num + m_bScale);
		byte bPrec = (byte)Math.Min(NUMERIC_MAX_PRECISION, Math.Max(1, num + m_bPrec));
		if (num > 0)
		{
			m_bScale = bScale;
			m_bPrec = bPrec;
			while (num > 0)
			{
				uint uiMultiplier;
				if (num >= 9)
				{
					uiMultiplier = x_rgulShiftBase[8];
					num -= 9;
				}
				else
				{
					uiMultiplier = x_rgulShiftBase[num - 1];
					num = 0;
				}
				MultByULong(uiMultiplier);
			}
		}
		else if (num < 0)
		{
			uint uiMultiplier;
			uint num2;
			do
			{
				if (num <= -9)
				{
					uiMultiplier = x_rgulShiftBase[8];
					num += 9;
				}
				else
				{
					uiMultiplier = x_rgulShiftBase[-num - 1];
					num = 0;
				}
				num2 = DivByULong(uiMultiplier);
			}
			while (num < 0);
			flag = num2 >= uiMultiplier / 2;
			m_bScale = bScale;
			m_bPrec = bPrec;
		}
		if (flag && fRound)
		{
			AddULong(1u);
		}
		else if (FZero())
		{
			m_bSign = 0;
		}
	}

	private void AddULong(uint ulAdd)
	{
		ulong num = ulAdd;
		int bLen = m_bLen;
		uint[] array = new uint[4] { m_data1, m_data2, m_data3, m_data4 };
		int num2 = 0;
		do
		{
			num += array[num2];
			array[num2] = (uint)num;
			num >>= 32;
			if (num == 0L)
			{
				StoreFromWorkingArray(array);
				return;
			}
			num2++;
		}
		while (num2 < bLen);
		if (num2 == x_cNumeMax)
		{
			throw new XmlException("Arithmetic Overflow.", (string)null);
		}
		array[num2] = (uint)num;
		m_bLen++;
		if (FGt10_38(array))
		{
			throw new XmlException("Arithmetic Overflow.", (string)null);
		}
		StoreFromWorkingArray(array);
	}

	private void MultByULong(uint uiMultiplier)
	{
		int bLen = m_bLen;
		ulong num = 0uL;
		ulong num2 = 0uL;
		uint[] array = new uint[4] { m_data1, m_data2, m_data3, m_data4 };
		for (int i = 0; i < bLen; i++)
		{
			num2 = (ulong)array[i] * (ulong)uiMultiplier;
			num += num2;
			num2 = ((num >= num2) ? 0 : x_ulInt32Base);
			array[i] = (uint)num;
			num = (num >> 32) + num2;
		}
		if (num != 0L)
		{
			if (bLen == x_cNumeMax)
			{
				throw new XmlException("Arithmetic Overflow.", (string)null);
			}
			array[bLen] = (uint)num;
			m_bLen++;
		}
		if (FGt10_38(array))
		{
			throw new XmlException("Arithmetic Overflow.", (string)null);
		}
		StoreFromWorkingArray(array);
	}

	internal uint DivByULong(uint iDivisor)
	{
		ulong num = iDivisor;
		ulong num2 = 0uL;
		uint num3 = 0u;
		bool flag = true;
		if (num == 0L)
		{
			throw new XmlException("Divide by zero error encountered.", (string)null);
		}
		uint[] array = new uint[4] { m_data1, m_data2, m_data3, m_data4 };
		for (int num4 = m_bLen; num4 > 0; num4--)
		{
			num2 = (num2 << 32) + array[num4 - 1];
			num3 = (array[num4 - 1] = (uint)(num2 / num));
			num2 %= num;
			flag = flag && num3 == 0;
			if (flag)
			{
				m_bLen--;
			}
		}
		StoreFromWorkingArray(array);
		if (flag)
		{
			m_bLen = 1;
		}
		return (uint)num2;
	}

	private static byte CLenFromPrec(byte bPrec)
	{
		return rgCLenFromPrec[bPrec - 1];
	}

	private static char ChFromDigit(uint uiDigit)
	{
		return (char)(uiDigit + 48);
	}

	public decimal ToDecimal()
	{
		if (m_data4 != 0 || m_bScale > 28)
		{
			throw new XmlException("Arithmetic Overflow.", (string)null);
		}
		return new decimal((int)m_data1, (int)m_data2, (int)m_data3, !IsPositive, m_bScale);
	}

	private void TrimTrailingZeros()
	{
		uint[] array = new uint[4] { m_data1, m_data2, m_data3, m_data4 };
		int ciulU = m_bLen;
		if (ciulU == 1 && array[0] == 0)
		{
			m_bScale = 0;
			return;
		}
		while (m_bScale > 0 && (ciulU > 1 || array[0] != 0))
		{
			MpDiv1(array, ref ciulU, 10u, out var iulR);
			if (iulR != 0)
			{
				break;
			}
			m_data1 = array[0];
			m_data2 = array[1];
			m_data3 = array[2];
			m_data4 = array[3];
			m_bScale--;
		}
		if (m_bLen == 4 && m_data4 == 0)
		{
			m_bLen = 3;
		}
		if (m_bLen == 3 && m_data3 == 0)
		{
			m_bLen = 2;
		}
		if (m_bLen == 2 && m_data2 == 0)
		{
			m_bLen = 1;
		}
	}

	public override string ToString()
	{
		uint[] array = new uint[4] { m_data1, m_data2, m_data3, m_data4 };
		int ciulU = m_bLen;
		char[] array2 = new char[NUMERIC_MAX_PRECISION + 1];
		int num = 0;
		while (ciulU > 1 || array[0] != 0)
		{
			MpDiv1(array, ref ciulU, 10u, out var iulR);
			array2[num++] = ChFromDigit(iulR);
		}
		while (num <= m_bScale)
		{
			array2[num++] = ChFromDigit(0u);
		}
		bool isPositive = IsPositive;
		int num2 = (isPositive ? num : (num + 1));
		if (m_bScale > 0)
		{
			num2++;
		}
		char[] array3 = new char[num2];
		int num3 = 0;
		if (!isPositive)
		{
			array3[num3++] = '-';
		}
		while (num > 0)
		{
			if (num-- == m_bScale)
			{
				array3[num3++] = '.';
			}
			array3[num3++] = array2[num];
		}
		return new string(array3);
	}

	[Conditional("DEBUG")]
	private void AssertValid()
	{
		_ = (new uint[4] { m_data1, m_data2, m_data3, m_data4 })[m_bLen - 1];
		for (int i = m_bLen; i < x_cNumeMax; i++)
		{
		}
	}
}
