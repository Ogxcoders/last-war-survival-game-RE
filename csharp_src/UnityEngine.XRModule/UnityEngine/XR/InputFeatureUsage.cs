using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
[NativeConditional("ENABLE_VR")]
[RequiredByNativeCode]
public struct InputFeatureUsage : IEquatable<InputFeatureUsage>
{
	internal string m_Name;

	[NativeName("m_FeatureType")]
	internal InputFeatureType m_InternalType;

	public string name
	{
		get
		{
			return m_Name;
		}
		internal set
		{
			m_Name = value;
		}
	}

	internal InputFeatureType internalType
	{
		get
		{
			return m_InternalType;
		}
		set
		{
			m_InternalType = value;
		}
	}

	public Type type => m_InternalType switch
	{
		InputFeatureType.Custom => typeof(byte[]), 
		InputFeatureType.Binary => typeof(bool), 
		InputFeatureType.DiscreteStates => typeof(uint), 
		InputFeatureType.Axis1D => typeof(float), 
		InputFeatureType.Axis2D => typeof(Vector2), 
		InputFeatureType.Axis3D => typeof(Vector3), 
		InputFeatureType.Rotation => typeof(Quaternion), 
		InputFeatureType.Hand => typeof(Hand), 
		InputFeatureType.Bone => typeof(Bone), 
		InputFeatureType.Eyes => typeof(Eyes), 
		_ => throw new InvalidCastException("No valid managed type for unknown native type."), 
	};

	internal InputFeatureUsage(string name, InputFeatureType type)
	{
		m_Name = name;
		m_InternalType = type;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is InputFeatureUsage))
		{
			return false;
		}
		return Equals((InputFeatureUsage)obj);
	}

	public bool Equals(InputFeatureUsage other)
	{
		return name == other.name && internalType == other.internalType;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode() ^ (internalType.GetHashCode() << 1);
	}

	public static bool operator ==(InputFeatureUsage a, InputFeatureUsage b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(InputFeatureUsage a, InputFeatureUsage b)
	{
		return !(a == b);
	}

	public InputFeatureUsage<T> As<T>()
	{
		if ((object)type != typeof(T))
		{
			throw new ArgumentException("InputFeatureUsage type does not match out variable type.");
		}
		return new InputFeatureUsage<T>(name);
	}
}
public struct InputFeatureUsage<T> : IEquatable<InputFeatureUsage<T>>
{
	public string name { get; set; }

	private Type usageType => typeof(T);

	public InputFeatureUsage(string usageName)
	{
		name = usageName;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is InputFeatureUsage<T>))
		{
			return false;
		}
		return Equals((InputFeatureUsage<T>)obj);
	}

	public bool Equals(InputFeatureUsage<T> other)
	{
		return name == other.name;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	public static bool operator ==(InputFeatureUsage<T> a, InputFeatureUsage<T> b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(InputFeatureUsage<T> a, InputFeatureUsage<T> b)
	{
		return !(a == b);
	}

	public static explicit operator InputFeatureUsage(InputFeatureUsage<T> self)
	{
		InputFeatureType inputFeatureType = InputFeatureType.kUnityXRInputFeatureTypeInvalid;
		Type type = self.usageType;
		if ((object)type == typeof(bool))
		{
			inputFeatureType = InputFeatureType.Binary;
		}
		else if ((object)type == typeof(uint))
		{
			inputFeatureType = InputFeatureType.DiscreteStates;
		}
		else if ((object)type == typeof(float))
		{
			inputFeatureType = InputFeatureType.Axis1D;
		}
		else if ((object)type == typeof(Vector2))
		{
			inputFeatureType = InputFeatureType.Axis2D;
		}
		else if ((object)type == typeof(Vector3))
		{
			inputFeatureType = InputFeatureType.Axis3D;
		}
		else if ((object)type == typeof(Quaternion))
		{
			inputFeatureType = InputFeatureType.Rotation;
		}
		else if ((object)type == typeof(Hand))
		{
			inputFeatureType = InputFeatureType.Hand;
		}
		else if ((object)type == typeof(Bone))
		{
			inputFeatureType = InputFeatureType.Bone;
		}
		else if ((object)type == typeof(Eyes))
		{
			inputFeatureType = InputFeatureType.Eyes;
		}
		else if ((object)type == typeof(byte[]))
		{
			inputFeatureType = InputFeatureType.Custom;
		}
		else if (type.IsEnum)
		{
			inputFeatureType = InputFeatureType.DiscreteStates;
		}
		if (inputFeatureType != InputFeatureType.kUnityXRInputFeatureTypeInvalid)
		{
			return new InputFeatureUsage(self.name, inputFeatureType);
		}
		throw new InvalidCastException("No valid InputFeatureType for " + self.name + ".");
	}
}
