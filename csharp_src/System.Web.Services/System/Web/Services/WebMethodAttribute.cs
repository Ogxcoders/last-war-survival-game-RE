namespace System.Web.Services;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class WebMethodAttribute : Attribute
{
	private bool bufferResponse;

	private int cacheDuration;

	private string description;

	private bool enableSession;

	private string messageName;

	private TransactionOption transactionOption;

	public bool BufferResponse
	{
		get
		{
			return bufferResponse;
		}
		set
		{
			bufferResponse = value;
		}
	}

	public int CacheDuration
	{
		get
		{
			return cacheDuration;
		}
		set
		{
			cacheDuration = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public bool EnableSession
	{
		get
		{
			return enableSession;
		}
		set
		{
			enableSession = value;
		}
	}

	public string MessageName
	{
		get
		{
			return messageName;
		}
		set
		{
			messageName = value;
		}
	}

	public TransactionOption TransactionOption
	{
		get
		{
			return transactionOption;
		}
		set
		{
			transactionOption = value;
		}
	}

	public WebMethodAttribute()
		: this(enableSession: false, TransactionOption.Disabled, 0, bufferResponse: true)
	{
	}

	public WebMethodAttribute(bool enableSession)
		: this(enableSession, TransactionOption.Disabled, 0, bufferResponse: true)
	{
	}

	public WebMethodAttribute(bool enableSession, TransactionOption transactionOption)
		: this(enableSession, transactionOption, 0, bufferResponse: true)
	{
	}

	public WebMethodAttribute(bool enableSession, TransactionOption transactionOption, int cacheDuration)
		: this(enableSession, transactionOption, cacheDuration, bufferResponse: true)
	{
	}

	public WebMethodAttribute(bool enableSession, TransactionOption transactionOption, int cacheDuration, bool bufferResponse)
	{
		this.bufferResponse = bufferResponse;
		this.cacheDuration = cacheDuration;
		this.enableSession = enableSession;
		this.transactionOption = transactionOption;
		description = string.Empty;
		messageName = string.Empty;
	}
}
