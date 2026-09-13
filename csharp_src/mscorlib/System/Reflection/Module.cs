using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace System.Reflection;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public abstract class Module : ICustomAttributeProvider, ISerializable
{
	public static readonly TypeFilter FilterTypeName = FilterTypeNameImpl;

	public static readonly TypeFilter FilterTypeNameIgnoreCase = FilterTypeNameIgnoreCaseImpl;

	private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

	public virtual Assembly Assembly
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	public virtual string FullyQualifiedName
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	public virtual string Name
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	public virtual int MDStreamVersion
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	public virtual Guid ModuleVersionId
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	public virtual string ScopeName
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	public ModuleHandle ModuleHandle => GetModuleHandleImpl();

	public virtual IEnumerable<CustomAttributeData> CustomAttributes => GetCustomAttributesData();

	public virtual int MetadataToken
	{
		get
		{
			throw NotImplemented.ByDesign;
		}
	}

	internal Guid MvId => GetModuleVersionId();

	internal virtual ModuleHandle GetModuleHandleImpl()
	{
		return ModuleHandle.EmptyHandle;
	}

	public virtual void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual bool IsResource()
	{
		throw NotImplemented.ByDesign;
	}

	public virtual bool IsDefined(Type attributeType, bool inherit)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw NotImplemented.ByDesign;
	}

	public virtual object[] GetCustomAttributes(bool inherit)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw NotImplemented.ByDesign;
	}

	public MethodInfo GetMethod(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return GetMethodImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, null, null);
	}

	public MethodInfo GetMethod(string name, Type[] types)
	{
		return GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, CallingConventions.Any, types, null);
	}

	public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (types == null)
		{
			throw new ArgumentNullException("types");
		}
		for (int i = 0; i < types.Length; i++)
		{
			if (types[i] == null)
			{
				throw new ArgumentNullException("types");
			}
		}
		return GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
	}

	protected virtual MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw NotImplemented.ByDesign;
	}

	public MethodInfo[] GetMethods()
	{
		return GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
	}

	public virtual MethodInfo[] GetMethods(BindingFlags bindingFlags)
	{
		throw NotImplemented.ByDesign;
	}

	public FieldInfo GetField(string name)
	{
		return GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
	}

	public virtual FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		throw NotImplemented.ByDesign;
	}

	public FieldInfo[] GetFields()
	{
		return GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
	}

	public virtual FieldInfo[] GetFields(BindingFlags bindingFlags)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual Type[] GetTypes()
	{
		throw NotImplemented.ByDesign;
	}

	public virtual Type GetType(string className)
	{
		return GetType(className, throwOnError: false, ignoreCase: false);
	}

	public virtual Type GetType(string className, bool ignoreCase)
	{
		return GetType(className, throwOnError: false, ignoreCase);
	}

	public virtual Type GetType(string className, bool throwOnError, bool ignoreCase)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual Type[] FindTypes(TypeFilter filter, object filterCriteria)
	{
		Type[] types = GetTypes();
		int num = 0;
		for (int i = 0; i < types.Length; i++)
		{
			if (filter != null && !filter(types[i], filterCriteria))
			{
				types[i] = null;
			}
			else
			{
				num++;
			}
		}
		if (num == types.Length)
		{
			return types;
		}
		Type[] array = new Type[num];
		num = 0;
		for (int j = 0; j < types.Length; j++)
		{
			if (types[j] != null)
			{
				array[num++] = types[j];
			}
		}
		return array;
	}

	public FieldInfo ResolveField(int metadataToken)
	{
		return ResolveField(metadataToken, null, null);
	}

	public virtual FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		throw NotImplemented.ByDesign;
	}

	public MemberInfo ResolveMember(int metadataToken)
	{
		return ResolveMember(metadataToken, null, null);
	}

	public virtual MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		throw NotImplemented.ByDesign;
	}

	public MethodBase ResolveMethod(int metadataToken)
	{
		return ResolveMethod(metadataToken, null, null);
	}

	public virtual MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual byte[] ResolveSignature(int metadataToken)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual string ResolveString(int metadataToken)
	{
		throw NotImplemented.ByDesign;
	}

	public Type ResolveType(int metadataToken)
	{
		return ResolveType(metadataToken, null, null);
	}

	public virtual Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		throw NotImplemented.ByDesign;
	}

	public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		throw NotImplemented.ByDesign;
	}

	public override bool Equals(object o)
	{
		return base.Equals(o);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(Module left, Module right)
	{
		if ((object)left == right)
		{
			return true;
		}
		if ((object)left == null || (object)right == null)
		{
			return false;
		}
		return left.Equals(right);
	}

	public static bool operator !=(Module left, Module right)
	{
		return !(left == right);
	}

	public override string ToString()
	{
		return ScopeName;
	}

	private static bool FilterTypeNameImpl(Type cls, object filterCriteria)
	{
		if (filterCriteria == null || !(filterCriteria is string))
		{
			throw new InvalidFilterCriteriaException("A String must be provided for the filter criteria.");
		}
		string text = (string)filterCriteria;
		if (text.Length > 0 && text[text.Length - 1] == '*')
		{
			text = text.Substring(0, text.Length - 1);
			return cls.Name.StartsWith(text, StringComparison.Ordinal);
		}
		return cls.Name.Equals(text);
	}

	private static bool FilterTypeNameIgnoreCaseImpl(Type cls, object filterCriteria)
	{
		if (filterCriteria == null || !(filterCriteria is string))
		{
			throw new InvalidFilterCriteriaException("A String must be provided for the filter criteria.");
		}
		string text = (string)filterCriteria;
		if (text.Length > 0 && text[text.Length - 1] == '*')
		{
			text = text.Substring(0, text.Length - 1);
			string name = cls.Name;
			if (name.Length >= text.Length)
			{
				return string.Compare(name, 0, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
			}
			return false;
		}
		return string.Compare(text, cls.Name, StringComparison.OrdinalIgnoreCase) == 0;
	}

	internal static Guid Mono_GetGuid(Module module)
	{
		return module.GetModuleVersionId();
	}

	internal virtual Guid GetModuleVersionId()
	{
		throw new NotImplementedException();
	}

	public virtual X509Certificate GetSignerCertificate()
	{
		throw NotImplemented.ByDesign;
	}
}
