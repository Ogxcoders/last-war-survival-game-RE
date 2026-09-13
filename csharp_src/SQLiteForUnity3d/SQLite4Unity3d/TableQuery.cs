using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SQLite4Unity3d;

public class TableQuery<T> : BaseTableQuery, IEnumerable<T>, IEnumerable
{
	private class CompileResult
	{
		public string CommandText { get; set; }

		public object Value { get; set; }
	}

	private Expression _where;

	private List<Ordering> _orderBys;

	private int? _limit;

	private int? _offset;

	private BaseTableQuery _joinInner;

	private Expression _joinInnerKeySelector;

	private BaseTableQuery _joinOuter;

	private Expression _joinOuterKeySelector;

	private Expression _joinSelector;

	private Expression _selector;

	private bool _deferred;

	public SQLiteConnection Connection { get; private set; }

	public TableMapping Table { get; private set; }

	private TableQuery(SQLiteConnection conn, TableMapping table)
	{
		Connection = conn;
		Table = table;
	}

	public TableQuery(SQLiteConnection conn)
	{
		Connection = conn;
		Table = Connection.GetMapping(typeof(T));
	}

	public TableQuery<U> Clone<U>()
	{
		TableQuery<U> tableQuery = new TableQuery<U>(Connection, Table);
		tableQuery._where = _where;
		tableQuery._deferred = _deferred;
		if (_orderBys != null)
		{
			tableQuery._orderBys = new List<Ordering>(_orderBys);
		}
		tableQuery._limit = _limit;
		tableQuery._offset = _offset;
		tableQuery._joinInner = _joinInner;
		tableQuery._joinInnerKeySelector = _joinInnerKeySelector;
		tableQuery._joinOuter = _joinOuter;
		tableQuery._joinOuterKeySelector = _joinOuterKeySelector;
		tableQuery._joinSelector = _joinSelector;
		tableQuery._selector = _selector;
		return tableQuery;
	}

	public TableQuery<T> Where(Expression<Func<T, bool>> predExpr)
	{
		if (predExpr.NodeType == ExpressionType.Lambda)
		{
			Expression body = predExpr.Body;
			TableQuery<T> tableQuery = Clone<T>();
			tableQuery.AddWhere(body);
			return tableQuery;
		}
		throw new NotSupportedException("Must be a predicate");
	}

	public TableQuery<T> Take(int n)
	{
		TableQuery<T> tableQuery = Clone<T>();
		tableQuery._limit = n;
		return tableQuery;
	}

	public TableQuery<T> Skip(int n)
	{
		TableQuery<T> tableQuery = Clone<T>();
		tableQuery._offset = n;
		return tableQuery;
	}

	public T ElementAt(int index)
	{
		return Skip(index).Take(1).First();
	}

	public TableQuery<T> Deferred()
	{
		TableQuery<T> tableQuery = Clone<T>();
		tableQuery._deferred = true;
		return tableQuery;
	}

	public TableQuery<T> OrderBy<U>(Expression<Func<T, U>> orderExpr)
	{
		return AddOrderBy(orderExpr, asc: true);
	}

	public TableQuery<T> OrderByDescending<U>(Expression<Func<T, U>> orderExpr)
	{
		return AddOrderBy(orderExpr, asc: false);
	}

	public TableQuery<T> ThenBy<U>(Expression<Func<T, U>> orderExpr)
	{
		return AddOrderBy(orderExpr, asc: true);
	}

	public TableQuery<T> ThenByDescending<U>(Expression<Func<T, U>> orderExpr)
	{
		return AddOrderBy(orderExpr, asc: false);
	}

	private TableQuery<T> AddOrderBy<U>(Expression<Func<T, U>> orderExpr, bool asc)
	{
		if (orderExpr.NodeType == ExpressionType.Lambda)
		{
			MemberExpression memberExpression = null;
			memberExpression = ((!(orderExpr.Body is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression)) ? (orderExpr.Body as MemberExpression) : (unaryExpression.Operand as MemberExpression));
			if (memberExpression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
			{
				TableQuery<T> tableQuery = Clone<T>();
				if (tableQuery._orderBys == null)
				{
					tableQuery._orderBys = new List<Ordering>();
				}
				tableQuery._orderBys.Add(new Ordering
				{
					ColumnName = Table.FindColumnWithPropertyName(memberExpression.Member.Name).Name,
					Ascending = asc
				});
				return tableQuery;
			}
			throw new NotSupportedException("Order By does not support: " + orderExpr);
		}
		throw new NotSupportedException("Must be a predicate");
	}

	private void AddWhere(Expression pred)
	{
		if (_where == null)
		{
			_where = pred;
		}
		else
		{
			_where = Expression.AndAlso(_where, pred);
		}
	}

	public TableQuery<TResult> Join<TInner, TKey, TResult>(TableQuery<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
	{
		return new TableQuery<TResult>(Connection, Connection.GetMapping(typeof(TResult)))
		{
			_joinOuter = this,
			_joinOuterKeySelector = outerKeySelector,
			_joinInner = inner,
			_joinInnerKeySelector = innerKeySelector,
			_joinSelector = resultSelector
		};
	}

	public TableQuery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
	{
		TableQuery<TResult> tableQuery = Clone<TResult>();
		tableQuery._selector = selector;
		return tableQuery;
	}

	private SQLiteCommand GenerateCommand(string selectionList)
	{
		if (_joinInner != null && _joinOuter != null)
		{
			throw new NotSupportedException("Joins are not supported.");
		}
		string text = "select " + selectionList + " from \"" + Table.TableName + "\"";
		List<object> list = new List<object>();
		if (_where != null)
		{
			CompileResult compileResult = CompileExpr(_where, list);
			text = text + " where " + compileResult.CommandText;
		}
		if (_orderBys != null && _orderBys.Count > 0)
		{
			string text2 = string.Join(", ", _orderBys.Select((Ordering o) => "\"" + o.ColumnName + "\"" + (o.Ascending ? "" : " desc")).ToArray());
			text = text + " order by " + text2;
		}
		if (_limit.HasValue)
		{
			text = text + " limit " + _limit.Value;
		}
		if (_offset.HasValue)
		{
			if (!_limit.HasValue)
			{
				text += " limit -1 ";
			}
			text = text + " offset " + _offset.Value;
		}
		return Connection.CreateCommand(text, list.ToArray());
	}

	private CompileResult CompileExpr(Expression expr, List<object> queryArgs)
	{
		if (expr == null)
		{
			throw new NotSupportedException("Expression is NULL");
		}
		if (expr is BinaryExpression)
		{
			BinaryExpression binaryExpression = (BinaryExpression)expr;
			CompileResult compileResult = CompileExpr(binaryExpression.Left, queryArgs);
			CompileResult compileResult2 = CompileExpr(binaryExpression.Right, queryArgs);
			string commandText = ((compileResult.CommandText == "?" && compileResult.Value == null) ? CompileNullBinaryExpression(binaryExpression, compileResult2) : ((!(compileResult2.CommandText == "?") || compileResult2.Value != null) ? ("(" + compileResult.CommandText + " " + GetSqlName(binaryExpression) + " " + compileResult2.CommandText + ")") : CompileNullBinaryExpression(binaryExpression, compileResult)));
			return new CompileResult
			{
				CommandText = commandText
			};
		}
		if (expr.NodeType == ExpressionType.Call)
		{
			MethodCallExpression methodCallExpression = (MethodCallExpression)expr;
			CompileResult[] array = new CompileResult[methodCallExpression.Arguments.Count];
			CompileResult compileResult3 = ((methodCallExpression.Object != null) ? CompileExpr(methodCallExpression.Object, queryArgs) : null);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = CompileExpr(methodCallExpression.Arguments[i], queryArgs);
			}
			string text = "";
			text = ((methodCallExpression.Method.Name == "Like" && array.Length == 2) ? ("(" + array[0].CommandText + " like " + array[1].CommandText + ")") : ((methodCallExpression.Method.Name == "Contains" && array.Length == 2) ? ("(" + array[1].CommandText + " in " + array[0].CommandText + ")") : ((methodCallExpression.Method.Name == "Contains" && array.Length == 1) ? ((methodCallExpression.Object == null || !(methodCallExpression.Object.Type == typeof(string))) ? ("(" + array[0].CommandText + " in " + compileResult3.CommandText + ")") : ("(" + compileResult3.CommandText + " like ('%' || " + array[0].CommandText + " || '%'))")) : ((methodCallExpression.Method.Name == "StartsWith" && array.Length == 1) ? ("(" + compileResult3.CommandText + " like (" + array[0].CommandText + " || '%'))") : ((methodCallExpression.Method.Name == "EndsWith" && array.Length == 1) ? ("(" + compileResult3.CommandText + " like ('%' || " + array[0].CommandText + "))") : ((methodCallExpression.Method.Name == "Equals" && array.Length == 1) ? ("(" + compileResult3.CommandText + " = (" + array[0].CommandText + "))") : ((methodCallExpression.Method.Name == "ToLower") ? ("(lower(" + compileResult3.CommandText + "))") : ((!(methodCallExpression.Method.Name == "ToUpper")) ? (methodCallExpression.Method.Name.ToLower() + "(" + string.Join(",", array.Select((CompileResult a) => a.CommandText).ToArray()) + ")") : ("(upper(" + compileResult3.CommandText + "))")))))))));
			return new CompileResult
			{
				CommandText = text
			};
		}
		if (expr.NodeType == ExpressionType.Constant)
		{
			ConstantExpression constantExpression = (ConstantExpression)expr;
			queryArgs.Add(constantExpression.Value);
			return new CompileResult
			{
				CommandText = "?",
				Value = constantExpression.Value
			};
		}
		if (expr.NodeType == ExpressionType.Convert)
		{
			UnaryExpression unaryExpression = (UnaryExpression)expr;
			Type type = unaryExpression.Type;
			CompileResult compileResult4 = CompileExpr(unaryExpression.Operand, queryArgs);
			return new CompileResult
			{
				CommandText = compileResult4.CommandText,
				Value = ((compileResult4.Value != null) ? ConvertTo(compileResult4.Value, type) : null)
			};
		}
		if (expr.NodeType == ExpressionType.Not)
		{
			UnaryExpression unaryExpression2 = (UnaryExpression)expr;
			_ = unaryExpression2.Type;
			CompileResult compileResult5 = CompileExpr(unaryExpression2.Operand, queryArgs);
			return new CompileResult
			{
				CommandText = "NOT " + compileResult5.CommandText,
				Value = ((compileResult5.Value != null) ? compileResult5.Value : null)
			};
		}
		if (expr.NodeType == ExpressionType.MemberAccess)
		{
			MemberExpression memberExpression = (MemberExpression)expr;
			if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Parameter)
			{
				string name = Table.FindColumnWithPropertyName(memberExpression.Member.Name).Name;
				return new CompileResult
				{
					CommandText = "\"" + name + "\""
				};
			}
			object obj = null;
			if (memberExpression.Expression != null)
			{
				CompileResult compileResult6 = CompileExpr(memberExpression.Expression, queryArgs);
				if (compileResult6.Value == null)
				{
					throw new NotSupportedException("Member access failed to compile expression");
				}
				if (compileResult6.CommandText == "?")
				{
					queryArgs.RemoveAt(queryArgs.Count - 1);
				}
				obj = compileResult6.Value;
			}
			object obj2 = null;
			if (memberExpression.Member.MemberType == MemberTypes.Property)
			{
				obj2 = ((PropertyInfo)memberExpression.Member).GetGetMethod().Invoke(obj, null);
			}
			else
			{
				if (memberExpression.Member.MemberType != MemberTypes.Field)
				{
					throw new NotSupportedException("MemberExpr: " + memberExpression.Member.MemberType);
				}
				obj2 = ((FieldInfo)memberExpression.Member).GetValue(obj);
			}
			if (obj2 != null && obj2 is IEnumerable && !(obj2 is string) && !(obj2 is IEnumerable<byte>))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("(");
				string value = "";
				foreach (object item in (IEnumerable)obj2)
				{
					queryArgs.Add(item);
					stringBuilder.Append(value);
					stringBuilder.Append("?");
					value = ",";
				}
				stringBuilder.Append(")");
				return new CompileResult
				{
					CommandText = stringBuilder.ToString(),
					Value = obj2
				};
			}
			queryArgs.Add(obj2);
			return new CompileResult
			{
				CommandText = "?",
				Value = obj2
			};
		}
		throw new NotSupportedException("Cannot compile: " + expr.NodeType);
	}

	private static object ConvertTo(object obj, Type t)
	{
		Type underlyingType = Nullable.GetUnderlyingType(t);
		if (underlyingType != null)
		{
			if (obj == null)
			{
				return null;
			}
			return Convert.ChangeType(obj, underlyingType);
		}
		return Convert.ChangeType(obj, t);
	}

	private string CompileNullBinaryExpression(BinaryExpression expression, CompileResult parameter)
	{
		if (expression.NodeType == ExpressionType.Equal)
		{
			return "(" + parameter.CommandText + " is ?)";
		}
		if (expression.NodeType == ExpressionType.NotEqual)
		{
			return "(" + parameter.CommandText + " is not ?)";
		}
		throw new NotSupportedException("Cannot compile Null-BinaryExpression with type " + expression.NodeType);
	}

	private string GetSqlName(Expression expr)
	{
		ExpressionType nodeType = expr.NodeType;
		return nodeType switch
		{
			ExpressionType.GreaterThan => ">", 
			ExpressionType.GreaterThanOrEqual => ">=", 
			ExpressionType.LessThan => "<", 
			ExpressionType.LessThanOrEqual => "<=", 
			ExpressionType.And => "&", 
			ExpressionType.AndAlso => "and", 
			ExpressionType.Or => "|", 
			ExpressionType.OrElse => "or", 
			ExpressionType.Equal => "=", 
			ExpressionType.NotEqual => "!=", 
			_ => throw new NotSupportedException("Cannot get SQL for: " + nodeType), 
		};
	}

	public int Count()
	{
		return GenerateCommand("count(*)").ExecuteScalar<int>();
	}

	public int Count(Expression<Func<T, bool>> predExpr)
	{
		return Where(predExpr).Count();
	}

	public IEnumerator<T> GetEnumerator()
	{
		if (!_deferred)
		{
			return GenerateCommand("*").ExecuteQuery<T>().GetEnumerator();
		}
		return GenerateCommand("*").ExecuteDeferredQuery<T>().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public T First()
	{
		return Take(1).ToList().First();
	}

	public T FirstOrDefault()
	{
		return Take(1).ToList().FirstOrDefault();
	}
}
