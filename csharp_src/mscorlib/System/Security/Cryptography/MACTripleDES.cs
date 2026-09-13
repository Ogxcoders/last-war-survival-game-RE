using System.Runtime.InteropServices;

namespace System.Security.Cryptography;

[ComVisible(true)]
public class MACTripleDES : KeyedHashAlgorithm
{
	private ICryptoTransform m_encryptor;

	private CryptoStream _cs;

	private TailStream _ts;

	private const int m_bitsPerByte = 8;

	private int m_bytesPerBlock;

	private TripleDES des;

	[ComVisible(false)]
	public PaddingMode Padding
	{
		get
		{
			return des.Padding;
		}
		set
		{
			if (value < PaddingMode.None || PaddingMode.ISO10126 < value)
			{
				throw new CryptographicException(Environment.GetResourceString("Specified padding mode is not valid for this algorithm."));
			}
			des.Padding = value;
		}
	}

	public MACTripleDES()
	{
		KeyValue = new byte[24];
		Utils.StaticRandomNumberGenerator.GetBytes(KeyValue);
		des = TripleDES.Create();
		HashSizeValue = des.BlockSize;
		m_bytesPerBlock = des.BlockSize / 8;
		des.IV = new byte[m_bytesPerBlock];
		des.Padding = PaddingMode.Zeros;
		m_encryptor = null;
	}

	public MACTripleDES(byte[] rgbKey)
		: this("System.Security.Cryptography.TripleDES", rgbKey)
	{
	}

	public MACTripleDES(string strTripleDES, byte[] rgbKey)
	{
		if (rgbKey == null)
		{
			throw new ArgumentNullException("rgbKey");
		}
		if (strTripleDES == null)
		{
			des = TripleDES.Create();
		}
		else
		{
			des = TripleDES.Create(strTripleDES);
		}
		HashSizeValue = des.BlockSize;
		KeyValue = (byte[])rgbKey.Clone();
		m_bytesPerBlock = des.BlockSize / 8;
		des.IV = new byte[m_bytesPerBlock];
		des.Padding = PaddingMode.Zeros;
		m_encryptor = null;
	}

	public override void Initialize()
	{
		m_encryptor = null;
	}

	protected override void HashCore(byte[] rgbData, int ibStart, int cbSize)
	{
		if (m_encryptor == null)
		{
			des.Key = Key;
			m_encryptor = des.CreateEncryptor();
			_ts = new TailStream(des.BlockSize / 8);
			_cs = new CryptoStream(_ts, m_encryptor, CryptoStreamMode.Write);
		}
		_cs.Write(rgbData, ibStart, cbSize);
	}

	protected override byte[] HashFinal()
	{
		if (m_encryptor == null)
		{
			des.Key = Key;
			m_encryptor = des.CreateEncryptor();
			_ts = new TailStream(des.BlockSize / 8);
			_cs = new CryptoStream(_ts, m_encryptor, CryptoStreamMode.Write);
		}
		_cs.FlushFinalBlock();
		return _ts.Buffer;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (des != null)
			{
				des.Clear();
			}
			if (m_encryptor != null)
			{
				m_encryptor.Dispose();
			}
			if (_cs != null)
			{
				_cs.Clear();
			}
			if (_ts != null)
			{
				_ts.Clear();
			}
		}
		base.Dispose(disposing);
	}
}
