using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class TimerEventScheduler : IScheduler
{
	private class TimerEventSchedulerItem : ScheduledItem
	{
		private readonly Action<TimerState> m_TimerUpdateEvent;

		public TimerEventSchedulerItem(Action<TimerState> updateEvent)
		{
			m_TimerUpdateEvent = updateEvent;
		}

		public override void PerformTimerUpdate(TimerState state)
		{
			m_TimerUpdateEvent?.Invoke(state);
		}

		public override string ToString()
		{
			return m_TimerUpdateEvent.ToString();
		}
	}

	private readonly List<ScheduledItem> m_ScheduledItems = new List<ScheduledItem>();

	private bool m_TransactionMode;

	private readonly List<ScheduledItem> m_ScheduleTransactions = new List<ScheduledItem>();

	private readonly HashSet<ScheduledItem> m_UnscheduleTransactions = new HashSet<ScheduledItem>();

	internal bool disableThrottling = false;

	private int m_LastUpdatedIndex = -1;

	public void Schedule(ScheduledItem item)
	{
		if (item == null)
		{
			return;
		}
		if (item == null)
		{
			throw new NotSupportedException("Scheduled Item type is not supported by this scheduler");
		}
		if (m_TransactionMode)
		{
			if (!m_UnscheduleTransactions.Remove(item))
			{
				if (m_ScheduledItems.Contains(item) || m_ScheduleTransactions.Contains(item))
				{
					throw new ArgumentException(string.Concat("Cannot schedule function ", item, " more than once"));
				}
				m_ScheduleTransactions.Add(item);
			}
		}
		else
		{
			if (m_ScheduledItems.Contains(item))
			{
				throw new ArgumentException(string.Concat("Cannot schedule function ", item, " more than once"));
			}
			m_ScheduledItems.Add(item);
		}
	}

	public ScheduledItem ScheduleOnce(Action<TimerState> timerUpdateEvent, long delayMs)
	{
		TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventSchedulerItem(timerUpdateEvent)
		{
			delayMs = delayMs
		};
		Schedule(timerEventSchedulerItem);
		return timerEventSchedulerItem;
	}

	public ScheduledItem ScheduleUntil(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, Func<bool> stopCondition)
	{
		TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventSchedulerItem(timerUpdateEvent)
		{
			delayMs = delayMs,
			intervalMs = intervalMs,
			timerUpdateStopCondition = stopCondition
		};
		Schedule(timerEventSchedulerItem);
		return timerEventSchedulerItem;
	}

	public ScheduledItem ScheduleForDuration(Action<TimerState> timerUpdateEvent, long delayMs, long intervalMs, long durationMs)
	{
		TimerEventSchedulerItem timerEventSchedulerItem = new TimerEventSchedulerItem(timerUpdateEvent)
		{
			delayMs = delayMs,
			intervalMs = intervalMs,
			timerUpdateStopCondition = null
		};
		timerEventSchedulerItem.SetDuration(durationMs);
		Schedule(timerEventSchedulerItem);
		return timerEventSchedulerItem;
	}

	private bool RemovedScheduledItemAt(int index)
	{
		if (index >= 0)
		{
			ScheduledItem scheduledItem = m_ScheduledItems[index];
			m_ScheduledItems.RemoveAt(index);
			return true;
		}
		return false;
	}

	public void Unschedule(ScheduledItem item)
	{
		if (item == null)
		{
			return;
		}
		if (m_TransactionMode)
		{
			if (m_UnscheduleTransactions.Contains(item))
			{
				throw new ArgumentException("Cannot unschedule scheduled function twice" + item);
			}
			if (!m_ScheduleTransactions.Remove(item))
			{
				if (!m_ScheduledItems.Contains(item))
				{
					throw new ArgumentException("Cannot unschedule unknown scheduled function " + item);
				}
				m_UnscheduleTransactions.Add(item);
			}
		}
		else if (!PrivateUnSchedule(item))
		{
			throw new ArgumentException("Cannot unschedule unknown scheduled function " + item);
		}
		item.OnItemUnscheduled();
	}

	private bool PrivateUnSchedule(ScheduledItem sItem)
	{
		return m_ScheduleTransactions.Remove(sItem) || RemovedScheduledItemAt(m_ScheduledItems.IndexOf(sItem));
	}

	public void UpdateScheduledEvents()
	{
		try
		{
			m_TransactionMode = true;
			long num = Panel.TimeSinceStartupMs();
			int count = m_ScheduledItems.Count;
			int num2 = m_LastUpdatedIndex + 1;
			if (num2 >= count)
			{
				num2 = 0;
			}
			for (int i = 0; i < count; i++)
			{
				int num3 = num2 + i;
				if (num3 >= count)
				{
					num3 -= count;
				}
				ScheduledItem scheduledItem = m_ScheduledItems[num3];
				bool flag = false;
				if (num - scheduledItem.delayMs >= scheduledItem.startMs)
				{
					TimerState state = new TimerState
					{
						start = scheduledItem.startMs,
						now = num
					};
					if (!m_UnscheduleTransactions.Contains(scheduledItem))
					{
						scheduledItem.PerformTimerUpdate(state);
					}
					scheduledItem.startMs = num;
					scheduledItem.delayMs = scheduledItem.intervalMs;
					if (scheduledItem.ShouldUnschedule())
					{
						flag = true;
					}
				}
				if ((flag || (scheduledItem.endTimeMs > 0 && num > scheduledItem.endTimeMs)) && !m_UnscheduleTransactions.Contains(scheduledItem))
				{
					Unschedule(scheduledItem);
				}
				m_LastUpdatedIndex = num3;
			}
		}
		finally
		{
			m_TransactionMode = false;
			foreach (ScheduledItem unscheduleTransaction in m_UnscheduleTransactions)
			{
				PrivateUnSchedule(unscheduleTransaction);
			}
			m_UnscheduleTransactions.Clear();
			foreach (ScheduledItem scheduleTransaction in m_ScheduleTransactions)
			{
				Schedule(scheduleTransaction);
			}
			m_ScheduleTransactions.Clear();
		}
	}
}
