using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security;

[Serializable]
[ComVisible(true)]
[MonoTODO("Not supported in the runtime")]
public class HostProtectionException : SystemException
{
	private HostProtectionResource _protected;

	private HostProtectionResource _demanded;

	public HostProtectionResource DemandedResources => _demanded;

	public HostProtectionResource ProtectedResources => _protected;

	public HostProtectionException()
	{
	}

	public HostProtectionException(string message)
		: base(message)
	{
	}

	public HostProtectionException(string message, Exception e)
		: base(message, e)
	{
	}

	public HostProtectionException(string message, HostProtectionResource protectedResources, HostProtectionResource demandedResources)
		: base(message)
	{
		_protected = protectedResources;
		_demanded = demandedResources;
	}

	protected HostProtectionException(SerializationInfo info, StreamingContext context)
	{
		GetObjectData(info, context);
	}

	[MonoTODO]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
	}

	[MonoTODO]
	public override string ToString()
	{
		return base.ToString();
	}
}
