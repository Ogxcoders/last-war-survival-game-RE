using System;
using System.Text;

public class AesEncryptor
{
	private AES m_pEncryptor;

	public AesEncryptor(byte[] key)
	{
		m_pEncryptor = new AES(key);
	}

	public AesEncryptor(string AESkey)
	{
		byte[] bytes = Encoding.Default.GetBytes(AESkey);
		byte[] array = new byte[17];
		int i;
		for (i = 0; i < 16; i++)
		{
			if (i < AESkey.Length)
			{
				array[i] = bytes[i];
			}
			else
			{
				array[i] = 48;
			}
		}
		array[i] = 0;
		m_pEncryptor = new AES(array);
	}

	public void Dispose()
	{
		m_pEncryptor = null;
	}

	public string EncryptString(string strInfor)
	{
		int length = strInfor.Length;
		int num = 16 - length % 16;
		byte[] input = new byte[length + num];
		input[length + num - 1] = 0;
		Array.Copy(Encoding.UTF8.GetBytes(strInfor), input, length);
		m_pEncryptor.Cipher(ref input);
		return Convert.ToBase64String(input);
	}

	public string DecryptString(string strMessage)
	{
		byte[] dest = new byte[strMessage.Length / 2];
		Hex2Byte(strMessage, strMessage.Length, ref dest);
		m_pEncryptor.InvCipher(ref dest);
		return Encoding.UTF8.GetString(dest);
	}

	public void Byte2Hex(byte[] src, int len, ref string dest)
	{
		for (int i = 0; i < len; i++)
		{
			dest = dest.Substring(0, i * 2) + $"{src[i]:X2}";
		}
	}

	public void Hex2Byte(string src, int len, ref byte[] dest)
	{
		char[] array = src.ToCharArray();
		int num = len / 2;
		for (int i = 0; i < num; i++)
		{
			dest[i] = (byte)(Char2Int(array[i * 2]) * 16 + Char2Int(array[i * 2 + 1]));
		}
	}

	public int Char2Int(char c)
	{
		if ('0' <= c && c <= '9')
		{
			return c - 48;
		}
		if ('a' <= c && c <= 'f')
		{
			return c - 97 + 10;
		}
		if ('A' <= c && c <= 'F')
		{
			return c - 65 + 10;
		}
		return -1;
	}
}
