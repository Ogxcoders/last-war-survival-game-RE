using System;
using UnityEngine;

public class LoopTimerSound : IDisposable
{
	private ITimer timer;

	private IDisposable _disposableImplementation;

	private string assetName;

	private string[] assetNameArray;

	private bool isBGM;

	private float volumeScale;

	private float soundVolumeSet;

	private int playingSoundSerialId = -1;

	public int soundId { get; set; }

	public LoopTimerSound(int soundId, string assetName, int soundLength, float volumeScale = 1f, float soundVolumeSet = -1f)
	{
		this.soundId = soundId;
		this.assetName = assetName;
		timer = GameEntry.Timer.RegisterTimerRepeat(0.01f, soundLength, delegate
		{
			CallbackAction();
		});
		timer.isPause = true;
		this.volumeScale = volumeScale;
		this.soundVolumeSet = soundVolumeSet;
	}

	public LoopTimerSound(int soundId, string[] assetNameArr, int soundLength, float volumeScale = 1f, float soundVolumeSet = -1f, bool isBGM = false)
	{
		this.soundId = soundId;
		assetNameArray = assetNameArr;
		timer = GameEntry.Timer.RegisterTimerRepeat(0.01f, soundLength, delegate
		{
			CallbackAction();
		});
		timer.isPause = true;
		this.volumeScale = volumeScale;
		this.soundVolumeSet = soundVolumeSet;
		this.isBGM = isBGM;
	}

	public void Pause()
	{
		if (timer != null)
		{
			timer.isPause = true;
		}
	}

	public void Resume()
	{
		if (timer != null)
		{
			timer.isPause = false;
		}
	}

	public void Dispose()
	{
		soundId = 0;
		if (timer != null)
		{
			GameEntry.Timer.CancelTimer(timer);
			timer = null;
		}
		GameEntry.Sound.StopSound(playingSoundSerialId);
		assetName = null;
		assetNameArray = null;
	}

	private void CallbackAction()
	{
		if (!string.IsNullOrEmpty(assetName))
		{
			playingSoundSerialId = GameEntry.Sound.PlayEffect(assetName, volumeScale, soundVolumeSet);
		}
		else if (assetNameArray != null && assetNameArray.Length != 0)
		{
			string name = assetNameArray[UnityEngine.Random.Range(0, assetNameArray.Length - 1)];
			if (!isBGM)
			{
				playingSoundSerialId = GameEntry.Sound.PlayEffect(name, volumeScale, soundVolumeSet);
			}
		}
	}
}
