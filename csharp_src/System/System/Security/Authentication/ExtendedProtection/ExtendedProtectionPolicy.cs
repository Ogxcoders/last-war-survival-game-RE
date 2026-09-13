using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.Authentication.ExtendedProtection;

[Serializable]
[System.MonoTODO]
[TypeConverter(typeof(ExtendedProtectionPolicyTypeConverter))]
public class ExtendedProtectionPolicy : ISerializable
{
	public ChannelBinding CustomChannelBinding
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ServiceNameCollection CustomServiceNames
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public static bool OSSupportsExtendedProtection
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public PolicyEnforcement PolicyEnforcement
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ProtectionScenario ProtectionScenario
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[System.MonoTODO("Not implemented.")]
	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement)
	{
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ChannelBinding customChannelBinding)
	{
		throw new NotImplementedException();
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ICollection customServiceNames)
	{
		throw new NotImplementedException();
	}

	public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ServiceNameCollection customServiceNames)
	{
		throw new NotImplementedException();
	}

	protected ExtendedProtectionPolicy(SerializationInfo info, StreamingContext context)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public override string ToString()
	{
		return base.ToString();
	}

	[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw new NotImplementedException();
	}
}
