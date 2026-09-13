using System;
using System.Collections.Generic;
using System.Linq;
using Tayx.Graphy.Audio;
using Tayx.Graphy.Fps;
using Tayx.Graphy.Ram;
using Tayx.Graphy.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Tayx.Graphy;

public class GraphyDebugger : G_Singleton<GraphyDebugger>
{
	public enum DebugVariable
	{
		Fps,
		Fps_Min,
		Fps_Max,
		Fps_Avg,
		Ram_Allocated,
		Ram_Reserved,
		Ram_Mono,
		Ram_Lua,
		Audio_DB
	}

	public enum DebugComparer
	{
		Less_than,
		Equals_or_less_than,
		Equals,
		Equals_or_greater_than,
		Greater_than
	}

	public enum ConditionEvaluation
	{
		All_conditions_must_be_met,
		Only_one_condition_has_to_be_met
	}

	public enum MessageType
	{
		Log,
		Warning,
		Error
	}

	[Serializable]
	public struct DebugCondition
	{
		[Tooltip("Variable to compare against")]
		public DebugVariable Variable;

		[Tooltip("Comparer operator to use")]
		public DebugComparer Comparer;

		[Tooltip("Value to compare against the chosen variable")]
		public float Value;
	}

	[Serializable]
	public class DebugPacket
	{
		[Tooltip("If false, it won't be checked")]
		public bool Active = true;

		[Tooltip("Optional Id. It's used to get or remove DebugPackets in runtime")]
		public int Id;

		[Tooltip("If true, once the actions are executed, this DebugPacket will delete itself")]
		public bool ExecuteOnce = true;

		[Tooltip("Time to wait before checking if conditions are met (use this to avoid low fps drops triggering the conditions when loading the game)")]
		public float InitSleepTime = 2f;

		[Tooltip("Time to wait before checking if conditions are met again (once they have already been met and if ExecuteOnce is false)")]
		public float ExecuteSleepTime = 2f;

		public ConditionEvaluation ConditionEvaluation;

		[Tooltip("List of conditions that will be checked each frame")]
		public List<DebugCondition> DebugConditions = new List<DebugCondition>();

		public MessageType MessageType;

		[Multiline]
		public string Message = string.Empty;

		public bool TakeScreenshot;

		public string ScreenshotFileName = "Graphy_Screenshot";

		[Tooltip("If true, it pauses the editor")]
		public bool DebugBreak;

		public UnityEvent UnityEvents;

		public List<Action> Callbacks = new List<Action>();

		private bool canBeChecked;

		private bool executed;

		private float timePassed;

		public bool Check => canBeChecked;

		public void Update()
		{
			if (!canBeChecked)
			{
				timePassed += Time.deltaTime;
				if ((executed && timePassed >= ExecuteSleepTime) || (!executed && timePassed >= InitSleepTime))
				{
					canBeChecked = true;
					timePassed = 0f;
				}
			}
		}

		public void Executed()
		{
			canBeChecked = false;
			executed = true;
		}
	}

	[SerializeField]
	private List<DebugPacket> m_debugPackets = new List<DebugPacket>();

	private G_FpsMonitor m_fpsMonitor;

	private G_RamMonitor m_ramMonitor;

	private G_AudioMonitor m_audioMonitor;

	protected GraphyDebugger()
	{
	}

	private void Start()
	{
		m_fpsMonitor = GetComponentInChildren<G_FpsMonitor>();
		m_ramMonitor = GetComponentInChildren<G_RamMonitor>();
		m_audioMonitor = GetComponentInChildren<G_AudioMonitor>();
	}

	private void Update()
	{
		CheckDebugPackets();
	}

	public void AddNewDebugPacket(DebugPacket newDebugPacket)
	{
		if (m_debugPackets != null)
		{
			m_debugPackets.Add(newDebugPacket);
		}
	}

	public void AddNewDebugPacket(int newId, DebugCondition newDebugCondition, MessageType newMessageType, string newMessage, bool newDebugBreak, Action newCallback)
	{
		DebugPacket debugPacket = new DebugPacket();
		debugPacket.Id = newId;
		debugPacket.DebugConditions.Add(newDebugCondition);
		debugPacket.MessageType = newMessageType;
		debugPacket.Message = newMessage;
		debugPacket.DebugBreak = newDebugBreak;
		debugPacket.Callbacks.Add(newCallback);
		AddNewDebugPacket(debugPacket);
	}

	public void AddNewDebugPacket(int newId, List<DebugCondition> newDebugConditions, MessageType newMessageType, string newMessage, bool newDebugBreak, Action newCallback)
	{
		DebugPacket debugPacket = new DebugPacket();
		debugPacket.Id = newId;
		debugPacket.DebugConditions = newDebugConditions;
		debugPacket.MessageType = newMessageType;
		debugPacket.Message = newMessage;
		debugPacket.DebugBreak = newDebugBreak;
		debugPacket.Callbacks.Add(newCallback);
		AddNewDebugPacket(debugPacket);
	}

	public void AddNewDebugPacket(int newId, DebugCondition newDebugCondition, MessageType newMessageType, string newMessage, bool newDebugBreak, List<Action> newCallbacks)
	{
		DebugPacket debugPacket = new DebugPacket();
		debugPacket.Id = newId;
		debugPacket.DebugConditions.Add(newDebugCondition);
		debugPacket.MessageType = newMessageType;
		debugPacket.Message = newMessage;
		debugPacket.DebugBreak = newDebugBreak;
		debugPacket.Callbacks = newCallbacks;
		AddNewDebugPacket(debugPacket);
	}

	public void AddNewDebugPacket(int newId, List<DebugCondition> newDebugConditions, MessageType newMessageType, string newMessage, bool newDebugBreak, List<Action> newCallbacks)
	{
		DebugPacket debugPacket = new DebugPacket();
		debugPacket.Id = newId;
		debugPacket.DebugConditions = newDebugConditions;
		debugPacket.MessageType = newMessageType;
		debugPacket.Message = newMessage;
		debugPacket.DebugBreak = newDebugBreak;
		debugPacket.Callbacks = newCallbacks;
		AddNewDebugPacket(debugPacket);
	}

	public DebugPacket GetFirstDebugPacketWithId(int packetId)
	{
		return m_debugPackets.First((DebugPacket x) => x.Id == packetId);
	}

	public List<DebugPacket> GetAllDebugPacketsWithId(int packetId)
	{
		return m_debugPackets.FindAll((DebugPacket x) => x.Id == packetId);
	}

	public void RemoveFirstDebugPacketWithId(int packetId)
	{
		if (m_debugPackets != null && GetFirstDebugPacketWithId(packetId) != null)
		{
			m_debugPackets.Remove(GetFirstDebugPacketWithId(packetId));
		}
	}

	public void RemoveAllDebugPacketsWithId(int packetId)
	{
		if (m_debugPackets != null)
		{
			m_debugPackets.RemoveAll((DebugPacket x) => x.Id == packetId);
		}
	}

	public void AddCallbackToFirstDebugPacketWithId(Action callback, int id)
	{
		if (GetFirstDebugPacketWithId(id) != null)
		{
			GetFirstDebugPacketWithId(id).Callbacks.Add(callback);
		}
	}

	public void AddCallbackToAllDebugPacketWithId(Action callback, int id)
	{
		if (GetAllDebugPacketsWithId(id) == null)
		{
			return;
		}
		foreach (DebugPacket item in GetAllDebugPacketsWithId(id))
		{
			if (callback != null)
			{
				item.Callbacks.Add(callback);
			}
		}
	}

	private void CheckDebugPackets()
	{
		if (m_debugPackets == null)
		{
			return;
		}
		for (int i = 0; i < m_debugPackets.Count; i++)
		{
			DebugPacket debugPacket = m_debugPackets[i];
			if (debugPacket == null || !debugPacket.Active)
			{
				continue;
			}
			debugPacket.Update();
			if (!debugPacket.Check)
			{
				continue;
			}
			switch (debugPacket.ConditionEvaluation)
			{
			case ConditionEvaluation.All_conditions_must_be_met:
			{
				int num = 0;
				foreach (DebugCondition debugCondition in debugPacket.DebugConditions)
				{
					if (CheckIfConditionIsMet(debugCondition))
					{
						num++;
					}
				}
				if (num >= debugPacket.DebugConditions.Count)
				{
					ExecuteOperationsInDebugPacket(debugPacket);
					if (debugPacket.ExecuteOnce)
					{
						m_debugPackets[i] = null;
					}
				}
				break;
			}
			case ConditionEvaluation.Only_one_condition_has_to_be_met:
				foreach (DebugCondition debugCondition2 in debugPacket.DebugConditions)
				{
					if (CheckIfConditionIsMet(debugCondition2))
					{
						ExecuteOperationsInDebugPacket(debugPacket);
						if (debugPacket.ExecuteOnce)
						{
							m_debugPackets[i] = null;
						}
						break;
					}
				}
				break;
			}
		}
		m_debugPackets.RemoveAll((DebugPacket packet) => packet == null);
	}

	private bool CheckIfConditionIsMet(DebugCondition debugCondition)
	{
		return debugCondition.Comparer switch
		{
			DebugComparer.Less_than => GetRequestedValueFromDebugVariable(debugCondition.Variable) < debugCondition.Value, 
			DebugComparer.Equals_or_less_than => GetRequestedValueFromDebugVariable(debugCondition.Variable) <= debugCondition.Value, 
			DebugComparer.Equals => Mathf.Approximately(GetRequestedValueFromDebugVariable(debugCondition.Variable), debugCondition.Value), 
			DebugComparer.Equals_or_greater_than => GetRequestedValueFromDebugVariable(debugCondition.Variable) >= debugCondition.Value, 
			DebugComparer.Greater_than => GetRequestedValueFromDebugVariable(debugCondition.Variable) > debugCondition.Value, 
			_ => false, 
		};
	}

	private float GetRequestedValueFromDebugVariable(DebugVariable debugVariable)
	{
		switch (debugVariable)
		{
		case DebugVariable.Fps:
			if (!(m_fpsMonitor != null))
			{
				return 0f;
			}
			return m_fpsMonitor.CurrentFPS;
		case DebugVariable.Fps_Min:
			if (!(m_fpsMonitor != null))
			{
				return 0f;
			}
			return m_fpsMonitor.MinFPS;
		case DebugVariable.Fps_Max:
			if (!(m_fpsMonitor != null))
			{
				return 0f;
			}
			return m_fpsMonitor.MaxFPS;
		case DebugVariable.Fps_Avg:
			if (!(m_fpsMonitor != null))
			{
				return 0f;
			}
			return m_fpsMonitor.AverageFPS;
		case DebugVariable.Ram_Allocated:
			if (!(m_ramMonitor != null))
			{
				return 0f;
			}
			return m_ramMonitor.AllocatedRam;
		case DebugVariable.Ram_Reserved:
			if (!(m_ramMonitor != null))
			{
				return 0f;
			}
			return m_ramMonitor.AllocatedRam;
		case DebugVariable.Ram_Mono:
			if (!(m_ramMonitor != null))
			{
				return 0f;
			}
			return m_ramMonitor.AllocatedRam;
		case DebugVariable.Audio_DB:
			if (!(m_audioMonitor != null))
			{
				return 0f;
			}
			return m_audioMonitor.MaxDB;
		default:
			return 0f;
		}
	}

	private void ExecuteOperationsInDebugPacket(DebugPacket debugPacket)
	{
		if (debugPacket == null)
		{
			return;
		}
		if (debugPacket.DebugBreak)
		{
			Debug.Break();
		}
		if (debugPacket.Message != "")
		{
			string message = string.Concat("[Graphy] (", DateTime.Now, "): ", debugPacket.Message);
			switch (debugPacket.MessageType)
			{
			case MessageType.Log:
				Debug.Log(message);
				break;
			case MessageType.Warning:
				Debug.LogWarning(message);
				break;
			case MessageType.Error:
				Debug.LogError(message);
				break;
			}
		}
		if (debugPacket.TakeScreenshot)
		{
			ScreenCapture.CaptureScreenshot(string.Concat(debugPacket.ScreenshotFileName, "_", DateTime.Now, ".png").Replace("/", "-").Replace(" ", "_")
				.Replace(":", "-"));
		}
		debugPacket.UnityEvents.Invoke();
		foreach (Action callback in debugPacket.Callbacks)
		{
			callback?.Invoke();
		}
		debugPacket.Executed();
	}
}
