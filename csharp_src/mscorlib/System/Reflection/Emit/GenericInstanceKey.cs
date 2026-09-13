namespace System.Reflection.Emit;

internal class GenericInstanceKey
{
	private Type gtd;

	internal Type[] args;

	private int hash_code;

	internal GenericInstanceKey(Type gtd, Type[] args)
	{
		this.gtd = gtd;
		this.args = args;
		hash_code = gtd.GetHashCode();
		for (int i = 0; i < args.Length; i++)
		{
			hash_code ^= args[i].GetHashCode();
		}
	}

	private static bool IsBoundedVector(Type type)
	{
		ArrayType arrayType = type as ArrayType;
		if (arrayType != null)
		{
			return arrayType.GetEffectiveRank() == 1;
		}
		return type.ToString().EndsWith("[*]", StringComparison.Ordinal);
	}

	private static bool TypeEquals(Type a, Type b)
	{
		if (a == b)
		{
			return true;
		}
		if (a.HasElementType)
		{
			if (!b.HasElementType)
			{
				return false;
			}
			if (!TypeEquals(a.GetElementType(), b.GetElementType()))
			{
				return false;
			}
			if (a.IsArray)
			{
				if (!b.IsArray)
				{
					return false;
				}
				int arrayRank = a.GetArrayRank();
				if (arrayRank != b.GetArrayRank())
				{
					return false;
				}
				if (arrayRank == 1 && IsBoundedVector(a) != IsBoundedVector(b))
				{
					return false;
				}
			}
			else if (a.IsByRef)
			{
				if (!b.IsByRef)
				{
					return false;
				}
			}
			else if (a.IsPointer && !b.IsPointer)
			{
				return false;
			}
			return true;
		}
		if (a.IsGenericType)
		{
			if (!b.IsGenericType)
			{
				return false;
			}
			if (a.IsGenericParameter)
			{
				return a == b;
			}
			if (a.IsGenericParameter)
			{
				return false;
			}
			if (a.IsGenericTypeDefinition)
			{
				if (!b.IsGenericTypeDefinition)
				{
					return false;
				}
			}
			else
			{
				if (b.IsGenericTypeDefinition)
				{
					return false;
				}
				if (!TypeEquals(a.GetGenericTypeDefinition(), b.GetGenericTypeDefinition()))
				{
					return false;
				}
				Type[] genericArguments = a.GetGenericArguments();
				Type[] genericArguments2 = b.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!TypeEquals(genericArguments[i], genericArguments2[i]))
					{
						return false;
					}
				}
			}
		}
		return a == b;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is GenericInstanceKey genericInstanceKey))
		{
			return false;
		}
		if (gtd != genericInstanceKey.gtd)
		{
			return false;
		}
		for (int i = 0; i < args.Length; i++)
		{
			Type type = args[i];
			Type type2 = genericInstanceKey.args[i];
			if (type != type2 && !type.Equals(type2))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		return hash_code;
	}
}
