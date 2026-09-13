using System.Collections.ObjectModel;

namespace System.Runtime.Serialization;

public class ExportOptions
{
	private Collection<Type> knownTypes;

	private IDataContractSurrogate dataContractSurrogate;

	public IDataContractSurrogate DataContractSurrogate
	{
		get
		{
			return dataContractSurrogate;
		}
		set
		{
			dataContractSurrogate = value;
		}
	}

	public Collection<Type> KnownTypes
	{
		get
		{
			if (knownTypes == null)
			{
				knownTypes = new Collection<Type>();
			}
			return knownTypes;
		}
	}

	internal IDataContractSurrogate GetSurrogate()
	{
		return dataContractSurrogate;
	}
}
