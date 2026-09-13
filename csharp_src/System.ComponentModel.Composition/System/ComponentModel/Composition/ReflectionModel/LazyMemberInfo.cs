using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

public struct LazyMemberInfo
{
	private readonly MemberTypes _memberType;

	private MemberInfo[] _accessors;

	private readonly Func<MemberInfo[]> _accessorsCreator;

	public MemberTypes MemberType => _memberType;

	public LazyMemberInfo(MemberInfo member)
	{
		Requires.NotNull(member, "member");
		EnsureSupportedMemberType(member.MemberType, "member");
		_accessorsCreator = null;
		_memberType = member.MemberType;
		switch (_memberType)
		{
		case MemberTypes.Property:
		{
			PropertyInfo propertyInfo = (PropertyInfo)member;
			Assumes.NotNull(propertyInfo);
			_accessors = new MemberInfo[2]
			{
				propertyInfo.GetGetMethod(nonPublic: true),
				propertyInfo.GetSetMethod(nonPublic: true)
			};
			break;
		}
		case MemberTypes.Event:
		{
			EventInfo eventInfo = (EventInfo)member;
			_accessors = new MemberInfo[3]
			{
				eventInfo.GetRaiseMethod(nonPublic: true),
				eventInfo.GetAddMethod(nonPublic: true),
				eventInfo.GetRemoveMethod(nonPublic: true)
			};
			break;
		}
		default:
			_accessors = new MemberInfo[1] { member };
			break;
		}
	}

	public LazyMemberInfo(MemberTypes memberType, params MemberInfo[] accessors)
	{
		EnsureSupportedMemberType(memberType, "memberType");
		Requires.NotNull(accessors, "accessors");
		if (!AreAccessorsValid(memberType, accessors, out var errorMessage))
		{
			throw new ArgumentException(errorMessage, "accessors");
		}
		_memberType = memberType;
		_accessors = accessors;
		_accessorsCreator = null;
	}

	public LazyMemberInfo(MemberTypes memberType, Func<MemberInfo[]> accessorsCreator)
	{
		EnsureSupportedMemberType(memberType, "memberType");
		Requires.NotNull(accessorsCreator, "accessorsCreator");
		_memberType = memberType;
		_accessors = null;
		_accessorsCreator = accessorsCreator;
	}

	public MemberInfo[] GetAccessors()
	{
		if (_accessors == null && _accessorsCreator != null)
		{
			MemberInfo[] accessors = _accessorsCreator();
			if (!AreAccessorsValid(MemberType, accessors, out var errorMessage))
			{
				throw new InvalidOperationException(errorMessage);
			}
			_accessors = accessors;
		}
		return _accessors;
	}

	public override int GetHashCode()
	{
		if (_accessorsCreator != null)
		{
			return MemberType.GetHashCode() ^ _accessorsCreator.GetHashCode();
		}
		Assumes.NotNull(_accessors);
		Assumes.NotNull(_accessors[0]);
		return MemberType.GetHashCode() ^ _accessors[0].GetHashCode();
	}

	public override bool Equals(object obj)
	{
		LazyMemberInfo lazyMemberInfo = (LazyMemberInfo)obj;
		if (_memberType != lazyMemberInfo._memberType)
		{
			return false;
		}
		if (_accessorsCreator != null || lazyMemberInfo._accessorsCreator != null)
		{
			return object.Equals(_accessorsCreator, lazyMemberInfo._accessorsCreator);
		}
		Assumes.NotNull(_accessors);
		Assumes.NotNull(lazyMemberInfo._accessors);
		return _accessors.SequenceEqual(lazyMemberInfo._accessors);
	}

	public static bool operator ==(LazyMemberInfo left, LazyMemberInfo right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(LazyMemberInfo left, LazyMemberInfo right)
	{
		return !left.Equals(right);
	}

	private static void EnsureSupportedMemberType(MemberTypes memberType, string argument)
	{
		MemberTypes enumFlagSet = MemberTypes.All;
		Requires.IsInMembertypeSet(memberType, argument, enumFlagSet);
	}

	private static bool AreAccessorsValid(MemberTypes memberType, MemberInfo[] accessors, out string errorMessage)
	{
		errorMessage = string.Empty;
		if (accessors == null)
		{
			errorMessage = Strings.LazyMemberInfo_AccessorsNull;
			return false;
		}
		if (accessors.All((MemberInfo accessor) => accessor == null))
		{
			errorMessage = Strings.LazyMemberInfo_NoAccessors;
			return false;
		}
		switch (memberType)
		{
		case MemberTypes.Property:
			if (accessors.Length != 2)
			{
				errorMessage = Strings.LazyMemberInfo_InvalidPropertyAccessors_Cardinality;
				return false;
			}
			if (accessors.Where((MemberInfo accessor) => accessor != null && accessor.MemberType != MemberTypes.Method).Any())
			{
				errorMessage = Strings.LazyMemberinfo_InvalidPropertyAccessors_AccessorType;
				return false;
			}
			break;
		case MemberTypes.Event:
			if (accessors.Length != 3)
			{
				errorMessage = Strings.LazyMemberInfo_InvalidEventAccessors_Cardinality;
				return false;
			}
			if (accessors.Where((MemberInfo accessor) => accessor != null && accessor.MemberType != MemberTypes.Method).Any())
			{
				errorMessage = Strings.LazyMemberinfo_InvalidEventAccessors_AccessorType;
				return false;
			}
			break;
		default:
			if (accessors.Length != 1 || (accessors.Length == 1 && accessors[0].MemberType != memberType))
			{
				errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.LazyMemberInfo_InvalidAccessorOnSimpleMember, memberType);
				return false;
			}
			break;
		}
		return true;
	}
}
