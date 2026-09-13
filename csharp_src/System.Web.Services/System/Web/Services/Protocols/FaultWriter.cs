using System.Xml.Serialization;

namespace System.Web.Services.Protocols;

internal class FaultWriter : XmlSerializationWriter
{
	public void WriteRoot_Fault(object o)
	{
		WriteStartDocument();
		Fault ob = (Fault)o;
		TopLevelElement();
		WriteObject_Fault(ob, "Fault", "http://schemas.xmlsoap.org/soap/envelope/", isNullable: true, needType: false, writeWrappingElem: true);
	}

	private void WriteObject_Fault(Fault ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)
	{
		if (ob == null)
		{
			if (isNullable)
			{
				WriteNullTagLiteral(element, namesp);
			}
			return;
		}
		if (writeWrappingElem)
		{
			WriteStartElement(element, namesp, ob);
		}
		if (needType)
		{
			WriteXsiType("Fault", "http://schemas.xmlsoap.org/soap/envelope/");
		}
		WriteElementQualifiedName("faultcode", "", ob.faultcode);
		WriteElementString("faultstring", "", ob.faultstring);
		WriteElementString("faultactor", "", ob.faultactor);
		WriteElementLiteral(ob.detail, "detail", "", isNullable: false, any: false);
		if (writeWrappingElem)
		{
			WriteEndElement(ob);
		}
	}

	protected override void InitCallbacks()
	{
	}
}
