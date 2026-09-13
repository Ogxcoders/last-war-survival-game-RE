namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public sealed class CollectionDataContractAttribute : Attribute
{
	private string name;

	private string ns;

	private string itemName;

	private string keyName;

	private string valueName;

	private bool isReference;

	private bool isNameSetExplicitly;

	private bool isNamespaceSetExplicitly;

	private bool isReferenceSetExplicitly;

	private bool isItemNameSetExplicitly;

	private bool isKeyNameSetExplicitly;

	private bool isValueNameSetExplicitly;

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

	public string ItemName
	{
		get
		{
			return itemName;
		}
		set
		{
			itemName = value;
			isItemNameSetExplicitly = true;
		}
	}

	public bool IsItemNameSetExplicitly => isItemNameSetExplicitly;

	public string KeyName
	{
		get
		{
			return keyName;
		}
		set
		{
			keyName = value;
			isKeyNameSetExplicitly = true;
		}
	}

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

	public bool IsKeyNameSetExplicitly => isKeyNameSetExplicitly;

	public string ValueName
	{
		get
		{
			return valueName;
		}
		set
		{
			valueName = value;
			isValueNameSetExplicitly = true;
		}
	}

	public bool IsValueNameSetExplicitly => isValueNameSetExplicitly;
}
