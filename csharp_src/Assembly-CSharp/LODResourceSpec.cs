using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LODResourceSpec
{
	public LODResourceType Type;

	public int LodHigh;

	public int LodMid;

	public int LodLow;

	public float OverDraw;

	public int VertexNum;

	public int ParticleNum;

	public LODResourceSpec(LODResourceType type, int lodHigh, int lodMid, int lodLow, float overDraw, int vertexNum, int particleNum)
	{
		Type = type;
		LodHigh = lodHigh;
		LodMid = lodMid;
		LodLow = lodLow;
		OverDraw = overDraw;
		VertexNum = vertexNum;
		ParticleNum = particleNum;
	}

	public void GetDesc(StringBuilder builder)
	{
		builder.AppendLine($"{Type} 高LOD:{LodHigh}，中LOD:{LodMid}，低LOD:{LodLow}");
	}

	public bool GetCheckResultDesc(StringBuilder builder, LODResourceInfo inst, List<GameObject> high = null, List<GameObject> mid = null, List<GameObject> low = null, bool ignoreValid = false)
	{
		bool result = true;
		if (inst.LodHigh > LodHigh)
		{
			builder.AppendLine($"高LOD数量超过限制:{inst.StateHigh} {inst.LodHigh}>{LodHigh}");
			result = false;
			if (high != null)
			{
				foreach (GameObject item in high)
				{
					builder.AppendLine(item.name);
				}
			}
		}
		else if (!ignoreValid)
		{
			builder.AppendLine($"高LOD数量:{inst.StateHigh} {inst.LodHigh}");
		}
		if (inst.LodMid > LodMid)
		{
			builder.AppendLine($"中LOD数量超过限制:{inst.StateMid} {inst.LodMid}>{LodMid}");
			result = false;
			if (mid != null)
			{
				foreach (GameObject item2 in mid)
				{
					builder.AppendLine(item2.name);
				}
			}
		}
		else if (!ignoreValid)
		{
			builder.AppendLine($"中LOD数量:{inst.StateMid} {inst.LodMid}");
		}
		if (inst.LodLow > LodLow)
		{
			builder.AppendLine($"低LOD数量超过限制:{inst.StateLow} {inst.LodLow}>{LodLow}");
			result = false;
			if (low != null)
			{
				foreach (GameObject item3 in low)
				{
					builder.AppendLine(item3.name);
				}
			}
		}
		else if (!ignoreValid)
		{
			builder.AppendLine($"低LOD数量:{inst.StateLow} {inst.LodLow}");
		}
		if (inst.TriangleNum > VertexNum)
		{
			builder.AppendLine($"模型面数超过限制:{inst.TriangleNum} {inst.TriangleNum}>{VertexNum}");
			result = false;
		}
		else if (!ignoreValid)
		{
			builder.AppendLine($"模型面数:{inst.TriangleNum}");
		}
		return result;
	}
}
