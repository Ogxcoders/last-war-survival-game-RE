using System.Collections.Generic;
using System.Security;

namespace System.Runtime.Serialization;

internal sealed class GenericParameterDataContract : DataContract
{
	private class GenericParameterDataContractCriticalHelper : DataContractCriticalHelper
	{
		private int parameterPosition;

		internal int ParameterPosition => parameterPosition;

		internal GenericParameterDataContractCriticalHelper(Type type)
			: base(type)
		{
			SetDataContractName(DataContract.GetStableName(type));
			parameterPosition = type.GenericParameterPosition;
		}
	}

	[SecurityCritical]
	private GenericParameterDataContractCriticalHelper helper;

	internal int ParameterPosition
	{
		[SecuritySafeCritical]
		get
		{
			return helper.ParameterPosition;
		}
	}

	internal override bool IsBuiltInDataContract => true;

	[SecuritySafeCritical]
	internal GenericParameterDataContract(Type type)
		: base(new GenericParameterDataContractCriticalHelper(type))
	{
		helper = base.Helper as GenericParameterDataContractCriticalHelper;
	}

	internal override DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
	{
		return paramContracts[ParameterPosition];
	}
}
