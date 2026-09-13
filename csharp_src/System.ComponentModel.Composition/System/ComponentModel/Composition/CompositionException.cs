using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition;

[Serializable]
[DebuggerTypeProxy(typeof(CompositionExceptionDebuggerProxy))]
[DebuggerDisplay("{Message}")]
public class CompositionException : Exception
{
	[Serializable]
	private struct CompositionExceptionData : ISafeSerializationData
	{
		public CompositionError[] _errors;

		void ISafeSerializationData.CompleteDeserialization(object obj)
		{
			(obj as CompositionException)._errors = new ReadOnlyCollection<CompositionError>(_errors);
		}
	}

	private struct VisitContext
	{
		public Stack<CompositionError> Path;

		public Action<Stack<CompositionError>> LeafVisitor;
	}

	private const string ErrorsKey = "Errors";

	private ReadOnlyCollection<CompositionError> _errors;

	public ReadOnlyCollection<CompositionError> Errors => _errors;

	public override string Message
	{
		get
		{
			if (Errors.Count == 0)
			{
				return base.Message;
			}
			return BuildDefaultMessage();
		}
	}

	public CompositionException()
		: this(null, null, null)
	{
	}

	public CompositionException(string message)
		: this(message, null, null)
	{
	}

	public CompositionException(string message, Exception innerException)
		: this(message, innerException, null)
	{
	}

	internal CompositionException(CompositionError error)
		: this(new CompositionError[1] { error })
	{
	}

	public CompositionException(IEnumerable<CompositionError> errors)
		: this(null, null, errors)
	{
	}

	internal CompositionException(string message, Exception innerException, IEnumerable<CompositionError> errors)
		: base(message, innerException)
	{
		Requires.NullOrNotNullElements(errors, "errors");
		base.SerializeObjectState += delegate(object exception, SafeSerializationEventArgs eventArgs)
		{
			CompositionExceptionData compositionExceptionData = default(CompositionExceptionData);
			if (_errors != null)
			{
				compositionExceptionData._errors = _errors.Select((CompositionError error) => new CompositionError(error.Id, error.Description, error.Element.ToSerializableElement(), error.Exception)).ToArray();
			}
			else
			{
				compositionExceptionData._errors = new CompositionError[0];
			}
			eventArgs.AddSerializedState(compositionExceptionData);
		};
		_errors = new ReadOnlyCollection<CompositionError>((errors == null) ? new CompositionError[0] : errors.ToArray());
	}

	private string BuildDefaultMessage()
	{
		IEnumerable<IEnumerable<CompositionError>> enumerable = CalculatePaths(this);
		StringBuilder stringBuilder = new StringBuilder();
		WriteHeader(stringBuilder, Errors.Count, enumerable.Count());
		WritePaths(stringBuilder, enumerable);
		return stringBuilder.ToString();
	}

	private static void WriteHeader(StringBuilder writer, int errorsCount, int pathCount)
	{
		if (errorsCount > 1 && pathCount > 1)
		{
			writer.AppendFormat(CultureInfo.CurrentCulture, Strings.CompositionException_MultipleErrorsWithMultiplePaths, pathCount);
		}
		else if (errorsCount == 1 && pathCount > 1)
		{
			writer.AppendFormat(CultureInfo.CurrentCulture, Strings.CompositionException_SingleErrorWithMultiplePaths, pathCount);
		}
		else
		{
			Assumes.IsTrue(errorsCount == 1);
			Assumes.IsTrue(pathCount == 1);
			writer.AppendFormat(CultureInfo.CurrentCulture, Strings.CompositionException_SingleErrorWithSinglePath, pathCount);
		}
		writer.Append(' ');
		writer.AppendLine(Strings.CompositionException_ReviewErrorProperty);
	}

	private static void WritePaths(StringBuilder writer, IEnumerable<IEnumerable<CompositionError>> paths)
	{
		int num = 0;
		foreach (IEnumerable<CompositionError> path in paths)
		{
			num++;
			WritePath(writer, path, num);
		}
	}

	private static void WritePath(StringBuilder writer, IEnumerable<CompositionError> path, int ordinal)
	{
		writer.AppendLine();
		writer.Append(ordinal.ToString(CultureInfo.CurrentCulture));
		writer.Append(Strings.CompositionException_PathsCountSeparator);
		writer.Append(' ');
		WriteError(writer, path.First());
		foreach (CompositionError item in path.Skip(1))
		{
			writer.AppendLine();
			writer.Append(Strings.CompositionException_ErrorPrefix);
			writer.Append(' ');
			WriteError(writer, item);
		}
	}

	private static void WriteError(StringBuilder writer, CompositionError error)
	{
		writer.AppendLine(error.Description);
		if (error.Element != null)
		{
			WriteElementGraph(writer, error.Element);
		}
	}

	private static void WriteElementGraph(StringBuilder writer, ICompositionElement element)
	{
		writer.AppendFormat(CultureInfo.CurrentCulture, Strings.CompositionException_ElementPrefix, element.DisplayName);
		while ((element = element.Origin) != null)
		{
			writer.AppendFormat(CultureInfo.CurrentCulture, Strings.CompositionException_OriginFormat, Strings.CompositionException_OriginSeparator, element.DisplayName);
		}
		writer.AppendLine();
	}

	private static IEnumerable<IEnumerable<CompositionError>> CalculatePaths(CompositionException exception)
	{
		List<IEnumerable<CompositionError>> paths = new List<IEnumerable<CompositionError>>();
		VisitCompositionException(exception, new VisitContext
		{
			Path = new Stack<CompositionError>(),
			LeafVisitor = delegate(Stack<CompositionError> path)
			{
				paths.Add(path.Copy());
			}
		});
		return paths;
	}

	private static void VisitCompositionException(CompositionException exception, VisitContext context)
	{
		foreach (CompositionError error in exception.Errors)
		{
			VisitError(error, context);
		}
		if (exception.InnerException != null)
		{
			VisitException(exception.InnerException, context);
		}
	}

	private static void VisitError(CompositionError error, VisitContext context)
	{
		context.Path.Push(error);
		if (error.Exception == null)
		{
			context.LeafVisitor(context.Path);
		}
		else
		{
			VisitException(error.Exception, context);
		}
		context.Path.Pop();
	}

	private static void VisitException(Exception exception, VisitContext context)
	{
		if (exception is CompositionException exception2)
		{
			VisitCompositionException(exception2, context);
		}
		else
		{
			VisitError(new CompositionError(exception.Message, exception.InnerException), context);
		}
	}
}
