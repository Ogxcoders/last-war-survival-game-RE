namespace System.Runtime.Serialization;

internal class ISerializableDataMember
{
	private string name;

	private IDataNode value;

	internal string Name
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

	internal IDataNode Value
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
}
