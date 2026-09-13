using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Security.Cryptography;

[ComVisible(true)]
public sealed class CspParameters
{
	public int ProviderType;

	public string ProviderName;

	public string KeyContainerName;

	public int KeyNumber;

	private int m_flags;

	private CryptoKeySecurity m_cryptoKeySecurity;

	private SecureString m_keyPassword;

	private IntPtr m_parentWindowHandle;

	public CspProviderFlags Flags
	{
		get
		{
			return (CspProviderFlags)m_flags;
		}
		set
		{
			int num = 255;
			if (((uint)value & (uint)(~num)) != 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Illegal enum value: {0}.", (int)value), "value");
			}
			m_flags = (int)value;
		}
	}

	public CryptoKeySecurity CryptoKeySecurity
	{
		get
		{
			return m_cryptoKeySecurity;
		}
		set
		{
			m_cryptoKeySecurity = value;
		}
	}

	public SecureString KeyPassword
	{
		get
		{
			return m_keyPassword;
		}
		set
		{
			m_keyPassword = value;
			m_parentWindowHandle = IntPtr.Zero;
		}
	}

	public IntPtr ParentWindowHandle
	{
		get
		{
			return m_parentWindowHandle;
		}
		set
		{
			m_parentWindowHandle = value;
			m_keyPassword = null;
		}
	}

	public CspParameters()
		: this(1, null, null)
	{
	}

	public CspParameters(int dwTypeIn)
		: this(dwTypeIn, null, null)
	{
	}

	public CspParameters(int dwTypeIn, string strProviderNameIn)
		: this(dwTypeIn, strProviderNameIn, null)
	{
	}

	public CspParameters(int dwTypeIn, string strProviderNameIn, string strContainerNameIn)
		: this(dwTypeIn, strProviderNameIn, strContainerNameIn, CspProviderFlags.NoFlags)
	{
	}

	public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, SecureString keyPassword)
		: this(providerType, providerName, keyContainerName)
	{
		m_cryptoKeySecurity = cryptoKeySecurity;
		m_keyPassword = keyPassword;
	}

	public CspParameters(int providerType, string providerName, string keyContainerName, CryptoKeySecurity cryptoKeySecurity, IntPtr parentWindowHandle)
		: this(providerType, providerName, keyContainerName)
	{
		m_cryptoKeySecurity = cryptoKeySecurity;
		m_parentWindowHandle = parentWindowHandle;
	}

	internal CspParameters(int providerType, string providerName, string keyContainerName, CspProviderFlags flags)
	{
		ProviderType = providerType;
		ProviderName = providerName;
		KeyContainerName = keyContainerName;
		KeyNumber = -1;
		Flags = flags;
	}

	internal CspParameters(CspParameters parameters)
	{
		ProviderType = parameters.ProviderType;
		ProviderName = parameters.ProviderName;
		KeyContainerName = parameters.KeyContainerName;
		KeyNumber = parameters.KeyNumber;
		Flags = parameters.Flags;
		m_cryptoKeySecurity = parameters.m_cryptoKeySecurity;
		m_keyPassword = parameters.m_keyPassword;
		m_parentWindowHandle = parameters.m_parentWindowHandle;
	}
}
