using System.Collections.Generic;
using System.Reflection;
using System.Security;

namespace System.Runtime.Serialization;

internal class DataMember
{
	private class CriticalHelper
	{
		private DataContract memberTypeContract;

		private string name;

		private int order;

		private bool isRequired;

		private bool emitDefaultValue;

		private bool isNullable;

		private bool isGetOnlyCollection;

		private MemberInfo memberInfo;

		private bool hasConflictingNameAndType;

		private DataMember conflictingMember;

		internal MemberInfo MemberInfo => memberInfo;

		internal string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		internal int Order
		{
			get
			{
				return order;
			}
			set
			{
				order = value;
			}
		}

		internal bool IsRequired
		{
			get
			{
				return isRequired;
			}
			set
			{
				isRequired = value;
			}
		}

		internal bool EmitDefaultValue
		{
			get
			{
				return emitDefaultValue;
			}
			set
			{
				emitDefaultValue = value;
			}
		}

		internal bool IsNullable
		{
			get
			{
				return isNullable;
			}
			set
			{
				isNullable = value;
			}
		}

		internal bool IsGetOnlyCollection
		{
			get
			{
				return isGetOnlyCollection;
			}
			set
			{
				isGetOnlyCollection = value;
			}
		}

		internal Type MemberType
		{
			get
			{
				FieldInfo fieldInfo = MemberInfo as FieldInfo;
				if (fieldInfo != null)
				{
					return fieldInfo.FieldType;
				}
				return ((PropertyInfo)MemberInfo).PropertyType;
			}
		}

		internal DataContract MemberTypeContract
		{
			get
			{
				if (memberTypeContract == null && MemberInfo != null)
				{
					if (IsGetOnlyCollection)
					{
						memberTypeContract = DataContract.GetGetOnlyCollectionDataContract(DataContract.GetId(MemberType.TypeHandle), MemberType.TypeHandle, MemberType, SerializationMode.SharedContract);
					}
					else
					{
						memberTypeContract = DataContract.GetDataContract(MemberType);
					}
				}
				return memberTypeContract;
			}
			set
			{
				memberTypeContract = value;
			}
		}

		internal bool HasConflictingNameAndType
		{
			get
			{
				return hasConflictingNameAndType;
			}
			set
			{
				hasConflictingNameAndType = value;
			}
		}

		internal DataMember ConflictingMember
		{
			get
			{
				return conflictingMember;
			}
			set
			{
				conflictingMember = value;
			}
		}

		internal CriticalHelper()
		{
			emitDefaultValue = true;
		}

		internal CriticalHelper(MemberInfo memberInfo)
		{
			emitDefaultValue = true;
			this.memberInfo = memberInfo;
		}

		internal CriticalHelper(string name)
		{
			Name = name;
		}

		internal CriticalHelper(DataContract memberTypeContract, string name, bool isNullable, bool isRequired, bool emitDefaultValue, int order)
		{
			MemberTypeContract = memberTypeContract;
			Name = name;
			IsNullable = isNullable;
			IsRequired = isRequired;
			EmitDefaultValue = emitDefaultValue;
			Order = order;
		}
	}

	[SecurityCritical]
	private CriticalHelper helper;

	internal MemberInfo MemberInfo
	{
		[SecuritySafeCritical]
		get
		{
			return helper.MemberInfo;
		}
	}

	internal string Name
	{
		[SecuritySafeCritical]
		get
		{
			return helper.Name;
		}
		[SecurityCritical]
		set
		{
			helper.Name = value;
		}
	}

	internal int Order
	{
		[SecuritySafeCritical]
		get
		{
			return helper.Order;
		}
		[SecurityCritical]
		set
		{
			helper.Order = value;
		}
	}

	internal bool IsRequired
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsRequired;
		}
		[SecurityCritical]
		set
		{
			helper.IsRequired = value;
		}
	}

	internal bool EmitDefaultValue
	{
		[SecuritySafeCritical]
		get
		{
			return helper.EmitDefaultValue;
		}
		[SecurityCritical]
		set
		{
			helper.EmitDefaultValue = value;
		}
	}

	internal bool IsNullable
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsNullable;
		}
		[SecurityCritical]
		set
		{
			helper.IsNullable = value;
		}
	}

	internal bool IsGetOnlyCollection
	{
		[SecuritySafeCritical]
		get
		{
			return helper.IsGetOnlyCollection;
		}
		[SecurityCritical]
		set
		{
			helper.IsGetOnlyCollection = value;
		}
	}

	internal Type MemberType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.MemberType;
		}
	}

	internal DataContract MemberTypeContract
	{
		[SecuritySafeCritical]
		get
		{
			return helper.MemberTypeContract;
		}
		[SecurityCritical]
		set
		{
			helper.MemberTypeContract = value;
		}
	}

	internal bool HasConflictingNameAndType
	{
		[SecuritySafeCritical]
		get
		{
			return helper.HasConflictingNameAndType;
		}
		[SecurityCritical]
		set
		{
			helper.HasConflictingNameAndType = value;
		}
	}

	internal DataMember ConflictingMember
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ConflictingMember;
		}
		[SecurityCritical]
		set
		{
			helper.ConflictingMember = value;
		}
	}

	[SecuritySafeCritical]
	internal DataMember()
	{
		helper = new CriticalHelper();
	}

	[SecuritySafeCritical]
	internal DataMember(MemberInfo memberInfo)
	{
		helper = new CriticalHelper(memberInfo);
	}

	[SecuritySafeCritical]
	internal DataMember(string name)
	{
		helper = new CriticalHelper(name);
	}

	[SecuritySafeCritical]
	internal DataMember(DataContract memberTypeContract, string name, bool isNullable, bool isRequired, bool emitDefaultValue, int order)
	{
		helper = new CriticalHelper(memberTypeContract, name, isNullable, isRequired, emitDefaultValue, order);
	}

	internal DataMember BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
	{
		DataContract dataContract = MemberTypeContract.BindGenericParameters(paramContracts, boundContracts);
		return new DataMember(dataContract, Name, !dataContract.IsValueType, IsRequired, EmitDefaultValue, Order);
	}

	internal bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
	{
		if (this == other)
		{
			return true;
		}
		if (other is DataMember dataMember)
		{
			bool flag = MemberTypeContract != null && !MemberTypeContract.IsValueType;
			bool flag2 = dataMember.MemberTypeContract != null && !dataMember.MemberTypeContract.IsValueType;
			if (Name == dataMember.Name && (IsNullable || flag) == (dataMember.IsNullable || flag2) && IsRequired == dataMember.IsRequired && EmitDefaultValue == dataMember.EmitDefaultValue)
			{
				return MemberTypeContract.Equals(dataMember.MemberTypeContract, checkedContracts);
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
