using System.Web.Services.Description;

namespace System.Web.Services.Protocols;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class SoapDocumentMethodAttribute : Attribute
{
	private string action;

	private string binding;

	private bool oneWay;

	private SoapParameterStyle parameterStyle;

	private string requestElementName;

	private string requestNamespace;

	private string responseElementName;

	private string responseNamespace;

	private SoapBindingUse use;

	public string Action
	{
		get
		{
			return action;
		}
		set
		{
			action = value;
		}
	}

	public string Binding
	{
		get
		{
			if (binding != null)
			{
				return binding;
			}
			return "";
		}
		set
		{
			binding = value;
		}
	}

	public bool OneWay
	{
		get
		{
			return oneWay;
		}
		set
		{
			oneWay = value;
		}
	}

	public SoapParameterStyle ParameterStyle
	{
		get
		{
			return parameterStyle;
		}
		set
		{
			parameterStyle = value;
		}
	}

	public string RequestElementName
	{
		get
		{
			if (requestElementName == null)
			{
				return "";
			}
			return requestElementName;
		}
		set
		{
			requestElementName = value;
		}
	}

	public string RequestNamespace
	{
		get
		{
			if (requestNamespace == null)
			{
				return "";
			}
			return requestNamespace;
		}
		set
		{
			requestNamespace = value;
		}
	}

	public string ResponseElementName
	{
		get
		{
			if (responseElementName == null)
			{
				return "";
			}
			return responseElementName;
		}
		set
		{
			responseElementName = value;
		}
	}

	public string ResponseNamespace
	{
		get
		{
			if (responseNamespace == null)
			{
				return "";
			}
			return responseNamespace;
		}
		set
		{
			responseNamespace = value;
		}
	}

	public SoapBindingUse Use
	{
		get
		{
			return use;
		}
		set
		{
			use = value;
		}
	}

	public SoapDocumentMethodAttribute()
	{
	}

	public SoapDocumentMethodAttribute(string action)
		: this()
	{
		this.action = action;
	}
}
