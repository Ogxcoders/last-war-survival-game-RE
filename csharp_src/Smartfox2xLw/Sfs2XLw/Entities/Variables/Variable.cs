using Sfs2XLw.Entities.Data;

namespace Sfs2XLw.Entities.Variables;

public interface Variable
{
	string Name { get; }

	VariableType Type { get; }

	object Value { get; }

	bool GetBoolValue();

	int GetIntValue();

	double GetDoubleValue();

	string GetStringValue();

	ISFSObject GetSFSObjectValue();

	ISFSArray GetSFSArrayValue();

	bool IsNull();

	ISFSArray ToSFSArray();
}
