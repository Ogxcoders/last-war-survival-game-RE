using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;
using System.Security;

namespace System.Runtime.Serialization.Formatters;

[Serializable]
[SoapType(Embedded = true)]
[ComVisible(true)]
public sealed class SoapFault : ISerializable
{
	private string faultCode;

	private string faultString;

	private string faultActor;

	[SoapField(Embedded = true)]
	private object detail;

	public string FaultCode
	{
		get
		{
			return faultCode;
		}
		set
		{
			faultCode = value;
		}
	}

	public string FaultString
	{
		get
		{
			return faultString;
		}
		set
		{
			faultString = value;
		}
	}

	public string FaultActor
	{
		get
		{
			return faultActor;
		}
		set
		{
			faultActor = value;
		}
	}

	public object Detail
	{
		get
		{
			return detail;
		}
		set
		{
			detail = value;
		}
	}

	public SoapFault()
	{
	}

	public SoapFault(string faultCode, string faultString, string faultActor, ServerFault serverFault)
	{
		this.faultCode = faultCode;
		this.faultString = faultString;
		this.faultActor = faultActor;
		detail = serverFault;
	}

	internal SoapFault(SerializationInfo info, StreamingContext context)
	{
		SerializationInfoEnumerator enumerator = info.GetEnumerator();
		while (enumerator.MoveNext())
		{
			string name = enumerator.Name;
			object value = enumerator.Value;
			if (string.Compare(name, "faultCode", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				int num = ((string)value).IndexOf(':');
				if (num > -1)
				{
					faultCode = ((string)value).Substring(++num);
				}
				else
				{
					faultCode = (string)value;
				}
			}
			else if (string.Compare(name, "faultString", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				faultString = (string)value;
			}
			else if (string.Compare(name, "faultActor", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				faultActor = (string)value;
			}
			else if (string.Compare(name, "detail", ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				detail = value;
			}
		}
	}

	[SecurityCritical]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		info.AddValue("faultcode", "SOAP-ENV:" + faultCode);
		info.AddValue("faultstring", faultString);
		if (faultActor != null)
		{
			info.AddValue("faultactor", faultActor);
		}
		info.AddValue("detail", detail, typeof(object));
	}
}
