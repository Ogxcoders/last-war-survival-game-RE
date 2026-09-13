using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

public class Ed25519Signer : ISigner
{
	private class Buffer : MemoryStream
	{
		internal byte[] GenerateSignature(Ed25519PrivateKeyParameters privateKey, Ed25519PublicKeyParameters publicKey)
		{
			lock (this)
			{
				byte[] buffer = GetBuffer();
				int msgLen = (int)Position;
				byte[] array = new byte[Ed25519PrivateKeyParameters.SignatureSize];
				privateKey.Sign(Ed25519.Algorithm.Ed25519, publicKey, null, buffer, 0, msgLen, array, 0);
				Reset();
				return array;
			}
		}

		internal bool VerifySignature(Ed25519PublicKeyParameters publicKey, byte[] signature)
		{
			lock (this)
			{
				byte[] buffer = GetBuffer();
				int mLen = (int)Position;
				byte[] encoded = publicKey.GetEncoded();
				bool result = Ed25519.Verify(signature, 0, encoded, 0, buffer, 0, mLen);
				Reset();
				return result;
			}
		}

		internal void Reset()
		{
			lock (this)
			{
				long position = Position;
				Array.Clear(GetBuffer(), 0, (int)position);
				Position = 0L;
			}
		}
	}

	private readonly Buffer buffer = new Buffer();

	private bool forSigning;

	private Ed25519PrivateKeyParameters privateKey;

	private Ed25519PublicKeyParameters publicKey;

	public virtual string AlgorithmName => "Ed25519";

	public virtual void Init(bool forSigning, ICipherParameters parameters)
	{
		this.forSigning = forSigning;
		if (forSigning)
		{
			privateKey = (Ed25519PrivateKeyParameters)parameters;
			publicKey = privateKey.GeneratePublicKey();
		}
		else
		{
			privateKey = null;
			publicKey = (Ed25519PublicKeyParameters)parameters;
		}
		Reset();
	}

	public virtual void Update(byte b)
	{
		buffer.WriteByte(b);
	}

	public virtual void BlockUpdate(byte[] buf, int off, int len)
	{
		buffer.Write(buf, off, len);
	}

	public virtual byte[] GenerateSignature()
	{
		if (!forSigning || privateKey == null)
		{
			throw new InvalidOperationException("Ed25519Signer not initialised for signature generation.");
		}
		return buffer.GenerateSignature(privateKey, publicKey);
	}

	public virtual bool VerifySignature(byte[] signature)
	{
		if (forSigning || publicKey == null)
		{
			throw new InvalidOperationException("Ed25519Signer not initialised for verification");
		}
		return buffer.VerifySignature(publicKey, signature);
	}

	public virtual void Reset()
	{
		buffer.Reset();
	}
}
