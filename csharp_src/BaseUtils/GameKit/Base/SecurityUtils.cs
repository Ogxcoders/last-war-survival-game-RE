using System;
using System.IO;
using System.Security.Cryptography;

namespace GameKit.Base;

public static class SecurityUtils
{
	public static string ComputeSHA1(byte[] data)
	{
		byte[] array = new SHA1CryptoServiceProvider().ComputeHash(data);
		string text = "";
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text += Convert.ToString(b, 16).PadLeft(2, '0');
		}
		return text;
	}

	public static string ComputeSHA1(string filePath)
	{
		using FileStream fileStream = File.OpenRead(filePath);
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, array.Length);
		return ComputeSHA1(array);
	}

	public static string ComputeMD5(byte[] data)
	{
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(data);
		string text = "";
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			text += Convert.ToString(b, 16).PadLeft(2, '0');
		}
		return text;
	}

	public static string ComputeMD5(string filePath)
	{
		using FileStream fileStream = File.OpenRead(filePath);
		byte[] array = new byte[fileStream.Length];
		fileStream.Read(array, 0, array.Length);
		return ComputeMD5(array);
	}
}
