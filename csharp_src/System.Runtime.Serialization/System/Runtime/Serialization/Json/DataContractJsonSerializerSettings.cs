using System.Collections.Generic;

namespace System.Runtime.Serialization.Json;

public class DataContractJsonSerializerSettings
{
	private int maxItemsInObjectGraph = int.MaxValue;

	public string RootName { get; set; }

	public IEnumerable<Type> KnownTypes { get; set; }

	public int MaxItemsInObjectGraph
	{
		get
		{
			return maxItemsInObjectGraph;
		}
		set
		{
			maxItemsInObjectGraph = value;
		}
	}

	public bool IgnoreExtensionDataObject { get; set; }

	public IDataContractSurrogate DataContractSurrogate { get; set; }

	public EmitTypeInformation EmitTypeInformation { get; set; }

	public DateTimeFormat DateTimeFormat { get; set; }

	public bool SerializeReadOnlyTypes { get; set; }

	public bool UseSimpleDictionaryFormat { get; set; }
}
