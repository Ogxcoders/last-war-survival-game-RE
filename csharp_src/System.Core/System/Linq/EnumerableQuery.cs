using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq;

public abstract class EnumerableQuery
{
	internal abstract Expression Expression { get; }

	internal abstract IEnumerable Enumerable { get; }

	internal static IQueryable Create(Type elementType, IEnumerable sequence)
	{
		return (IQueryable)Activator.CreateInstance(typeof(EnumerableQuery<>).MakeGenericType(elementType), sequence);
	}

	internal static IQueryable Create(Type elementType, Expression expression)
	{
		return (IQueryable)Activator.CreateInstance(typeof(EnumerableQuery<>).MakeGenericType(elementType), expression);
	}
}
public class EnumerableQuery<T> : EnumerableQuery, IOrderedQueryable<T>, IQueryable<T>, IEnumerable<T>, IEnumerable, IQueryable, IOrderedQueryable, IQueryProvider
{
	private readonly Expression _expression;

	private IEnumerable<T> _enumerable;

	IQueryProvider IQueryable.Provider => this;

	internal override Expression Expression => _expression;

	internal override IEnumerable Enumerable => _enumerable;

	Expression IQueryable.Expression => _expression;

	Type IQueryable.ElementType => typeof(T);

	public EnumerableQuery(IEnumerable<T> enumerable)
	{
		_enumerable = enumerable;
		_expression = Expression.Constant(this);
	}

	public EnumerableQuery(Expression expression)
	{
		_expression = expression;
	}

	IQueryable IQueryProvider.CreateQuery(Expression expression)
	{
		if (expression == null)
		{
			throw Error.ArgumentNull("expression");
		}
		Type type = TypeHelper.FindGenericType(typeof(IQueryable<>), expression.Type);
		if (type == null)
		{
			throw Error.ArgumentNotValid("expression");
		}
		return EnumerableQuery.Create(type.GetGenericArguments()[0], expression);
	}

	IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
	{
		if (expression == null)
		{
			throw Error.ArgumentNull("expression");
		}
		if (!typeof(IQueryable<TElement>).IsAssignableFrom(expression.Type))
		{
			throw Error.ArgumentNotValid("expression");
		}
		return new EnumerableQuery<TElement>(expression);
	}

	object IQueryProvider.Execute(Expression expression)
	{
		if (expression == null)
		{
			throw Error.ArgumentNull("expression");
		}
		return EnumerableExecutor.Create(expression).ExecuteBoxed();
	}

	TElement IQueryProvider.Execute<TElement>(Expression expression)
	{
		if (expression == null)
		{
			throw Error.ArgumentNull("expression");
		}
		if (!typeof(TElement).IsAssignableFrom(expression.Type))
		{
			throw Error.ArgumentNotValid("expression");
		}
		return new EnumerableExecutor<TElement>(expression).Execute();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return GetEnumerator();
	}

	private IEnumerator<T> GetEnumerator()
	{
		if (_enumerable == null)
		{
			IEnumerable<T> enumerable = Expression.Lambda<Func<IEnumerable<T>>>(new EnumerableRewriter().Visit(_expression), (IEnumerable<ParameterExpression>)null).Compile()();
			if (enumerable == this)
			{
				throw Error.EnumeratingNullEnumerableExpression();
			}
			_enumerable = enumerable;
		}
		return _enumerable.GetEnumerator();
	}

	public override string ToString()
	{
		if (_expression is ConstantExpression constantExpression && constantExpression.Value == this)
		{
			if (_enumerable != null)
			{
				return _enumerable.ToString();
			}
			return "null";
		}
		return _expression.ToString();
	}
}
