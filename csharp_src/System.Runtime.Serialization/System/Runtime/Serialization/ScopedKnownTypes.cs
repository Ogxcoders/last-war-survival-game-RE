using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization;

internal struct ScopedKnownTypes
{
	internal Dictionary<XmlQualifiedName, DataContract>[] dataContractDictionaries;

	private int count;

	internal void Push(Dictionary<XmlQualifiedName, DataContract> dataContractDictionary)
	{
		if (dataContractDictionaries == null)
		{
			dataContractDictionaries = new Dictionary<XmlQualifiedName, DataContract>[4];
		}
		else if (count == dataContractDictionaries.Length)
		{
			Array.Resize(ref dataContractDictionaries, dataContractDictionaries.Length * 2);
		}
		dataContractDictionaries[count++] = dataContractDictionary;
	}

	internal void Pop()
	{
		count--;
	}

	internal DataContract GetDataContract(XmlQualifiedName qname)
	{
		for (int num = count - 1; num >= 0; num--)
		{
			if (dataContractDictionaries[num].TryGetValue(qname, out var value))
			{
				return value;
			}
		}
		return null;
	}
}
