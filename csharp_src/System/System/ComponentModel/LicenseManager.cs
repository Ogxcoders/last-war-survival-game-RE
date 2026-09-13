using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;

namespace System.ComponentModel;

public sealed class LicenseManager
{
	private static readonly object s_selfLock = new object();

	private static volatile LicenseContext s_context;

	private static object s_contextLockHolder;

	private static volatile Hashtable s_providers;

	private static volatile Hashtable s_providerInstances;

	private static readonly object s_internalSyncObject = new object();

	public static LicenseContext CurrentContext
	{
		get
		{
			if (s_context == null)
			{
				lock (s_internalSyncObject)
				{
					if (s_context == null)
					{
						s_context = new RuntimeLicenseContext();
					}
				}
			}
			return s_context;
		}
		set
		{
			lock (s_internalSyncObject)
			{
				if (s_contextLockHolder != null)
				{
					throw new InvalidOperationException("The CurrentContext property of the LicenseManager is currently locked and cannot be changed.");
				}
				s_context = value;
			}
		}
	}

	public static LicenseUsageMode UsageMode
	{
		get
		{
			if (s_context != null)
			{
				return s_context.UsageMode;
			}
			return LicenseUsageMode.Runtime;
		}
	}

	private LicenseManager()
	{
	}

	private static void CacheProvider(Type type, LicenseProvider provider)
	{
		if (s_providers == null)
		{
			s_providers = new Hashtable();
		}
		s_providers[type] = provider;
		if (provider != null)
		{
			if (s_providerInstances == null)
			{
				s_providerInstances = new Hashtable();
			}
			s_providerInstances[provider.GetType()] = provider;
		}
	}

	public static object CreateWithContext(Type type, LicenseContext creationContext)
	{
		return CreateWithContext(type, creationContext, Array.Empty<object>());
	}

	public static object CreateWithContext(Type type, LicenseContext creationContext, object[] args)
	{
		object obj = null;
		lock (s_internalSyncObject)
		{
			LicenseContext currentContext = CurrentContext;
			try
			{
				CurrentContext = creationContext;
				LockContext(s_selfLock);
				try
				{
					return SecurityUtils.SecureCreateInstance(type, args);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
			finally
			{
				UnlockContext(s_selfLock);
				CurrentContext = currentContext;
			}
		}
	}

	private static bool GetCachedNoLicenseProvider(Type type)
	{
		if (s_providers != null)
		{
			return s_providers.ContainsKey(type);
		}
		return false;
	}

	private static LicenseProvider GetCachedProvider(Type type)
	{
		return (LicenseProvider)(s_providers?[type]);
	}

	private static LicenseProvider GetCachedProviderInstance(Type providerType)
	{
		return (LicenseProvider)(s_providerInstances?[providerType]);
	}

	public static bool IsLicensed(Type type)
	{
		License license;
		bool result = ValidateInternal(type, null, allowExceptions: false, out license);
		if (license != null)
		{
			license.Dispose();
			license = null;
		}
		return result;
	}

	public static bool IsValid(Type type)
	{
		License license;
		bool result = ValidateInternal(type, null, allowExceptions: false, out license);
		if (license != null)
		{
			license.Dispose();
			license = null;
		}
		return result;
	}

	public static bool IsValid(Type type, object instance, out License license)
	{
		return ValidateInternal(type, instance, allowExceptions: false, out license);
	}

	public static void LockContext(object contextUser)
	{
		lock (s_internalSyncObject)
		{
			if (s_contextLockHolder != null)
			{
				throw new InvalidOperationException("The CurrentContext property of the LicenseManager is already locked by another user.");
			}
			s_contextLockHolder = contextUser;
		}
	}

	public static void UnlockContext(object contextUser)
	{
		lock (s_internalSyncObject)
		{
			if (s_contextLockHolder != contextUser)
			{
				throw new ArgumentException("The CurrentContext property of the LicenseManager can only be unlocked with the same contextUser.");
			}
			s_contextLockHolder = null;
		}
	}

	private static bool ValidateInternal(Type type, object instance, bool allowExceptions, out License license)
	{
		string licenseKey;
		return ValidateInternalRecursive(CurrentContext, type, instance, allowExceptions, out license, out licenseKey);
	}

	private static bool ValidateInternalRecursive(LicenseContext context, Type type, object instance, bool allowExceptions, out License license, out string licenseKey)
	{
		LicenseProvider licenseProvider = GetCachedProvider(type);
		if (licenseProvider == null && !GetCachedNoLicenseProvider(type))
		{
			LicenseProviderAttribute licenseProviderAttribute = (LicenseProviderAttribute)Attribute.GetCustomAttribute(type, typeof(LicenseProviderAttribute), inherit: false);
			if (licenseProviderAttribute != null)
			{
				Type licenseProvider2 = licenseProviderAttribute.LicenseProvider;
				licenseProvider = GetCachedProviderInstance(licenseProvider2) ?? ((LicenseProvider)SecurityUtils.SecureCreateInstance(licenseProvider2));
			}
			CacheProvider(type, licenseProvider);
		}
		license = null;
		bool flag = true;
		licenseKey = null;
		if (licenseProvider != null)
		{
			license = licenseProvider.GetLicense(context, type, instance, allowExceptions);
			if (license == null)
			{
				flag = false;
			}
			else
			{
				licenseKey = license.LicenseKey;
			}
		}
		if (flag && instance == null)
		{
			Type baseType = type.BaseType;
			if (baseType != typeof(object) && baseType != null)
			{
				if (license != null)
				{
					license.Dispose();
					license = null;
				}
				flag = ValidateInternalRecursive(context, baseType, null, allowExceptions, out license, out var _);
				if (license != null)
				{
					license.Dispose();
					license = null;
				}
			}
		}
		return flag;
	}

	public static void Validate(Type type)
	{
		if (!ValidateInternal(type, null, allowExceptions: true, out var license))
		{
			throw new LicenseException(type);
		}
		if (license != null)
		{
			license.Dispose();
			license = null;
		}
	}

	public static License Validate(Type type, object instance)
	{
		if (!ValidateInternal(type, instance, allowExceptions: true, out var license))
		{
			throw new LicenseException(type, instance);
		}
		return license;
	}
}
