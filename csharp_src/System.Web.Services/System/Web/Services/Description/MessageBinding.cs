namespace System.Web.Services.Description;

public abstract class MessageBinding : NamedItem
{
	private OperationBinding operationBinding;

	public OperationBinding OperationBinding => operationBinding;

	internal void SetParent(OperationBinding ob)
	{
		operationBinding = ob;
	}
}
