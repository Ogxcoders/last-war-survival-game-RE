using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

[Serializable]
internal sealed class BinaryMethodCallMessage
{
	private object[] _inargs;

	private string _methodName;

	private string _typeName;

	private object _methodSignature;

	private Type[] _instArgs;

	private object[] _args;

	[SecurityCritical]
	private LogicalCallContext _logicalCallContext;

	private object[] _properties;

	public string MethodName => _methodName;

	public string TypeName => _typeName;

	public Type[] InstantiationArgs => _instArgs;

	public object MethodSignature => _methodSignature;

	public object[] Args => _args;

	public LogicalCallContext LogicalCallContext
	{
		[SecurityCritical]
		get
		{
			return _logicalCallContext;
		}
	}

	public bool HasProperties => _properties != null;

	[SecurityCritical]
	internal BinaryMethodCallMessage(string uri, string methodName, string typeName, Type[] instArgs, object[] args, object methodSignature, LogicalCallContext callContext, object[] properties)
	{
		_methodName = methodName;
		_typeName = typeName;
		if (args == null)
		{
			args = new object[0];
		}
		_inargs = args;
		_args = args;
		_instArgs = instArgs;
		_methodSignature = methodSignature;
		if (callContext == null)
		{
			_logicalCallContext = new LogicalCallContext();
		}
		else
		{
			_logicalCallContext = callContext;
		}
		_properties = properties;
	}

	internal void PopulateMessageProperties(IDictionary dict)
	{
		object[] properties = _properties;
		for (int i = 0; i < properties.Length; i++)
		{
			DictionaryEntry dictionaryEntry = (DictionaryEntry)properties[i];
			dict[dictionaryEntry.Key] = dictionaryEntry.Value;
		}
	}
}
