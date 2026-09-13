using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BaseUtils;
using GameFramework;
using GameKit.Base;
using UnityEngine;

namespace ProtoBufNet;

public class NetProfiler
{
	private const string PUSH_WORLD_MARCH_NEW = "push.world.march.new";

	private readonly ConcurrentDictionary<INetPacket, NetSendProfiler> _sendData = new ConcurrentDictionary<INetPacket, NetSendProfiler>();

	private readonly ConcurrentDictionary<INetPacket, NetReceiveProfiler> _receiveData = new ConcurrentDictionary<INetPacket, NetReceiveProfiler>();

	private readonly List<int> _pinData = new List<int>();

	private Stopwatch _flushTimer = Stopwatch.StartNew();

	private NetIOService _ioService = new NetIOService(null);

	private ProfilerType _profilerType;

	private Dictionary<string, string> _parameters = new Dictionary<string, string>();

	public string lineName { get; private set; }

	public NetProfiler(NetRawProxy proxy)
	{
		lineName = proxy.proxyName;
		_profilerType = (proxy.parent.IsMainLine() ? ProfilerType.MainLine : ProfilerType.CrossLine);
	}

	public void OnSendPacketBegin(INetPacket packet)
	{
		NetSendProfiler netSendProfiler = NetSendProfiler.Get();
		netSendProfiler.Start();
		_sendData[packet] = netSendProfiler;
	}

	public void OnSendPacketFinish(INetPacket packet)
	{
		if (_sendData.TryGetValue(packet, out var value))
		{
			value.Stop();
		}
	}

	public void OnReceivePacketBegin(INetPacket packet)
	{
		NetReceiveProfiler netReceiveProfiler = NetReceiveProfiler.Get();
		netReceiveProfiler.OnReceivePacketBegin();
		_receiveData[packet] = netReceiveProfiler;
	}

	public void OnReceivePacketFinish(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceivePacketFinish(packet.bodyBuffLength());
		}
	}

	public void OnReceiveDecryptBegin(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveDecryptBegin();
		}
	}

	public void OnReceiveDecryptFinish(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveDecryptFinish();
		}
	}

	public void OnReceiveUncompressBegin(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveUncompressBegin();
		}
	}

	public void OnReceiveUncompressFinish(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveUncompressFinish();
		}
	}

	public void OnReceiveDecodeSFSObjectBegin(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveDecodeSFSObjectBegin();
		}
	}

	public void OnReceiveDecodeSFSObjectFinish(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveDecodeSFSObjectFinish();
		}
	}

	public void OnReceiveDispatchBegin(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveDispatchBegin(packet);
		}
	}

	public void OnReceiveDispatchFinish(INetPacket packet)
	{
		if (_receiveData.TryGetValue(packet, out var value))
		{
			value.OnReceiveDispatchFinish();
		}
	}

	public void RecordPin(int pin)
	{
		_pinData.Add(pin);
	}

	public void Tick()
	{
		if (_ioService != null)
		{
			_ioService.Poll();
		}
	}

	public void Flush(Action<Dictionary<string, string>> cb)
	{
		_parameters.Clear();
		long elapsedMilliseconds = _flushTimer.ElapsedMilliseconds;
		_parameters.Add("lineType", _profilerType.ToString());
		_parameters.Add("lineName", lineName);
		_parameters.Add("window", elapsedMilliseconds.ToString());
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine($"[NetProfiler] [window:{elapsedMilliseconds}ms]");
		if (_pinData.Count > 0)
		{
			_pinData.Sort();
			float num = GetPercentile(_pinData, 90);
			float num2 = GetPercentile(_pinData, 99);
			string arg = ((_pinData.Count() <= 10) ? string.Join(",", _pinData) : $"[{num}~{num2}]ms");
			stringBuilder.AppendLine($"[pin] count:{_pinData.Count} {arg}");
			_parameters.Add("pinP90", num.ToString());
			_parameters.Add("pinP99", num2.ToString());
			_pinData.Clear();
		}
		if (_sendData.Count > 0)
		{
			List<long> sortedData = (from profiler in _sendData.Values
				select profiler.useTime into time
				orderby time
				select time).ToList();
			long percentile = GetPercentile(sortedData, 90);
			long percentile2 = GetPercentile(sortedData, 99);
			int count = _sendData.Count;
			stringBuilder.AppendLine($"[send] count:{count}  [{percentile}~{percentile2}]ms");
			_parameters.Add("sendCount", count.ToString());
			_parameters.Add("sendTimeP90", percentile.ToString());
			_parameters.Add("sendTimeP99", percentile2.ToString());
			foreach (KeyValuePair<INetPacket, NetSendProfiler> sendDatum in _sendData)
			{
				sendDatum.Value.Recycle();
			}
			_sendData.Clear();
		}
		if (_receiveData.Count > 0)
		{
			List<long> sortedData2 = (from profiler in _receiveData.Values
				select profiler.receiveTime into time
				orderby time
				select time).ToList();
			long percentile3 = GetPercentile(sortedData2, 90);
			long percentile4 = GetPercentile(sortedData2, 99);
			long num3 = _receiveData.Values.Sum((NetReceiveProfiler profiler) => profiler.receiveTime);
			List<long> sortedData3 = (from profiler in _receiveData.Values
				select profiler.decryptTime into time
				orderby time
				select time).ToList();
			long percentile5 = GetPercentile(sortedData3, 90);
			long percentile6 = GetPercentile(sortedData3, 99);
			long num4 = _receiveData.Values.Sum((NetReceiveProfiler profiler) => profiler.decryptTime);
			List<long> sortedData4 = (from profiler in _receiveData.Values
				select profiler.uncompressTime into time
				orderby time
				select time).ToList();
			long percentile7 = GetPercentile(sortedData4, 90);
			long percentile8 = GetPercentile(sortedData4, 99);
			long num5 = _receiveData.Values.Sum((NetReceiveProfiler profiler) => profiler.uncompressTime);
			List<long> sortedData5 = (from profiler in _receiveData.Values
				select profiler.sfsObjTime into time
				orderby time
				select time).ToList();
			long percentile9 = GetPercentile(sortedData5, 90);
			long percentile10 = GetPercentile(sortedData5, 99);
			long num6 = _receiveData.Values.Sum((NetReceiveProfiler profiler) => profiler.sfsObjTime);
			long num7 = _receiveData.Values.Sum((NetReceiveProfiler profiler) => profiler.totalTime);
			ulong bytes = (ulong)_receiveData.Values.Sum((NetReceiveProfiler profiler) => profiler.packetSize);
			stringBuilder.AppendLine($"[rcv] count:{_receiveData.Count},size:{StringUtils.FormatBytes(bytes)},time:{num7}ms,receive:[{percentile3}~{percentile4}]/{num3}ms,decrypt:[{percentile5}~{percentile6}]/{num4}ms,uncompress:[{percentile7}~{percentile8}]/{num5}ms,sfsObj:[{percentile9}~{percentile10}]/{num6}ms");
			_parameters.Add("recCount", _receiveData.Count.ToString());
			_parameters.Add("recSize", bytes.ToString());
			_parameters.Add("recTimeTot", num7.ToString());
			_parameters.Add("recTimeP90", percentile3.ToString());
			_parameters.Add("recTimeP99", percentile4.ToString());
			_parameters.Add("recDecryptTimeP90", percentile5.ToString());
			_parameters.Add("recDecryptTimeP99", percentile6.ToString());
			_parameters.Add("recUncompressTimeP90", percentile7.ToString());
			_parameters.Add("recUncompressTimeP99", percentile8.ToString());
			_parameters.Add("recSfsObjTimeP90", percentile9.ToString());
			_parameters.Add("recSfsObjTimeP99", percentile10.ToString());
			bool flag = PlayerPrefs.GetInt("Setting.GM_FLAG") > 0;
			if (CommonUtils.IsDebug() || flag)
			{
				var list = (from item in _receiveData.Values
					group item by item.cmd into @group
					select new
					{
						Cmd = @group.Key,
						Count = @group.Count(),
						totalSize = @group.Sum((NetReceiveProfiler item) => item.packetSize),
						totalTime = @group.Sum((NetReceiveProfiler item) => item.totalTime),
						totalReceiveTime = @group.Sum((NetReceiveProfiler item) => item.receiveTime),
						totalUncompressTime = @group.Sum((NetReceiveProfiler item) => item.uncompressTime),
						totalDecryptTime = @group.Sum((NetReceiveProfiler item) => item.decryptTime)
					} into x
					where x.Count > 10 || x.Cmd == "push.world.march.new"
					orderby x.Count descending
					select x).Take(10).ToList();
				if (list.Count > 0)
				{
					stringBuilder.AppendLine($"[rcv Top{list.Count}]:count|packSize|totalTime|receiveTime|uncompressTime|decryptTime");
					foreach (var item in list)
					{
						stringBuilder.AppendLine($"{item.Cmd}[{item.Count}][{item.totalSize}][{item.totalTime}][{item.totalReceiveTime}][{item.totalUncompressTime}][{item.totalDecryptTime}]");
					}
				}
				var list2 = (from item in _receiveData.Values
					where item.dispatchTime > 5 && !string.IsNullOrEmpty(item.cmd)
					group item by item.cmd into @group
					select new
					{
						Cmd = @group.Key,
						Count = @group.Count(),
						MinTime = @group.Min((NetReceiveProfiler x) => x.dispatchTime),
						MaxTime = @group.Max((NetReceiveProfiler x) => x.dispatchTime),
						AvgTime = @group.Average((NetReceiveProfiler x) => x.dispatchTime)
					} into x
					where x.Count > 1
					orderby x.MaxTime descending
					select x).Take(10).ToList();
				if (list2.Count > 0)
				{
					stringBuilder.AppendLine($"[Dispatch Top{list2.Count}]:");
					foreach (var item2 in list2)
					{
						stringBuilder.AppendLine($"{item2.Cmd}[{item2.Count}] min:{item2.MinTime}ms max:{item2.MaxTime}ms avg:{item2.AvgTime:F1}ms");
					}
				}
			}
			foreach (KeyValuePair<INetPacket, NetReceiveProfiler> receiveDatum in _receiveData)
			{
				receiveDatum.Value.Recycle();
			}
			_receiveData.Clear();
		}
		string message = stringBuilder.ToString();
		PostLog(message);
		cb?.Invoke(_parameters);
		_flushTimer.Restart();
	}

	private long GetPercentile(List<long> sortedData, int percentile)
	{
		if (sortedData.Count == 0)
		{
			return 0L;
		}
		int value = Mathf.CeilToInt((float)percentile / 100f * (float)sortedData.Count) - 1;
		value = Mathf.Clamp(value, 0, sortedData.Count - 1);
		return sortedData[value];
	}

	private int GetPercentile(List<int> sortedData, int percentile)
	{
		if (sortedData.Count == 0)
		{
			return 0;
		}
		int value = Mathf.CeilToInt((float)percentile / 100f * (float)sortedData.Count) - 1;
		value = Mathf.Clamp(value, 0, sortedData.Count - 1);
		return sortedData[value];
	}

	private void PostLog(string message)
	{
		_ioService.Post(delegate
		{
			Log.Info(message);
		});
	}
}
