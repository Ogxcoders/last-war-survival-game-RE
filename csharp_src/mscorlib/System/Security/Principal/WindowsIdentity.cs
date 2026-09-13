using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal;

[Serializable]
[ComVisible(true)]
public class WindowsIdentity : ClaimsIdentity, IIdentity, IDeserializationCallback, ISerializable, IDisposable
{
	private IntPtr _token;

	private string _type;

	private WindowsAccountType _account;

	private bool _authenticated;

	private string _name;

	private SerializationInfo _info;

	private static IntPtr invalidWindows = IntPtr.Zero;

	[NonSerialized]
	public new const string DefaultIssuer = "AD AUTHORITY";

	public sealed override string AuthenticationType => _type;

	public virtual bool IsAnonymous => _account == WindowsAccountType.Anonymous;

	public override bool IsAuthenticated => _authenticated;

	public virtual bool IsGuest => _account == WindowsAccountType.Guest;

	public virtual bool IsSystem => _account == WindowsAccountType.System;

	public override string Name
	{
		get
		{
			if (_name == null)
			{
				_name = GetTokenName(_token);
			}
			return _name;
		}
	}

	public virtual IntPtr Token => _token;

	[MonoTODO("not implemented")]
	public IdentityReferenceCollection Groups
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO("not implemented")]
	[ComVisible(false)]
	public TokenImpersonationLevel ImpersonationLevel
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO("not implemented")]
	[ComVisible(false)]
	public SecurityIdentifier Owner
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[MonoTODO("not implemented")]
	[ComVisible(false)]
	public SecurityIdentifier User
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public SafeAccessTokenHandle AccessToken
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(IntPtr userToken)
		: this(userToken, null, WindowsAccountType.Normal, isAuthenticated: false)
	{
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(IntPtr userToken, string type)
		: this(userToken, type, WindowsAccountType.Normal, isAuthenticated: false)
	{
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType)
		: this(userToken, type, acctType, isAuthenticated: false)
	{
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(IntPtr userToken, string type, WindowsAccountType acctType, bool isAuthenticated)
	{
		_type = type;
		_account = acctType;
		_authenticated = isAuthenticated;
		_name = null;
		SetToken(userToken);
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(string sUserPrincipalName)
		: this(sUserPrincipalName, null)
	{
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(string sUserPrincipalName, string type)
	{
		if (sUserPrincipalName == null)
		{
			throw new NullReferenceException("sUserPrincipalName");
		}
		IntPtr userToken = GetUserToken(sUserPrincipalName);
		if (!Environment.IsUnix && userToken == IntPtr.Zero)
		{
			throw new ArgumentException("only for Windows Server 2003 +");
		}
		_authenticated = true;
		_account = WindowsAccountType.Normal;
		_type = type;
		SetToken(userToken);
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public WindowsIdentity(SerializationInfo info, StreamingContext context)
	{
		_info = info;
	}

	internal WindowsIdentity(ClaimsIdentity claimsIdentity, IntPtr userToken)
		: base(claimsIdentity)
	{
		if (userToken != IntPtr.Zero && userToken.ToInt64() > 0)
		{
			SetToken(userToken);
		}
	}

	[ComVisible(false)]
	public void Dispose()
	{
		_token = IntPtr.Zero;
	}

	[ComVisible(false)]
	protected virtual void Dispose(bool disposing)
	{
		_token = IntPtr.Zero;
	}

	public static WindowsIdentity GetAnonymous()
	{
		WindowsIdentity windowsIdentity = null;
		if (Environment.IsUnix)
		{
			windowsIdentity = new WindowsIdentity("nobody");
			windowsIdentity._account = WindowsAccountType.Anonymous;
			windowsIdentity._authenticated = false;
			windowsIdentity._type = string.Empty;
		}
		else
		{
			windowsIdentity = new WindowsIdentity(IntPtr.Zero, string.Empty, WindowsAccountType.Anonymous, isAuthenticated: false);
			windowsIdentity._name = string.Empty;
		}
		return windowsIdentity;
	}

	public static WindowsIdentity GetCurrent()
	{
		return new WindowsIdentity(GetCurrentToken(), null, WindowsAccountType.Normal, isAuthenticated: true);
	}

	[MonoTODO("need icall changes")]
	public static WindowsIdentity GetCurrent(bool ifImpersonating)
	{
		throw new NotImplementedException();
	}

	[MonoTODO("need icall changes")]
	public static WindowsIdentity GetCurrent(TokenAccessLevels desiredAccess)
	{
		throw new NotImplementedException();
	}

	public virtual WindowsImpersonationContext Impersonate()
	{
		return new WindowsImpersonationContext(_token);
	}

	[SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)]
	public static WindowsImpersonationContext Impersonate(IntPtr userToken)
	{
		return new WindowsImpersonationContext(userToken);
	}

	[SecuritySafeCritical]
	public static void RunImpersonated(SafeAccessTokenHandle safeAccessTokenHandle, Action action)
	{
		throw new NotImplementedException();
	}

	[SecuritySafeCritical]
	public static T RunImpersonated<T>(SafeAccessTokenHandle safeAccessTokenHandle, Func<T> func)
	{
		throw new NotImplementedException();
	}

	void IDeserializationCallback.OnDeserialization(object sender)
	{
		_token = (IntPtr)_info.GetValue("m_userToken", typeof(IntPtr));
		_name = _info.GetString("m_name");
		if (_name != null)
		{
			if (GetTokenName(_token) != _name)
			{
				throw new SerializationException("Token-Name mismatch.");
			}
		}
		else
		{
			_name = GetTokenName(_token);
			if (_name == null)
			{
				throw new SerializationException("Token doesn't match a user.");
			}
		}
		_type = _info.GetString("m_type");
		_account = (WindowsAccountType)_info.GetValue("m_acctType", typeof(WindowsAccountType));
		_authenticated = _info.GetBoolean("m_isAuthenticated");
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("m_userToken", _token);
		info.AddValue("m_name", _name);
		info.AddValue("m_type", _type);
		info.AddValue("m_acctType", _account);
		info.AddValue("m_isAuthenticated", _authenticated);
	}

	internal ClaimsIdentity CloneAsBase()
	{
		return base.Clone();
	}

	internal IntPtr GetTokenInternal()
	{
		return _token;
	}

	private void SetToken(IntPtr token)
	{
		if (Environment.IsUnix)
		{
			_token = token;
			if (_type == null)
			{
				_type = "POSIX";
			}
			if (_token == IntPtr.Zero)
			{
				_account = WindowsAccountType.System;
			}
		}
		else
		{
			if (token == invalidWindows && _account != WindowsAccountType.Anonymous)
			{
				throw new ArgumentException("Invalid token");
			}
			_token = token;
			if (_type == null)
			{
				_type = "NTLM";
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string[] _GetRoles(IntPtr token);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetCurrentToken();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string GetTokenName(IntPtr token);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr GetUserToken(string username);
}
