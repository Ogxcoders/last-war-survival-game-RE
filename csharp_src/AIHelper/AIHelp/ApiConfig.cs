namespace AIHelp;

public class ApiConfig
{
	public class Builder
	{
		private string entranceId;

		private string welcomeMessage;

		public Builder SetEntranceId(string entranceId)
		{
			this.entranceId = entranceId;
			return this;
		}

		public Builder SetWelcomeMessage(string welcomeMessage)
		{
			this.welcomeMessage = welcomeMessage;
			return this;
		}

		public ApiConfig build()
		{
			return new ApiConfig(entranceId, welcomeMessage);
		}
	}

	private string entranceId;

	private string welcomeMessage;

	public string GetEntranceId()
	{
		return entranceId;
	}

	public string GetWelcomeMessage()
	{
		return welcomeMessage;
	}

	private ApiConfig(string entranceId, string welcomeMessage)
	{
		this.entranceId = entranceId;
		this.welcomeMessage = welcomeMessage;
	}
}
