using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.ReflectionModel;

internal class ReflectionComposablePartDefinition : ComposablePartDefinition, ICompositionElement
{
	private readonly IReflectionPartCreationInfo _creationInfo;

	private volatile IEnumerable<ImportDefinition> _imports;

	private volatile IEnumerable<ExportDefinition> _exports;

	private volatile IDictionary<string, object> _metadata;

	private volatile ConstructorInfo _constructor;

	private object _lock = new object();

	public override IEnumerable<ExportDefinition> ExportDefinitions
	{
		get
		{
			if (_exports == null)
			{
				ExportDefinition[] exports = _creationInfo.GetExports().ToArray();
				lock (_lock)
				{
					if (_exports == null)
					{
						_exports = exports;
					}
				}
			}
			return _exports;
		}
	}

	public override IEnumerable<ImportDefinition> ImportDefinitions
	{
		get
		{
			if (_imports == null)
			{
				ImportDefinition[] imports = _creationInfo.GetImports().ToArray();
				lock (_lock)
				{
					if (_imports == null)
					{
						_imports = imports;
					}
				}
			}
			return _imports;
		}
	}

	public override IDictionary<string, object> Metadata
	{
		get
		{
			if (_metadata == null)
			{
				IDictionary<string, object> metadata = _creationInfo.GetMetadata().AsReadOnly();
				lock (_lock)
				{
					if (_metadata == null)
					{
						_metadata = metadata;
					}
				}
			}
			return _metadata;
		}
	}

	internal bool IsDisposalRequired => _creationInfo.IsDisposalRequired;

	string ICompositionElement.DisplayName => _creationInfo.DisplayName;

	ICompositionElement ICompositionElement.Origin => _creationInfo.Origin;

	public ReflectionComposablePartDefinition(IReflectionPartCreationInfo creationInfo)
	{
		Assumes.NotNull(creationInfo);
		_creationInfo = creationInfo;
	}

	public Type GetPartType()
	{
		return _creationInfo.GetPartType();
	}

	public Lazy<Type> GetLazyPartType()
	{
		return _creationInfo.GetLazyPartType();
	}

	public ConstructorInfo GetConstructor()
	{
		if (_constructor == null)
		{
			ConstructorInfo constructor = _creationInfo.GetConstructor();
			lock (_lock)
			{
				if (_constructor == null)
				{
					_constructor = constructor;
				}
			}
		}
		return _constructor;
	}

	public override ComposablePart CreatePart()
	{
		if (IsDisposalRequired)
		{
			return new DisposableReflectionComposablePart(this);
		}
		return new ReflectionComposablePart(this);
	}

	internal override ComposablePartDefinition GetGenericPartDefinition()
	{
		if (_creationInfo is GenericSpecializationPartCreationInfo genericSpecializationPartCreationInfo)
		{
			return genericSpecializationPartCreationInfo.OriginalPart;
		}
		return null;
	}

	internal override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
	{
		if (this.IsGeneric())
		{
			List<Tuple<ComposablePartDefinition, ExportDefinition>> list = null;
			IEnumerable<object> enumerable = ((definition.Metadata.Count > 0) ? definition.Metadata.GetValue<IEnumerable<object>>("System.ComponentModel.Composition.GenericParameters") : null);
			if (enumerable != null)
			{
				Type[] genericTypeParameters = null;
				if (TryGetGenericTypeParameters(enumerable, out genericTypeParameters))
				{
					foreach (Type[] candidateParameter in GetCandidateParameters(genericTypeParameters))
					{
						ComposablePartDefinition genericPartDefinition = null;
						if (TryMakeGenericPartDefinition(candidateParameter, out genericPartDefinition))
						{
							IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> exports = genericPartDefinition.GetExports(definition);
							if (exports != ComposablePartDefinition._EmptyExports)
							{
								list = list.FastAppendToListAllowNulls(exports);
							}
						}
					}
				}
			}
			IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> enumerable2 = list;
			return enumerable2 ?? ComposablePartDefinition._EmptyExports;
		}
		return base.GetExports(definition);
	}

	private IEnumerable<Type[]> GetCandidateParameters(Type[] genericParameters)
	{
		foreach (ExportDefinition exportDefinition in ExportDefinitions)
		{
			int[] value = exportDefinition.Metadata.GetValue<int[]>("System.ComponentModel.Composition.GenericExportParametersOrderMetadataName");
			if (value != null && value.Length == genericParameters.Length)
			{
				yield return GenericServices.Reorder(genericParameters, value);
			}
		}
	}

	private static bool TryGetGenericTypeParameters(IEnumerable<object> genericParameters, out Type[] genericTypeParameters)
	{
		genericTypeParameters = genericParameters as Type[];
		if (genericTypeParameters == null)
		{
			object[] array = genericParameters.AsArray();
			genericTypeParameters = new Type[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				genericTypeParameters[i] = array[i] as Type;
				if (genericTypeParameters[i] == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	internal bool TryMakeGenericPartDefinition(Type[] genericTypeParameters, out ComposablePartDefinition genericPartDefinition)
	{
		genericPartDefinition = null;
		if (!GenericSpecializationPartCreationInfo.CanSpecialize(Metadata, genericTypeParameters))
		{
			return false;
		}
		genericPartDefinition = new ReflectionComposablePartDefinition(new GenericSpecializationPartCreationInfo(_creationInfo, this, genericTypeParameters));
		return true;
	}

	public override string ToString()
	{
		return _creationInfo.DisplayName;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ReflectionComposablePartDefinition reflectionComposablePartDefinition))
		{
			return false;
		}
		return _creationInfo.Equals(reflectionComposablePartDefinition._creationInfo);
	}

	public override int GetHashCode()
	{
		return _creationInfo.GetHashCode();
	}
}
