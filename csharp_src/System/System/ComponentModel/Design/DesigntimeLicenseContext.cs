using System.Collections;
using System.Reflection;

namespace System.ComponentModel.Design;

public class DesigntimeLicenseContext : LicenseContext
{
	internal Hashtable savedLicenseKeys = new Hashtable();

	public override LicenseUsageMode UsageMode => LicenseUsageMode.Designtime;

	public override string GetSavedLicenseKey(Type type, Assembly resourceAssembly)
	{
		return null;
	}

	public override void SetSavedLicenseKey(Type type, string key)
	{
		savedLicenseKeys[type.AssemblyQualifiedName] = key;
	}
}
