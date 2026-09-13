using System.Collections;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging;

[Serializable]
internal class ErrorMessage : IMethodCallMessage, IMethodMessage, IMessage
{
	private string _uri = "Exception";

	public int ArgCount => 0;

	public object[] Args => null;

	public bool HasVarArgs => false;

	public MethodBase MethodBase => null;

	public string MethodName => "unknown";

	public object MethodSignature => null;

	public virtual IDictionary Properties => null;

	public string TypeName => "unknown";

	public string Uri
	{
		get
		{
			return _uri;
		}
		set
		{
			_uri = value;
		}
	}

	public int InArgCount => 0;

	public object[] InArgs => null;

	public LogicalCallContext LogicalCallContext => null;

	public object GetArg(int arg_num)
	{
		return null;
	}

	public string GetArgName(int arg_num)
	{
		return "unknown";
	}

	public string GetInArgName(int index)
	{
		return null;
	}

	public object GetInArg(int argNum)
	{
		return null;
	}
}
