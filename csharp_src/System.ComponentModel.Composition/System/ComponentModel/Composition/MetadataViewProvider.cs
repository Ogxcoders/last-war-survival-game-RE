using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

internal static class MetadataViewProvider
{
	public static TMetadataView GetMetadataView<TMetadataView>(IDictionary<string, object> metadata)
	{
		Assumes.NotNull(metadata);
		Type typeFromHandle = typeof(TMetadataView);
		if (typeFromHandle.IsAssignableFrom(typeof(IDictionary<string, object>)))
		{
			return (TMetadataView)metadata;
		}
		Type type;
		if (typeFromHandle.IsInterface)
		{
			if (!typeFromHandle.IsAttributeDefined<MetadataViewImplementationAttribute>())
			{
				try
				{
					type = MetadataViewGenerator.GenerateView(typeFromHandle);
				}
				catch (TypeLoadException innerException)
				{
					throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Strings.NotSupportedInterfaceMetadataView, typeFromHandle.FullName), innerException);
				}
			}
			else
			{
				type = typeFromHandle.GetFirstAttribute<MetadataViewImplementationAttribute>().ImplementationType;
				if (type == null)
				{
					throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_MetadataViewImplementationCanNotBeNull, typeFromHandle.FullName, type.FullName));
				}
				if (!typeFromHandle.IsAssignableFrom(type))
				{
					throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_MetadataViewImplementationDoesNotImplementViewInterface, typeFromHandle.FullName, type.FullName));
				}
			}
		}
		else
		{
			type = typeFromHandle;
		}
		try
		{
			return (TMetadataView)type.SafeCreateInstance(metadata);
		}
		catch (MissingMethodException innerException2)
		{
			throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.CompositionException_MetadataViewInvalidConstructor, type.AssemblyQualifiedName), innerException2);
		}
		catch (TargetInvocationException ex)
		{
			if (typeFromHandle.IsInterface)
			{
				if (ex.InnerException.GetType() == typeof(InvalidCastException))
				{
					throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_InvalidCastOnMetadataField, ex.InnerException.Data["MetadataViewType"], ex.InnerException.Data["MetadataItemKey"], ex.InnerException.Data["MetadataItemValue"], ex.InnerException.Data["MetadataItemSourceType"], ex.InnerException.Data["MetadataItemTargetType"]), ex);
				}
				if (ex.InnerException.GetType() == typeof(NullReferenceException))
				{
					throw new CompositionContractMismatchException(string.Format(CultureInfo.CurrentCulture, Strings.ContractMismatch_NullReferenceOnMetadataField, ex.InnerException.Data["MetadataViewType"], ex.InnerException.Data["MetadataItemKey"], ex.InnerException.Data["MetadataItemTargetType"]), ex);
				}
			}
			throw;
		}
	}

	public static bool IsViewTypeValid(Type metadataViewType)
	{
		Assumes.NotNull(metadataViewType);
		if (ExportServices.IsDefaultMetadataViewType(metadataViewType) || metadataViewType.IsInterface || ExportServices.IsDictionaryConstructorViewType(metadataViewType))
		{
			return true;
		}
		return false;
	}
}
