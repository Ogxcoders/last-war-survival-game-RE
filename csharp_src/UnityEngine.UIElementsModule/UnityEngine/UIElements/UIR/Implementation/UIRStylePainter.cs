#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.TextCore;

namespace UnityEngine.UIElements.UIR.Implementation;

internal class UIRStylePainter : IStylePainter, IDisposable
{
	internal struct Entry
	{
		public NativeSlice<Vertex> vertices;

		public NativeSlice<ushort> indices;

		public Material material;

		public Texture custom;

		public Texture font;

		public RenderChainCommand customCommand;

		public BMPAlloc clipRectID;

		public VertexFlags addFlags;

		public bool uvIsDisplacement;

		public bool isTextEntry;

		public bool isClipRegisterEntry;

		public bool isStencilClipped;
	}

	internal struct ClosingInfo
	{
		public bool needsClosing;

		public bool popViewMatrix;

		public bool popScissorClip;

		public RenderChainCommand clipUnregisterDrawCommand;

		public NativeSlice<Vertex> clipperRegisterVertices;

		public NativeSlice<ushort> clipperRegisterIndices;

		public int clipperRegisterIndexOffset;
	}

	internal struct TempDataAlloc<T> : IDisposable where T : struct
	{
		private int maxPoolElemCount;

		private NativeArray<T> pool;

		private List<NativeArray<T>> excess;

		private uint takenFromPool;

		public TempDataAlloc(int maxPoolElems)
		{
			maxPoolElemCount = maxPoolElems;
			pool = default(NativeArray<T>);
			excess = new List<NativeArray<T>>();
			takenFromPool = 0u;
		}

		public void Dispose()
		{
			foreach (NativeArray<T> item in excess)
			{
				item.Dispose();
			}
			excess.Clear();
			if (pool.IsCreated)
			{
				pool.Dispose();
			}
		}

		internal NativeSlice<T> Alloc(uint count)
		{
			if (takenFromPool + count <= pool.Length)
			{
				NativeSlice<T> result = pool.Slice((int)takenFromPool, (int)count);
				takenFromPool += count;
				return result;
			}
			NativeArray<T> nativeArray = new NativeArray<T>((int)count, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			excess.Add(nativeArray);
			return nativeArray;
		}

		internal void SessionDone()
		{
			int num = pool.Length;
			foreach (NativeArray<T> item in excess)
			{
				if (item.Length < maxPoolElemCount)
				{
					num += item.Length;
				}
				item.Dispose();
			}
			excess.Clear();
			if (num > pool.Length)
			{
				if (pool.IsCreated)
				{
					pool.Dispose();
				}
				pool = new NativeArray<T>(num, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			}
			takenFromPool = 0u;
		}
	}

	private RenderChain m_Owner;

	private List<Entry> m_Entries = new List<Entry>();

	private UIRAtlasManager m_AtlasManager;

	private VectorImageManager m_VectorImageManager;

	private Entry m_CurrentEntry;

	private ClosingInfo m_ClosingInfo;

	private bool m_StencilClip = false;

	private BMPAlloc m_ClipRectID = UIRVEShaderInfoAllocator.infiniteClipRect;

	private int m_SVGBackgroundEntryIndex = -1;

	private TempDataAlloc<Vertex> m_VertsPool = new TempDataAlloc<Vertex>(8192);

	private TempDataAlloc<ushort> m_IndicesPool = new TempDataAlloc<ushort>(16384);

	private List<MeshWriteData> m_MeshWriteDataPool;

	private int m_NextMeshWriteDataPoolItem;

	private MeshBuilder.AllocMeshData.Allocator m_AllocRawVertsIndicesDelegate;

	private MeshBuilder.AllocMeshData.Allocator m_AllocThroughDrawMeshDelegate;

	public MeshGenerationContext meshGenerationContext { get; }

	public VisualElement currentElement { get; private set; }

	public UIRenderDevice device { get; }

	public List<Entry> entries => m_Entries;

	public ClosingInfo closingInfo => m_ClosingInfo;

	public int totalVertices { get; private set; }

	public int totalIndices { get; private set; }

	protected bool disposed { get; private set; }

	public VisualElement visualElement => currentElement;

	private MeshWriteData GetPooledMeshWriteData()
	{
		if (m_NextMeshWriteDataPoolItem == m_MeshWriteDataPool.Count)
		{
			m_MeshWriteDataPool.Add(new MeshWriteData());
		}
		return m_MeshWriteDataPool[m_NextMeshWriteDataPoolItem++];
	}

	private MeshWriteData AllocRawVertsIndices(uint vertexCount, uint indexCount, ref MeshBuilder.AllocMeshData allocatorData)
	{
		m_CurrentEntry.vertices = m_VertsPool.Alloc(vertexCount);
		m_CurrentEntry.indices = m_IndicesPool.Alloc(indexCount);
		MeshWriteData pooledMeshWriteData = GetPooledMeshWriteData();
		pooledMeshWriteData.Reset(m_CurrentEntry.vertices, m_CurrentEntry.indices);
		return pooledMeshWriteData;
	}

	private MeshWriteData AllocThroughDrawMesh(uint vertexCount, uint indexCount, ref MeshBuilder.AllocMeshData allocatorData)
	{
		return DrawMesh((int)vertexCount, (int)indexCount, allocatorData.texture, allocatorData.material, allocatorData.flags);
	}

	public UIRStylePainter(RenderChain renderChain)
	{
		m_Owner = renderChain;
		meshGenerationContext = new MeshGenerationContext(this);
		device = renderChain.device;
		m_AtlasManager = renderChain.atlasManager;
		m_VectorImageManager = renderChain.vectorImageManager;
		m_AllocRawVertsIndicesDelegate = AllocRawVertsIndices;
		m_AllocThroughDrawMeshDelegate = AllocThroughDrawMesh;
		int num = 32;
		m_MeshWriteDataPool = new List<MeshWriteData>(num);
		for (int i = 0; i < num; i++)
		{
			m_MeshWriteDataPool.Add(new MeshWriteData());
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				m_IndicesPool.Dispose();
				m_VertsPool.Dispose();
			}
			disposed = true;
		}
	}

	public void Begin(VisualElement ve)
	{
		currentElement = ve;
		m_NextMeshWriteDataPoolItem = 0;
		m_SVGBackgroundEntryIndex = -1;
		currentElement.renderChainData.usesLegacyText = (currentElement.renderChainData.usesAtlas = (currentElement.renderChainData.disableNudging = false));
		currentElement.renderChainData.displacementUVStart = (currentElement.renderChainData.displacementUVEnd = 0);
		bool flag = (currentElement.renderHints & RenderHints.GroupTransform) != 0;
		if (flag)
		{
			RenderChainCommand renderChainCommand = m_Owner.AllocCommand();
			renderChainCommand.owner = currentElement;
			renderChainCommand.type = CommandType.PushView;
			m_Entries.Add(new Entry
			{
				customCommand = renderChainCommand
			});
			m_ClosingInfo.needsClosing = (m_ClosingInfo.popViewMatrix = true);
		}
		if (currentElement.hierarchy.parent != null)
		{
			m_StencilClip = currentElement.hierarchy.parent.renderChainData.isStencilClipped;
			m_ClipRectID = (flag ? UIRVEShaderInfoAllocator.infiniteClipRect : currentElement.hierarchy.parent.renderChainData.clipRectID);
		}
		else
		{
			m_StencilClip = false;
			m_ClipRectID = UIRVEShaderInfoAllocator.infiniteClipRect;
		}
	}

	public void LandClipUnregisterMeshDrawCommand(RenderChainCommand cmd)
	{
		Debug.Assert(m_ClosingInfo.needsClosing);
		m_ClosingInfo.clipUnregisterDrawCommand = cmd;
	}

	public void LandClipRegisterMesh(NativeSlice<Vertex> vertices, NativeSlice<ushort> indices, int indexOffset)
	{
		Debug.Assert(m_ClosingInfo.needsClosing);
		m_ClosingInfo.clipperRegisterVertices = vertices;
		m_ClosingInfo.clipperRegisterIndices = indices;
		m_ClosingInfo.clipperRegisterIndexOffset = indexOffset;
	}

	public MeshWriteData DrawMesh(int vertexCount, int indexCount, Texture texture, Material material, MeshGenerationContext.MeshFlags flags)
	{
		MeshWriteData pooledMeshWriteData = GetPooledMeshWriteData();
		if (vertexCount == 0 || indexCount == 0)
		{
			pooledMeshWriteData.Reset(default(NativeSlice<Vertex>), default(NativeSlice<ushort>));
			return pooledMeshWriteData;
		}
		m_CurrentEntry = new Entry
		{
			vertices = m_VertsPool.Alloc((uint)vertexCount),
			indices = m_IndicesPool.Alloc((uint)indexCount),
			material = material,
			uvIsDisplacement = (flags == MeshGenerationContext.MeshFlags.UVisDisplacement),
			clipRectID = m_ClipRectID,
			isStencilClipped = m_StencilClip,
			addFlags = VertexFlags.IsSolid
		};
		Debug.Assert(m_CurrentEntry.vertices.Length == vertexCount);
		Debug.Assert(m_CurrentEntry.indices.Length == indexCount);
		Rect uvRegion = new Rect(0f, 0f, 1f, 1f);
		bool flag = flags == MeshGenerationContext.MeshFlags.IsSVGGradients;
		bool flag2 = flags == MeshGenerationContext.MeshFlags.IsCustomSVGGradients;
		if (flag || flag2)
		{
			m_CurrentEntry.addFlags = (flag ? VertexFlags.IsSVGGradients : VertexFlags.IsCustomSVGGradients);
			if (flag2)
			{
				m_CurrentEntry.custom = texture;
			}
			currentElement.renderChainData.usesAtlas = true;
		}
		else if (texture != null)
		{
			if (m_AtlasManager != null && m_AtlasManager.TryGetLocation(texture as Texture2D, out var uvs))
			{
				m_CurrentEntry.addFlags = ((texture.filterMode == FilterMode.Point) ? VertexFlags.IsAtlasTexturedPoint : VertexFlags.IsAtlasTexturedBilinear);
				currentElement.renderChainData.usesAtlas = true;
				uvRegion = new Rect(uvs.x, uvs.y, uvs.width, uvs.height);
			}
			else
			{
				m_CurrentEntry.addFlags = VertexFlags.IsCustomTextured;
				m_CurrentEntry.custom = texture;
			}
		}
		pooledMeshWriteData.Reset(m_CurrentEntry.vertices, m_CurrentEntry.indices, uvRegion);
		m_Entries.Add(m_CurrentEntry);
		totalVertices += m_CurrentEntry.vertices.Length;
		totalIndices += m_CurrentEntry.indices.Length;
		m_CurrentEntry = default(Entry);
		return pooledMeshWriteData;
	}

	public void DrawText(MeshGenerationContextUtils.TextParams textParams, TextHandle handle, float pixelsPerPoint)
	{
		if (!(textParams.font == null))
		{
			if (handle.useLegacy)
			{
				DrawTextNative(textParams, handle, pixelsPerPoint);
			}
			else
			{
				DrawTextCore(textParams, handle, pixelsPerPoint);
			}
		}
	}

	private void DrawTextNative(MeshGenerationContextUtils.TextParams textParams, TextHandle handle, float pixelsPerPoint)
	{
		float scaling = TextHandle.ComputeTextScaling(currentElement.worldTransform, pixelsPerPoint);
		TextNativeSettings textNativeSettings = MeshGenerationContextUtils.TextParams.GetTextNativeSettings(textParams, scaling);
		using NativeArray<TextVertex> uiVertices = TextNative.GetVertices(textNativeSettings);
		if (uiVertices.Length != 0)
		{
			Vector2 offset = TextNative.GetOffset(textNativeSettings, textParams.rect);
			m_CurrentEntry.isTextEntry = true;
			m_CurrentEntry.clipRectID = m_ClipRectID;
			m_CurrentEntry.isStencilClipped = m_StencilClip;
			MeshBuilder.MakeText(uiVertices, offset, new MeshBuilder.AllocMeshData
			{
				alloc = m_AllocRawVertsIndicesDelegate
			});
			m_CurrentEntry.font = textParams.font.material.mainTexture;
			m_Entries.Add(m_CurrentEntry);
			totalVertices += m_CurrentEntry.vertices.Length;
			totalIndices += m_CurrentEntry.indices.Length;
			m_CurrentEntry = default(Entry);
			currentElement.renderChainData.usesLegacyText = true;
		}
	}

	private void DrawTextCore(MeshGenerationContextUtils.TextParams textParams, TextHandle handle, float pixelsPerPoint)
	{
		TextInfo textInfo = handle.Update(textParams, pixelsPerPoint);
		for (int i = 0; i < textInfo.materialCount && textInfo.meshInfo[i].vertexCount != 0; i++)
		{
			m_CurrentEntry.isTextEntry = true;
			m_CurrentEntry.clipRectID = m_ClipRectID;
			m_CurrentEntry.isStencilClipped = m_StencilClip;
			MeshBuilder.MakeText(textInfo.meshInfo[i], textParams.rect.min, new MeshBuilder.AllocMeshData
			{
				alloc = m_AllocRawVertsIndicesDelegate
			});
			m_CurrentEntry.font = textInfo.meshInfo[i].material.mainTexture;
			m_Entries.Add(m_CurrentEntry);
			totalVertices += m_CurrentEntry.vertices.Length;
			totalIndices += m_CurrentEntry.indices.Length;
			m_CurrentEntry = default(Entry);
		}
	}

	public void DrawRectangle(MeshGenerationContextUtils.RectangleParams rectParams)
	{
		MeshBuilder.AllocMeshData meshAlloc = new MeshBuilder.AllocMeshData
		{
			alloc = m_AllocThroughDrawMeshDelegate,
			texture = rectParams.texture,
			material = rectParams.material
		};
		if (rectParams.vectorImage != null)
		{
			DrawVectorImage(rectParams);
		}
		else if (rectParams.texture != null)
		{
			MeshBuilder.MakeTexturedRect(rectParams, 0.995f, meshAlloc);
		}
		else
		{
			MeshBuilder.MakeSolidRect(rectParams, 0.995f, meshAlloc);
		}
	}

	public void DrawBorder(MeshGenerationContextUtils.BorderParams borderParams)
	{
		MeshBuilder.MakeBorder(borderParams, 0.995f, new MeshBuilder.AllocMeshData
		{
			alloc = m_AllocThroughDrawMeshDelegate,
			material = borderParams.material,
			texture = null,
			flags = MeshGenerationContext.MeshFlags.UVisDisplacement
		});
	}

	public void DrawImmediate(Action callback)
	{
		RenderChainCommand renderChainCommand = m_Owner.AllocCommand();
		renderChainCommand.type = CommandType.Immediate;
		renderChainCommand.owner = currentElement;
		renderChainCommand.callback = callback;
		m_Entries.Add(new Entry
		{
			customCommand = renderChainCommand
		});
	}

	public void DrawVisualElementBackground()
	{
		if (currentElement.layout.width <= Mathf.Epsilon || currentElement.layout.height <= Mathf.Epsilon)
		{
			return;
		}
		ComputedStyle computedStyle = currentElement.computedStyle;
		if (computedStyle.backgroundColor != Color.clear)
		{
			MeshGenerationContextUtils.RectangleParams rectParams = new MeshGenerationContextUtils.RectangleParams
			{
				rect = GUIUtility.AlignRectToDevice(currentElement.rect),
				color = computedStyle.backgroundColor.value,
				playmodeTintColor = ((currentElement.panel.contextType == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white)
			};
			MeshGenerationContextUtils.GetVisualElementRadii(currentElement, out rectParams.topLeftRadius, out rectParams.bottomLeftRadius, out rectParams.topRightRadius, out rectParams.bottomRightRadius);
			DrawRectangle(rectParams);
		}
		Background value = computedStyle.backgroundImage.value;
		if (value.texture != null || value.vectorImage != null)
		{
			MeshGenerationContextUtils.RectangleParams rectParams2 = default(MeshGenerationContextUtils.RectangleParams);
			if (value.texture != null)
			{
				rectParams2 = MeshGenerationContextUtils.RectangleParams.MakeTextured(GUIUtility.AlignRectToDevice(currentElement.rect), new Rect(0f, 0f, 1f, 1f), value.texture, computedStyle.unityBackgroundScaleMode.value, currentElement.panel.contextType);
			}
			else if (value.vectorImage != null)
			{
				rectParams2 = MeshGenerationContextUtils.RectangleParams.MakeVectorTextured(GUIUtility.AlignRectToDevice(currentElement.rect), new Rect(0f, 0f, 1f, 1f), value.vectorImage, computedStyle.unityBackgroundScaleMode.value, currentElement.panel.contextType);
			}
			MeshGenerationContextUtils.GetVisualElementRadii(currentElement, out rectParams2.topLeftRadius, out rectParams2.bottomLeftRadius, out rectParams2.topRightRadius, out rectParams2.bottomRightRadius);
			rectParams2.leftSlice = computedStyle.unitySliceLeft.value;
			rectParams2.topSlice = computedStyle.unitySliceTop.value;
			rectParams2.rightSlice = computedStyle.unitySliceRight.value;
			rectParams2.bottomSlice = computedStyle.unitySliceBottom.value;
			if (computedStyle.unityBackgroundImageTintColor != Color.clear)
			{
				rectParams2.color = computedStyle.unityBackgroundImageTintColor.value;
			}
			DrawRectangle(rectParams2);
		}
	}

	public void DrawVisualElementBorder()
	{
		if (currentElement.layout.width >= Mathf.Epsilon && currentElement.layout.height >= Mathf.Epsilon)
		{
			ComputedStyle computedStyle = currentElement.computedStyle;
			if ((computedStyle.borderLeftColor != Color.clear && computedStyle.borderLeftWidth.value > 0f) || (computedStyle.borderTopColor != Color.clear && computedStyle.borderTopWidth.value > 0f) || (computedStyle.borderRightColor != Color.clear && computedStyle.borderRightWidth.value > 0f) || (computedStyle.borderBottomColor != Color.clear && computedStyle.borderBottomWidth.value > 0f))
			{
				MeshGenerationContextUtils.BorderParams borderParams = new MeshGenerationContextUtils.BorderParams
				{
					rect = GUIUtility.AlignRectToDevice(currentElement.rect),
					leftColor = computedStyle.borderLeftColor.value,
					topColor = computedStyle.borderTopColor.value,
					rightColor = computedStyle.borderRightColor.value,
					bottomColor = computedStyle.borderBottomColor.value,
					leftWidth = computedStyle.borderLeftWidth.value,
					topWidth = computedStyle.borderTopWidth.value,
					rightWidth = computedStyle.borderRightWidth.value,
					bottomWidth = computedStyle.borderBottomWidth.value,
					playmodeTintColor = ((currentElement.panel.contextType == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white)
				};
				MeshGenerationContextUtils.GetVisualElementRadii(currentElement, out borderParams.topLeftRadius, out borderParams.bottomLeftRadius, out borderParams.topRightRadius, out borderParams.bottomRightRadius);
				DrawBorder(borderParams);
			}
		}
	}

	public void ApplyVisualElementClipping()
	{
		if (currentElement.renderChainData.clipMethod == ClipMethod.Scissor)
		{
			RenderChainCommand renderChainCommand = m_Owner.AllocCommand();
			renderChainCommand.type = CommandType.PushScissor;
			renderChainCommand.owner = currentElement;
			m_Entries.Add(new Entry
			{
				customCommand = renderChainCommand
			});
			m_ClosingInfo.needsClosing = (m_ClosingInfo.popScissorClip = true);
		}
		else if (currentElement.renderChainData.clipMethod == ClipMethod.Stencil)
		{
			if (UIRUtility.IsVectorImageBackground(currentElement))
			{
				GenerateStencilClipEntryForSVGBackground();
			}
			else
			{
				GenerateStencilClipEntryForRoundedRectBackground();
			}
			m_StencilClip = true;
		}
		m_ClipRectID = currentElement.renderChainData.clipRectID;
	}

	public void DrawVectorImage(MeshGenerationContextUtils.RectangleParams rectParams)
	{
		VectorImage vectorImage = rectParams.vectorImage;
		Debug.Assert(vectorImage != null);
		VertexFlags vertexFlags = ((vectorImage.atlas != null) ? VertexFlags.IsSVGGradients : VertexFlags.IsSolid);
		int settingIndexOffset = 0;
		if (vectorImage.atlas != null && m_VectorImageManager != null)
		{
			GradientRemap gradientRemap = m_VectorImageManager.AddUser(vectorImage);
			vertexFlags = (gradientRemap.isAtlassed ? VertexFlags.IsSVGGradients : VertexFlags.IsCustomSVGGradients);
			settingIndexOffset = gradientRemap.destIndex;
		}
		int count = m_Entries.Count;
		MeshGenerationContext.MeshFlags flags = MeshGenerationContext.MeshFlags.None;
		switch (vertexFlags)
		{
		case VertexFlags.IsSVGGradients:
			flags = MeshGenerationContext.MeshFlags.IsSVGGradients;
			break;
		case VertexFlags.IsCustomSVGGradients:
			flags = MeshGenerationContext.MeshFlags.IsCustomSVGGradients;
			break;
		}
		MeshBuilder.AllocMeshData meshAlloc = new MeshBuilder.AllocMeshData
		{
			alloc = m_AllocThroughDrawMeshDelegate,
			texture = ((vertexFlags == VertexFlags.IsCustomSVGGradients) ? vectorImage.atlas : null),
			flags = flags
		};
		MeshBuilder.MakeVectorGraphics(rectParams, settingIndexOffset, meshAlloc, out var finalVertexCount, out var finalIndexCount);
		Debug.Assert(count <= m_Entries.Count + 1);
		if (count != m_Entries.Count)
		{
			m_SVGBackgroundEntryIndex = m_Entries.Count - 1;
			if (finalVertexCount != 0 && finalIndexCount != 0)
			{
				Entry value = m_Entries[m_SVGBackgroundEntryIndex];
				value.vertices = value.vertices.Slice(0, finalVertexCount);
				value.indices = value.indices.Slice(0, finalIndexCount);
				m_Entries[m_SVGBackgroundEntryIndex] = value;
			}
		}
	}

	internal void Reset()
	{
		if (disposed)
		{
			DisposeHelper.NotifyDisposedUsed(this);
			return;
		}
		m_Entries.Clear();
		m_VertsPool.SessionDone();
		m_IndicesPool.SessionDone();
		m_ClosingInfo = default(ClosingInfo);
		m_NextMeshWriteDataPoolItem = 0;
		currentElement = null;
		int num = (totalIndices = 0);
		totalVertices = num;
	}

	private void GenerateStencilClipEntryForRoundedRectBackground()
	{
		if (!(currentElement.layout.width <= Mathf.Epsilon) && !(currentElement.layout.height <= Mathf.Epsilon))
		{
			ComputedStyle computedStyle = currentElement.computedStyle;
			MeshGenerationContextUtils.GetVisualElementRadii(currentElement, out var topLeft, out var bottomLeft, out var topRight, out var bottomRight);
			float value = computedStyle.borderTopWidth.value;
			float value2 = computedStyle.borderLeftWidth.value;
			float value3 = computedStyle.borderBottomWidth.value;
			float value4 = computedStyle.borderRightWidth.value;
			MeshGenerationContextUtils.RectangleParams rectParams = new MeshGenerationContextUtils.RectangleParams
			{
				rect = GUIUtility.AlignRectToDevice(currentElement.rect),
				color = Color.white,
				topLeftRadius = Vector2.Max(Vector2.zero, topLeft - new Vector2(value2, value)),
				topRightRadius = Vector2.Max(Vector2.zero, topRight - new Vector2(value4, value)),
				bottomLeftRadius = Vector2.Max(Vector2.zero, bottomLeft - new Vector2(value2, value3)),
				bottomRightRadius = Vector2.Max(Vector2.zero, bottomRight - new Vector2(value4, value3)),
				playmodeTintColor = ((currentElement.panel.contextType == ContextType.Editor) ? UIElementsUtility.editorPlayModeTintColor : Color.white)
			};
			rectParams.rect.x += value2;
			rectParams.rect.y += value;
			rectParams.rect.width -= value2 + value4;
			rectParams.rect.height -= value + value3;
			if (computedStyle.unityOverflowClipBox == OverflowClipBox.ContentBox)
			{
				rectParams.rect.x += computedStyle.paddingLeft.value.value;
				rectParams.rect.y += computedStyle.paddingTop.value.value;
				rectParams.rect.width -= computedStyle.paddingLeft.value.value + computedStyle.paddingRight.value.value;
				rectParams.rect.height -= computedStyle.paddingTop.value.value + computedStyle.paddingBottom.value.value;
			}
			m_CurrentEntry.clipRectID = m_ClipRectID;
			m_CurrentEntry.isStencilClipped = m_StencilClip;
			m_CurrentEntry.isClipRegisterEntry = true;
			MeshBuilder.MakeSolidRect(rectParams, -0.995f, new MeshBuilder.AllocMeshData
			{
				alloc = m_AllocRawVertsIndicesDelegate
			});
			if (m_CurrentEntry.vertices.Length > 0 && m_CurrentEntry.indices.Length > 0)
			{
				m_Entries.Add(m_CurrentEntry);
				totalVertices += m_CurrentEntry.vertices.Length;
				totalIndices += m_CurrentEntry.indices.Length;
				m_ClosingInfo.needsClosing = true;
			}
			m_CurrentEntry = default(Entry);
		}
	}

	private void GenerateStencilClipEntryForSVGBackground()
	{
		if (m_SVGBackgroundEntryIndex != -1)
		{
			Entry entry = m_Entries[m_SVGBackgroundEntryIndex];
			Debug.Assert(entry.vertices.Length > 0);
			Debug.Assert(entry.indices.Length > 0);
			m_CurrentEntry.vertices = entry.vertices;
			m_CurrentEntry.indices = entry.indices;
			m_CurrentEntry.uvIsDisplacement = entry.uvIsDisplacement;
			m_CurrentEntry.clipRectID = m_ClipRectID;
			m_CurrentEntry.isStencilClipped = m_StencilClip;
			m_CurrentEntry.isClipRegisterEntry = true;
			m_ClosingInfo.needsClosing = true;
			int length = m_CurrentEntry.vertices.Length;
			NativeSlice<Vertex> vertices = m_VertsPool.Alloc((uint)length);
			for (int i = 0; i < length; i++)
			{
				Vertex value = m_CurrentEntry.vertices[i];
				value.position.z = -0.995f;
				vertices[i] = value;
			}
			m_CurrentEntry.vertices = vertices;
			totalVertices += m_CurrentEntry.vertices.Length;
			totalIndices += m_CurrentEntry.indices.Length;
			m_Entries.Add(m_CurrentEntry);
			m_CurrentEntry = default(Entry);
		}
	}
}
