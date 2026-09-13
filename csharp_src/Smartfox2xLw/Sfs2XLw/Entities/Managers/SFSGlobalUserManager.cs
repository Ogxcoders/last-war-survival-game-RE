using System.Collections.Generic;

namespace Sfs2XLw.Entities.Managers;

public class SFSGlobalUserManager : SFSUserManager, IUserManager
{
	private Dictionary<User, int> roomRefCount;

	public SFSGlobalUserManager(SmartFox sfs)
		: base(sfs)
	{
		roomRefCount = new Dictionary<User, int>();
	}

	public SFSGlobalUserManager(Room room)
		: base(room)
	{
		roomRefCount = new Dictionary<User, int>();
	}

	public override void AddUser(User user)
	{
		lock (roomRefCount)
		{
			if (!roomRefCount.ContainsKey(user))
			{
				base.AddUser(user);
				roomRefCount[user] = 1;
			}
			else
			{
				roomRefCount[user]++;
			}
		}
	}

	public override void RemoveUser(User user)
	{
		RemoveUserReference(user, disconnected: false);
	}

	public void RemoveUserReference(User user, bool disconnected)
	{
		lock (roomRefCount)
		{
			if (roomRefCount.ContainsKey(user))
			{
				if (roomRefCount[user] < 1)
				{
					LogWarn("GlobalUserManager RefCount is already at zero. User: " + user);
					return;
				}
				roomRefCount[user]--;
				if (roomRefCount[user] == 0 || disconnected)
				{
					base.RemoveUser(user);
					roomRefCount.Remove(user);
				}
			}
			else
			{
				LogWarn("Can't remove User from GlobalUserManager. RefCount missing. User: " + user);
			}
		}
	}
}
