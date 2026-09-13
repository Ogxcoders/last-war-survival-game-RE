using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace Mono.Security.Interface;

public sealed class MonoTlsSettings
{
	private bool cloned;

	private bool checkCertName = true;

	private bool checkCertRevocationStatus;

	private bool? useServicePointManagerCallback;

	private bool skipSystemValidators;

	private bool callbackNeedsChain = true;

	private ICertificateValidator certificateValidator;

	private static MonoTlsSettings defaultSettings;

	public MonoRemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

	public MonoLocalCertificateSelectionCallback ClientCertificateSelectionCallback { get; set; }

	public bool CheckCertificateName
	{
		get
		{
			return checkCertName;
		}
		set
		{
			checkCertName = value;
		}
	}

	public bool CheckCertificateRevocationStatus
	{
		get
		{
			return checkCertRevocationStatus;
		}
		set
		{
			checkCertRevocationStatus = value;
		}
	}

	public bool? UseServicePointManagerCallback
	{
		get
		{
			return useServicePointManagerCallback;
		}
		set
		{
			useServicePointManagerCallback = value;
		}
	}

	public bool SkipSystemValidators
	{
		get
		{
			return skipSystemValidators;
		}
		set
		{
			skipSystemValidators = value;
		}
	}

	public bool CallbackNeedsCertificateChain
	{
		get
		{
			return callbackNeedsChain;
		}
		set
		{
			callbackNeedsChain = value;
		}
	}

	public DateTime? CertificateValidationTime { get; set; }

	public X509CertificateCollection TrustAnchors { get; set; }

	public object UserSettings { get; set; }

	internal string[] CertificateSearchPaths { get; set; }

	internal bool SendCloseNotify { get; set; }

	public string[] ClientCertificateIssuers { get; set; }

	public bool DisallowUnauthenticatedCertificateRequest { get; set; }

	public TlsProtocols? EnabledProtocols { get; set; }

	[CLSCompliant(false)]
	public CipherSuiteCode[] EnabledCiphers { get; set; }

	public static MonoTlsSettings DefaultSettings
	{
		get
		{
			if (defaultSettings == null)
			{
				Interlocked.CompareExchange(ref defaultSettings, new MonoTlsSettings(), null);
			}
			return defaultSettings;
		}
		set
		{
			defaultSettings = value ?? new MonoTlsSettings();
		}
	}

	[Obsolete("Do not use outside System.dll!")]
	public ICertificateValidator CertificateValidator => certificateValidator;

	public MonoTlsSettings()
	{
	}

	public static MonoTlsSettings CopyDefaultSettings()
	{
		return DefaultSettings.Clone();
	}

	[Obsolete("Do not use outside System.dll!")]
	public MonoTlsSettings CloneWithValidator(ICertificateValidator validator)
	{
		if (cloned)
		{
			certificateValidator = validator;
			return this;
		}
		return new MonoTlsSettings(this)
		{
			certificateValidator = validator
		};
	}

	public MonoTlsSettings Clone()
	{
		return new MonoTlsSettings(this);
	}

	private MonoTlsSettings(MonoTlsSettings other)
	{
		RemoteCertificateValidationCallback = other.RemoteCertificateValidationCallback;
		ClientCertificateSelectionCallback = other.ClientCertificateSelectionCallback;
		checkCertName = other.checkCertName;
		checkCertRevocationStatus = other.checkCertRevocationStatus;
		UseServicePointManagerCallback = other.useServicePointManagerCallback;
		skipSystemValidators = other.skipSystemValidators;
		callbackNeedsChain = other.callbackNeedsChain;
		UserSettings = other.UserSettings;
		EnabledProtocols = other.EnabledProtocols;
		EnabledCiphers = other.EnabledCiphers;
		CertificateValidationTime = other.CertificateValidationTime;
		SendCloseNotify = other.SendCloseNotify;
		ClientCertificateIssuers = other.ClientCertificateIssuers;
		DisallowUnauthenticatedCertificateRequest = other.DisallowUnauthenticatedCertificateRequest;
		if (other.TrustAnchors != null)
		{
			TrustAnchors = new X509CertificateCollection(other.TrustAnchors);
		}
		if (other.CertificateSearchPaths != null)
		{
			CertificateSearchPaths = new string[other.CertificateSearchPaths.Length];
			other.CertificateSearchPaths.CopyTo(CertificateSearchPaths, 0);
		}
		cloned = true;
	}
}
