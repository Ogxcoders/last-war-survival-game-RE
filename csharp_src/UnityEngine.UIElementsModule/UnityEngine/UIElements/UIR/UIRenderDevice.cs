#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine.Rendering;

namespace UnityEngine.UIElements.UIR;

internal class UIRenderDevice : IDisposable
{
	private struct AllocToUpdate
	{
		public uint id;

		public uint allocTime;

		public MeshHandle meshHandle;

		public Alloc permAllocVerts;

		public Alloc permAllocIndices;

		public Page permPage;

		public bool copyBackIndices;
	}

	private struct AllocToFree
	{
		public Alloc alloc;

		public Page page;

		public bool vertices;
	}

	private struct DeviceToFree
	{
		public uint handle;

		public Page page;

		public NativeArray<DrawBufferRange> drawRanges;

		public void Dispose()
		{
			while (this.page != null)
			{
				Page page = this.page;
				this.page = this.page.next;
				page.Dispose();
			}
			if (drawRanges.IsCreated)
			{
				drawRanges.Dispose();
			}
		}
	}

	public enum DrawingModes
	{
		FlipY,
		StraightY,
		DisableClipping
	}

	internal struct AllocationStatistics
	{
		public struct PageStatistics
		{
			internal HeapStatistics vertices;

			internal HeapStatistics indices;
		}

		public PageStatistics[] pages;

		public int[] freesDeferred;

		public bool completeInit;
	}

	internal struct DrawStatistics
	{
		public int currentFrameIndex;

		public int currentDrawRangeStart;

		public uint totalIndices;

		public uint commandCount;

		public uint drawCommandCount;

		public uint materialSetCount;

		public uint drawRangeCount;

		public uint drawRangeCallCount;

		public uint immediateDraws;
	}

	internal const uint k_MaxQueuedFrameCount = 4u;

	internal const int k_PruneEmptyPageFrameCount = 60;

	private readonly bool m_MockDevice;

	private int m_LazyCreationDrawRangeRingSize;

	private Shader m_DefaultMaterialShader;

	private Material m_DefaultMaterial;

	private DrawingModes m_DrawingMode;

	private Page m_FirstPage;

	private uint m_NextPageVertexCount;

	private uint m_LargeMeshVertexCount;

	private float m_IndexToVertexCountRatio;

	private List<List<AllocToFree>> m_DeferredFrees;

	private List<List<AllocToUpdate>> m_Updates;

	private uint[] m_Fences;

	private NativeArray<DrawBufferRange> m_DrawRanges;

	private int m_DrawRangeStart;

	private uint m_FrameIndex;

	private uint m_NextUpdateID = 1u;

	private DrawStatistics m_DrawStats;

	private bool m_APIUsesStraightYCoordinateSystem;

	private readonly Pool<MeshHandle> m_MeshHandles = new Pool<MeshHandle>();

	private readonly DrawParams m_DrawParams = new DrawParams();

	private static LinkedList<DeviceToFree> m_DeviceFreeQueue;

	private static int m_ActiveDeviceCount;

	private static bool m_SubscribedToNotifications;

	private static bool m_SynchronousFree;

	private static readonly int s_FontTexPropID;

	private static readonly int s_CustomTexPropID;

	private static readonly int s_1PixelClipInvViewPropID;

	private static readonly int s_GradientSettingsTexID;

	private static readonly int s_ShaderInfoTexID;

	private static readonly int s_PixelClipRectPropID;

	private static readonly int s_TransformsPropID;

	private static readonly int s_ClipRectsPropID;

	private static ProfilerMarker s_MarkerAllocate;

	private static ProfilerMarker s_MarkerFree;

	private static ProfilerMarker s_MarkerAdvanceFrame;

	private static ProfilerMarker s_MarkerFence;

	private static ProfilerMarker s_MarkerBeforeDraw;

	private static bool? s_VertexTexturingIsAvailable;

	private const string k_VertexTexturingIsAvailableTag = "UIE_VertexTexturingIsAvailable";

	private const string k_VertexTexturingIsAvailableTrue = "1";

	private static Texture2D s_DefaultShaderInfoTexFloat;

	private static Texture2D s_DefaultShaderInfoTexARGB8;

	internal uint maxVerticesPerPage { get; } = 65535u;

	internal static Texture2D defaultShaderInfoTexFloat
	{
		get
		{
			if (s_DefaultShaderInfoTexFloat == null)
			{
				s_DefaultShaderInfoTexFloat = new Texture2D(64, 64, TextureFormat.RGBAFloat, mipChain: false);
				s_DefaultShaderInfoTexFloat.hideFlags = HideFlags.HideAndDontSave;
				s_DefaultShaderInfoTexFloat.filterMode = FilterMode.Point;
				s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.identityTransformTexel.x, UIRVEShaderInfoAllocator.identityTransformTexel.y, UIRVEShaderInfoAllocator.identityTransformRow0Value);
				s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.identityTransformTexel.x, UIRVEShaderInfoAllocator.identityTransformTexel.y + 1, UIRVEShaderInfoAllocator.identityTransformRow1Value);
				s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.identityTransformTexel.x, UIRVEShaderInfoAllocator.identityTransformTexel.y + 2, UIRVEShaderInfoAllocator.identityTransformRow2Value);
				s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.infiniteClipRectTexel.x, UIRVEShaderInfoAllocator.infiniteClipRectTexel.y, UIRVEShaderInfoAllocator.infiniteClipRectValue);
				s_DefaultShaderInfoTexFloat.SetPixel(UIRVEShaderInfoAllocator.fullOpacityTexel.x, UIRVEShaderInfoAllocator.fullOpacityTexel.y, UIRVEShaderInfoAllocator.fullOpacityValue);
				s_DefaultShaderInfoTexFloat.Apply(updateMipmaps: false, makeNoLongerReadable: true);
			}
			return s_DefaultShaderInfoTexFloat;
		}
	}

	internal static Texture2D defaultShaderInfoTexARGB8
	{
		get
		{
			if (s_DefaultShaderInfoTexARGB8 == null)
			{
				s_DefaultShaderInfoTexARGB8 = new Texture2D(64, 64, TextureFormat.RGBA32, mipChain: false);
				s_DefaultShaderInfoTexARGB8.hideFlags = HideFlags.HideAndDontSave;
				s_DefaultShaderInfoTexARGB8.filterMode = FilterMode.Point;
				s_DefaultShaderInfoTexARGB8.SetPixel(UIRVEShaderInfoAllocator.fullOpacityTexel.x, UIRVEShaderInfoAllocator.fullOpacityTexel.y, UIRVEShaderInfoAllocator.fullOpacityValue);
				s_DefaultShaderInfoTexARGB8.Apply(updateMipmaps: false, makeNoLongerReadable: true);
			}
			return s_DefaultShaderInfoTexARGB8;
		}
	}

	internal static bool vertexTexturingIsAvailable
	{
		get
		{
			if (!s_VertexTexturingIsAvailable.HasValue)
			{
				Shader shader = Shader.Find(UIRUtility.k_DefaultShaderName);
				Material material = new Material(shader);
				material.hideFlags |= HideFlags.DontSaveInEditor;
				string tag = material.GetTag("UIE_VertexTexturingIsAvailable", searchFallbacks: false);
				UIRUtility.Destroy(material);
				s_VertexTexturingIsAvailable = tag == "1";
			}
			return s_VertexTexturingIsAvailable.Value;
		}
	}

	protected bool disposed { get; private set; }

	public Shader standardShader
	{
		get
		{
			return m_DefaultMaterialShader;
		}
		set
		{
			if (m_DefaultMaterialShader != value)
			{
				m_DefaultMaterialShader = value;
				UIRUtility.Destroy(m_DefaultMaterial);
				m_DefaultMaterial = null;
			}
		}
	}

	static UIRenderDevice()
	{
		m_DeviceFreeQueue = new LinkedList<DeviceToFree>();
		m_ActiveDeviceCount = 0;
		s_FontTexPropID = Shader.PropertyToID("_FontTex");
		s_CustomTexPropID = Shader.PropertyToID("_CustomTex");
		s_1PixelClipInvViewPropID = Shader.PropertyToID("_1PixelClipInvView");
		s_GradientSettingsTexID = Shader.PropertyToID("_GradientSettingsTex");
		s_ShaderInfoTexID = Shader.PropertyToID("_ShaderInfoTex");
		s_PixelClipRectPropID = Shader.PropertyToID("_PixelClipRect");
		s_TransformsPropID = Shader.PropertyToID("_Transforms");
		s_ClipRectsPropID = Shader.PropertyToID("_ClipRects");
		s_MarkerAllocate = new ProfilerMarker("UIR.Allocate");
		s_MarkerFree = new ProfilerMarker("UIR.Free");
		s_MarkerAdvanceFrame = new ProfilerMarker("UIR.AdvanceFrame");
		s_MarkerFence = new ProfilerMarker("UIR.WaitOnFence");
		s_MarkerBeforeDraw = new ProfilerMarker("UIR.BeforeDraw");
		Utility.EngineUpdate += OnEngineUpdateGlobal;
		Utility.FlushPendingResources += OnFlushPendingResources;
	}

	public UIRenderDevice(Shader defaultMaterialShader, uint initialVertexCapacity = 0u, uint initialIndexCapacity = 0u, DrawingModes drawingMode = DrawingModes.FlipY, int drawRangeRingSize = 1024)
		: this(defaultMaterialShader, initialVertexCapacity, initialIndexCapacity, drawingMode, drawRangeRingSize, mockDevice: false)
	{
	}

	protected UIRenderDevice(uint initialVertexCapacity = 0u, uint initialIndexCapacity = 0u, DrawingModes drawingMode = DrawingModes.FlipY, int drawRangeRingSize = 1024)
		: this(null, initialVertexCapacity, initialIndexCapacity, drawingMode, drawRangeRingSize, mockDevice: true)
	{
	}

	private UIRenderDevice(Shader defaultMaterialShader, uint initialVertexCapacity, uint initialIndexCapacity, DrawingModes drawingMode, int drawRangeRingSize, bool mockDevice)
	{
		m_MockDevice = mockDevice;
		Debug.Assert(!m_SynchronousFree);
		Debug.Assert(condition: true);
		if (m_ActiveDeviceCount++ == 0 && !m_SubscribedToNotifications && !m_MockDevice)
		{
			Utility.NotifyOfUIREvents(subscribe: true);
			m_SubscribedToNotifications = true;
		}
		m_DefaultMaterialShader = defaultMaterialShader;
		m_DrawingMode = drawingMode;
		m_NextPageVertexCount = Math.Max(initialVertexCapacity, 2048u);
		m_LargeMeshVertexCount = m_NextPageVertexCount;
		m_IndexToVertexCountRatio = (float)initialIndexCapacity / (float)initialVertexCapacity;
		m_IndexToVertexCountRatio = Mathf.Max(m_IndexToVertexCountRatio, 2f);
		m_LazyCreationDrawRangeRingSize = (Mathf.IsPowerOfTwo(drawRangeRingSize) ? drawRangeRingSize : Mathf.NextPowerOfTwo(drawRangeRingSize));
		m_DeferredFrees = new List<List<AllocToFree>>(4);
		m_Updates = new List<List<AllocToUpdate>>(4);
		for (int i = 0; (long)i < 4L; i++)
		{
			m_DeferredFrees.Add(new List<AllocToFree>());
			m_Updates.Add(new List<AllocToUpdate>());
		}
		m_APIUsesStraightYCoordinateSystem = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES2 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3;
	}

	private void CompleteCreation()
	{
		if (!m_DrawRanges.IsCreated)
		{
			m_DrawRanges = new NativeArray<DrawBufferRange>(m_LazyCreationDrawRangeRingSize, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			m_Fences = (m_MockDevice ? null : new uint[4]);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	internal void DisposeImmediate()
	{
		Debug.Assert(!m_SynchronousFree);
		m_SynchronousFree = true;
		Dispose();
		m_SynchronousFree = false;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposed)
		{
			return;
		}
		m_ActiveDeviceCount--;
		if (disposing)
		{
			if (m_DefaultMaterial != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(m_DefaultMaterial);
				}
				else
				{
					Object.DestroyImmediate(m_DefaultMaterial);
				}
			}
			DeviceToFree value = new DeviceToFree
			{
				handle = ((!m_MockDevice) ? Utility.InsertCPUFence() : 0u),
				page = m_FirstPage,
				drawRanges = m_DrawRanges
			};
			if (value.handle == 0)
			{
				value.Dispose();
			}
			else
			{
				m_DeviceFreeQueue.AddLast(value);
				if (m_SynchronousFree)
				{
					ProcessDeviceFreeQueue();
				}
			}
		}
		disposed = true;
	}

	public MeshHandle Allocate(uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, out ushort indexOffset)
	{
		MeshHandle meshHandle = m_MeshHandles.Get();
		meshHandle.triangleCount = indexCount / 3;
		Allocate(meshHandle, vertexCount, indexCount, out vertexData, out indexData, shortLived: false);
		indexOffset = (ushort)meshHandle.allocVerts.start;
		return meshHandle;
	}

	public void Update(MeshHandle mesh, uint vertexCount, out NativeSlice<Vertex> vertexData)
	{
		Debug.Assert(mesh.allocVerts.size >= vertexCount);
		if (mesh.allocTime == m_FrameIndex)
		{
			vertexData = mesh.allocPage.vertices.cpuData.Slice((int)mesh.allocVerts.start, (int)vertexCount);
			return;
		}
		uint start = mesh.allocVerts.start;
		NativeSlice<ushort> nativeSlice = new NativeSlice<ushort>(mesh.allocPage.indices.cpuData, (int)mesh.allocIndices.start, (int)mesh.allocIndices.size);
		UpdateAfterGPUUsedData(mesh, vertexCount, mesh.allocIndices.size, out vertexData, out var indexData, out var indexOffset, out var _, copyBackIndices: false);
		int size = (int)mesh.allocIndices.size;
		int num = (int)(indexOffset - start);
		for (int i = 0; i < size; i++)
		{
			indexData[i] = (ushort)(nativeSlice[i] + num);
		}
	}

	public void Update(MeshHandle mesh, uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, out ushort indexOffset)
	{
		Debug.Assert(mesh.allocVerts.size >= vertexCount);
		Debug.Assert(mesh.allocIndices.size >= indexCount);
		if (mesh.allocTime == m_FrameIndex)
		{
			vertexData = mesh.allocPage.vertices.cpuData.Slice((int)mesh.allocVerts.start, (int)vertexCount);
			indexData = mesh.allocPage.indices.cpuData.Slice((int)mesh.allocIndices.start, (int)indexCount);
			indexOffset = (ushort)mesh.allocVerts.start;
		}
		else
		{
			UpdateAfterGPUUsedData(mesh, vertexCount, indexCount, out vertexData, out indexData, out indexOffset, out var _, copyBackIndices: true);
		}
	}

	private bool TryAllocFromPage(Page page, uint vertexCount, uint indexCount, ref Alloc va, ref Alloc ia, bool shortLived)
	{
		va = page.vertices.allocator.Allocate(vertexCount, shortLived);
		if (va.size != 0)
		{
			ia = page.indices.allocator.Allocate(indexCount, shortLived);
			if (ia.size != 0)
			{
				return true;
			}
			page.vertices.allocator.Free(va);
			va.size = 0u;
		}
		return false;
	}

	private void Allocate(MeshHandle meshHandle, uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, bool shortLived)
	{
		Page page = null;
		Alloc va = default(Alloc);
		Alloc ia = default(Alloc);
		if (vertexCount <= m_LargeMeshVertexCount)
		{
			if (m_FirstPage != null)
			{
				page = m_FirstPage;
				while (!TryAllocFromPage(page, vertexCount, indexCount, ref va, ref ia, shortLived) && page.next != null)
				{
					page = page.next;
				}
			}
			else
			{
				CompleteCreation();
			}
			if (ia.size == 0)
			{
				m_NextPageVertexCount <<= 1;
				m_NextPageVertexCount = Math.Max(m_NextPageVertexCount, vertexCount * 2);
				m_NextPageVertexCount = Math.Min(m_NextPageVertexCount, maxVerticesPerPage);
				uint val = (uint)((float)m_NextPageVertexCount * m_IndexToVertexCountRatio + 0.5f);
				val = Math.Max(val, indexCount * 2);
				Debug.Assert(page?.next == null);
				page = new Page(m_NextPageVertexCount, val, 4u, m_MockDevice);
				page.next = m_FirstPage;
				m_FirstPage = page;
				va = page.vertices.allocator.Allocate(vertexCount, shortLived);
				ia = page.indices.allocator.Allocate(indexCount, shortLived);
				Debug.Assert(va.size != 0);
				Debug.Assert(ia.size != 0);
			}
		}
		else
		{
			CompleteCreation();
			Page page2 = m_FirstPage;
			Page page3 = m_FirstPage;
			int num = int.MaxValue;
			while (page2 != null)
			{
				int num2 = page2.vertices.cpuData.Length - (int)vertexCount;
				int num3 = page2.indices.cpuData.Length - (int)indexCount;
				if (page2.isEmpty && num2 >= 0 && num3 >= 0 && num2 < num)
				{
					page = page2;
					num = num2;
				}
				page3 = page2;
				page2 = page2.next;
			}
			if (page == null)
			{
				uint vertexMaxCount = ((vertexCount > maxVerticesPerPage) ? 2u : vertexCount);
				Debug.Assert(vertexCount <= maxVerticesPerPage, $"Requested Vertex count ({vertexCount}) is above the limit ({maxVerticesPerPage}). Alloc will fail.");
				page = new Page(vertexMaxCount, indexCount, 4u, m_MockDevice);
				if (page3 != null)
				{
					page3.next = page;
				}
				else
				{
					m_FirstPage = page;
				}
			}
			va = page.vertices.allocator.Allocate(vertexCount, shortLived);
			ia = page.indices.allocator.Allocate(indexCount, shortLived);
		}
		Debug.Assert(va.size == vertexCount, $"Vertices allocated ({va.size}) != Vertices requested ({vertexCount})");
		Debug.Assert(ia.size == indexCount, $"Indices allocated ({ia.size}) != Indices requested ({indexCount})");
		if (va.size != vertexCount || ia.size != indexCount)
		{
			if (va.handle != null)
			{
				page.vertices.allocator.Free(va);
			}
			if (ia.handle != null)
			{
				page.vertices.allocator.Free(ia);
			}
			ia = default(Alloc);
			va = default(Alloc);
		}
		page.vertices.RegisterUpdate(va.start, va.size);
		page.indices.RegisterUpdate(ia.start, ia.size);
		vertexData = new NativeSlice<Vertex>(page.vertices.cpuData, (int)va.start, (int)va.size);
		indexData = new NativeSlice<ushort>(page.indices.cpuData, (int)ia.start, (int)ia.size);
		meshHandle.allocPage = page;
		meshHandle.allocVerts = va;
		meshHandle.allocIndices = ia;
		meshHandle.allocTime = m_FrameIndex;
	}

	private void UpdateAfterGPUUsedData(MeshHandle mesh, uint vertexCount, uint indexCount, out NativeSlice<Vertex> vertexData, out NativeSlice<ushort> indexData, out ushort indexOffset, out AllocToUpdate allocToUpdate, bool copyBackIndices)
	{
		allocToUpdate = new AllocToUpdate
		{
			id = m_NextUpdateID++,
			allocTime = m_FrameIndex,
			meshHandle = mesh,
			copyBackIndices = copyBackIndices
		};
		Debug.Assert(m_NextUpdateID != 0);
		if (mesh.updateAllocID == 0)
		{
			allocToUpdate.permAllocVerts = mesh.allocVerts;
			allocToUpdate.permAllocIndices = mesh.allocIndices;
			allocToUpdate.permPage = mesh.allocPage;
		}
		else
		{
			int index = (int)(mesh.updateAllocID - 1);
			List<AllocToUpdate> list = m_Updates[(int)mesh.allocTime % m_Updates.Count];
			AllocToUpdate value = list[index];
			Debug.Assert(value.id == mesh.updateAllocID);
			allocToUpdate.copyBackIndices |= value.copyBackIndices;
			allocToUpdate.permAllocVerts = value.permAllocVerts;
			allocToUpdate.permAllocIndices = value.permAllocIndices;
			allocToUpdate.permPage = value.permPage;
			value.allocTime = uint.MaxValue;
			list[index] = value;
			List<AllocToFree> list2 = m_DeferredFrees[(int)(m_FrameIndex % (uint)m_DeferredFrees.Count)];
			list2.Add(new AllocToFree
			{
				alloc = mesh.allocVerts,
				page = mesh.allocPage,
				vertices = true
			});
			list2.Add(new AllocToFree
			{
				alloc = mesh.allocIndices,
				page = mesh.allocPage,
				vertices = false
			});
		}
		if (TryAllocFromPage(mesh.allocPage, vertexCount, indexCount, ref mesh.allocVerts, ref mesh.allocIndices, shortLived: true))
		{
			mesh.allocPage.vertices.RegisterUpdate(mesh.allocVerts.start, mesh.allocVerts.size);
			mesh.allocPage.indices.RegisterUpdate(mesh.allocIndices.start, mesh.allocIndices.size);
		}
		else
		{
			Allocate(mesh, vertexCount, indexCount, out vertexData, out indexData, shortLived: true);
		}
		mesh.triangleCount = indexCount / 3;
		mesh.updateAllocID = allocToUpdate.id;
		mesh.allocTime = allocToUpdate.allocTime;
		m_Updates[(int)(m_FrameIndex % m_Updates.Count)].Add(allocToUpdate);
		vertexData = new NativeSlice<Vertex>(mesh.allocPage.vertices.cpuData, (int)mesh.allocVerts.start, (int)vertexCount);
		indexData = new NativeSlice<ushort>(mesh.allocPage.indices.cpuData, (int)mesh.allocIndices.start, (int)indexCount);
		indexOffset = (ushort)mesh.allocVerts.start;
	}

	public void Free(MeshHandle mesh)
	{
		if (mesh.updateAllocID != 0)
		{
			int index = (int)(mesh.updateAllocID - 1);
			List<AllocToUpdate> list = m_Updates[(int)mesh.allocTime % m_Updates.Count];
			AllocToUpdate value = list[index];
			Debug.Assert(value.id == mesh.updateAllocID);
			List<AllocToFree> list2 = m_DeferredFrees[(int)(m_FrameIndex % (uint)m_DeferredFrees.Count)];
			list2.Add(new AllocToFree
			{
				alloc = value.permAllocVerts,
				page = value.permPage,
				vertices = true
			});
			list2.Add(new AllocToFree
			{
				alloc = value.permAllocIndices,
				page = value.permPage,
				vertices = false
			});
			list2.Add(new AllocToFree
			{
				alloc = mesh.allocVerts,
				page = mesh.allocPage,
				vertices = true
			});
			list2.Add(new AllocToFree
			{
				alloc = mesh.allocIndices,
				page = mesh.allocPage,
				vertices = false
			});
			value.allocTime = uint.MaxValue;
			list[index] = value;
		}
		else if (mesh.allocTime != m_FrameIndex)
		{
			int index2 = (int)(m_FrameIndex % (uint)m_DeferredFrees.Count);
			m_DeferredFrees[index2].Add(new AllocToFree
			{
				alloc = mesh.allocVerts,
				page = mesh.allocPage,
				vertices = true
			});
			m_DeferredFrees[index2].Add(new AllocToFree
			{
				alloc = mesh.allocIndices,
				page = mesh.allocPage,
				vertices = false
			});
		}
		else
		{
			mesh.allocPage.vertices.allocator.Free(mesh.allocVerts);
			mesh.allocPage.indices.allocator.Free(mesh.allocIndices);
		}
		mesh.allocVerts = default(Alloc);
		mesh.allocIndices = default(Alloc);
		mesh.allocPage = null;
		mesh.updateAllocID = 0u;
		m_MeshHandles.Return(mesh);
	}

	public Material GetStandardMaterial()
	{
		if (m_DefaultMaterial == null && m_DefaultMaterialShader != null)
		{
			m_DefaultMaterial = new Material(m_DefaultMaterialShader);
			SetupStandardMaterial(m_DefaultMaterial, m_DrawingMode);
		}
		return m_DefaultMaterial;
	}

	private static void SetupStandardMaterial(Material material, DrawingModes mode)
	{
		material.hideFlags |= HideFlags.DontSaveInEditor;
		switch (mode)
		{
		case DrawingModes.StraightY:
			material.SetInt("_StencilCompFront", 3);
			material.SetInt("_StencilPassFront", 0);
			material.SetInt("_StencilZFailFront", 1);
			material.SetInt("_StencilFailFront", 0);
			material.SetInt("_StencilCompBack", 8);
			material.SetInt("_StencilPassBack", 0);
			material.SetInt("_StencilZFailBack", 2);
			material.SetInt("_StencilFailBack", 0);
			break;
		case DrawingModes.FlipY:
			material.SetInt("_StencilCompFront", 8);
			material.SetInt("_StencilPassFront", 0);
			material.SetInt("_StencilZFailFront", 2);
			material.SetInt("_StencilFailFront", 0);
			material.SetInt("_StencilCompBack", 3);
			material.SetInt("_StencilPassBack", 0);
			material.SetInt("_StencilZFailBack", 1);
			material.SetInt("_StencilFailBack", 0);
			break;
		case DrawingModes.DisableClipping:
			material.SetInt("_StencilCompFront", 8);
			material.SetInt("_StencilPassFront", 0);
			material.SetInt("_StencilZFailFront", 0);
			material.SetInt("_StencilFailFront", 0);
			material.SetInt("_StencilCompBack", 8);
			material.SetInt("_StencilPassBack", 0);
			material.SetInt("_StencilZFailBack", 0);
			material.SetInt("_StencilFailBack", 0);
			break;
		}
	}

	private static void Set1PixelSizeOnMaterial(DrawParams drawParams, Material mat)
	{
		Vector4 value = default(Vector4);
		RectInt activeViewport = Utility.GetActiveViewport();
		value.x = 2f / (float)activeViewport.width;
		value.y = 2f / (float)activeViewport.height;
		Vector3 vector = (drawParams.projection * drawParams.view.Peek().transform).inverse.MultiplyVector(new Vector3(value.x, value.y));
		value.z = 1f / (Mathf.Abs(vector.x) + Mathf.Epsilon);
		value.w = 1f / (Mathf.Abs(vector.y) + Mathf.Epsilon);
		mat.SetVector(s_1PixelClipInvViewPropID, value);
	}

	private void BeforeDraw()
	{
		AdvanceFrame();
		m_DrawStats = default(DrawStatistics);
		m_DrawStats.currentFrameIndex = (int)m_FrameIndex;
		m_DrawStats.currentDrawRangeStart = m_DrawRangeStart;
		for (Page page = m_FirstPage; page != null; page = page.next)
		{
			page.vertices.SendUpdates();
			page.indices.SendUpdates();
		}
	}

	private void EvaluateChain(RenderChainCommand head, Rect viewport, Matrix4x4 projection, PanelClearFlags clearFlags, Texture atlas, Texture gradientSettings, Texture shaderInfo, float pixelsPerPoint, NativeArray<Transform3x4> transforms, NativeArray<Vector4> clipRects, ref Exception immediateException)
	{
		bool flag = m_APIUsesStraightYCoordinateSystem;
		if (Utility.GetInvertProjectionMatrix())
		{
			flag = !flag;
		}
		DrawParams drawParams = m_DrawParams;
		drawParams.Reset(viewport, projection);
		Material material = null;
		if (!m_MockDevice)
		{
			material = GetStandardMaterial();
			material.mainTexture = atlas;
			material.SetTexture(s_GradientSettingsTexID, gradientSettings);
			material.SetTexture(s_ShaderInfoTexID, shaderInfo);
			if (transforms.Length > 0)
			{
				Utility.SetVectorArray<Transform3x4>(material, s_TransformsPropID, transforms);
			}
			if (clipRects.Length > 0)
			{
				Utility.SetVectorArray<Vector4>(material, s_ClipRectsPropID, clipRects);
			}
			Set1PixelSizeOnMaterial(drawParams, material);
			material.SetVector(s_PixelClipRectPropID, drawParams.view.Peek().clipRect);
			if (clearFlags != PanelClearFlags.None)
			{
				GL.Clear((clearFlags & PanelClearFlags.Depth) != 0, (clearFlags & PanelClearFlags.Color) != 0, Color.clear, 0.99f);
			}
			GL.modelview = drawParams.view.Peek().transform;
			GL.LoadProjectionMatrix(drawParams.projection);
		}
		NativeArray<DrawBufferRange> drawRanges = m_DrawRanges;
		int length = drawRanges.Length;
		int num = drawRanges.Length - 1;
		int rangesStart = m_DrawRangeStart;
		int rangesReady = 0;
		DrawBufferRange value = default(DrawBufferRange);
		Page page = null;
		State state = new State
		{
			material = m_DefaultMaterial
		};
		int num2 = -1;
		int num3 = 0;
		while (head != null)
		{
			m_DrawStats.commandCount++;
			m_DrawStats.drawCommandCount += ((head.type == CommandType.Draw) ? 1u : 0u);
			bool flag2 = head.type != CommandType.Draw;
			bool flag3 = true;
			bool flag4 = false;
			if (!flag2)
			{
				flag4 = head.state.material != state.material;
				state.material = head.state.material;
				if (head.state.custom != null)
				{
					flag4 |= head.state.custom != state.custom;
					state.custom = head.state.custom;
				}
				if (head.state.font != null)
				{
					flag4 |= head.state.font != state.font;
					state.font = head.state.font;
				}
				flag2 = flag4 || head.mesh.allocPage != page;
				if (!flag2)
				{
					flag3 = num2 != head.mesh.allocIndices.start + head.indexOffset;
				}
			}
			if (flag3)
			{
				if (value.indexCount > 0)
				{
					int index = (rangesStart + rangesReady++) & num;
					drawRanges[index] = value;
					if (rangesReady == length)
					{
						KickRanges(drawRanges, ref rangesReady, ref rangesStart, length, page);
					}
					value = default(DrawBufferRange);
					m_DrawStats.drawRangeCount++;
				}
				if (head.type == CommandType.Draw)
				{
					value.firstIndex = (int)head.mesh.allocIndices.start + head.indexOffset;
					value.indexCount = head.indexCount;
					value.vertsReferenced = (int)(head.mesh.allocVerts.start + head.mesh.allocVerts.size);
					value.minIndexVal = (int)head.mesh.allocVerts.start;
					num2 = value.firstIndex + head.indexCount;
					num3 = value.vertsReferenced + value.minIndexVal;
					m_DrawStats.totalIndices += (uint)head.indexCount;
				}
				if (flag2)
				{
					KickRanges(drawRanges, ref rangesReady, ref rangesStart, length, page);
					if (head.type != CommandType.Draw)
					{
						if (!m_MockDevice)
						{
							head.ExecuteNonDrawMesh(drawParams, flag, pixelsPerPoint, ref immediateException);
						}
						if (head.type == CommandType.Immediate)
						{
							state.material = m_DefaultMaterial;
							m_DrawStats.immediateDraws++;
						}
					}
					else
					{
						page = head.mesh.allocPage;
					}
					if (flag4)
					{
						if (!m_MockDevice)
						{
							Material material2 = ((state.material != null) ? state.material : material);
							if (material2 != material)
							{
								material2.mainTexture = atlas;
								material2.SetTexture(s_GradientSettingsTexID, gradientSettings);
								material2.SetTexture(s_ShaderInfoTexID, shaderInfo);
								if (transforms.Length > 0)
								{
									Utility.SetVectorArray<Transform3x4>(material2, s_TransformsPropID, transforms);
								}
								if (clipRects.Length > 0)
								{
									Utility.SetVectorArray<Vector4>(material2, s_ClipRectsPropID, clipRects);
								}
								Set1PixelSizeOnMaterial(drawParams, material2);
								material2.SetVector(s_PixelClipRectPropID, drawParams.view.Peek().clipRect);
							}
							else if (head.type == CommandType.PushView || head.type == CommandType.PopView)
							{
								Set1PixelSizeOnMaterial(drawParams, material2);
								material2.SetVector(s_PixelClipRectPropID, drawParams.view.Peek().clipRect);
							}
							material2.SetTexture(s_CustomTexPropID, state.custom);
							material2.SetTexture(s_FontTexPropID, state.font);
							material2.SetPass(0);
						}
						m_DrawStats.materialSetCount++;
					}
					else if (head.type == CommandType.PushView || head.type == CommandType.PopView)
					{
						Material material3 = ((state.material != null) ? state.material : material);
						if (!m_MockDevice)
						{
							Set1PixelSizeOnMaterial(drawParams, material3);
							material3.SetVector(s_PixelClipRectPropID, drawParams.view.Peek().clipRect);
							material3.SetPass(0);
						}
						m_DrawStats.materialSetCount++;
					}
				}
				head = head.next;
			}
			else
			{
				if (value.indexCount == 0)
				{
					num2 = (value.firstIndex = (int)head.mesh.allocIndices.start + head.indexOffset);
				}
				num3 = Math.Max(num3, (int)(head.mesh.allocVerts.size + head.mesh.allocVerts.start));
				value.indexCount += head.indexCount;
				value.minIndexVal = Math.Min(value.minIndexVal, (int)head.mesh.allocVerts.start);
				value.vertsReferenced = num3 - value.minIndexVal;
				num2 += head.indexCount;
				m_DrawStats.totalIndices += (uint)head.indexCount;
				head = head.next;
			}
		}
		if (value.indexCount > 0)
		{
			int index2 = (rangesStart + rangesReady++) & num;
			drawRanges[index2] = value;
		}
		if (rangesReady > 0)
		{
			KickRanges(drawRanges, ref rangesReady, ref rangesStart, length, page);
		}
		m_DrawRangeStart = rangesStart;
	}

	private void KickRanges(NativeArray<DrawBufferRange> ranges, ref int rangesReady, ref int rangesStart, int rangesCount, Page curPage)
	{
		if (rangesReady <= 0)
		{
			return;
		}
		if (rangesStart + rangesReady <= rangesCount)
		{
			if (!m_MockDevice)
			{
				Utility.DrawRanges(curPage.indices.gpuData, curPage.vertices.gpuData, new NativeSlice<DrawBufferRange>(ranges, rangesStart, rangesReady));
			}
			m_DrawStats.drawRangeCallCount++;
		}
		else
		{
			int num = ranges.Length - rangesStart;
			int length = rangesReady - num;
			if (!m_MockDevice)
			{
				Utility.DrawRanges(curPage.indices.gpuData, curPage.vertices.gpuData, new NativeSlice<DrawBufferRange>(ranges, rangesStart, num));
				Utility.DrawRanges(curPage.indices.gpuData, curPage.vertices.gpuData, new NativeSlice<DrawBufferRange>(ranges, 0, length));
			}
			m_DrawStats.drawRangeCallCount += 2u;
		}
		rangesStart = (rangesStart + rangesReady) & (rangesCount - 1);
		rangesReady = 0;
	}

	public void DrawChain(RenderChainCommand head, Rect viewport, Matrix4x4 projection, PanelClearFlags clearFlags, Texture atlas, Texture gradientSettings, Texture shaderInfo, float pixelsPerPoint, NativeArray<Transform3x4> transforms, NativeArray<Vector4> clipRects, ref Exception immediateException)
	{
		if (head != null)
		{
			BeforeDraw();
			Utility.ProfileDrawChainBegin();
			EvaluateChain(head, viewport, projection, clearFlags, atlas, gradientSettings, shaderInfo, pixelsPerPoint, transforms, clipRects, ref immediateException);
			Utility.ProfileDrawChainEnd();
			if (m_Fences != null)
			{
				m_Fences[(int)(m_FrameIndex % m_Fences.Length)] = Utility.InsertCPUFence();
			}
		}
	}

	internal void WaitOnAllCpuFences()
	{
		for (int i = 0; i < m_Fences.Length; i++)
		{
			WaitOnCpuFence(m_Fences[i]);
		}
	}

	private void WaitOnCpuFence(uint fence)
	{
		if (fence != 0 && !Utility.CPUFencePassed(fence))
		{
			Utility.WaitForCPUFencePassed(fence);
		}
	}

	public void AdvanceFrame()
	{
		m_FrameIndex++;
		m_DrawStats.currentFrameIndex = (int)m_FrameIndex;
		if (m_Fences != null)
		{
			int num = (int)(m_FrameIndex % m_Fences.Length);
			uint fence = m_Fences[num];
			WaitOnCpuFence(fence);
			m_Fences[num] = 0u;
		}
		m_NextUpdateID = 1u;
		List<AllocToFree> list = m_DeferredFrees[(int)(m_FrameIndex % (uint)m_DeferredFrees.Count)];
		foreach (AllocToFree item in list)
		{
			if (item.vertices)
			{
				item.page.vertices.allocator.Free(item.alloc);
			}
			else
			{
				item.page.indices.allocator.Free(item.alloc);
			}
		}
		list.Clear();
		List<AllocToUpdate> list2 = m_Updates[(int)(m_FrameIndex % (uint)m_DeferredFrees.Count)];
		foreach (AllocToUpdate item2 in list2)
		{
			if (item2.meshHandle.updateAllocID != item2.id || item2.meshHandle.allocTime != item2.allocTime)
			{
				continue;
			}
			NativeSlice<Vertex> slice = new NativeSlice<Vertex>(item2.meshHandle.allocPage.vertices.cpuData, (int)item2.meshHandle.allocVerts.start, (int)item2.meshHandle.allocVerts.size);
			new NativeSlice<Vertex>(item2.permPage.vertices.cpuData, (int)item2.permAllocVerts.start, (int)item2.meshHandle.allocVerts.size).CopyFrom(slice);
			item2.permPage.vertices.RegisterUpdate(item2.permAllocVerts.start, item2.meshHandle.allocVerts.size);
			if (item2.copyBackIndices)
			{
				NativeSlice<ushort> nativeSlice = new NativeSlice<ushort>(item2.meshHandle.allocPage.indices.cpuData, (int)item2.meshHandle.allocIndices.start, (int)item2.meshHandle.allocIndices.size);
				NativeSlice<ushort> nativeSlice2 = new NativeSlice<ushort>(item2.permPage.indices.cpuData, (int)item2.permAllocIndices.start, (int)item2.meshHandle.allocIndices.size);
				int length = nativeSlice2.Length;
				int num2 = (int)(item2.permAllocVerts.start - item2.meshHandle.allocVerts.start);
				for (int i = 0; i < length; i++)
				{
					nativeSlice2[i] = (ushort)(nativeSlice[i] + num2);
				}
				item2.permPage.indices.RegisterUpdate(item2.permAllocIndices.start, item2.meshHandle.allocIndices.size);
			}
			list.Add(new AllocToFree
			{
				alloc = item2.meshHandle.allocVerts,
				page = item2.meshHandle.allocPage,
				vertices = true
			});
			list.Add(new AllocToFree
			{
				alloc = item2.meshHandle.allocIndices,
				page = item2.meshHandle.allocPage,
				vertices = false
			});
			item2.meshHandle.allocVerts = item2.permAllocVerts;
			item2.meshHandle.allocIndices = item2.permAllocIndices;
			item2.meshHandle.allocPage = item2.permPage;
			item2.meshHandle.updateAllocID = 0u;
		}
		list2.Clear();
		PruneUnusedPages();
	}

	private void PruneUnusedPages()
	{
		Page page2;
		Page page3;
		Page page4;
		Page page = (page2 = (page3 = (page4 = null)));
		Page page5 = m_FirstPage;
		while (page5 != null)
		{
			if (!page5.isEmpty)
			{
				page5.framesEmpty = 0;
			}
			else
			{
				page5.framesEmpty++;
			}
			if (page5.framesEmpty < 60)
			{
				if (page != null)
				{
					page2.next = page5;
				}
				else
				{
					page = page5;
				}
				page2 = page5;
			}
			else
			{
				if (page3 != null)
				{
					page4.next = page5;
				}
				else
				{
					page3 = page5;
				}
				page4 = page5;
			}
			Page next = page5.next;
			page5.next = null;
			page5 = next;
		}
		m_FirstPage = page;
		page5 = page3;
		while (page5 != null)
		{
			Page next2 = page5.next;
			page5.next = null;
			page5.Dispose();
			page5 = next2;
		}
	}

	internal static void PrepareForGfxDeviceRecreate()
	{
		m_ActiveDeviceCount++;
		if (s_DefaultShaderInfoTexFloat != null)
		{
			UIRUtility.Destroy(s_DefaultShaderInfoTexFloat);
			s_DefaultShaderInfoTexFloat = null;
		}
		if (s_DefaultShaderInfoTexARGB8 != null)
		{
			UIRUtility.Destroy(s_DefaultShaderInfoTexARGB8);
			s_DefaultShaderInfoTexARGB8 = null;
		}
	}

	internal static void WrapUpGfxDeviceRecreate()
	{
		m_ActiveDeviceCount--;
	}

	internal static void FlushAllPendingDeviceDisposes()
	{
		Utility.SyncRenderThread();
		ProcessDeviceFreeQueue();
	}

	internal AllocationStatistics GatherAllocationStatistics()
	{
		AllocationStatistics result = new AllocationStatistics
		{
			completeInit = m_DrawRanges.IsCreated,
			freesDeferred = new int[m_DeferredFrees.Count]
		};
		for (int i = 0; i < m_DeferredFrees.Count; i++)
		{
			result.freesDeferred[i] = m_DeferredFrees[i].Count;
		}
		int num = 0;
		for (Page page = m_FirstPage; page != null; page = page.next)
		{
			num++;
		}
		result.pages = new AllocationStatistics.PageStatistics[num];
		num = 0;
		for (Page page = m_FirstPage; page != null; page = page.next)
		{
			result.pages[num].vertices = page.vertices.allocator.GatherStatistics();
			result.pages[num].indices = page.indices.allocator.GatherStatistics();
			num++;
		}
		return result;
	}

	internal DrawStatistics GatherDrawStatistics()
	{
		return m_DrawStats;
	}

	private static void ProcessDeviceFreeQueue()
	{
		if (m_SynchronousFree)
		{
			Utility.SyncRenderThread();
		}
		LinkedListNode<DeviceToFree> first = m_DeviceFreeQueue.First;
		while (first != null && Utility.CPUFencePassed(first.Value.handle))
		{
			first.Value.Dispose();
			m_DeviceFreeQueue.RemoveFirst();
			first = m_DeviceFreeQueue.First;
		}
		Debug.Assert(!m_SynchronousFree || m_DeviceFreeQueue.Count == 0);
		if (m_ActiveDeviceCount == 0 && m_SubscribedToNotifications)
		{
			if (s_DefaultShaderInfoTexFloat != null)
			{
				UIRUtility.Destroy(s_DefaultShaderInfoTexFloat);
				s_DefaultShaderInfoTexFloat = null;
			}
			if (s_DefaultShaderInfoTexARGB8 != null)
			{
				UIRUtility.Destroy(s_DefaultShaderInfoTexARGB8);
				s_DefaultShaderInfoTexARGB8 = null;
			}
			Utility.NotifyOfUIREvents(subscribe: false);
			m_SubscribedToNotifications = false;
		}
	}

	private static void OnEngineUpdateGlobal()
	{
		ProcessDeviceFreeQueue();
	}

	private static void OnFlushPendingResources()
	{
		m_SynchronousFree = true;
		ProcessDeviceFreeQueue();
	}
}
