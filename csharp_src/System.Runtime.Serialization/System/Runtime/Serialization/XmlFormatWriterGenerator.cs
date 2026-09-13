using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;

namespace System.Runtime.Serialization;

internal sealed class XmlFormatWriterGenerator
{
	private class CriticalHelper
	{
		internal XmlFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
		{
			return delegate(XmlWriterDelegator xw, object obj, XmlObjectSerializerWriteContext ctx, ClassDataContract ctr)
			{
				new XmlFormatWriterInterpreter(classContract).WriteToXml(xw, obj, ctx, ctr);
			};
		}

		internal XmlFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
		{
			return delegate(XmlWriterDelegator xw, object obj, XmlObjectSerializerWriteContext ctx, CollectionDataContract ctr)
			{
				new XmlFormatWriterInterpreter(collectionContract).WriteCollectionToXml(xw, obj, ctx, ctr);
			};
		}
	}

	[SecurityCritical]
	private CriticalHelper helper;

	[SecurityCritical]
	public XmlFormatWriterGenerator()
	{
		helper = new CriticalHelper();
	}

	[SecurityCritical]
	internal XmlFormatClassWriterDelegate GenerateClassWriter(ClassDataContract classContract)
	{
		try
		{
			if (TD.DCGenWriterStartIsEnabled())
			{
				TD.DCGenWriterStart("Class", classContract.UnderlyingType.FullName);
			}
			return helper.GenerateClassWriter(classContract);
		}
		finally
		{
			if (TD.DCGenWriterStopIsEnabled())
			{
				TD.DCGenWriterStop();
			}
		}
	}

	[SecurityCritical]
	internal XmlFormatCollectionWriterDelegate GenerateCollectionWriter(CollectionDataContract collectionContract)
	{
		try
		{
			if (TD.DCGenWriterStartIsEnabled())
			{
				TD.DCGenWriterStart("Collection", collectionContract.UnderlyingType.FullName);
			}
			return helper.GenerateCollectionWriter(collectionContract);
		}
		finally
		{
			if (TD.DCGenWriterStopIsEnabled())
			{
				TD.DCGenWriterStop();
			}
		}
	}
}
