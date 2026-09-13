using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FibMatrix.Rendering;

[HelpURL("https://rivergame.feishu.cn/wiki/Mca3wXs9ji9ue7kw3rgczjJQnPe")]
public class OverlayBackgroundComponent : MonoBehaviour
{
	private const string TestGroupName = "Test";

	[SerializeField]
	private List<Renderer> renderers;

	[SerializeField]
	[Range(0f, 1f)]
	private float target = 0.5f;

	public static OverlayBackgroundComponent instance { get; private set; }

	public bool clearDepth { get; set; } = true;

	private void Test()
	{
		DOTweenPath componentInChildren = GetComponentInChildren<DOTweenPath>();
		if (componentInChildren.tween != null && !componentInChildren.tween.IsComplete())
		{
			componentInChildren.tween.Complete();
		}
		componentInChildren.onPlay.RemoveAllListeners();
		componentInChildren.onComplete.RemoveAllListeners();
		if (OverlayBackground.m_Tween != null && !OverlayBackground.m_Tween.IsComplete())
		{
			OverlayBackground.m_Tween.onComplete = null;
			OverlayBackground.m_Tween.Complete(withCallbacks: false);
			OverlayBackground.m_Tween = null;
			OverlayBackground.Deregister();
		}
		componentInChildren.onPlay.AddListener(delegate
		{
			foreach (Renderer renderer in renderers)
			{
				OverlayBackground.Register(renderer);
			}
			OverlayBackground.Tween(target, 0.3f, active: true, clearDepth);
		});
		componentInChildren.onComplete.AddListener(delegate
		{
			OverlayBackground.Tween(OverlayBackground.Intensity, 1f, active: true, clearDepth, delegate(bool active, bool complete)
			{
				if (complete)
				{
					OverlayBackground.Tween(0f, 1f, active: false, clearDepth);
				}
			});
			GetComponentInChildren<ParticleSystem>().Play();
		});
		componentInChildren.tween.Restart();
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		if (instance != null)
		{
			Debug.LogError("duplicated instance");
		}
		else
		{
			instance = this;
		}
	}

	private void OnDisable()
	{
		instance = null;
	}

	private void OnDestroy()
	{
		OverlayBackground.Tween(0f, clearDepth: true, active: false);
		OverlayBackground.Deregister();
	}
}
