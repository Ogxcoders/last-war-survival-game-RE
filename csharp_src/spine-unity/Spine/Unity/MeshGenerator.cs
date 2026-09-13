using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[Serializable]
public class MeshGenerator
{
	[Serializable]
	public struct Settings
	{
		public bool useClipping;

		[Space]
		[Range(-0.1f, 0f)]
		public float zSpacing;

		[Space]
		[Header("Vertex Data")]
		public bool pmaVertexColors;

		public bool tintBlack;

		[Tooltip("Enable when using Additive blend mode at SkeletonGraphic under a CanvasGroup. When enabled, Additive alpha value is stored at uv2.g instead of color.a to capture CanvasGroup modifying color.a.")]
		public bool canvasGroupTintBlack;

		public bool calculateTangents;

		public bool addNormals;

		public bool immutableTriangles;

		public static Settings Default => new Settings
		{
			pmaVertexColors = true,
			zSpacing = 0f,
			useClipping = true,
			tintBlack = false,
			calculateTangents = false,
			addNormals = false,
			immutableTriangles = false
		};
	}

	public Settings settings = Settings.Default;

	private const float BoundsMinDefault = float.PositiveInfinity;

	private const float BoundsMaxDefault = float.NegativeInfinity;

	[NonSerialized]
	protected readonly ExposedList<Vector3> vertexBuffer = new ExposedList<Vector3>(4);

	[NonSerialized]
	protected readonly ExposedList<Vector2> uvBuffer = new ExposedList<Vector2>(4);

	[NonSerialized]
	protected readonly ExposedList<Color32> colorBuffer = new ExposedList<Color32>(4);

	[NonSerialized]
	protected readonly ExposedList<ExposedList<int>> submeshes = new ExposedList<ExposedList<int>>
	{
		new ExposedList<int>(6)
	};

	[NonSerialized]
	private Vector2 meshBoundsMin;

	[NonSerialized]
	private Vector2 meshBoundsMax;

	[NonSerialized]
	private float meshBoundsThickness;

	[NonSerialized]
	private int submeshIndex;

	[NonSerialized]
	private SkeletonClipping clipper = new SkeletonClipping();

	[NonSerialized]
	private float[] tempVerts = new float[8];

	[NonSerialized]
	private int[] regionTriangles = new int[6] { 0, 1, 2, 2, 3, 0 };

	[NonSerialized]
	private Vector3[] normals;

	[NonSerialized]
	private Vector4[] tangents;

	[NonSerialized]
	private Vector2[] tempTanBuffer;

	[NonSerialized]
	private ExposedList<Vector2> uv2;

	[NonSerialized]
	private ExposedList<Vector2> uv3;

	private static List<Vector3> AttachmentVerts = new List<Vector3>();

	private static List<Vector2> AttachmentUVs = new List<Vector2>();

	private static List<Color32> AttachmentColors32 = new List<Color32>();

	private static List<int> AttachmentIndices = new List<int>();

	public int VertexCount => vertexBuffer.Count;

	public MeshGeneratorBuffers Buffers => new MeshGeneratorBuffers
	{
		vertexCount = VertexCount,
		vertexBuffer = vertexBuffer.Items,
		uvBuffer = uvBuffer.Items,
		colorBuffer = colorBuffer.Items,
		meshGenerator = this
	};

	public int SubmeshIndexCount(int submeshIndex)
	{
		return submeshes.Items[submeshIndex].Count;
	}

	public MeshGenerator()
	{
		submeshes.TrimExcess();
	}

	public static void GenerateSingleSubmeshInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Material material)
	{
		ExposedList<Slot> drawOrder = skeleton.DrawOrder;
		int count = drawOrder.Count;
		instructionOutput.Clear();
		ExposedList<SubmeshInstruction> submeshInstructions = instructionOutput.submeshInstructions;
		instructionOutput.attachments.Resize(count);
		Attachment[] items = instructionOutput.attachments.Items;
		int num = 0;
		SubmeshInstruction submeshInstruction = new SubmeshInstruction
		{
			skeleton = skeleton,
			preActiveClippingSlotSource = -1,
			startSlot = 0,
			rawFirstVertexIndex = 0,
			material = material,
			forceSeparate = false,
			endSlot = count
		};
		object obj = null;
		bool hasActiveClipping = false;
		Slot[] items2 = drawOrder.Items;
		for (int i = 0; i < count; i++)
		{
			Slot slot = items2[i];
			if (!slot.Bone.Active)
			{
				items[i] = null;
				continue;
			}
			if (slot.Data.BlendMode == BlendMode.Additive)
			{
				submeshInstruction.hasPMAAdditiveSlot = true;
			}
			Attachment attachment = (items[i] = slot.Attachment);
			int num2;
			int num3;
			if (attachment is RegionAttachment regionAttachment)
			{
				obj = regionAttachment.RendererObject;
				num2 = 4;
				num3 = 6;
			}
			else if (attachment is MeshAttachment meshAttachment)
			{
				obj = meshAttachment.RendererObject;
				num2 = meshAttachment.WorldVerticesLength >> 1;
				num3 = meshAttachment.Triangles.Length;
			}
			else
			{
				if (attachment is ClippingAttachment)
				{
					submeshInstruction.hasClipping = true;
					hasActiveClipping = true;
				}
				num2 = 0;
				num3 = 0;
			}
			submeshInstruction.rawTriangleCount += num3;
			submeshInstruction.rawVertexCount += num2;
			num += num2;
		}
		if (material == null && obj != null)
		{
			submeshInstruction.material = (Material)((AtlasRegion)obj).page.rendererObject;
		}
		instructionOutput.hasActiveClipping = hasActiveClipping;
		instructionOutput.rawVertexCount = num;
		if (num > 0)
		{
			submeshInstructions.Resize(1);
			submeshInstructions.Items[0] = submeshInstruction;
		}
		else
		{
			submeshInstructions.Resize(0);
		}
	}

	public static bool RequiresMultipleSubmeshesByDrawOrder(Skeleton skeleton)
	{
		ExposedList<Slot> drawOrder = skeleton.DrawOrder;
		int count = drawOrder.Count;
		Slot[] items = drawOrder.Items;
		Material material = null;
		for (int i = 0; i < count; i++)
		{
			Slot slot = items[i];
			if (!slot.Bone.Active || !(slot.Attachment is IHasRendererObject hasRendererObject))
			{
				continue;
			}
			Material material2 = (Material)((AtlasRegion)hasRendererObject.RendererObject).page.rendererObject;
			if (material != material2)
			{
				if (material != null)
				{
					return true;
				}
				material = material2;
			}
		}
		return false;
	}

	public static void GenerateSkeletonRendererInstruction(SkeletonRendererInstruction instructionOutput, Skeleton skeleton, Dictionary<Slot, Material> customSlotMaterials, List<Slot> separatorSlots, bool generateMeshOverride, bool immutableTriangles = false)
	{
		ExposedList<Slot> drawOrder = skeleton.DrawOrder;
		int count = drawOrder.Count;
		instructionOutput.Clear();
		ExposedList<SubmeshInstruction> submeshInstructions = instructionOutput.submeshInstructions;
		instructionOutput.attachments.Resize(count);
		Attachment[] items = instructionOutput.attachments.Items;
		int num = 0;
		bool hasActiveClipping = false;
		SubmeshInstruction submeshInstruction = new SubmeshInstruction
		{
			skeleton = skeleton,
			preActiveClippingSlotSource = -1
		};
		bool flag = customSlotMaterials != null && customSlotMaterials.Count > 0;
		int num2 = separatorSlots?.Count ?? 0;
		bool flag2 = num2 > 0;
		int num3 = -1;
		int preActiveClippingSlotSource = -1;
		SlotData slotData = null;
		int num4 = 0;
		Slot[] items2 = drawOrder.Items;
		for (int i = 0; i < count; i++)
		{
			Slot slot = items2[i];
			if (!slot.Bone.Active)
			{
				items[i] = null;
				continue;
			}
			if (slot.Data.BlendMode == BlendMode.Additive)
			{
				submeshInstruction.hasPMAAdditiveSlot = true;
			}
			Attachment attachment = (items[i] = slot.Attachment);
			int num5 = 0;
			int num6 = 0;
			object obj = null;
			bool flag3 = false;
			if (attachment is RegionAttachment regionAttachment)
			{
				obj = regionAttachment.RendererObject;
				num5 = 4;
				num6 = 6;
			}
			else if (attachment is MeshAttachment meshAttachment)
			{
				obj = meshAttachment.RendererObject;
				num5 = meshAttachment.WorldVerticesLength >> 1;
				num6 = meshAttachment.Triangles.Length;
			}
			else
			{
				if (attachment is ClippingAttachment clippingAttachment)
				{
					slotData = clippingAttachment.EndSlot;
					num3 = i;
					submeshInstruction.hasClipping = true;
					hasActiveClipping = true;
				}
				flag3 = true;
			}
			if (flag2)
			{
				submeshInstruction.forceSeparate = false;
				for (int j = 0; j < num2; j++)
				{
					if (slot == separatorSlots[j])
					{
						submeshInstruction.forceSeparate = true;
						break;
					}
				}
			}
			if (flag3)
			{
				if (submeshInstruction.forceSeparate && generateMeshOverride)
				{
					submeshInstruction.endSlot = i;
					submeshInstruction.preActiveClippingSlotSource = preActiveClippingSlotSource;
					submeshInstructions.Resize(num4 + 1);
					submeshInstructions.Items[num4] = submeshInstruction;
					num4++;
					submeshInstruction.startSlot = i;
					preActiveClippingSlotSource = num3;
					submeshInstruction.rawTriangleCount = 0;
					submeshInstruction.rawVertexCount = 0;
					submeshInstruction.rawFirstVertexIndex = num;
					submeshInstruction.hasClipping = num3 >= 0;
				}
			}
			else
			{
				Material value;
				if (flag)
				{
					if (!customSlotMaterials.TryGetValue(slot, out value))
					{
						value = (Material)((AtlasRegion)obj).page.rendererObject;
					}
				}
				else
				{
					value = (Material)((AtlasRegion)obj).page.rendererObject;
				}
				if (submeshInstruction.forceSeparate || (submeshInstruction.rawVertexCount > 0 && (object)submeshInstruction.material != value))
				{
					submeshInstruction.endSlot = i;
					submeshInstruction.preActiveClippingSlotSource = preActiveClippingSlotSource;
					submeshInstructions.Resize(num4 + 1);
					submeshInstructions.Items[num4] = submeshInstruction;
					num4++;
					submeshInstruction.startSlot = i;
					preActiveClippingSlotSource = num3;
					submeshInstruction.rawTriangleCount = 0;
					submeshInstruction.rawVertexCount = 0;
					submeshInstruction.rawFirstVertexIndex = num;
					submeshInstruction.hasClipping = num3 >= 0;
				}
				submeshInstruction.material = value;
				submeshInstruction.rawTriangleCount += num6;
				submeshInstruction.rawVertexCount += num5;
				submeshInstruction.rawFirstVertexIndex = num;
				num += num5;
			}
			if (slotData != null && slot.Data == slotData && i != num3)
			{
				slotData = null;
				num3 = -1;
			}
		}
		if (submeshInstruction.rawVertexCount > 0)
		{
			submeshInstruction.endSlot = count;
			submeshInstruction.preActiveClippingSlotSource = preActiveClippingSlotSource;
			submeshInstruction.forceSeparate = false;
			submeshInstructions.Resize(num4 + 1);
			submeshInstructions.Items[num4] = submeshInstruction;
		}
		instructionOutput.hasActiveClipping = hasActiveClipping;
		instructionOutput.rawVertexCount = num;
		instructionOutput.immutableTriangles = immutableTriangles;
	}

	public static void TryReplaceMaterials(ExposedList<SubmeshInstruction> workingSubmeshInstructions, Dictionary<Material, Material> customMaterialOverride)
	{
		SubmeshInstruction[] items = workingSubmeshInstructions.Items;
		for (int i = 0; i < workingSubmeshInstructions.Count; i++)
		{
			Material material = items[i].material;
			if (customMaterialOverride.TryGetValue(material, out var value))
			{
				items[i].material = value;
			}
		}
	}

	public void Begin()
	{
		vertexBuffer.Clear(clearArray: false);
		colorBuffer.Clear(clearArray: false);
		uvBuffer.Clear(clearArray: false);
		clipper.ClipEnd();
		meshBoundsMin.x = float.PositiveInfinity;
		meshBoundsMin.y = float.PositiveInfinity;
		meshBoundsMax.x = float.NegativeInfinity;
		meshBoundsMax.y = float.NegativeInfinity;
		meshBoundsThickness = 0f;
		submeshIndex = 0;
		submeshes.Count = 1;
	}

	public void AddSubmesh(SubmeshInstruction instruction, bool updateTriangles = true)
	{
		Settings settings = this.settings;
		int num = submeshIndex + 1;
		if (submeshes.Items.Length < num)
		{
			submeshes.Resize(num);
		}
		submeshes.Count = num;
		ExposedList<int> exposedList = submeshes.Items[submeshIndex];
		if (exposedList == null)
		{
			exposedList = (submeshes.Items[submeshIndex] = new ExposedList<int>());
		}
		exposedList.Clear(clearArray: false);
		Skeleton skeleton = instruction.skeleton;
		Slot[] items = skeleton.DrawOrder.Items;
		Color32 color = default(Color32);
		float a = skeleton.A;
		float r = skeleton.R;
		float g = skeleton.G;
		float b = skeleton.B;
		Vector2 vector = meshBoundsMin;
		Vector2 vector2 = meshBoundsMax;
		float zSpacing = settings.zSpacing;
		bool pmaVertexColors = settings.pmaVertexColors;
		bool tintBlack = settings.tintBlack;
		bool flag = settings.useClipping && instruction.hasClipping;
		bool flag2 = settings.tintBlack && settings.canvasGroupTintBlack;
		if (flag && instruction.preActiveClippingSlotSource >= 0)
		{
			Slot slot = items[instruction.preActiveClippingSlotSource];
			clipper.ClipStart(slot, slot.Attachment as ClippingAttachment);
		}
		for (int i = instruction.startSlot; i < instruction.endSlot; i++)
		{
			Slot slot2 = items[i];
			if (!slot2.Bone.Active)
			{
				clipper.ClipEnd(slot2);
				continue;
			}
			Attachment attachment = slot2.Attachment;
			float z = zSpacing * (float)i;
			float[] array = tempVerts;
			Color color2 = default(Color);
			float[] array2;
			int[] array3;
			int num2;
			int num3;
			if (attachment is RegionAttachment regionAttachment)
			{
				regionAttachment.ComputeWorldVertices(slot2.Bone, array, 0);
				array2 = regionAttachment.UVs;
				array3 = regionTriangles;
				color2.r = regionAttachment.R;
				color2.g = regionAttachment.G;
				color2.b = regionAttachment.B;
				color2.a = regionAttachment.A;
				num2 = 4;
				num3 = 6;
			}
			else
			{
				if (!(attachment is MeshAttachment { WorldVerticesLength: var worldVerticesLength } meshAttachment))
				{
					if (flag && attachment is ClippingAttachment clip)
					{
						clipper.ClipStart(slot2, clip);
					}
					else
					{
						clipper.ClipEnd(slot2);
					}
					continue;
				}
				if (array.Length < worldVerticesLength)
				{
					array = (tempVerts = new float[worldVerticesLength]);
				}
				meshAttachment.ComputeWorldVertices(slot2, 0, worldVerticesLength, array, 0);
				array2 = meshAttachment.UVs;
				array3 = meshAttachment.Triangles;
				color2.r = meshAttachment.R;
				color2.g = meshAttachment.G;
				color2.b = meshAttachment.B;
				color2.a = meshAttachment.A;
				num2 = worldVerticesLength >> 1;
				num3 = meshAttachment.Triangles.Length;
			}
			float a2 = 1f;
			if (pmaVertexColors)
			{
				float num4 = a * slot2.A * color2.a;
				color.a = (byte)(num4 * 255f);
				color.r = (byte)(r * slot2.R * color2.r * (float)(int)color.a);
				color.g = (byte)(g * slot2.G * color2.g * (float)(int)color.a);
				color.b = (byte)(b * slot2.B * color2.b * (float)(int)color.a);
				if (slot2.Data.BlendMode == BlendMode.Additive)
				{
					if (flag2)
					{
						a2 = 0f;
					}
					else
					{
						color.a = 0;
					}
				}
				else if (flag2)
				{
					a2 = num4;
				}
			}
			else
			{
				color.a = (byte)(a * slot2.A * color2.a * 255f);
				color.r = (byte)(r * slot2.R * color2.r * 255f);
				color.g = (byte)(g * slot2.G * color2.g * 255f);
				color.b = (byte)(b * slot2.B * color2.b * 255f);
			}
			if (flag && clipper.IsClipping)
			{
				clipper.ClipTriangles(array, num2 << 1, array3, num3, array2);
				array = clipper.ClippedVertices.Items;
				num2 = clipper.ClippedVertices.Count >> 1;
				array3 = clipper.ClippedTriangles.Items;
				num3 = clipper.ClippedTriangles.Count;
				array2 = clipper.ClippedUVs.Items;
			}
			if (num2 != 0 && num3 != 0)
			{
				if (tintBlack)
				{
					float num5 = slot2.R2;
					float num6 = slot2.G2;
					float num7 = slot2.B2;
					if (pmaVertexColors)
					{
						float num8 = a * slot2.A * color2.a;
						num5 *= num8;
						num6 *= num8;
						num7 *= num8;
					}
					AddAttachmentTintBlack(num5, num6, num7, a2, num2);
				}
				int count = vertexBuffer.Count;
				int num9 = count + num2;
				int num10 = vertexBuffer.Items.Length;
				if (num9 > num10)
				{
					int num11 = (int)((float)num10 * 1.3f);
					if (num11 < num9)
					{
						num11 = num9;
					}
					Array.Resize(ref vertexBuffer.Items, num11);
					Array.Resize(ref uvBuffer.Items, num11);
					Array.Resize(ref colorBuffer.Items, num11);
				}
				vertexBuffer.Count = (uvBuffer.Count = (colorBuffer.Count = num9));
				Vector3[] items2 = vertexBuffer.Items;
				Vector2[] items3 = uvBuffer.Items;
				Color32[] items4 = colorBuffer.Items;
				if (count == 0)
				{
					for (int j = 0; j < num2; j++)
					{
						int num12 = count + j;
						int num13 = j << 1;
						float num14 = array[num13];
						float num15 = array[num13 + 1];
						items2[num12].x = num14;
						items2[num12].y = num15;
						items2[num12].z = z;
						items3[num12].x = array2[num13];
						items3[num12].y = array2[num13 + 1];
						items4[num12] = color;
						if (num14 < vector.x)
						{
							vector.x = num14;
						}
						if (num14 > vector2.x)
						{
							vector2.x = num14;
						}
						if (num15 < vector.y)
						{
							vector.y = num15;
						}
						if (num15 > vector2.y)
						{
							vector2.y = num15;
						}
					}
				}
				else
				{
					for (int k = 0; k < num2; k++)
					{
						int num16 = count + k;
						int num17 = k << 1;
						float num18 = array[num17];
						float num19 = array[num17 + 1];
						items2[num16].x = num18;
						items2[num16].y = num19;
						items2[num16].z = z;
						items3[num16].x = array2[num17];
						items3[num16].y = array2[num17 + 1];
						items4[num16] = color;
						if (num18 < vector.x)
						{
							vector.x = num18;
						}
						else if (num18 > vector2.x)
						{
							vector2.x = num18;
						}
						if (num19 < vector.y)
						{
							vector.y = num19;
						}
						else if (num19 > vector2.y)
						{
							vector2.y = num19;
						}
					}
				}
				if (updateTriangles)
				{
					int count2 = exposedList.Count;
					int num20 = count2 + num3;
					if (num20 > exposedList.Items.Length)
					{
						Array.Resize(ref exposedList.Items, num20);
					}
					exposedList.Count = num20;
					int[] items5 = exposedList.Items;
					for (int l = 0; l < num3; l++)
					{
						items5[count2 + l] = array3[l] + count;
					}
				}
			}
			clipper.ClipEnd(slot2);
		}
		clipper.ClipEnd();
		meshBoundsMin = vector;
		meshBoundsMax = vector2;
		meshBoundsThickness = (float)instruction.endSlot * zSpacing;
		int[] items6 = exposedList.Items;
		int m = exposedList.Count;
		for (int num21 = items6.Length; m < num21; m++)
		{
			items6[m] = 0;
		}
		submeshIndex++;
	}

	public void BuildMesh(SkeletonRendererInstruction instruction, bool updateTriangles)
	{
		SubmeshInstruction[] items = instruction.submeshInstructions.Items;
		int i = 0;
		for (int count = instruction.submeshInstructions.Count; i < count; i++)
		{
			AddSubmesh(items[i], updateTriangles);
		}
	}

	public void BuildMeshWithArrays(SkeletonRendererInstruction instruction, bool updateTriangles)
	{
		Settings settings = this.settings;
		bool flag = settings.tintBlack && settings.canvasGroupTintBlack;
		int rawVertexCount = instruction.rawVertexCount;
		if (rawVertexCount > vertexBuffer.Items.Length)
		{
			Array.Resize(ref vertexBuffer.Items, rawVertexCount);
			Array.Resize(ref uvBuffer.Items, rawVertexCount);
			Array.Resize(ref colorBuffer.Items, rawVertexCount);
		}
		vertexBuffer.Count = (uvBuffer.Count = (colorBuffer.Count = rawVertexCount));
		Color32 color = default(Color32);
		int num = 0;
		float[] array = tempVerts;
		Vector2 vector = meshBoundsMin;
		Vector2 vector2 = meshBoundsMax;
		Vector3[] items = vertexBuffer.Items;
		Vector2[] items2 = uvBuffer.Items;
		Color32[] items3 = colorBuffer.Items;
		int num2 = 0;
		int i = 0;
		Vector2 vector3 = default(Vector2);
		Vector2 vector4 = default(Vector2);
		for (int count = instruction.submeshInstructions.Count; i < count; i++)
		{
			SubmeshInstruction submeshInstruction = instruction.submeshInstructions.Items[i];
			Skeleton skeleton = submeshInstruction.skeleton;
			Slot[] items4 = skeleton.DrawOrder.Items;
			float a = skeleton.A;
			float r = skeleton.R;
			float g = skeleton.G;
			float b = skeleton.B;
			int endSlot = submeshInstruction.endSlot;
			int startSlot = submeshInstruction.startSlot;
			num2 = endSlot;
			if (settings.tintBlack)
			{
				int num3 = num;
				vector3.y = 1f;
				if (uv2 == null)
				{
					uv2 = new ExposedList<Vector2>();
					uv3 = new ExposedList<Vector2>();
				}
				if (rawVertexCount > uv2.Items.Length)
				{
					Array.Resize(ref uv2.Items, rawVertexCount);
					Array.Resize(ref uv3.Items, rawVertexCount);
				}
				uv2.Count = (uv3.Count = rawVertexCount);
				Vector2[] items5 = uv2.Items;
				Vector2[] items6 = uv3.Items;
				for (int j = startSlot; j < endSlot; j++)
				{
					Slot slot = items4[j];
					if (!slot.Bone.Active)
					{
						continue;
					}
					Attachment attachment = slot.Attachment;
					vector4.x = slot.R2;
					vector4.y = slot.G2;
					vector3.x = slot.B2;
					vector3.y = 1f;
					if (attachment is RegionAttachment regionAttachment)
					{
						if (settings.pmaVertexColors)
						{
							float num4 = a * slot.A * regionAttachment.A;
							vector4.x *= num4;
							vector4.y *= num4;
							vector3.x *= num4;
							vector3.y = ((slot.Data.BlendMode == BlendMode.Additive) ? 0f : num4);
						}
						items5[num3] = vector4;
						items5[num3 + 1] = vector4;
						items5[num3 + 2] = vector4;
						items5[num3 + 3] = vector4;
						items6[num3] = vector3;
						items6[num3 + 1] = vector3;
						items6[num3 + 2] = vector3;
						items6[num3 + 3] = vector3;
						num3 += 4;
					}
					else if (attachment is MeshAttachment meshAttachment)
					{
						if (settings.pmaVertexColors)
						{
							float num5 = a * slot.A * meshAttachment.A;
							vector4.x *= num5;
							vector4.y *= num5;
							vector3.x *= num5;
							vector3.y = ((slot.Data.BlendMode == BlendMode.Additive) ? 0f : num5);
						}
						int worldVerticesLength = meshAttachment.WorldVerticesLength;
						for (int k = 0; k < worldVerticesLength; k += 2)
						{
							items5[num3] = vector4;
							items6[num3] = vector3;
							num3++;
						}
					}
				}
			}
			for (int l = startSlot; l < endSlot; l++)
			{
				Slot slot2 = items4[l];
				if (!slot2.Bone.Active)
				{
					continue;
				}
				Attachment attachment2 = slot2.Attachment;
				float z = (float)l * settings.zSpacing;
				if (attachment2 is RegionAttachment regionAttachment2)
				{
					regionAttachment2.ComputeWorldVertices(slot2.Bone, array, 0);
					float num6 = array[0];
					float num7 = array[1];
					float num8 = array[2];
					float num9 = array[3];
					float num10 = array[4];
					float num11 = array[5];
					float num12 = array[6];
					float num13 = array[7];
					items[num].x = num6;
					items[num].y = num7;
					items[num].z = z;
					items[num + 1].x = num12;
					items[num + 1].y = num13;
					items[num + 1].z = z;
					items[num + 2].x = num8;
					items[num + 2].y = num9;
					items[num + 2].z = z;
					items[num + 3].x = num10;
					items[num + 3].y = num11;
					items[num + 3].z = z;
					if (settings.pmaVertexColors)
					{
						color.a = (byte)(a * slot2.A * regionAttachment2.A * 255f);
						color.r = (byte)(r * slot2.R * regionAttachment2.R * (float)(int)color.a);
						color.g = (byte)(g * slot2.G * regionAttachment2.G * (float)(int)color.a);
						color.b = (byte)(b * slot2.B * regionAttachment2.B * (float)(int)color.a);
						if (slot2.Data.BlendMode == BlendMode.Additive && !flag)
						{
							color.a = 0;
						}
					}
					else
					{
						color.a = (byte)(a * slot2.A * regionAttachment2.A * 255f);
						color.r = (byte)(r * slot2.R * regionAttachment2.R * 255f);
						color.g = (byte)(g * slot2.G * regionAttachment2.G * 255f);
						color.b = (byte)(b * slot2.B * regionAttachment2.B * 255f);
					}
					items3[num] = color;
					items3[num + 1] = color;
					items3[num + 2] = color;
					items3[num + 3] = color;
					float[] uVs = regionAttachment2.UVs;
					items2[num].x = uVs[0];
					items2[num].y = uVs[1];
					items2[num + 1].x = uVs[6];
					items2[num + 1].y = uVs[7];
					items2[num + 2].x = uVs[2];
					items2[num + 2].y = uVs[3];
					items2[num + 3].x = uVs[4];
					items2[num + 3].y = uVs[5];
					if (num6 < vector.x)
					{
						vector.x = num6;
					}
					if (num6 > vector2.x)
					{
						vector2.x = num6;
					}
					if (num8 < vector.x)
					{
						vector.x = num8;
					}
					else if (num8 > vector2.x)
					{
						vector2.x = num8;
					}
					if (num10 < vector.x)
					{
						vector.x = num10;
					}
					else if (num10 > vector2.x)
					{
						vector2.x = num10;
					}
					if (num12 < vector.x)
					{
						vector.x = num12;
					}
					else if (num12 > vector2.x)
					{
						vector2.x = num12;
					}
					if (num7 < vector.y)
					{
						vector.y = num7;
					}
					if (num7 > vector2.y)
					{
						vector2.y = num7;
					}
					if (num9 < vector.y)
					{
						vector.y = num9;
					}
					else if (num9 > vector2.y)
					{
						vector2.y = num9;
					}
					if (num11 < vector.y)
					{
						vector.y = num11;
					}
					else if (num11 > vector2.y)
					{
						vector2.y = num11;
					}
					if (num13 < vector.y)
					{
						vector.y = num13;
					}
					else if (num13 > vector2.y)
					{
						vector2.y = num13;
					}
					num += 4;
				}
				else
				{
					if (!(attachment2 is MeshAttachment { WorldVerticesLength: var worldVerticesLength2 } meshAttachment2))
					{
						continue;
					}
					if (array.Length < worldVerticesLength2)
					{
						array = (tempVerts = new float[worldVerticesLength2]);
					}
					meshAttachment2.ComputeWorldVertices(slot2, array);
					if (settings.pmaVertexColors)
					{
						color.a = (byte)(a * slot2.A * meshAttachment2.A * 255f);
						color.r = (byte)(r * slot2.R * meshAttachment2.R * (float)(int)color.a);
						color.g = (byte)(g * slot2.G * meshAttachment2.G * (float)(int)color.a);
						color.b = (byte)(b * slot2.B * meshAttachment2.B * (float)(int)color.a);
						if (slot2.Data.BlendMode == BlendMode.Additive && !flag)
						{
							color.a = 0;
						}
					}
					else
					{
						color.a = (byte)(a * slot2.A * meshAttachment2.A * 255f);
						color.r = (byte)(r * slot2.R * meshAttachment2.R * 255f);
						color.g = (byte)(g * slot2.G * meshAttachment2.G * 255f);
						color.b = (byte)(b * slot2.B * meshAttachment2.B * 255f);
					}
					float[] uVs2 = meshAttachment2.UVs;
					if (num == 0)
					{
						float num14 = array[0];
						float num15 = array[1];
						if (num14 < vector.x)
						{
							vector.x = num14;
						}
						if (num14 > vector2.x)
						{
							vector2.x = num14;
						}
						if (num15 < vector.y)
						{
							vector.y = num15;
						}
						if (num15 > vector2.y)
						{
							vector2.y = num15;
						}
					}
					for (int m = 0; m < worldVerticesLength2; m += 2)
					{
						float num16 = array[m];
						float num17 = array[m + 1];
						items[num].x = num16;
						items[num].y = num17;
						items[num].z = z;
						items3[num] = color;
						items2[num].x = uVs2[m];
						items2[num].y = uVs2[m + 1];
						if (num16 < vector.x)
						{
							vector.x = num16;
						}
						else if (num16 > vector2.x)
						{
							vector2.x = num16;
						}
						if (num17 < vector.y)
						{
							vector.y = num17;
						}
						else if (num17 > vector2.y)
						{
							vector2.y = num17;
						}
						num++;
					}
				}
			}
		}
		meshBoundsMin = vector;
		meshBoundsMax = vector2;
		meshBoundsThickness = (float)num2 * settings.zSpacing;
		int count2 = instruction.submeshInstructions.Count;
		submeshes.Count = count2;
		if (!updateTriangles)
		{
			return;
		}
		if (submeshes.Items.Length < count2)
		{
			submeshes.Resize(count2);
			int n = 0;
			for (int num18 = count2; n < num18; n++)
			{
				ExposedList<int> exposedList = submeshes.Items[n];
				if (exposedList == null)
				{
					submeshes.Items[n] = new ExposedList<int>();
				}
				else
				{
					exposedList.Clear(clearArray: false);
				}
			}
		}
		SubmeshInstruction[] items7 = instruction.submeshInstructions.Items;
		int num19 = 0;
		for (int num20 = 0; num20 < count2; num20++)
		{
			SubmeshInstruction submeshInstruction2 = items7[num20];
			ExposedList<int> exposedList2 = submeshes.Items[num20];
			int rawTriangleCount = submeshInstruction2.rawTriangleCount;
			if (rawTriangleCount > exposedList2.Items.Length)
			{
				Array.Resize(ref exposedList2.Items, rawTriangleCount);
			}
			else if (rawTriangleCount < exposedList2.Items.Length)
			{
				int[] items8 = exposedList2.Items;
				int num21 = rawTriangleCount;
				for (int num22 = items8.Length; num21 < num22; num21++)
				{
					items8[num21] = 0;
				}
			}
			exposedList2.Count = rawTriangleCount;
			int[] items9 = exposedList2.Items;
			int num23 = 0;
			Slot[] items10 = submeshInstruction2.skeleton.DrawOrder.Items;
			int num24 = submeshInstruction2.startSlot;
			for (int endSlot2 = submeshInstruction2.endSlot; num24 < endSlot2; num24++)
			{
				if (!items10[num24].Bone.Active)
				{
					continue;
				}
				Attachment attachment3 = items10[num24].Attachment;
				if (attachment3 is RegionAttachment)
				{
					items9[num23] = num19;
					items9[num23 + 1] = num19 + 2;
					items9[num23 + 2] = num19 + 1;
					items9[num23 + 3] = num19 + 2;
					items9[num23 + 4] = num19 + 3;
					items9[num23 + 5] = num19 + 1;
					num23 += 6;
					num19 += 4;
				}
				else if (attachment3 is MeshAttachment { Triangles: var triangles } meshAttachment3)
				{
					int num25 = 0;
					int num26 = triangles.Length;
					while (num25 < num26)
					{
						items9[num23] = num19 + triangles[num25];
						num25++;
						num23++;
					}
					num19 += meshAttachment3.WorldVerticesLength >> 1;
				}
			}
		}
	}

	public void ScaleVertexData(float scale)
	{
		Vector3[] items = vertexBuffer.Items;
		int i = 0;
		for (int count = vertexBuffer.Count; i < count; i++)
		{
			items[i] *= scale;
		}
		meshBoundsMin *= scale;
		meshBoundsMax *= scale;
		meshBoundsThickness *= scale;
	}

	public Bounds GetMeshBounds()
	{
		if (float.IsInfinity(meshBoundsMin.x))
		{
			return default(Bounds);
		}
		float num = (meshBoundsMax.x - meshBoundsMin.x) * 0.5f;
		float num2 = (meshBoundsMax.y - meshBoundsMin.y) * 0.5f;
		return new Bounds
		{
			center = new Vector3(meshBoundsMin.x + num, meshBoundsMin.y + num2),
			extents = new Vector3(num, num2, meshBoundsThickness * 0.5f)
		};
	}

	private void AddAttachmentTintBlack(float r2, float g2, float b2, float a, int vertexCount)
	{
		Vector2 vector = new Vector2(r2, g2);
		Vector2 vector2 = new Vector2(b2, a);
		int count = vertexBuffer.Count;
		int num = count + vertexCount;
		if (uv2 == null)
		{
			uv2 = new ExposedList<Vector2>();
			uv3 = new ExposedList<Vector2>();
		}
		if (num > uv2.Items.Length)
		{
			Array.Resize(ref uv2.Items, num);
			Array.Resize(ref uv3.Items, num);
		}
		uv2.Count = (uv3.Count = num);
		Vector2[] items = uv2.Items;
		Vector2[] items2 = uv3.Items;
		for (int i = 0; i < vertexCount; i++)
		{
			items[count + i] = vector;
			items2[count + i] = vector2;
		}
	}

	public void FillVertexData(Mesh mesh)
	{
		Vector3[] items = vertexBuffer.Items;
		Vector2[] items2 = uvBuffer.Items;
		Color32[] items3 = colorBuffer.Items;
		int num = items.Length;
		int count = vertexBuffer.Count;
		Vector3 zero = Vector3.zero;
		for (int i = count; i < num; i++)
		{
			items[i] = zero;
		}
		mesh.vertices = items;
		mesh.uv = items2;
		mesh.colors32 = items3;
		mesh.bounds = GetMeshBounds();
		if (settings.addNormals)
		{
			int num2 = 0;
			if (normals == null)
			{
				normals = new Vector3[num];
			}
			else
			{
				num2 = normals.Length;
			}
			if (num2 != num)
			{
				Array.Resize(ref normals, num);
				Vector3[] array = normals;
				for (int j = num2; j < num; j++)
				{
					array[j] = Vector3.back;
				}
			}
			mesh.normals = normals;
		}
		if (settings.tintBlack && uv2 != null)
		{
			if (num != uv2.Items.Length)
			{
				Array.Resize(ref uv2.Items, num);
				Array.Resize(ref uv3.Items, num);
				uv2.Count = (uv3.Count = num);
			}
			mesh.uv2 = uv2.Items;
			mesh.uv3 = uv3.Items;
		}
	}

	public void FillLateVertexData(Mesh mesh)
	{
		if (settings.calculateTangents)
		{
			int count = vertexBuffer.Count;
			ExposedList<int>[] items = submeshes.Items;
			int count2 = submeshes.Count;
			Vector3[] items2 = vertexBuffer.Items;
			Vector2[] items3 = uvBuffer.Items;
			SolveTangents2DEnsureSize(ref tangents, ref tempTanBuffer, count, items2.Length);
			for (int i = 0; i < count2; i++)
			{
				int[] items4 = items[i].Items;
				int count3 = items[i].Count;
				SolveTangents2DTriangles(tempTanBuffer, items4, count3, items2, items3, count);
			}
			SolveTangents2DBuffer(tangents, tempTanBuffer, count);
			mesh.tangents = tangents;
		}
	}

	public void FillTriangles(Mesh mesh)
	{
		int count = submeshes.Count;
		ExposedList<int>[] items = submeshes.Items;
		mesh.subMeshCount = count;
		for (int i = 0; i < count; i++)
		{
			mesh.SetTriangles(items[i].Items, 0, items[i].Count, i, calculateBounds: false);
		}
	}

	public void EnsureVertexCapacity(int minimumVertexCount, bool inlcudeTintBlack = false, bool includeTangents = false, bool includeNormals = false)
	{
		if (minimumVertexCount <= vertexBuffer.Items.Length)
		{
			return;
		}
		Array.Resize(ref vertexBuffer.Items, minimumVertexCount);
		Array.Resize(ref uvBuffer.Items, minimumVertexCount);
		Array.Resize(ref colorBuffer.Items, minimumVertexCount);
		if (inlcudeTintBlack)
		{
			if (uv2 == null)
			{
				uv2 = new ExposedList<Vector2>(minimumVertexCount);
				uv3 = new ExposedList<Vector2>(minimumVertexCount);
			}
			uv2.Resize(minimumVertexCount);
			uv3.Resize(minimumVertexCount);
		}
		if (includeNormals)
		{
			if (normals == null)
			{
				normals = new Vector3[minimumVertexCount];
			}
			else
			{
				Array.Resize(ref normals, minimumVertexCount);
			}
		}
		if (includeTangents)
		{
			if (tangents == null)
			{
				tangents = new Vector4[minimumVertexCount];
			}
			else
			{
				Array.Resize(ref tangents, minimumVertexCount);
			}
		}
	}

	public void TrimExcess()
	{
		vertexBuffer.TrimExcess();
		uvBuffer.TrimExcess();
		colorBuffer.TrimExcess();
		if (uv2 != null)
		{
			uv2.TrimExcess();
		}
		if (uv3 != null)
		{
			uv3.TrimExcess();
		}
		int newSize = vertexBuffer.Items.Length;
		if (normals != null)
		{
			Array.Resize(ref normals, newSize);
		}
		if (tangents != null)
		{
			Array.Resize(ref tangents, newSize);
		}
	}

	internal static void SolveTangents2DEnsureSize(ref Vector4[] tangentBuffer, ref Vector2[] tempTanBuffer, int vertexCount, int vertexBufferLength)
	{
		if (tangentBuffer == null || tangentBuffer.Length != vertexBufferLength)
		{
			tangentBuffer = new Vector4[vertexBufferLength];
		}
		if (tempTanBuffer == null || tempTanBuffer.Length < vertexCount * 2)
		{
			tempTanBuffer = new Vector2[vertexCount * 2];
		}
	}

	internal static void SolveTangents2DTriangles(Vector2[] tempTanBuffer, int[] triangles, int triangleCount, Vector3[] vertices, Vector2[] uvs, int vertexCount)
	{
		Vector2 vector7 = default(Vector2);
		Vector2 vector8 = default(Vector2);
		for (int i = 0; i < triangleCount; i += 3)
		{
			int num = triangles[i];
			int num2 = triangles[i + 1];
			int num3 = triangles[i + 2];
			Vector3 vector = vertices[num];
			Vector3 vector2 = vertices[num2];
			Vector3 vector3 = vertices[num3];
			Vector2 vector4 = uvs[num];
			Vector2 vector5 = uvs[num2];
			Vector2 vector6 = uvs[num3];
			float num4 = vector2.x - vector.x;
			float num5 = vector3.x - vector.x;
			float num6 = vector2.y - vector.y;
			float num7 = vector3.y - vector.y;
			float num8 = vector5.x - vector4.x;
			float num9 = vector6.x - vector4.x;
			float num10 = vector5.y - vector4.y;
			float num11 = vector6.y - vector4.y;
			float num12 = num8 * num11 - num9 * num10;
			float num13 = ((num12 == 0f) ? 0f : (1f / num12));
			vector7.x = (num11 * num4 - num10 * num5) * num13;
			vector7.y = (num11 * num6 - num10 * num7) * num13;
			tempTanBuffer[num] = (tempTanBuffer[num2] = (tempTanBuffer[num3] = vector7));
			vector8.x = (num8 * num5 - num9 * num4) * num13;
			vector8.y = (num8 * num7 - num9 * num6) * num13;
			tempTanBuffer[vertexCount + num] = (tempTanBuffer[vertexCount + num2] = (tempTanBuffer[vertexCount + num3] = vector8));
		}
	}

	internal static void SolveTangents2DBuffer(Vector4[] tangents, Vector2[] tempTanBuffer, int vertexCount)
	{
		Vector4 vector = default(Vector4);
		vector.z = 0f;
		for (int i = 0; i < vertexCount; i++)
		{
			Vector2 vector2 = tempTanBuffer[i];
			float num = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);
			if ((double)num > 1E-05)
			{
				float num2 = 1f / num;
				vector2.x *= num2;
				vector2.y *= num2;
			}
			Vector2 vector3 = tempTanBuffer[vertexCount + i];
			vector.x = vector2.x;
			vector.y = vector2.y;
			vector.w = ((vector2.y * vector3.x > vector2.x * vector3.y) ? 1 : (-1));
			tangents[i] = vector;
		}
	}

	public static void FillMeshLocal(Mesh mesh, RegionAttachment regionAttachment)
	{
		if (!(mesh == null) && regionAttachment != null)
		{
			AttachmentVerts.Clear();
			float[] offset = regionAttachment.Offset;
			AttachmentVerts.Add(new Vector3(offset[0], offset[1]));
			AttachmentVerts.Add(new Vector3(offset[2], offset[3]));
			AttachmentVerts.Add(new Vector3(offset[4], offset[5]));
			AttachmentVerts.Add(new Vector3(offset[6], offset[7]));
			AttachmentUVs.Clear();
			float[] uVs = regionAttachment.UVs;
			AttachmentUVs.Add(new Vector2(uVs[2], uVs[3]));
			AttachmentUVs.Add(new Vector2(uVs[4], uVs[5]));
			AttachmentUVs.Add(new Vector2(uVs[6], uVs[7]));
			AttachmentUVs.Add(new Vector2(uVs[0], uVs[1]));
			AttachmentColors32.Clear();
			Color32 item = new Color(regionAttachment.R, regionAttachment.G, regionAttachment.B, regionAttachment.A);
			for (int i = 0; i < 4; i++)
			{
				AttachmentColors32.Add(item);
			}
			AttachmentIndices.Clear();
			AttachmentIndices.AddRange(new int[6] { 0, 2, 1, 0, 3, 2 });
			mesh.Clear();
			mesh.name = regionAttachment.Name;
			mesh.SetVertices(AttachmentVerts);
			mesh.SetUVs(0, AttachmentUVs);
			mesh.SetColors(AttachmentColors32);
			mesh.SetTriangles(AttachmentIndices, 0);
			mesh.RecalculateBounds();
			AttachmentVerts.Clear();
			AttachmentUVs.Clear();
			AttachmentColors32.Clear();
			AttachmentIndices.Clear();
		}
	}

	public static void FillMeshLocal(Mesh mesh, MeshAttachment meshAttachment, SkeletonData skeletonData)
	{
		if (mesh == null || meshAttachment == null)
		{
			return;
		}
		int num = meshAttachment.WorldVerticesLength / 2;
		AttachmentVerts.Clear();
		if (meshAttachment.IsWeighted())
		{
			int worldVerticesLength = meshAttachment.WorldVerticesLength;
			int[] bones = meshAttachment.Bones;
			int num2 = 0;
			float[] vertices = meshAttachment.Vertices;
			int i = 0;
			int num3 = 0;
			for (; i < worldVerticesLength; i += 2)
			{
				float num4 = 0f;
				float num5 = 0f;
				int num6 = bones[num2++];
				num6 += num2;
				while (num2 < num6)
				{
					BoneMatrix boneMatrix = BoneMatrix.CalculateSetupWorld(skeletonData.Bones.Items[bones[num2]]);
					float num7 = vertices[num3];
					float num8 = vertices[num3 + 1];
					float num9 = vertices[num3 + 2];
					num4 += (num7 * boneMatrix.a + num8 * boneMatrix.b + boneMatrix.x) * num9;
					num5 += (num7 * boneMatrix.c + num8 * boneMatrix.d + boneMatrix.y) * num9;
					num2++;
					num3 += 3;
				}
				AttachmentVerts.Add(new Vector3(num4, num5));
			}
		}
		else
		{
			float[] vertices2 = meshAttachment.Vertices;
			Vector3 item = default(Vector3);
			for (int j = 0; j < num; j++)
			{
				int num10 = j * 2;
				item.x = vertices2[num10];
				item.y = vertices2[num10 + 1];
				AttachmentVerts.Add(item);
			}
		}
		float[] uVs = meshAttachment.UVs;
		Vector2 item2 = default(Vector2);
		Color32 item3 = new Color(meshAttachment.R, meshAttachment.G, meshAttachment.B, meshAttachment.A);
		AttachmentUVs.Clear();
		AttachmentColors32.Clear();
		for (int k = 0; k < num; k++)
		{
			int num11 = k * 2;
			item2.x = uVs[num11];
			item2.y = uVs[num11 + 1];
			AttachmentUVs.Add(item2);
			AttachmentColors32.Add(item3);
		}
		AttachmentIndices.Clear();
		AttachmentIndices.AddRange(meshAttachment.Triangles);
		mesh.Clear();
		mesh.name = meshAttachment.Name;
		mesh.SetVertices(AttachmentVerts);
		mesh.SetUVs(0, AttachmentUVs);
		mesh.SetColors(AttachmentColors32);
		mesh.SetTriangles(AttachmentIndices, 0);
		mesh.RecalculateBounds();
		AttachmentVerts.Clear();
		AttachmentUVs.Clear();
		AttachmentColors32.Clear();
		AttachmentIndices.Clear();
	}
}
