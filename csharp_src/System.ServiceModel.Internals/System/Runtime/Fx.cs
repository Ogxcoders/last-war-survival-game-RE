using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Diagnostics;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;

namespace System.Runtime;

internal static class Fx
{
	public abstract class ExceptionHandler
	{
		public abstract bool HandleException(Exception exception);
	}

	public static class Tag
	{
		public enum CacheAttrition
		{
			None,
			ElementOnTimer,
			ElementOnGC,
			ElementOnCallback,
			FullPurgeOnTimer,
			FullPurgeOnEachAccess,
			PartialPurgeOnTimer,
			PartialPurgeOnEachAccess
		}

		public enum ThrottleAction
		{
			Reject,
			Pause
		}

		public enum ThrottleMetric
		{
			Count,
			Rate,
			Other
		}

		public enum Location
		{
			InProcess,
			OutOfProcess,
			LocalSystem,
			LocalOrRemoteSystem,
			RemoteSystem
		}

		public enum SynchronizationKind
		{
			LockStatement,
			MonitorWait,
			MonitorExplicit,
			InterlockedNoSpin,
			InterlockedWithSpin,
			FromFieldType
		}

		[Flags]
		public enum BlocksUsing
		{
			MonitorEnter = 0,
			MonitorWait = 1,
			ManualResetEvent = 2,
			AutoResetEvent = 3,
			AsyncResult = 4,
			IAsyncResult = 5,
			PInvoke = 6,
			InputQueue = 7,
			ThreadNeutralSemaphore = 8,
			PrivatePrimitive = 9,
			OtherInternalPrimitive = 0xA,
			OtherFrameworkPrimitive = 0xB,
			OtherInterop = 0xC,
			Other = 0xD,
			NonBlocking = 0xE
		}

		public static class Strings
		{
			internal const string ExternallyManaged = "externally managed";

			internal const string AppDomain = "AppDomain";

			internal const string DeclaringInstance = "instance of declaring class";

			internal const string Unbounded = "unbounded";

			internal const string Infinite = "infinite";
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
		[Conditional("DEBUG")]
		public sealed class FriendAccessAllowedAttribute : Attribute
		{
			public string AssemblyName { get; set; }

			public FriendAccessAllowedAttribute(string assemblyName)
			{
				AssemblyName = assemblyName;
			}
		}

		public static class Throws
		{
			[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
			[Conditional("CODE_ANALYSIS_CDF")]
			public sealed class TimeoutAttribute : ThrowsAttribute
			{
				public TimeoutAttribute()
					: this("The operation timed out.")
				{
				}

				public TimeoutAttribute(string diagnosis)
					: base(typeof(TimeoutException), diagnosis)
				{
				}
			}
		}

		[AttributeUsage(AttributeTargets.Field)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class CacheAttribute : Attribute
		{
			private readonly Type elementType;

			private readonly CacheAttrition cacheAttrition;

			public Type ElementType => elementType;

			public CacheAttrition CacheAttrition => cacheAttrition;

			public string Scope { get; set; }

			public string SizeLimit { get; set; }

			public string Timeout { get; set; }

			public CacheAttribute(Type elementType, CacheAttrition cacheAttrition)
			{
				Scope = "instance of declaring class";
				SizeLimit = "unbounded";
				Timeout = "infinite";
				if (elementType == null)
				{
					throw Exception.ArgumentNull("elementType");
				}
				this.elementType = elementType;
				this.cacheAttrition = cacheAttrition;
			}
		}

		[AttributeUsage(AttributeTargets.Field)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class QueueAttribute : Attribute
		{
			private readonly Type elementType;

			public Type ElementType => elementType;

			public string Scope { get; set; }

			public string SizeLimit { get; set; }

			public bool StaleElementsRemovedImmediately { get; set; }

			public bool EnqueueThrowsIfFull { get; set; }

			public QueueAttribute(Type elementType)
			{
				Scope = "instance of declaring class";
				SizeLimit = "unbounded";
				if (elementType == null)
				{
					throw Exception.ArgumentNull("elementType");
				}
				this.elementType = elementType;
			}
		}

		[AttributeUsage(AttributeTargets.Field)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class ThrottleAttribute : Attribute
		{
			private readonly ThrottleAction throttleAction;

			private readonly ThrottleMetric throttleMetric;

			private readonly string limit;

			public ThrottleAction ThrottleAction => throttleAction;

			public ThrottleMetric ThrottleMetric => throttleMetric;

			public string Limit => limit;

			public string Scope { get; set; }

			public ThrottleAttribute(ThrottleAction throttleAction, ThrottleMetric throttleMetric, string limit)
			{
				Scope = "AppDomain";
				if (string.IsNullOrEmpty(limit))
				{
					throw Exception.ArgumentNullOrEmpty("limit");
				}
				this.throttleAction = throttleAction;
				this.throttleMetric = throttleMetric;
				this.limit = limit;
			}
		}

		[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class ExternalResourceAttribute : Attribute
		{
			private readonly Location location;

			private readonly string description;

			public Location Location => location;

			public string Description => description;

			public ExternalResourceAttribute(Location location, string description)
			{
				this.location = location;
				this.description = description;
			}
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class SynchronizationObjectAttribute : Attribute
		{
			public bool Blocking { get; set; }

			public string Scope { get; set; }

			public SynchronizationKind Kind { get; set; }

			public SynchronizationObjectAttribute()
			{
				Blocking = true;
				Scope = "instance of declaring class";
				Kind = SynchronizationKind.FromFieldType;
			}
		}

		[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class SynchronizationPrimitiveAttribute : Attribute
		{
			private readonly BlocksUsing blocksUsing;

			public BlocksUsing BlocksUsing => blocksUsing;

			public bool SupportsAsync { get; set; }

			public bool Spins { get; set; }

			public string ReleaseMethod { get; set; }

			public SynchronizationPrimitiveAttribute(BlocksUsing blocksUsing)
			{
				this.blocksUsing = blocksUsing;
			}
		}

		[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class BlockingAttribute : Attribute
		{
			public string CancelMethod { get; set; }

			public Type CancelDeclaringType { get; set; }

			public string Conditional { get; set; }
		}

		[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class GuaranteeNonBlockingAttribute : Attribute
		{
		}

		[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class NonThrowingAttribute : Attribute
		{
		}

		[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public class ThrowsAttribute : Attribute
		{
			private readonly Type exceptionType;

			private readonly string diagnosis;

			public Type ExceptionType => exceptionType;

			public string Diagnosis => diagnosis;

			public ThrowsAttribute(Type exceptionType, string diagnosis)
			{
				if (exceptionType == null)
				{
					throw Exception.ArgumentNull("exceptionType");
				}
				if (string.IsNullOrEmpty(diagnosis))
				{
					throw Exception.ArgumentNullOrEmpty("diagnosis");
				}
				this.exceptionType = exceptionType;
				this.diagnosis = diagnosis;
			}
		}

		[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class InheritThrowsAttribute : Attribute
		{
			public Type FromDeclaringType { get; set; }

			public string From { get; set; }
		}

		[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class KnownXamlExternalAttribute : Attribute
		{
		}

		[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class XamlVisibleAttribute : Attribute
		{
			public bool Visible { get; private set; }

			public XamlVisibleAttribute()
				: this(visible: true)
			{
			}

			public XamlVisibleAttribute(bool visible)
			{
				Visible = visible;
			}
		}

		[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
		[Conditional("CODE_ANALYSIS_CDF")]
		public sealed class SecurityNoteAttribute : Attribute
		{
			public string Critical { get; set; }

			public string Safe { get; set; }

			public string Miscellaneous { get; set; }
		}
	}

	private abstract class Thunk<T> where T : class
	{
		[SecurityCritical]
		private T callback;

		internal T Callback
		{
			[SecuritySafeCritical]
			get
			{
				return callback;
			}
		}

		[SecuritySafeCritical]
		protected Thunk(T callback)
		{
			this.callback = callback;
		}
	}

	private sealed class ActionThunk<T1> : Thunk<Action<T1>>
	{
		public Action<T1> ThunkFrame => UnhandledExceptionFrame;

		public ActionThunk(Action<T1> callback)
			: base(callback)
		{
		}

		[SecuritySafeCritical]
		private void UnhandledExceptionFrame(T1 result)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.Callback(result);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	private sealed class AsyncThunk : Thunk<AsyncCallback>
	{
		public AsyncCallback ThunkFrame => UnhandledExceptionFrame;

		public AsyncThunk(AsyncCallback callback)
			: base(callback)
		{
		}

		[SecuritySafeCritical]
		private void UnhandledExceptionFrame(IAsyncResult result)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.Callback(result);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	private sealed class WaitThunk : Thunk<WaitCallback>
	{
		public WaitCallback ThunkFrame => UnhandledExceptionFrame;

		public WaitThunk(WaitCallback callback)
			: base(callback)
		{
		}

		[SecuritySafeCritical]
		private void UnhandledExceptionFrame(object state)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.Callback(state);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	private sealed class TimerThunk : Thunk<TimerCallback>
	{
		public TimerCallback ThunkFrame => UnhandledExceptionFrame;

		public TimerThunk(TimerCallback callback)
			: base(callback)
		{
		}

		[SecuritySafeCritical]
		private void UnhandledExceptionFrame(object state)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.Callback(state);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	private sealed class WaitOrTimerThunk : Thunk<WaitOrTimerCallback>
	{
		public WaitOrTimerCallback ThunkFrame => UnhandledExceptionFrame;

		public WaitOrTimerThunk(WaitOrTimerCallback callback)
			: base(callback)
		{
		}

		[SecuritySafeCritical]
		private void UnhandledExceptionFrame(object state, bool timedOut)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.Callback(state, timedOut);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	private sealed class SendOrPostThunk : Thunk<SendOrPostCallback>
	{
		public SendOrPostCallback ThunkFrame => UnhandledExceptionFrame;

		public SendOrPostThunk(SendOrPostCallback callback)
			: base(callback)
		{
		}

		[SecuritySafeCritical]
		private void UnhandledExceptionFrame(object state)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.Callback(state);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	[SecurityCritical]
	private sealed class IOCompletionThunk
	{
		private IOCompletionCallback callback;

		public unsafe IOCompletionCallback ThunkFrame => UnhandledExceptionFrame;

		public IOCompletionThunk(IOCompletionCallback callback)
		{
			this.callback = callback;
		}

		private unsafe void UnhandledExceptionFrame(uint error, uint bytesRead, NativeOverlapped* nativeOverlapped)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				callback(error, bytesRead, nativeOverlapped);
			}
			catch (Exception exception)
			{
				if (!HandleAtThreadBase(exception))
				{
					throw;
				}
			}
		}
	}

	[Serializable]
	private class InternalException : SystemException
	{
		public InternalException(string description)
			: base(InternalSR.ShipAssertExceptionMessage(description))
		{
		}

		protected InternalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	[Serializable]
	private class FatalInternalException : InternalException
	{
		public FatalInternalException(string description)
			: base(description)
		{
		}

		protected FatalInternalException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	private const string defaultEventSource = "System.Runtime";

	private static ExceptionTrace exceptionTrace;

	private static EtwDiagnosticTrace diagnosticTrace;

	[SecurityCritical]
	private static ExceptionHandler asynchronousThreadExceptionHandler;

	public static ExceptionTrace Exception
	{
		get
		{
			if (exceptionTrace == null)
			{
				exceptionTrace = new ExceptionTrace("System.Runtime", Trace);
			}
			return exceptionTrace;
		}
	}

	public static EtwDiagnosticTrace Trace
	{
		get
		{
			if (diagnosticTrace == null)
			{
				diagnosticTrace = InitializeTracing();
			}
			return diagnosticTrace;
		}
	}

	public static ExceptionHandler AsynchronousThreadExceptionHandler
	{
		[SecuritySafeCritical]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		get
		{
			return asynchronousThreadExceptionHandler;
		}
		[SecurityCritical]
		set
		{
			asynchronousThreadExceptionHandler = value;
		}
	}

	internal static bool AssertsFailFast => false;

	internal static Type[] BreakOnExceptionTypes => null;

	internal static bool FastDebug => false;

	internal static bool StealthDebugger => false;

	[SecuritySafeCritical]
	private static EtwDiagnosticTrace InitializeTracing()
	{
		EtwDiagnosticTrace etwDiagnosticTrace = new EtwDiagnosticTrace("System.Runtime", EtwDiagnosticTrace.DefaultEtwProviderId);
		if (etwDiagnosticTrace.EtwProvider != null)
		{
			etwDiagnosticTrace.RefreshState = (Action)Delegate.Combine(etwDiagnosticTrace.RefreshState, (Action)delegate
			{
				UpdateLevel();
			});
		}
		UpdateLevel(etwDiagnosticTrace);
		return etwDiagnosticTrace;
	}

	[Conditional("DEBUG")]
	public static void Assert(bool condition, string description)
	{
	}

	[Conditional("DEBUG")]
	public static void Assert(string description)
	{
		AssertHelper.FireAssert(description);
	}

	public static void AssertAndThrow(bool condition, string description)
	{
		if (!condition)
		{
			AssertAndThrow(description);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Exception AssertAndThrow(string description)
	{
		TraceCore.ShipAssertExceptionMessage(Trace, description);
		throw new InternalException(description);
	}

	public static void AssertAndThrowFatal(bool condition, string description)
	{
		if (!condition)
		{
			AssertAndThrowFatal(description);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Exception AssertAndThrowFatal(string description)
	{
		TraceCore.ShipAssertExceptionMessage(Trace, description);
		throw new FatalInternalException(description);
	}

	public static void AssertAndFailFast(bool condition, string description)
	{
		if (!condition)
		{
			AssertAndFailFast(description);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	public static Exception AssertAndFailFast(string description)
	{
		string message = InternalSR.FailFastMessage(description);
		try
		{
			try
			{
				Exception.TraceFailFast(message);
			}
			finally
			{
				Environment.FailFast(message);
			}
		}
		catch
		{
			throw;
		}
		return null;
	}

	public static bool IsFatal(Exception exception)
	{
		while (exception != null)
		{
			if (exception is FatalException || (exception is OutOfMemoryException && !(exception is InsufficientMemoryException)) || exception is ThreadAbortException || exception is FatalInternalException)
			{
				return true;
			}
			if (exception is TypeInitializationException || exception is TargetInvocationException)
			{
				exception = exception.InnerException;
				continue;
			}
			if (!(exception is AggregateException))
			{
				break;
			}
			foreach (Exception innerException in ((AggregateException)exception).InnerExceptions)
			{
				if (IsFatal(innerException))
				{
					return true;
				}
			}
			break;
		}
		return false;
	}

	public static Action<T1> ThunkCallback<T1>(Action<T1> callback)
	{
		return new ActionThunk<T1>(callback).ThunkFrame;
	}

	public static AsyncCallback ThunkCallback(AsyncCallback callback)
	{
		return new AsyncThunk(callback).ThunkFrame;
	}

	public static WaitCallback ThunkCallback(WaitCallback callback)
	{
		return new WaitThunk(callback).ThunkFrame;
	}

	public static TimerCallback ThunkCallback(TimerCallback callback)
	{
		return new TimerThunk(callback).ThunkFrame;
	}

	public static WaitOrTimerCallback ThunkCallback(WaitOrTimerCallback callback)
	{
		return new WaitOrTimerThunk(callback).ThunkFrame;
	}

	public static SendOrPostCallback ThunkCallback(SendOrPostCallback callback)
	{
		return new SendOrPostThunk(callback).ThunkFrame;
	}

	[SecurityCritical]
	public static IOCompletionCallback ThunkCallback(IOCompletionCallback callback)
	{
		return new IOCompletionThunk(callback).ThunkFrame;
	}

	public static Guid CreateGuid(string guidString)
	{
		bool flag = false;
		Guid empty = Guid.Empty;
		try
		{
			empty = new Guid(guidString);
			flag = true;
			return empty;
		}
		finally
		{
			if (!flag)
			{
				AssertAndThrow("Creation of the Guid failed.");
			}
		}
	}

	public static bool TryCreateGuid(string guidString, out Guid result)
	{
		bool result2 = false;
		result = Guid.Empty;
		try
		{
			result = new Guid(guidString);
			result2 = true;
		}
		catch (ArgumentException)
		{
		}
		catch (FormatException)
		{
		}
		catch (OverflowException)
		{
		}
		return result2;
	}

	public static byte[] AllocateByteArray(int size)
	{
		try
		{
			return new byte[size];
		}
		catch (OutOfMemoryException innerException)
		{
			throw Exception.AsError(new InsufficientMemoryException(InternalSR.BufferAllocationFailed(size), innerException));
		}
	}

	public static char[] AllocateCharArray(int size)
	{
		try
		{
			return new char[size];
		}
		catch (OutOfMemoryException innerException)
		{
			throw Exception.AsError(new InsufficientMemoryException(InternalSR.BufferAllocationFailed(size * 2), innerException));
		}
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	private static void TraceExceptionNoThrow(Exception exception)
	{
		try
		{
			Exception.TraceUnhandledException(exception);
		}
		catch
		{
		}
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	private static bool HandleAtThreadBase(Exception exception)
	{
		if (exception == null)
		{
			return false;
		}
		TraceExceptionNoThrow(exception);
		try
		{
			return AsynchronousThreadExceptionHandler?.HandleException(exception) ?? false;
		}
		catch (Exception exception2)
		{
			TraceExceptionNoThrow(exception2);
		}
		return false;
	}

	private static void UpdateLevel(EtwDiagnosticTrace trace)
	{
		if (trace != null && (TraceCore.ActionItemCallbackInvokedIsEnabled(trace) || TraceCore.ActionItemScheduledIsEnabled(trace)))
		{
			trace.SetEnd2EndActivityTracingEnabled(isEnd2EndTracingEnabled: true);
		}
	}

	private static void UpdateLevel()
	{
		UpdateLevel(Trace);
	}
}
