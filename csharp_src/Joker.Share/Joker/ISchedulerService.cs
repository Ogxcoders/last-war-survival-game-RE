namespace Joker;

public interface ISchedulerService : IService
{
	IScheduler Create(ESchedulerType type, int id = -1);

	IScheduler Find(int id);
}
