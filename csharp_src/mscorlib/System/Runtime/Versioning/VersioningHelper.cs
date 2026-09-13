using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using Microsoft.Win32;

namespace System.Runtime.Versioning;

public static class VersioningHelper
{
	private const ResourceScope ResTypeMask = ResourceScope.Machine | ResourceScope.Process | ResourceScope.AppDomain | ResourceScope.Library;

	private const ResourceScope VisibilityMask = ResourceScope.Private | ResourceScope.Assembly;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SecuritySafeCritical]
	private static extern int GetRuntimeId();

	public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to)
	{
		return MakeVersionSafeName(name, from, to, null);
	}

	[SecuritySafeCritical]
	public static string MakeVersionSafeName(string name, ResourceScope from, ResourceScope to, Type type)
	{
		ResourceScope resourceScope = from & (ResourceScope.Machine | ResourceScope.Process | ResourceScope.AppDomain | ResourceScope.Library);
		ResourceScope resourceScope2 = to & (ResourceScope.Machine | ResourceScope.Process | ResourceScope.AppDomain | ResourceScope.Library);
		if (resourceScope > resourceScope2)
		{
			throw new ArgumentException(Environment.GetResourceString("Resource type in the ResourceScope enum is going from a more restrictive resource type to a more general one.  From: \"{0}\"  To: \"{1}\"", resourceScope, resourceScope2), "from");
		}
		SxSRequirements requirements = GetRequirements(to, from);
		if ((requirements & (SxSRequirements.AssemblyName | SxSRequirements.TypeName)) != SxSRequirements.None && type == null)
		{
			throw new ArgumentNullException("type", Environment.GetResourceString("The type parameter cannot be null when scoping the resource's visibility to Private or Assembly."));
		}
		StringBuilder stringBuilder = new StringBuilder(name);
		char value = '_';
		if ((requirements & SxSRequirements.ProcessID) != SxSRequirements.None)
		{
			stringBuilder.Append(value);
			stringBuilder.Append('p');
			stringBuilder.Append(NativeMethods.GetCurrentProcessId());
		}
		if ((requirements & SxSRequirements.CLRInstanceID) != SxSRequirements.None)
		{
			string cLRInstanceString = GetCLRInstanceString();
			stringBuilder.Append(value);
			stringBuilder.Append('r');
			stringBuilder.Append(cLRInstanceString);
		}
		if ((requirements & SxSRequirements.AppDomainID) != SxSRequirements.None)
		{
			stringBuilder.Append(value);
			stringBuilder.Append("ad");
			stringBuilder.Append(AppDomain.CurrentDomain.Id);
		}
		if ((requirements & SxSRequirements.TypeName) != SxSRequirements.None)
		{
			stringBuilder.Append(value);
			stringBuilder.Append(type.Name);
		}
		if ((requirements & SxSRequirements.AssemblyName) != SxSRequirements.None)
		{
			stringBuilder.Append(value);
			stringBuilder.Append(type.Assembly.FullName);
		}
		return stringBuilder.ToString();
	}

	private static string GetCLRInstanceString()
	{
		return GetRuntimeId().ToString(CultureInfo.InvariantCulture);
	}

	private static SxSRequirements GetRequirements(ResourceScope consumeAsScope, ResourceScope calleeScope)
	{
		SxSRequirements sxSRequirements = SxSRequirements.None;
		switch (calleeScope & (ResourceScope.Machine | ResourceScope.Process | ResourceScope.AppDomain | ResourceScope.Library))
		{
		case ResourceScope.Machine:
			switch (consumeAsScope & (ResourceScope.Machine | ResourceScope.Process | ResourceScope.AppDomain | ResourceScope.Library))
			{
			case ResourceScope.Process:
				sxSRequirements |= SxSRequirements.ProcessID;
				break;
			case ResourceScope.AppDomain:
				sxSRequirements |= SxSRequirements.AppDomainID | SxSRequirements.ProcessID | SxSRequirements.CLRInstanceID;
				break;
			default:
				throw new ArgumentException(Environment.GetResourceString("Unknown value for the ResourceScope: {0}  Too many resource type bits may be set.", consumeAsScope), "consumeAsScope");
			case ResourceScope.Machine:
				break;
			}
			break;
		case ResourceScope.Process:
			if ((consumeAsScope & ResourceScope.AppDomain) != ResourceScope.None)
			{
				sxSRequirements |= SxSRequirements.AppDomainID | SxSRequirements.CLRInstanceID;
			}
			break;
		default:
			throw new ArgumentException(Environment.GetResourceString("Unknown value for the ResourceScope: {0}  Too many resource type bits may be set.", calleeScope), "calleeScope");
		case ResourceScope.AppDomain:
			break;
		}
		switch (calleeScope & (ResourceScope.Private | ResourceScope.Assembly))
		{
		case ResourceScope.None:
			switch (consumeAsScope & (ResourceScope.Private | ResourceScope.Assembly))
			{
			case ResourceScope.Assembly:
				sxSRequirements |= SxSRequirements.AssemblyName;
				break;
			case ResourceScope.Private:
				sxSRequirements |= SxSRequirements.AssemblyName | SxSRequirements.TypeName;
				break;
			default:
				throw new ArgumentException(Environment.GetResourceString("Unknown value for the ResourceScope: {0}  Too many resource visibility bits may be set.", consumeAsScope), "consumeAsScope");
			case ResourceScope.None:
				break;
			}
			break;
		case ResourceScope.Assembly:
			if ((consumeAsScope & ResourceScope.Private) != ResourceScope.None)
			{
				sxSRequirements |= SxSRequirements.TypeName;
			}
			break;
		default:
			throw new ArgumentException(Environment.GetResourceString("Unknown value for the ResourceScope: {0}  Too many resource visibility bits may be set.", calleeScope), "calleeScope");
		case ResourceScope.Private:
			break;
		}
		return sxSRequirements;
	}
}
