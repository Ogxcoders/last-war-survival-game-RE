using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy;

[ComVisible(true)]
public static class ApplicationSecurityManager
{
	private static IApplicationTrustManager _appTrustManager;

	private static ApplicationTrustCollection _userAppTrusts;

	public static IApplicationTrustManager ApplicationTrustManager
	{
		[SecurityPermission(SecurityAction.Demand, ControlPolicy = true)]
		get
		{
			if (_appTrustManager == null)
			{
				_appTrustManager = new MonoTrustManager();
			}
			return _appTrustManager;
		}
	}

	public static ApplicationTrustCollection UserApplicationTrusts
	{
		get
		{
			if (_userAppTrusts == null)
			{
				_userAppTrusts = new ApplicationTrustCollection();
			}
			return _userAppTrusts;
		}
	}

	[MonoTODO("Missing application manifest support")]
	[SecurityPermission(SecurityAction.Demand, ControlPolicy = true, ControlEvidence = true)]
	public static bool DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context)
	{
		if (activationContext == null)
		{
			throw new NullReferenceException("activationContext");
		}
		return ApplicationTrustManager.DetermineApplicationTrust(activationContext, context).IsApplicationTrustedToRun;
	}
}
