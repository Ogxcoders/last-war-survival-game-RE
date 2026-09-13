namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
public sealed class DataContractAttribute : Attribute
{
	private string name;

	private string ns;

	private bool isNameSetExplicitly;

	private bool isNamespaceSetExplicitly;

	private bool isReference;

	private bool isReferenceSetExplicitly;

	public bool IsReference
	{
		get
		{
			return isReference;
		}
		set
		{
			isReference = value;
			isReferenceSetExplicitly = true;
		}
	}

	public bool IsReferenceSetExplicitly => isReferenceSetExplicitly;

	public string Namespace
	{
		get
		{
			return ns;
		}
		set
		{
			ns = value;
			isNamespaceSetExplicitly = true;
		}
	}

	public bool IsNamespaceSetExplicitly => isNamespaceSetExplicitly;

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
			isNameSetExplicitly = true;
		}
	}

	public bool IsNameSetExplicitly => isNameSetExplicitly;
}
