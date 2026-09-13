#define UNITY_ASSERTIONS
using System;
using Unity.Collections;

namespace UnityEngine.UIElements.UIR.Implementation;

internal class UIRTextUpdatePainter : IStylePainter, IDisposable
{
	private VisualElement m_CurrentElement;

	private int m_TextEntryIndex;

	private NativeArray<Vertex> m_DudVerts;

	private NativeArray<ushort> m_DudIndices;

	private NativeSlice<Vertex> m_MeshDataVerts;

	private Color32 m_XFormClipPages;

	private Color32 m_IDsFlags;

	private Color32 m_OpacityPagesSettingsIndex;

	public MeshGenerationContext meshGenerationContext { get; }

	public VisualElement visualElement => m_CurrentElement;

	public UIRTextUpdatePainter()
	{
		meshGenerationContext = new MeshGenerationContext(this);
	}

	public void Begin(VisualElement ve, UIRenderDevice device)
	{
		Debug.Assert(ve.renderChainData.usesLegacyText && ve.renderChainData.textEntries.Count > 0);
		m_CurrentElement = ve;
		m_TextEntryIndex = 0;
		Alloc allocVerts = ve.renderChainData.data.allocVerts;
		NativeSlice<Vertex> slice = ve.renderChainData.data.allocPage.vertices.cpuData.Slice((int)allocVerts.start, (int)allocVerts.size);
		device.Update(ve.renderChainData.data, ve.renderChainData.data.allocVerts.size, out m_MeshDataVerts);
		RenderChainTextEntry renderChainTextEntry = ve.renderChainData.textEntries[0];
		if (ve.renderChainData.textEntries.Count > 1 || renderChainTextEntry.vertexCount != m_MeshDataVerts.Length)
		{
			m_MeshDataVerts.CopyFrom(slice);
		}
		int firstVertex = renderChainTextEntry.firstVertex;
		m_XFormClipPages = slice[firstVertex].xformClipPages;
		m_IDsFlags = slice[firstVertex].idsFlags;
		m_OpacityPagesSettingsIndex = slice[firstVertex].opacityPageSVGSettingIndex;
	}

	public void End()
	{
		Debug.Assert(m_TextEntryIndex == m_CurrentElement.renderChainData.textEntries.Count);
		m_CurrentElement = null;
	}

	public void Dispose()
	{
		if (m_DudVerts.IsCreated)
		{
			m_DudVerts.Dispose();
		}
		if (m_DudIndices.IsCreated)
		{
			m_DudIndices.Dispose();
		}
	}

	public void DrawRectangle(MeshGenerationContextUtils.RectangleParams rectParams)
	{
	}

	public void DrawBorder(MeshGenerationContextUtils.BorderParams borderParams)
	{
	}

	public void DrawImmediate(Action callback)
	{
	}

	public MeshWriteData DrawMesh(int vertexCount, int indexCount, Texture texture, Material material, MeshGenerationContext.MeshFlags flags)
	{
		if (m_DudVerts.Length < vertexCount)
		{
			if (m_DudVerts.IsCreated)
			{
				m_DudVerts.Dispose();
			}
			m_DudVerts = new NativeArray<Vertex>(vertexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}
		if (m_DudIndices.Length < indexCount)
		{
			if (m_DudIndices.IsCreated)
			{
				m_DudIndices.Dispose();
			}
			m_DudIndices = new NativeArray<ushort>(indexCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}
		return new MeshWriteData
		{
			m_Vertices = m_DudVerts.Slice(0, vertexCount),
			m_Indices = m_DudIndices.Slice(0, indexCount)
		};
	}

	public void DrawText(MeshGenerationContextUtils.TextParams textParams, TextHandle handle, float pixelsPerPoint)
	{
		float scaling = TextNative.ComputeTextScaling(m_CurrentElement.worldTransform, pixelsPerPoint);
		TextNativeSettings settings = new TextNativeSettings
		{
			text = textParams.text,
			font = textParams.font,
			size = textParams.fontSize,
			scaling = scaling,
			style = textParams.fontStyle,
			color = textParams.fontColor,
			anchor = textParams.anchor,
			wordWrap = textParams.wordWrap,
			wordWrapWidth = textParams.wordWrapWidth,
			richText = textParams.richText
		};
		if (settings.font == null)
		{
			return;
		}
		using NativeArray<TextVertex> uiVertices = TextNative.GetVertices(settings);
		RenderChainTextEntry renderChainTextEntry = m_CurrentElement.renderChainData.textEntries[m_TextEntryIndex++];
		Vector2 offset = TextNative.GetOffset(settings, textParams.rect);
		MeshBuilder.UpdateText(uiVertices, offset, m_CurrentElement.renderChainData.verticesSpace, m_XFormClipPages, m_IDsFlags, m_OpacityPagesSettingsIndex, m_MeshDataVerts.Slice(renderChainTextEntry.firstVertex, renderChainTextEntry.vertexCount));
		renderChainTextEntry.command.state.font = textParams.font.material.mainTexture;
	}
}
