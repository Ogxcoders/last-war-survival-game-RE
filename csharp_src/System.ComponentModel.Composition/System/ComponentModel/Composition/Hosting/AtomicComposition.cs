using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

public class AtomicComposition : IDisposable
{
	private readonly AtomicComposition _outerAtomicComposition;

	private KeyValuePair<object, object>[] _values;

	private int _valueCount;

	private List<Action> _completeActionList;

	private List<Action> _revertActionList;

	private bool _isDisposed;

	private bool _isCompleted;

	private bool _containsInnerAtomicComposition;

	private bool ContainsInnerAtomicComposition
	{
		set
		{
			if (value && _containsInnerAtomicComposition)
			{
				throw new InvalidOperationException(Strings.AtomicComposition_AlreadyNested);
			}
			_containsInnerAtomicComposition = value;
		}
	}

	public AtomicComposition()
		: this(null)
	{
	}

	public AtomicComposition(AtomicComposition outerAtomicComposition)
	{
		if (outerAtomicComposition != null)
		{
			_outerAtomicComposition = outerAtomicComposition;
			_outerAtomicComposition.ContainsInnerAtomicComposition = true;
		}
	}

	public void SetValue(object key, object value)
	{
		ThrowIfDisposed();
		ThrowIfCompleted();
		ThrowIfContainsInnerAtomicComposition();
		Requires.NotNull(key, "key");
		SetValueInternal(key, value);
	}

	public bool TryGetValue<T>(object key, out T value)
	{
		return TryGetValue<T>(key, localAtomicCompositionOnly: false, out value);
	}

	public bool TryGetValue<T>(object key, bool localAtomicCompositionOnly, out T value)
	{
		ThrowIfDisposed();
		ThrowIfCompleted();
		Requires.NotNull(key, "key");
		return TryGetValueInternal<T>(key, localAtomicCompositionOnly, out value);
	}

	public void AddCompleteAction(Action completeAction)
	{
		ThrowIfDisposed();
		ThrowIfCompleted();
		ThrowIfContainsInnerAtomicComposition();
		Requires.NotNull(completeAction, "completeAction");
		if (_completeActionList == null)
		{
			_completeActionList = new List<Action>();
		}
		_completeActionList.Add(completeAction);
	}

	public void AddRevertAction(Action revertAction)
	{
		ThrowIfDisposed();
		ThrowIfCompleted();
		ThrowIfContainsInnerAtomicComposition();
		Requires.NotNull(revertAction, "revertAction");
		if (_revertActionList == null)
		{
			_revertActionList = new List<Action>();
		}
		_revertActionList.Add(revertAction);
	}

	public void Complete()
	{
		ThrowIfDisposed();
		ThrowIfCompleted();
		if (_outerAtomicComposition == null)
		{
			FinalComplete();
		}
		else
		{
			CopyComplete();
		}
		_isCompleted = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		ThrowIfDisposed();
		_isDisposed = true;
		if (_outerAtomicComposition != null)
		{
			_outerAtomicComposition.ContainsInnerAtomicComposition = false;
		}
		if (!_isCompleted && _revertActionList != null)
		{
			for (int num = _revertActionList.Count - 1; num >= 0; num--)
			{
				_revertActionList[num]();
			}
			_revertActionList = null;
		}
	}

	private void FinalComplete()
	{
		if (_completeActionList == null)
		{
			return;
		}
		foreach (Action completeAction in _completeActionList)
		{
			completeAction();
		}
		_completeActionList = null;
	}

	private void CopyComplete()
	{
		Assumes.NotNull(_outerAtomicComposition);
		_outerAtomicComposition.ContainsInnerAtomicComposition = false;
		if (_completeActionList != null)
		{
			foreach (Action completeAction in _completeActionList)
			{
				_outerAtomicComposition.AddCompleteAction(completeAction);
			}
		}
		if (_revertActionList != null)
		{
			foreach (Action revertAction in _revertActionList)
			{
				_outerAtomicComposition.AddRevertAction(revertAction);
			}
		}
		for (int i = 0; i < _valueCount; i++)
		{
			_outerAtomicComposition.SetValueInternal(_values[i].Key, _values[i].Value);
		}
	}

	private bool TryGetValueInternal<T>(object key, bool localAtomicCompositionOnly, out T value)
	{
		for (int i = 0; i < _valueCount; i++)
		{
			if (_values[i].Key == key)
			{
				value = (T)_values[i].Value;
				return true;
			}
		}
		if (!localAtomicCompositionOnly && _outerAtomicComposition != null)
		{
			return _outerAtomicComposition.TryGetValueInternal<T>(key, localAtomicCompositionOnly, out value);
		}
		value = default(T);
		return false;
	}

	private void SetValueInternal(object key, object value)
	{
		for (int i = 0; i < _valueCount; i++)
		{
			if (_values[i].Key == key)
			{
				_values[i] = new KeyValuePair<object, object>(key, value);
				return;
			}
		}
		if (_values == null || _valueCount == _values.Length)
		{
			KeyValuePair<object, object>[] array = new KeyValuePair<object, object>[(_valueCount == 0) ? 5 : (_valueCount * 2)];
			if (_values != null)
			{
				Array.Copy(_values, array, _valueCount);
			}
			_values = array;
		}
		_values[_valueCount] = new KeyValuePair<object, object>(key, value);
		_valueCount++;
	}

	[DebuggerStepThrough]
	private void ThrowIfContainsInnerAtomicComposition()
	{
		if (_containsInnerAtomicComposition)
		{
			throw new InvalidOperationException(Strings.AtomicComposition_PartOfAnotherAtomicComposition);
		}
	}

	[DebuggerStepThrough]
	private void ThrowIfCompleted()
	{
		if (_isCompleted)
		{
			throw new InvalidOperationException(Strings.AtomicComposition_AlreadyCompleted);
		}
	}

	[DebuggerStepThrough]
	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}
}
