using System.Diagnostics;
using System.Dynamic.Utils;
using System.Runtime.CompilerServices;

namespace System.Linq.Expressions;

[DebuggerTypeProxy(typeof(TypeBinaryExpressionProxy))]
public sealed class TypeBinaryExpression : Expression
{
	public sealed override Type Type => typeof(bool);

	public sealed override ExpressionType NodeType { get; }

	public Expression Expression { get; }

	public Type TypeOperand { get; }

	internal TypeBinaryExpression(Expression expression, Type typeOperand, ExpressionType nodeType)
	{
		Expression = expression;
		TypeOperand = typeOperand;
		NodeType = nodeType;
	}

	internal Expression ReduceTypeEqual()
	{
		Type type = Expression.Type;
		if (type.IsValueType || TypeOperand.IsPointer)
		{
			if (type.IsNullableType())
			{
				if (type.GetNonNullableType() != TypeOperand.GetNonNullableType())
				{
					return Expression.Block(Expression, Utils.Constant(value: false));
				}
				return Expression.NotEqual(Expression, Expression.Constant(null, Expression.Type));
			}
			return Expression.Block(Expression, Utils.Constant(type == TypeOperand.GetNonNullableType()));
		}
		if (Expression.NodeType == ExpressionType.Constant)
		{
			return ReduceConstantTypeEqual();
		}
		if (Expression is ParameterExpression { IsByRef: false } parameterExpression)
		{
			return ByValParameterTypeEqual(parameterExpression);
		}
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
		return Expression.Block(new TrueReadOnlyCollection<ParameterExpression>(parameterExpression2), new TrueReadOnlyCollection<Expression>(Expression.Assign(parameterExpression2, Expression), ByValParameterTypeEqual(parameterExpression2)));
	}

	private Expression ByValParameterTypeEqual(ParameterExpression value)
	{
		Expression expression = Expression.Call(value, CachedReflectionInfo.Object_GetType);
		if (TypeOperand.IsInterface)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(Type));
			expression = Expression.Block(new TrueReadOnlyCollection<ParameterExpression>(parameterExpression), new TrueReadOnlyCollection<Expression>(Expression.Assign(parameterExpression, expression), parameterExpression));
		}
		return Expression.AndAlso(Expression.ReferenceNotEqual(value, Utils.Null), Expression.ReferenceEqual(expression, Expression.Constant(TypeOperand.GetNonNullableType(), typeof(Type))));
	}

	private Expression ReduceConstantTypeEqual()
	{
		ConstantExpression constantExpression = Expression as ConstantExpression;
		if (constantExpression.Value == null)
		{
			return Utils.Constant(value: false);
		}
		return Utils.Constant(TypeOperand.GetNonNullableType() == constantExpression.Value.GetType());
	}

	protected internal override Expression Accept(ExpressionVisitor visitor)
	{
		return visitor.VisitTypeBinary(this);
	}

	public TypeBinaryExpression Update(Expression expression)
	{
		if (expression == Expression)
		{
			return this;
		}
		if (NodeType == ExpressionType.TypeIs)
		{
			return Expression.TypeIs(expression, TypeOperand);
		}
		return Expression.TypeEqual(expression, TypeOperand);
	}
}
