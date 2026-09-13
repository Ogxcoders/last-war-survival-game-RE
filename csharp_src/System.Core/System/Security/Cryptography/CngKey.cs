using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography;

[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
public sealed class CngKey : IDisposable
{
	public CngAlgorithmGroup AlgorithmGroup
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public CngAlgorithm Algorithm
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public CngExportPolicies ExportPolicy
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public SafeNCryptKeyHandle Handle
	{
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		get
		{
			throw new NotImplementedException();
		}
	}

	public bool IsEphemeral
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
		[SecurityCritical]
		private set
		{
			throw new NotImplementedException();
		}
	}

	public bool IsMachineKey
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public string KeyName
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public int KeySize
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public CngKeyUsages KeyUsage
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public IntPtr ParentWindowHandle
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		set
		{
			throw new NotImplementedException();
		}
	}

	public CngProvider Provider
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public SafeNCryptProviderHandle ProviderHandle
	{
		[SecurityCritical]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		get
		{
			throw new NotImplementedException();
		}
	}

	public string UniqueName
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public CngUIPolicy UIPolicy
	{
		[SecuritySafeCritical]
		get
		{
			throw new NotImplementedException();
		}
	}

	public static CngKey Create(CngAlgorithm algorithm)
	{
		throw new NotImplementedException();
	}

	public static CngKey Create(CngAlgorithm algorithm, string keyName)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public static CngKey Create(CngAlgorithm algorithm, string keyName, CngKeyCreationParameters creationParameters)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public void Delete()
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public void Dispose()
	{
		throw new NotImplementedException();
	}

	public static bool Exists(string keyName)
	{
		throw new NotImplementedException();
	}

	public static bool Exists(string keyName, CngProvider provider)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public static bool Exists(string keyName, CngProvider provider, CngKeyOpenOptions options)
	{
		throw new NotImplementedException();
	}

	public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public static CngKey Import(byte[] keyBlob, CngKeyBlobFormat format, CngProvider provider)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public byte[] Export(CngKeyBlobFormat format)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public CngProperty GetProperty(string name, CngPropertyOptions options)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public bool HasProperty(string name, CngPropertyOptions options)
	{
		throw new NotImplementedException();
	}

	public static CngKey Open(string keyName)
	{
		throw new NotImplementedException();
	}

	public static CngKey Open(string keyName, CngProvider provider)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public static CngKey Open(string keyName, CngProvider provider, CngKeyOpenOptions openOptions)
	{
		throw new NotImplementedException();
	}

	[SecurityCritical]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public static CngKey Open(SafeNCryptKeyHandle keyHandle, CngKeyHandleOpenOptions keyHandleOpenOptions)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	public void SetProperty(CngProperty property)
	{
		throw new NotImplementedException();
	}
}
