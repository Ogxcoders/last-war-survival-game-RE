using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices;

[StructLayout(LayoutKind.Auto)]
public struct AsyncIteratorMethodBuilder
{
	private AsyncTaskMethodBuilder _methodBuilder;

	internal object ObjectIdForDebugger => _methodBuilder.ObjectIdForDebugger;

	public static AsyncIteratorMethodBuilder Create()
	{
		return default(AsyncIteratorMethodBuilder);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveNext<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
	{
		AsyncMethodBuilderCore.Start(ref stateMachine);
	}

	public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
	{
		_methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
	}

	public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
	{
		_methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
	}

	public void Complete()
	{
		_methodBuilder.SetResult();
	}
}
