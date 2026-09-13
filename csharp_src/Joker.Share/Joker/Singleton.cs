using System;

namespace Joker;

public abstract class Singleton<T> where T : class, new()
{
	private static T _msInstance;

	public bool IsDisposed { get; private set; }

	public static T Instance => _msInstance;

	public static bool IsValid => _msInstance != null;

	protected Singleton()
	{
		if (_msInstance == null)
		{
			_msInstance = this as T;
			return;
		}
		throw new Exception("Singleton already initialized");
	}
}
