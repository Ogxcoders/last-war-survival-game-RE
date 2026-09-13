using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace System.Xml;

internal class SecureStringHasher : IEqualityComparer<string>
{
	[SecurityCritical]
	private delegate int HashCodeOfStringDelegate(string s, int sLen, long additionalEntropy);

	[SecurityCritical]
	private static HashCodeOfStringDelegate hashCodeDelegate;

	private int hashCodeRandomizer;

	public SecureStringHasher()
	{
		hashCodeRandomizer = Environment.TickCount;
	}

	public bool Equals(string x, string y)
	{
		return string.Equals(x, y, StringComparison.Ordinal);
	}

	[SecuritySafeCritical]
	public int GetHashCode(string key)
	{
		if (hashCodeDelegate == null)
		{
			hashCodeDelegate = GetHashCodeDelegate();
		}
		return hashCodeDelegate(key, key.Length, hashCodeRandomizer);
	}

	[SecurityCritical]
	private static int GetHashCodeOfString(string key, int sLen, long additionalEntropy)
	{
		int num = (int)additionalEntropy;
		for (int i = 0; i < key.Length; i++)
		{
			num += (num << 7) ^ key[i];
		}
		num -= num >> 17;
		num -= num >> 11;
		return num - (num >> 5);
	}

	[SecuritySafeCritical]
	[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
	private static HashCodeOfStringDelegate GetHashCodeDelegate()
	{
		MethodInfo method = typeof(string).GetMethod("InternalMarvin32HashString", BindingFlags.Static | BindingFlags.NonPublic);
		if (method != null)
		{
			return (HashCodeOfStringDelegate)Delegate.CreateDelegate(typeof(HashCodeOfStringDelegate), method);
		}
		return GetHashCodeOfString;
	}
}
