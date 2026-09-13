using UnityEngine;

namespace FibMatrix.PerfTools;

public class OverdrawMonitorBuiltinRP : OverdrawMonitor
{
	private Shader replacementShader;

	private string replacementTag;

	protected override void AwakeInternal()
	{
		base.AwakeInternal();
		replacementShader = Shader.Find("Hidden/PerfTools/OverdrawMonitor/Overdraw");
		replacementTag = "RenderType";
	}

	private void OnPostRender()
	{
		CalculateOverdrawFromRT();
	}

	protected override void RecreateTexture()
	{
		base.RecreateTexture();
		if (base.overdrawTexture == null)
		{
			base.overdrawTexture = new RenderTexture(256, 256, 24, RenderTextureFormat.Default);
			base.overdrawTexture.name = "Overdraw_" + m_OverdrawCamera?.name;
			base.overdrawTexture.hideFlags = HideFlags.HideAndDontSave;
			base.overdrawTexture.enableRandomWrite = false;
		}
	}

	protected override void SetCameraTarget()
	{
		base.SetCameraTarget();
		m_OverdrawCamera.targetTexture = base.overdrawTexture;
		m_OverdrawCamera.aspect = m_TargetCamera.aspect;
		m_OverdrawCamera.SetReplacementShader(replacementShader, replacementTag);
	}

	public override void SetReplacementTag(string tag)
	{
		replacementTag = tag;
	}

	public override void ResetReplacementTag()
	{
		replacementTag = "RenderType";
	}

	public override string GetCurReplacementTag()
	{
		return replacementTag;
	}
}
