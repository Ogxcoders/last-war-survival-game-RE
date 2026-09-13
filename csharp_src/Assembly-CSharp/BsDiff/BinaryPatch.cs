using System;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace BsDiff;

public static class BinaryPatch
{
	private const long c_fileSignature = 3473478480300364610L;

	private const int c_headerSize = 32;

	public static void Create(byte[] oldData, byte[] newData, Stream output)
	{
		if (oldData == null)
		{
			throw new ArgumentNullException("oldData");
		}
		if (newData == null)
		{
			throw new ArgumentNullException("newData");
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (!output.CanSeek)
		{
			throw new ArgumentException("Output stream must be seekable.", "output");
		}
		if (!output.CanWrite)
		{
			throw new ArgumentException("Output stream must be writable.", "output");
		}
		byte[] array = new byte[32];
		WriteInt64(3473478480300364610L, array, 0);
		WriteInt64(0L, array, 8);
		WriteInt64(0L, array, 16);
		WriteInt64(newData.Length, array, 24);
		long position = output.Position;
		output.Write(array, 0, array.Length);
		int[] i = SuffixSort(oldData);
		byte[] array2 = new byte[newData.Length];
		byte[] array3 = new byte[newData.Length];
		int num = 0;
		int num2 = 0;
		using (BZip2OutputStream bZip2OutputStream = new BZip2OutputStream(output)
		{
			IsStreamOwner = false
		})
		{
			int j = 0;
			int pos = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			while (j < newData.Length)
			{
				int num7 = 0;
				int k = (j += num3);
				for (; j < newData.Length; j++)
				{
					for (num3 = Search(i, oldData, newData, j, 0, oldData.Length, out pos); k < j + num3; k++)
					{
						if (k + num6 < oldData.Length && oldData[k + num6] == newData[k])
						{
							num7++;
						}
					}
					if ((num3 == num7 && num3 != 0) || num3 > num7 + 8)
					{
						break;
					}
					if (j + num6 < oldData.Length && oldData[j + num6] == newData[j])
					{
						num7--;
					}
				}
				if (num3 == num7 && j != newData.Length)
				{
					continue;
				}
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				while (num4 + num11 < j && num5 + num11 < oldData.Length)
				{
					if (oldData[num5 + num11] == newData[num4 + num11])
					{
						num8++;
					}
					num11++;
					if (num8 * 2 - num11 > num9 * 2 - num10)
					{
						num9 = num8;
						num10 = num11;
					}
				}
				int num12 = 0;
				if (j < newData.Length)
				{
					num8 = 0;
					int num13 = 0;
					for (int l = 1; j >= num4 + l && pos >= l; l++)
					{
						if (oldData[pos - l] == newData[j - l])
						{
							num8++;
						}
						if (num8 * 2 - l > num13 * 2 - num12)
						{
							num13 = num8;
							num12 = l;
						}
					}
				}
				if (num4 + num10 > j - num12)
				{
					int num14 = num4 + num10 - (j - num12);
					num8 = 0;
					int num15 = 0;
					int num16 = 0;
					for (int m = 0; m < num14; m++)
					{
						if (newData[num4 + num10 - num14 + m] == oldData[num5 + num10 - num14 + m])
						{
							num8++;
						}
						if (newData[j - num12 + m] == oldData[pos - num12 + m])
						{
							num8--;
						}
						if (num8 > num15)
						{
							num15 = num8;
							num16 = m + 1;
						}
					}
					num10 += num16 - num14;
					num12 -= num16;
				}
				for (int n = 0; n < num10; n++)
				{
					array2[num + n] = (byte)(newData[num4 + n] - oldData[num5 + n]);
				}
				for (int num17 = 0; num17 < j - num12 - (num4 + num10); num17++)
				{
					array3[num2 + num17] = newData[num4 + num10 + num17];
				}
				num += num10;
				num2 += j - num12 - (num4 + num10);
				byte[] array4 = new byte[8];
				WriteInt64(num10, array4, 0);
				bZip2OutputStream.Write(array4, 0, 8);
				WriteInt64(j - num12 - (num4 + num10), array4, 0);
				bZip2OutputStream.Write(array4, 0, 8);
				WriteInt64(pos - num12 - (num5 + num10), array4, 0);
				bZip2OutputStream.Write(array4, 0, 8);
				num4 = j - num12;
				num5 = pos - num12;
				num6 = pos - j;
			}
		}
		long position2 = output.Position;
		WriteInt64(position2 - position - 32, array, 8);
		using (BZip2OutputStream bZip2OutputStream2 = new BZip2OutputStream(output)
		{
			IsStreamOwner = false
		})
		{
			bZip2OutputStream2.Write(array2, 0, num);
		}
		WriteInt64(output.Position - position2, array, 16);
		using (BZip2OutputStream bZip2OutputStream3 = new BZip2OutputStream(output)
		{
			IsStreamOwner = false
		})
		{
			bZip2OutputStream3.Write(array3, 0, num2);
		}
		long position3 = output.Position;
		output.Position = position;
		output.Write(array, 0, array.Length);
		output.Position = position3;
	}

	public static void Apply(Stream input, Func<Stream> openPatchStream, Stream output)
	{
		if (input == null)
		{
			throw new ArgumentNullException("input");
		}
		if (openPatchStream == null)
		{
			throw new ArgumentNullException("openPatchStream");
		}
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		long num;
		long num2;
		long num3;
		using (Stream stream = openPatchStream())
		{
			if (!stream.CanRead)
			{
				throw new ArgumentException("Patch stream must be readable.", "openPatchStream");
			}
			if (!stream.CanSeek)
			{
				throw new ArgumentException("Patch stream must be seekable.", "openPatchStream");
			}
			byte[] buf = ReadExactly(stream, 32);
			if (ReadInt64(buf, 0) != 3473478480300364610L)
			{
				throw new InvalidOperationException("Corrupt patch.");
			}
			num = ReadInt64(buf, 8);
			num2 = ReadInt64(buf, 16);
			num3 = ReadInt64(buf, 24);
			if (num < 0 || num2 < 0 || num3 < 0)
			{
				throw new InvalidOperationException("Corrupt patch.");
			}
		}
		byte[] array = new byte[1048576];
		byte[] array2 = new byte[1048576];
		Stream stream2 = openPatchStream();
		Stream stream3 = openPatchStream();
		Stream stream4 = openPatchStream();
		stream2.Seek(32L, SeekOrigin.Current);
		stream3.Seek(32 + num, SeekOrigin.Current);
		stream4.Seek(32 + num + num2, SeekOrigin.Current);
		BZip2InputStream bZip2InputStream = new BZip2InputStream(stream2);
		BZip2InputStream bZip2InputStream2 = new BZip2InputStream(stream3);
		BZip2InputStream bZip2InputStream3 = new BZip2InputStream(stream4);
		long[] array3 = new long[3];
		byte[] array4 = new byte[8];
		int num4 = 0;
		int num5 = 0;
		while (num5 < num3)
		{
			for (int i = 0; i < 3; i++)
			{
				ReadExactly(bZip2InputStream, array4, 0, 8);
				array3[i] = ReadInt64(array4, 0);
			}
			if (num5 + array3[0] > num3)
			{
				throw new InvalidOperationException("Corrupt patch.");
			}
			input.Position = num4;
			int num6 = (int)array3[0];
			while (num6 > 0)
			{
				int num7 = Math.Min(num6, 1048576);
				ReadExactly(bZip2InputStream2, array, 0, num7);
				int num8 = Math.Min(num7, (int)(input.Length - input.Position));
				ReadExactly(input, array2, 0, num8);
				for (int j = 0; j < num8; j++)
				{
					array[j] += array2[j];
				}
				output.Write(array, 0, num7);
				num5 += num7;
				num4 += num7;
				num6 -= num7;
			}
			if (num5 + array3[1] > num3)
			{
				throw new InvalidOperationException("Corrupt patch.");
			}
			num6 = (int)array3[1];
			while (num6 > 0)
			{
				int num9 = Math.Min(num6, 1048576);
				ReadExactly(bZip2InputStream3, array, 0, num9);
				output.Write(array, 0, num9);
				num5 += num9;
				num6 -= num9;
			}
			num4 = (int)(num4 + array3[2]);
		}
		bZip2InputStream.Dispose();
		bZip2InputStream2.Dispose();
		bZip2InputStream3.Dispose();
		stream2.Dispose();
		stream3.Dispose();
		stream4.Dispose();
	}

	private static int CompareBytes(byte[] left, int leftOffset, byte[] right, int rightOffset)
	{
		for (int i = 0; i < left.Length - leftOffset && i < right.Length - rightOffset; i++)
		{
			int num = left[i + leftOffset] - right[i + rightOffset];
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}

	private static int MatchLength(byte[] oldData, int oldOffset, byte[] newData, int newOffset)
	{
		int i;
		for (i = 0; i < oldData.Length - oldOffset && i < newData.Length - newOffset && oldData[i + oldOffset] == newData[i + newOffset]; i++)
		{
		}
		return i;
	}

	private static int Search(int[] I, byte[] oldData, byte[] newData, int newOffset, int start, int end, out int pos)
	{
		if (end - start < 2)
		{
			int num = MatchLength(oldData, I[start], newData, newOffset);
			int num2 = MatchLength(oldData, I[end], newData, newOffset);
			if (num > num2)
			{
				pos = I[start];
				return num;
			}
			pos = I[end];
			return num2;
		}
		int num3 = start + (end - start) / 2;
		if (CompareBytes(oldData, I[num3], newData, newOffset) >= 0)
		{
			return Search(I, oldData, newData, newOffset, start, num3, out pos);
		}
		return Search(I, oldData, newData, newOffset, num3, end, out pos);
	}

	private static void Split(int[] I, int[] v, int start, int len, int h)
	{
		if (len < 16)
		{
			int num;
			for (int i = start; i < start + len; i += num)
			{
				num = 1;
				int num2 = v[I[i] + h];
				for (int j = 1; i + j < start + len; j++)
				{
					if (v[I[i + j] + h] < num2)
					{
						num2 = v[I[i + j] + h];
						num = 0;
					}
					if (v[I[i + j] + h] == num2)
					{
						Swap(ref I[i + num], ref I[i + j]);
						num++;
					}
				}
				for (int k = 0; k < num; k++)
				{
					v[I[i + k]] = i + num - 1;
				}
				if (num == 1)
				{
					I[i] = -1;
				}
			}
			return;
		}
		int num3 = v[I[start + len / 2] + h];
		int num4 = 0;
		int num5 = 0;
		for (int l = start; l < start + len; l++)
		{
			if (v[I[l] + h] < num3)
			{
				num4++;
			}
			if (v[I[l] + h] == num3)
			{
				num5++;
			}
		}
		num4 += start;
		num5 += num4;
		int num6 = start;
		int num7 = 0;
		int num8 = 0;
		while (num6 < num4)
		{
			if (v[I[num6] + h] < num3)
			{
				num6++;
			}
			else if (v[I[num6] + h] == num3)
			{
				Swap(ref I[num6], ref I[num4 + num7]);
				num7++;
			}
			else
			{
				Swap(ref I[num6], ref I[num5 + num8]);
				num8++;
			}
		}
		while (num4 + num7 < num5)
		{
			if (v[I[num4 + num7] + h] == num3)
			{
				num7++;
				continue;
			}
			Swap(ref I[num4 + num7], ref I[num5 + num8]);
			num8++;
		}
		if (num4 > start)
		{
			Split(I, v, start, num4 - start, h);
		}
		for (num6 = 0; num6 < num5 - num4; num6++)
		{
			v[I[num4 + num6]] = num5 - 1;
		}
		if (num4 == num5 - 1)
		{
			I[num4] = -1;
		}
		if (start + len > num5)
		{
			Split(I, v, num5, start + len - num5, h);
		}
	}

	private static int[] SuffixSort(byte[] oldData)
	{
		int[] array = new int[256];
		foreach (byte b in oldData)
		{
			array[b]++;
		}
		for (int j = 1; j < 256; j++)
		{
			array[j] += array[j - 1];
		}
		for (int num = 255; num > 0; num--)
		{
			array[num] = array[num - 1];
		}
		array[0] = 0;
		int[] array2 = new int[oldData.Length + 1];
		for (int k = 0; k < oldData.Length; k++)
		{
			array2[++array[oldData[k]]] = k;
		}
		int[] array3 = new int[oldData.Length + 1];
		for (int l = 0; l < oldData.Length; l++)
		{
			array3[l] = array[oldData[l]];
		}
		for (int m = 1; m < 256; m++)
		{
			if (array[m] == array[m - 1] + 1)
			{
				array2[array[m]] = -1;
			}
		}
		array2[0] = -1;
		int num2 = 1;
		while (array2[0] != -(oldData.Length + 1))
		{
			int num3 = 0;
			int num4 = 0;
			while (num4 < oldData.Length + 1)
			{
				if (array2[num4] < 0)
				{
					num3 -= array2[num4];
					num4 -= array2[num4];
					continue;
				}
				if (num3 != 0)
				{
					array2[num4 - num3] = -num3;
				}
				num3 = array3[array2[num4]] + 1 - num4;
				Split(array2, array3, num4, num3, num2);
				num4 += num3;
				num3 = 0;
			}
			if (num3 != 0)
			{
				array2[num4 - num3] = -num3;
			}
			num2 += num2;
		}
		for (int n = 0; n < oldData.Length + 1; n++)
		{
			array2[array3[n]] = n;
		}
		return array2;
	}

	private static void Swap(ref int first, ref int second)
	{
		int num = first;
		first = second;
		second = num;
	}

	private static long ReadInt64(byte[] buf, int offset)
	{
		long num = buf[offset + 7] & 0x7F;
		for (int num2 = 6; num2 >= 0; num2--)
		{
			num *= 256;
			num += buf[offset + num2];
		}
		if ((buf[offset + 7] & 0x80) != 0)
		{
			num = -num;
		}
		return num;
	}

	private static void WriteInt64(long value, byte[] buf, int offset)
	{
		long num = ((value < 0) ? (-value) : value);
		for (int i = 0; i < 8; i++)
		{
			buf[offset + i] = (byte)num;
			num >>= 8;
		}
		if (value < 0)
		{
			buf[offset + 7] |= 128;
		}
	}

	private static byte[] ReadExactly(Stream stream, int count)
	{
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		byte[] array = new byte[count];
		ReadExactly(stream, array, 0, count);
		return array;
	}

	private static void ReadExactly(Stream stream, byte[] buffer, int offset, int count)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || buffer.Length - offset < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		while (count > 0)
		{
			int num = stream.Read(buffer, offset, count);
			if (num == 0)
			{
				throw new EndOfStreamException();
			}
			offset += num;
			count -= num;
		}
	}
}
