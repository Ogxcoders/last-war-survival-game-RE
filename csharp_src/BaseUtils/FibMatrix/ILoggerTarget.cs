namespace FibMatrix;

public interface ILoggerTarget
{
	LogTargetType targetType { get; }

	void Debug(string message, object context, LogImportance importance);

	void Info(string message, object context, LogImportance importance);

	void Warning(string message, object context, LogImportance importance);

	void Error(string message, string stackTrace, object context, LogImportance importance);
}
