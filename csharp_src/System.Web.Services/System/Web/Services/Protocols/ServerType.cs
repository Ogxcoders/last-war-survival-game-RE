namespace System.Web.Services.Protocols;

public class ServerType
{
	private LogicalTypeInfo type;

	internal LogicalTypeInfo LogicalType => type;

	public ServerType(Type type)
	{
		this.type = TypeStubManager.GetLogicalTypeInfo(type);
	}
}
