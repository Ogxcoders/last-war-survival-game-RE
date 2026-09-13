using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using Mono.Security.Cryptography;

namespace Mono.Security;

internal sealed class StrongName
{
	internal class StrongNameSignature
	{
		private byte[] hash;

		private byte[] signature;

		private uint signaturePosition;

		private uint signatureLength;

		private uint metadataPosition;

		private uint metadataLength;

		private byte cliFlag;

		private uint cliFlagPosition;

		public byte[] Hash
		{
			get
			{
				return hash;
			}
			set
			{
				hash = value;
			}
		}

		public byte[] Signature
		{
			get
			{
				return signature;
			}
			set
			{
				signature = value;
			}
		}

		public uint MetadataPosition
		{
			get
			{
				return metadataPosition;
			}
			set
			{
				metadataPosition = value;
			}
		}

		public uint MetadataLength
		{
			get
			{
				return metadataLength;
			}
			set
			{
				metadataLength = value;
			}
		}

		public uint SignaturePosition
		{
			get
			{
				return signaturePosition;
			}
			set
			{
				signaturePosition = value;
			}
		}

		public uint SignatureLength
		{
			get
			{
				return signatureLength;
			}
			set
			{
				signatureLength = value;
			}
		}

		public byte CliFlag
		{
			get
			{
				return cliFlag;
			}
			set
			{
				cliFlag = value;
			}
		}

		public uint CliFlagPosition
		{
			get
			{
				return cliFlagPosition;
			}
			set
			{
				cliFlagPosition = value;
			}
		}
	}

	internal enum StrongNameOptions
	{
		Metadata,
		Signature
	}

	private RSA rsa;

	private byte[] publicKey;

	private byte[] keyToken;

	private string tokenAlgorithm;

	private static object lockObject = new object();

	private static bool initialized;

	public bool CanSign
	{
		get
		{
			if (rsa == null)
			{
				return false;
			}
			if (RSA is RSACryptoServiceProvider)
			{
				return !(rsa as RSACryptoServiceProvider).PublicOnly;
			}
			if (RSA is RSAManaged)
			{
				return !(rsa as RSAManaged).PublicOnly;
			}
			try
			{
				RSAParameters rSAParameters = rsa.ExportParameters(includePrivateParameters: true);
				return rSAParameters.D != null && rSAParameters.P != null && rSAParameters.Q != null;
			}
			catch (CryptographicException)
			{
				return false;
			}
		}
	}

	public RSA RSA
	{
		get
		{
			if (rsa == null)
			{
				rsa = RSA.Create();
			}
			return rsa;
		}
		set
		{
			rsa = value;
			InvalidateCache();
		}
	}

	public byte[] PublicKey
	{
		get
		{
			if (publicKey == null)
			{
				byte[] array = CryptoConvert.ToCapiKeyBlob(rsa, includePrivateKey: false);
				publicKey = new byte[32 + (rsa.KeySize >> 3)];
				publicKey[0] = array[4];
				publicKey[1] = array[5];
				publicKey[2] = array[6];
				publicKey[3] = array[7];
				publicKey[4] = 4;
				publicKey[5] = 128;
				publicKey[6] = 0;
				publicKey[7] = 0;
				byte[] bytes = BitConverterLE.GetBytes(publicKey.Length - 12);
				publicKey[8] = bytes[0];
				publicKey[9] = bytes[1];
				publicKey[10] = bytes[2];
				publicKey[11] = bytes[3];
				publicKey[12] = 6;
				Buffer.BlockCopy(array, 1, publicKey, 13, publicKey.Length - 13);
				publicKey[23] = 49;
			}
			return (byte[])publicKey.Clone();
		}
	}

	public byte[] PublicKeyToken
	{
		get
		{
			if (keyToken == null)
			{
				byte[] array = PublicKey;
				if (array == null)
				{
					return null;
				}
				byte[] array2 = GetHashAlgorithm(TokenAlgorithm).ComputeHash(array);
				keyToken = new byte[8];
				Buffer.BlockCopy(array2, array2.Length - 8, keyToken, 0, 8);
				Array.Reverse(keyToken, 0, 8);
			}
			return (byte[])keyToken.Clone();
		}
	}

	public string TokenAlgorithm
	{
		get
		{
			if (tokenAlgorithm == null)
			{
				tokenAlgorithm = "SHA1";
			}
			return tokenAlgorithm;
		}
		set
		{
			string text = value.ToUpper(CultureInfo.InvariantCulture);
			if (text == "SHA1" || text == "MD5")
			{
				tokenAlgorithm = value;
				InvalidateCache();
				return;
			}
			throw new ArgumentException("Unsupported hash algorithm for token");
		}
	}

	public StrongName()
	{
	}

	public StrongName(int keySize)
	{
		rsa = new RSAManaged(keySize);
	}

	public StrongName(byte[] data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		if (data.Length == 16)
		{
			int num = 0;
			int num2 = 0;
			while (num < data.Length)
			{
				num2 += data[num++];
			}
			if (num2 == 4)
			{
				publicKey = (byte[])data.Clone();
			}
		}
		else
		{
			RSA = CryptoConvert.FromCapiKeyBlob(data);
			if (rsa == null)
			{
				throw new ArgumentException("data isn't a correctly encoded RSA public key");
			}
		}
	}

	public StrongName(RSA rsa)
	{
		if (rsa == null)
		{
			throw new ArgumentNullException("rsa");
		}
		RSA = rsa;
	}

	private void InvalidateCache()
	{
		publicKey = null;
		keyToken = null;
	}

	private static HashAlgorithm GetHashAlgorithm(string algorithm)
	{
		return HashAlgorithm.Create(algorithm);
	}

	public byte[] GetBytes()
	{
		return CryptoConvert.ToCapiPrivateKeyBlob(RSA);
	}

	private uint RVAtoPosition(uint r, int sections, byte[] headers)
	{
		for (int i = 0; i < sections; i++)
		{
			uint num = BitConverterLE.ToUInt32(headers, i * 40 + 20);
			uint num2 = BitConverterLE.ToUInt32(headers, i * 40 + 12);
			int num3 = (int)BitConverterLE.ToUInt32(headers, i * 40 + 8);
			if (num2 <= r && r < num2 + num3)
			{
				return num + r - num2;
			}
		}
		return 0u;
	}

	private static StrongNameSignature Error(string a)
	{
		return null;
	}

	private static byte[] ReadMore(Stream stream, byte[] a, int newSize)
	{
		int num = a.Length;
		Array.Resize(ref a, newSize);
		if (newSize <= num)
		{
			return a;
		}
		int num2 = newSize - num;
		if (stream.Read(a, num, num2) != num2)
		{
			return null;
		}
		return a;
	}

	internal StrongNameSignature StrongHash(Stream stream, StrongNameOptions options)
	{
		byte[] array = new byte[64];
		int num = 0;
		int num2 = stream.Read(array, 0, 64);
		if (num2 == 64 && array[0] == 77 && array[1] == 90)
		{
			num = BitConverterLE.ToInt32(array, 60);
			if (num < 64)
			{
				return Error("peHeader_lt_64");
			}
			array = ReadMore(stream, array, num);
			if (array == null)
			{
				return Error("read_mz2_failed");
			}
		}
		else
		{
			if (num2 < 4 || array[0] != 80 || array[1] != 69 || array[2] != 0 || array[3] != 0)
			{
				return Error("read_mz_or_mzsig_failed");
			}
			stream.Position = 0L;
			array = new byte[0];
		}
		int num3 = 2;
		int num4 = 24 + num3;
		byte[] array2 = new byte[num4];
		if (stream.Read(array2, 0, num4) != num4 || array2[0] != 80 || array2[1] != 69 || array2[2] != 0 || array2[3] != 0)
		{
			return Error("read_minimumHeadersSize_or_pesig_failed");
		}
		num3 = BitConverterLE.ToUInt16(array2, 20);
		if (num3 < 2)
		{
			return Error($"sizeOfOptionalHeader_lt_2 ${num3}");
		}
		int num5 = 24 + num3;
		if (num5 < 24)
		{
			return Error("headers_overflow");
		}
		array2 = ReadMore(stream, array2, num5);
		if (array2 == null)
		{
			return Error("read_pe2_failed");
		}
		uint num6 = BitConverterLE.ToUInt16(array2, 24);
		int num7 = 0;
		bool flag = false;
		switch (num6)
		{
		case 523u:
			num7 = 16;
			break;
		case 263u:
			flag = true;
			break;
		default:
			return Error("bad_magic_value");
		case 267u:
			break;
		}
		uint num8 = 0u;
		if (!flag)
		{
			if (num3 >= 116 + num7 + 4)
			{
				num8 = BitConverterLE.ToUInt32(array2, 116 + num7);
			}
			for (int i = 64; i < num3 && i < 68; i++)
			{
				array2[24 + i] = 0;
			}
			for (int j = 128 + num7; j < num3 && j < 136 + num7; j++)
			{
				array2[24 + j] = 0;
			}
		}
		int num9 = BitConverterLE.ToUInt16(array2, 6);
		byte[] array3 = new byte[num9 * 40];
		if (stream.Read(array3, 0, array3.Length) != array3.Length)
		{
			return Error("read_section_headers_failed");
		}
		uint num10 = 0u;
		uint num11 = 0u;
		uint num12 = 0u;
		uint num13 = 0u;
		if (15 < num8 && num3 >= 216 + num7)
		{
			uint r = BitConverterLE.ToUInt32(array2, 232 + num7);
			uint num14 = RVAtoPosition(r, num9, array3);
			int num15 = BitConverterLE.ToInt32(array2, 236 + num7);
			byte[] array4 = new byte[num15];
			stream.Position = num14;
			if (stream.Read(array4, 0, num15) != num15)
			{
				return Error("read_cli_header_failed");
			}
			uint r2 = BitConverterLE.ToUInt32(array4, 32);
			num10 = RVAtoPosition(r2, num9, array3);
			num11 = BitConverterLE.ToUInt32(array4, 36);
			uint r3 = BitConverterLE.ToUInt32(array4, 8);
			num12 = RVAtoPosition(r3, num9, array3);
			num13 = BitConverterLE.ToUInt32(array4, 12);
		}
		StrongNameSignature strongNameSignature = new StrongNameSignature();
		strongNameSignature.SignaturePosition = num10;
		strongNameSignature.SignatureLength = num11;
		strongNameSignature.MetadataPosition = num12;
		strongNameSignature.MetadataLength = num13;
		using HashAlgorithm hashAlgorithm = HashAlgorithm.Create(TokenAlgorithm);
		if (options == StrongNameOptions.Metadata)
		{
			hashAlgorithm.Initialize();
			byte[] buffer = new byte[num13];
			stream.Position = num12;
			if (stream.Read(buffer, 0, (int)num13) != (int)num13)
			{
				return Error("read_cli_metadata_failed");
			}
			strongNameSignature.Hash = hashAlgorithm.ComputeHash(buffer);
			return strongNameSignature;
		}
		using (CryptoStream cryptoStream = new CryptoStream(Stream.Null, hashAlgorithm, CryptoStreamMode.Write))
		{
			cryptoStream.Write(array, 0, array.Length);
			cryptoStream.Write(array2, 0, array2.Length);
			cryptoStream.Write(array3, 0, array3.Length);
			for (int k = 0; k < num9; k++)
			{
				uint num16 = BitConverterLE.ToUInt32(array3, k * 40 + 20);
				int num17 = BitConverterLE.ToInt32(array3, k * 40 + 16);
				byte[] array5 = new byte[num17];
				stream.Position = num16;
				if (stream.Read(array5, 0, num17) != num17)
				{
					return Error("read_section_failed");
				}
				if (num16 <= num10 && num10 < (uint)((int)num16 + num17))
				{
					int num18 = (int)(num10 - num16);
					if (num18 > 0)
					{
						cryptoStream.Write(array5, 0, num18);
					}
					strongNameSignature.Signature = new byte[num11];
					Buffer.BlockCopy(array5, num18, strongNameSignature.Signature, 0, (int)num11);
					Array.Reverse(strongNameSignature.Signature);
					int num19 = (int)(num18 + num11);
					int num20 = num17 - num19;
					if (num20 > 0)
					{
						cryptoStream.Write(array5, num19, num20);
					}
				}
				else
				{
					cryptoStream.Write(array5, 0, num17);
				}
			}
		}
		strongNameSignature.Hash = hashAlgorithm.Hash;
		return strongNameSignature;
	}

	public byte[] Hash(string fileName)
	{
		using FileStream stream = File.OpenRead(fileName);
		return StrongHash(stream, StrongNameOptions.Metadata).Hash;
	}

	public bool Sign(string fileName)
	{
		StrongNameSignature strongNameSignature;
		using (FileStream stream = File.OpenRead(fileName))
		{
			strongNameSignature = StrongHash(stream, StrongNameOptions.Signature);
		}
		if (strongNameSignature.Hash == null)
		{
			return false;
		}
		byte[] array = null;
		try
		{
			RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new RSAPKCS1SignatureFormatter(rsa);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm(TokenAlgorithm);
			array = rSAPKCS1SignatureFormatter.CreateSignature(strongNameSignature.Hash);
			Array.Reverse(array);
		}
		catch (CryptographicException)
		{
			return false;
		}
		using (FileStream fileStream = File.OpenWrite(fileName))
		{
			fileStream.Position = strongNameSignature.SignaturePosition;
			fileStream.Write(array, 0, array.Length);
		}
		return true;
	}

	public bool Verify(string fileName)
	{
		using FileStream stream = File.OpenRead(fileName);
		return Verify(stream);
	}

	public bool Verify(Stream stream)
	{
		StrongNameSignature strongNameSignature = StrongHash(stream, StrongNameOptions.Signature);
		if (strongNameSignature.Hash == null)
		{
			return false;
		}
		try
		{
			AssemblyHashAlgorithm algorithm = AssemblyHashAlgorithm.SHA1;
			if (tokenAlgorithm == "MD5")
			{
				algorithm = AssemblyHashAlgorithm.MD5;
			}
			return Verify(rsa, algorithm, strongNameSignature.Hash, strongNameSignature.Signature);
		}
		catch (CryptographicException)
		{
			return false;
		}
	}

	public static bool IsAssemblyStrongnamed(string assemblyName)
	{
		if (!initialized)
		{
			lock (lockObject)
			{
				if (!initialized)
				{
					StrongNameManager.LoadConfig(Environment.GetMachineConfigPath());
					initialized = true;
				}
			}
		}
		try
		{
			AssemblyName assemblyName2 = AssemblyName.GetAssemblyName(assemblyName);
			if (assemblyName2 == null)
			{
				return false;
			}
			byte[] mappedPublicKey = StrongNameManager.GetMappedPublicKey(assemblyName2.GetPublicKeyToken());
			if (mappedPublicKey == null || mappedPublicKey.Length < 12)
			{
				mappedPublicKey = assemblyName2.GetPublicKey();
				if (mappedPublicKey == null || mappedPublicKey.Length < 12)
				{
					return false;
				}
			}
			if (!StrongNameManager.MustVerify(assemblyName2))
			{
				return true;
			}
			return new StrongName(CryptoConvert.FromCapiPublicKeyBlob(mappedPublicKey, 12)).Verify(assemblyName);
		}
		catch
		{
			return false;
		}
	}

	public static bool VerifySignature(byte[] publicKey, int algorithm, byte[] hash, byte[] signature)
	{
		try
		{
			return Verify(CryptoConvert.FromCapiPublicKeyBlob(publicKey), (AssemblyHashAlgorithm)algorithm, hash, signature);
		}
		catch
		{
			return false;
		}
	}

	private static bool Verify(RSA rsa, AssemblyHashAlgorithm algorithm, byte[] hash, byte[] signature)
	{
		RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
		switch (algorithm)
		{
		case AssemblyHashAlgorithm.MD5:
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			break;
		default:
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
			break;
		}
		return rSAPKCS1SignatureDeformatter.VerifySignature(hash, signature);
	}
}
