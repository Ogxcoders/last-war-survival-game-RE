using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PowerVRExtensions
{
	private IntPtr _glFinishFuncPtr;

	private bool _usePlugin;

	private string[] _deviceMap = new string[20]
	{
		"cph19", "cph20", "cph21", "cph22", "cph23", "y15", "y16", "a57", "v2111", "v2048",
		"vivo201", "hotplay", "infinixhot30", "realmec1", "redmi9c", "redmia2", "redmi9", "galaxya12", "galaxya0", "galaxytaba7lite"
	};

	private int _counter;

	private const int COUNT_MAX = 1000;

	[DllImport("PVRNativeRenderPlugin")]
	private static extern IntPtr GetRenderEventFunc();

	public void Init()
	{
		_usePlugin = false;
	}

	private void EndFrame(ScriptableRenderContext context, Camera[] cam)
	{
		if (++_counter >= 1000)
		{
			_counter = 0;
			CommandBuffer commandBuffer = CommandBufferPool.Get();
			commandBuffer.IssuePluginEvent(_glFinishFuncPtr, 0);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
	}

	public void Shutdown()
	{
		if (_usePlugin)
		{
			Debug.Log("CSPowerVRExtensions::Shutdown");
			RenderPipelineManager.endFrameRendering -= EndFrame;
			_glFinishFuncPtr = IntPtr.Zero;
		}
	}
}
