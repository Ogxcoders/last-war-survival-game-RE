using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public static class MeshSerializer
{
	private enum EChunkID : byte
	{
		End,
		Name,
		Normals,
		Tangents,
		Colors,
		BoneWeights,
		UV0,
		UV1,
		UV2,
		UV3,
		Submesh,
		Bindposes,
		BlendShape
	}

	private const uint m_Magic = 1752393037u;

	public static byte[] SerializeMesh(Mesh aMesh)
	{
		using MemoryStream memoryStream = new MemoryStream();
		SerializeMesh(memoryStream, aMesh);
		return memoryStream.ToArray();
	}

	public static void SerializeMesh(MemoryStream aStream, Mesh aMesh)
	{
		using BinaryWriter aWriter = new BinaryWriter(aStream);
		SerializeMesh(aWriter, aMesh);
	}

	public static void SerializeMesh(BinaryWriter aWriter, Mesh aMesh)
	{
		aWriter.Write(1752393037u);
		Vector3[] vertices = aMesh.vertices;
		int num = vertices.Length;
		int subMeshCount = aMesh.subMeshCount;
		aWriter.Write(num);
		aWriter.Write(subMeshCount);
		Vector3[] array = vertices;
		foreach (Vector3 aVec in array)
		{
			aWriter.WriteVector3(aVec);
		}
		if (!string.IsNullOrEmpty(aMesh.name))
		{
			aWriter.Write((byte)1);
			aWriter.Write(aMesh.name);
		}
		Vector3[] normals = aMesh.normals;
		if (normals != null && normals.Length == num)
		{
			aWriter.Write((byte)2);
			array = normals;
			foreach (Vector3 aVec2 in array)
			{
				aWriter.WriteVector3(aVec2);
			}
			normals = null;
		}
		Vector4[] tangents = aMesh.tangents;
		if (tangents != null && tangents.Length == num)
		{
			aWriter.Write((byte)3);
			Vector4[] array2 = tangents;
			foreach (Vector4 aVec3 in array2)
			{
				aWriter.WriteVector4(aVec3);
			}
			tangents = null;
		}
		Color32[] colors = aMesh.colors32;
		if (colors != null && colors.Length == num)
		{
			aWriter.Write((byte)4);
			Color32[] array3 = colors;
			foreach (Color32 aCol in array3)
			{
				aWriter.WriteColor32(aCol);
			}
			colors = null;
		}
		BoneWeight[] boneWeights = aMesh.boneWeights;
		if (boneWeights != null && boneWeights.Length == num)
		{
			aWriter.Write((byte)5);
			BoneWeight[] array4 = boneWeights;
			foreach (BoneWeight aWeight in array4)
			{
				aWriter.WriteBoneWeight(aWeight);
			}
			boneWeights = null;
		}
		List<Vector4> list = new List<Vector4>();
		for (int j = 0; j < 4; j++)
		{
			list.Clear();
			aMesh.GetUVs(j, list);
			if (list.Count != num)
			{
				continue;
			}
			aWriter.Write((byte)(6 + j));
			byte b = 2;
			foreach (Vector4 item in list)
			{
				if (item.z != 0f)
				{
					b = 3;
				}
				if (item.w != 0f)
				{
					b = 4;
					break;
				}
			}
			aWriter.Write(b);
			switch (b)
			{
			case 2:
				foreach (Vector4 item2 in list)
				{
					aWriter.WriteVector2(item2);
				}
				continue;
			case 3:
				foreach (Vector4 item3 in list)
				{
					aWriter.WriteVector3(item3);
				}
				continue;
			}
			foreach (Vector4 item4 in list)
			{
				aWriter.WriteVector4(item4);
			}
		}
		List<int> list2 = new List<int>(num * 3);
		for (int k = 0; k < subMeshCount; k++)
		{
			list2.Clear();
			aMesh.GetIndices(list2, k);
			if (list2.Count <= 0)
			{
				continue;
			}
			aWriter.Write((byte)10);
			aWriter.Write((byte)aMesh.GetTopology(k));
			aWriter.Write(list2.Count);
			int num2 = list2.Max();
			if (num2 < 256)
			{
				aWriter.Write((byte)1);
				foreach (int item5 in list2)
				{
					aWriter.Write((byte)item5);
				}
				continue;
			}
			if (num2 < 65536)
			{
				aWriter.Write((byte)2);
				foreach (int item6 in list2)
				{
					aWriter.Write((ushort)item6);
				}
				continue;
			}
			aWriter.Write((byte)4);
			foreach (int item7 in list2)
			{
				aWriter.Write(item7);
			}
		}
		Matrix4x4[] bindposes = aMesh.bindposes;
		if (bindposes != null && bindposes.Length != 0)
		{
			aWriter.Write((byte)11);
			aWriter.Write(bindposes.Length);
			Matrix4x4[] array5 = bindposes;
			foreach (Matrix4x4 aMat in array5)
			{
				aWriter.WriteMatrix4x4(aMat);
			}
			bindposes = null;
		}
		int blendShapeCount = aMesh.blendShapeCount;
		if (blendShapeCount > 0)
		{
			Vector3[] array6 = new Vector3[num];
			Vector3[] array7 = new Vector3[num];
			Vector3[] array8 = new Vector3[num];
			for (int l = 0; l < blendShapeCount; l++)
			{
				aWriter.Write((byte)12);
				aWriter.Write(aMesh.GetBlendShapeName(l));
				int blendShapeFrameCount = aMesh.GetBlendShapeFrameCount(l);
				aWriter.Write(blendShapeFrameCount);
				for (int m = 0; m < blendShapeFrameCount; m++)
				{
					aMesh.GetBlendShapeFrameVertices(l, m, array6, array7, array8);
					aWriter.Write(aMesh.GetBlendShapeFrameWeight(l, m));
					for (int n = 0; n < num; n++)
					{
						aWriter.WriteVector3(array6[n]);
						aWriter.WriteVector3(array7[n]);
						aWriter.WriteVector3(array8[n]);
					}
				}
			}
		}
		aWriter.Write((byte)0);
	}

	public static Mesh DeserializeMesh(byte[] aData, Mesh aTarget = null)
	{
		using MemoryStream aStream = new MemoryStream(aData);
		return DeserializeMesh(aStream, aTarget);
	}

	public static Mesh DeserializeMesh(MemoryStream aStream, Mesh aTarget = null)
	{
		using BinaryReader aReader = new BinaryReader(aStream);
		return DeserializeMesh(aReader, aTarget);
	}

	public static Mesh DeserializeMesh(BinaryReader aReader, Mesh aTarget = null)
	{
		if (aReader.ReadUInt32() != 1752393037)
		{
			return null;
		}
		if (aTarget == null)
		{
			aTarget = new Mesh();
		}
		aTarget.Clear();
		aTarget.ClearBlendShapes();
		int num = aReader.ReadInt32();
		if (num > 65534)
		{
			aTarget.indexFormat = IndexFormat.UInt32;
		}
		int subMeshCount = aReader.ReadInt32();
		Vector3[] array = new Vector3[num];
		Vector3[] array2 = null;
		Vector3[] array3 = null;
		List<Vector4> list = null;
		for (int i = 0; i < num; i++)
		{
			array[i] = aReader.ReadVector3();
		}
		aTarget.vertices = array;
		aTarget.subMeshCount = subMeshCount;
		int num2 = 0;
		byte b = 0;
		Stream baseStream = aReader.BaseStream;
		while ((baseStream.CanSeek && baseStream.Position < baseStream.Length) || baseStream.CanRead)
		{
			EChunkID eChunkID = (EChunkID)aReader.ReadByte();
			switch (eChunkID)
			{
			case EChunkID.Name:
				aTarget.name = aReader.ReadString();
				continue;
			case EChunkID.Normals:
			{
				for (int l = 0; l < num; l++)
				{
					array[l] = aReader.ReadVector3();
				}
				aTarget.normals = array;
				continue;
			}
			case EChunkID.Tangents:
			{
				if (list == null)
				{
					list = new List<Vector4>(num);
				}
				list.Clear();
				for (int num10 = 0; num10 < num; num10++)
				{
					list.Add(aReader.ReadVector4());
				}
				aTarget.SetTangents(list);
				continue;
			}
			case EChunkID.Colors:
			{
				Color32[] array5 = new Color32[num];
				for (int num6 = 0; num6 < num; num6++)
				{
					array5[num6] = aReader.ReadColor32();
				}
				aTarget.colors32 = array5;
				continue;
			}
			case EChunkID.BoneWeights:
			{
				BoneWeight[] array7 = new BoneWeight[num];
				for (int num13 = 0; num13 < num; num13++)
				{
					array7[num13] = aReader.ReadBoneWeight();
				}
				aTarget.boneWeights = array7;
				continue;
			}
			case EChunkID.UV0:
			case EChunkID.UV1:
			case EChunkID.UV2:
			case EChunkID.UV3:
			{
				int channel = (int)(eChunkID - 6);
				b = aReader.ReadByte();
				if (list == null)
				{
					list = new List<Vector4>(num);
				}
				list.Clear();
				switch (b)
				{
				case 2:
				{
					for (int num8 = 0; num8 < num; num8++)
					{
						list.Add(aReader.ReadVector2());
					}
					break;
				}
				case 3:
				{
					for (int num9 = 0; num9 < num; num9++)
					{
						list.Add(aReader.ReadVector3());
					}
					break;
				}
				case 4:
				{
					for (int num7 = 0; num7 < num; num7++)
					{
						list.Add(aReader.ReadVector4());
					}
					break;
				}
				}
				aTarget.SetUVs(channel, list);
				continue;
			}
			case EChunkID.Submesh:
			{
				MeshTopology topology = (MeshTopology)aReader.ReadByte();
				int num4 = aReader.ReadInt32();
				int[] array4 = new int[num4];
				switch (aReader.ReadByte())
				{
				case 1:
				{
					for (int n = 0; n < num4; n++)
					{
						array4[n] = aReader.ReadByte();
					}
					break;
				}
				case 2:
				{
					for (int num5 = 0; num5 < num4; num5++)
					{
						array4[num5] = aReader.ReadUInt16();
					}
					break;
				}
				case 4:
				{
					for (int m = 0; m < num4; m++)
					{
						array4[m] = aReader.ReadInt32();
					}
					break;
				}
				}
				aTarget.SetIndices(array4, topology, num2++, calculateBounds: false);
				continue;
			}
			case EChunkID.Bindposes:
			{
				int num11 = aReader.ReadInt32();
				Matrix4x4[] array6 = new Matrix4x4[num11];
				for (int num12 = 0; num12 < num11; num12++)
				{
					array6[num12] = aReader.ReadMatrix4x4();
				}
				aTarget.bindposes = array6;
				continue;
			}
			case EChunkID.BlendShape:
			{
				string shapeName = aReader.ReadString();
				int num3 = aReader.ReadInt32();
				if (array2 == null)
				{
					array2 = new Vector3[num];
				}
				if (array3 == null)
				{
					array3 = new Vector3[num];
				}
				for (int j = 0; j < num3; j++)
				{
					float frameWeight = aReader.ReadSingle();
					for (int k = 0; k < num; k++)
					{
						array[k] = aReader.ReadVector3();
						array2[k] = aReader.ReadVector3();
						array3[k] = aReader.ReadVector3();
					}
					aTarget.AddBlendShapeFrame(shapeName, frameWeight, array, array2, array3);
				}
				continue;
			}
			default:
				continue;
			case EChunkID.End:
				break;
			}
			break;
		}
		return aTarget;
	}
}
