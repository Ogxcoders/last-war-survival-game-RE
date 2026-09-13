using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Diagnostics.Contracts;

public sealed class ContractFailedEventArgs : EventArgs
{
	private ContractFailureKind _failureKind;

	private string _message;

	private string _condition;

	private Exception _originalException;

	private bool _handled;

	private bool _unwind;

	internal Exception thrownDuringHandler;

	public string Message => _message;

	public string Condition => _condition;

	public ContractFailureKind FailureKind => _failureKind;

	public Exception OriginalException => _originalException;

	public bool Handled => _handled;

	public bool Unwind => _unwind;

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public ContractFailedEventArgs(ContractFailureKind failureKind, string message, string condition, Exception originalException)
	{
		_failureKind = failureKind;
		_message = message;
		_condition = condition;
		_originalException = originalException;
	}

	[SecurityCritical]
	public void SetHandled()
	{
		_handled = true;
	}

	[SecurityCritical]
	public void SetUnwind()
	{
		_unwind = true;
	}
}
