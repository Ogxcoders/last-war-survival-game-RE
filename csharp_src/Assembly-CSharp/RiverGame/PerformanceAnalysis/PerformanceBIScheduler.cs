using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

namespace RiverGame.PerformanceAnalysis;

[Preserve]
public class PerformanceBIScheduler
{
	public delegate bool DelegateBIMetricsProvider();

	[DefaultExecutionOrder(1073741823)]
	public class SchedulerContainer : MonoBehaviour
	{
		public Action ActionLateUpdate;

		private void LateUpdate()
		{
			ActionLateUpdate?.Invoke();
		}
	}

	private static readonly Dictionary<string, object> m_TracePerformanceContext = new Dictionary<string, object>(3);

	private const float DelaySeconds = 10f;

	private const float IntervalSeconds = 300f;

	public float delaySeconds = 10f;

	public float intervalSeconds = 300f;

	private static bool s_SystemInfoFilled = false;

	private string m_LogicModuleTagLast = PerformanceMetrics.LogicModuleTag.Value;

	private string m_LogicModuleSourceTagLast = PerformanceMetrics.LogicModuleSourceTag.Value;

	private bool m_OverrideLogicModuleTag;

	private SchedulerContainer m_SchedulerContainer;

	private Coroutine m_Scheduler;

	private WaitForSeconds m_WaitForSecondsDelay;

	private WaitForSeconds m_WaitForSecondsInterval;

	public FPSCounter fpsCounter
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	} = new FPSCounter();

	public FPSPercentileCounter fpsPercentileCounter
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	} = new FPSPercentileCounter(60);

	public FPSShouldHighTimeCounter fpsShouldHighTimeCounter
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	} = new FPSShouldHighTimeCounter();

	public FPSJankCounter fpsJankCounter
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	} = new FPSJankCounter();

	public event DelegateBIMetricsProvider FillBIMetricsAdditional;

	private IEnumerator CoroutineSchedulerDelay()
	{
		yield return m_WaitForSecondsDelay;
		PerformanceMetrics.CurrentEventType = PerformanceMetrics.EnEventType.Delay;
		TracePerformance();
		while (true)
		{
			yield return m_WaitForSecondsInterval;
			PerformanceMetrics.CurrentEventType = PerformanceMetrics.EnEventType.Interval;
			TracePerformance();
		}
	}

	public void Start(float delaySeconds = 10f, float intervalSeconds = 300f)
	{
		this.delaySeconds = delaySeconds;
		this.intervalSeconds = intervalSeconds;
		fpsCounter.StartNew();
		fpsPercentileCounter.StartNew();
		fpsShouldHighTimeCounter.StartNew();
		fpsJankCounter.StartNew();
		PerformanceMetrics.CurrentFPS.CustomProvider = () => fpsCounter.currFPS;
		PerformanceMetrics.AverageFPS.CustomProvider = () => fpsCounter.avgFPS;
		PerformanceMetrics.FPS0_1.CustomProvider = () => fpsPercentileCounter.GetFramePercentile(0.1f);
		PerformanceMetrics.FPS01.CustomProvider = () => fpsPercentileCounter.GetFramePercentile(1f);
		PerformanceMetrics.FPS10.CustomProvider = () => fpsPercentileCounter.GetFramePercentile(10f);
		PerformanceMetrics.FPS20.CustomProvider = () => fpsPercentileCounter.GetFramePercentile(20f);
		PerformanceMetrics.FPS30.CustomProvider = () => fpsPercentileCounter.GetFramePercentile(30f);
		PerformanceMetrics.FPS50.CustomProvider = () => fpsPercentileCounter.GetFramePercentile(50f);
		PerformanceMetrics.FPS_Target.CustomProvider = () => Application.targetFrameRate;
		PerformanceMetrics.FPSShouldHighTime.CustomProvider = () => fpsShouldHighTimeCounter.highFPSTime;
		PerformanceMetrics.FPSShouldHighTime2.CustomProvider = () => fpsShouldHighTimeCounter.highFPSTime2;
		PerformanceMetrics.SmallJankCount.CustomProvider = () => fpsJankCounter.smallJankCount;
		PerformanceMetrics.BigJankCount.CustomProvider = () => fpsJankCounter.bigJankCount;
		PerformanceMetrics.SmallJankTime.CustomProvider = () => fpsJankCounter.smallJankTime;
		PerformanceMetrics.BigJankTime.CustomProvider = () => fpsJankCounter.bigJankTime;
		PerformanceMetrics.Duration.CustomProvider = () => fpsJankCounter.GetDuration();
		m_WaitForSecondsDelay = new WaitForSeconds(delaySeconds);
		m_WaitForSecondsInterval = new WaitForSeconds(intervalSeconds);
		if (m_SchedulerContainer == null)
		{
			m_SchedulerContainer = new GameObject("PerformanceBIScheduler").AddComponent<SchedulerContainer>();
			UnityEngine.Object.DontDestroyOnLoad(m_SchedulerContainer);
		}
		SchedulerContainer schedulerContainer = m_SchedulerContainer;
		schedulerContainer.ActionLateUpdate = (Action)Delegate.Combine(schedulerContainer.ActionLateUpdate, new Action(LateUpdate));
		m_Scheduler = m_SchedulerContainer.StartCoroutine(CoroutineSchedulerDelay());
	}

	public void Stop()
	{
		if (m_SchedulerContainer != null)
		{
			m_SchedulerContainer.StopAllCoroutines();
		}
		m_Scheduler = null;
		if (m_SchedulerContainer != null)
		{
			SchedulerContainer schedulerContainer = m_SchedulerContainer;
			schedulerContainer.ActionLateUpdate = (Action)Delegate.Remove(schedulerContainer.ActionLateUpdate, new Action(LateUpdate));
		}
		PerformanceMetrics.CurrentFPS.CustomProvider = null;
		PerformanceMetrics.AverageFPS.CustomProvider = null;
		PerformanceMetrics.FPS0_1.CustomProvider = null;
		PerformanceMetrics.FPS01.CustomProvider = null;
		PerformanceMetrics.FPS10.CustomProvider = null;
		PerformanceMetrics.FPS20.CustomProvider = null;
		PerformanceMetrics.FPS30.CustomProvider = null;
		PerformanceMetrics.FPS50.CustomProvider = null;
		PerformanceMetrics.FPS_Target.CustomProvider = null;
		PerformanceMetrics.FPSShouldHighTime.CustomProvider = null;
		PerformanceMetrics.FPSShouldHighTime2.CustomProvider = null;
		PerformanceMetrics.SmallJankCount.CustomProvider = null;
		PerformanceMetrics.BigJankCount.CustomProvider = null;
		PerformanceMetrics.SmallJankTime.CustomProvider = null;
		PerformanceMetrics.BigJankTime.CustomProvider = null;
		PerformanceMetrics.Duration.CustomProvider = null;
		Clear();
	}

	public void Clear()
	{
		fpsCounter.StartNew();
		fpsPercentileCounter.StartNew();
		fpsShouldHighTimeCounter.StartNew();
		fpsJankCounter.StartNew();
	}

	public void Destroy()
	{
		Stop();
		if (m_SchedulerContainer != null)
		{
			UnityEngine.Object.Destroy(m_SchedulerContainer.gameObject);
			m_SchedulerContainer = null;
		}
	}

	public void ReStart()
	{
		ReStart(delaySeconds, intervalSeconds);
	}

	public void ReStart(float delaySeconds = 10f, float intervalSeconds = 300f)
	{
		Stop();
		Start(delaySeconds, intervalSeconds);
	}

	public void LateUpdate()
	{
		fpsCounter.TryCount();
		fpsPercentileCounter.TryCount();
		fpsShouldHighTimeCounter.TryCount();
		fpsJankCounter.TryCount();
		if (m_LogicModuleTagLast.CompareTo(PerformanceMetrics.LogicModuleTag.Value) != 0)
		{
			PerformanceMetrics.CurrentEventType = PerformanceMetrics.EnEventType.LogicModuleChanged;
			m_OverrideLogicModuleTag = true;
			TracePerformance();
			m_OverrideLogicModuleTag = false;
			m_LogicModuleTagLast = PerformanceMetrics.LogicModuleTag.Value;
			m_LogicModuleSourceTagLast = PerformanceMetrics.LogicModuleSourceTag.Value;
			ReStart();
		}
	}

	public void TracePerformance()
	{
		PerformanceMetrics.GetPerformanceMetrics(m_TracePerformanceContext);
		if (!s_SystemInfoFilled)
		{
			s_SystemInfoFilled = true;
			PerformanceMetrics.GetSystemInfoMetrics(m_TracePerformanceContext);
		}
		if (m_OverrideLogicModuleTag)
		{
			m_TracePerformanceContext["pd_logic_module_tag"] = m_LogicModuleTagLast;
			m_TracePerformanceContext["source_type"] = m_LogicModuleSourceTagLast;
		}
		this.FillBIMetricsAdditional?.Invoke();
		PostEventLog.TrackMap("device_performance", m_TracePerformanceContext);
		Clear();
	}

	public bool SetMetric(string key, object value)
	{
		if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key) || value == null)
		{
			return false;
		}
		m_TracePerformanceContext[key] = value;
		return true;
	}
}
