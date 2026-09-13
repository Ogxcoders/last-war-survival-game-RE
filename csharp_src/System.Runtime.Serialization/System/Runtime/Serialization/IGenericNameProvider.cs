using System.Collections.Generic;

namespace System.Runtime.Serialization;

internal interface IGenericNameProvider
{
	bool ParametersFromBuiltInNamespaces { get; }

	int GetParameterCount();

	IList<int> GetNestedParameterCounts();

	string GetParameterName(int paramIndex);

	string GetNamespaces();

	string GetGenericTypeName();
}
