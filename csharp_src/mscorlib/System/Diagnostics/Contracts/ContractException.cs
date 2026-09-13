using System.Runtime.Serialization;
using System.Security;

namespace System.Diagnostics.Contracts;

[Serializable]
internal sealed class ContractException : Exception
{
	private readonly ContractFailureKind _Kind;

	private readonly string _UserMessage;

	private readonly string _Condition;

	public ContractFailureKind Kind => _Kind;

	public string Failure => Message;

	public string UserMessage => _UserMessage;

	public string Condition => _Condition;

	private ContractException()
	{
		base.HResult = -2146233022;
	}

	public ContractException(ContractFailureKind kind, string failure, string userMessage, string condition, Exception innerException)
		: base(failure, innerException)
	{
		base.HResult = -2146233022;
		_Kind = kind;
		_UserMessage = userMessage;
		_Condition = condition;
	}

	private ContractException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		_Kind = (ContractFailureKind)info.GetInt32("Kind");
		_UserMessage = info.GetString("UserMessage");
		_Condition = info.GetString("Condition");
	}

	[SecurityCritical]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("Kind", _Kind);
		info.AddValue("UserMessage", _UserMessage);
		info.AddValue("Condition", _Condition);
	}
}
