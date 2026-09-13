using System.Reflection;

namespace System.Xml.Xsl.Runtime;

internal sealed class EarlyBoundInfo
{
	private string namespaceUri;

	private ConstructorInfo constrInfo;

	public string NamespaceUri => namespaceUri;

	public Type EarlyBoundType => constrInfo.DeclaringType;

	public EarlyBoundInfo(string namespaceUri, Type ebType)
	{
		this.namespaceUri = namespaceUri;
		constrInfo = ebType.GetConstructor(Type.EmptyTypes);
	}

	public object CreateObject()
	{
		return constrInfo.Invoke(new object[0]);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is EarlyBoundInfo earlyBoundInfo))
		{
			return false;
		}
		if (namespaceUri == earlyBoundInfo.namespaceUri)
		{
			return constrInfo == earlyBoundInfo.constrInfo;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return namespaceUri.GetHashCode();
	}
}
