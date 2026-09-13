namespace System.Web.Services.Protocols;

internal class MethodStubInfo
{
	internal LogicalMethodInfo MethodInfo;

	internal TypeStubInfo TypeStub;

	internal string Name;

	internal WebMethodAttribute MethodAttribute;

	internal string OperationName => MethodInfo.Name;

	public MethodStubInfo(TypeStubInfo parent, LogicalMethodInfo source)
	{
		TypeStub = parent;
		MethodInfo = source;
		object[] customAttributes = source.GetCustomAttributes(typeof(WebMethodAttribute));
		if (customAttributes.Length != 0)
		{
			MethodAttribute = (WebMethodAttribute)customAttributes[0];
			Name = MethodAttribute.MessageName;
			if (Name == "")
			{
				Name = source.Name;
			}
		}
		else
		{
			Name = source.Name;
		}
	}
}
