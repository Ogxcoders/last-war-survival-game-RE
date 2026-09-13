using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization;

internal sealed class XmlFormatReaderGenerator
{
	private class CriticalHelper
	{
		internal XmlFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
		{
			return (XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString[] memberNames, XmlDictionaryString[] memberNamespaces) => new XmlFormatReaderInterpreter(classContract).ReadFromXml(xr, ctx, memberNames, memberNamespaces);
		}

		internal XmlFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
		{
			return (XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString inm, XmlDictionaryString ins, CollectionDataContract cc) => new XmlFormatReaderInterpreter(collectionContract, isGetOnly: false).ReadCollectionFromXml(xr, ctx, inm, ins, cc);
		}

		internal XmlFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
		{
			return delegate(XmlReaderDelegator xr, XmlObjectSerializerReadContext ctx, XmlDictionaryString inm, XmlDictionaryString ins, CollectionDataContract cc)
			{
				new XmlFormatReaderInterpreter(collectionContract, isGetOnly: true).ReadGetOnlyCollectionFromXml(xr, ctx, inm, ins, cc);
			};
		}
	}

	[SecurityCritical]
	private CriticalHelper helper;

	[SecurityCritical]
	public XmlFormatReaderGenerator()
	{
		helper = new CriticalHelper();
	}

	[SecurityCritical]
	public XmlFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
	{
		try
		{
			if (TD.DCGenReaderStartIsEnabled())
			{
				TD.DCGenReaderStart("Class", classContract.UnderlyingType.FullName);
			}
			return helper.GenerateClassReader(classContract);
		}
		finally
		{
			if (TD.DCGenReaderStopIsEnabled())
			{
				TD.DCGenReaderStop();
			}
		}
	}

	[SecurityCritical]
	public XmlFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
	{
		try
		{
			if (TD.DCGenReaderStartIsEnabled())
			{
				TD.DCGenReaderStart("Collection", collectionContract.UnderlyingType.FullName);
			}
			return helper.GenerateCollectionReader(collectionContract);
		}
		finally
		{
			if (TD.DCGenReaderStopIsEnabled())
			{
				TD.DCGenReaderStop();
			}
		}
	}

	[SecurityCritical]
	public XmlFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
	{
		try
		{
			if (TD.DCGenReaderStartIsEnabled())
			{
				TD.DCGenReaderStart("GetOnlyCollection", collectionContract.UnderlyingType.FullName);
			}
			return helper.GenerateGetOnlyCollectionReader(collectionContract);
		}
		finally
		{
			if (TD.DCGenReaderStopIsEnabled())
			{
				TD.DCGenReaderStop();
			}
		}
	}

	[SecuritySafeCritical]
	internal static object UnsafeGetUninitializedObject(int id)
	{
		return FormatterServices.GetUninitializedObject(DataContract.GetDataContractForInitialization(id).TypeForInitialization);
	}
}
