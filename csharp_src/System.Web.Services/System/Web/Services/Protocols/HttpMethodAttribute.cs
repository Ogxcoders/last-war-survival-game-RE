namespace System.Web.Services.Protocols;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public sealed class HttpMethodAttribute : Attribute
{
	private Type parameterFormatter;

	private Type returnFormatter;

	public Type ParameterFormatter
	{
		get
		{
			return parameterFormatter;
		}
		set
		{
			parameterFormatter = value;
		}
	}

	public Type ReturnFormatter
	{
		get
		{
			return returnFormatter;
		}
		set
		{
			returnFormatter = value;
		}
	}

	public HttpMethodAttribute()
	{
	}

	public HttpMethodAttribute(Type returnFormatter, Type parameterFormatter)
		: this()
	{
		this.parameterFormatter = parameterFormatter;
		this.returnFormatter = returnFormatter;
	}
}
