using System.Collections;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class Soap12FaultSerializerImplementation : XmlSerializerImplementation
{
	private Hashtable readMethods;

	private Hashtable writeMethods;

	private Hashtable typedSerializers;

	public override XmlSerializationReader Reader => new Soap12FaultReader();

	public override XmlSerializationWriter Writer => new Soap12FaultWriter();

	public override Hashtable ReadMethods
	{
		get
		{
			lock (this)
			{
				if (readMethods == null)
				{
					readMethods = new Hashtable();
					readMethods.Add("", "ReadRoot_Soap12Fault");
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
					writeMethods.Add("", "WriteRoot_Soap12Fault");
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
					typedSerializers.Add("", new FaultSerializer());
				}
				return typedSerializers;
			}
		}
	}

	public override XmlSerializer GetSerializer(Type type)
	{
		if (type.FullName == "System.Web.Services.Protocols.Soap12Fault")
		{
			return (XmlSerializer)TypedSerializers[""];
		}
		return base.GetSerializer(type);
	}

	public override bool CanSerialize(Type type)
	{
		if (type == typeof(Soap12Fault))
		{
			return true;
		}
		return false;
	}
}
