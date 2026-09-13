namespace Leopotam.EcsLite;

public interface TEcsPoolDelegate<T> : IEcsPoolDelegate, IEcsAutoReset<T>, IEcsAutoCopy<T>, IEcsAutoSnapshot<T> where T : struct
{
}
