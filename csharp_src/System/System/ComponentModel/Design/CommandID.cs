using System.Globalization;

namespace System.ComponentModel.Design;

public class CommandID
{
	public virtual int ID { get; }

	public virtual Guid Guid { get; }

	public CommandID(Guid menuGroup, int commandID)
	{
		Guid = menuGroup;
		ID = commandID;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is CommandID))
		{
			return false;
		}
		CommandID commandID = (CommandID)obj;
		if (commandID.Guid.Equals(Guid))
		{
			return commandID.ID == ID;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (Guid.GetHashCode() << 2) | ID;
	}

	public override string ToString()
	{
		return Guid.ToString() + " : " + ID.ToString(CultureInfo.CurrentCulture);
	}
}
