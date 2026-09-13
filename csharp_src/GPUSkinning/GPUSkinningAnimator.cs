using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GPUSkinningAnimator : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private GPUSkinningAnimation anim;

	[HideInInspector]
	[SerializeField]
	private int defaultPlayingClipIndex;

	public static readonly int PropertyID_TextureSize = Shader.PropertyToID("_GPUSKin_TextureSize");

	public static readonly int PropertyID_ClipParams = Shader.PropertyToID("_GPUSKin_ClipParams");

	public static readonly int PropertyID_Matrix = Shader.PropertyToID("_GPUSKin_Matrix");

	public static readonly int PropertyID_GPUSkin = Shader.PropertyToID("_GPUSkin");

	private MeshRenderer[] mrArr;

	private float time;

	private int lastPlayingFrameIndex = -1;

	private GPUSkinningClip lastPlayingClip;

	private GPUSkinningClip playingClip;

	private MaterialPropertyBlock mpb;

	private Queue<string> statesQueued = new Queue<string>();

	private bool visible = true;

	public Action<string> PlayEndCallBack;

	private bool isPlaying;

	public bool Visible
	{
		get
		{
			if (!Application.isPlaying)
			{
				return true;
			}
			return visible;
		}
		set
		{
			visible = value;
		}
	}

	public bool IsPlaying => isPlaying;

	public string PlayingClipName
	{
		get
		{
			if (playingClip != null)
			{
				return playingClip.name;
			}
			return null;
		}
	}

	public Vector3 Position => base.transform.position;

	public Vector3 LocalPosition => base.transform.localPosition;

	public GPUSkinningWrapMode WrapMode
	{
		get
		{
			if (playingClip != null)
			{
				return playingClip.wrapMode;
			}
			return GPUSkinningWrapMode.Once;
		}
	}

	public bool IsTimeAtTheEndOfLoop
	{
		get
		{
			if (playingClip == null)
			{
				return false;
			}
			return GetFrameIndex() == (int)(playingClip.length * (float)playingClip.fps) - 1;
		}
	}

	public float NormalizedTime
	{
		get
		{
			if (playingClip == null)
			{
				return 0f;
			}
			return (float)GetFrameIndex() / (float)((int)(playingClip.length * (float)playingClip.fps) - 1);
		}
		set
		{
			if (playingClip != null)
			{
				float num = Mathf.Clamp01(value);
				time = num * playingClip.length;
			}
		}
	}

	public void Init()
	{
		if (anim != null)
		{
			mrArr = GetComponentsInChildren<MeshRenderer>();
			mpb = new MaterialPropertyBlock();
			if (anim.clips != null && anim.clips.Length != 0)
			{
				Play(anim.clips[Mathf.Clamp(defaultPlayingClipIndex, 0, anim.clips.Length)].name);
			}
		}
	}

	public void Play(string clipName, float normalizedTime = 0f)
	{
		GPUSkinningClip[] clips = anim.clips;
		int num = ((clips != null) ? clips.Length : 0);
		for (int i = 0; i < num; i++)
		{
			if (clips[i].name == clipName)
			{
				if (playingClip != clips[i] || (playingClip != null && playingClip.wrapMode == GPUSkinningWrapMode.Once && IsTimeAtTheEndOfLoop) || (playingClip != null && !isPlaying))
				{
					SetNewPlayingClip(clips[i], normalizedTime);
				}
				break;
			}
		}
	}

	public void PlayQueued(string clipName)
	{
		statesQueued.Enqueue(clipName);
	}

	public void Stop()
	{
		isPlaying = false;
	}

	public void Resume()
	{
		if (playingClip != null)
		{
			isPlaying = true;
		}
	}

	public float GetClipLength(string name)
	{
		GPUSkinningClip[] clips = anim.clips;
		int num = ((clips != null) ? clips.Length : 0);
		for (int i = 0; i < num; i++)
		{
			if (clips[i].name == name)
			{
				return clips[i].length;
			}
		}
		return 0f;
	}

	public bool HasClip(string name)
	{
		GPUSkinningClip[] clips = anim.clips;
		int num = ((clips != null) ? clips.Length : 0);
		for (int i = 0; i < num; i++)
		{
			if (clips[i].name == name)
			{
				return true;
			}
		}
		return false;
	}

	private void Update_Game(float deltaTime)
	{
		if (!isPlaying || playingClip == null)
		{
			return;
		}
		time += deltaTime;
		if (playingClip.wrapMode == GPUSkinningWrapMode.Once && time > playingClip.length)
		{
			time = playingClip.length;
			if (PlayEndCallBack != null)
			{
				PlayEndCallBack(PlayingClipName);
			}
		}
		if (statesQueued.Count > 0 && time >= playingClip.length)
		{
			time = 0f;
			Play(statesQueued.Dequeue());
		}
		if (!visible)
		{
			return;
		}
		int frameIndex = GetFrameIndex();
		if (lastPlayingClip != playingClip || lastPlayingFrameIndex != frameIndex)
		{
			lastPlayingClip = playingClip;
			lastPlayingFrameIndex = frameIndex;
			mpb.SetVector(PropertyID_ClipParams, new Vector4(playingClip.pixelSegmentation, frameIndex, 0f, 0f));
			for (int i = 0; i < mrArr.Length; i++)
			{
				mrArr[i].SetPropertyBlock(mpb);
			}
		}
	}

	private void SetNewPlayingClip(GPUSkinningClip clip, float normalizedTime)
	{
		isPlaying = true;
		playingClip = clip;
		time = normalizedTime;
	}

	private int GetFrameIndex()
	{
		if (playingClip.length == time)
		{
			return (int)(playingClip.length * (float)playingClip.fps) - 1;
		}
		return (int)(time * (float)playingClip.fps) % (int)(playingClip.length * (float)playingClip.fps);
	}

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		Update_Game(Time.deltaTime);
	}

	private void OnDestroy()
	{
		anim = null;
	}
}
