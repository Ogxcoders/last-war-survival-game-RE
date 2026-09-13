using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HoleImage : Image
{
	public override Material GetModifiedMaterial(Material baseMaterial)
	{
		Material material = baseMaterial;
		if (m_ShouldRecalculateStencil)
		{
			Transform stopAfter = MaskUtilities.FindRootSortOverrideCanvas(base.transform);
			m_StencilValue = (base.maskable ? MaskUtilities.GetStencilDepth(base.transform, stopAfter) : 0);
			m_ShouldRecalculateStencil = false;
		}
		Mask component = GetComponent<Mask>();
		if (m_StencilValue > 0 && (component == null || !component.IsActive()))
		{
			Material maskMaterial = StencilMaterial.Add(material, (1 << m_StencilValue) - 1, StencilOp.Keep, CompareFunction.NotEqual, ColorWriteMask.All, (1 << m_StencilValue) - 1, 0);
			StencilMaterial.Remove(m_MaskMaterial);
			m_MaskMaterial = maskMaterial;
			material = m_MaskMaterial;
		}
		return material;
	}
}
