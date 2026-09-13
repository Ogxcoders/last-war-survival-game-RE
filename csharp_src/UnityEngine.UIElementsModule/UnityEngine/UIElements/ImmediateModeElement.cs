using System;

namespace UnityEngine.UIElements;

public abstract class ImmediateModeElement : VisualElement
{
	public ImmediateModeElement()
	{
		base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(OnGenerateVisualContent));
	}

	private void OnGenerateVisualContent(MeshGenerationContext mgc)
	{
		mgc.painter.DrawImmediate(ImmediateRepaint);
	}

	protected abstract void ImmediateRepaint();
}
