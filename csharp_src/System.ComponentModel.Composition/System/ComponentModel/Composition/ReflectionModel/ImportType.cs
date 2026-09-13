using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ImportType
{
	private static readonly Type LazyOfTType = typeof(Lazy<>);

	private static readonly Type LazyOfTMType = typeof(Lazy<, >);

	private static readonly Type ExportFactoryOfTType = typeof(ExportFactory<>);

	private static readonly Type ExportFactoryOfTMType = typeof(ExportFactory<, >);

	private readonly Type _type;

	private readonly bool _isAssignableCollectionType;

	private readonly Type _contractType;

	private Func<Export, object> _castSingleValue;

	private bool _isOpenGeneric;

	public bool IsAssignableCollectionType => _isAssignableCollectionType;

	public Type ElementType { get; private set; }

	public Type ActualType => _type;

	public bool IsPartCreator { get; private set; }

	public Type ContractType => _contractType;

	public Func<Export, object> CastExport
	{
		get
		{
			Assumes.IsTrue(!_isOpenGeneric);
			return _castSingleValue;
		}
	}

	public Type MetadataViewType { get; private set; }

	public ImportType(Type type, ImportCardinality cardinality)
	{
		Assumes.NotNull(type);
		_type = type;
		_contractType = type;
		if (cardinality == ImportCardinality.ZeroOrMore)
		{
			_isAssignableCollectionType = IsTypeAssignableCollectionType(type);
			_contractType = CheckForCollection(type);
		}
		_isOpenGeneric = type.ContainsGenericParameters;
		_contractType = CheckForLazyAndPartCreator(_contractType);
	}

	private Type CheckForCollection(Type type)
	{
		ElementType = CollectionServices.GetEnumerableElementType(type);
		if (ElementType != null)
		{
			return ElementType;
		}
		return type;
	}

	private static bool IsGenericDescendentOf(Type type, Type baseGenericTypeDefinition)
	{
		if (type == typeof(object) || type == null)
		{
			return false;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition() == baseGenericTypeDefinition)
		{
			return true;
		}
		return IsGenericDescendentOf(type.BaseType, baseGenericTypeDefinition);
	}

	public static bool IsDescendentOf(Type type, Type baseType)
	{
		Assumes.NotNull(type);
		Assumes.NotNull(baseType);
		if (!baseType.IsGenericTypeDefinition)
		{
			return baseType.IsAssignableFrom(type);
		}
		return IsGenericDescendentOf(type, baseType.GetGenericTypeDefinition());
	}

	private Type CheckForLazyAndPartCreator(Type type)
	{
		if (type.IsGenericType)
		{
			Type underlyingSystemType = type.GetGenericTypeDefinition().UnderlyingSystemType;
			Type[] genericArguments = type.GetGenericArguments();
			if (underlyingSystemType == LazyOfTType)
			{
				if (!_isOpenGeneric)
				{
					_castSingleValue = ExportServices.CreateStronglyTypedLazyFactory(genericArguments[0].UnderlyingSystemType, null);
				}
				return genericArguments[0];
			}
			if (underlyingSystemType == LazyOfTMType)
			{
				MetadataViewType = genericArguments[1];
				if (!_isOpenGeneric)
				{
					_castSingleValue = ExportServices.CreateStronglyTypedLazyFactory(genericArguments[0].UnderlyingSystemType, genericArguments[1].UnderlyingSystemType);
				}
				return genericArguments[0];
			}
			if (underlyingSystemType != null && IsDescendentOf(underlyingSystemType, ExportFactoryOfTType))
			{
				IsPartCreator = true;
				if (genericArguments.Length == 1)
				{
					if (!_isOpenGeneric)
					{
						_castSingleValue = new ExportFactoryCreator(underlyingSystemType).CreateStronglyTypedExportFactoryFactory(genericArguments[0].UnderlyingSystemType, null);
					}
				}
				else
				{
					if (genericArguments.Length != 2)
					{
						throw ExceptionBuilder.ExportFactory_TooManyGenericParameters(underlyingSystemType.FullName);
					}
					if (!_isOpenGeneric)
					{
						_castSingleValue = new ExportFactoryCreator(underlyingSystemType).CreateStronglyTypedExportFactoryFactory(genericArguments[0].UnderlyingSystemType, genericArguments[1].UnderlyingSystemType);
					}
					MetadataViewType = genericArguments[1];
				}
				return genericArguments[0];
			}
		}
		return type;
	}

	private static bool IsTypeAssignableCollectionType(Type type)
	{
		if (type.IsArray || CollectionServices.IsEnumerableOfT(type))
		{
			return true;
		}
		return false;
	}
}
