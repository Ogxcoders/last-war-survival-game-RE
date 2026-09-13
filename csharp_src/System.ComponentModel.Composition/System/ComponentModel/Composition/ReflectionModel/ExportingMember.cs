using System.ComponentModel.Composition.Primitives;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ExportingMember
{
	private readonly ExportDefinition _definition;

	private readonly ReflectionMember _member;

	private object _cachedValue;

	private volatile bool _isValueCached;

	public bool RequiresInstance => _member.RequiresInstance;

	public ExportDefinition Definition => _definition;

	public ExportingMember(ExportDefinition definition, ReflectionMember member)
	{
		Assumes.NotNull(definition, member);
		_definition = definition;
		_member = member;
	}

	public object GetExportedValue(object instance, object @lock)
	{
		EnsureReadable();
		if (!_isValueCached)
		{
			object value;
			try
			{
				value = _member.GetValue(instance);
			}
			catch (TargetInvocationException ex)
			{
				throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ExportThrewException, _member.GetDisplayName()), Definition.ToElement(), ex.InnerException);
			}
			catch (TargetParameterCountException ex2)
			{
				throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ExportNotValidOnIndexers, _member.GetDisplayName()), Definition.ToElement(), ex2.InnerException);
			}
			lock (@lock)
			{
				if (!_isValueCached)
				{
					_cachedValue = value;
					Thread.MemoryBarrier();
					_isValueCached = true;
				}
			}
		}
		return _cachedValue;
	}

	private void EnsureReadable()
	{
		if (!_member.CanRead)
		{
			throw new ComposablePartException(string.Format(CultureInfo.CurrentCulture, Strings.ReflectionModel_ExportNotReadable, _member.GetDisplayName()), Definition.ToElement());
		}
	}
}
