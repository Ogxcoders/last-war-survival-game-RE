using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

internal class GenericNameProvider : IGenericNameProvider
{
	private string genericTypeName;

	private object[] genericParams;

	private IList<int> nestedParamCounts;

	public bool ParametersFromBuiltInNamespaces
	{
		get
		{
			bool flag = true;
			for (int i = 0; i < GetParameterCount(); i++)
			{
				if (!flag)
				{
					break;
				}
				flag = DataContract.IsBuiltInNamespace(GetStableName(i).Namespace);
			}
			return flag;
		}
	}

	internal GenericNameProvider(Type type)
		: this(DataContract.GetClrTypeFullName(type.GetGenericTypeDefinition()), type.GetGenericArguments())
	{
	}

	internal GenericNameProvider(string genericTypeName, object[] genericParams)
	{
		this.genericTypeName = genericTypeName;
		this.genericParams = new object[genericParams.Length];
		genericParams.CopyTo(this.genericParams, 0);
		DataContract.GetClrNameAndNamespace(genericTypeName, out var localName, out var _);
		nestedParamCounts = DataContract.GetDataContractNameForGenericName(localName, null);
	}

	public int GetParameterCount()
	{
		return genericParams.Length;
	}

	public IList<int> GetNestedParameterCounts()
	{
		return nestedParamCounts;
	}

	public string GetParameterName(int paramIndex)
	{
		return GetStableName(paramIndex).Name;
	}

	public string GetNamespaces()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < GetParameterCount(); i++)
		{
			stringBuilder.Append(" ").Append(GetStableName(i).Namespace);
		}
		return stringBuilder.ToString();
	}

	public string GetGenericTypeName()
	{
		return genericTypeName;
	}

	private XmlQualifiedName GetStableName(int i)
	{
		object obj = genericParams[i];
		XmlQualifiedName xmlQualifiedName = obj as XmlQualifiedName;
		if (xmlQualifiedName == null)
		{
			Type type = obj as Type;
			xmlQualifiedName = (XmlQualifiedName)((type != null) ? (genericParams[i] = DataContract.GetStableName(type)) : (genericParams[i] = ((DataContract)obj).StableName));
		}
		return xmlQualifiedName;
	}
}
