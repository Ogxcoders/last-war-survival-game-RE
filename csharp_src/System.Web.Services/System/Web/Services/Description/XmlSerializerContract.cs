using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Description;

internal class XmlSerializerContract : XmlSerializerImplementation
{
	private Hashtable readMethods;

	private Hashtable writeMethods;

	private Hashtable typedSerializers;

	public override XmlSerializationReader Reader => new ServiceDescriptionReaderBase();

	public override XmlSerializationWriter Writer => new ServiceDescriptionWriterBase();

	public override Hashtable ReadMethods
	{
		get
		{
			lock (this)
			{
				if (readMethods == null)
				{
					readMethods = new Hashtable();
					readMethods.Add("System.Web.Services.Description.ServiceDescription", "ReadRoot_ServiceDescription");
				}
				return readMethods;
			}
		}
	}

	public override Hashtable WriteMethods
	{
		get
		{
			lock (this)
			{
				if (writeMethods == null)
				{
					writeMethods = new Hashtable();
					writeMethods.Add("System.Web.Services.Description.ServiceDescription", "WriteRoot_ServiceDescription");
				}
				return writeMethods;
			}
		}
	}

	public override Hashtable TypedSerializers
	{
		get
		{
			lock (this)
			{
				if (typedSerializers == null)
				{
					typedSerializers = new Hashtable();
					typedSerializers.Add("System.Web.Services.Description.ServiceDescription", new definitionsSerializer());
				}
				return typedSerializers;
			}
		}
	}

	public override XmlSerializer GetSerializer(Type type)
	{
		if (type.FullName == "System.Web.Services.Description.ServiceDescription")
		{
			return (XmlSerializer)TypedSerializers["System.Web.Services.Description.ServiceDescription"];
		}
		return base.GetSerializer(type);
	}

	public override bool CanSerialize(Type type)
	{
		if (type == typeof(ServiceDescription))
		{
			return true;
		}
		return false;
	}
}
