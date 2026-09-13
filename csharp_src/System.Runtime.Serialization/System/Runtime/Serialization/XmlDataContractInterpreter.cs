using System.Reflection;
using System.Xml.Serialization;

namespace System.Runtime.Serialization;

internal class XmlDataContractInterpreter
{
	private XmlDataContract contract;

	public XmlDataContractInterpreter(XmlDataContract contract)
	{
		this.contract = contract;
	}

	public IXmlSerializable CreateXmlSerializable()
	{
		Type underlyingType = contract.UnderlyingType;
		object obj = null;
		obj = ((!underlyingType.IsValueType) ? GetConstructor().Invoke(new object[0]) : FormatterServices.GetUninitializedObject(underlyingType));
		return (IXmlSerializable)obj;
	}

	private ConstructorInfo GetConstructor()
	{
		Type underlyingType = contract.UnderlyingType;
		if (underlyingType.IsValueType)
		{
			return null;
		}
		ConstructorInfo constructor = underlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
		if (constructor == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("IXmlSerializable Type '{0}' must have default constructor.", DataContract.GetClrTypeFullName(underlyingType))));
		}
		return constructor;
	}
}
