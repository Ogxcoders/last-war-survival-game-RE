using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using UnityEngine;
using UnityEngine.Audio;
using VEngine;

public sealed class SoundComponent
{
	internal static class Constant
	{
		internal const float DefaultTime = 0f;

		internal const bool DefaultMute = false;

		internal const bool DefaultLoop = false;

		internal const int DefaultPriority = 0;

		internal const float DefaultVolume = 1f;

		internal const float DefaultFadeInSeconds = 0f;

		internal const float DefaultFadeOutSeconds = 0f;

		internal const float DefaultPitch = 1f;

		internal const float DefaultPanStereo = 0f;

		internal const float DefaultSpatialBlend = 0f;

		internal const float DefaultMaxDistance = 100f;

		internal const float DefaultDopplerLevel = 1f;

		internal const float DefaultSoundVolumeSet = -1f;

		internal const int DefaultLoop_Gap = -1;

		internal const int DefaultPreTime = -1;
	}

	public sealed class PlaySoundParams
	{
		public int serialId;

		private float m_Time;

		private bool m_MuteInSoundGroup;

		private bool m_Loop;

		private int m_Priority;

		private float m_VolumeInSoundGroup;

		private float m_FadeInSeconds;

		private float m_Pitch;

		private float m_PanStereo;

		private float m_SpatialBlend;

		private float m_MaxDistance;

		private float m_DopplerLevel;

		private float m_soundVolumeSet;

		private float m_FadeOutSeconds;

		private float m_ShotValumeScale;

		private int m_Loop_gap;

		private List<string> m_SoundAssetPaths;

		public float Time
		{
			get
			{
				return m_Time;
			}
			set
			{
				m_Time = value;
			}
		}

		public bool MuteInSoundGroup
		{
			get
			{
				return m_MuteInSoundGroup;
			}
			set
			{
				m_MuteInSoundGroup = value;
			}
		}

		public bool Loop
		{
			get
			{
				return m_Loop;
			}
			set
			{
				m_Loop = value;
			}
		}

		public int Priority
		{
			get
			{
				return m_Priority;
			}
			set
			{
				m_Priority = value;
			}
		}

		public float VolumeInSoundGroup
		{
			get
			{
				return m_VolumeInSoundGroup;
			}
			set
			{
				m_VolumeInSoundGroup = value;
			}
		}

		public float ShotVolumeScale
		{
			get
			{
				return m_ShotValumeScale;
			}
			set
			{
				m_ShotValumeScale = value;
			}
		}

		public float FadeInSeconds
		{
			get
			{
				return m_FadeInSeconds;
			}
			set
			{
				m_FadeInSeconds = value;
			}
		}

		public float FadeOutSeconds
		{
			get
			{
				return m_FadeOutSeconds;
			}
			set
			{
				m_FadeOutSeconds = value;
			}
		}

		public float Pitch
		{
			get
			{
				return m_Pitch;
			}
			set
			{
				m_Pitch = value;
			}
		}

		public float PanStereo
		{
			get
			{
				return m_PanStereo;
			}
			set
			{
				m_PanStereo = value;
			}
		}

		public float SpatialBlend
		{
			get
			{
				return m_SpatialBlend;
			}
			set
			{
				m_SpatialBlend = value;
			}
		}

		public float MaxDistance
		{
			get
			{
				return m_MaxDistance;
			}
			set
			{
				m_MaxDistance = value;
			}
		}

		public float DopplerLevel
		{
			get
			{
				return m_DopplerLevel;
			}
			set
			{
				m_DopplerLevel = value;
			}
		}

		public float SoundVolumeSet
		{
			get
			{
				return m_soundVolumeSet;
			}
			set
			{
				m_soundVolumeSet = value;
			}
		}

		public int Loop_Gap
		{
			get
			{
				return m_Loop_gap;
			}
			set
			{
				m_Loop_gap = value;
			}
		}

		public List<string> SoundAssetPaths
		{
			get
			{
				return m_SoundAssetPaths;
			}
			set
			{
				m_SoundAssetPaths = value;
			}
		}

		public PlaySoundParams()
		{
			m_Time = 0f;
			m_MuteInSoundGroup = false;
			m_Loop = false;
			m_Priority = 0;
			m_VolumeInSoundGroup = 1f;
			m_FadeInSeconds = 0f;
			m_Pitch = 1f;
			m_PanStereo = 0f;
			m_SpatialBlend = 0f;
			m_MaxDistance = 100f;
			m_DopplerLevel = 1f;
			m_soundVolumeSet = -1f;
			m_FadeOutSeconds = 0f;
			m_ShotValumeScale = 1f;
			m_Loop_gap = -1;
			m_SoundAssetPaths = null;
		}
	}

	internal sealed class PlaySoundInfo
	{
		private readonly Vector3 m_WorldPosition;

		private readonly object m_UserData;

		private readonly int m_SerialId;

		private readonly SoundGroup m_SoundGroup;

		private readonly PlaySoundParams m_PlaySoundParams;

		public Vector3 WorldPosition => m_WorldPosition;

		public object UserData => m_UserData;

		public int SerialId => m_SerialId;

		public SoundGroup SoundGroup => m_SoundGroup;

		public PlaySoundParams PlaySoundParams => m_PlaySoundParams;

		public PlaySoundInfo(int serialId, SoundGroup soundGroup, PlaySoundParams playSoundParams, object userData)
		{
			m_SerialId = serialId;
			m_SoundGroup = soundGroup;
			m_PlaySoundParams = playSoundParams;
			m_UserData = userData;
		}

		public PlaySoundInfo(Vector3 worldPosition, object userData)
		{
			m_WorldPosition = worldPosition;
			m_UserData = userData;
		}
	}

	[Serializable]
	public sealed class SoundGroup
	{
		private class OneShotSound
		{
			public Asset soundAsset;

			public float playTime;

			public float clipLength;

			public bool cacheAsset;

			public int serialId;

			public float soundVolumeSet;
		}

		public AudioMixerGroup mixerGroup;

		private List<AudioSource> _audioSources = new List<AudioSource>();

		private List<bool> _audioSourcePauses = new List<bool>();

		private List<int> _serialIds = new List<int>();

		private List<string> _audioUrls = new List<string>();

		public bool isEff;

		public bool isEvnSound;

		private bool _newGroup;

		private float _globalSoundVolumeRatio = 1f;

		private float _globalSettingVolumeRatio = 1f;

		private int m_serialId;

		private string m_Name;

		private AudioSource m_AudioSource;

		private Transform m_AudioSourceTransform;

		private Asset m_SoundAsset;

		private PlaySoundParams mCurrentSoundParams;

		private List<string> m_SoundAssetPaths;

		private float m_soundVolumeSet = -1f;

		private float m_soundStopTimer = -1f;

		private bool m_soundStopFadeOut;

		private float _fromVolume;

		private float _toVolume;

		private float _time;

		private float _curTime;

		private bool _changeVolume;

		private bool _inFadeOut;

		private int _waitSerialId;

		private float _startTime;

		private Asset _waitSoundAsset;

		private PlaySoundParams _waitSoundParams;

		private Action _waitOnFinishCallback;

		private Action _onFinishCallback;

		private List<OneShotSound> m_OneShots = new List<OneShotSound>();

		private bool _mute;

		private float _volume = 1f;

		public bool IsNewGroup => _newGroup;

		public float GlobalSoundVolumeRatio
		{
			get
			{
				return _globalSoundVolumeRatio;
			}
			set
			{
				_globalSoundVolumeRatio = value;
				float volume = Volume;
				Volume = volume;
			}
		}

		public float GlobalSettingVolumeRatio
		{
			get
			{
				return _globalSettingVolumeRatio;
			}
			set
			{
				_globalSettingVolumeRatio = value;
				float volume = Volume;
				Volume = volume;
			}
		}

		public int SerialId => m_serialId;

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		public float CurTime => m_AudioSource.time;

		public AudioSource AudioSource => m_AudioSource;

		public Transform AudioSourceTransform => m_AudioSourceTransform;

		public bool Mute
		{
			get
			{
				return _mute;
			}
			set
			{
				_mute = value;
				m_AudioSource.mute = value;
				for (int i = 0; i < _audioSources.Count; i++)
				{
					_audioSources[i].mute = value;
				}
			}
		}

		public float Volume
		{
			get
			{
				return _volume;
			}
			set
			{
				float num = value * GlobalSoundVolumeRatio * GlobalSettingVolumeRatio;
				SetAudioMixerGroupDbVolume(num);
				m_AudioSource.volume = num;
				_volume = value;
			}
		}

		public bool Loop
		{
			get
			{
				return m_AudioSource.loop;
			}
			set
			{
				m_AudioSource.loop = value;
			}
		}

		public bool HasSerialId(int serialId)
		{
			if (_serialIds.IndexOf(serialId) != -1)
			{
				return true;
			}
			return false;
		}

		public List<AudioSource> GetAudioSources()
		{
			return _audioSources;
		}

		public List<string> GetAudioURLs()
		{
			return _audioUrls;
		}

		public void AddAudioSource(AudioSource audioSource, string url, int id)
		{
			_serialIds.Add(id);
			_audioSources.Add(audioSource);
			_audioSourcePauses.Add(item: false);
			_audioUrls.Add(url);
		}

		public void RemoveAudioSource(int index)
		{
			_serialIds.RemoveAt(index);
			_audioSources.RemoveAt(index);
			_audioSourcePauses.RemoveAt(index);
			_audioUrls.RemoveAt(index);
		}

		public void ClearAllAudioSource()
		{
			for (int i = 0; i < _audioSources.Count; i++)
			{
				AudioSource audioSource = _audioSources[i];
				string path = _audioUrls[i];
				audioSource.Stop();
				GameEntry.Sound.UnspawnAudioSource(path, audioSource);
			}
			_serialIds.Clear();
			_audioSources.Clear();
			_audioSourcePauses.Clear();
			_audioUrls.Clear();
		}

		private SoundGroup()
		{
		}

		public SoundGroup(string name, bool useAudioMixer = false)
		{
			m_Name = name;
			_onFinishCallback = null;
			_newGroup = useAudioMixer;
			GameObject gameObject = new GameObject("SoundGroup_" + name);
			m_AudioSourceTransform = gameObject.transform;
			m_AudioSource = gameObject.AddComponent<AudioSource>();
			m_AudioSource.playOnAwake = false;
			m_AudioSource.rolloffMode = AudioRolloffMode.Custom;
			GlobalSoundVolumeRatio = GameEntry.Sound.globalSoundVolumeRatio;
			switch (name)
			{
			case "Music":
			case "MUSIC":
			{
				float globalSettingVolumeRatio3 = GameEntry.Setting.GetFloat("MUSIC_VOLUME", 1f);
				GlobalSettingVolumeRatio = globalSettingVolumeRatio3;
				break;
			}
			case "AMBSound":
			case "AMB":
			{
				float globalSettingVolumeRatio2 = GameEntry.Setting.GetFloat("ENV_SOUND_VOLUME", 1f);
				GlobalSettingVolumeRatio = globalSettingVolumeRatio2;
				break;
			}
			default:
			{
				float globalSettingVolumeRatio = GameEntry.Setting.GetFloat("EFFECT_VOLUME", 1f);
				GlobalSettingVolumeRatio = globalSettingVolumeRatio;
				break;
			}
			}
		}

		private void SetAudioMixerGroupDbVolume(float v)
		{
			if (mixerGroup != null)
			{
				float num = Mathf.Clamp01(v);
				float value = ((num > 0.0001f) ? (Mathf.Log10(num) * 20f) : (-80f));
				string name = mixerGroup.name + "_Volume";
				mixerGroup.audioMixer.SetFloat(name, value);
			}
		}

		public float GetAudioMixerGroupDbVolume()
		{
			float value = float.MinValue;
			if (mixerGroup != null)
			{
				string name = mixerGroup.name + "_Volume";
				mixerGroup.audioMixer.GetFloat(name, out value);
			}
			return value;
		}

		public void PlaySound(int serialId, Asset soundAsset, PlaySoundParams playSoundParams, float startTime = 0f, float dspTime = -1f, Action onFinishCallback = null)
		{
			if (_inFadeOut)
			{
				_waitSerialId = serialId;
				_waitSoundAsset = soundAsset;
				_waitSoundParams = playSoundParams;
				_startTime = startTime;
				_waitOnFinishCallback = onFinishCallback;
				return;
			}
			_onFinishCallback = onFinishCallback;
			mCurrentSoundParams = playSoundParams;
			if (m_SoundAsset != null)
			{
				m_SoundAsset.Release();
			}
			if (m_OneShots.Count > 0)
			{
				foreach (OneShotSound oneShot in m_OneShots)
				{
					if (!oneShot.cacheAsset)
					{
						oneShot.soundAsset.Release();
					}
				}
				m_OneShots.Clear();
			}
			m_SoundAsset = soundAsset;
			AudioClip audioClip = soundAsset.asset as AudioClip;
			if (audioClip == null)
			{
				Log.Error("Audio Clip is Null");
				return;
			}
			if (m_AudioSource == null)
			{
				Log.Error("Audio Source is Null");
				return;
			}
			if (m_soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
				m_soundVolumeSet = -1f;
			}
			m_serialId = serialId;
			Loop = playSoundParams.Loop;
			m_soundStopTimer = -1f;
			m_soundStopFadeOut = false;
			float soundVolumeSet = playSoundParams.SoundVolumeSet;
			if (soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryControlGlobalSound(serialId, Name, soundVolumeSet);
				GlobalSoundVolumeRatio = 1f;
			}
			if (!Loop && audioClip.length > 0f)
			{
				m_soundStopTimer = audioClip.length;
			}
			if (playSoundParams.FadeOutSeconds > 0f && !Loop)
			{
				if (m_soundStopTimer < 0f)
				{
					m_soundStopTimer = audioClip.length;
				}
				m_soundStopTimer -= playSoundParams.FadeOutSeconds;
				m_soundStopTimer = Mathf.Max(float.Epsilon, m_soundStopTimer);
				m_soundStopFadeOut = true;
			}
			m_soundVolumeSet = soundVolumeSet;
			Volume = playSoundParams.VolumeInSoundGroup;
			if (startTime < 0f || startTime >= audioClip.length)
			{
				Log.Error("[AudioMixer]Set AudioSource to a wrong time : " + soundAsset.pathOrURL);
				startTime = 0f;
			}
			m_AudioSource.time = startTime;
			m_AudioSource.clip = audioClip;
			m_AudioSource.pitch = playSoundParams.Pitch;
			if (dspTime < 0f)
			{
				m_AudioSource.Play();
			}
			else
			{
				m_AudioSource.PlayScheduled(dspTime);
			}
			if (playSoundParams.FadeInSeconds > 0f)
			{
				Volume = 0f;
				ChangeVolume(playSoundParams.VolumeInSoundGroup, playSoundParams.FadeInSeconds);
			}
		}

		public void PlayAudioSource(int serialId, Asset soundAsset, PlaySoundParams playSoundParams, float startTime = 0f, float dspTime = -1f, Action onFinishCallback = null)
		{
			if (_inFadeOut)
			{
				_waitSerialId = serialId;
				_waitSoundAsset = soundAsset;
				_waitSoundParams = playSoundParams;
				_startTime = startTime;
				_waitOnFinishCallback = onFinishCallback;
				return;
			}
			_onFinishCallback = onFinishCallback;
			mCurrentSoundParams = playSoundParams;
			AudioSource audioSource = GameEntry.Sound.SpawnAudioSource(soundAsset.pathOrURL);
			if (audioSource == null)
			{
				Log.Error("[AudioMixer]AudioSource is Null");
				return;
			}
			audioSource.gameObject.name = soundAsset.pathOrURL;
			if (m_soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
				m_soundVolumeSet = -1f;
			}
			m_serialId = serialId;
			m_soundStopFadeOut = false;
			if (!Loop && audioSource.clip.length > 0f)
			{
				m_soundStopTimer = audioSource.clip.length;
			}
			if (playSoundParams.FadeOutSeconds > 0f && !Loop)
			{
				if (m_soundStopTimer < 0f)
				{
					m_soundStopTimer = audioSource.clip.length;
				}
				m_soundStopTimer -= playSoundParams.FadeOutSeconds;
				m_soundStopTimer = Mathf.Max(float.Epsilon, m_soundStopTimer);
				m_soundStopFadeOut = true;
			}
			Volume = playSoundParams.VolumeInSoundGroup;
			audioSource.pitch = playSoundParams.Pitch;
			audioSource.time = 0f;
			audioSource.mute = _mute;
			if (dspTime <= 0f)
			{
				if (startTime < 0f || startTime >= audioSource.clip.length)
				{
					Log.Error("[AudioMixer]Set AudioSource to a wrong time : " + soundAsset.pathOrURL);
					startTime = 0f;
				}
				audioSource.time = startTime;
				audioSource.Play();
			}
			else
			{
				audioSource.PlayScheduled(dspTime);
			}
			AddAudioSource(audioSource, soundAsset.pathOrURL, serialId);
			if (playSoundParams.FadeInSeconds > 0f)
			{
				Volume = 0f;
				ChangeVolume(1f, playSoundParams.FadeInSeconds);
			}
		}

		public void PlayOneShot(int serialId, Asset soundAsset, PlaySoundParams playSoundParams, bool cacheAsset = false)
		{
			AudioClip audioClip = soundAsset.asset as AudioClip;
			m_OneShots.Add(new OneShotSound
			{
				soundAsset = soundAsset,
				clipLength = ((audioClip != null) ? audioClip.length : 0f),
				playTime = 0f,
				cacheAsset = cacheAsset,
				serialId = serialId,
				soundVolumeSet = playSoundParams.SoundVolumeSet
			});
			m_serialId = serialId;
			Loop = playSoundParams.Loop;
			m_soundVolumeSet = -1f;
			float soundVolumeSet = playSoundParams.SoundVolumeSet;
			if (soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryControlGlobalSound(serialId, Name, soundVolumeSet);
				GlobalSoundVolumeRatio = 1f;
			}
			m_soundVolumeSet = soundVolumeSet;
			Volume = playSoundParams.VolumeInSoundGroup;
			m_AudioSource.PlayOneShot(audioClip, playSoundParams.ShotVolumeScale);
		}

		public bool StopSound()
		{
			if (m_soundVolumeSet > -1f)
			{
				GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
			}
			m_serialId = 0;
			m_soundVolumeSet = -1f;
			m_soundStopTimer = -1f;
			m_soundStopFadeOut = false;
			if (!_newGroup)
			{
				if (m_AudioSource != null)
				{
					m_AudioSource.Stop();
					m_AudioSource.clip = null;
				}
				if (m_SoundAsset != null)
				{
					m_SoundAsset.Release();
					m_SoundAsset = null;
				}
				if (m_OneShots.Count > 0)
				{
					foreach (OneShotSound oneShot in m_OneShots)
					{
						if (!oneShot.cacheAsset)
						{
							oneShot.soundAsset.Release();
						}
						if (oneShot.soundVolumeSet > -1f)
						{
							GameEntry.Sound.TryResumeGlobalSoundControl(oneShot.serialId);
						}
					}
					m_OneShots.Clear();
				}
			}
			else
			{
				ClearAllAudioSource();
			}
			_onFinishCallback = null;
			_inFadeOut = false;
			_waitSerialId = 0;
			_startTime = 0f;
			_waitSoundAsset = null;
			_waitSoundParams = null;
			return false;
		}

		public void PauseSound()
		{
			if (m_AudioSource != null)
			{
				m_AudioSource.Pause();
			}
			for (int i = 0; i < _audioSources.Count; i++)
			{
				_audioSources[i].Pause();
				_audioSourcePauses[i] = true;
			}
		}

		public void ResumeSound()
		{
			if (m_AudioSource != null)
			{
				m_AudioSource.UnPause();
			}
			for (int i = 0; i < _audioSources.Count; i++)
			{
				_audioSources[i].UnPause();
				_audioSourcePauses[i] = false;
			}
		}

		public void OnUpdate()
		{
			for (int num = m_OneShots.Count - 1; num >= 0; num--)
			{
				OneShotSound oneShotSound = m_OneShots[num];
				if (oneShotSound.playTime < oneShotSound.clipLength)
				{
					oneShotSound.playTime += Time.unscaledDeltaTime;
				}
				else
				{
					if (!oneShotSound.cacheAsset)
					{
						oneShotSound.soundAsset.Release();
					}
					if (oneShotSound.soundVolumeSet > -1f)
					{
						GameEntry.Sound.TryResumeGlobalSoundControl(oneShotSound.serialId);
					}
					m_OneShots.RemoveAt(num);
				}
			}
			if (_changeVolume)
			{
				_curTime += Time.unscaledDeltaTime;
				if (_curTime >= _time)
				{
					_changeVolume = false;
					Volume = _toVolume;
					if (_inFadeOut)
					{
						_inFadeOut = false;
						if (_waitSerialId > 0)
						{
							if (m_AudioSource != null)
							{
								m_AudioSource.Stop();
							}
							if (!GameEntry.Sound.IsUseAudioMixer() && !_newGroup)
							{
								PlaySound(_waitSerialId, _waitSoundAsset, _waitSoundParams, _startTime, -1f, _waitOnFinishCallback);
							}
							else if (GameEntry.Sound.IsUseAudioMixer() && _newGroup)
							{
								for (int i = 0; i < _audioSources.Count; i++)
								{
									_audioSources[i].Stop();
								}
								PlayAudioSource(_waitSerialId, _waitSoundAsset, _waitSoundParams, _startTime, -1f, _waitOnFinishCallback);
							}
							_waitSerialId = 0;
							_startTime = 0f;
							_waitSoundAsset = null;
							_waitSoundParams = null;
							_waitOnFinishCallback = null;
						}
						else
						{
							StopSound();
						}
					}
				}
				else
				{
					Volume = Mathf.Lerp(_fromVolume, _toVolume, _curTime / _time);
				}
			}
			if (m_soundStopTimer > 0f)
			{
				m_soundStopTimer -= Time.unscaledDeltaTime;
				if (m_soundStopTimer <= 0f)
				{
					if (m_soundVolumeSet > -1f)
					{
						GameEntry.Sound.TryResumeGlobalSoundControl(m_serialId);
						m_soundVolumeSet = -1f;
					}
					if (m_soundStopFadeOut)
					{
						m_soundStopFadeOut = false;
						FadeOutAndPlaySound(0.5f);
					}
					if (_onFinishCallback != null)
					{
						_onFinishCallback();
						_onFinishCallback = null;
					}
				}
			}
			if (_audioSources.Count <= 0)
			{
				return;
			}
			for (int num2 = _audioSources.Count - 1; num2 >= 0; num2--)
			{
				AudioSource audioSource = _audioSources[num2];
				if (!audioSource.isPlaying && !_audioSourcePauses[num2])
				{
					string path = _audioUrls[num2];
					GameEntry.Sound.UnspawnAudioSource(path, audioSource);
					RemoveAudioSource(num2);
				}
			}
		}

		public void ChangeVolume(float to, float time)
		{
			if (time > 0f)
			{
				_fromVolume = Volume;
				_toVolume = to;
				_time = time;
				_curTime = 0f;
				_changeVolume = true;
			}
		}

		public void FadeOutAndPlaySound(float time)
		{
			_inFadeOut = true;
			ChangeVolume(0f, time);
		}
	}

	private struct LastAmbParams
	{
		public string name;

		public bool useSoundPath2;

		public List<string> soundPathTable;

		public int reactive;

		public int loop_gap;

		public int pre_time;

		public float speed;
	}

	private HashSet<string> _groupNameStrs = new HashSet<string>
	{
		"Master", "UI_Reward", "UI_Click", "SFX_Battle_Player_Attk", "SFX_Battle_Player_Skill", "SFX_Battle_Enemy_Attk", "SFX_Battle_Boss_Attk", "SFX_Battle_Boss_Skill", "SFX_Battle_Object_Basic", "SFX_Battle_Object_Fast",
		"SFX_Env", "SFX_Gameplay", "VO", "AMB", "TIMELINE_SFX", "TIMELINE_Music", "MUSIC"
	};

	private AudioListener m_AudioListener;

	private Transform m_InstanceRoot;

	private readonly Dictionary<string, SoundGroup> m_SoundGroupDic = new Dictionary<string, SoundGroup>();

	private int m_Serial;

	private readonly Dictionary<int, Asset> m_SoundsBeingLoaded = new Dictionary<int, Asset>();

	private readonly HashSet<int> m_SoundsToReleaseOnLoad = new HashSet<int>();

	private int _bgMusicId;

	private int _ambIdSoundId;

	private Dictionary<ELoopSoundLimit, int> m_loopSoundLimitMap = new Dictionary<ELoopSoundLimit, int>();

	private Dictionary<ELoopSoundLimit, List<LoopTimerSound>> m_loopSoundTimerMap = new Dictionary<ELoopSoundLimit, List<LoopTimerSound>>();

	private readonly HashSet<int> _soundBeingLoadedCache = new HashSet<int>();

	private readonly Dictionary<string, Asset> _effectAssetCache = new Dictionary<string, Asset>();

	private string mCurrentBGMAssetPath;

	private PlaySoundParams mCurrentPlayBGMSoundParams;

	private ITimer mBGMFinishTimer;

	private ITimer mBGMDelayTimer;

	private string mCurrentAMBSoundAssetPath;

	private bool mAMBIsPlaying;

	private float mCurrentAMBSoundVolume = 1f;

	private PlaySoundParams mCurrentPlayAMBSoundParams;

	private ITimer mAMBSoundTimer;

	private ITimer mAMBSoundDelayTimer;

	private bool mPauseAMBSound;

	private ITimer mResetAMBdelayTimer;

	private bool _useAudioMixer;

	private LastAmbParams _lastAmbParams;

	private AudioMixer _audioMixer;

	private AudioSourcePool _audioSourcePool;

	private Dictionary<string, Asset> _assetMap = new Dictionary<string, Asset>();

	private Dictionary<string, List<AudioSource>> _audioRefMap = new Dictionary<string, List<AudioSource>>();

	private bool _audioMixerMode;

	private string _controlSoundGroupName = string.Empty;

	private int _controlSoundSerialId = -1;

	private float _globalSoundVolumeRatio = 1f;

	private const string MIXER_PATH = "Assets/Main/Sound/AudioMixer.mixer";

	private Asset _audioMixerAsset;

	private List<GameObject> _objs = new List<GameObject>();

	private List<string> _paths = new List<string>();

	public int SoundGroupCount => m_SoundGroupDic.Count;

	private float globalSoundVolumeRatio
	{
		get
		{
			return _globalSoundVolumeRatio;
		}
		set
		{
			if (!(Math.Abs(_globalSoundVolumeRatio - value) > float.Epsilon))
			{
				return;
			}
			_globalSoundVolumeRatio = value;
			foreach (SoundGroup value2 in m_SoundGroupDic.Values)
			{
				if (!value2.Name.Equals(_controlSoundGroupName))
				{
					value2.GlobalSoundVolumeRatio = value;
				}
			}
		}
	}

	public bool HasMixerGroup_Str(string name)
	{
		if (_groupNameStrs.Contains(name))
		{
			return true;
		}
		return false;
	}

	public bool CheckSoundWrongOutput()
	{
		if (IsUseAudioMixer())
		{
			bool flag = true;
			foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
			{
				if (!item.Value.Mute)
				{
					flag = false;
					break;
				}
			}
			if (flag && IsAnySoundPlaying())
			{
				return true;
			}
		}
		return false;
	}

	private bool IsAnySoundPlaying(float threshold = 0.01f, int sampleSize = 256)
	{
		float[] array = new float[sampleSize];
		AudioListener.GetOutputData(array, 0);
		float num = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			num += Mathf.Abs(array[i]);
		}
		return num / (float)array.Length > threshold;
	}

	public bool IsAudioMixerLoaded()
	{
		return _audioMixer != null;
	}

	private bool IsUseAudioMixer()
	{
		if (_useAudioMixer && _audioMixer != null)
		{
			return true;
		}
		return false;
	}

	public void SyncAudioMixerUsing(bool use)
	{
		if (use)
		{
			if (_audioMixerAsset != null)
			{
				_audioMixer = null;
				_audioMixerAsset.Release();
			}
			LoadAudioMixer(delegate(bool suc)
			{
				if (suc)
				{
					ChangeToAudioMixerMode();
				}
			});
		}
		else if (_audioMixerMode)
		{
			ChangeToSingleAudioSourceMode();
		}
	}

	private void ChangeToAudioMixerMode()
	{
		_audioMixerMode = true;
		CancelAMBSoundFinishTimer();
		CancelAMBSoundDelayTimer();
		string text = mCurrentBGMAssetPath;
		string value = mCurrentAMBSoundAssetPath;
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			item.Value.FadeOutAndPlaySound(0.5f);
		}
		_useAudioMixer = true;
		if (!string.IsNullOrEmpty(text))
		{
			PlayLastMusic(text);
		}
		if (!string.IsNullOrEmpty(value))
		{
			PlayLastAmbSound();
		}
	}

	private void ChangeToSingleAudioSourceMode()
	{
		_audioMixerMode = false;
		string text = mCurrentBGMAssetPath;
		string value = mCurrentAMBSoundAssetPath;
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			item.Value.FadeOutAndPlaySound(0.5f);
			item.Value.mixerGroup = null;
		}
		if (_audioMixerAsset != null)
		{
			_audioMixer = null;
			_audioMixerAsset.Release();
		}
		_useAudioMixer = false;
		GameEntry.Lua.Call("CSharpCallLuaInterface.SetUseMixerToLWSoundManager", param1: false);
		if (!string.IsNullOrEmpty(text))
		{
			PlayLastMusic(text);
		}
		if (!string.IsNullOrEmpty(value))
		{
			PlayLastAmbSound();
		}
	}

	private void PlayLastMusic(string path)
	{
		PlayMusic(path, loop: true, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true, -1, -1, 600);
	}

	private void PlayLastAmbSound()
	{
		mCurrentAMBSoundVolume = 1f;
		mCurrentAMBSoundAssetPath = "";
		int pre_time = _lastAmbParams.pre_time;
		PlayAMBSound(_lastAmbParams.name, _lastAmbParams.useSoundPath2, _lastAmbParams.soundPathTable, _lastAmbParams.reactive, _lastAmbParams.loop_gap, _lastAmbParams.pre_time + 600, _lastAmbParams.speed);
		_lastAmbParams.pre_time = pre_time;
	}

	public void TryAddAudioSourceAsset(Asset soundAsset)
	{
		if (_assetMap.ContainsKey(soundAsset.pathOrURL))
		{
			_assetMap[soundAsset.pathOrURL] = soundAsset;
		}
		else
		{
			_assetMap.Add(soundAsset.pathOrURL, soundAsset);
		}
		if (!_audioRefMap.ContainsKey(soundAsset.pathOrURL))
		{
			_audioRefMap.Add(soundAsset.pathOrURL, new List<AudioSource>());
		}
	}

	public AudioSource SpawnAudioSource(string path)
	{
		if (_assetMap.ContainsKey(path))
		{
			AudioSource audioSource = _audioSourcePool.Spawn();
			AudioSourcePool.CopyFromTo((_assetMap[path].asset as GameObject).GetComponent<AudioSource>(), audioSource);
			_audioRefMap[path].Add(audioSource);
			return audioSource;
		}
		return null;
	}

	public void UnspawnAudioSource(string path, AudioSource audioSource)
	{
		_audioSourcePool.Unspawn(audioSource);
		if (_audioRefMap.ContainsKey(path))
		{
			_audioRefMap[path].Remove(audioSource);
		}
		if (_audioRefMap[path].Count == 0 && !_effectAssetCache.ContainsKey(path))
		{
			_assetMap[path].Release();
			_assetMap.Remove(path);
		}
	}

	public void SetMasterGroupVolume(float v)
	{
		if (_audioMixer != null)
		{
			float num = Mathf.Clamp01(v);
			float value = ((num > 0.0001f) ? (Mathf.Log10(num) * 20f) : (-80f));
			_audioMixer.SetFloat("Master_Volume", value);
		}
	}

	public void SetAMBSoundPause(bool pause)
	{
		if (pause)
		{
			mPauseAMBSound = true;
			return;
		}
		mPauseAMBSound = false;
		if (!mAMBIsPlaying)
		{
			PlayAMBSoundInLoop();
		}
	}

	public void SetAMBSoundVolumeTo0()
	{
		mCurrentAMBSoundVolume = 0f;
		if (IsUseAudioMixer())
		{
			ChangeVolume("AMB", 0f, 0.5f);
		}
		else
		{
			ChangeVolume("AMBSound", 0f, 0.5f);
		}
		CancelResetAmbSoundVolumeTimer();
	}

	public void ResetAMBSoundVolumeInternal()
	{
		mCurrentAMBSoundVolume = 1f;
		if (IsUseAudioMixer())
		{
			ChangeVolume("AMB", 1f, 1f);
		}
		else
		{
			ChangeVolume("AMBSound", 1f, 1f);
		}
	}

	public void ResetAMBSoundVolume()
	{
		CancelResetAmbSoundVolumeTimer();
		mResetAMBdelayTimer = GameEntry.Timer.RegisterTimer(1.5f, ResetAMBSoundVolumeInternal);
	}

	public void CancelResetAmbSoundVolumeTimer()
	{
		if (mResetAMBdelayTimer != null)
		{
			GameEntry.Timer.CancelTimer(mResetAMBdelayTimer);
			mResetAMBdelayTimer = null;
		}
	}

	public void TryControlGlobalSound(int serialId, string soundGroupName, float volumeRatio)
	{
		_controlSoundSerialId = serialId;
		_controlSoundGroupName = soundGroupName;
		globalSoundVolumeRatio = volumeRatio;
	}

	public void TryResumeGlobalSoundControl(int serialId)
	{
		if (serialId == _controlSoundSerialId)
		{
			_controlSoundSerialId = -1;
			_controlSoundGroupName = string.Empty;
			globalSoundVolumeRatio = 1f;
		}
	}

	public SoundComponent()
	{
		m_Serial = 1;
		GameObject gameObject = new GameObject("SoundComponent");
		m_InstanceRoot = gameObject.transform;
		m_AudioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
		if (m_AudioListener == null)
		{
			m_InstanceRoot.gameObject.AddComponent<AudioListener>();
		}
		_audioSourcePool = new AudioSourcePool(gameObject.transform);
	}

	private void LoadAudioMixer(Action<bool> complete = null)
	{
		if (!GameEntry.Resource.HasAsset("Assets/Main/Sound/AudioMixer.mixer"))
		{
			return;
		}
		_audioMixerAsset = GameEntry.Resource.LoadAssetAsync("Assets/Main/Sound/AudioMixer.mixer", typeof(AudioMixer));
		if (_audioMixerAsset != null)
		{
			Asset audioMixerAsset = _audioMixerAsset;
			audioMixerAsset.completed = (Action<Asset>)Delegate.Combine(audioMixerAsset.completed, (Action<Asset>)delegate
			{
				if (!_audioMixerAsset.isError)
				{
					_audioMixer = _audioMixerAsset.asset as AudioMixer;
					complete?.Invoke(obj: true);
					GameEntry.Lua.Call("CSharpCallLuaInterface.SetUseMixerToLWSoundManager", param1: true);
				}
				else
				{
					complete?.Invoke(obj: false);
					Log.Warning("[AudioMixer]AudioMixer.mixer Load Failed!");
				}
			});
		}
		else
		{
			complete?.Invoke(obj: false);
			Log.Warning("[AudioMixer]AudioMixer.mixer File Not Find!");
		}
	}

	public bool HasSoundGroup(string soundGroupName)
	{
		return m_SoundGroupDic.ContainsKey(soundGroupName);
	}

	public void SetSoundGroupMute(string soundGroupName, bool mute, bool newGroup = false)
	{
		GetSoundGroup(soundGroupName, soundGroupMute: false, 1f, newGroup).Mute = mute;
		TimelineAudioManager.Inst.ChangeMute(soundGroupName, mute);
	}

	public int PlaySound(string soundAssetName, string soundGroupName, float volume = 1f, float soundVolumeSet = -1f)
	{
		return PlaySound(soundAssetName, soundGroupName, new PlaySoundParams
		{
			VolumeInSoundGroup = volume,
			SoundVolumeSet = soundVolumeSet
		}, null);
	}

	private void CancelBGMFinishTimer()
	{
		if (mBGMFinishTimer != null)
		{
			GameEntry.Timer.CancelTimer(mBGMFinishTimer);
			mBGMFinishTimer = null;
		}
	}

	public void OnBGMPlayFinished()
	{
		if (mCurrentPlayBGMSoundParams != null && mCurrentPlayBGMSoundParams.Loop_Gap >= 0)
		{
			CancelBGMFinishTimer();
			float delaySec = (float)mCurrentPlayBGMSoundParams.Loop_Gap / 1000f;
			mBGMFinishTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayBGMSoundInLoop);
		}
	}

	private void CancelBGMDelayTimer()
	{
		if (mBGMDelayTimer != null)
		{
			GameEntry.Timer.CancelTimer(mBGMDelayTimer);
			mBGMDelayTimer = null;
		}
	}

	private void PlayBGMSoundInDelay()
	{
		string text = mCurrentBGMAssetPath;
		CancelBGMDelayTimer();
		if (!string.IsNullOrEmpty(text))
		{
			_bgMusicId = PlaySound(text, "Music", mCurrentPlayBGMSoundParams, null, 0f, OnBGMPlayFinished);
		}
	}

	public int PlayMusic(string name, bool loop = true, float fadeInSeconds = 0.5f, float startTime = 0f, float volume = 1f, float soundVolumeSet = -1f, float speed = 1f, bool useSoundPath2 = false, int reactive = -1, int loop_gap = -1, int pre_time = -1, List<string> soundPathTable = null)
	{
		string text = "";
		text = ((!useSoundPath2) ? $"Assets/Main/Sound/Music/{name}.ogg" : name);
		Action onFinishCallback = null;
		if (loop_gap >= 0)
		{
			onFinishCallback = OnBGMPlayFinished;
		}
		if (reactive == 1 && mCurrentBGMAssetPath != null && mCurrentBGMAssetPath == text)
		{
			return _bgMusicId;
		}
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, fadeInSeconds);
		}
		mCurrentBGMAssetPath = text;
		mCurrentPlayBGMSoundParams = new PlaySoundParams
		{
			Loop = loop,
			FadeInSeconds = fadeInSeconds,
			VolumeInSoundGroup = volume,
			SoundVolumeSet = soundVolumeSet,
			Loop_Gap = loop_gap,
			SoundAssetPaths = soundPathTable,
			Pitch = speed
		};
		if (pre_time > 0)
		{
			CancelBGMDelayTimer();
			float delaySec = (float)pre_time / 1000f;
			mBGMDelayTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayBGMSoundInDelay);
			return -1;
		}
		return PlaySound(text, "Music", mCurrentPlayBGMSoundParams, null, startTime, onFinishCallback);
	}

	private void PlayBGMSoundInLoop()
	{
		string text = null;
		if (mCurrentBGMAssetPath != null)
		{
			text = ((mCurrentPlayBGMSoundParams.SoundAssetPaths == null || mCurrentPlayBGMSoundParams.SoundAssetPaths.Count <= 0) ? mCurrentBGMAssetPath : mCurrentPlayBGMSoundParams.SoundAssetPaths[UnityEngine.Random.Range(0, mCurrentPlayBGMSoundParams.SoundAssetPaths.Count)]);
			CancelBGMFinishTimer();
			if (!string.IsNullOrEmpty(text))
			{
				_bgMusicId = PlaySound(text, "Music", mCurrentPlayBGMSoundParams, null, 0f, OnBGMPlayFinished);
			}
		}
	}

	public int PlayAMBSound(string name, bool useSoundPath2, List<string> soundPathTable, int reactive = -1, int loop_gap = -1, int pre_time = -1, float speed = 1f)
	{
		_lastAmbParams = default(LastAmbParams);
		_lastAmbParams.name = name;
		_lastAmbParams.useSoundPath2 = useSoundPath2;
		_lastAmbParams.soundPathTable = soundPathTable;
		_lastAmbParams.reactive = reactive;
		_lastAmbParams.loop_gap = loop_gap;
		_lastAmbParams.pre_time = pre_time;
		_lastAmbParams.speed = speed;
		if (string.IsNullOrEmpty(name))
		{
			Log.Error("PlayAMBSound name '{0}' is not exist.", name);
			return -1;
		}
		string text = "";
		text = ((!useSoundPath2) ? $"Assets/Main/Sound/Music/{name}.ogg" : name);
		Action onFinishCallback = null;
		if (loop_gap >= 0)
		{
			onFinishCallback = OnAMBSoundPlayFinished;
		}
		if (reactive == 1 && mCurrentAMBSoundAssetPath != null && mCurrentAMBSoundAssetPath == text)
		{
			return _ambIdSoundId;
		}
		mCurrentPlayAMBSoundParams = new PlaySoundParams
		{
			Loop = false,
			FadeInSeconds = 0f,
			VolumeInSoundGroup = mCurrentAMBSoundVolume,
			SoundVolumeSet = -1f,
			Loop_Gap = loop_gap,
			SoundAssetPaths = soundPathTable,
			Pitch = speed
		};
		mCurrentAMBSoundAssetPath = text;
		if (pre_time > 0)
		{
			CancelAMBSoundDelayTimer();
			float delaySec = (float)pre_time / 1000f;
			mAMBSoundDelayTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayAMBSoundInDelay);
			return -1;
		}
		mAMBIsPlaying = true;
		_ambIdSoundId = PlaySound(text, "AMBSound", mCurrentPlayAMBSoundParams, null, 0f, onFinishCallback);
		return _ambIdSoundId;
	}

	public void OnAMBSoundPlayFinished()
	{
		if (mCurrentPlayAMBSoundParams != null && mCurrentPlayAMBSoundParams.Loop_Gap >= 0)
		{
			CancelAMBSoundFinishTimer();
			float delaySec = (float)mCurrentPlayAMBSoundParams.Loop_Gap / 1000f;
			mAMBIsPlaying = false;
			mAMBSoundTimer = GameEntry.Timer.RegisterTimer(delaySec, PlayAMBSoundInLoop);
		}
	}

	private void CancelAMBSoundFinishTimer()
	{
		if (mAMBSoundTimer != null)
		{
			GameEntry.Timer.CancelTimer(mAMBSoundTimer);
			mAMBSoundTimer = null;
		}
	}

	private void PlayAMBSoundInLoop()
	{
		string text = null;
		if (mCurrentPlayAMBSoundParams == null)
		{
			return;
		}
		text = ((mCurrentPlayAMBSoundParams.SoundAssetPaths == null || mCurrentPlayAMBSoundParams.SoundAssetPaths.Count <= 0) ? mCurrentAMBSoundAssetPath : mCurrentPlayAMBSoundParams.SoundAssetPaths[UnityEngine.Random.Range(0, mCurrentPlayAMBSoundParams.SoundAssetPaths.Count)]);
		CancelAMBSoundFinishTimer();
		if (string.IsNullOrEmpty(mCurrentAMBSoundAssetPath))
		{
			return;
		}
		if (mPauseAMBSound)
		{
			_ = mCurrentPlayAMBSoundParams.Loop_Gap;
			return;
		}
		mCurrentPlayAMBSoundParams.VolumeInSoundGroup = mCurrentAMBSoundVolume;
		mAMBIsPlaying = true;
		if (!string.IsNullOrEmpty(text))
		{
			_ambIdSoundId = PlaySound(text, "AMBSound", mCurrentPlayAMBSoundParams, null, 0f, OnAMBSoundPlayFinished);
		}
	}

	private void CancelAMBSoundDelayTimer()
	{
		if (mAMBSoundDelayTimer != null)
		{
			GameEntry.Timer.CancelTimer(mAMBSoundDelayTimer);
			mAMBSoundDelayTimer = null;
		}
	}

	private void PlayAMBSoundInDelay()
	{
		string text = mCurrentAMBSoundAssetPath;
		CancelAMBSoundFinishTimer();
		mAMBIsPlaying = true;
		mCurrentPlayAMBSoundParams.VolumeInSoundGroup = mCurrentAMBSoundVolume;
		if (!string.IsNullOrEmpty(text))
		{
			_ambIdSoundId = PlaySound(text, "AMBSound", mCurrentPlayAMBSoundParams, null, 0f, OnAMBSoundPlayFinished);
		}
	}

	public int PlayEffectFullPath(string path, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		if (IsUseAudioMixer())
		{
			path = SwitchToAudioSourcePath(path);
		}
		if (!SoundResourceDownloadManager.Instance.IsCanAsync(path, "Effect"))
		{
			return -1;
		}
		Asset soundAsset = GameEntry.Resource.LoadAssetAsync(path, typeof(UnityEngine.Object));
		if (soundAsset != null)
		{
			int serialId = GetSerial();
			m_SoundsBeingLoaded.Add(serialId, soundAsset);
			bool useAudioMixer = IsUseAudioMixer();
			Asset asset = soundAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (!soundAsset.isError)
				{
					m_SoundsBeingLoaded.Remove(serialId);
					if (m_SoundsToReleaseOnLoad.Contains(serialId))
					{
						m_SoundsToReleaseOnLoad.Remove(serialId);
						soundAsset.Release();
					}
					else if (useAudioMixer != IsUseAudioMixer())
					{
						soundAsset.Release();
					}
					else if (!useAudioMixer)
					{
						GetSoundGroup("Effect").PlayOneShot(serialId, soundAsset, new PlaySoundParams
						{
							VolumeInSoundGroup = 1f,
							ShotVolumeScale = volumeScale,
							SoundVolumeSet = soundVolumeSet
						});
					}
					else
					{
						GameObject gameObject = soundAsset.asset as GameObject;
						if (gameObject != null)
						{
							AudioSource component = gameObject.GetComponent<AudioSource>();
							if (component.clip == null)
							{
								Log.Error("AudioSource: {0} clip is null.", soundAsset.pathOrURL);
								soundAsset.Release();
							}
							else if (component.outputAudioMixerGroup == null)
							{
								Log.Error("AudioSource: {0} outputAudioMixerGroup is null.", soundAsset.pathOrURL);
								soundAsset.Release();
							}
							else
							{
								SoundGroup soundGroup = GetSoundGroup(component.outputAudioMixerGroup.name, soundGroupMute: false, 1f, useAudioMixer: true);
								TryAddAudioSourceAsset(soundAsset);
								soundGroup.PlayAudioSource(serialId, soundAsset, new PlaySoundParams
								{
									VolumeInSoundGroup = 1f,
									ShotVolumeScale = volumeScale,
									SoundVolumeSet = soundVolumeSet
								});
							}
						}
						else
						{
							Log.Error("AudioSource: {0} is not a gameObject.", soundAsset.pathOrURL);
							soundAsset.Release();
						}
					}
				}
				else
				{
					m_SoundsBeingLoaded.Remove(serialId);
					m_SoundsToReleaseOnLoad.Remove(serialId);
					soundAsset.Release();
				}
			});
			return serialId;
		}
		return -1;
	}

	public int PlayEffect(string name, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		string path = $"Assets/Main/Sound/Effect/{name}.ogg";
		return PlayEffectFullPath(path, volumeScale, soundVolumeSet);
	}

	private string SwitchToAudioSourcePath(string path)
	{
		if (!path.Contains("/prefab/"))
		{
			int num = path.LastIndexOf('/');
			string text = path.Substring(0, num);
			string text2 = path.Substring(num + 1);
			path = text + "/prefab/" + text2;
		}
		return Path.ChangeExtension(path, ".prefab");
	}

	private string SwitchToAudioClipPath(string path)
	{
		if (path.Contains("/prefab/"))
		{
			int num = path.LastIndexOf('/');
			if (num > 0)
			{
				string text = path.Substring(0, num);
				string text2 = path.Substring(num + 1);
				if (text.EndsWith("/prefab"))
				{
					text = text.Substring(0, text.LastIndexOf("/prefab"));
				}
				path = text + "/" + text2;
			}
		}
		return Path.ChangeExtension(path, ".ogg");
	}

	public int PlayEffectCache(string soundAssetName, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		soundAssetName = ((!IsUseAudioMixer()) ? SwitchToAudioClipPath(soundAssetName) : SwitchToAudioSourcePath(soundAssetName));
		int serialId = GetSerial();
		if (!_effectAssetCache.TryGetValue(soundAssetName, out var soundAsset))
		{
			if (!SoundResourceDownloadManager.Instance.IsCanAsync(soundAssetName, "Effect"))
			{
				return -1;
			}
			soundAsset = GameEntry.Resource.LoadAssetAsync(soundAssetName, typeof(UnityEngine.Object));
			if (soundAsset != null)
			{
				_effectAssetCache.Add(soundAssetName, soundAsset);
			}
		}
		if (soundAsset != null)
		{
			if (soundAsset.isDone)
			{
				if (!IsUseAudioMixer())
				{
					GetSoundGroup("Effect").PlayOneShot(serialId, soundAsset, new PlaySoundParams
					{
						VolumeInSoundGroup = 1f,
						ShotVolumeScale = volumeScale,
						SoundVolumeSet = soundVolumeSet
					}, cacheAsset: true);
				}
				else
				{
					GameObject gameObject = soundAsset.asset as GameObject;
					if (!(gameObject != null))
					{
						Log.Error("AudioSource: {0} is not a gameObject.", soundAsset.pathOrURL);
						return serialId;
					}
					AudioSource component = gameObject.GetComponent<AudioSource>();
					if (component.clip == null)
					{
						Log.Error("AudioSource: {0} clip is null.", soundAsset.pathOrURL);
						return serialId;
					}
					if (component.outputAudioMixerGroup == null)
					{
						Log.Error("AudioSource: {0} outputAudioMixerGroup is null.", soundAsset.pathOrURL);
						return serialId;
					}
					SoundGroup soundGroup = GetSoundGroup(component.outputAudioMixerGroup.name, soundGroupMute: false, 1f, useAudioMixer: true);
					TryAddAudioSourceAsset(soundAsset);
					soundGroup.PlayAudioSource(serialId, soundAsset, new PlaySoundParams
					{
						VolumeInSoundGroup = 1f,
						ShotVolumeScale = volumeScale,
						SoundVolumeSet = soundVolumeSet
					});
				}
				return serialId;
			}
			_soundBeingLoadedCache.Add(serialId);
			bool useAudioMixer = IsUseAudioMixer();
			Asset asset = soundAsset;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (!soundAsset.isError)
				{
					bool flag = _soundBeingLoadedCache.Remove(serialId);
					if (m_SoundsToReleaseOnLoad.Contains(serialId))
					{
						m_SoundsToReleaseOnLoad.Remove(serialId);
						if (!flag)
						{
							soundAsset.Release();
						}
					}
					else if (useAudioMixer != IsUseAudioMixer())
					{
						soundAsset.Release();
					}
					else if (!useAudioMixer)
					{
						GetSoundGroup("Effect").PlayOneShot(serialId, soundAsset, new PlaySoundParams
						{
							VolumeInSoundGroup = 1f,
							ShotVolumeScale = volumeScale,
							SoundVolumeSet = soundVolumeSet
						}, cacheAsset: true);
					}
					else
					{
						GameObject gameObject2 = soundAsset.asset as GameObject;
						if (gameObject2 == null)
						{
							Log.Error("soundAsset.asset: {0} is null or not a GameObject.", soundAsset.pathOrURL);
							soundAsset.Release();
						}
						else
						{
							AudioSource component2 = gameObject2.GetComponent<AudioSource>();
							if (component2.clip == null)
							{
								Log.Error("AudioSource: {0} clip is null.", soundAsset.pathOrURL);
								soundAsset.Release();
							}
							else if (component2.outputAudioMixerGroup == null)
							{
								Log.Error("AudioSource: {0} outputAudioMixerGroup is null.", soundAsset.pathOrURL);
								soundAsset.Release();
							}
							else
							{
								SoundGroup soundGroup2 = GetSoundGroup(component2.outputAudioMixerGroup.name, soundGroupMute: false, 1f, useAudioMixer: true);
								TryAddAudioSourceAsset(soundAsset);
								soundGroup2.PlayAudioSource(serialId, soundAsset, new PlaySoundParams
								{
									VolumeInSoundGroup = 1f,
									ShotVolumeScale = volumeScale,
									SoundVolumeSet = soundVolumeSet
								});
							}
						}
					}
				}
				else
				{
					bool num = _soundBeingLoadedCache.Remove(serialId);
					m_SoundsToReleaseOnLoad.Remove(serialId);
					if (!num)
					{
						soundAsset.Release();
					}
				}
			});
			return serialId;
		}
		return -1;
	}

	public bool ReleaseEffect(string name)
	{
		if (_effectAssetCache.TryGetValue(name, out var value))
		{
			value?.Release();
			_effectAssetCache.Remove(name);
			return true;
		}
		return false;
	}

	public void PlaySpecialMusic(string strMusicPath, bool loop, float fadeIn, Action<float, string> OnGetAudioLength = null)
	{
		if (IsUseAudioMixer())
		{
			strMusicPath = SwitchToAudioSourcePath(strMusicPath);
		}
		else
		{
			strMusicPath = SwitchToAudioClipPath(strMusicPath);
		}
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
		}
		Asset req = GameEntry.Resource.LoadAssetAsync(strMusicPath, typeof(UnityEngine.Object));
		if (req == null)
		{
			return;
		}
		int serialId = GetSerial();
		bool useAudioMixer = IsUseAudioMixer();
		SoundGroup soundGroup = GetSoundGroup("Music");
		m_SoundsBeingLoaded.Add(serialId, req);
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			PlaySoundParams playSoundParams = new PlaySoundParams
			{
				Loop = loop,
				FadeInSeconds = fadeIn,
				VolumeInSoundGroup = 1f,
				SoundVolumeSet = -1f
			};
			if (req.isError)
			{
				LoadSoundFailure(strMusicPath, req, new PlaySoundInfo(serialId, soundGroup, playSoundParams, null));
				if (OnGetAudioLength != null)
				{
					OnGetAudioLength(0f, req.error);
				}
			}
			else
			{
				LoadSoundSuccess(strMusicPath, req, 0f, new PlaySoundInfo(serialId, soundGroup, playSoundParams, null), 0f, -1f, null, useAudioMixer);
				if (OnGetAudioLength != null)
				{
					AudioClip audioClip = req.asset as AudioClip;
					if (audioClip != null)
					{
						OnGetAudioLength(audioClip.length, null);
					}
				}
			}
		});
	}

	public void PlayBGMWithDspTime(string strMusicPath, bool loop, float fadeIn, Func<float> getDspTimeFunc, Action onMusicLoadFinish = null)
	{
		SoundGroup soundGroup = null;
		if (IsUseAudioMixer())
		{
			strMusicPath = SwitchToAudioSourcePath(strMusicPath);
		}
		else
		{
			strMusicPath = SwitchToAudioClipPath(strMusicPath);
			soundGroup = GetSoundGroup("Music");
		}
		if (_bgMusicId > 0)
		{
			StopSound(_bgMusicId);
		}
		int serialId = GetSerial();
		_bgMusicId = serialId;
		Asset req = GameEntry.Resource.LoadAssetAsync(strMusicPath, typeof(UnityEngine.Object));
		if (req == null)
		{
			return;
		}
		bool useAudioMixer = IsUseAudioMixer();
		m_SoundsBeingLoaded.Add(serialId, req);
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			PlaySoundParams playSoundParams = new PlaySoundParams
			{
				Loop = loop,
				FadeInSeconds = fadeIn,
				VolumeInSoundGroup = 1f,
				SoundVolumeSet = -1f
			};
			if (req.isError)
			{
				LoadSoundFailure(strMusicPath, req, new PlaySoundInfo(serialId, soundGroup, playSoundParams, null));
				if (onMusicLoadFinish != null)
				{
					onMusicLoadFinish();
				}
			}
			else
			{
				float dspTime = 0f;
				if (getDspTimeFunc != null)
				{
					dspTime = getDspTimeFunc();
				}
				LoadSoundSuccess(strMusicPath, req, 0f, new PlaySoundInfo(serialId, soundGroup, playSoundParams, null), 0f, dspTime, null, useAudioMixer);
				if (onMusicLoadFinish != null)
				{
					if (useAudioMixer)
					{
						if (req.asset as GameObject != null)
						{
							onMusicLoadFinish();
						}
					}
					else if (req.asset as AudioClip != null)
					{
						onMusicLoadFinish();
					}
				}
			}
		});
	}

	public void PlayMainSceneBGMusic()
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
		}
		_bgMusicId = PlayMusic("bgm_base_day");
	}

	public void PlayBGMusicByName(string nameStr, float volume = 1f, float soundVolumeSet = -1f)
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 0.5f);
		}
		_bgMusicId = PlayMusic(nameStr, loop: true, 0.5f, 0f, volume, soundVolumeSet);
	}

	public void PlayBGMusicByNameWithStartTime(string nameStr, float startTime, bool isloop, float volume = 1f, float soundVolumeSet = -1f, float fadeTime = 0.5f, float speed = 1f, bool usePath2 = false, int reactive = -1, int loop_gap = -1, int pre_time = -1)
	{
		_bgMusicId = PlayMusic(nameStr, isloop, fadeTime, startTime, volume, soundVolumeSet, speed, usePath2, reactive, loop_gap, pre_time);
	}

	public void PlayGuideSceneBgMusic()
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
		}
		_bgMusicId = PlayMusic("bgm_base_day_02");
	}

	public void PlayLoadingBgMusic()
	{
		bool flag = false;
		long num = PlayerPrefs.GetString("SeasonEndTime", "0").ToLong();
		bool flag2 = PlayerPrefs.GetInt("USE_SEASON_BGM", 1) == 1;
		if (num > 0 && GameEntry.Timer.GetServerTime() < num && flag2)
		{
			SeasonType seasonType = (SeasonType)PlayerPrefs.GetInt("SEASON_MAP_TYPE", 0);
			DownloadMode downloadMode = DownloadMode.Base;
			switch (seasonType)
			{
			case SeasonType.CityStronghold:
				downloadMode = DownloadMode.Season2;
				break;
			case SeasonType.Snow:
				downloadMode = DownloadMode.Season3;
				break;
			case SeasonType.Mummy:
				downloadMode = DownloadMode.Season4;
				break;
			case SeasonType.Darkness:
				downloadMode = DownloadMode.Season5;
				break;
			}
			if (downloadMode != DownloadMode.Base && ResourcePackageManager.IsPackageDownloaded((int)downloadMode))
			{
				flag = TryPlayLoadingSeasonBGMMusic();
			}
		}
		if (!flag)
		{
			if (_bgMusicId > 0)
			{
				FadeOutAndPlayMusic(_bgMusicId, 1f);
			}
			if (!GameEntry.Setting.HasSetting("LOADING_DEFAULT_BGM"))
			{
				GameEntry.Setting.SetString("LOADING_DEFAULT_BGM", "Assets/Main/Sound/Music/bgm_base_day_01.ogg");
			}
			string name = GameEntry.Setting.GetString("LOADING_DEFAULT_BGM", "Assets/Main/Sound/Music/bgm_base_day_01.ogg");
			_bgMusicId = PlayMusic(name, loop: true, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true);
		}
	}

	public bool TryPlayLoadingSeasonBGMMusic()
	{
		string text = PlayerPrefs.GetString("SeasonBGM", "");
		if (!text.IsNullOrEmpty())
		{
			string[] array = text.Split(new char[1] { '|' });
			if (array.Length > 1)
			{
				if (array[0] == "1")
				{
					if (array.Length == 6)
					{
						string s = array[1];
						string text2 = array[2];
						_ = array[3];
						string s2 = array[4];
						string text3 = array[5];
						int.Parse(s);
						int loop_gap = -1;
						if (!string.IsNullOrEmpty(text3) && int.TryParse(text3, out var result))
						{
							loop_gap = result;
						}
						string text4 = string.Empty;
						string[] array2 = text2.Split(new char[1] { ';' });
						if (array2.Length > 1)
						{
							if (array2.Length != 0)
							{
								text4 = array2[UnityEngine.Random.Range(0, array2.Length - 1)];
							}
						}
						else
						{
							if (array2.Length != 1)
							{
								return false;
							}
							text4 = text2;
						}
						int.TryParse(s2, out var result2);
						if (_bgMusicId > 0)
						{
							FadeOutAndPlayMusic(_bgMusicId, 1f);
						}
						if (GameEntry.Resource.HasAsset(text4))
						{
							_bgMusicId = PlayMusic(text4, loop: false, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true, result2, loop_gap, 0);
						}
						else
						{
							_bgMusicId = PlayMusic("Assets/Main/Sound/Music/bgm_base_day_01.ogg", loop: true, 0.5f, 0f, 1f, -1f, 1f, useSoundPath2: true);
						}
						return true;
					}
					return false;
				}
				return false;
			}
			return false;
		}
		return false;
	}

	public void PlayPveSceneBGMusic()
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
		}
		_bgMusicId = PlayMusic("Bgm_Movie_Battle1");
	}

	public void PlayPveSceneBGMusicLW()
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
		}
		_bgMusicId = PlayMusic("Bgm_Movie_Battle1");
	}

	public void PlayParkourBattleBGMusic()
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
		}
		_bgMusicId = PlayMusic("bgm_pve_30034");
	}

	public void StopBGMusic()
	{
		if (_bgMusicId > 0)
		{
			FadeOutAndPlayMusic(_bgMusicId, 1f);
			_bgMusicId = 0;
		}
		mCurrentBGMAssetPath = null;
		StopAMBSound();
	}

	public void StopAMBSound()
	{
		if (_ambIdSoundId > 0)
		{
			FadeOutAndPlayMusic(_ambIdSoundId, 0.5f);
			_ambIdSoundId = 0;
		}
	}

	public int GetBGMusic()
	{
		return _bgMusicId;
	}

	public void OnUpdate(float elapseSeconds)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			item.Value.OnUpdate();
		}
		_audioSourcePool.Update();
	}

	public bool StopSound(int serialId)
	{
		if (m_SoundsBeingLoaded.ContainsKey(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		if (_soundBeingLoadedCache.Contains(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			if (item.Value == null)
			{
				continue;
			}
			if (item.Value.IsNewGroup)
			{
				if (item.Value.HasSerialId(serialId))
				{
					item.Value.StopSound();
					break;
				}
			}
			else if (item.Value.SerialId == serialId)
			{
				item.Value.StopSound();
				break;
			}
		}
		if (serialId == _bgMusicId)
		{
			mCurrentBGMAssetPath = null;
		}
		else if (serialId == _ambIdSoundId)
		{
			mAMBIsPlaying = false;
			mCurrentAMBSoundAssetPath = null;
		}
		return false;
	}

	public bool FadeOutAndPlayMusic(int serialId, float time)
	{
		if (m_SoundsBeingLoaded.ContainsKey(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		if (_soundBeingLoadedCache.Contains(serialId))
		{
			m_SoundsToReleaseOnLoad.Add(serialId);
			return true;
		}
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			if (item.Value != null && item.Value.SerialId == serialId)
			{
				item.Value.FadeOutAndPlaySound(time);
				if (serialId == _bgMusicId)
				{
					mCurrentBGMAssetPath = null;
				}
				else if (serialId == _ambIdSoundId)
				{
					mAMBIsPlaying = false;
					mCurrentAMBSoundAssetPath = null;
				}
				break;
			}
		}
		return false;
	}

	public void StopAllSounds()
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			item.Value.StopSound();
		}
		foreach (KeyValuePair<int, Asset> item2 in m_SoundsBeingLoaded)
		{
			m_SoundsToReleaseOnLoad.Add(item2.Key);
		}
		_soundBeingLoadedCache.Clear();
		foreach (KeyValuePair<string, Asset> item3 in _effectAssetCache)
		{
			Asset value = item3.Value;
			if (value.isDone)
			{
				value.Release();
			}
		}
		_effectAssetCache.Clear();
		mCurrentBGMAssetPath = null;
		mAMBIsPlaying = false;
		mCurrentAMBSoundAssetPath = null;
	}

	public void PauseSound(int serialId)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			if (item.Value.SerialId == serialId)
			{
				item.Value.PauseSound();
			}
		}
	}

	public void ResumeSound(int serialId)
	{
		foreach (KeyValuePair<string, SoundGroup> item in m_SoundGroupDic)
		{
			if (item.Value.SerialId == serialId)
			{
				item.Value.ResumeSound();
			}
		}
	}

	private int GetSerial()
	{
		if (++m_Serial == int.MaxValue)
		{
			m_Serial = 1;
		}
		return m_Serial;
	}

	public int PlaySound(string soundAssetName, string soundGroupName, PlaySoundParams playSoundParams, object userData, float startTime = 0f, Action _onFinishCallback = null)
	{
		int serialId = GetSerial();
		if (playSoundParams == null)
		{
			playSoundParams = new PlaySoundParams();
		}
		PlaySoundErrorCode? playSoundErrorCode = null;
		SoundGroup soundGroup = null;
		string text = null;
		if (IsUseAudioMixer())
		{
			soundAssetName = SwitchToAudioSourcePath(soundAssetName);
		}
		else
		{
			soundAssetName = SwitchToAudioClipPath(soundAssetName);
			soundGroup = GetSoundGroup(soundGroupName, soundGroupMute: false, 1f, IsUseAudioMixer());
			if (soundGroup == null)
			{
				Log.Error("Sound group '{0}' is not exist.", soundGroupName);
				playSoundErrorCode = PlaySoundErrorCode.SoundGroupNotExist;
				text = $"Sound group '{soundGroupName}' is not exist.";
				if (playSoundErrorCode.HasValue)
				{
					throw new GameFrameworkException(text);
				}
			}
		}
		bool useAudioMixer = IsUseAudioMixer();
		if (!SoundResourceDownloadManager.Instance.IsCanAsync(soundAssetName, soundGroupName))
		{
			return -1;
		}
		Asset req = GameEntry.Resource.LoadAssetAsync(soundAssetName, typeof(UnityEngine.Object));
		if (req != null)
		{
			m_SoundsBeingLoaded.Add(serialId, req);
			Asset asset = req;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (!req.isError)
				{
					LoadSoundSuccess(soundAssetName, req, 0f, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData), startTime, -1f, _onFinishCallback, useAudioMixer);
				}
				else
				{
					LoadSoundFailure(soundAssetName, req, new PlaySoundInfo(serialId, soundGroup, playSoundParams, userData));
				}
			});
		}
		return serialId;
	}

	private SoundGroup GetSoundGroup(string soundGroupName, bool soundGroupMute = false, float volume = 1f, bool useAudioMixer = false)
	{
		SoundGroup value = null;
		if (m_SoundGroupDic.TryGetValue(soundGroupName, out value))
		{
			if (useAudioMixer)
			{
				TryToSetupMixerGroup(value);
			}
			return value;
		}
		value = new SoundGroup(soundGroupName, useAudioMixer)
		{
			Mute = soundGroupMute,
			Volume = volume
		};
		if (useAudioMixer)
		{
			TryToSetupMixerGroup(value);
		}
		value.AudioSourceTransform.SetParent(m_InstanceRoot);
		m_SoundGroupDic.Add(soundGroupName, value);
		if (useAudioMixer)
		{
			if (!HasMixerGroup_Str(soundGroupName))
			{
				Log.Error("[AudioMixer]Group not find  : " + soundGroupName);
			}
		}
		else if (HasMixerGroup_Str(soundGroupName))
		{
			Log.Error("[AudioMixer]Group wrong : " + soundGroupName);
		}
		return value;
	}

	private void TryToSetupMixerGroup(SoundGroup soundGroup)
	{
		if (_audioMixer != null)
		{
			AudioMixerGroup[] array = _audioMixer.FindMatchingGroups(soundGroup.Name);
			if (array != null && array.Length != 0 && soundGroup.mixerGroup != array[0])
			{
				soundGroup.mixerGroup = array[0];
				soundGroup.isEff = GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.IsSoundEffectGroup", soundGroup.Name);
				soundGroup.isEvnSound = GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.IsEvnSoundEffectGroup", soundGroup.Name);
			}
		}
		if (soundGroup.isEff)
		{
			soundGroup.Mute = !GameEntry.Setting.GetBool("isEffectMusicOn");
		}
		else if (soundGroup.isEvnSound)
		{
			soundGroup.Mute = !GameEntry.Setting.GetBool("ENV_SOUND_ON");
		}
		else
		{
			soundGroup.Mute = !GameEntry.Setting.GetBool("isBGMusicOn");
		}
	}

	private void LoadSoundSuccess(string soundAssetName, Asset soundAsset, float duration, object userData, float startTime = 0f, float dspTime = -1f, Action _onFinishCallback = null, bool useAudioMixer = false)
	{
		PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
		if (playSoundInfo == null)
		{
			Debug.LogError("Play sound info is invalid.");
		}
		m_SoundsBeingLoaded.Remove(playSoundInfo.SerialId);
		if (m_SoundsToReleaseOnLoad.Contains(playSoundInfo.SerialId))
		{
			m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
			soundAsset.Release();
			return;
		}
		if (useAudioMixer != IsUseAudioMixer())
		{
			soundAsset.Release();
			return;
		}
		if (!useAudioMixer)
		{
			if (playSoundInfo.SoundGroup != null)
			{
				playSoundInfo.SoundGroup.PlaySound(playSoundInfo.SerialId, soundAsset, playSoundInfo.PlaySoundParams, startTime, dspTime, _onFinishCallback);
				return;
			}
			soundAsset.Release();
			Log.Error("Sound Group Can not Find!");
			return;
		}
		GameObject gameObject = soundAsset.asset as GameObject;
		if (gameObject != null)
		{
			AudioSource component = gameObject.GetComponent<AudioSource>();
			if (component.clip == null)
			{
				soundAsset.Release();
				Log.Warning("[AudioMixer]AudioSource: {0} clip is null.", soundAssetName);
				return;
			}
			if (component.outputAudioMixerGroup == null)
			{
				soundAsset.Release();
				Log.Warning("[AudioMixer]AudioSource: {0} outputAudioMixerGroup is null.", soundAssetName);
				return;
			}
			SoundGroup soundGroup = GetSoundGroup(component.outputAudioMixerGroup.name, soundGroupMute: false, 1f, useAudioMixer: true);
			soundGroup.ClearAllAudioSource();
			TryAddAudioSourceAsset(soundAsset);
			soundGroup.PlayAudioSource(playSoundInfo.SerialId, soundAsset, playSoundInfo.PlaySoundParams, startTime, dspTime, _onFinishCallback);
		}
		else
		{
			soundAsset.Release();
			Log.Warning("[AudioMixer]AudioSource: {0} is not a gameObject.", soundAssetName);
		}
	}

	private void LoadSoundFailure(string soundAssetName, Asset soundAsset, object userData)
	{
		PlaySoundInfo playSoundInfo = (PlaySoundInfo)userData;
		if (playSoundInfo == null)
		{
			Log.Error("Play sound info is invalid.");
		}
		m_SoundsBeingLoaded.Remove(playSoundInfo.SerialId);
		m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialId);
		Log.Error(string.Format("Load sound failure, asset name '{0}', status '{1}', error message '{2}'.", soundAssetName, soundAsset.error));
		soundAsset.Release();
	}

	public void ChangeVolume(string soundGroupName, float toVolume, float time)
	{
		SoundGroup soundGroup = GetSoundGroup(soundGroupName, soundGroupMute: false, 1f, IsUseAudioMixer());
		if (soundGroup != null)
		{
			if (time <= 0f)
			{
				soundGroup.Volume = toVolume;
			}
			else
			{
				soundGroup.ChangeVolume(toVolume, time);
			}
		}
	}

	public void ChangeGlobalSettingVolumeRatio(string soundGroupName, float toVolume, float time)
	{
		SoundGroup soundGroup = GetSoundGroup(soundGroupName, soundGroupMute: false, 1f, IsUseAudioMixer());
		if (soundGroup != null)
		{
			soundGroup.GlobalSettingVolumeRatio = toVolume;
			TimelineAudioManager.Inst.ChangeVolume(soundGroupName, toVolume);
		}
	}

	public void PlaySoundById(int soundId, string soundGroupName)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound2");
		if (!string.IsNullOrEmpty(templateData))
		{
			PlaySound(templateData, soundGroupName);
		}
	}

	public LoopTimerSound PlaySoundByIdWithLimit(int soundId)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_num");
		string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_time");
		if (int.TryParse(templateData, out var result) && int.TryParse(templateData2, out var result2))
		{
			float volumeScale = 1f;
			float soundVolumeSet = -1f;
			string templateData3 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_volume");
			string templateData4 = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound_set");
			if (!string.IsNullOrEmpty(templateData3) && float.TryParse(templateData3, out var result3))
			{
				volumeScale = result3;
			}
			if (!string.IsNullOrEmpty(templateData4) && float.TryParse(templateData4, out var result4))
			{
				soundVolumeSet = result4;
			}
			m_loopSoundLimitMap[(ELoopSoundLimit)soundId] = result;
			string[] array = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound").Split(new char[1] { ';' });
			string assetName = string.Empty;
			if (array.Length != 0)
			{
				assetName = array[UnityEngine.Random.Range(0, array.Length - 1)];
			}
			if (!m_loopSoundTimerMap.TryGetValue((ELoopSoundLimit)soundId, out var value))
			{
				LoopTimerSound loopTimerSound = new LoopTimerSound(soundId, assetName, result2 / 1000, volumeScale, soundVolumeSet);
				value = new List<LoopTimerSound> { loopTimerSound };
				m_loopSoundTimerMap.Add((ELoopSoundLimit)soundId, value);
				return loopTimerSound;
			}
			if (value.Count < result)
			{
				LoopTimerSound loopTimerSound2 = new LoopTimerSound(soundId, assetName, result2 / 1000, volumeScale, soundVolumeSet);
				value.Add(loopTimerSound2);
				return loopTimerSound2;
			}
		}
		return null;
	}

	public void StopPlayLoopSoundWithLimit(LoopTimerSound timer)
	{
		if (timer != null && m_loopSoundTimerMap.TryGetValue((ELoopSoundLimit)timer.soundId, out var value))
		{
			timer.Dispose();
			value.Remove(timer);
		}
	}

	public void GetAudioLength(string soundAssetName, Action<float> onGetAudioLength)
	{
		if (soundAssetName.IsNullOrEmpty())
		{
			Debug.LogError("soundAssetName is null or empty");
			return;
		}
		soundAssetName = (IsUseAudioMixer() ? SwitchToAudioSourcePath(soundAssetName) : SwitchToAudioClipPath(soundAssetName));
		Asset req = GameEntry.Resource.LoadAssetAsync(soundAssetName, typeof(UnityEngine.Object));
		if (req == null)
		{
			return;
		}
		bool useAudioMixer = IsUseAudioMixer();
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			if (!req.isError && useAudioMixer == IsUseAudioMixer())
			{
				if (!useAudioMixer)
				{
					AudioClip audioClip = req.asset as AudioClip;
					if (audioClip != null)
					{
						onGetAudioLength(audioClip.length);
					}
				}
				else
				{
					GameObject gameObject = req.asset as GameObject;
					if (gameObject != null)
					{
						AudioSource component = gameObject.GetComponent<AudioSource>();
						if (component.clip != null)
						{
							onGetAudioLength(component.clip.length);
						}
					}
				}
			}
		});
	}

	public int PlayTimelineSound(string name, bool loop = false, float fadeInSeconds = 0f, float startTime = 0f, float volume = 1f, float soundVolumeSet = -1f)
	{
		if (!GameEntry.Setting.GetBool("isEffectMusicOn"))
		{
			return 0;
		}
		string soundAssetName = $"Assets/Main/Sound/Timeline/{name}.ogg";
		return PlaySound(soundAssetName, "Timeline", new PlaySoundParams
		{
			Loop = loop,
			FadeInSeconds = fadeInSeconds,
			VolumeInSoundGroup = volume,
			SoundVolumeSet = soundVolumeSet
		}, null, startTime);
	}

	public double GetDSPTime()
	{
		return AudioSettings.dspTime;
	}

	public static string[] GetSoundPathArray(int soundId)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound");
		if (templateData.IsNullOrEmpty())
		{
			return GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound2").Split(new char[1] { ';' });
		}
		return templateData.Split(new char[1] { ';' });
	}

	public static string GetSoundPath(int soundId)
	{
		if (GameEntry.Sound.IsUseAudioMixer())
		{
			return GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "audiosource");
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound");
		if (templateData.IsNullOrEmpty())
		{
			return GameEntry.ConfigCache.GetTemplateData("lw_Sound", soundId, "sound2");
		}
		return templateData;
	}

	public void SetDspBufferSize(int buffSize)
	{
		AudioConfiguration configuration = AudioSettings.GetConfiguration();
		configuration.dspBufferSize = buffSize;
		AudioSettings.Reset(configuration);
	}

	public int GetDspBufferSize()
	{
		return AudioSettings.GetConfiguration().dspBufferSize;
	}

	public void GetAudioSourceObjs(Action<List<GameObject>> onGetObjs)
	{
		for (int i = 0; i < _audioSourcePool.root.childCount; i++)
		{
			_objs.Add(_audioSourcePool.root.GetChild(i).gameObject);
		}
		onGetObjs?.Invoke(_objs);
		_objs.Clear();
	}

	public void GetAudioSourcePaths(Action<List<string>> onGetPaths)
	{
	}
}
