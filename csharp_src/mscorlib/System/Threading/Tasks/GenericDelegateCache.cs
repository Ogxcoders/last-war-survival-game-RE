namespace System.Threading.Tasks;

internal static class GenericDelegateCache<TAntecedentResult, TResult>
{
	internal static Func<Task<Task>, object, TResult> CWAnyFuncDelegate = delegate(Task<Task> wrappedWinner, object state)
	{
		Func<Task<TAntecedentResult>, TResult> obj = (Func<Task<TAntecedentResult>, TResult>)state;
		Task<TAntecedentResult> arg = (Task<TAntecedentResult>)wrappedWinner.Result;
		return obj(arg);
	};

	internal static Func<Task<Task>, object, TResult> CWAnyActionDelegate = delegate(Task<Task> wrappedWinner, object state)
	{
		Action<Task<TAntecedentResult>> obj = (Action<Task<TAntecedentResult>>)state;
		Task<TAntecedentResult> obj2 = (Task<TAntecedentResult>)wrappedWinner.Result;
		obj(obj2);
		return default(TResult);
	};

	internal static Func<Task<Task<TAntecedentResult>[]>, object, TResult> CWAllFuncDelegate = delegate(Task<Task<TAntecedentResult>[]> wrappedAntecedents, object state)
	{
		wrappedAntecedents.NotifyDebuggerOfWaitCompletionIfNecessary();
		return ((Func<Task<TAntecedentResult>[], TResult>)state)(wrappedAntecedents.Result);
	};

	internal static Func<Task<Task<TAntecedentResult>[]>, object, TResult> CWAllActionDelegate = delegate(Task<Task<TAntecedentResult>[]> wrappedAntecedents, object state)
	{
		wrappedAntecedents.NotifyDebuggerOfWaitCompletionIfNecessary();
		((Action<Task<TAntecedentResult>[]>)state)(wrappedAntecedents.Result);
		return default(TResult);
	};
}
