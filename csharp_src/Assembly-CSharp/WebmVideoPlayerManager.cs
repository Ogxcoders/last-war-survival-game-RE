using System;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using VEngine;

public class WebmVideoPlayerManager : SingletonBehaviour<WebmVideoPlayerManager>
{
	public VideoPlayer mVideoPlayer;

	public string mCurVideoPath = "";

	public RawImage mRawImage;

	public RenderTexture mRT;

	public Vector2Int mRTSize = new Vector2Int(486, 810);

	public bool mIsVideoLoaded;

	private RawAssetFile rawFileAsync;

	public string mVideoURL = string.Empty;

	public void LoadVideo(RawImage rawImage, string videoPath, int width, int height)
	{
		if (rawImage == null || string.IsNullOrEmpty(videoPath))
		{
			Log.Error("LoadVideo invalid params.");
			return;
		}
		mRawImage = rawImage;
		if (mVideoPlayer == null)
		{
			CreateVideoPlayer();
		}
		mRTSize = new Vector2Int(width, height);
		if (string.Equals(mCurVideoPath, videoPath) && mIsVideoLoaded && !string.IsNullOrEmpty(mVideoURL))
		{
			PlayInternal();
			return;
		}
		mVideoPlayer.Stop();
		mIsVideoLoaded = false;
		mVideoURL = string.Empty;
		ClearRT();
		mCurVideoPath = videoPath;
		if (rawFileAsync != null)
		{
			rawFileAsync.Release();
			rawFileAsync.completed = null;
			rawFileAsync = null;
		}
		rawFileAsync = RawAssetFile.LoadAsync(mCurVideoPath, "webm");
		if (rawFileAsync == null)
		{
			Log.Error("Video loadAsync is null");
			return;
		}
		RawAssetFile rawAssetFile = rawFileAsync;
		rawAssetFile.completed = (Action<RawAssetFile>)Delegate.Combine(rawAssetFile.completed, new Action<RawAssetFile>(OnRawCompleted));
	}

	public void PuseVideo(string videoPath)
	{
		if (string.Equals(mCurVideoPath, videoPath))
		{
			mVideoPlayer.Pause();
			ClearRT();
		}
	}

	public void StopVideo(string videoPath)
	{
		if (string.Equals(mCurVideoPath, videoPath))
		{
			mVideoPlayer.Stop();
			ClearRT();
			if (rawFileAsync != null)
			{
				rawFileAsync.Release();
				rawFileAsync = null;
			}
			mCurVideoPath = string.Empty;
			mVideoURL = string.Empty;
		}
	}

	private void PlayInternal()
	{
		if (!(mVideoPlayer == null) && !string.IsNullOrEmpty(mVideoURL))
		{
			TryCreateRT();
			mVideoPlayer.targetTexture = mRT;
			if (mVideoPlayer.url != mVideoURL)
			{
				mVideoPlayer.url = mVideoURL;
			}
			mVideoPlayer.Play();
			GameEntry.Event.Fire(EventId.OnVideoWebmAssetLoaded);
		}
	}

	public void CreateVideoPlayer()
	{
		if (mVideoPlayer == null)
		{
			mVideoPlayer = base.gameObject.GetComponent<VideoPlayer>();
			if (mVideoPlayer == null)
			{
				mVideoPlayer = base.gameObject.AddComponent<VideoPlayer>();
			}
			mVideoPlayer.playOnAwake = false;
			mVideoPlayer.waitForFirstFrame = true;
			mVideoPlayer.skipOnDrop = true;
			mVideoPlayer.isLooping = true;
			mVideoPlayer.source = VideoSource.Url;
			mVideoPlayer.audioOutputMode = VideoAudioOutputMode.None;
			mVideoPlayer.renderMode = VideoRenderMode.RenderTexture;
		}
	}

	public void TryCreateRT()
	{
		if (mRawImage != null)
		{
			if (mRT == null)
			{
				mRT = RenderTexture.GetTemporary(mRTSize.x, mRTSize.y, 0, RenderTextureFormat.ARGB32);
				mRT.name = "webmVideoRT";
			}
			else if (mRT.width != mRTSize.x || mRT.height != mRTSize.y)
			{
				RenderTexture.ReleaseTemporary(mRT);
				mRT = RenderTexture.GetTemporary(mRTSize.x, mRTSize.y, 0, RenderTextureFormat.ARGB32);
				mRT.name = "webmVideoRT";
			}
			mRawImage.texture = mRT;
		}
		if (mVideoPlayer != null)
		{
			mVideoPlayer.targetTexture = mRT;
		}
	}

	public void ClearRT()
	{
		if (mRT != null)
		{
			RenderTexture active = RenderTexture.active;
			try
			{
				RenderTexture.active = mRT;
				GL.Clear(clearDepth: true, clearColor: true, Color.black);
			}
			finally
			{
				RenderTexture.active = active;
			}
			RenderTexture.ReleaseTemporary(mRT);
			mRT = null;
		}
	}

	private void OnRawCompleted(RawAssetFile rawFile)
	{
		if (rawFileAsync == null)
		{
			return;
		}
		if (rawFile != null && rawFile.isDone && rawFile.status == LoadableStatus.SuccessToLoad)
		{
			if (rawFile.name == mCurVideoPath)
			{
				OnVideoLoaded(rawFile.localRawAssetPath);
			}
			else
			{
				Log.Info("rawFile is already Unloaded:" + rawFile.name);
			}
		}
		else
		{
			Log.Error("Video file loading failed or incomplete.");
		}
		rawFileAsync.Release();
		rawFileAsync = null;
	}

	public void OnVideoLoaded(string path)
	{
		if (mVideoPlayer == null)
		{
			Log.Error("VideoPlayer component not found!");
			return;
		}
		TryCreateRT();
		mVideoPlayer.renderMode = VideoRenderMode.RenderTexture;
		mVideoPlayer.targetTexture = mRT;
		mVideoPlayer.source = VideoSource.Url;
		mVideoPlayer.url = path;
		mVideoURL = path;
		mVideoPlayer.Play();
		mIsVideoLoaded = true;
		GameEntry.Event.Fire(EventId.OnVideoWebmAssetLoaded);
	}
}
