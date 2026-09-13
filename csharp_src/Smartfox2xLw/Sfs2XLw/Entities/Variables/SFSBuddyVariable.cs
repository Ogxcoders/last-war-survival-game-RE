using Sfs2XLw.Entities.Data;

namespace Sfs2XLw.Entities.Variables;

public class SFSBuddyVariable : BaseVariable, BuddyVariable, Variable
{
	public static readonly string OFFLINE_PREFIX = "$";

	public bool IsOffline => name.StartsWith("$");

	public static BuddyVariable FromSFSArray(ISFSArray sfsa)
	{
		return new SFSBuddyVariable(sfsa.GetUtfString(0), sfsa.GetElementAt(2), sfsa.GetByte(1));
	}

	public SFSBuddyVariable(string name, object val, int type)
		: base(name, val, type)
	{
	}

	public SFSBuddyVariable(string name, object val)
		: base(name, val)
	{
	}

	public override string ToString()
	{
		return string.Concat("[BuddyVar: ", name, ", type: ", type, ", value: ", val, "]");
	}
}
