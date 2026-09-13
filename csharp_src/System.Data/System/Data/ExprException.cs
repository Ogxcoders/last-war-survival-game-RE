using System.Globalization;

namespace System.Data;

internal sealed class ExprException
{
	private ExprException()
	{
	}

	private static OverflowException _Overflow(string error)
	{
		OverflowException ex = new OverflowException(error);
		ExceptionBuilder.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	private static InvalidExpressionException _Expr(string error)
	{
		InvalidExpressionException ex = new InvalidExpressionException(error);
		ExceptionBuilder.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	private static SyntaxErrorException _Syntax(string error)
	{
		SyntaxErrorException ex = new SyntaxErrorException(error);
		ExceptionBuilder.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	private static EvaluateException _Eval(string error)
	{
		EvaluateException ex = new EvaluateException(error);
		ExceptionBuilder.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	private static EvaluateException _Eval(string error, Exception innerException)
	{
		EvaluateException ex = new EvaluateException(error);
		ExceptionBuilder.TraceExceptionAsReturnValue(ex);
		return ex;
	}

	public static Exception InvokeArgument()
	{
		return ExceptionBuilder._Argument("Need a row or a table to Invoke DataFilter.");
	}

	public static Exception NYI(string moreinfo)
	{
		return _Expr(global::SR.Format("The feature not implemented. {0}.", moreinfo));
	}

	public static Exception MissingOperand(OperatorInfo before)
	{
		return _Syntax(global::SR.Format("Syntax error: Missing operand after '{0}' operator.", Operators.ToString(before._op)));
	}

	public static Exception MissingOperator(string token)
	{
		return _Syntax(global::SR.Format("Syntax error: Missing operand after '{0}' operator.", token));
	}

	public static Exception TypeMismatch(string expr)
	{
		return _Eval(global::SR.Format("Type mismatch in expression '{0}'.", expr));
	}

	public static Exception FunctionArgumentOutOfRange(string arg, string func)
	{
		return ExceptionBuilder._ArgumentOutOfRange(arg, global::SR.Format("{0}() argument is out of range.", func));
	}

	public static Exception ExpressionTooComplex()
	{
		return _Eval("Expression is too complex.");
	}

	public static Exception UnboundName(string name)
	{
		return _Eval(global::SR.Format("Cannot find column [{0}].", name));
	}

	public static Exception InvalidString(string str)
	{
		return _Syntax(global::SR.Format("The expression contains an invalid string constant: {0}.", str));
	}

	public static Exception UndefinedFunction(string name)
	{
		return _Eval(global::SR.Format("The expression contains undefined function call {0}().", name));
	}

	public static Exception SyntaxError()
	{
		return _Syntax("Syntax error in the expression.");
	}

	public static Exception FunctionArgumentCount(string name)
	{
		return _Eval(global::SR.Format("Invalid number of arguments: function {0}().", name));
	}

	public static Exception MissingRightParen()
	{
		return _Syntax("The expression is missing the closing parenthesis.");
	}

	public static Exception UnknownToken(string token, int position)
	{
		return _Syntax(global::SR.Format("Cannot interpret token '{0}' at position {1}.", token, position.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception UnknownToken(Tokens tokExpected, Tokens tokCurr, int position)
	{
		return _Syntax(global::SR.Format("Expected {0}, but actual token at the position {2} is {1}.", tokExpected.ToString(), tokCurr.ToString(), position.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception DatatypeConvertion(Type type1, Type type2)
	{
		return _Eval(global::SR.Format("Cannot convert from {0} to {1}.", type1.ToString(), type2.ToString()));
	}

	public static Exception DatavalueConvertion(object value, Type type, Exception innerException)
	{
		return _Eval(global::SR.Format("Cannot convert value '{0}' to Type: {1}.", value.ToString(), type.ToString()), innerException);
	}

	public static Exception InvalidName(string name)
	{
		return _Syntax(global::SR.Format("Invalid column name [{0}].", name));
	}

	public static Exception InvalidDate(string date)
	{
		return _Syntax(global::SR.Format("The expression contains invalid date constant '{0}'.", date));
	}

	public static Exception NonConstantArgument()
	{
		return _Eval("Only constant expressions are allowed in the expression list for the IN operator.");
	}

	public static Exception InvalidPattern(string pat)
	{
		return _Eval(global::SR.Format("Error in Like operator: the string pattern '{0}' is invalid.", pat));
	}

	public static Exception InWithoutParentheses()
	{
		return _Syntax("Syntax error: The items following the IN keyword must be separated by commas and be enclosed in parentheses.");
	}

	public static Exception InWithoutList()
	{
		return _Syntax("Syntax error: The IN keyword must be followed by a non-empty list of expressions separated by commas, and also must be enclosed in parentheses.");
	}

	public static Exception InvalidIsSyntax()
	{
		return _Syntax("Syntax error: Invalid usage of 'Is' operator. Correct syntax: <expression> Is [Not] Null.");
	}

	public static Exception Overflow(Type type)
	{
		return _Overflow(global::SR.Format("Value is either too large or too small for Type '{0}'.", type.Name));
	}

	public static Exception ArgumentType(string function, int arg, Type type)
	{
		return _Eval(global::SR.Format("Type mismatch in function argument: {0}(), argument {1}, expected {2}.", function, arg.ToString(CultureInfo.InvariantCulture), type.ToString()));
	}

	public static Exception ArgumentTypeInteger(string function, int arg)
	{
		return _Eval(global::SR.Format("Type mismatch in function argument: {0}(), argument {1}, expected one of the Integer types.", function, arg.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception TypeMismatchInBinop(int op, Type type1, Type type2)
	{
		return _Eval(global::SR.Format("Cannot perform '{0}' operation on {1} and {2}.", Operators.ToString(op), type1.ToString(), type2.ToString()));
	}

	public static Exception AmbiguousBinop(int op, Type type1, Type type2)
	{
		return _Eval(global::SR.Format("Operator '{0}' is ambiguous on operands of type '{1}' and '{2}'. Cannot mix signed and unsigned types. Please use explicit Convert() function.", Operators.ToString(op), type1.ToString(), type2.ToString()));
	}

	public static Exception UnsupportedOperator(int op)
	{
		return _Eval(global::SR.Format("The expression contains unsupported operator '{0}'.", Operators.ToString(op)));
	}

	public static Exception InvalidNameBracketing(string name)
	{
		return _Syntax(global::SR.Format("The expression contains invalid name: '{0}'.", name));
	}

	public static Exception MissingOperandBefore(string op)
	{
		return _Syntax(global::SR.Format("Syntax error: Missing operand before '{0}' operator.", op));
	}

	public static Exception TooManyRightParentheses()
	{
		return _Syntax("The expression has too many closing parentheses.");
	}

	public static Exception UnresolvedRelation(string name, string expr)
	{
		return _Eval(global::SR.Format("The table [{0}] involved in more than one relation. You must explicitly mention a relation name in the expression '{1}'.", name, expr));
	}

	internal static EvaluateException BindFailure(string relationName)
	{
		return _Eval(global::SR.Format("Cannot find the parent relation '{0}'.", relationName));
	}

	public static Exception AggregateArgument()
	{
		return _Syntax("Syntax error in aggregate argument: Expecting a single column argument with possible 'Child' qualifier.");
	}

	public static Exception AggregateUnbound(string expr)
	{
		return _Eval(global::SR.Format("Unbound reference in the aggregate expression '{0}'.", expr));
	}

	public static Exception EvalNoContext()
	{
		return _Eval("Cannot evaluate non-constant expression without current row.");
	}

	public static Exception ExpressionUnbound(string expr)
	{
		return _Eval(global::SR.Format("Unbound reference in the expression '{0}'.", expr));
	}

	public static Exception ComputeNotAggregate(string expr)
	{
		return _Eval(global::SR.Format("Cannot evaluate. Expression '{0}' is not an aggregate.", expr));
	}

	public static Exception FilterConvertion(string expr)
	{
		return _Eval(global::SR.Format("Filter expression '{0}' does not evaluate to a Boolean term.", expr));
	}

	public static Exception LookupArgument()
	{
		return _Syntax("Syntax error in Lookup expression: Expecting keyword 'Parent' followed by a single column argument with possible relation qualifier: Parent[(<relation_name>)].<column_name>.");
	}

	public static Exception InvalidType(string typeName)
	{
		return _Eval(global::SR.Format("Invalid type name '{0}'.", typeName));
	}

	public static Exception InvalidHoursArgument()
	{
		return _Eval("'hours' argument is out of range. Value must be between -14 and +14.");
	}

	public static Exception InvalidMinutesArgument()
	{
		return _Eval("'minutes' argument is out of range. Value must be between -59 and +59.");
	}

	public static Exception InvalidTimeZoneRange()
	{
		return _Eval("Provided range for time one exceeds total of 14 hours.");
	}

	public static Exception MismatchKindandTimeSpan()
	{
		return _Eval("Kind property of provided DateTime argument, does not match 'hours' and 'minutes' arguments.");
	}

	public static Exception UnsupportedDataType(Type type)
	{
		return ExceptionBuilder._Argument(global::SR.Format("A DataColumn of type '{0}' does not support expression.", type.FullName));
	}
}
