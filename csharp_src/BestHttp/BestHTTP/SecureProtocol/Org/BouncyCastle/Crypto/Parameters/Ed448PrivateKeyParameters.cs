using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

public sealed class Ed448PrivateKeyParameters : AsymmetricKeyParameter
{
	public static readonly int KeySize = Ed448.SecretKeySize;

	public static readonly int SignatureSize = Ed448.SignatureSize;

	private readonly byte[] data = new byte[KeySize];

	public Ed448PrivateKeyParameters(SecureRandom random)
		: base(privateKey: true)
	{
		Ed448.GeneratePrivateKey(random, data);
	}

	public Ed448PrivateKeyParameters(byte[] buf, int off)
		: base(privateKey: true)
	{
		Array.Copy(buf, off, data, 0, KeySize);
	}

	public Ed448PrivateKeyParameters(Stream input)
		: base(privateKey: true)
	{
		if (KeySize != Streams.ReadFully(input, data))
		{
			throw new EndOfStreamException("EOF encountered in middle of Ed448 private key");
		}
	}

	public void Encode(byte[] buf, int off)
	{
		Array.Copy(data, 0, buf, off, KeySize);
	}

	public byte[] GetEncoded()
	{
		return Arrays.Clone(data);
	}

	public Ed448PublicKeyParameters GeneratePublicKey()
	{
		byte[] array = new byte[Ed448.PublicKeySize];
		Ed448.GeneratePublicKey(data, 0, array, 0);
		return new Ed448PublicKeyParameters(array, 0);
	}

	public void Sign(Ed448.Algorithm algorithm, Ed448PublicKeyParameters publicKey, byte[] ctx, byte[] msg, int msgOff, int msgLen, byte[] sig, int sigOff)
	{
		byte[] array = new byte[Ed448.PublicKeySize];
		if (publicKey == null)
		{
			Ed448.GeneratePublicKey(data, 0, array, 0);
		}
		else
		{
			publicKey.Encode(array, 0);
		}
		switch (algorithm)
		{
		case Ed448.Algorithm.Ed448:
			Ed448.Sign(data, 0, array, 0, ctx, msg, msgOff, msgLen, sig, sigOff);
			break;
		case Ed448.Algorithm.Ed448ph:
			if (Ed448.PrehashSize != msgLen)
			{
				throw new ArgumentException("msgLen");
			}
			Ed448.SignPrehash(data, 0, array, 0, ctx, msg, msgOff, sig, sigOff);
			break;
		default:
			throw new ArgumentException("algorithm");
		}
	}
}
