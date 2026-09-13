using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderTiming : MonoBehaviour
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void MyDelegate(string str);

	public static RenderTiming instance;

	private bool logTiming;

	private bool isInitialized;

	private CommandBuffer commandBufferStart;

	private CommandBuffer commandBufferEnd;

	private Coroutine loggingCoroutine;

	public bool isSupported => GetStartTimingFunc() != IntPtr.Zero;

	public float deltaTime => GetTiming();

	[DllImport("RenderTimingPlugin")]
	private static extern void SetDebugFunction(IntPtr ftp);

	[DllImport("RenderTimingPlugin")]
	private static extern IntPtr GetStartTimingFunc();

	[DllImport("RenderTimingPlugin")]
	private static extern IntPtr GetEndTimingFunc();

	[DllImport("RenderTimingPlugin")]
	private static extern float GetTiming();

	[MonoPInvokeCallback(typeof(MyDelegate))]
	private static void DebugCallback(string str)
	{
		Debug.LogWarning("RenderTimingPlugin: " + str);
	}

	private void Awake()
	{
		instance = this;
	}

	private void Init()
	{
		SetDebugFunction(Marshal.GetFunctionPointerForDelegate<MyDelegate>(DebugCallback));
		commandBufferStart = new CommandBuffer();
		commandBufferStart.name = "StartGpuTiming";
		commandBufferStart.IssuePluginEvent(GetStartTimingFunc(), 0);
		commandBufferEnd = new CommandBuffer();
		commandBufferEnd.name = "EndGpuTiming";
		commandBufferEnd.IssuePluginEvent(GetEndTimingFunc(), 0);
		isInitialized = true;
	}

	private void LateUpdate()
	{
		GL.IssuePluginEvent(GetStartTimingFunc(), 0);
	}

	private IEnumerator FrameEnd()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
			if (base.enabled)
			{
				GL.IssuePluginEvent(GetEndTimingFunc(), 0);
				continue;
			}
			break;
		}
	}

	private void OnEnable()
	{
		if (!isSupported)
		{
			base.enabled = false;
			return;
		}
		if (!isInitialized)
		{
			Init();
		}
		StartCoroutine(FrameEnd());
		if (logTiming)
		{
			loggingCoroutine = StartCoroutine(ConsoleDisplay());
		}
	}

	private void OnDisable()
	{
		if (isSupported && loggingCoroutine != null)
		{
			StopCoroutine(loggingCoroutine);
			loggingCoroutine = null;
		}
	}

	private IEnumerator ConsoleDisplay()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			Debug.LogFormat("Render time: {0:F3} ms", deltaTime * 1000f);
		}
	}
}
