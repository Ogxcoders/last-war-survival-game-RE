using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security;

[Serializable]
[ComVisible(true)]
[MonoTODO("CAS support is experimental (and unsupported).")]
[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
public abstract class CodeAccessPermission : IPermission, ISecurityEncodable, IStackWalk
{
	[Conditional("MONO_FEATURE_CAS")]
	public void Assert()
	{
		new PermissionSet(this).Assert();
	}

	public abstract IPermission Copy();

	[Conditional("MONO_FEATURE_CAS")]
	public void Demand()
	{
		if (SecurityManager.SecurityEnabled)
		{
			new PermissionSet(this).CasOnlyDemand(3);
		}
	}

	[Conditional("MONO_FEATURE_CAS")]
	public void Deny()
	{
		new PermissionSet(this).Deny();
	}

	[ComVisible(false)]
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		CodeAccessPermission codeAccessPermission = obj as CodeAccessPermission;
		if (IsSubsetOf(codeAccessPermission))
		{
			return codeAccessPermission.IsSubsetOf(this);
		}
		return false;
	}

	public abstract void FromXml(SecurityElement elem);

	[ComVisible(false)]
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public abstract IPermission Intersect(IPermission target);

	public abstract bool IsSubsetOf(IPermission target);

	public override string ToString()
	{
		return ToXml().ToString();
	}

	public abstract SecurityElement ToXml();

	public virtual IPermission Union(IPermission other)
	{
		if (other != null)
		{
			throw new NotSupportedException();
		}
		return null;
	}

	[Conditional("MONO_FEATURE_CAS")]
	public void PermitOnly()
	{
		new PermissionSet(this).PermitOnly();
	}

	[Conditional("MONO_FEATURE_CAS")]
	public static void RevertAll()
	{
		if (!SecurityManager.SecurityEnabled)
		{
			return;
		}
		throw new NotImplementedException();
	}

	[Conditional("MONO_FEATURE_CAS")]
	public static void RevertAssert()
	{
		if (!SecurityManager.SecurityEnabled)
		{
			return;
		}
		throw new NotImplementedException();
	}

	[Conditional("MONO_FEATURE_CAS")]
	public static void RevertDeny()
	{
		if (!SecurityManager.SecurityEnabled)
		{
			return;
		}
		throw new NotImplementedException();
	}

	[Conditional("MONO_FEATURE_CAS")]
	public static void RevertPermitOnly()
	{
		if (!SecurityManager.SecurityEnabled)
		{
			return;
		}
		throw new NotImplementedException();
	}

	internal SecurityElement Element(int version)
	{
		SecurityElement securityElement = new SecurityElement("IPermission");
		Type type = GetType();
		securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
		securityElement.AddAttribute("version", version.ToString());
		return securityElement;
	}

	internal static PermissionState CheckPermissionState(PermissionState state, bool allowUnrestricted)
	{
		if (state != PermissionState.None && state != PermissionState.Unrestricted)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid enum {0}"), state), "state");
		}
		return state;
	}

	internal static int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
	{
		if (se == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (se.Tag != "IPermission")
		{
			throw new ArgumentException(string.Format(Locale.GetText("Invalid tag {0}"), se.Tag), parameterName);
		}
		int num = minimumVersion;
		string text = se.Attribute("version");
		if (text != null)
		{
			try
			{
				num = int.Parse(text);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(string.Format(Locale.GetText("Couldn't parse version from '{0}'."), text), parameterName, innerException);
			}
		}
		if (num < minimumVersion || num > maximumVersion)
		{
			throw new ArgumentException(string.Format(Locale.GetText("Unknown version '{0}', expected versions between ['{1}','{2}']."), num, minimumVersion, maximumVersion), parameterName);
		}
		return num;
	}

	internal static bool IsUnrestricted(SecurityElement se)
	{
		string text = se.Attribute("Unrestricted");
		if (text == null)
		{
			return false;
		}
		return string.Compare(text, bool.TrueString, ignoreCase: true, CultureInfo.InvariantCulture) == 0;
	}

	internal static void ThrowInvalidPermission(IPermission target, Type expected)
	{
		throw new ArgumentException(string.Format(Locale.GetText("Invalid permission type '{0}', expected type '{1}'."), target.GetType(), expected), "target");
	}

	void IStackWalk.Assert()
	{
	}

	void IStackWalk.Deny()
	{
	}

	void IStackWalk.PermitOnly()
	{
	}

	void IStackWalk.Demand()
	{
	}

	void IPermission.Demand()
	{
	}
}
