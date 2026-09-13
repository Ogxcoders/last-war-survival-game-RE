using System.Collections.Generic;

namespace System.Data.Common;

public static class DbProviderFactories
{
	public static DbProviderFactory GetFactory(DbConnection connection)
	{
		throw new PlatformNotSupportedException();
	}

	public static DbProviderFactory GetFactory(DataRow providerRow)
	{
		throw new PlatformNotSupportedException();
	}

	public static DbProviderFactory GetFactory(string providerInvariantName)
	{
		throw new PlatformNotSupportedException();
	}

	public static DataTable GetFactoryClasses()
	{
		throw new PlatformNotSupportedException();
	}

	public static IEnumerable<string> GetProviderInvariantNames()
	{
		throw new PlatformNotSupportedException();
	}

	public static void RegisterFactory(string providerInvariantName, DbProviderFactory factory)
	{
		throw new PlatformNotSupportedException();
	}

	public static void RegisterFactory(string providerInvariantName, string factoryTypeAssemblyQualifiedName)
	{
		throw new PlatformNotSupportedException();
	}

	public static void RegisterFactory(string providerInvariantName, Type providerFactoryClass)
	{
		throw new PlatformNotSupportedException();
	}

	public static bool TryGetFactory(string providerInvariantName, out DbProviderFactory factory)
	{
		throw new PlatformNotSupportedException();
	}

	public static bool UnregisterFactory(string providerInvariantName)
	{
		throw new PlatformNotSupportedException();
	}
}
