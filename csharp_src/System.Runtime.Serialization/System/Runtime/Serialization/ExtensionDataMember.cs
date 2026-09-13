namespace System.Runtime.Serialization;

internal class ExtensionDataMember
{
	private string name;

	private string ns;

	private IDataNode value;

	private int memberIndex;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string Namespace
	{
		get
		{
			return ns;
		}
		set
		{
			ns = value;
		}
	}

	public IDataNode Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public int MemberIndex
	{
		get
		{
			return memberIndex;
		}
		set
		{
			memberIndex = value;
		}
	}
}
