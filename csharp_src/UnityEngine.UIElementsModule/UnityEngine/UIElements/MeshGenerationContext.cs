using System;

namespace UnityEngine.UIElements;

public class MeshGenerationContext
{
	[Flags]
	internal enum MeshFlags
	{
		None = 0,
		UVisDisplacement = 1,
		IsSVGGradients = 2,
		IsCustomSVGGradients = 3
	}

	internal IStylePainter painter;

	public VisualElement visualElement => painter.visualElement;

	internal MeshGenerationContext(IStylePainter painter)
	{
		this.painter = painter;
	}

	public MeshWriteData Allocate(int vertexCount, int indexCount, Texture texture = null)
	{
		return painter.DrawMesh(vertexCount, indexCount, texture, null, MeshFlags.None);
	}

	internal MeshWriteData Allocate(int vertexCount, int indexCount, Texture texture, Material material, MeshFlags flags)
	{
		return painter.DrawMesh(vertexCount, indexCount, texture, material, flags);
	}
}
