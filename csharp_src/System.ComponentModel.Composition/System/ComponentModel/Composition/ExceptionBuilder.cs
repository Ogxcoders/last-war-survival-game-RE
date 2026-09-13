using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

internal static class ExceptionBuilder
{
	public static Exception CreateDiscoveryException(string messageFormat, params string[] arguments)
	{
		return new InvalidOperationException(Format(messageFormat, arguments));
	}

	public static ArgumentException CreateContainsNullElement(string parameterName)
	{
		Assumes.NotNull(parameterName);
		return new ArgumentException(Format(Strings.Argument_NullElement, parameterName), parameterName);
	}

	public static ObjectDisposedException CreateObjectDisposed(object instance)
	{
		Assumes.NotNull(instance);
		return new ObjectDisposedException(instance.GetType().ToString());
	}

	public static NotImplementedException CreateNotOverriddenByDerived(string memberName)
	{
		Assumes.NotNullOrEmpty(memberName);
		return new NotImplementedException(Format(Strings.NotImplemented_NotOverriddenByDerived, memberName));
	}

	public static ArgumentException CreateExportDefinitionNotOnThisComposablePart(string parameterName)
	{
		Assumes.NotNullOrEmpty(parameterName);
		return new ArgumentException(Format(Strings.ExportDefinitionNotOnThisComposablePart, parameterName), parameterName);
	}

	public static ArgumentException CreateImportDefinitionNotOnThisComposablePart(string parameterName)
	{
		Assumes.NotNullOrEmpty(parameterName);
		return new ArgumentException(Format(Strings.ImportDefinitionNotOnThisComposablePart, parameterName), parameterName);
	}

	public static CompositionException CreateCannotGetExportedValue(ComposablePart part, ExportDefinition definition, Exception innerException)
	{
		Assumes.NotNull(part, definition, innerException);
		return new CompositionException(ErrorBuilder.CreateCannotGetExportedValue(part, definition, innerException));
	}

	public static ArgumentException CreateReflectionModelInvalidPartDefinition(string parameterName, Type partDefinitionType)
	{
		Assumes.NotNullOrEmpty(parameterName);
		Assumes.NotNull(partDefinitionType);
		return new ArgumentException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_InvalidPartDefinition, partDefinitionType), parameterName);
	}

	public static ArgumentException ExportFactory_TooManyGenericParameters(string typeName)
	{
		Assumes.NotNullOrEmpty(typeName);
		return new ArgumentException(Format(Strings.ExportFactory_TooManyGenericParameters, typeName), typeName);
	}

	private static string Format(string format, params string[] arguments)
	{
		return string.Format(CultureInfo.CurrentCulture, format, arguments);
	}
}
