using System.Collections;
using System.Threading;

namespace System.Runtime.Collections;

internal class HopperCache
{
	private class LastHolder
	{
		private readonly object key;

		private readonly object value;

		internal object Key => key;

		internal object Value => value;

		internal LastHolder(object key, object value)
		{
			this.key = key;
			this.value = value;
		}
	}

	private readonly int hopperSize;

	private readonly bool weak;

	private Hashtable outstandingHopper;

	private Hashtable strongHopper;

	private Hashtable limitedHopper;

	private int promoting;

	private LastHolder mruEntry;

	public HopperCache(int hopperSize, bool weak)
	{
		this.hopperSize = hopperSize;
		this.weak = weak;
		outstandingHopper = new Hashtable(hopperSize * 2);
		strongHopper = new Hashtable(hopperSize * 2);
		limitedHopper = new Hashtable(hopperSize * 2);
	}

	public void Add(object key, object value)
	{
		if (weak && value != DBNull.Value)
		{
			value = new WeakReference(value);
		}
		if (strongHopper.Count >= hopperSize * 2)
		{
			Hashtable hashtable = limitedHopper;
			hashtable.Clear();
			hashtable.Add(key, value);
			try
			{
				return;
			}
			finally
			{
				limitedHopper = strongHopper;
				strongHopper = hashtable;
			}
		}
		strongHopper[key] = value;
	}

	public object GetValue(object syncObject, object key)
	{
		LastHolder lastHolder = mruEntry;
		if (lastHolder != null && key.Equals(lastHolder.Key))
		{
			if (!weak || !(lastHolder.Value is WeakReference { Target: var target }))
			{
				return lastHolder.Value;
			}
			if (target != null)
			{
				return target;
			}
			mruEntry = null;
		}
		object obj = outstandingHopper[key];
		object obj2 = ((weak && obj is WeakReference weakReference2) ? weakReference2.Target : obj);
		if (obj2 != null)
		{
			mruEntry = new LastHolder(key, obj);
			return obj2;
		}
		obj = strongHopper[key];
		obj2 = ((weak && obj is WeakReference weakReference3) ? weakReference3.Target : obj);
		if (obj2 == null)
		{
			obj = limitedHopper[key];
			obj2 = ((weak && obj is WeakReference weakReference4) ? weakReference4.Target : obj);
			if (obj2 == null)
			{
				return null;
			}
		}
		mruEntry = new LastHolder(key, obj);
		int num = 1;
		try
		{
			try
			{
			}
			finally
			{
				num = Interlocked.CompareExchange(ref promoting, 1, 0);
			}
			if (num == 0)
			{
				if (outstandingHopper.Count >= hopperSize)
				{
					lock (syncObject)
					{
						Hashtable hashtable = limitedHopper;
						hashtable.Clear();
						hashtable.Add(key, obj);
						try
						{
						}
						finally
						{
							limitedHopper = strongHopper;
							strongHopper = outstandingHopper;
							outstandingHopper = hashtable;
						}
					}
				}
				else
				{
					outstandingHopper[key] = obj;
				}
			}
		}
		finally
		{
			if (num == 0)
			{
				promoting = 0;
			}
		}
		return obj2;
	}
}
