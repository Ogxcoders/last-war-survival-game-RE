namespace Sfs2XLw.Entities.Variables;

public interface RoomVariable : Variable
{
	bool IsPrivate { get; set; }

	bool IsPersistent { get; set; }
}
