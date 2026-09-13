using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

internal sealed class CinemachineMixer : PlayableBehaviour
{
	private struct ClipInfo
	{
		public ICinemachineCamera vcam;

		public float weight;

		public double localTime;

		public double duration;
	}

	private CinemachineBrain mBrain;

	private int mBrainOverrideId = -1;

	private bool mPlaying;

	private float mLastOverrideFrame;

	public override void OnPlayableDestroy(Playable playable)
	{
		if (mBrain != null)
		{
			mBrain.ReleaseCameraOverride(mBrainOverrideId);
		}
		mBrainOverrideId = -1;
	}

	public override void PrepareFrame(Playable playable, FrameData info)
	{
		mPlaying = info.evaluationType == FrameData.EvaluationType.Playback;
	}

	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		base.ProcessFrame(playable, info, playerData);
		GameObject gameObject = playerData as GameObject;
		if (gameObject == null)
		{
			mBrain = (CinemachineBrain)playerData;
		}
		else
		{
			mBrain = gameObject.GetComponent<CinemachineBrain>();
		}
		if (mBrain == null)
		{
			return;
		}
		int num = 0;
		ClipInfo clipInfo = default(ClipInfo);
		ClipInfo clipInfo2 = default(ClipInfo);
		for (int i = 0; i < playable.GetInputCount(); i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			ScriptPlayable<CinemachineShotPlayable> playable2 = (ScriptPlayable<CinemachineShotPlayable>)playable.GetInput(i);
			CinemachineShotPlayable behaviour = playable2.GetBehaviour();
			if (behaviour != null && behaviour.IsValid && playable.GetPlayState() == PlayState.Playing && inputWeight > 0f)
			{
				clipInfo = clipInfo2;
				clipInfo2.vcam = behaviour.VirtualCamera;
				clipInfo2.weight = inputWeight;
				clipInfo2.localTime = playable2.GetTime();
				clipInfo2.duration = playable2.GetDuration();
				if (++num == 2)
				{
					break;
				}
			}
		}
		bool flag = clipInfo2.weight >= 1f || clipInfo2.localTime < clipInfo2.duration / 2.0;
		if (num == 2)
		{
			flag = clipInfo2.localTime < clipInfo.localTime || (!(clipInfo2.localTime > clipInfo.localTime) && clipInfo2.duration >= clipInfo.duration);
		}
		ICinemachineCamera camA = (flag ? clipInfo.vcam : clipInfo2.vcam);
		ICinemachineCamera camB = (flag ? clipInfo2.vcam : clipInfo.vcam);
		float weightB = (flag ? clipInfo2.weight : (1f - clipInfo2.weight));
		mBrainOverrideId = mBrain.SetCameraOverride(mBrainOverrideId, camA, camB, weightB, GetDeltaTime(info.deltaTime));
	}

	private float GetDeltaTime(float deltaTime)
	{
		if (!mPlaying)
		{
			if (mBrainOverrideId < 0)
			{
				mLastOverrideFrame = -1f;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			deltaTime = Time.unscaledDeltaTime;
			if (!Application.isPlaying && (mLastOverrideFrame < 0f || realtimeSinceStartup - mLastOverrideFrame > Time.maximumDeltaTime))
			{
				deltaTime = -1f;
			}
			mLastOverrideFrame = realtimeSinceStartup;
		}
		return deltaTime;
	}
}
