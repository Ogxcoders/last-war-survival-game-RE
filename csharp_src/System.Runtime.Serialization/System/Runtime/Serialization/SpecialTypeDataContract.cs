using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal sealed class SpecialTypeDataContract : DataContract
{
	private class SpecialTypeDataContractCriticalHelper : DataContractCriticalHelper
	{
		internal SpecialTypeDataContractCriticalHelper(Type type)
			: base(type)
		{
		}

		internal SpecialTypeDataContractCriticalHelper(Type type, XmlDictionaryString name, XmlDictionaryString ns)
			: base(type)
		{
			SetDataContractName(name, ns);
		}
	}

	[SecurityCritical]
	private SpecialTypeDataContractCriticalHelper helper;

	internal override bool IsBuiltInDataContract => true;

	[SecuritySafeCritical]
	public SpecialTypeDataContract(Type type)
		: base(new SpecialTypeDataContractCriticalHelper(type))
	{
		helper = base.Helper as SpecialTypeDataContractCriticalHelper;
	}

	[SecuritySafeCritical]
	public SpecialTypeDataContract(Type type, XmlDictionaryString name, XmlDictionaryString ns)
		: base(new SpecialTypeDataContractCriticalHelper(type, name, ns))
	{
		helper = base.Helper as SpecialTypeDataContractCriticalHelper;
	}
}
