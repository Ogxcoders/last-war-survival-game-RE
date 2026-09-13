using Sfs2XLw.Entities.Data;

namespace Sfs2XLw.Entities.Invitation;

public interface Invitation
{
	int Id { get; set; }

	User Inviter { get; }

	User Invitee { get; }

	int SecondsForAnswer { get; }

	ISFSObject Params { get; }
}
