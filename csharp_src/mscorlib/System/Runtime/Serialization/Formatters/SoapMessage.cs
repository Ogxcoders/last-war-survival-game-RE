using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters;

[Serializable]
[ComVisible(true)]
public class SoapMessage : ISoapMessage
{
	internal string[] paramNames;

	internal object[] paramValues;

	internal Type[] paramTypes;

	internal string methodName;

	internal string xmlNameSpace;

	internal Header[] headers;

	public string[] ParamNames
	{
		get
		{
			return paramNames;
		}
		set
		{
			paramNames = value;
		}
	}

	public object[] ParamValues
	{
		get
		{
			return paramValues;
		}
		set
		{
			paramValues = value;
		}
	}

	public Type[] ParamTypes
	{
		get
		{
			return paramTypes;
		}
		set
		{
			paramTypes = value;
		}
	}

	public string MethodName
	{
		get
		{
			return methodName;
		}
		set
		{
			methodName = value;
		}
	}

	public string XmlNameSpace
	{
		get
		{
			return xmlNameSpace;
		}
		set
		{
			xmlNameSpace = value;
		}
	}

	public Header[] Headers
	{
		get
		{
			return headers;
		}
		set
		{
			headers = value;
		}
	}
}
