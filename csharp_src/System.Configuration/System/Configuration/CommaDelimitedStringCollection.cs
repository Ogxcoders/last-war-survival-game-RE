using System.Collections.Specialized;
using System.Reflection;
using Unity;

namespace System.Configuration;

[DefaultMember("Item")]
public sealed class CommaDelimitedStringCollection : StringCollection
{
	public bool IsModified
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(bool);
		}
	}

	public CommaDelimitedStringCollection()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public CommaDelimitedStringCollection Clone()
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public void SetReadOnly()
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
