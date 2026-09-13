using System.Collections;
using System.IO;
using System.Text;

namespace System.Resources;

internal class Win32ResFileReader
{
	private Stream res_file;

	public Win32ResFileReader(Stream s)
	{
		res_file = s;
	}

	private int read_int16()
	{
		int num = res_file.ReadByte();
		if (num == -1)
		{
			return -1;
		}
		int num2 = res_file.ReadByte();
		if (num2 == -1)
		{
			return -1;
		}
		return num | (num2 << 8);
	}

	private int read_int32()
	{
		int num = read_int16();
		if (num == -1)
		{
			return -1;
		}
		int num2 = read_int16();
		if (num2 == -1)
		{
			return -1;
		}
		return num | (num2 << 16);
	}

	private bool read_padding()
	{
		while (res_file.Position % 4 != 0L)
		{
			if (read_int16() == -1)
			{
				return false;
			}
		}
		return true;
	}

	private NameOrId read_ordinal()
	{
		if ((read_int16() & 0xFFFF) != 0)
		{
			return new NameOrId(read_int16());
		}
		byte[] array = new byte[16];
		int num = 0;
		while (true)
		{
			int num2 = read_int16();
			if (num2 == 0)
			{
				break;
			}
			if (num == array.Length)
			{
				byte[] array2 = new byte[array.Length * 2];
				Array.Copy(array, array2, array.Length);
				array = array2;
			}
			array[num] = (byte)(num2 >> 8);
			array[num + 1] = (byte)(num2 & 0xFF);
			num += 2;
		}
		return new NameOrId(new string(Encoding.Unicode.GetChars(array, 0, num)));
	}

	public ICollection ReadResources()
	{
		ArrayList arrayList = new ArrayList();
		while (read_padding())
		{
			int num = read_int32();
			if (num == -1)
			{
				break;
			}
			read_int32();
			NameOrId type = read_ordinal();
			NameOrId name = read_ordinal();
			if (!read_padding())
			{
				break;
			}
			read_int32();
			read_int16();
			int language = read_int16();
			read_int32();
			read_int32();
			if (num != 0)
			{
				byte[] array = new byte[num];
				if (res_file.Read(array, 0, num) != num)
				{
					break;
				}
				arrayList.Add(new Win32EncodedResource(type, name, language, array));
			}
		}
		return arrayList;
	}
}
