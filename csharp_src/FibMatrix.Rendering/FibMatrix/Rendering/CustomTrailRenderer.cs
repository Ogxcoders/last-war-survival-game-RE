using UnityEngine;

namespace FibMatrix.Rendering;

[RequireComponent(typeof(TrailRenderer))]
[HelpURL("https://rivergame.feishu.cn/wiki/wikcnksbofoZP0DrHDUGvoRR7Wf")]
public class CustomTrailRenderer : MonoBehaviour
{
	private TrailRenderer trailRenderer;

	private MaterialPropertyBlock materialPropertyBlock;

	private Vector3 positionOld;

	private float dissolveOffset = 1f;

	private float dissolveOffsetLast;

	private int dissolveOffsetId;

	private void Awake()
	{
		if (trailRenderer == null)
		{
			trailRenderer = GetComponent<TrailRenderer>();
		}
	}

	private void Update()
	{
		if (!(trailRenderer == null))
		{
			if (materialPropertyBlock == null)
			{
				materialPropertyBlock = new MaterialPropertyBlock();
				dissolveOffsetId = Shader.PropertyToID("_DissolveOffset");
			}
			Vector3 position = base.transform.position;
			float sqrMagnitude = (position - positionOld).sqrMagnitude;
			positionOld = position;
			if (sqrMagnitude > 0.0001f)
			{
				dissolveOffset = 1f;
			}
			else
			{
				dissolveOffset = Mathf.Clamp01(dissolveOffset - Time.deltaTime / trailRenderer.time);
			}
			if ((double)Mathf.Abs(dissolveOffset - dissolveOffsetLast) > 1E-05)
			{
				trailRenderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(dissolveOffsetId, dissolveOffset);
				trailRenderer.SetPropertyBlock(materialPropertyBlock);
			}
			dissolveOffsetLast = dissolveOffset;
		}
	}
}
