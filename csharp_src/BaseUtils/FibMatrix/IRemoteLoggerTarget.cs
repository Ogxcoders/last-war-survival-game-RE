namespace FibMatrix;

public interface IRemoteLoggerTarget : ILoggerTarget
{
	void Update();

	void UpdateUserInfo(string userId, string serverId);

	void UpdateResVersion(string resVersion);

	void UpdateCountry(string country);

	void Dispose();
}
