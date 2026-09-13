using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace System.Data.SqlClient;

internal sealed class SqlStatistics
{
	private sealed class StatisticsDictionary : Dictionary<object, object>, IDictionary, ICollection, IEnumerable
	{
		private sealed class Collection : ICollection, IEnumerable
		{
			private readonly StatisticsDictionary _dictionary;

			private readonly ICollection _collection;

			int ICollection.Count => _collection.Count;

			bool ICollection.IsSynchronized => _collection.IsSynchronized;

			object ICollection.SyncRoot => _collection.SyncRoot;

			public Collection(StatisticsDictionary dictionary, ICollection collection)
			{
				_dictionary = dictionary;
				_collection = collection;
			}

			void ICollection.CopyTo(Array array, int arrayIndex)
			{
				if (_collection is KeyCollection)
				{
					_dictionary.CopyKeys(array, arrayIndex);
				}
				else
				{
					_dictionary.CopyValues(array, arrayIndex);
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _collection.GetEnumerator();
			}
		}

		private Collection _keys;

		private Collection _values;

		ICollection IDictionary.Keys => _keys ?? (_keys = new Collection(this, base.Keys));

		ICollection IDictionary.Values => _values ?? (_values = new Collection(this, base.Values));

		public StatisticsDictionary(int capacity)
			: base(capacity)
		{
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary)this).GetEnumerator();
		}

		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			ValidateCopyToArguments(array, arrayIndex);
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<object, object> current = enumerator.Current;
				DictionaryEntry dictionaryEntry = new DictionaryEntry(current.Key, current.Value);
				array.SetValue(dictionaryEntry, arrayIndex++);
			}
		}

		private void CopyKeys(Array array, int arrayIndex)
		{
			ValidateCopyToArguments(array, arrayIndex);
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				array.SetValue(enumerator.Current.Key, arrayIndex++);
			}
		}

		private void CopyValues(Array array, int arrayIndex)
		{
			ValidateCopyToArguments(array, arrayIndex);
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				array.SetValue(enumerator.Current.Value, arrayIndex++);
			}
		}

		private void ValidateCopyToArguments(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Non-negative number required.");
			}
			if (array.Length - arrayIndex < base.Count)
			{
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}
		}
	}

	internal long _closeTimestamp;

	internal long _openTimestamp;

	internal long _startExecutionTimestamp;

	internal long _startFetchTimestamp;

	internal long _startNetworkServerTimestamp;

	internal long _buffersReceived;

	internal long _buffersSent;

	internal long _bytesReceived;

	internal long _bytesSent;

	internal long _connectionTime;

	internal long _cursorOpens;

	internal long _executionTime;

	internal long _iduCount;

	internal long _iduRows;

	internal long _networkServerTime;

	internal long _preparedExecs;

	internal long _prepares;

	internal long _selectCount;

	internal long _selectRows;

	internal long _serverRoundtrips;

	internal long _sumResultSets;

	internal long _transactions;

	internal long _unpreparedExecs;

	private bool _waitForDoneAfterRow;

	private bool _waitForReply;

	internal bool WaitForDoneAfterRow
	{
		get
		{
			return _waitForDoneAfterRow;
		}
		set
		{
			_waitForDoneAfterRow = value;
		}
	}

	internal bool WaitForReply => _waitForReply;

	internal static SqlStatistics StartTimer(SqlStatistics statistics)
	{
		if (statistics != null && !statistics.RequestExecutionTimer())
		{
			statistics = null;
		}
		return statistics;
	}

	internal static void StopTimer(SqlStatistics statistics)
	{
		statistics?.ReleaseAndUpdateExecutionTimer();
	}

	internal SqlStatistics()
	{
	}

	internal void ContinueOnNewConnection()
	{
		_startExecutionTimestamp = 0L;
		_startFetchTimestamp = 0L;
		_waitForDoneAfterRow = false;
		_waitForReply = false;
	}

	internal IDictionary GetDictionary()
	{
		return new StatisticsDictionary(18)
		{
			{ "BuffersReceived", _buffersReceived },
			{ "BuffersSent", _buffersSent },
			{ "BytesReceived", _bytesReceived },
			{ "BytesSent", _bytesSent },
			{ "CursorOpens", _cursorOpens },
			{ "IduCount", _iduCount },
			{ "IduRows", _iduRows },
			{ "PreparedExecs", _preparedExecs },
			{ "Prepares", _prepares },
			{ "SelectCount", _selectCount },
			{ "SelectRows", _selectRows },
			{ "ServerRoundtrips", _serverRoundtrips },
			{ "SumResultSets", _sumResultSets },
			{ "Transactions", _transactions },
			{ "UnpreparedExecs", _unpreparedExecs },
			{
				"ConnectionTime",
				ADP.TimerToMilliseconds(_connectionTime)
			},
			{
				"ExecutionTime",
				ADP.TimerToMilliseconds(_executionTime)
			},
			{
				"NetworkServerTime",
				ADP.TimerToMilliseconds(_networkServerTime)
			}
		};
	}

	internal bool RequestExecutionTimer()
	{
		if (_startExecutionTimestamp == 0L)
		{
			ADP.TimerCurrent(out _startExecutionTimestamp);
			return true;
		}
		return false;
	}

	internal void RequestNetworkServerTimer()
	{
		if (_startNetworkServerTimestamp == 0L)
		{
			ADP.TimerCurrent(out _startNetworkServerTimestamp);
		}
		_waitForReply = true;
	}

	internal void ReleaseAndUpdateExecutionTimer()
	{
		if (_startExecutionTimestamp > 0)
		{
			_executionTime += ADP.TimerCurrent() - _startExecutionTimestamp;
			_startExecutionTimestamp = 0L;
		}
	}

	internal void ReleaseAndUpdateNetworkServerTimer()
	{
		if (_waitForReply && _startNetworkServerTimestamp > 0)
		{
			_networkServerTime += ADP.TimerCurrent() - _startNetworkServerTimestamp;
			_startNetworkServerTimestamp = 0L;
		}
		_waitForReply = false;
	}

	internal void Reset()
	{
		_buffersReceived = 0L;
		_buffersSent = 0L;
		_bytesReceived = 0L;
		_bytesSent = 0L;
		_connectionTime = 0L;
		_cursorOpens = 0L;
		_executionTime = 0L;
		_iduCount = 0L;
		_iduRows = 0L;
		_networkServerTime = 0L;
		_preparedExecs = 0L;
		_prepares = 0L;
		_selectCount = 0L;
		_selectRows = 0L;
		_serverRoundtrips = 0L;
		_sumResultSets = 0L;
		_transactions = 0L;
		_unpreparedExecs = 0L;
		_waitForDoneAfterRow = false;
		_waitForReply = false;
		_startExecutionTimestamp = 0L;
		_startNetworkServerTimestamp = 0L;
	}

	internal void SafeAdd(ref long value, long summand)
	{
		if (long.MaxValue - value > summand)
		{
			value += summand;
		}
		else
		{
			value = long.MaxValue;
		}
	}

	internal long SafeIncrement(ref long value)
	{
		if (value < long.MaxValue)
		{
			value++;
		}
		return value;
	}

	internal void UpdateStatistics()
	{
		if (_closeTimestamp >= _openTimestamp)
		{
			SafeAdd(ref _connectionTime, _closeTimestamp - _openTimestamp);
		}
		else
		{
			_connectionTime = long.MaxValue;
		}
	}
}
