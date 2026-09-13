using System.Runtime.CompilerServices;

namespace GameKit.Base;

public class EncryptUtils
{
	private static byte[] lua_header = new byte[4] { 27, 76, 117, 97 };

	private static byte[] enc_header = new byte[4] { 11, 69, 78, 67 };

	private static byte enc_byte = 83;

	private static byte[] lua_header_extend = new byte[32]
	{
		27, 76, 117, 97, 83, 1, 25, 147, 13, 10,
		26, 10, 4, 4, 8, 8, 120, 86, 0, 0,
		0, 0, 0, 0, 0, 0, 0, 0, 0, 40,
		119, 64
	};

	private static byte[] lua_header_after_path = new byte[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };

	private static byte[] lua_tail = new byte[9] { 1, 0, 0, 0, 5, 95, 69, 78, 86 };

	private static byte[] _key = new byte[32];

	private static byte[] _nonce = new byte[12];

	private static byte[] _xor_key = new byte[32];

	private static bool _isInit = false;

	private static byte[] bzip_header = new byte[3] { 66, 90, 104 };

	private static byte[] bsdiff_patch_header = new byte[6] { 66, 83, 68, 73, 70, 70 };

	private static byte[] pure_encoded_bzip_header = new byte[3] { 80, 82, 66 };

	private static byte[] pure_encoded_bsdiff_patch_header = new byte[6] { 80, 85, 82, 66, 68, 80 };

	private static byte[] pkzip_header = new byte[4] { 80, 75, 3, 4 };

	private static byte[] chacha_pkzip_header = new byte[4] { 67, 72, 65, 67 };

	private static uint[] chacha_state = new uint[16];

	private static uint[] chacha_keystream = new uint[16];

	private static uint[] chacha_constants = new uint[4] { 1634760805u, 857760878u, 2036477234u, 1797285236u };

	private static int chacha_rounds = 8;

	private static int chacha_block_size = 64;

	private static byte[] dummy_key = new byte[32]
	{
		1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
		11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
		21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
		31, 32
	};

	private static byte[] dummy_nonce = new byte[12]
	{
		33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
		43, 44
	};

	private static byte[] dummy_xor_key = new byte[32]
	{
		9, 10, 11, 12, 17, 34, 67, 84, 117, 6,
		7, 8, 9, 10, 11, 12, 9, 10, 11, 12,
		17, 34, 67, 84, 117, 6, 7, 8, 9, 10,
		11, 12
	};

	private unsafe static int getLuaPathEndIndex(byte* data, int length)
	{
		int i = lua_header_extend.Length;
		while (i < length)
		{
			int* ptr = (int*)(data + i);
			i += 4;
			if (*ptr == 0)
			{
				break;
			}
		}
		for (; i < length; i++)
		{
			if (data[i] == 1)
			{
				return i - lua_header_after_path.Length;
			}
		}
		return -1;
	}

	private unsafe static void flash_xor(byte* data, int start, int end, byte* key, int key_length)
	{
		for (int i = start; i < end; i++)
		{
			byte* num = data + i;
			*num ^= key[i % key_length];
		}
	}

	private unsafe static bool IsHeaderMatch(byte* data, byte* header, int header_length)
	{
		byte* ptr = data;
		byte* ptr2 = header;
		for (int i = 0; i < header_length; i++)
		{
			if (*ptr != *ptr2)
			{
				return false;
			}
			ptr++;
			ptr2++;
		}
		return true;
	}

	private unsafe static bool IsHeaderMatch(byte[] data, byte[] header)
	{
		if (data == null || header == null || data.Length < header.Length)
		{
			return false;
		}
		fixed (byte* data2 = data)
		{
			fixed (byte* header2 = header)
			{
				return IsHeaderMatch(data2, header2, header.Length);
			}
		}
	}

	public static bool IsLuaByteCode(byte[] data)
	{
		return IsHeaderMatch(data, lua_header);
	}

	public static bool IsLuaEncCode(byte[] data)
	{
		return IsHeaderMatch(data, enc_header);
	}

	private unsafe static void SuperEncrypt(byte* pData, int length)
	{
		fixed (byte* ptr = enc_header)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					fixed (byte* xor_key = _xor_key)
					{
						int* ptr2 = (int*)ptr;
						*(int*)pData = *ptr2;
						byte* ptr3 = pData + enc_header.Length;
						int num = lua_header_extend.Length - enc_header.Length;
						for (int i = 0; i < num; i++)
						{
							ptr3[i] = enc_byte;
						}
						int num2 = 0;
						num2 = getLuaPathEndIndex(pData, length);
						if (num2 == -1)
						{
							return;
						}
						int start = lua_header_extend.Length + 1;
						flash_xor(pData, start, num2, xor_key, _xor_key.Length);
						int start2 = num2 + lua_header_after_path.Length + 1;
						int end = length - lua_tail.Length;
						chacha_encrypt(pData, key, nonce, start2, end);
						byte* ptr4 = pData + length - lua_tail.Length;
						int num3 = lua_tail.Length;
						for (int j = 0; j < num3; j++)
						{
							ptr4[j] = enc_byte;
						}
					}
				}
			}
		}
	}

	private unsafe static void SuperDecrypt(byte* pData, int length)
	{
		fixed (byte* key = _key)
		{
			fixed (byte* nonce = _nonce)
			{
				fixed (byte* ptr = lua_header_extend)
				{
					fixed (byte* ptr2 = lua_tail)
					{
						fixed (byte* xor_key = _xor_key)
						{
							long* ptr3 = (long*)pData;
							long* ptr4 = (long*)ptr;
							int num = lua_header_extend.Length / 8;
							for (int i = 0; i < num; i++)
							{
								*ptr3 = *ptr4;
								ptr3++;
								ptr4++;
							}
							int num2 = 0;
							num2 = getLuaPathEndIndex(pData, length);
							if (num2 == -1)
							{
								return;
							}
							int start = lua_header_extend.Length + 1;
							flash_xor(pData, start, num2, xor_key, _xor_key.Length);
							int start2 = num2 + lua_header_after_path.Length + 1;
							int end = length - lua_tail.Length;
							chacha_encrypt(pData, key, nonce, start2, end);
							byte* ptr5 = pData + length - lua_tail.Length;
							int num3 = lua_tail.Length;
							for (int j = 0; j < num3; j++)
							{
								ptr5[j] = ptr2[j];
							}
						}
					}
				}
			}
		}
	}

	public unsafe static void SuperEncrypt(byte[] data, long offset, int length)
	{
		DoInit();
		if (!_isInit)
		{
			return;
		}
		int num = lua_header.Length;
		if (num >= length)
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* header = lua_header)
			{
				if (IsHeaderMatch(ptr + offset, header, num))
				{
					SuperEncrypt(ptr + offset, length);
				}
			}
		}
	}

	public unsafe static void SuperDecrypt(byte[] data, long offset, int length)
	{
		DoInit();
		if (!_isInit)
		{
			return;
		}
		int num = enc_header.Length;
		if (num >= length)
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* header = enc_header)
			{
				if (IsHeaderMatch(ptr + offset, header, num))
				{
					SuperDecrypt(ptr + offset, length);
				}
			}
		}
	}

	public unsafe static void SuperEncrypt(byte[] data)
	{
		DoInit();
		if (_isInit && IsLuaByteCode(data))
		{
			int length = data.Length;
			fixed (byte* pData = data)
			{
				SuperEncrypt(pData, length);
			}
		}
	}

	public unsafe static void SuperDecrypt(byte[] data)
	{
		DoInit();
		if (_isInit && IsLuaEncCode(data))
		{
			int length = data.Length;
			fixed (byte* pData = data)
			{
				SuperDecrypt(pData, length);
			}
		}
	}

	public unsafe static void FromBZipToChacha(byte[] data)
	{
		DoInit();
		if (!_isInit || !IsHeaderMatch(data, bzip_header))
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					*ptr = pure_encoded_bzip_header[0];
					ptr[1] = pure_encoded_bzip_header[1];
					ptr[2] = pure_encoded_bzip_header[2];
					chacha_encrypt(ptr, key, nonce, 4, data.Length);
				}
			}
		}
	}

	public unsafe static void FromChachaToBZip(byte[] data)
	{
		DoInit();
		if (!_isInit || !IsHeaderMatch(data, pure_encoded_bzip_header))
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					*ptr = bzip_header[0];
					ptr[1] = bzip_header[1];
					ptr[2] = bzip_header[2];
					chacha_encrypt(ptr, key, nonce, 4, data.Length);
				}
			}
		}
	}

	public static bool IsChachaPackage(byte[] data)
	{
		return IsHeaderMatch(data, pure_encoded_bzip_header);
	}

	public unsafe static void FromBsDiffPatchToChacha(byte[] data)
	{
		DoInit();
		if (!_isInit || !IsHeaderMatch(data, bsdiff_patch_header))
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					*ptr = pure_encoded_bsdiff_patch_header[0];
					ptr[1] = pure_encoded_bsdiff_patch_header[1];
					ptr[2] = pure_encoded_bsdiff_patch_header[2];
					ptr[3] = pure_encoded_bsdiff_patch_header[3];
					ptr[4] = pure_encoded_bsdiff_patch_header[4];
					ptr[5] = pure_encoded_bsdiff_patch_header[5];
					chacha_encrypt(ptr, key, nonce, 8, data.Length);
				}
			}
		}
	}

	public unsafe static void FromChachaToBsDiffPatch(byte[] data)
	{
		DoInit();
		if (!_isInit || !IsHeaderMatch(data, pure_encoded_bsdiff_patch_header))
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					*ptr = bsdiff_patch_header[0];
					ptr[1] = bsdiff_patch_header[1];
					ptr[2] = bsdiff_patch_header[2];
					ptr[3] = bsdiff_patch_header[3];
					ptr[4] = bsdiff_patch_header[4];
					ptr[5] = bsdiff_patch_header[5];
					chacha_encrypt(ptr, key, nonce, 8, data.Length);
				}
			}
		}
	}

	public static bool IsChachaPatch(byte[] data)
	{
		return IsHeaderMatch(data, pure_encoded_bsdiff_patch_header);
	}

	public unsafe static void FromPKZipToChacha(byte[] data)
	{
		DoInit();
		if (!_isInit || !IsHeaderMatch(data, pkzip_header))
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					*ptr = chacha_pkzip_header[0];
					ptr[1] = chacha_pkzip_header[1];
					ptr[2] = chacha_pkzip_header[2];
					ptr[3] = chacha_pkzip_header[3];
					chacha_encrypt(ptr, key, nonce, 4, data.Length);
				}
			}
		}
	}

	public unsafe static void FromChachaToPKZip(byte[] data)
	{
		DoInit();
		if (!_isInit || !IsHeaderMatch(data, chacha_pkzip_header))
		{
			return;
		}
		fixed (byte* ptr = data)
		{
			fixed (byte* key = _key)
			{
				fixed (byte* nonce = _nonce)
				{
					*ptr = pkzip_header[0];
					ptr[1] = pkzip_header[1];
					ptr[2] = pkzip_header[2];
					ptr[3] = pkzip_header[3];
					chacha_encrypt(ptr, key, nonce, 4, data.Length);
				}
			}
		}
	}

	public static bool IsChachaTable(byte[] data)
	{
		return IsHeaderMatch(data, chacha_pkzip_header);
	}

	private unsafe static void chacha_encrypt(byte* data, byte* key, byte* nonce, int start, int end)
	{
		fixed (uint* ptr = chacha_state)
		{
			fixed (uint* ptr2 = chacha_keystream)
			{
				fixed (uint* ptr3 = chacha_constants)
				{
					*ptr = *ptr3;
					ptr[1] = ptr3[1];
					ptr[2] = ptr3[2];
					ptr[3] = ptr3[3];
					ptr[4] = *(uint*)key;
					ptr[5] = ((uint*)key)[1];
					ptr[6] = ((uint*)key)[2];
					ptr[7] = ((uint*)key)[3];
					ptr[8] = ((uint*)key)[4];
					ptr[9] = ((uint*)key)[5];
					ptr[10] = ((uint*)key)[6];
					ptr[11] = ((uint*)key)[7];
					ptr[12] = 0u;
					ptr[13] = *(uint*)nonce;
					ptr[14] = ((uint*)nonce)[1];
					ptr[15] = ((uint*)nonce)[2];
					int num = chacha_rounds / 2;
					int num2 = chacha_block_size / 4;
					for (int i = start; i < end; i += chacha_block_size)
					{
						for (int j = 0; j < 16; j++)
						{
							ptr2[j] = ptr[j];
						}
						for (int k = 0; k < num; k++)
						{
							QuarterRound(ptr2, ptr2 + 4, ptr2 + 8, ptr2 + 12);
							QuarterRound(ptr2 + 1, ptr2 + 5, ptr2 + 9, ptr2 + 13);
							QuarterRound(ptr2 + 2, ptr2 + 6, ptr2 + 10, ptr2 + 14);
							QuarterRound(ptr2 + 3, ptr2 + 7, ptr2 + 11, ptr2 + 15);
							QuarterRound(ptr2, ptr2 + 5, ptr2 + 10, ptr2 + 15);
							QuarterRound(ptr2 + 1, ptr2 + 6, ptr2 + 11, ptr2 + 12);
							QuarterRound(ptr2 + 2, ptr2 + 7, ptr2 + 8, ptr2 + 13);
							QuarterRound(ptr2 + 3, ptr2 + 4, ptr2 + 9, ptr2 + 14);
						}
						if (i + chacha_block_size > end)
						{
							byte* ptr4 = (byte*)ptr2;
							int num3 = end - i;
							byte* ptr5 = data + i;
							for (int l = 0; l < num3; l++)
							{
								byte* num4 = ptr5 + l;
								*num4 ^= ptr4[l];
							}
							return;
						}
						uint* ptr6 = (uint*)(data + i);
						for (int m = 0; m < num2; m++)
						{
							ptr6[m] ^= ptr2[m];
						}
						ptr[12]++;
					}
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static void QuarterRound(uint* a, uint* b, uint* c, uint* d)
	{
		*a += *b;
		*d ^= *a;
		*d = (*d << 16) | (*d >> 16);
		*c += *d;
		*b ^= *c;
		*b = (*b << 12) | (*b >> 20);
		*a += *b;
		*d ^= *a;
		*d = (*d << 8) | (*d >> 24);
		*c += *d;
		*b ^= *c;
		*b = (*b << 7) | (*b >> 25);
	}

	public static void DoInit()
	{
		Init(dummy_key, dummy_nonce, dummy_xor_key);
	}

	public unsafe static bool Init(byte[] key, byte[] nonce, byte[] xor_key)
	{
		if (_isInit)
		{
			return true;
		}
		if (key == null || nonce == null || xor_key == null)
		{
			return false;
		}
		if (key.Length != 32 || nonce.Length != 12 || xor_key.Length != 32)
		{
			return false;
		}
		fixed (byte* key2 = _key)
		{
			fixed (byte* ptr = key)
			{
				fixed (byte* nonce2 = _nonce)
				{
					fixed (byte* ptr2 = nonce)
					{
						fixed (byte* xor_key2 = _xor_key)
						{
							fixed (byte* ptr3 = xor_key)
							{
								for (int i = 0; i < 32; i++)
								{
									key2[i] = ptr[i];
								}
								for (int j = 0; j < 12; j++)
								{
									nonce2[j] = ptr2[j];
								}
								for (int k = 0; k < 32; k++)
								{
									xor_key2[k] = ptr3[k];
								}
								chacha_encrypt(key2, xor_key2, nonce2, 0, 32);
								chacha_encrypt(xor_key2, key2, nonce2, 0, 32);
							}
						}
					}
				}
			}
		}
		_isInit = true;
		return true;
	}
}
