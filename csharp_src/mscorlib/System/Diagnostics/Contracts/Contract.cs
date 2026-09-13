using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System.Diagnostics.Contracts;

public static class Contract
{
	[ThreadStatic]
	private static bool _assertingMustUseRewriter;

	public static event EventHandler<ContractFailedEventArgs> ContractFailed
	{
		[SecurityCritical]
		add
		{
			ContractHelper.InternalContractFailed += value;
		}
		[SecurityCritical]
		remove
		{
			ContractHelper.InternalContractFailed -= value;
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assume(bool condition)
	{
		if (!condition)
		{
			ReportFailure(ContractFailureKind.Assume, null, null, null);
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assume(bool condition, string userMessage)
	{
		if (!condition)
		{
			ReportFailure(ContractFailureKind.Assume, userMessage, null, null);
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assert(bool condition)
	{
		if (!condition)
		{
			ReportFailure(ContractFailureKind.Assert, null, null, null);
		}
	}

	[Conditional("DEBUG")]
	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Assert(bool condition, string userMessage)
	{
		if (!condition)
		{
			ReportFailure(ContractFailureKind.Assert, userMessage, null, null);
		}
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires(bool condition)
	{
		AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires(bool condition, string userMessage)
	{
		AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires<TException>(bool condition) where TException : Exception
	{
		AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Requires<TException>(bool condition, string userMessage) where TException : Exception
	{
		AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Ensures(bool condition)
	{
		AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Ensures(bool condition, string userMessage)
	{
		AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void EnsuresOnThrow<TException>(bool condition) where TException : Exception
	{
		AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void EnsuresOnThrow<TException>(bool condition, string userMessage) where TException : Exception
	{
		AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static T Result<T>()
	{
		return default(T);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static T ValueAtReturn<T>(out T value)
	{
		value = default(T);
		return value;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static T OldValue<T>(T value)
	{
		return default(T);
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Invariant(bool condition)
	{
		AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static void Invariant(bool condition, string userMessage)
	{
		AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
	{
		if (fromInclusive > toExclusive)
		{
			throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		for (int i = fromInclusive; i < toExclusive; i++)
		{
			if (!predicate(i))
			{
				return false;
			}
		}
		return true;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		foreach (T item in collection)
		{
			if (!predicate(item))
			{
				return false;
			}
		}
		return true;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
	{
		if (fromInclusive > toExclusive)
		{
			throw new ArgumentException("fromInclusive must be less than or equal to toExclusive.");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		for (int i = fromInclusive; i < toExclusive; i++)
		{
			if (predicate(i))
			{
				return true;
			}
		}
		return false;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		if (predicate == null)
		{
			throw new ArgumentNullException("predicate");
		}
		foreach (T item in collection)
		{
			if (predicate(item))
			{
				return true;
			}
		}
		return false;
	}

	[Conditional("CONTRACTS_FULL")]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static void EndContractBlock()
	{
	}

	[DebuggerNonUserCode]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	private static void ReportFailure(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
	{
		if (failureKind < ContractFailureKind.Precondition || failureKind > ContractFailureKind.Assume)
		{
			throw new ArgumentException(Environment.GetResourceString("Illegal enum value: {0}.", failureKind), "failureKind");
		}
		string text = ContractHelper.RaiseContractFailedEvent(failureKind, userMessage, conditionText, innerException);
		if (text != null)
		{
			ContractHelper.TriggerFailure(failureKind, text, userMessage, conditionText, innerException);
		}
	}

	[SecuritySafeCritical]
	private static void AssertMustUseRewriter(ContractFailureKind kind, string contractKind)
	{
		if (_assertingMustUseRewriter)
		{
			System.Diagnostics.Assert.Fail("Asserting that we must use the rewriter went reentrant.", "Didn't rewrite this mscorlib?");
		}
		_assertingMustUseRewriter = true;
		Assembly assembly = typeof(Contract).Assembly;
		StackTrace stackTrace = new StackTrace();
		Assembly assembly2 = null;
		for (int i = 0; i < stackTrace.FrameCount; i++)
		{
			Assembly assembly3 = stackTrace.GetFrame(i).GetMethod().DeclaringType.Assembly;
			if (assembly3 != assembly)
			{
				assembly2 = assembly3;
				break;
			}
		}
		if (assembly2 == null)
		{
			assembly2 = assembly;
		}
		string name = assembly2.GetName().Name;
		ContractHelper.TriggerFailure(kind, Environment.GetResourceString("An assembly (probably \"{1}\") must be rewritten using the code contracts binary rewriter (CCRewrite) because it is calling Contract.{0} and the CONTRACTS_FULL symbol is defined.  Remove any explicit definitions of the CONTRACTS_FULL symbol from your project and rebuild.  CCRewrite can be downloaded from http://go.microsoft.com/fwlink/?LinkID=169180. \\r\\nAfter the rewriter is installed, it can be enabled in Visual Studio from the project's Properties page on the Code Contracts pane.  Ensure that \"Perform Runtime Contract Checking\" is enabled, which will define CONTRACTS_FULL.", contractKind, name), null, null, null);
		_assertingMustUseRewriter = false;
	}
}
