using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;

public class UrlAndHash
{
	protected readonly string mUrl;

	protected readonly byte[] mSha1Hash;

	public virtual string Url => mUrl;

	public virtual byte[] Sha1Hash => mSha1Hash;

	public UrlAndHash(string url, byte[] sha1Hash)
	{
		if (url == null || url.Length < 1 || url.Length >= 65536)
		{
			throw new ArgumentException("must have length from 1 to (2^16 - 1)", "url");
		}
		if (sha1Hash != null && sha1Hash.Length != 20)
		{
			throw new ArgumentException("must have length == 20, if present", "sha1Hash");
		}
		mUrl = url;
		mSha1Hash = sha1Hash;
	}

	public virtual void Encode(Stream output)
	{
		TlsUtilities.WriteOpaque16(Strings.ToByteArray(mUrl), output);
		if (mSha1Hash == null)
		{
			TlsUtilities.WriteUint8(0, output);
			return;
		}
		TlsUtilities.WriteUint8(1, output);
		output.Write(mSha1Hash, 0, mSha1Hash.Length);
	}

	public static UrlAndHash Parse(TlsContext context, Stream input)
	{
		byte[] array = TlsUtilities.ReadOpaque16(input);
		if (array.Length < 1)
		{
			throw new TlsFatalAlert(47);
		}
		string url = Strings.FromByteArray(array);
		byte[] sha1Hash = null;
		switch (TlsUtilities.ReadUint8(input))
		{
		case 0:
			if (TlsUtilities.IsTlsV12(context))
			{
				throw new TlsFatalAlert(47);
			}
			break;
		case 1:
			sha1Hash = TlsUtilities.ReadFully(20, input);
			break;
		default:
			throw new TlsFatalAlert(47);
		}
		return new UrlAndHash(url, sha1Hash);
	}
}
