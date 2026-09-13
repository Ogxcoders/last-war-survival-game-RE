namespace Leopotam.EcsLite;

public interface IEcsAutoCopy<T> where T : struct
{
	void AutoCopy(ref T src, ref T dst);
}
