using GameKit.Base;
using UnityEngine;

public class VideoPlayerCreater : MonoBehaviour
{
	public string mCurentVideoPath;

	public void Start()
	{
	}

	public void OnDestroy()
	{
		if (!string.IsNullOrEmpty(mCurentVideoPath))
		{
			SingletonBehaviour<WebmVideoPlayerManager>.Instance.StopVideo(mCurentVideoPath);
		}
	}
}
