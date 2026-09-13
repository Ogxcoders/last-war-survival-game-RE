using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Reflection;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ImportingMember : ImportingItem
{
	private readonly ReflectionWritableMember _member;

	public ImportingMember(ContractBasedImportDefinition definition, ReflectionWritableMember member, ImportType importType)
		: base(definition, importType)
	{
		Assumes.NotNull(definition, member);
		_member = member;
	}

	public void SetExportedValue(object instance, object value)
	{
		if (RequiresCollectionNormalization())
		{
			SetCollectionMemberValue(instance, (IEnumerable)value);
		}
		else
		{
			SetSingleMemberValue(instance, value);
		}
	}

	private bool RequiresCollectionNormalization()
	{
		if (base.Definition.Cardinality != ImportCardinality.ZeroOrMore)
		{
			return false;
		}
		if (_member.CanWrite && base.ImportType.IsAssignableCollectionType)
		{
			return false;
		}
		return true;
	}

	private void SetSingleMemberValue(object instance, object value)
	{
		EnsureWritable();
		try
		{
			_member.SetValue(instance, value);
		}
		catch (TargetInvocationException ex)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportThrewException, _member.GetDisplayName()), base.Definition.ToElement(), ex.InnerException);
		}
		catch (TargetParameterCountException ex2)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ImportNotValidOnIndexers, _member.GetDisplayName()), base.Definition.ToElement(), ex2.InnerException);
		}
	}

	private void EnsureWritable()
	{
		if (!_member.CanWrite)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportNotWritable, _member.GetDisplayName()), base.Definition.ToElement());
		}
	}

	private void SetCollectionMemberValue(object instance, IEnumerable values)
	{
		Assumes.NotNull(values);
		ICollection<object> collection = null;
		Type collectionElementType = CollectionServices.GetCollectionElementType(base.ImportType.ActualType);
		if (collectionElementType != null)
		{
			collection = GetNormalizedCollection(collectionElementType, instance);
		}
		EnsureCollectionIsWritable(collection);
		PopulateCollection(collection, values);
	}

	private ICollection<object> GetNormalizedCollection(Type itemType, object instance)
	{
		Assumes.NotNull(itemType);
		object obj = null;
		if (_member.CanRead)
		{
			try
			{
				obj = _member.GetValue(instance);
			}
			catch (TargetInvocationException ex)
			{
				throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionGetThrewException, _member.GetDisplayName()), base.Definition.ToElement(), ex.InnerException);
			}
		}
		if (obj == null)
		{
			ConstructorInfo constructor = base.ImportType.ActualType.GetConstructor(Type.EmptyTypes);
			if (constructor != null)
			{
				try
				{
					obj = constructor.SafeInvoke();
				}
				catch (TargetInvocationException ex2)
				{
					throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionConstructionThrewException, _member.GetDisplayName(), base.ImportType.ActualType.FullName), base.Definition.ToElement(), ex2.InnerException);
				}
				SetSingleMemberValue(instance, obj);
			}
		}
		if (obj == null)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionNull, _member.GetDisplayName()), base.Definition.ToElement());
		}
		return CollectionServices.GetCollectionWrapper(itemType, obj);
	}

	private void EnsureCollectionIsWritable(ICollection<object> collection)
	{
		bool flag = true;
		try
		{
			if (collection != null)
			{
				flag = collection.IsReadOnly;
			}
		}
		catch (Exception innerException)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionIsReadOnlyThrewException, _member.GetDisplayName(), collection.GetType().FullName), base.Definition.ToElement(), innerException);
		}
		if (flag)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionNotWritable, _member.GetDisplayName()), base.Definition.ToElement());
		}
	}

	private void PopulateCollection(ICollection<object> collection, IEnumerable values)
	{
		Assumes.NotNull(collection, values);
		try
		{
			collection.Clear();
		}
		catch (Exception innerException)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionClearThrewException, _member.GetDisplayName(), collection.GetType().FullName), base.Definition.ToElement(), innerException);
		}
		foreach (object value in values)
		{
			try
			{
				collection.Add(value);
			}
			catch (Exception innerException2)
			{
				throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ImportCollectionAddThrewException, _member.GetDisplayName(), collection.GetType().FullName), base.Definition.ToElement(), innerException2);
			}
		}
	}
}
