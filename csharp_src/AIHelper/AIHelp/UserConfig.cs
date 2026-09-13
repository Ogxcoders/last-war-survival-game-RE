namespace AIHelp;

public class UserConfig
{
	public class Builder
	{
		private string userId;

		private string userName = "anonymous";

		private string serverId = "-1";

		private string userTags = "";

		private string customData = "";

		private bool isSyncCrmInfo;

		public Builder SetUserId(string userId)
		{
			this.userId = userId;
			return this;
		}

		public Builder SetUserName(string userName)
		{
			this.userName = userName;
			return this;
		}

		public Builder SetServerId(string serverId)
		{
			this.serverId = serverId;
			return this;
		}

		public Builder SetUserTags(string userTags)
		{
			this.userTags = userTags;
			return this;
		}

		public Builder SetCustomData(string customDataJsonstring)
		{
			customData = customDataJsonstring;
			return this;
		}

		public Builder SetSyncCrmInfo(bool syncCrmInfo)
		{
			isSyncCrmInfo = syncCrmInfo;
			return this;
		}

		public UserConfig build()
		{
			return new UserConfig(userId, userName, serverId, userTags, customData, isSyncCrmInfo);
		}
	}

	private string userId;

	private string userName;

	private string serverId = "-1";

	private string userTags;

	private string customData;

	private bool isSyncCrmInfo;

	public bool GetWhetherSyncCrmInfo()
	{
		return isSyncCrmInfo;
	}

	public string GetUserId()
	{
		return userId;
	}

	public string GetUserName()
	{
		return userName;
	}

	public string GetServerId()
	{
		return serverId;
	}

	public string GetUserTags()
	{
		return userTags;
	}

	public string GetCustomData()
	{
		return customData;
	}

	private UserConfig(string userId, string userName, string serverId, string userTags, string customData, bool isSyncCrmInfo)
	{
		this.userId = userId;
		this.userName = userName;
		this.serverId = serverId;
		this.userTags = userTags;
		this.customData = customData;
		this.isSyncCrmInfo = isSyncCrmInfo;
	}
}
