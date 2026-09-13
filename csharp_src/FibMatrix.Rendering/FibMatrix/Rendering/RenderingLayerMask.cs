using System;
using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

public class RenderingLayerMask
{
	public static readonly int OverrideRenderingLayerCount = 32;

	private static readonly string[] OverrideRenderingLayerNames = new string[OverrideRenderingLayerCount];

	private static readonly Dictionary<string, int> NameToOverrideRenderingLayer = new Dictionary<string, int>(OverrideRenderingLayerCount);

	public static bool Register(int layer, string name)
	{
		if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
		{
			return Override(layer, name);
		}
		return Deregister(layer);
	}

	public static bool Deregister(int layer)
	{
		return Override(layer, $"OverrideLayer{layer}");
	}

	private static bool Override(int layer, string name)
	{
		if (!ValidateLayer(layer) || string.IsNullOrEmpty(name))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(OverrideRenderingLayerNames[layer]) && NameToOverrideRenderingLayer.ContainsKey(OverrideRenderingLayerNames[layer]))
		{
			NameToOverrideRenderingLayer.Remove(OverrideRenderingLayerNames[layer]);
		}
		OverrideRenderingLayerNames[layer] = name;
		NameToOverrideRenderingLayer[name] = layer;
		return true;
	}

	public static bool ValidateLayer(int layer)
	{
		if (layer >= 0)
		{
			return layer < OverrideRenderingLayerCount;
		}
		return false;
	}

	public static string LayerToName(int layer)
	{
		if (ValidateLayer(layer))
		{
			return OverrideRenderingLayerNames[layer];
		}
		return string.Empty;
	}

	public static int NameToLayer(string name)
	{
		if (NameToOverrideRenderingLayer.TryGetValue(name, out var value))
		{
			return value;
		}
		return -1;
	}

	public static int NameToMask(string name)
	{
		int num = NameToLayer(name);
		if (num != -1)
		{
			return 1 << num;
		}
		return 0;
	}

	public static string[] MaskToNames(uint mask)
	{
		List<string> list = new List<string>(4);
		MaskToNames(mask, list);
		return list.ToArray();
	}

	public static void MaskToNames(uint mask, List<string> names)
	{
		if (names == null)
		{
			throw new ArgumentNullException();
		}
		names.Capacity = Math.Max(names.Capacity, 4);
		for (int i = 0; i < 32; i++)
		{
			if ((mask & (uint)(1 << i)) != 0)
			{
				names.Add(OverrideRenderingLayerNames[i]);
			}
		}
	}

	public static uint NamesToMask(params string[] names)
	{
		if (names == null)
		{
			throw new ArgumentNullException();
		}
		uint num = 0u;
		for (int i = 0; i < names.Length; i++)
		{
			int num2 = NameToLayer(names[i]);
			if (num2 != -1)
			{
				num |= (uint)(1 << num2);
			}
		}
		return num;
	}

	private static void TestCaseLog(string log)
	{
		Debug.Log("RenderingLayerMaskTestCase " + log);
	}

	private static void TestCase()
	{
		TestCaseLog("after init\n" + string.Join(Environment.NewLine, OverrideRenderingLayerNames));
		Register(0, "OverlayBackground");
		TestCaseLog("after register 0: OverlayBackground\n" + string.Join(Environment.NewLine, OverrideRenderingLayerNames));
		TestCaseLog(string.Format("layer to name {0}, name to layer {1}", LayerToName(0), NameToLayer("OverlayBackground")));
		TestCaseLog(string.Format("mask to names {0}, names to mask {1}", string.Join(" ", MaskToNames(1u)), NamesToMask("OverlayBackground")));
		Register(1, "GrassAmbientOcclusion");
		TestCaseLog("after register 1: GrassAmbientOcclusion\n" + string.Join(Environment.NewLine, OverrideRenderingLayerNames));
		TestCaseLog(string.Format("layer to name {0}, name to layer {1}", LayerToName(1), NameToLayer("GrassAmbientOcclusion")));
		TestCaseLog(string.Format("mask to names {0}, names to mask {1}", string.Join(" ", MaskToNames(3u)), NamesToMask("OverlayBackground", "GrassAmbientOcclusion")));
		Deregister(0);
		TestCaseLog("after deregister 0\n" + string.Join(Environment.NewLine, OverrideRenderingLayerNames));
		TestCaseLog(string.Format("mask to names {0}, names to mask {1}", string.Join(" ", MaskToNames(2u)), NamesToMask("GrassAmbientOcclusion")));
		Deregister(1);
		TestCaseLog("after deregister 1\n" + string.Join(Environment.NewLine, OverrideRenderingLayerNames));
	}
}
