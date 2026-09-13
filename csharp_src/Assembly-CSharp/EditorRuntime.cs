using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitBenderGames;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class EditorRuntime : MonoBehaviour
{
	private MobileTouchCamera _touchCamera;

	private Camera _camera;

	private StringBuilder _builderMsg = new StringBuilder();

	private StringBuilder _builderProfiler = new StringBuilder();

	private string _message;

	private bool _isValid;

	private GameLODManager _lod;

	private DebugLODStrategy _lodStrategy = new DebugLODStrategy();

	private LODResourceSpec _spec;

	private void Awake()
	{
		InitCamera();
		_lod = new GameLODManager();
		_lod.Initialize();
		SingletonBehaviour<SceneLODManager>.Instance.ClearAllStrategies();
		IEnumerable<LODType> enumerable = Enum.GetValues(typeof(LODType)).Cast<LODType>();
		_lodStrategy.lod = 0;
		foreach (LODType item in enumerable)
		{
			SingletonBehaviour<SceneLODManager>.Instance.SetupLODType(item, 0.2f, 10);
			SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(item, _lodStrategy);
		}
	}

	private void Start()
	{
		_touchCamera.use45XCamera = true;
		_touchCamera.SetFov(10f);
		CameraUtil.CustomTransparencyAxis(_camera);
		_touchCamera.AutoLookat(Vector3.zero, -1f, 0f, null);
	}

	private void InitCamera()
	{
		SceneManager.CurrSceneID = 2;
		if (_touchCamera == null)
		{
			_touchCamera = UnityEngine.Object.FindObjectOfType<MobileTouchCamera>();
			_camera = _touchCamera.GetComponent<Camera>();
		}
	}

	private void SetupShadow(bool enableShadow, bool enableOutline)
	{
		ShadowLODComponent.EnableShadow(enableShadow);
		foreach (ScriptableRendererFeature rendererFeature in (QualitySettings.renderPipeline as UniversalRenderPipelineAsset).scriptableRenderer.rendererFeatures)
		{
			string text = rendererFeature.name.ToLower();
			if (!text.Contains("deprecated"))
			{
				if (text.Contains("shadow"))
				{
					rendererFeature.SetActive(enableShadow);
				}
				if (text.Contains("outline"))
				{
					rendererFeature.SetActive(enableOutline);
				}
			}
		}
	}

	public HashSet<string> GetStates()
	{
		HashSet<string> hashSet = new HashSet<string>();
		WorldBuildingAniEffect[] array = UnityEngine.Object.FindObjectsOfType<WorldBuildingAniEffect>();
		if (array != null)
		{
			WorldBuildingAniEffect[] array2 = array;
			foreach (WorldBuildingAniEffect worldBuildingAniEffect in array2)
			{
				for (int j = 0; j < worldBuildingAniEffect.aniData.Count; j++)
				{
					if (worldBuildingAniEffect.aniData[j].key != null)
					{
						hashSet.Add(worldBuildingAniEffect.aniData[j].key);
					}
				}
			}
		}
		WorldBuildingAniEffectAni[] array3 = UnityEngine.Object.FindObjectsOfType<WorldBuildingAniEffectAni>();
		if (array3 != null)
		{
			WorldBuildingAniEffectAni[] array4 = array3;
			foreach (WorldBuildingAniEffectAni worldBuildingAniEffectAni in array4)
			{
				for (int k = 0; k < worldBuildingAniEffectAni.aniData.Count; k++)
				{
					if (worldBuildingAniEffectAni.aniData[k].key != null)
					{
						hashSet.Add(worldBuildingAniEffectAni.aniData[k].key);
					}
				}
			}
		}
		SimpleAnimation[] array5 = UnityEngine.Object.FindObjectsOfType<SimpleAnimation>();
		if (array5 != null)
		{
			SimpleAnimation[] array6 = array5;
			for (int i = 0; i < array6.Length; i++)
			{
				SimpleAnimation.EditorState[] editorStates = array6[i].GetEditorStates();
				foreach (SimpleAnimation.EditorState editorState in editorStates)
				{
					if (string.IsNullOrEmpty(editorState.name) && editorState.name.ToLower() != "default")
					{
						hashSet.Add(editorState.name);
					}
				}
			}
		}
		return hashSet;
	}

	public PlayableDirector[] GetTimelines()
	{
		return UnityEngine.Object.FindObjectsOfType<PlayableDirector>();
	}

	public void PlayState(string name)
	{
		SimpleAnimation[] array = UnityEngine.Object.FindObjectsOfType<SimpleAnimation>();
		if (array != null)
		{
			SimpleAnimation[] array2 = array;
			foreach (SimpleAnimation obj in array2)
			{
				obj.Stop();
				obj.Play(name);
			}
		}
		WorldBuildingAniEffect[] array3 = UnityEngine.Object.FindObjectsOfType<WorldBuildingAniEffect>();
		if (array3 != null)
		{
			WorldBuildingAniEffect[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].PlayAnimation(name, 0f);
			}
		}
		WorldBuildingAniEffectAni[] array5 = UnityEngine.Object.FindObjectsOfType<WorldBuildingAniEffectAni>();
		if (array5 != null)
		{
			WorldBuildingAniEffectAni[] array6 = array5;
			for (int i = 0; i < array6.Length; i++)
			{
				array6[i].PlayAnimation(name, 0f);
			}
		}
	}

	public void StopState()
	{
		SimpleAnimation[] array = UnityEngine.Object.FindObjectsOfType<SimpleAnimation>();
		if (array != null)
		{
			SimpleAnimation[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Stop();
			}
		}
		WorldBuildingAniEffect[] array3 = UnityEngine.Object.FindObjectsOfType<WorldBuildingAniEffect>();
		if (array3 != null)
		{
			WorldBuildingAniEffect[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].StopAll();
			}
		}
		WorldBuildingAniEffectAni[] array5 = UnityEngine.Object.FindObjectsOfType<WorldBuildingAniEffectAni>();
		if (array5 != null)
		{
			WorldBuildingAniEffectAni[] array6 = array5;
			for (int i = 0; i < array6.Length; i++)
			{
				array6[i].StopAll();
			}
		}
	}

	public void PlayTimeline(PlayableDirector timeline)
	{
		timeline.time = 0.0;
		timeline.Play();
	}

	public void StopTimeline(PlayableDirector timeline)
	{
		timeline.time = 0.0;
		timeline.Play();
		timeline.Stop();
	}

	public void PlayCommon()
	{
		HashSet<string> states = GetStates();
		PlayableDirector[] timelines = GetTimelines();
		if ((states != null && states.Count > 0) || (timelines != null && timelines.Length != 0))
		{
			return;
		}
		ParticleSystem[] array = UnityEngine.Object.FindObjectsOfType<ParticleSystem>();
		if (array != null && array.Length != 0)
		{
			ParticleSystem[] array2 = array;
			foreach (ParticleSystem obj in array2)
			{
				obj.Simulate(0f);
				obj.Play();
			}
		}
		SimpleAnimation[] array3 = UnityEngine.Object.FindObjectsOfType<SimpleAnimation>();
		if (array3 != null && array3.Length != 0)
		{
			SimpleAnimation[] array4 = array3;
			foreach (SimpleAnimation obj2 in array4)
			{
				obj2.Stop();
				obj2.Play("Default");
			}
		}
	}

	public void StopCommon()
	{
		HashSet<string> states = GetStates();
		PlayableDirector[] timelines = GetTimelines();
		if ((states != null && states.Count > 0) || (timelines != null && timelines.Length != 0))
		{
			return;
		}
		ParticleSystem[] array = UnityEngine.Object.FindObjectsOfType<ParticleSystem>();
		if (array != null && array.Length != 0)
		{
			ParticleSystem[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
			}
		}
		SimpleAnimation[] array3 = UnityEngine.Object.FindObjectsOfType<SimpleAnimation>();
		if (array3 != null && array3.Length != 0)
		{
			SimpleAnimation[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				array4[i].Stop();
			}
		}
	}

	public void UpdateLOD()
	{
		SceneLODEffect[] array = UnityEngine.Object.FindObjectsOfType<SceneLODEffect>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateLOD(_lodStrategy.lod);
		}
	}
}
