namespace Joker;

public interface ILogService : IService
{
	ILogger GetLogger();

	ILogger GetLogger(string name);
}
