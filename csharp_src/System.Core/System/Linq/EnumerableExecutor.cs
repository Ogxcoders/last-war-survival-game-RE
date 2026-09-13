using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq;

public abstract class EnumerableExecutor
{
	internal abstract object ExecuteBoxed();

	internal static EnumerableExecutor Create(Expression expression)
	{
		return (EnumerableExecutor)Activator.CreateInstance(typeof(EnumerableExecutor<>).MakeGenericType(expression.Type), expression);
	}
}
public class EnumerableExecutor<T> : EnumerableExecutor
{
	private readonly Expression _expression;

	public EnumerableExecutor(Expression expression)
	{
		_expression = expression;
	}

	internal override object ExecuteBoxed()
	{
		return Execute();
	}

	internal T Execute()
	{
		return Expression.Lambda<Func<T>>(new EnumerableRewriter().Visit(_expression), (IEnumerable<ParameterExpression>)null).Compile()();
	}
}
