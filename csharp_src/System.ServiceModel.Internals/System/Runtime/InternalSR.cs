namespace System.Runtime;

internal static class InternalSR
{
	public const string ActionItemIsAlreadyScheduled = "Action Item Is Already Scheduled";

	public const string AsyncCallbackThrewException = "Async Callback Threw Exception";

	public const string AsyncResultAlreadyEnded = "Async Result Already Ended";

	public const string BadCopyToArray = "Bad Copy To Array";

	public const string BufferIsNotRightSizeForBufferManager = "Buffer Is Not Right Size For Buffer Manager";

	public const string DictionaryIsReadOnly = "Dictionary Is Read Only";

	public const string InvalidAsyncResult = "Invalid Async Result";

	public const string InvalidAsyncResultImplementationGeneric = "Invalid Async Result Implementation Generic";

	public const string InvalidNullAsyncResult = "Invalid Null Async Result";

	public const string InvalidSemaphoreExit = "Invalid Semaphore Exit";

	public const string KeyCollectionUpdatesNotAllowed = "Key Collection Updates Not Allowed";

	public const string KeyNotFoundInDictionary = "Key Not Found In Dictionary";

	public const string MustCancelOldTimer = "Must Cancel Old Timer";

	public const string NullKeyAlreadyPresent = "Null Key Already Present";

	public const string ReadNotSupported = "Read Not Supported";

	public const string SFxTaskNotStarted = "SFx Task Not Started";

	public const string SeekNotSupported = "Seek Not Supported";

	public const string ThreadNeutralSemaphoreAborted = "Thread Neutral Semaphore Aborted";

	public const string ValueCollectionUpdatesNotAllowed = "Value Collection Updates Not Allowed";

	public const string ValueMustBeNonNegative = "Value Must Be Non Negative";

	public static string ArgumentNullOrEmpty(string paramName)
	{
		return string.Format("{0} is null or empty");
	}

	public static string AsyncEventArgsCompletedTwice(Type t)
	{
		return $"AsyncEventArgs completed twice for {t}";
	}

	public static string AsyncEventArgsCompletionPending(Type t)
	{
		return $"AsyncEventArgs completion pending for {t}";
	}

	public static string BufferAllocationFailed(int size)
	{
		return $"Buffer allocation of size {size} failed";
	}

	public static string BufferedOutputStreamQuotaExceeded(int maxSizeQuota)
	{
		return $"Buffered output stream quota exceeded (maxSizeQuota={maxSizeQuota})";
	}

	public static string CannotConvertObject(object source, Type t)
	{
		return $"Cannot convert object {source} to {t}";
	}

	public static string EtwAPIMaxStringCountExceeded(object max)
	{
		return $"ETW API max string count exceeded {max}";
	}

	public static string EtwMaxNumberArgumentsExceeded(object max)
	{
		return $"ETW max number arguments exceeded {max}";
	}

	public static string EtwRegistrationFailed(object arg)
	{
		return $"ETW registration failed {arg}";
	}

	public static string FailFastMessage(string description)
	{
		return $"Fail fast: {description}";
	}

	public static string InvalidAsyncResultImplementation(Type t)
	{
		return $"Invalid AsyncResult implementation: {t}";
	}

	public static string LockTimeoutExceptionMessage(object timeout)
	{
		return $"Lock timeout {timeout}";
	}

	public static string ShipAssertExceptionMessage(object description)
	{
		return $"Ship assert exception {description}";
	}

	public static string TaskTimedOutError(object timeout)
	{
		return $"Task timed out error {timeout}";
	}

	public static string TimeoutInputQueueDequeue(object timeout)
	{
		return $"Timeout input queue dequeue {timeout}";
	}

	public static string TimeoutMustBeNonNegative(object argumentName, object timeout)
	{
		return $"Timeout must be non-negative {argumentName} and {timeout}";
	}

	public static string TimeoutMustBePositive(string argumentName, object timeout)
	{
		return $"Timeout must be positive {argumentName} {timeout}";
	}

	public static string TimeoutOnOperation(object timeout)
	{
		return $"Timeout on operation {timeout}";
	}

	public static string AsyncResultCompletedTwice(Type t)
	{
		return $"AsyncResult Completed Twice for {t}";
	}
}
