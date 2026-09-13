using System.Collections.Generic;

namespace System.Linq.Parallel;

internal struct ConcatKey<TLeftKey, TRightKey>
{
	private class ConcatKeyComparer : IComparer<ConcatKey<TLeftKey, TRightKey>>
	{
		private IComparer<TLeftKey> _leftComparer;

		private IComparer<TRightKey> _rightComparer;

		internal ConcatKeyComparer(IComparer<TLeftKey> leftComparer, IComparer<TRightKey> rightComparer)
		{
			_leftComparer = leftComparer;
			_rightComparer = rightComparer;
		}

		public int Compare(ConcatKey<TLeftKey, TRightKey> x, ConcatKey<TLeftKey, TRightKey> y)
		{
			if (x._isLeft != y._isLeft)
			{
				if (!x._isLeft)
				{
					return 1;
				}
				return -1;
			}
			if (x._isLeft)
			{
				return _leftComparer.Compare(x._leftKey, y._leftKey);
			}
			return _rightComparer.Compare(x._rightKey, y._rightKey);
		}
	}

	private readonly TLeftKey _leftKey;

	private readonly TRightKey _rightKey;

	private readonly bool _isLeft;

	private ConcatKey(TLeftKey leftKey, TRightKey rightKey, bool isLeft)
	{
		_leftKey = leftKey;
		_rightKey = rightKey;
		_isLeft = isLeft;
	}

	internal static ConcatKey<TLeftKey, TRightKey> MakeLeft(TLeftKey leftKey)
	{
		return new ConcatKey<TLeftKey, TRightKey>(leftKey, default(TRightKey), isLeft: true);
	}

	internal static ConcatKey<TLeftKey, TRightKey> MakeRight(TRightKey rightKey)
	{
		return new ConcatKey<TLeftKey, TRightKey>(default(TLeftKey), rightKey, isLeft: false);
	}

	internal static IComparer<ConcatKey<TLeftKey, TRightKey>> MakeComparer(IComparer<TLeftKey> leftComparer, IComparer<TRightKey> rightComparer)
	{
		return new ConcatKeyComparer(leftComparer, rightComparer);
	}
}
