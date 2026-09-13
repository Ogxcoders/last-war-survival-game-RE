using Box2DSharp.Common;
using UnityEngine;

namespace Box2DSharp;

public static class Utils
{
	public static UnityEngine.Color ToUnityColor(this Box2DSharp.Common.Color color)
	{
		return new UnityEngine.Color((float)(int)color.R / 255f, (float)(int)color.G / 255f, (float)(int)color.B / 255f, (float)(int)color.A / 255f);
	}
}
