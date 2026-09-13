namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class DataMemberAttribute : Attribute
{
	private string name;

	private bool isNameSetExplicitly;

	private int order = -1;

	private bool isRequired;

	private bool emitDefaultValue = true;

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

	public int Order
	{
		get
		{
			return order;
		}
		set
		{
			if (value < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Property 'Order' in DataMemberAttribute attribute cannot be a negative number.")));
			}
			order = value;
		}
	}

	public bool IsRequired
	{
		get
		{
			return isRequired;
		}
		set
		{
			isRequired = value;
		}
	}

	public bool EmitDefaultValue
	{
		get
		{
			return emitDefaultValue;
		}
		set
		{
			emitDefaultValue = value;
		}
	}
}
