using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization;

internal class GenericInfo : IGenericNameProvider
{
	private string genericTypeName;

	private XmlQualifiedName stableName;

	private List<GenericInfo> paramGenericInfos;

	private List<int> nestedParamCounts;

	internal XmlQualifiedName StableName => stableName;

	internal IList<GenericInfo> Parameters => paramGenericInfos;

	public bool ParametersFromBuiltInNamespaces
	{
		get
		{
			bool flag = true;
			for (int i = 0; i < paramGenericInfos.Count; i++)
			{
				if (!flag)
				{
					break;
				}
				flag = DataContract.IsBuiltInNamespace(paramGenericInfos[i].GetStableNamespace());
			}
			return flag;
		}
	}

	internal GenericInfo(XmlQualifiedName stableName, string genericTypeName)
	{
		this.stableName = stableName;
		this.genericTypeName = genericTypeName;
		nestedParamCounts = new List<int>();
		nestedParamCounts.Add(0);
	}

	internal void Add(GenericInfo actualParamInfo)
	{
		if (paramGenericInfos == null)
		{
			paramGenericInfos = new List<GenericInfo>();
		}
		paramGenericInfos.Add(actualParamInfo);
	}

	internal void AddToLevel(int level, int count)
	{
		if (level >= nestedParamCounts.Count)
		{
			do
			{
				nestedParamCounts.Add((level == nestedParamCounts.Count) ? count : 0);
			}
			while (level >= nestedParamCounts.Count);
		}
		else
		{
			nestedParamCounts[level] += count;
		}
	}

	internal XmlQualifiedName GetExpandedStableName()
	{
		if (paramGenericInfos == null)
		{
			return stableName;
		}
		return new XmlQualifiedName(DataContract.EncodeLocalName(DataContract.ExpandGenericParameters(XmlConvert.DecodeName(stableName.Name), this)), stableName.Namespace);
	}

	internal string GetStableNamespace()
	{
		return stableName.Namespace;
	}

	public int GetParameterCount()
	{
		return paramGenericInfos.Count;
	}

	public IList<int> GetNestedParameterCounts()
	{
		return nestedParamCounts;
	}

	public string GetParameterName(int paramIndex)
	{
		return paramGenericInfos[paramIndex].GetExpandedStableName().Name;
	}

	public string GetNamespaces()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < paramGenericInfos.Count; i++)
		{
			stringBuilder.Append(" ").Append(paramGenericInfos[i].GetStableNamespace());
		}
		return stringBuilder.ToString();
	}

	public string GetGenericTypeName()
	{
		return genericTypeName;
	}
}
