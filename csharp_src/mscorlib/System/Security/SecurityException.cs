using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security;

[Serializable]
[ComVisible(true)]
public class SecurityException : SystemException
{
	private string permissionState;

	private Type permissionType;

	private string _granted;

	private string _refused;

	private object _demanded;

	private IPermission _firstperm;

	private MethodInfo _method;

	private SecurityAction _action;

	private object _denyset;

	private object _permitset;

	private AssemblyName _assembly;

	private string _url;

	private SecurityZone _zone;

	[ComVisible(false)]
	public SecurityAction Action
	{
		get
		{
			return _action;
		}
		set
		{
			_action = value;
		}
	}

	[ComVisible(false)]
	public object DenySetInstance
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _denyset;
		}
		set
		{
			_denyset = value;
		}
	}

	[ComVisible(false)]
	public AssemblyName FailedAssemblyInfo
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _assembly;
		}
		set
		{
			_assembly = value;
		}
	}

	[ComVisible(false)]
	public MethodInfo Method
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _method;
		}
		set
		{
			_method = value;
		}
	}

	[ComVisible(false)]
	public object PermitOnlySetInstance
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _permitset;
		}
		set
		{
			_permitset = value;
		}
	}

	public string Url
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _url;
		}
		set
		{
			_url = value;
		}
	}

	public SecurityZone Zone
	{
		get
		{
			return _zone;
		}
		set
		{
			_zone = value;
		}
	}

	[ComVisible(false)]
	public object Demanded
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _demanded;
		}
		set
		{
			_demanded = value;
		}
	}

	public IPermission FirstPermissionThatFailed
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _firstperm;
		}
		set
		{
			_firstperm = value;
		}
	}

	public string PermissionState
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return permissionState;
		}
		set
		{
			permissionState = value;
		}
	}

	public Type PermissionType
	{
		get
		{
			return permissionType;
		}
		set
		{
			permissionType = value;
		}
	}

	public string GrantedSet
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _granted;
		}
		set
		{
			_granted = value;
		}
	}

	public string RefusedSet
	{
		[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
		get
		{
			return _refused;
		}
		set
		{
			_refused = value;
		}
	}

	public SecurityException()
		: this(Locale.GetText("A security error has been detected."))
	{
	}

	public SecurityException(string message)
		: base(message)
	{
		base.HResult = -2146233078;
	}

	protected SecurityException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		base.HResult = -2146233078;
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Name == "PermissionState")
			{
				permissionState = (string)enumerator.Value;
				break;
			}
		}
	}

	public SecurityException(string message, Exception inner)
		: base(message, inner)
	{
		base.HResult = -2146233078;
	}

	public SecurityException(string message, Type type)
		: base(message)
	{
		base.HResult = -2146233078;
		permissionType = type;
	}

	public SecurityException(string message, Type type, string state)
		: base(message)
	{
		base.HResult = -2146233078;
		permissionType = type;
		permissionState = state;
	}

	internal SecurityException(string message, PermissionSet granted, PermissionSet refused)
		: base(message)
	{
		base.HResult = -2146233078;
		_granted = granted.ToString();
		_refused = refused.ToString();
	}

	public SecurityException(string message, object deny, object permitOnly, MethodInfo method, object demanded, IPermission permThatFailed)
		: base(message)
	{
		base.HResult = -2146233078;
		_denyset = deny;
		_permitset = permitOnly;
		_method = method;
		_demanded = demanded;
		_firstperm = permThatFailed;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		try
		{
			info.AddValue("PermissionState", permissionState);
		}
		catch (SecurityException)
		{
		}
	}

	public override string ToString()
	{
		return base.ToString();
	}
}
