using System.Collections.Generic;

namespace System.Runtime.Serialization;

internal class ClassDataNode : DataNode<object>
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

	internal ClassDataNode()
	{
		dataType = Globals.TypeOfClassDataNode;
	}

	public override void Clear()
	{
		base.Clear();
		members = null;
	}
}
