using System.Collections.Generic;

namespace Sfs2XLw.Entities.Managers;

public class SFSUserManager : IUserManager
{
	private Dictionary<string, User> usersByName;

	private Dictionary<int, User> usersById;

	private readonly object listLock = new object();

	protected Room room;

	protected SmartFox sfs;

	public int UserCount
	{
		get
		{
			lock (listLock)
			{
				return usersById.Count;
			}
		}
	}

	public SmartFox SmartFoxClient => sfs;

	protected void LogWarn(string msg)
	{
		if (sfs != null)
		{
			sfs.Log.Warn(msg);
		}
		else if (room != null && room.RoomManager != null)
		{
			room.RoomManager.SmartFoxClient.Log.Warn(msg);
		}
	}

	public SFSUserManager(SmartFox sfs)
	{
		this.sfs = sfs;
		usersByName = new Dictionary<string, User>();
		usersById = new Dictionary<int, User>();
	}

	public SFSUserManager(Room room)
	{
		this.room = room;
		usersByName = new Dictionary<string, User>();
		usersById = new Dictionary<int, User>();
	}

	public bool ContainsUserName(string userName)
	{
		lock (listLock)
		{
			return usersByName.ContainsKey(userName);
		}
	}

	public bool ContainsUserId(int userId)
	{
		lock (listLock)
		{
			return usersById.ContainsKey(userId);
		}
	}

	public bool ContainsUser(User user)
	{
		lock (listLock)
		{
			return usersByName.ContainsValue(user);
		}
	}

	public User GetUserByName(string userName)
	{
		lock (listLock)
		{
			try
			{
				return usersByName[userName];
			}
			catch (KeyNotFoundException)
			{
				return null;
			}
		}
	}

	public User GetUserById(int userId)
	{
		lock (listLock)
		{
			try
			{
				return usersById[userId];
			}
			catch (KeyNotFoundException)
			{
				return null;
			}
		}
	}

	public virtual void AddUser(User user)
	{
		lock (listLock)
		{
			if (ContainsUserId(user.Id))
			{
				LogWarn("Unexpected: duplicate user in UserManager: " + user);
			}
			AddUserInternal(user);
		}
	}

	protected void AddUserInternal(User user)
	{
		lock (listLock)
		{
			usersByName[user.Name] = user;
			usersById[user.Id] = user;
		}
	}

	public virtual void RemoveUser(User user)
	{
		lock (listLock)
		{
			usersByName.Remove(user.Name);
			usersById.Remove(user.Id);
		}
	}

	public void RemoveUserById(int id)
	{
		lock (listLock)
		{
			if (ContainsUserId(id))
			{
				User user = usersById[id];
				RemoveUser(user);
			}
		}
	}

	public List<User> GetUserList()
	{
		lock (listLock)
		{
			return new List<User>(usersById.Values);
		}
	}

	public void ReplaceAll(List<User> newUserList)
	{
		lock (listLock)
		{
			usersByName.Clear();
			usersById.Clear();
			foreach (User newUser in newUserList)
			{
				AddUser(newUser);
			}
		}
	}
}
