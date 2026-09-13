using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary;

[Serializable]
internal class BinaryMethodReturnMessage
{
	private object[] _outargs;

	private Exception _exception;

	private object _returnValue;

	private object[] _args;

	[SecurityCritical]
	private LogicalCallContext _logicalCallContext;

	private object[] _properties;

	public Exception Exception => _exception;

	public object ReturnValue => _returnValue;

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
	internal BinaryMethodReturnMessage(object returnValue, object[] args, Exception e, LogicalCallContext callContext, object[] properties)
	{
		_returnValue = returnValue;
		if (args == null)
		{
			args = new object[0];
		}
		_outargs = args;
		_args = args;
		_exception = e;
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
