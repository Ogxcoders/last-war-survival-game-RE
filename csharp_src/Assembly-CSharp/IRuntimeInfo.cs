public interface IRuntimeInfo
{
	string name { get; }

	string deviceModel { get; }

	DeviceLevel deviceLevel { get; }

	string operatingSystem { get; }

	float deviceScore { get; }

	void Init();

	void LogRunTimeInfo();
}
