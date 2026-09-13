using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine;

[NativeHeader("Modules/ScreenCapture/Public/CaptureScreenshot.h")]
public static class ScreenCapture
{
	public enum StereoScreenCaptureMode
	{
		LeftEye = 1,
		RightEye,
		BothEyes
	}

	public static void CaptureScreenshot(string filename)
	{
		CaptureScreenshot(filename, 1, StereoScreenCaptureMode.LeftEye);
	}

	public static void CaptureScreenshot(string filename, int superSize)
	{
		CaptureScreenshot(filename, superSize, StereoScreenCaptureMode.LeftEye);
	}

	public static void CaptureScreenshot(string filename, StereoScreenCaptureMode stereoCaptureMode)
	{
		CaptureScreenshot(filename, 1, stereoCaptureMode);
	}

	public static Texture2D CaptureScreenshotAsTexture()
	{
		return CaptureScreenshotAsTexture(1, StereoScreenCaptureMode.LeftEye);
	}

	public static Texture2D CaptureScreenshotAsTexture(int superSize)
	{
		return CaptureScreenshotAsTexture(superSize, StereoScreenCaptureMode.LeftEye);
	}

	public static Texture2D CaptureScreenshotAsTexture(StereoScreenCaptureMode stereoCaptureMode)
	{
		return CaptureScreenshotAsTexture(1, stereoCaptureMode);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void CaptureScreenshotIntoRenderTexture(RenderTexture renderTexture);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void CaptureScreenshot(string filename, [DefaultValue("1")] int superSize, [DefaultValue("1")] StereoScreenCaptureMode CaptureMode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Texture2D CaptureScreenshotAsTexture(int superSize, StereoScreenCaptureMode stereoScreenCaptureMode);
}
