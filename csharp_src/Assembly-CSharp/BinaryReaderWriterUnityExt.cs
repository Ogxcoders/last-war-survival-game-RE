using System.IO;
using UnityEngine;

public static class BinaryReaderWriterUnityExt
{
	public static void WriteVector2(this BinaryWriter aWriter, Vector2 aVec)
	{
		aWriter.Write(aVec.x);
		aWriter.Write(aVec.y);
	}

	public static Vector2 ReadVector2(this BinaryReader aReader)
	{
		return new Vector2(aReader.ReadSingle(), aReader.ReadSingle());
	}

	public static void WriteVector3(this BinaryWriter aWriter, Vector3 aVec)
	{
		aWriter.Write(aVec.x);
		aWriter.Write(aVec.y);
		aWriter.Write(aVec.z);
	}

	public static Vector3 ReadVector3(this BinaryReader aReader)
	{
		return new Vector3(aReader.ReadSingle(), aReader.ReadSingle(), aReader.ReadSingle());
	}

	public static void WriteVector4(this BinaryWriter aWriter, Vector4 aVec)
	{
		aWriter.Write(aVec.x);
		aWriter.Write(aVec.y);
		aWriter.Write(aVec.z);
		aWriter.Write(aVec.w);
	}

	public static Vector4 ReadVector4(this BinaryReader aReader)
	{
		return new Vector4(aReader.ReadSingle(), aReader.ReadSingle(), aReader.ReadSingle(), aReader.ReadSingle());
	}

	public static void WriteColor32(this BinaryWriter aWriter, Color32 aCol)
	{
		aWriter.Write(aCol.r);
		aWriter.Write(aCol.g);
		aWriter.Write(aCol.b);
		aWriter.Write(aCol.a);
	}

	public static Color32 ReadColor32(this BinaryReader aReader)
	{
		return new Color32(aReader.ReadByte(), aReader.ReadByte(), aReader.ReadByte(), aReader.ReadByte());
	}

	public static void WriteMatrix4x4(this BinaryWriter aWriter, Matrix4x4 aMat)
	{
		aWriter.Write(aMat.m00);
		aWriter.Write(aMat.m01);
		aWriter.Write(aMat.m02);
		aWriter.Write(aMat.m03);
		aWriter.Write(aMat.m10);
		aWriter.Write(aMat.m11);
		aWriter.Write(aMat.m12);
		aWriter.Write(aMat.m13);
		aWriter.Write(aMat.m20);
		aWriter.Write(aMat.m21);
		aWriter.Write(aMat.m22);
		aWriter.Write(aMat.m23);
		aWriter.Write(aMat.m30);
		aWriter.Write(aMat.m31);
		aWriter.Write(aMat.m32);
		aWriter.Write(aMat.m33);
	}

	public static Matrix4x4 ReadMatrix4x4(this BinaryReader aReader)
	{
		return new Matrix4x4
		{
			m00 = aReader.ReadSingle(),
			m01 = aReader.ReadSingle(),
			m02 = aReader.ReadSingle(),
			m03 = aReader.ReadSingle(),
			m10 = aReader.ReadSingle(),
			m11 = aReader.ReadSingle(),
			m12 = aReader.ReadSingle(),
			m13 = aReader.ReadSingle(),
			m20 = aReader.ReadSingle(),
			m21 = aReader.ReadSingle(),
			m22 = aReader.ReadSingle(),
			m23 = aReader.ReadSingle(),
			m30 = aReader.ReadSingle(),
			m31 = aReader.ReadSingle(),
			m32 = aReader.ReadSingle(),
			m33 = aReader.ReadSingle()
		};
	}

	public static void WriteBoneWeight(this BinaryWriter aWriter, BoneWeight aWeight)
	{
		aWriter.Write(aWeight.boneIndex0);
		aWriter.Write(aWeight.weight0);
		aWriter.Write(aWeight.boneIndex1);
		aWriter.Write(aWeight.weight1);
		aWriter.Write(aWeight.boneIndex2);
		aWriter.Write(aWeight.weight2);
		aWriter.Write(aWeight.boneIndex3);
		aWriter.Write(aWeight.weight3);
	}

	public static BoneWeight ReadBoneWeight(this BinaryReader aReader)
	{
		return new BoneWeight
		{
			boneIndex0 = aReader.ReadInt32(),
			weight0 = aReader.ReadSingle(),
			boneIndex1 = aReader.ReadInt32(),
			weight1 = aReader.ReadSingle(),
			boneIndex2 = aReader.ReadInt32(),
			weight2 = aReader.ReadSingle(),
			boneIndex3 = aReader.ReadInt32(),
			weight3 = aReader.ReadSingle()
		};
	}
}
