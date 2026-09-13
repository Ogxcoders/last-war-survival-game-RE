using System.IO;
using System.Reflection;
using System.Text;

namespace System.Web.Services.Protocols;

public abstract class UrlEncodedParameterWriter : MimeParameterWriter
{
	private Encoding requestEncoding;

	private ParameterInfo[] parameters;

	public override Encoding RequestEncoding
	{
		get
		{
			return requestEncoding;
		}
		set
		{
			requestEncoding = value;
		}
	}

	protected void Encode(TextWriter writer, object[] values)
	{
		for (int i = 0; i < values.Length; i++)
		{
			if (i > 0)
			{
				writer.Write("&");
			}
			Encode(writer, parameters[i].Name, values[i]);
		}
	}

	protected void Encode(TextWriter writer, string name, object value)
	{
		if (requestEncoding != null)
		{
			writer.Write(HttpUtility.UrlEncode(name, requestEncoding));
			writer.Write("=");
			writer.Write(HttpUtility.UrlEncode(MimeFormatter.ObjToString(value), requestEncoding));
		}
		else
		{
			writer.Write(HttpUtility.UrlEncode(name));
			writer.Write("=");
			writer.Write(HttpUtility.UrlEncode(MimeFormatter.ObjToString(value)));
		}
	}

	public override object GetInitializer(LogicalMethodInfo methodInfo)
	{
		if (methodInfo.OutParameters.Length != 0)
		{
			return null;
		}
		return methodInfo.Parameters;
	}

	public override void Initialize(object initializer)
	{
		parameters = (ParameterInfo[])initializer;
	}
}
