using System.Globalization;
using System.Runtime.Serialization;

namespace System.Security.AccessControl;

[Serializable]
public sealed class PrivilegeNotHeldException : UnauthorizedAccessException, ISerializable
{
	private readonly string _privilegeName;

	public string PrivilegeName => _privilegeName;

	public PrivilegeNotHeldException()
		: base("The process does not possess some privilege required for this operation.")
	{
	}

	public PrivilegeNotHeldException(string privilege)
		: base(string.Format(CultureInfo.CurrentCulture, "The process does not possess the '{0}' privilege which is required for this operation.", privilege))
	{
		_privilegeName = privilege;
	}

	public PrivilegeNotHeldException(string privilege, Exception inner)
		: base(string.Format(CultureInfo.CurrentCulture, "The process does not possess the '{0}' privilege which is required for this operation.", privilege), inner)
	{
		_privilegeName = privilege;
	}

	private PrivilegeNotHeldException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		_privilegeName = info.GetString("PrivilegeName");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("PrivilegeName", _privilegeName, typeof(string));
	}
}
