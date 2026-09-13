using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl;

internal class AESHelper
{
	public static RSAParameters _p = default(RSAParameters);

	public static string _salt = "";

	public static string AESEncrypt(string Data, string Key)
	{
		MemoryStream memoryStream = new MemoryStream();
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		byte[] bytes = Encoding.UTF8.GetBytes(Data);
		byte[] array = new byte[32];
		Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(array.Length)), array, array.Length);
		rijndaelManaged.Mode = CipherMode.ECB;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.Key = array;
		CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
		try
		{
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		finally
		{
			cryptoStream.Close();
			memoryStream.Close();
			rijndaelManaged.Clear();
		}
	}

	public static string ToUrlSafeBase64(string base64)
	{
		return base64.Replace('+', '-').Replace('/', '_').TrimEnd(new char[1] { '=' });
	}

	public static string FromUrlSafeBase64(string urlSafeBase64)
	{
		string text = urlSafeBase64.Replace('-', '+').Replace('_', '/');
		int num = text.Length % 4;
		if (num > 0)
		{
			text = text.PadRight(text.Length + (4 - num), '=');
		}
		return text;
	}

	public static string ToPemFormat(string base64Key)
	{
		string text = "-----BEGIN PUBLIC KEY-----";
		string text2 = "-----END PUBLIC KEY-----";
		string text3 = InsertLineBreaks(base64Key, 64);
		return text + "\n" + text3 + "\n" + text2;
	}

	private static string InsertLineBreaks(string base64, int lineLength)
	{
		int length = base64.Length;
		int num = (int)Math.Ceiling((double)length / (double)lineLength);
		string text = "";
		for (int i = 0; i < num; i++)
		{
			int num2 = i * lineLength;
			int num3 = Math.Min(num2 + lineLength, length);
			text = text + base64.Substring(num2, num3 - num2) + "\n";
		}
		return text.TrimEnd(new char[1] { '\n' });
	}

	public static void Setp(string p)
	{
		_p = PemToRsaParameters(ToPemFormat(p));
	}

	public static string GetGSLUuid(string key)
	{
		return ToUrlSafeBase64(UuidEncrypt(key));
	}

	public static string GenerateRandomSalt(int length)
	{
		Random random = new Random();
		char[] array = new char[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-/=<>?{}[]"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-/=<>?{}[]".Length)];
		}
		return new string(array);
	}

	public static string GetData(string data, string key)
	{
		return ToUrlSafeBase64(AESEncrypt(data, key));
	}

	public static Tuple<string, string> GetGSLRequestParams(string deviceId, string requestData)
	{
		_salt = GenerateRandomSalt(20);
		string gSLUuid = GetGSLUuid(_salt);
		string data = GetData(requestData, GetMd5Hash(_salt));
		return new Tuple<string, string>(gSLUuid, data);
	}

	public static string GetGSLResp(string deviceId, string respData)
	{
		return AESDecrypt(FromUrlSafeBase64(respData), GetMd5Hash(_salt));
	}

	public static string UuidEncrypt(string content)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(content);
		try
		{
			using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(_p);
			return Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(bytes, fOAEP: false));
		}
		catch (Exception ex)
		{
			return "Encryption failed: " + ex.Message;
		}
	}

	public static RSAParameters PemToRsaParameters(string pem)
	{
		using StringReader reader = new StringReader(pem);
		if (!(((new PemReader(reader).ReadObject() as AsymmetricKeyParameter) ?? throw new Exception("Invalid PEM file format.")) is RsaKeyParameters rsaKeyParameters))
		{
			throw new Exception("Not an RSA public key.");
		}
		return new RSAParameters
		{
			Modulus = rsaKeyParameters.Modulus.ToByteArray(),
			Exponent = rsaKeyParameters.Exponent.ToByteArray()
		};
	}

	public static string AESDecrypt(string Data, string Key)
	{
		byte[] array = Convert.FromBase64String(Data);
		byte[] array2 = new byte[32];
		Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(array2.Length)), array2, array2.Length);
		MemoryStream memoryStream = new MemoryStream(array);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Mode = CipherMode.ECB;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.Key = array2;
		CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read);
		try
		{
			byte[] array3 = new byte[array.Length + 32];
			int num = cryptoStream.Read(array3, 0, array.Length + 32);
			byte[] array4 = new byte[num];
			Array.Copy(array3, 0, array4, 0, num);
			return Encoding.UTF8.GetString(array4);
		}
		finally
		{
			cryptoStream.Close();
			memoryStream.Close();
			rijndaelManaged.Clear();
		}
	}

	public static string Encrypt(string countent, string Key)
	{
		AesEncryptor aesEncryptor = new AesEncryptor(Key);
		string result = aesEncryptor.EncryptString(countent);
		aesEncryptor.Dispose();
		return result;
	}

	public static string Decrypt(string ciphertext, string Key)
	{
		AesEncryptor aesEncryptor = new AesEncryptor(Key);
		string result = aesEncryptor.DecryptString(ciphertext);
		aesEncryptor.Dispose();
		return result;
	}

	public static string GetMd5Hash(string input)
	{
		byte[] array = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}
}
