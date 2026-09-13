using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json;

internal class JsonFormatWriterGenerator
{
	private class CriticalHelper
	{
		internal JsonFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
		{
			return delegate(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, ClassDataContract dataContract, XmlDictionaryString[] memberNames)
			{
				new JsonFormatWriterInterpreter(classContract).WriteToJson(xmlWriter, obj, context, dataContract, memberNames);
			};
		}

		internal JsonFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
		{
			return delegate(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, CollectionDataContract dataContract)
			{
				new JsonFormatWriterInterpreter(collectionContract).WriteCollectionToJson(xmlWriter, obj, context, dataContract);
			};
		}
	}

	[SecurityCritical]
	private CriticalHelper helper;

	[SecurityCritical]
	public JsonFormatWriterGenerator()
	{
		helper = new CriticalHelper();
	}

	[SecurityCritical]
	internal JsonFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
	{
		try
		{
			if (TD.DCJsonGenWriterStartIsEnabled())
			{
				TD.DCJsonGenWriterStart("Class", classContract.UnderlyingType.FullName);
			}
			return helper.GenerateClassWriter(classContract);
		}
		finally
		{
			if (TD.DCJsonGenWriterStopIsEnabled())
			{
				TD.DCJsonGenWriterStop();
			}
		}
	}

	[SecurityCritical]
	internal JsonFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
	{
		try
		{
			if (TD.DCJsonGenWriterStartIsEnabled())
			{
				TD.DCJsonGenWriterStart("Collection", collectionContract.UnderlyingType.FullName);
			}
			return helper.GenerateCollectionWriter(collectionContract);
		}
		finally
		{
			if (TD.DCJsonGenWriterStopIsEnabled())
			{
				TD.DCJsonGenWriterStop();
			}
		}
	}
}
