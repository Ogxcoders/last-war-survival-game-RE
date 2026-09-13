using System.Collections.Generic;

namespace System.Runtime.Serialization;

public sealed class ExtensionDataObject
{
	private IList<ExtensionDataMember> members;

	internal IList<ExtensionDataMember> Members
	{
		get
		{
			return members;
		}
		set
		{
			members = value;
		}
	}

	internal ExtensionDataObject()
	{
	}
}
