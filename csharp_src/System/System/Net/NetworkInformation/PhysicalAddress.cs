using System.Text;

namespace System.Net.NetworkInformation;

public class PhysicalAddress
{
	private byte[] address;

	private bool changed = true;

	private int hash;

	public static readonly PhysicalAddress None = new PhysicalAddress(new byte[0]);

	public PhysicalAddress(byte[] address)
	{
		this.address = address;
	}

	public override int GetHashCode()
	{
		if (changed)
		{
			changed = false;
			hash = 0;
			int num = address.Length & -4;
			int i;
			for (i = 0; i < num; i += 4)
			{
				hash ^= address[i] | (address[i + 1] << 8) | (address[i + 2] << 16) | (address[i + 3] << 24);
			}
			if ((address.Length & 3) != 0)
			{
				int num2 = 0;
				int num3 = 0;
				for (; i < address.Length; i++)
				{
					num2 |= address[i] << num3;
					num3 += 8;
				}
				hash ^= num2;
			}
		}
		return hash;
	}

	public override bool Equals(object comparand)
	{
		if (!(comparand is PhysicalAddress physicalAddress))
		{
			return false;
		}
		if (address.Length != physicalAddress.address.Length)
		{
			return false;
		}
		for (int i = 0; i < physicalAddress.address.Length; i++)
		{
			if (address[i] != physicalAddress.address[i])
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = address;
		foreach (byte b in array)
		{
			int num = (b >> 4) & 0xF;
			for (int j = 0; j < 2; j++)
			{
				if (num < 10)
				{
					stringBuilder.Append((char)(num + 48));
				}
				else
				{
					stringBuilder.Append((char)(num + 55));
				}
				num = b & 0xF;
			}
		}
		return stringBuilder.ToString();
	}

	public byte[] GetAddressBytes()
	{
		byte[] array = new byte[address.Length];
		Buffer.BlockCopy(address, 0, array, 0, address.Length);
		return array;
	}

	public static PhysicalAddress Parse(string address)
	{
		int num = 0;
		bool flag = false;
		byte[] array = null;
		if (address == null)
		{
			return None;
		}
		if (address.IndexOf('-') >= 0)
		{
			flag = true;
			array = new byte[(address.Length + 1) / 3];
		}
		else
		{
			if (address.Length % 2 > 0)
			{
				throw new FormatException(global::SR.GetString("An invalid physical address was specified."));
			}
			array = new byte[address.Length / 2];
		}
		int num2 = 0;
		for (int i = 0; i < address.Length; i++)
		{
			int num3 = address[i];
			if (num3 >= 48 && num3 <= 57)
			{
				num3 -= 48;
			}
			else
			{
				if (num3 < 65 || num3 > 70)
				{
					if (num3 == 45)
					{
						if (num == 2)
						{
							num = 0;
							continue;
						}
						throw new FormatException(global::SR.GetString("An invalid physical address was specified."));
					}
					throw new FormatException(global::SR.GetString("An invalid physical address was specified."));
				}
				num3 -= 55;
			}
			if (flag && num >= 2)
			{
				throw new FormatException(global::SR.GetString("An invalid physical address was specified."));
			}
			if (num % 2 == 0)
			{
				array[num2] = (byte)(num3 << 4);
			}
			else
			{
				array[num2++] |= (byte)num3;
			}
			num++;
		}
		if (num < 2)
		{
			throw new FormatException(global::SR.GetString("An invalid physical address was specified."));
		}
		return new PhysicalAddress(array);
	}
}
