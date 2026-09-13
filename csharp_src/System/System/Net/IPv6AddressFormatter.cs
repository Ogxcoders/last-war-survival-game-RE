using System.Text;

namespace System.Net;

internal struct IPv6AddressFormatter
{
	private ushort[] address;

	private long scopeId;

	public IPv6AddressFormatter(ushort[] addr, long scopeId)
	{
		address = addr;
		this.scopeId = scopeId;
	}

	private static ushort SwapUShort(ushort number)
	{
		return (ushort)(((number >> 8) & 0xFF) + ((number << 8) & 0xFF00));
	}

	private uint AsIPv4Int()
	{
		return (uint)((SwapUShort(address[7]) << 16) + SwapUShort(address[6]));
	}

	private bool IsIPv4Compatible()
	{
		for (int i = 0; i < 6; i++)
		{
			if (address[i] != 0)
			{
				return false;
			}
		}
		if (address[6] == 0)
		{
			return false;
		}
		return AsIPv4Int() > 1;
	}

	private bool IsIPv4Mapped()
	{
		for (int i = 0; i < 5; i++)
		{
			if (address[i] != 0)
			{
				return false;
			}
		}
		if (address[6] == 0)
		{
			return false;
		}
		return address[5] == ushort.MaxValue;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (IsIPv4Compatible() || IsIPv4Mapped())
		{
			stringBuilder.Append("::");
			if (IsIPv4Mapped())
			{
				stringBuilder.Append("ffff:");
			}
			stringBuilder.Append(new IPAddress(AsIPv4Int()).ToString());
			return stringBuilder.ToString();
		}
		int num = -1;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < 8; i++)
		{
			if (address[i] != 0)
			{
				if (num3 > num2 && num3 > 1)
				{
					num2 = num3;
					num = i - num3;
				}
				num3 = 0;
			}
			else
			{
				num3++;
			}
		}
		if (num3 > num2 && num3 > 1)
		{
			num2 = num3;
			num = 8 - num3;
		}
		if (num == 0)
		{
			stringBuilder.Append(":");
		}
		for (int j = 0; j < 8; j++)
		{
			if (j == num)
			{
				stringBuilder.Append(":");
				j += num2 - 1;
				continue;
			}
			stringBuilder.AppendFormat("{0:x}", address[j]);
			if (j < 7)
			{
				stringBuilder.Append(':');
			}
		}
		if (scopeId != 0L)
		{
			stringBuilder.Append('%').Append(scopeId);
		}
		return stringBuilder.ToString();
	}
}
