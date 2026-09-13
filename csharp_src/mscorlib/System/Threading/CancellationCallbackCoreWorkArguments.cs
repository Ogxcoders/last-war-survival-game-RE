namespace System.Threading;

internal struct CancellationCallbackCoreWorkArguments
{
	internal SparselyPopulatedArrayFragment<CancellationCallbackInfo> _currArrayFragment;

	internal int _currArrayIndex;

	public CancellationCallbackCoreWorkArguments(SparselyPopulatedArrayFragment<CancellationCallbackInfo> currArrayFragment, int currArrayIndex)
	{
		_currArrayFragment = currArrayFragment;
		_currArrayIndex = currArrayIndex;
	}
}
