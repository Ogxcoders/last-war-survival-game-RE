using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
public class CatalogReflectionContextAttribute : Attribute
{
	private Type _reflectionContextType;

	public CatalogReflectionContextAttribute(Type reflectionContextType)
	{
		Requires.NotNull(reflectionContextType, "reflectionContextType");
		_reflectionContextType = reflectionContextType;
	}

	public ReflectionContext CreateReflectionContext()
	{
		Assumes.NotNull(_reflectionContextType);
		ReflectionContext reflectionContext = null;
		try
		{
			return (ReflectionContext)Activator.CreateInstance(_reflectionContextType);
		}
		catch (InvalidCastException innerException)
		{
			throw new InvalidOperationException(Strings.ReflectionContext_Type_Required, innerException);
		}
		catch (MissingMethodException inner)
		{
			throw new MissingMethodException(Strings.ReflectionContext_Requires_DefaultConstructor, inner);
		}
	}
}
