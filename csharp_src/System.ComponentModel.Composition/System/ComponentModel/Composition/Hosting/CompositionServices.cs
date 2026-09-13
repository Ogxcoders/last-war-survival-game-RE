using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

internal static class CompositionServices
{
	private class MetadataList
	{
		private Type _arrayType;

		private bool _containsNulls;

		private static readonly Type ObjectType = typeof(object);

		private static readonly Type TypeType = typeof(Type);

		private Collection<object> _innerList = new Collection<object>();

		public void Add(object item, Type itemType)
		{
			_containsNulls |= item == null;
			if (itemType == ObjectType)
			{
				itemType = null;
			}
			if (itemType == null && item != null)
			{
				itemType = item.GetType();
			}
			if (item is Type)
			{
				itemType = TypeType;
			}
			if (itemType != null)
			{
				InferArrayType(itemType);
			}
			_innerList.Add(item);
		}

		private void InferArrayType(Type itemType)
		{
			Assumes.NotNull(itemType);
			if (_arrayType == null)
			{
				_arrayType = itemType;
			}
			else if (_arrayType != itemType)
			{
				_arrayType = ObjectType;
			}
		}

		public Array ToArray()
		{
			if (_arrayType == null)
			{
				_arrayType = ObjectType;
			}
			else if (_containsNulls && _arrayType.IsValueType)
			{
				_arrayType = ObjectType;
			}
			Array array = Array.CreateInstance(_arrayType, _innerList.Count);
			for (int i = 0; i < array.Length; i++)
			{
				array.SetValue(_innerList[i], i);
			}
			return array;
		}
	}

	internal static readonly Type InheritedExportAttributeType = typeof(InheritedExportAttribute);

	internal static readonly Type ExportAttributeType = typeof(ExportAttribute);

	internal static readonly Type AttributeType = typeof(Attribute);

	internal static readonly Type ObjectType = typeof(object);

	private static readonly string[] reservedMetadataNames = new string[1] { "System.ComponentModel.Composition.CreationPolicy" };

	internal static Type GetDefaultTypeFromMember(this MemberInfo member)
	{
		Assumes.NotNull(member);
		switch (member.MemberType)
		{
		case MemberTypes.Property:
			return ((PropertyInfo)member).PropertyType;
		case MemberTypes.TypeInfo:
		case MemberTypes.NestedType:
			return (Type)member;
		default:
			Assumes.IsTrue(member.MemberType == MemberTypes.Field);
			return ((FieldInfo)member).FieldType;
		}
	}

	internal static Type AdjustSpecifiedTypeIdentityType(this Type specifiedContractType, MemberInfo member)
	{
		if (member.MemberType == MemberTypes.Method)
		{
			return specifiedContractType;
		}
		return specifiedContractType.AdjustSpecifiedTypeIdentityType(member.GetDefaultTypeFromMember());
	}

	internal static Type AdjustSpecifiedTypeIdentityType(this Type specifiedContractType, Type memberType)
	{
		Assumes.NotNull(specifiedContractType);
		if (memberType != null && memberType.IsGenericType && specifiedContractType.IsGenericType)
		{
			if (specifiedContractType.ContainsGenericParameters && !memberType.ContainsGenericParameters)
			{
				Type[] genericArguments = memberType.GetGenericArguments();
				Type[] genericArguments2 = specifiedContractType.GetGenericArguments();
				if (genericArguments.Length == genericArguments2.Length)
				{
					return specifiedContractType.MakeGenericType(genericArguments);
				}
			}
			else if (specifiedContractType.ContainsGenericParameters && memberType.ContainsGenericParameters)
			{
				IList<Type> pureGenericParameters = memberType.GetPureGenericParameters();
				if (specifiedContractType.GetPureGenericArity() == pureGenericParameters.Count)
				{
					return specifiedContractType.GetGenericTypeDefinition().MakeGenericType(pureGenericParameters.ToArray());
				}
			}
		}
		return specifiedContractType;
	}

	private static string AdjustTypeIdentity(string originalTypeIdentity, Type typeIdentityType)
	{
		return GenericServices.GetGenericName(originalTypeIdentity, GenericServices.GetGenericParametersOrder(typeIdentityType), typeIdentityType.GetPureGenericArity());
	}

	internal static void GetContractInfoFromExport(this MemberInfo member, ExportAttribute export, out Type typeIdentityType, out string contractName)
	{
		typeIdentityType = member.GetTypeIdentityTypeFromExport(export);
		if (!string.IsNullOrEmpty(export.ContractName))
		{
			contractName = export.ContractName;
		}
		else
		{
			contractName = member.GetTypeIdentityFromExport(typeIdentityType);
		}
	}

	internal static string GetTypeIdentityFromExport(this MemberInfo member, Type typeIdentityType)
	{
		if (typeIdentityType != null)
		{
			string text = AttributedModelServices.GetTypeIdentity(typeIdentityType);
			if (typeIdentityType.ContainsGenericParameters)
			{
				text = AdjustTypeIdentity(text, typeIdentityType);
			}
			return text;
		}
		MethodInfo obj = member as MethodInfo;
		Assumes.NotNull(obj);
		return AttributedModelServices.GetTypeIdentity(obj);
	}

	private static Type GetTypeIdentityTypeFromExport(this MemberInfo member, ExportAttribute export)
	{
		if (export.ContractType != null)
		{
			return export.ContractType.AdjustSpecifiedTypeIdentityType(member);
		}
		if (member.MemberType == MemberTypes.Method)
		{
			return null;
		}
		return member.GetDefaultTypeFromMember();
	}

	internal static bool IsContractNameSameAsTypeIdentity(this ExportAttribute export)
	{
		return string.IsNullOrEmpty(export.ContractName);
	}

	internal static Type GetContractTypeFromImport(this IAttributedImport import, ImportType importType)
	{
		if (import.ContractType != null)
		{
			return import.ContractType.AdjustSpecifiedTypeIdentityType(importType.ContractType);
		}
		return importType.ContractType;
	}

	internal static string GetContractNameFromImport(this IAttributedImport import, ImportType importType)
	{
		if (!string.IsNullOrEmpty(import.ContractName))
		{
			return import.ContractName;
		}
		return AttributedModelServices.GetContractName(import.GetContractTypeFromImport(importType));
	}

	internal static string GetTypeIdentityFromImport(this IAttributedImport import, ImportType importType)
	{
		Type contractTypeFromImport = import.GetContractTypeFromImport(importType);
		if (contractTypeFromImport == ObjectType)
		{
			return null;
		}
		return AttributedModelServices.GetTypeIdentity(contractTypeFromImport);
	}

	internal static IDictionary<string, object> GetPartMetadataForType(this Type type, CreationPolicy creationPolicy)
	{
		IDictionary<string, object> dictionary = new Dictionary<string, object>(StringComparers.MetadataKeyNames);
		if (creationPolicy != CreationPolicy.Any)
		{
			dictionary.Add("System.ComponentModel.Composition.CreationPolicy", creationPolicy);
		}
		PartMetadataAttribute[] attributes = type.GetAttributes<PartMetadataAttribute>();
		foreach (PartMetadataAttribute partMetadataAttribute in attributes)
		{
			if (!reservedMetadataNames.Contains(partMetadataAttribute.Name, StringComparers.MetadataKeyNames) && !dictionary.ContainsKey(partMetadataAttribute.Name))
			{
				dictionary.Add(partMetadataAttribute.Name, partMetadataAttribute.Value);
			}
		}
		if (type.ContainsGenericParameters)
		{
			dictionary.Add("System.ComponentModel.Composition.IsGenericPart", true);
			Type[] genericArguments = type.GetGenericArguments();
			dictionary.Add("System.ComponentModel.Composition.GenericPartArity", genericArguments.Length);
			bool flag = false;
			object[] array = new object[genericArguments.Length];
			GenericParameterAttributes[] array2 = new GenericParameterAttributes[genericArguments.Length];
			for (int j = 0; j < genericArguments.Length; j++)
			{
				Type obj = genericArguments[j];
				Type[] array3 = obj.GetGenericParameterConstraints();
				if (array3.Length == 0)
				{
					array3 = null;
				}
				GenericParameterAttributes genericParameterAttributes = obj.GenericParameterAttributes;
				if (array3 != null || genericParameterAttributes != GenericParameterAttributes.None)
				{
					array[j] = array3;
					array2[j] = genericParameterAttributes;
					flag = true;
				}
			}
			if (flag)
			{
				dictionary.Add("System.ComponentModel.Composition.GenericParameterConstraints", array);
				dictionary.Add("System.ComponentModel.Composition.GenericParameterAttributes", array2);
			}
		}
		if (dictionary.Count == 0)
		{
			return MetadataServices.EmptyMetadata;
		}
		return dictionary;
	}

	internal static void TryExportMetadataForMember(this MemberInfo member, out IDictionary<string, object> dictionary)
	{
		dictionary = new Dictionary<string, object>();
		Attribute[] attributes = member.GetAttributes<Attribute>();
		foreach (Attribute attribute in attributes)
		{
			ExportMetadataAttribute exportMetadataAttribute = attribute as ExportMetadataAttribute;
			if (exportMetadataAttribute != null)
			{
				if (reservedMetadataNames.Contains(exportMetadataAttribute.Name, StringComparers.MetadataKeyNames))
				{
					throw ExceptionBuilder.CreateDiscoveryException(Strings.Discovery_ReservedMetadataNameUsed, member.GetDisplayName(), exportMetadataAttribute.Name);
				}
				if (!dictionary.TryContributeMetadataValue(exportMetadataAttribute.Name, exportMetadataAttribute.Value, null, exportMetadataAttribute.IsMultiple))
				{
					throw ExceptionBuilder.CreateDiscoveryException(Strings.Discovery_DuplicateMetadataNameValues, member.GetDisplayName(), exportMetadataAttribute.Name);
				}
				continue;
			}
			Type type = attribute.GetType();
			if (!(type != ExportAttributeType) || !type.IsAttributeDefined<MetadataAttributeAttribute>(inherit: true))
			{
				continue;
			}
			bool allowsMultiple = false;
			AttributeUsageAttribute firstAttribute = type.GetFirstAttribute<AttributeUsageAttribute>(inherit: true);
			if (firstAttribute != null)
			{
				allowsMultiple = firstAttribute.AllowMultiple;
			}
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!(propertyInfo.DeclaringType == ExportAttributeType) && !(propertyInfo.DeclaringType == AttributeType))
				{
					if (reservedMetadataNames.Contains(propertyInfo.Name, StringComparers.MetadataKeyNames))
					{
						throw ExceptionBuilder.CreateDiscoveryException(Strings.Discovery_ReservedMetadataNameUsed, member.GetDisplayName(), exportMetadataAttribute.Name);
					}
					object value = propertyInfo.GetValue(attribute, null);
					if (value != null && !IsValidAttributeType(value.GetType()))
					{
						throw ExceptionBuilder.CreateDiscoveryException(Strings.Discovery_MetadataContainsValueWithInvalidType, propertyInfo.GetDisplayName(), value.GetType().GetDisplayName());
					}
					if (!dictionary.TryContributeMetadataValue(propertyInfo.Name, value, propertyInfo.PropertyType, allowsMultiple))
					{
						throw ExceptionBuilder.CreateDiscoveryException(Strings.Discovery_DuplicateMetadataNameValues, member.GetDisplayName(), propertyInfo.Name);
					}
				}
			}
		}
		string[] array = dictionary.Keys.ToArray();
		foreach (string key in array)
		{
			if (dictionary[key] is MetadataList metadataList)
			{
				dictionary[key] = metadataList.ToArray();
			}
		}
	}

	private static bool TryContributeMetadataValue(this IDictionary<string, object> dictionary, string name, object value, Type valueType, bool allowsMultiple)
	{
		if (!dictionary.TryGetValue(name, out var value2))
		{
			if (allowsMultiple)
			{
				MetadataList metadataList = new MetadataList();
				metadataList.Add(value, valueType);
				value = metadataList;
			}
			dictionary.Add(name, value);
		}
		else
		{
			MetadataList metadataList2 = value2 as MetadataList;
			if (!allowsMultiple || metadataList2 == null)
			{
				dictionary.Remove(name);
				return false;
			}
			metadataList2.Add(value, valueType);
		}
		return true;
	}

	internal static IEnumerable<KeyValuePair<string, Type>> GetRequiredMetadata(Type metadataViewType)
	{
		if (metadataViewType == null || ExportServices.IsDefaultMetadataViewType(metadataViewType) || ExportServices.IsDictionaryConstructorViewType(metadataViewType) || !metadataViewType.IsInterface)
		{
			return Enumerable.Empty<KeyValuePair<string, Type>>();
		}
		return from property in (from property in metadataViewType.GetAllProperties()
				where property.GetFirstAttribute<DefaultValueAttribute>() == null
				select property).ToList()
			select new KeyValuePair<string, Type>(property.Name, property.PropertyType);
	}

	internal static IDictionary<string, object> GetImportMetadata(ImportType importType, IAttributedImport attributedImport)
	{
		return GetImportMetadata(importType.ContractType, attributedImport);
	}

	internal static IDictionary<string, object> GetImportMetadata(Type type, IAttributedImport attributedImport)
	{
		Dictionary<string, object> dictionary = null;
		if (type.IsGenericType)
		{
			dictionary = new Dictionary<string, object>();
			if (type.ContainsGenericParameters)
			{
				dictionary["System.ComponentModel.Composition.GenericImportParametersOrderMetadataName"] = GenericServices.GetGenericParametersOrder(type);
			}
			else
			{
				dictionary["System.ComponentModel.Composition.GenericContractName"] = ContractNameServices.GetTypeIdentity(type.GetGenericTypeDefinition());
				dictionary["System.ComponentModel.Composition.GenericParameters"] = type.GetGenericArguments();
			}
		}
		if (attributedImport != null && attributedImport.Source != ImportSource.Any)
		{
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, object>();
			}
			dictionary["System.ComponentModel.Composition.ImportSource"] = attributedImport.Source;
		}
		if (dictionary != null)
		{
			return dictionary.AsReadOnly();
		}
		return MetadataServices.EmptyMetadata;
	}

	internal static object GetExportedValueFromComposedPart(ImportEngine engine, ComposablePart part, ExportDefinition definition)
	{
		if (engine != null)
		{
			try
			{
				engine.SatisfyImports(part);
			}
			catch (CompositionException innerException)
			{
				throw ExceptionBuilder.CreateCannotGetExportedValue(part, definition, innerException);
			}
		}
		try
		{
			return part.GetExportedValue(definition);
		}
		catch (ComposablePartException innerException2)
		{
			throw ExceptionBuilder.CreateCannotGetExportedValue(part, definition, innerException2);
		}
	}

	internal static bool IsRecomposable(this ComposablePart part)
	{
		return part.ImportDefinitions.Any((ImportDefinition import) => import.IsRecomposable);
	}

	internal static CompositionResult TryInvoke(Action action)
	{
		try
		{
			action();
			return CompositionResult.SucceededResult;
		}
		catch (CompositionException ex)
		{
			return new CompositionResult(ex.Errors);
		}
	}

	internal static CompositionResult TryFire<TEventArgs>(EventHandler<TEventArgs> _delegate, object sender, TEventArgs e) where TEventArgs : EventArgs
	{
		CompositionResult result = CompositionResult.SucceededResult;
		Delegate[] invocationList = _delegate.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			EventHandler<TEventArgs> eventHandler = (EventHandler<TEventArgs>)invocationList[i];
			try
			{
				eventHandler(sender, e);
			}
			catch (CompositionException ex)
			{
				result = result.MergeErrors(ex.Errors);
			}
		}
		return result;
	}

	internal static CreationPolicy GetRequiredCreationPolicy(this ImportDefinition definition)
	{
		if (definition is ContractBasedImportDefinition contractBasedImportDefinition)
		{
			return contractBasedImportDefinition.RequiredCreationPolicy;
		}
		return CreationPolicy.Any;
	}

	internal static bool IsAtMostOne(this ImportCardinality cardinality)
	{
		if (cardinality != ImportCardinality.ZeroOrOne)
		{
			return cardinality == ImportCardinality.ExactlyOne;
		}
		return true;
	}

	private static bool IsValidAttributeType(Type type)
	{
		return IsValidAttributeType(type, arrayAllowed: true);
	}

	private static bool IsValidAttributeType(Type type, bool arrayAllowed)
	{
		Assumes.NotNull(type);
		if (type.IsPrimitive)
		{
			return true;
		}
		if (type == typeof(string))
		{
			return true;
		}
		if (type.IsEnum && type.IsVisible)
		{
			return true;
		}
		if (typeof(Type).IsAssignableFrom(type))
		{
			return true;
		}
		if (arrayAllowed && type.IsArray && type.GetArrayRank() == 1 && IsValidAttributeType(type.GetElementType(), arrayAllowed: false))
		{
			return true;
		}
		return false;
	}
}
