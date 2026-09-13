using System;
using System.Collections;
using System.IO;

namespace Mono.Security.X509;

public sealed class X509StoreManager
{
	private static string _userPath;

	private static string _localMachinePath;

	private static string _newUserPath;

	private static string _newLocalMachinePath;

	private static X509Stores _userStore;

	private static X509Stores _machineStore;

	private static X509Stores _newUserStore;

	private static X509Stores _newMachineStore;

	internal static string CurrentUserPath
	{
		get
		{
			if (_userPath == null)
			{
				_userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono");
				_userPath = Path.Combine(_userPath, "certs");
			}
			return _userPath;
		}
	}

	internal static string LocalMachinePath
	{
		get
		{
			if (_localMachinePath == null)
			{
				_localMachinePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ".mono");
				_localMachinePath = Path.Combine(_localMachinePath, "certs");
			}
			return _localMachinePath;
		}
	}

	internal static string NewCurrentUserPath
	{
		get
		{
			if (_newUserPath == null)
			{
				_newUserPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mono");
				_newUserPath = Path.Combine(_newUserPath, "new-certs");
			}
			return _newUserPath;
		}
	}

	internal static string NewLocalMachinePath
	{
		get
		{
			if (_newLocalMachinePath == null)
			{
				_newLocalMachinePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), ".mono");
				_newLocalMachinePath = Path.Combine(_newLocalMachinePath, "new-certs");
			}
			return _newLocalMachinePath;
		}
	}

	public static X509Stores CurrentUser
	{
		get
		{
			if (_userStore == null)
			{
				_userStore = new X509Stores(CurrentUserPath, newFormat: false);
			}
			return _userStore;
		}
	}

	public static X509Stores LocalMachine
	{
		get
		{
			if (_machineStore == null)
			{
				_machineStore = new X509Stores(LocalMachinePath, newFormat: false);
			}
			return _machineStore;
		}
	}

	public static X509Stores NewCurrentUser
	{
		get
		{
			if (_newUserStore == null)
			{
				_newUserStore = new X509Stores(NewCurrentUserPath, newFormat: true);
			}
			return _newUserStore;
		}
	}

	public static X509Stores NewLocalMachine
	{
		get
		{
			if (_newMachineStore == null)
			{
				_newMachineStore = new X509Stores(NewLocalMachinePath, newFormat: true);
			}
			return _newMachineStore;
		}
	}

	public static X509CertificateCollection IntermediateCACertificates
	{
		get
		{
			X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
			x509CertificateCollection.AddRange(CurrentUser.IntermediateCA.Certificates);
			x509CertificateCollection.AddRange(LocalMachine.IntermediateCA.Certificates);
			return x509CertificateCollection;
		}
	}

	public static ArrayList IntermediateCACrls
	{
		get
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(CurrentUser.IntermediateCA.Crls);
			arrayList.AddRange(LocalMachine.IntermediateCA.Crls);
			return arrayList;
		}
	}

	public static X509CertificateCollection TrustedRootCertificates
	{
		get
		{
			X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
			x509CertificateCollection.AddRange(CurrentUser.TrustedRoot.Certificates);
			x509CertificateCollection.AddRange(LocalMachine.TrustedRoot.Certificates);
			return x509CertificateCollection;
		}
	}

	public static ArrayList TrustedRootCACrls
	{
		get
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(CurrentUser.TrustedRoot.Crls);
			arrayList.AddRange(LocalMachine.TrustedRoot.Crls);
			return arrayList;
		}
	}

	public static X509CertificateCollection UntrustedCertificates
	{
		get
		{
			X509CertificateCollection x509CertificateCollection = new X509CertificateCollection();
			x509CertificateCollection.AddRange(CurrentUser.Untrusted.Certificates);
			x509CertificateCollection.AddRange(LocalMachine.Untrusted.Certificates);
			return x509CertificateCollection;
		}
	}

	private X509StoreManager()
	{
	}
}
