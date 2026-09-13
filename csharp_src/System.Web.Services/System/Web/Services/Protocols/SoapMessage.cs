using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Web.Services.Protocols;

public abstract class SoapMessage
{
	private string content_type = "text/xml";

	private string content_encoding;

	private SoapException exception;

	private SoapHeaderCollection headers;

	private SoapMessageStage stage;

	private Stream stream;

	private object[] inParameters;

	private object[] outParameters;

	private SoapProtocolVersion soapVersion;

	internal object[] InParameters
	{
		get
		{
			return inParameters;
		}
		set
		{
			inParameters = value;
		}
	}

	internal object[] OutParameters
	{
		get
		{
			return outParameters;
		}
		set
		{
			outParameters = value;
		}
	}

	public abstract string Action { get; }

	public string ContentType
	{
		get
		{
			return content_type;
		}
		set
		{
			content_type = value;
		}
	}

	public SoapException Exception
	{
		get
		{
			return exception;
		}
		set
		{
			exception = value;
		}
	}

	public SoapHeaderCollection Headers => headers;

	public abstract LogicalMethodInfo MethodInfo { get; }

	public abstract bool OneWay { get; }

	public SoapMessageStage Stage => stage;

	public Stream Stream => stream;

	public abstract string Url { get; }

	public string ContentEncoding
	{
		get
		{
			return content_encoding;
		}
		set
		{
			content_encoding = value;
		}
	}

	internal bool IsSoap12 => SoapVersion == SoapProtocolVersion.Soap12;

	[ComVisible(false)]
	[DefaultValue(SoapProtocolVersion.Default)]
	public virtual SoapProtocolVersion SoapVersion => soapVersion;

	internal Stream InternalStream
	{
		set
		{
			stream = value;
		}
	}

	internal SoapMessage()
	{
		headers = new SoapHeaderCollection();
	}

	internal SoapMessage(Stream stream, SoapException exception)
	{
		this.exception = exception;
		this.stream = stream;
		headers = new SoapHeaderCollection();
	}

	internal void SetStage(SoapMessageStage stage)
	{
		this.stage = stage;
	}

	protected abstract void EnsureInStage();

	protected abstract void EnsureOutStage();

	protected void EnsureStage(SoapMessageStage stage)
	{
		if ((stage & Stage) == 0)
		{
			throw new InvalidOperationException("The current SoapMessageStage is not the asserted stage or stages.");
		}
	}

	public object GetInParameterValue(int index)
	{
		return inParameters[index];
	}

	public object GetOutParameterValue(int index)
	{
		if (MethodInfo.IsVoid)
		{
			return outParameters[index];
		}
		return outParameters[index + 1];
	}

	public object GetReturnValue()
	{
		if (!MethodInfo.IsVoid && exception == null)
		{
			return outParameters[0];
		}
		return null;
	}

	internal void SetHeaders(SoapHeaderCollection headers)
	{
		this.headers = headers;
	}

	internal void SetException(SoapException ex)
	{
		exception = ex;
	}

	internal void CollectHeaders(object target, SoapHeaderMapping[] headers, SoapHeaderDirection direction)
	{
		Headers.Clear();
		foreach (SoapHeaderMapping soapHeaderMapping in headers)
		{
			if ((soapHeaderMapping.Direction & direction) != 0 && !soapHeaderMapping.Custom && soapHeaderMapping.GetHeaderValue(target) is SoapHeader header)
			{
				Headers.Add(header);
			}
		}
	}

	internal void UpdateHeaderValues(object target, SoapHeaderMapping[] headersInfo)
	{
		foreach (SoapHeader header in Headers)
		{
			SoapHeaderMapping soapHeaderMapping = FindHeader(headersInfo, header.GetType());
			if (soapHeaderMapping != null)
			{
				soapHeaderMapping.SetHeaderValue(target, header);
				header.DidUnderstand = !soapHeaderMapping.Custom;
			}
		}
	}

	private SoapHeaderMapping FindHeader(SoapHeaderMapping[] headersInfo, Type headerType)
	{
		SoapHeaderMapping result = null;
		foreach (SoapHeaderMapping soapHeaderMapping in headersInfo)
		{
			if (soapHeaderMapping.HeaderType == headerType)
			{
				return soapHeaderMapping;
			}
			if (soapHeaderMapping.Custom)
			{
				result = soapHeaderMapping;
			}
		}
		return result;
	}
}
