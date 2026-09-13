using System;
using System.Threading;

namespace Joker;

public abstract class SchedulerThreadBase : IScheduler, IDisposable
{
	private const int kMaxArrayLength = 2146435071;

	private const int kInitialSize = 16;

	private SpinLock _spin = new SpinLock(enableThreadOwnerTracking: false);

	private bool _dequing;

	private int _actionListCount;

	private Action[] _actionList = new Action[16];

	private int _waitingListCount;

	private Action[] _waitingList = new Action[16];

	public bool IsDisposed { get; private set; }

	public int Id { get; }

	public event Action Updater;

	public event Action LateUpdater;

	public SchedulerThreadBase(ESchedulerType type, int id)
	{
		Id = id;
	}

	public void Post(Action action)
	{
		bool lockTaken = false;
		try
		{
			_spin.Enter(ref lockTaken);
			if (_dequing)
			{
				if (_waitingList.Length == _waitingListCount)
				{
					int num = _waitingListCount * 2;
					if ((uint)num > 2146435071u)
					{
						num = 2146435071;
					}
					Action[] array = new Action[num];
					Array.Copy(_waitingList, array, _waitingListCount);
					_waitingList = array;
				}
				_waitingList[_waitingListCount] = action;
				_waitingListCount++;
				return;
			}
			if (_actionList.Length == _actionListCount)
			{
				int num2 = _actionListCount * 2;
				if ((uint)num2 > 2146435071u)
				{
					num2 = 2146435071;
				}
				Action[] array2 = new Action[num2];
				Array.Copy(_actionList, array2, _actionListCount);
				_actionList = array2;
			}
			_actionList[_actionListCount] = action;
			_actionListCount++;
		}
		finally
		{
			if (lockTaken)
			{
				_spin.Exit(useMemoryBarrier: false);
			}
		}
	}

	public abstract void Start();

	internal virtual void Update()
	{
		RunPostActions();
		this.Updater?.Invoke();
	}

	internal virtual void LateUpdate()
	{
		this.LateUpdater?.Invoke();
	}

	private void RunPostActions()
	{
		bool lockTaken = false;
		try
		{
			_spin.Enter(ref lockTaken);
			if (_actionListCount == 0)
			{
				return;
			}
			_dequing = true;
		}
		finally
		{
			if (lockTaken)
			{
				_spin.Exit(useMemoryBarrier: false);
			}
		}
		for (int i = 0; i < _actionListCount; i++)
		{
			Action action = _actionList[i];
			_actionList[i] = null;
			try
			{
				action();
			}
			catch (Exception e)
			{
				Log.Exception(e);
			}
		}
		bool lockTaken2 = false;
		try
		{
			_spin.Enter(ref lockTaken2);
			_dequing = false;
			Action[] actionList = _actionList;
			_actionListCount = _waitingListCount;
			_actionList = _waitingList;
			_waitingListCount = 0;
			_waitingList = actionList;
		}
		finally
		{
			if (lockTaken2)
			{
				_spin.Exit(useMemoryBarrier: false);
			}
		}
	}

	public virtual void Dispose()
	{
		BaseDispose();
	}

	internal void BaseDispose()
	{
		IsDisposed = true;
		this.Updater = null;
		this.LateUpdater = null;
		for (int i = 0; i < _actionListCount; i++)
		{
			_actionList[i] = null;
		}
		_actionListCount = 0;
		_actionList = null;
		for (int j = 0; j < _waitingListCount; j++)
		{
			_waitingList[j] = null;
		}
		_waitingListCount = 0;
		_waitingList = null;
		Singleton<SchedulerServiceThread>.Instance.Finish(Id);
	}
}
