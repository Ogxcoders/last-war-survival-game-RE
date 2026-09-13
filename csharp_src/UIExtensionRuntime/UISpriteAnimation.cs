using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISpriteAnimation : MonoBehaviour
{
	private Image ImageSource;

	private int mCurFrame;

	private float mDelta;

	public float FPS = 5f;

	public List<Sprite> SpriteFrames;

	public bool IsPlaying;

	public bool Foward = true;

	public bool AutoPlay;

	public bool Loop;

	public int FrameCount => SpriteFrames.Count;

	private void Awake()
	{
		ImageSource = GetComponent<Image>();
	}

	private void Start()
	{
		if (AutoPlay)
		{
			Play();
		}
		else
		{
			IsPlaying = false;
		}
	}

	private void SetSprite(int idx)
	{
		ImageSource.sprite = SpriteFrames[idx];
	}

	public void Play()
	{
		IsPlaying = true;
		Foward = true;
	}

	public void PlayReverse()
	{
		IsPlaying = true;
		Foward = false;
	}

	private void Update()
	{
		if (!IsPlaying || FrameCount == 0)
		{
			return;
		}
		mDelta += Time.deltaTime;
		if (!(mDelta > 1f / FPS))
		{
			return;
		}
		mDelta = 0f;
		if (Foward)
		{
			mCurFrame++;
		}
		else
		{
			mCurFrame--;
		}
		if (mCurFrame >= FrameCount)
		{
			if (!Loop)
			{
				IsPlaying = false;
				return;
			}
			mCurFrame = 0;
		}
		else if (mCurFrame < 0)
		{
			if (!Loop)
			{
				IsPlaying = false;
				return;
			}
			mCurFrame = FrameCount - 1;
		}
		SetSprite(mCurFrame);
	}

	public void Pause()
	{
		IsPlaying = false;
	}

	public void Resume()
	{
		if (!IsPlaying)
		{
			IsPlaying = true;
		}
	}

	public void Stop()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		IsPlaying = false;
	}

	public void Rewind()
	{
		mCurFrame = 0;
		SetSprite(mCurFrame);
		Play();
	}
}
