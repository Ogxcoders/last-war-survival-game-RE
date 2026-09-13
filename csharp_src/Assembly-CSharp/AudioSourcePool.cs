using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AudioSourcePool
{
	private const int WARM_SIZE = 20;

	private List<AudioSource> _pool = new List<AudioSource>();

	private float _checkInterval = 30f;

	private Stopwatch _stopwatch = new Stopwatch();

	public Transform root { get; private set; }

	public AudioSourcePool(Transform parent)
	{
		root = new GameObject("AudioSourcePool").transform;
		root.SetParent(parent);
		Initialize(20);
	}

	private void Initialize(int count)
	{
		for (int i = 0; i < count; i++)
		{
			AudioSource audioSource = CreateNewAudioSource();
			audioSource.gameObject.SetActive(value: false);
			_pool.Add(audioSource);
		}
	}

	private AudioSource CreateNewAudioSource()
	{
		GameObject gameObject = new GameObject("AudioSource");
		gameObject.transform.SetParent(root);
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.playOnAwake = false;
		return audioSource;
	}

	public AudioSource Spawn()
	{
		AudioSource audioSource;
		if (_pool.Count > 0)
		{
			int index = _pool.Count - 1;
			audioSource = _pool[index];
			_pool.RemoveAt(index);
		}
		else
		{
			audioSource = CreateNewAudioSource();
		}
		int childCount = root.childCount;
		if (childCount > 0 && _pool.Count >= Mathf.FloorToInt((float)childCount * 0.5f) && !_stopwatch.IsRunning)
		{
			_stopwatch.Start();
		}
		audioSource.gameObject.SetActive(value: true);
		return audioSource;
	}

	public void Unspawn(AudioSource audioSource)
	{
		if (!(audioSource == null))
		{
			ResetAudioSource(audioSource);
			int childCount = root.childCount;
			if (_pool.Count < Mathf.FloorToInt((float)childCount * 0.5f) && _stopwatch.IsRunning)
			{
				_stopwatch.Reset();
			}
			audioSource.gameObject.SetActive(value: false);
			_pool.Add(audioSource);
		}
	}

	private void ResetAudioSource(AudioSource audioSource)
	{
		audioSource.Stop();
		audioSource.clip = null;
		audioSource.loop = false;
		audioSource.volume = 1f;
		audioSource.pitch = 1f;
		audioSource.spatialBlend = 0f;
		audioSource.outputAudioMixerGroup = null;
	}

	public static void CopyFromTo(AudioSource from, AudioSource to)
	{
		if (!(from == null) && !(to == null))
		{
			to.clip = from.clip;
			to.outputAudioMixerGroup = from.outputAudioMixerGroup;
			to.mute = from.mute;
			to.bypassEffects = from.bypassEffects;
			to.bypassListenerEffects = from.bypassListenerEffects;
			to.bypassReverbZones = from.bypassReverbZones;
			to.loop = from.loop;
			to.priority = from.priority;
			to.volume = from.volume;
			to.pitch = from.pitch;
			to.panStereo = from.panStereo;
			to.spatialBlend = from.spatialBlend;
			to.reverbZoneMix = from.reverbZoneMix;
			to.dopplerLevel = from.dopplerLevel;
			to.spread = from.spread;
			to.rolloffMode = from.rolloffMode;
			to.minDistance = from.minDistance;
			to.maxDistance = from.maxDistance;
			to.SetCustomCurve(AudioSourceCurveType.CustomRolloff, from.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
			to.SetCustomCurve(AudioSourceCurveType.SpatialBlend, from.GetCustomCurve(AudioSourceCurveType.SpatialBlend));
			to.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, from.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix));
			to.SetCustomCurve(AudioSourceCurveType.Spread, from.GetCustomCurve(AudioSourceCurveType.Spread));
		}
	}

	public void Update()
	{
		CheckUsageRoutine();
	}

	private void CheckUsageRoutine()
	{
		if ((float)_stopwatch.ElapsedMilliseconds / 1000f > _checkInterval)
		{
			ShrinkPool();
			_stopwatch.Reset();
		}
	}

	private void ShrinkPool()
	{
		int num = Mathf.FloorToInt((float)root.childCount * 0.5f);
		if (num <= 0)
		{
			return;
		}
		int num2 = root.childCount - 1;
		while (num2 > -1)
		{
			Transform child = root.GetChild(num2);
			if (!child.gameObject.activeSelf)
			{
				AudioSource component = child.GetComponent<AudioSource>();
				_pool.Remove(component);
				Object.Destroy(child.gameObject);
				num--;
			}
			if (num != 0)
			{
				num2--;
				continue;
			}
			break;
		}
	}
}
