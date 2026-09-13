using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public static class ScriptableRendererExtensions
{
	private static readonly Dictionary<ScriptableRenderer, List<ScriptableRendererFeature>> RendererToFeaturesList = new Dictionary<ScriptableRenderer, List<ScriptableRendererFeature>>();

	private static readonly Dictionary<ScriptableRenderer, Dictionary<string, ScriptableRendererFeature>> RendererToFeaturesDictionary = new Dictionary<ScriptableRenderer, Dictionary<string, ScriptableRendererFeature>>();

	private static FieldInfo RendererFeaturesFieldInfo;

	public static void ActiveRendererFeature<T>(this ScriptableRenderer renderer, bool enabled) where T : ScriptableRendererFeature
	{
		List<ScriptableRendererFeature> rendererFeaturesList = renderer.GetRendererFeaturesList();
		for (int i = 0; i < rendererFeaturesList.Count; i++)
		{
			if (rendererFeaturesList[i] is T val)
			{
				val.SetActive(enabled);
			}
		}
	}

	public static T GetRendererFeature<T>(this ScriptableRenderer renderer) where T : ScriptableRendererFeature
	{
		List<ScriptableRendererFeature> rendererFeaturesList = renderer.GetRendererFeaturesList();
		for (int i = 0; i < rendererFeaturesList.Count; i++)
		{
			if (rendererFeaturesList[i] is T result)
			{
				return result;
			}
		}
		return null;
	}

	public static ScriptableRendererFeature GetRendererFeatureByName(this ScriptableRenderer renderer, string name)
	{
		renderer.GetRendererFeaturesDictionary().TryGetValue(name, out var value);
		return value;
	}

	public static Dictionary<string, ScriptableRendererFeature> GetRendererFeaturesDictionary(this ScriptableRenderer renderer)
	{
		if (!RendererToFeaturesDictionary.TryGetValue(renderer, out var value))
		{
			List<ScriptableRendererFeature> list = renderer.ReflectionRendererFeatures();
			value = new Dictionary<string, ScriptableRendererFeature>(list.Count);
			RendererToFeaturesDictionary[renderer] = value;
			for (int i = 0; i < list.Count; i++)
			{
				value[list[i].name] = list[i];
			}
		}
		return value;
	}

	public static List<ScriptableRendererFeature> GetRendererFeaturesList(this ScriptableRenderer renderer)
	{
		if (!RendererToFeaturesList.TryGetValue(renderer, out var value))
		{
			value = renderer.ReflectionRendererFeatures();
			RendererToFeaturesList.Add(renderer, value);
		}
		return value;
	}

	private static List<ScriptableRendererFeature> ReflectionRendererFeatures(this ScriptableRenderer renderer)
	{
		if (RendererFeaturesFieldInfo == null)
		{
			RendererFeaturesFieldInfo = typeof(ScriptableRenderer).GetField("m_RendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);
		}
		return RendererFeaturesFieldInfo?.GetValue(renderer) as List<ScriptableRendererFeature>;
	}
}
