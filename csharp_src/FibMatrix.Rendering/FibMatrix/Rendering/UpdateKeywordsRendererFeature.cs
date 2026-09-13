using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.Rendering;

public class UpdateKeywordsRendererFeature : ScriptableRendererFeature
{
	[Serializable]
	public struct KeywordsState
	{
		public string keywords;

		public bool enable;
	}

	private UpdateKeywordsPass m_UpdateKeywordsPass;

	[SerializeField]
	public RenderPassEvent renderPassEvent;

	[SerializeField]
	public KeywordsState[] keywords;

	public override void Create()
	{
		m_UpdateKeywordsPass = new UpdateKeywordsPass(renderPassEvent, keywords);
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		renderer.EnqueuePass(m_UpdateKeywordsPass);
	}
}
