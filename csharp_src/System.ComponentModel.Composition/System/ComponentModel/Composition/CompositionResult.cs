using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition;

internal struct CompositionResult
{
	public static readonly CompositionResult SucceededResult;

	private readonly IEnumerable<CompositionError> _errors;

	public bool Succeeded
	{
		get
		{
			if (_errors != null)
			{
				return !_errors.FastAny();
			}
			return true;
		}
	}

	public IEnumerable<CompositionError> Errors => _errors ?? Enumerable.Empty<CompositionError>();

	public CompositionResult(params CompositionError[] errors)
		: this((IEnumerable<CompositionError>)errors)
	{
	}

	public CompositionResult(IEnumerable<CompositionError> errors)
	{
		_errors = errors;
	}

	public CompositionResult MergeResult(CompositionResult result)
	{
		if (Succeeded)
		{
			return result;
		}
		if (result.Succeeded)
		{
			return this;
		}
		return MergeErrors(result._errors);
	}

	public CompositionResult MergeError(CompositionError error)
	{
		return MergeErrors(new CompositionError[1] { error });
	}

	public CompositionResult MergeErrors(IEnumerable<CompositionError> errors)
	{
		return new CompositionResult(_errors.ConcatAllowingNull(errors));
	}

	public CompositionResult<T> ToResult<T>(T value)
	{
		return new CompositionResult<T>(value, _errors);
	}

	public void ThrowOnErrors()
	{
		ThrowOnErrors(null);
	}

	public void ThrowOnErrors(AtomicComposition atomicComposition)
	{
		if (!Succeeded)
		{
			if (atomicComposition == null)
			{
				throw new CompositionException(_errors);
			}
			throw new ChangeRejectedException(_errors);
		}
	}
}
internal struct CompositionResult<T>
{
	private readonly IEnumerable<CompositionError> _errors;

	private readonly T _value;

	public bool Succeeded
	{
		get
		{
			if (_errors != null)
			{
				return !_errors.FastAny();
			}
			return true;
		}
	}

	public IEnumerable<CompositionError> Errors => _errors ?? Enumerable.Empty<CompositionError>();

	public T Value
	{
		get
		{
			ThrowOnErrors();
			return _value;
		}
	}

	public CompositionResult(T value)
		: this(value, null)
	{
	}

	public CompositionResult(params CompositionError[] errors)
		: this(default(T), errors)
	{
	}

	public CompositionResult(IEnumerable<CompositionError> errors)
		: this(default(T), errors)
	{
	}

	internal CompositionResult(T value, IEnumerable<CompositionError> errors)
	{
		_errors = errors;
		_value = value;
	}

	internal CompositionResult<TValue> ToResult<TValue>()
	{
		return new CompositionResult<TValue>(_errors);
	}

	internal CompositionResult ToResult()
	{
		return new CompositionResult(_errors);
	}

	private void ThrowOnErrors()
	{
		if (!Succeeded)
		{
			throw new CompositionException(_errors);
		}
	}
}
