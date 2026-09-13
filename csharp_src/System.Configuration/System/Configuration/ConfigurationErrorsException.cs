using System.Collections;
using System.Runtime.Serialization;
using System.Xml;
using Unity;

namespace System.Configuration;

[Serializable]
public class ConfigurationErrorsException : ConfigurationException
{
	public override string BareMessage
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public ICollection Errors
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public override string Filename
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}
	}

	public override int Line
	{
		get
		{
			ThrowStub.ThrowNotSupportedException();
			return default(int);
		}
	}

	public ConfigurationErrorsException()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected ConfigurationErrorsException(SerializationInfo info, StreamingContext context)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, Exception inner)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, Exception inner, string filename, int line)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, Exception inner, XmlNode node)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, Exception inner, XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, string filename, int line)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, XmlNode node)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ConfigurationErrorsException(string message, XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public static string GetFilename(XmlNode node)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static string GetFilename(XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
		return null;
	}

	public static int GetLineNumber(XmlNode node)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(int);
	}

	public static int GetLineNumber(XmlReader reader)
	{
		ThrowStub.ThrowNotSupportedException();
		return default(int);
	}
}
