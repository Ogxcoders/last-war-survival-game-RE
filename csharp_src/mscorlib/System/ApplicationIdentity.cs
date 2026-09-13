using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System;

[Serializable]
[ComVisible(false)]
public sealed class ApplicationIdentity : ISerializable
{
	private string _fullName;

	private string _codeBase;

	public string CodeBase => _codeBase;

	public string FullName => _fullName;

	public ApplicationIdentity(string applicationIdentityFullName)
	{
		if (applicationIdentityFullName == null)
		{
			throw new ArgumentNullException("applicationIdentityFullName");
		}
		if (applicationIdentityFullName.IndexOf(", Culture=") == -1)
		{
			_fullName = applicationIdentityFullName + ", Culture=neutral";
		}
		else
		{
			_fullName = applicationIdentityFullName;
		}
	}

	public override string ToString()
	{
		return _fullName;
	}

	[MonoTODO("Missing serialization")]
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
	}
}
