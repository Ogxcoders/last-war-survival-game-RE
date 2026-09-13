using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal sealed class JsonFormatReaderGenerator
{
	private class CriticalHelper
	{
		internal JsonFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
		{
			return (XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDictionaryString, XmlDictionaryString[] memberNames) => new JsonFormatReaderInterpreter(classContract).ReadFromJson(xr, ctx, emptyDictionaryString, memberNames);
		}

		internal JsonFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
		{
			return (XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDS, XmlDictionaryString inm, CollectionDataContract cc) => new JsonFormatReaderInterpreter(collectionContract, isGetOnly: false).ReadCollectionFromJson(xr, ctx, emptyDS, inm, cc);
		}

		internal JsonFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
		{
			return delegate(XmlReaderDelegator xr, XmlObjectSerializerReadContextComplexJson ctx, XmlDictionaryString emptyDS, XmlDictionaryString inm, CollectionDataContract cc)
			{
				new JsonFormatReaderInterpreter(collectionContract, isGetOnly: true).ReadGetOnlyCollectionFromJson(xr, ctx, emptyDS, inm, cc);
			};
		}
	}

	[SecurityCritical]
	private CriticalHelper helper;

	[SecurityCritical]
	public JsonFormatReaderGenerator()
	{
		helper = new CriticalHelper();
	}

	[SecurityCritical]
	public JsonFormatClassReaderDelegate GenerateClassReader(ClassDataContract classContract)
	{
		try
		{
			if (TD.DCJsonGenReaderStartIsEnabled())
			{
				TD.DCJsonGenReaderStart("Class", classContract.UnderlyingType.FullName);
			}
			return helper.GenerateClassReader(classContract);
		}
		finally
		{
			if (TD.DCJsonGenReaderStopIsEnabled())
			{
				TD.DCJsonGenReaderStop();
			}
		}
	}

	[SecurityCritical]
	public JsonFormatCollectionReaderDelegate GenerateCollectionReader(CollectionDataContract collectionContract)
	{
		try
		{
			if (TD.DCJsonGenReaderStartIsEnabled())
			{
				TD.DCJsonGenReaderStart("Collection", collectionContract.StableName.Name);
			}
			return helper.GenerateCollectionReader(collectionContract);
		}
		finally
		{
			if (TD.DCJsonGenReaderStopIsEnabled())
			{
				TD.DCJsonGenReaderStop();
			}
		}
	}

	[SecurityCritical]
	public JsonFormatGetOnlyCollectionReaderDelegate GenerateGetOnlyCollectionReader(CollectionDataContract collectionContract)
	{
		try
		{
			if (TD.DCJsonGenReaderStartIsEnabled())
			{
				TD.DCJsonGenReaderStart("GetOnlyCollection", collectionContract.UnderlyingType.FullName);
			}
			return helper.GenerateGetOnlyCollectionReader(collectionContract);
		}
		finally
		{
			if (TD.DCJsonGenReaderStopIsEnabled())
			{
				TD.DCJsonGenReaderStop();
			}
		}
	}
}
