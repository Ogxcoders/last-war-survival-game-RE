using System.Collections.Generic;
using System.Threading;

namespace System.Linq.Parallel;

internal class HashJoinQueryOperatorEnumerator<TLeftInput, TLeftKey, TRightInput, THashKey, TOutput> : QueryOperatorEnumerator<TOutput, TLeftKey>
{
	private class Mutables
	{
		internal TLeftInput _currentLeft;

		internal TLeftKey _currentLeftKey;

		internal HashLookup<THashKey, Pair<TRightInput, ListChunk<TRightInput>>> _rightHashLookup;

		internal ListChunk<TRightInput> _currentRightMatches;

		internal int _currentRightMatchesIndex;

		internal int _outputLoopCount;
	}

	private readonly QueryOperatorEnumerator<Pair<TLeftInput, THashKey>, TLeftKey> _leftSource;

	private readonly QueryOperatorEnumerator<Pair<TRightInput, THashKey>, int> _rightSource;

	private readonly Func<TLeftInput, TRightInput, TOutput> _singleResultSelector;

	private readonly Func<TLeftInput, IEnumerable<TRightInput>, TOutput> _groupResultSelector;

	private readonly IEqualityComparer<THashKey> _keyComparer;

	private readonly CancellationToken _cancellationToken;

	private Mutables _mutables;

	internal HashJoinQueryOperatorEnumerator(QueryOperatorEnumerator<Pair<TLeftInput, THashKey>, TLeftKey> leftSource, QueryOperatorEnumerator<Pair<TRightInput, THashKey>, int> rightSource, Func<TLeftInput, TRightInput, TOutput> singleResultSelector, Func<TLeftInput, IEnumerable<TRightInput>, TOutput> groupResultSelector, IEqualityComparer<THashKey> keyComparer, CancellationToken cancellationToken)
	{
		_leftSource = leftSource;
		_rightSource = rightSource;
		_singleResultSelector = singleResultSelector;
		_groupResultSelector = groupResultSelector;
		_keyComparer = keyComparer;
		_cancellationToken = cancellationToken;
	}

	internal override bool MoveNext(ref TOutput currentElement, ref TLeftKey currentKey)
	{
		Mutables mutables = _mutables;
		if (mutables == null)
		{
			mutables = (_mutables = new Mutables());
			mutables._rightHashLookup = new HashLookup<THashKey, Pair<TRightInput, ListChunk<TRightInput>>>(_keyComparer);
			Pair<TRightInput, THashKey> currentElement2 = default(Pair<TRightInput, THashKey>);
			int currentKey2 = 0;
			int num = 0;
			while (_rightSource.MoveNext(ref currentElement2, ref currentKey2))
			{
				if ((num++ & 0x3F) == 0)
				{
					CancellationState.ThrowIfCanceled(_cancellationToken);
				}
				TRightInput first = currentElement2.First;
				THashKey second = currentElement2.Second;
				if (second == null)
				{
					continue;
				}
				Pair<TRightInput, ListChunk<TRightInput>> value = default(Pair<TRightInput, ListChunk<TRightInput>>);
				if (!mutables._rightHashLookup.TryGetValue(second, ref value))
				{
					value = new Pair<TRightInput, ListChunk<TRightInput>>(first, null);
					if (_groupResultSelector != null)
					{
						value.Second = new ListChunk<TRightInput>(2);
						value.Second.Add(first);
					}
					mutables._rightHashLookup.Add(second, value);
				}
				else
				{
					if (value.Second == null)
					{
						value.Second = new ListChunk<TRightInput>(2);
						mutables._rightHashLookup[second] = value;
					}
					value.Second.Add(first);
				}
			}
		}
		ListChunk<TRightInput> currentRightMatches = mutables._currentRightMatches;
		if (currentRightMatches != null && mutables._currentRightMatchesIndex == currentRightMatches.Count)
		{
			currentRightMatches = (mutables._currentRightMatches = currentRightMatches.Next);
			mutables._currentRightMatchesIndex = 0;
		}
		if (mutables._currentRightMatches == null)
		{
			Pair<TLeftInput, THashKey> currentElement3 = default(Pair<TLeftInput, THashKey>);
			TLeftKey currentKey3 = default(TLeftKey);
			while (_leftSource.MoveNext(ref currentElement3, ref currentKey3))
			{
				if ((mutables._outputLoopCount++ & 0x3F) == 0)
				{
					CancellationState.ThrowIfCanceled(_cancellationToken);
				}
				Pair<TRightInput, ListChunk<TRightInput>> value2 = default(Pair<TRightInput, ListChunk<TRightInput>>);
				TLeftInput first2 = currentElement3.First;
				THashKey second2 = currentElement3.Second;
				if (second2 != null && mutables._rightHashLookup.TryGetValue(second2, ref value2) && _singleResultSelector != null)
				{
					mutables._currentRightMatches = value2.Second;
					mutables._currentRightMatchesIndex = 0;
					currentElement = _singleResultSelector(first2, value2.First);
					currentKey = currentKey3;
					if (value2.Second != null)
					{
						mutables._currentLeft = first2;
						mutables._currentLeftKey = currentKey3;
					}
					return true;
				}
				if (_groupResultSelector != null)
				{
					IEnumerable<TRightInput> enumerable = value2.Second;
					if (enumerable == null)
					{
						enumerable = ParallelEnumerable.Empty<TRightInput>();
					}
					currentElement = _groupResultSelector(first2, enumerable);
					currentKey = currentKey3;
					return true;
				}
			}
			return false;
		}
		currentElement = _singleResultSelector(mutables._currentLeft, mutables._currentRightMatches._chunk[mutables._currentRightMatchesIndex]);
		currentKey = mutables._currentLeftKey;
		mutables._currentRightMatchesIndex++;
		return true;
	}

	protected override void Dispose(bool disposing)
	{
		_leftSource.Dispose();
		_rightSource.Dispose();
	}
}
