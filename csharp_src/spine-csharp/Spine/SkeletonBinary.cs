using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Spine;

public class SkeletonBinary : SkeletonLoader
{
	internal class Vertices
	{
		public int[] bones;

		public float[] vertices;
	}

	internal class SkeletonInput
	{
		private byte[] chars = new byte[32];

		private byte[] bytesBigEndian = new byte[8];

		internal string[] strings;

		private Stream input;

		public SkeletonInput(Stream input)
		{
			this.input = input;
		}

		public int Read()
		{
			return input.ReadByte();
		}

		public byte ReadByte()
		{
			return (byte)input.ReadByte();
		}

		public sbyte ReadSByte()
		{
			int num = input.ReadByte();
			if (num == -1)
			{
				throw new EndOfStreamException();
			}
			return (sbyte)num;
		}

		public bool ReadBoolean()
		{
			return input.ReadByte() != 0;
		}

		public float ReadFloat()
		{
			input.Read(bytesBigEndian, 0, 4);
			chars[3] = bytesBigEndian[0];
			chars[2] = bytesBigEndian[1];
			chars[1] = bytesBigEndian[2];
			chars[0] = bytesBigEndian[3];
			return BitConverter.ToSingle(chars, 0);
		}

		public int ReadInt()
		{
			input.Read(bytesBigEndian, 0, 4);
			return (bytesBigEndian[0] << 24) + (bytesBigEndian[1] << 16) + (bytesBigEndian[2] << 8) + bytesBigEndian[3];
		}

		public long ReadLong()
		{
			input.Read(bytesBigEndian, 0, 8);
			return (long)(((ulong)bytesBigEndian[0] << 56) + ((ulong)bytesBigEndian[1] << 48) + ((ulong)bytesBigEndian[2] << 40) + ((ulong)bytesBigEndian[3] << 32) + ((ulong)bytesBigEndian[4] << 24) + ((ulong)bytesBigEndian[5] << 16) + ((ulong)bytesBigEndian[6] << 8) + bytesBigEndian[7]);
		}

		public int ReadInt(bool optimizePositive)
		{
			int num = input.ReadByte();
			int num2 = num & 0x7F;
			if ((num & 0x80) != 0)
			{
				num = input.ReadByte();
				num2 |= (num & 0x7F) << 7;
				if ((num & 0x80) != 0)
				{
					num = input.ReadByte();
					num2 |= (num & 0x7F) << 14;
					if ((num & 0x80) != 0)
					{
						num = input.ReadByte();
						num2 |= (num & 0x7F) << 21;
						if ((num & 0x80) != 0)
						{
							num2 |= (input.ReadByte() & 0x7F) << 28;
						}
					}
				}
			}
			if (!optimizePositive)
			{
				return (num2 >> 1) ^ -(num2 & 1);
			}
			return num2;
		}

		public string ReadString()
		{
			int num = ReadInt(optimizePositive: true);
			switch (num)
			{
			case 0:
				return null;
			case 1:
				return "";
			default:
			{
				num--;
				byte[] array = chars;
				if (array.Length < num)
				{
					array = new byte[num];
				}
				ReadFully(array, 0, num);
				return Encoding.UTF8.GetString(array, 0, num);
			}
			}
		}

		public string ReadStringRef()
		{
			int num = ReadInt(optimizePositive: true);
			if (num != 0)
			{
				return strings[num - 1];
			}
			return null;
		}

		public void ReadFully(byte[] buffer, int offset, int length)
		{
			while (length > 0)
			{
				int num = input.Read(buffer, offset, length);
				if (num <= 0)
				{
					throw new EndOfStreamException();
				}
				offset += num;
				length -= num;
			}
		}

		public string GetVersionString()
		{
			try
			{
				long position = input.Position;
				ReadLong();
				long position2 = input.Position;
				int num = ReadInt(optimizePositive: true);
				input.Position = position2;
				if (num <= 13)
				{
					string text = ReadString();
					if (char.IsDigit(text[0]))
					{
						return text;
					}
				}
				input.Position = position;
				return GetVersionStringOld3X();
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Stream does not contain a valid binary Skeleton Data.\n" + ex, "input");
			}
		}

		public string GetVersionStringOld3X()
		{
			int num = ReadInt(optimizePositive: true);
			if (num > 1)
			{
				input.Position += num - 1;
			}
			num = ReadInt(optimizePositive: true);
			if (num > 1)
			{
				num--;
				byte[] array = new byte[num];
				ReadFully(array, 0, num);
				return Encoding.UTF8.GetString(array, 0, num);
			}
			return null;
		}
	}

	public const int BONE_ROTATE = 0;

	public const int BONE_TRANSLATE = 1;

	public const int BONE_TRANSLATEX = 2;

	public const int BONE_TRANSLATEY = 3;

	public const int BONE_SCALE = 4;

	public const int BONE_SCALEX = 5;

	public const int BONE_SCALEY = 6;

	public const int BONE_SHEAR = 7;

	public const int BONE_SHEARX = 8;

	public const int BONE_SHEARY = 9;

	public const int SLOT_ATTACHMENT = 0;

	public const int SLOT_RGBA = 1;

	public const int SLOT_RGB = 2;

	public const int SLOT_RGBA2 = 3;

	public const int SLOT_RGB2 = 4;

	public const int SLOT_ALPHA = 5;

	public const int PATH_POSITION = 0;

	public const int PATH_SPACING = 1;

	public const int PATH_MIX = 2;

	public const int CURVE_LINEAR = 0;

	public const int CURVE_STEPPED = 1;

	public const int CURVE_BEZIER = 2;

	public static readonly TransformMode[] TransformModeValues = new TransformMode[5]
	{
		TransformMode.Normal,
		TransformMode.OnlyTranslation,
		TransformMode.NoRotationOrReflection,
		TransformMode.NoScale,
		TransformMode.NoScaleOrReflection
	};

	public SkeletonBinary(AttachmentLoader attachmentLoader)
		: base(attachmentLoader)
	{
	}

	public SkeletonBinary(params Atlas[] atlasArray)
		: base(atlasArray)
	{
	}

	public override SkeletonData ReadSkeletonData(string path)
	{
		using FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		SkeletonData skeletonData = ReadSkeletonData(file);
		skeletonData.name = Path.GetFileNameWithoutExtension(path);
		return skeletonData;
	}

	public static string GetVersionString(Stream file)
	{
		if (file == null)
		{
			throw new ArgumentNullException("file");
		}
		return new SkeletonInput(file).GetVersionString();
	}

	public SkeletonData ReadSkeletonData(Stream file)
	{
		if (file == null)
		{
			throw new ArgumentNullException("file");
		}
		float num = scale;
		SkeletonData skeletonData = new SkeletonData();
		SkeletonInput skeletonInput = new SkeletonInput(file);
		long num2 = skeletonInput.ReadLong();
		skeletonData.hash = ((num2 == 0L) ? null : num2.ToString());
		skeletonData.version = skeletonInput.ReadString();
		if (skeletonData.version.Length == 0)
		{
			skeletonData.version = null;
		}
		if (skeletonData.version.Length > 13)
		{
			return null;
		}
		skeletonData.x = skeletonInput.ReadFloat();
		skeletonData.y = skeletonInput.ReadFloat();
		skeletonData.width = skeletonInput.ReadFloat();
		skeletonData.height = skeletonInput.ReadFloat();
		bool flag = skeletonInput.ReadBoolean();
		if (flag)
		{
			skeletonData.fps = skeletonInput.ReadFloat();
			skeletonData.imagesPath = skeletonInput.ReadString();
			if (string.IsNullOrEmpty(skeletonData.imagesPath))
			{
				skeletonData.imagesPath = null;
			}
			skeletonData.audioPath = skeletonInput.ReadString();
			if (string.IsNullOrEmpty(skeletonData.audioPath))
			{
				skeletonData.audioPath = null;
			}
		}
		int num3;
		object[] array = (skeletonInput.strings = new string[num3 = skeletonInput.ReadInt(optimizePositive: true)]);
		for (int i = 0; i < num3; i++)
		{
			array[i] = skeletonInput.ReadString();
		}
		BoneData[] items = skeletonData.bones.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		for (int j = 0; j < num3; j++)
		{
			string name = skeletonInput.ReadString();
			BoneData parent = ((j == 0) ? null : items[skeletonInput.ReadInt(optimizePositive: true)]);
			BoneData boneData = new BoneData(j, name, parent);
			boneData.rotation = skeletonInput.ReadFloat();
			boneData.x = skeletonInput.ReadFloat() * num;
			boneData.y = skeletonInput.ReadFloat() * num;
			boneData.scaleX = skeletonInput.ReadFloat();
			boneData.scaleY = skeletonInput.ReadFloat();
			boneData.shearX = skeletonInput.ReadFloat();
			boneData.shearY = skeletonInput.ReadFloat();
			boneData.Length = skeletonInput.ReadFloat() * num;
			boneData.transformMode = TransformModeValues[skeletonInput.ReadInt(optimizePositive: true)];
			boneData.skinRequired = skeletonInput.ReadBoolean();
			if (flag)
			{
				skeletonInput.ReadInt();
			}
			items[j] = boneData;
		}
		SlotData[] items2 = skeletonData.slots.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		for (int k = 0; k < num3; k++)
		{
			string name2 = skeletonInput.ReadString();
			BoneData boneData2 = items[skeletonInput.ReadInt(optimizePositive: true)];
			SlotData slotData = new SlotData(k, name2, boneData2);
			int num4 = skeletonInput.ReadInt();
			slotData.r = (float)((num4 & 0xFF000000u) >> 24) / 255f;
			slotData.g = (float)((num4 & 0xFF0000) >> 16) / 255f;
			slotData.b = (float)((num4 & 0xFF00) >> 8) / 255f;
			slotData.a = (float)(num4 & 0xFF) / 255f;
			int num5 = skeletonInput.ReadInt();
			if (num5 != -1)
			{
				slotData.hasSecondColor = true;
				slotData.r2 = (float)((num5 & 0xFF0000) >> 16) / 255f;
				slotData.g2 = (float)((num5 & 0xFF00) >> 8) / 255f;
				slotData.b2 = (float)(num5 & 0xFF) / 255f;
			}
			slotData.attachmentName = skeletonInput.ReadStringRef();
			slotData.blendMode = (BlendMode)skeletonInput.ReadInt(optimizePositive: true);
			items2[k] = slotData;
		}
		object[] items3 = skeletonData.ikConstraints.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		array = items3;
		for (int l = 0; l < num3; l++)
		{
			IkConstraintData ikConstraintData = new IkConstraintData(skeletonInput.ReadString());
			ikConstraintData.order = skeletonInput.ReadInt(optimizePositive: true);
			ikConstraintData.skinRequired = skeletonInput.ReadBoolean();
			int num6;
			BoneData[] items4 = ikConstraintData.bones.Resize(num6 = skeletonInput.ReadInt(optimizePositive: true)).Items;
			for (int m = 0; m < num6; m++)
			{
				items4[m] = items[skeletonInput.ReadInt(optimizePositive: true)];
			}
			ikConstraintData.target = items[skeletonInput.ReadInt(optimizePositive: true)];
			ikConstraintData.mix = skeletonInput.ReadFloat();
			ikConstraintData.softness = skeletonInput.ReadFloat() * num;
			ikConstraintData.bendDirection = skeletonInput.ReadSByte();
			ikConstraintData.compress = skeletonInput.ReadBoolean();
			ikConstraintData.stretch = skeletonInput.ReadBoolean();
			ikConstraintData.uniform = skeletonInput.ReadBoolean();
			array[l] = ikConstraintData;
		}
		items3 = skeletonData.transformConstraints.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		array = items3;
		for (int n = 0; n < num3; n++)
		{
			TransformConstraintData transformConstraintData = new TransformConstraintData(skeletonInput.ReadString());
			transformConstraintData.order = skeletonInput.ReadInt(optimizePositive: true);
			transformConstraintData.skinRequired = skeletonInput.ReadBoolean();
			int num7;
			BoneData[] items5 = transformConstraintData.bones.Resize(num7 = skeletonInput.ReadInt(optimizePositive: true)).Items;
			for (int num8 = 0; num8 < num7; num8++)
			{
				items5[num8] = items[skeletonInput.ReadInt(optimizePositive: true)];
			}
			transformConstraintData.target = items[skeletonInput.ReadInt(optimizePositive: true)];
			transformConstraintData.local = skeletonInput.ReadBoolean();
			transformConstraintData.relative = skeletonInput.ReadBoolean();
			transformConstraintData.offsetRotation = skeletonInput.ReadFloat();
			transformConstraintData.offsetX = skeletonInput.ReadFloat() * num;
			transformConstraintData.offsetY = skeletonInput.ReadFloat() * num;
			transformConstraintData.offsetScaleX = skeletonInput.ReadFloat();
			transformConstraintData.offsetScaleY = skeletonInput.ReadFloat();
			transformConstraintData.offsetShearY = skeletonInput.ReadFloat();
			transformConstraintData.mixRotate = skeletonInput.ReadFloat();
			transformConstraintData.mixX = skeletonInput.ReadFloat();
			transformConstraintData.mixY = skeletonInput.ReadFloat();
			transformConstraintData.mixScaleX = skeletonInput.ReadFloat();
			transformConstraintData.mixScaleY = skeletonInput.ReadFloat();
			transformConstraintData.mixShearY = skeletonInput.ReadFloat();
			array[n] = transformConstraintData;
		}
		items3 = skeletonData.pathConstraints.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		array = items3;
		for (int num9 = 0; num9 < num3; num9++)
		{
			PathConstraintData pathConstraintData = new PathConstraintData(skeletonInput.ReadString());
			pathConstraintData.order = skeletonInput.ReadInt(optimizePositive: true);
			pathConstraintData.skinRequired = skeletonInput.ReadBoolean();
			int num10;
			items3 = pathConstraintData.bones.Resize(num10 = skeletonInput.ReadInt(optimizePositive: true)).Items;
			object[] array2 = items3;
			for (int num11 = 0; num11 < num10; num11++)
			{
				array2[num11] = items[skeletonInput.ReadInt(optimizePositive: true)];
			}
			pathConstraintData.target = items2[skeletonInput.ReadInt(optimizePositive: true)];
			pathConstraintData.positionMode = (PositionMode)Enum.GetValues(typeof(PositionMode)).GetValue(skeletonInput.ReadInt(optimizePositive: true));
			pathConstraintData.spacingMode = (SpacingMode)Enum.GetValues(typeof(SpacingMode)).GetValue(skeletonInput.ReadInt(optimizePositive: true));
			pathConstraintData.rotateMode = (RotateMode)Enum.GetValues(typeof(RotateMode)).GetValue(skeletonInput.ReadInt(optimizePositive: true));
			pathConstraintData.offsetRotation = skeletonInput.ReadFloat();
			pathConstraintData.position = skeletonInput.ReadFloat();
			if (pathConstraintData.positionMode == PositionMode.Fixed)
			{
				pathConstraintData.position *= num;
			}
			pathConstraintData.spacing = skeletonInput.ReadFloat();
			if (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed)
			{
				pathConstraintData.spacing *= num;
			}
			pathConstraintData.mixRotate = skeletonInput.ReadFloat();
			pathConstraintData.mixX = skeletonInput.ReadFloat();
			pathConstraintData.mixY = skeletonInput.ReadFloat();
			array[num9] = pathConstraintData;
		}
		Skin skin = ReadSkin(skeletonInput, skeletonData, defaultSkin: true, flag);
		if (skin != null)
		{
			skeletonData.defaultSkin = skin;
			skeletonData.skins.Add(skin);
		}
		int num12 = skeletonData.skins.Count;
		items3 = skeletonData.skins.Resize(num3 = num12 + skeletonInput.ReadInt(optimizePositive: true)).Items;
		array = items3;
		for (; num12 < num3; num12++)
		{
			array[num12] = ReadSkin(skeletonInput, skeletonData, defaultSkin: false, flag);
		}
		num3 = linkedMeshes.Count;
		for (int num13 = 0; num13 < num3; num13++)
		{
			LinkedMesh linkedMesh = linkedMeshes[num13];
			Attachment attachment = (((linkedMesh.skin == null) ? skeletonData.DefaultSkin : skeletonData.FindSkin(linkedMesh.skin)) ?? throw new Exception("Skin not found: " + linkedMesh.skin)).GetAttachment(linkedMesh.slotIndex, linkedMesh.parent);
			if (attachment == null)
			{
				throw new Exception("Parent mesh not found: " + linkedMesh.parent);
			}
			linkedMesh.mesh.DeformAttachment = (linkedMesh.inheritDeform ? ((VertexAttachment)attachment) : linkedMesh.mesh);
			linkedMesh.mesh.ParentMesh = (MeshAttachment)attachment;
			linkedMesh.mesh.UpdateUVs();
		}
		linkedMeshes.Clear();
		items3 = skeletonData.events.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		array = items3;
		for (int num14 = 0; num14 < num3; num14++)
		{
			EventData eventData = new EventData(skeletonInput.ReadStringRef());
			eventData.Int = skeletonInput.ReadInt(optimizePositive: false);
			eventData.Float = skeletonInput.ReadFloat();
			eventData.String = skeletonInput.ReadString();
			eventData.AudioPath = skeletonInput.ReadString();
			if (eventData.AudioPath != null)
			{
				eventData.Volume = skeletonInput.ReadFloat();
				eventData.Balance = skeletonInput.ReadFloat();
			}
			array[num14] = eventData;
		}
		items3 = skeletonData.animations.Resize(num3 = skeletonInput.ReadInt(optimizePositive: true)).Items;
		array = items3;
		for (int num15 = 0; num15 < num3; num15++)
		{
			array[num15] = ReadAnimation(skeletonInput.ReadString(), skeletonInput, skeletonData);
		}
		return skeletonData;
	}

	private Skin ReadSkin(SkeletonInput input, SkeletonData skeletonData, bool defaultSkin, bool nonessential)
	{
		int num;
		Skin skin;
		if (defaultSkin)
		{
			num = input.ReadInt(optimizePositive: true);
			if (num == 0)
			{
				return null;
			}
			skin = new Skin("default");
		}
		else
		{
			skin = new Skin(input.ReadStringRef());
			object[] items = skin.bones.Resize(input.ReadInt(optimizePositive: true)).Items;
			object[] array = items;
			BoneData[] items2 = skeletonData.bones.Items;
			int i = 0;
			for (int count = skin.bones.Count; i < count; i++)
			{
				array[i] = items2[input.ReadInt(optimizePositive: true)];
			}
			IkConstraintData[] items3 = skeletonData.ikConstraints.Items;
			int j = 0;
			for (int num2 = input.ReadInt(optimizePositive: true); j < num2; j++)
			{
				skin.constraints.Add(items3[input.ReadInt(optimizePositive: true)]);
			}
			TransformConstraintData[] items4 = skeletonData.transformConstraints.Items;
			int k = 0;
			for (int num3 = input.ReadInt(optimizePositive: true); k < num3; k++)
			{
				skin.constraints.Add(items4[input.ReadInt(optimizePositive: true)]);
			}
			PathConstraintData[] items5 = skeletonData.pathConstraints.Items;
			int l = 0;
			for (int num4 = input.ReadInt(optimizePositive: true); l < num4; l++)
			{
				skin.constraints.Add(items5[input.ReadInt(optimizePositive: true)]);
			}
			skin.constraints.TrimExcess();
			num = input.ReadInt(optimizePositive: true);
		}
		for (int m = 0; m < num; m++)
		{
			int slotIndex = input.ReadInt(optimizePositive: true);
			int n = 0;
			for (int num5 = input.ReadInt(optimizePositive: true); n < num5; n++)
			{
				string text = input.ReadStringRef();
				Attachment attachment = ReadAttachment(input, skeletonData, skin, slotIndex, text, nonessential);
				if (attachment != null)
				{
					skin.SetAttachment(slotIndex, text, attachment);
				}
			}
		}
		return skin;
	}

	private Attachment ReadAttachment(SkeletonInput input, SkeletonData skeletonData, Skin skin, int slotIndex, string attachmentName, bool nonessential)
	{
		float num = scale;
		string text = input.ReadStringRef();
		if (text == null)
		{
			text = attachmentName;
		}
		switch ((AttachmentType)input.ReadByte())
		{
		case AttachmentType.Region:
		{
			string text2 = input.ReadStringRef();
			float rotation = input.ReadFloat();
			float num4 = input.ReadFloat();
			float num5 = input.ReadFloat();
			float scaleX = input.ReadFloat();
			float scaleY = input.ReadFloat();
			float num6 = input.ReadFloat();
			float num7 = input.ReadFloat();
			int num8 = input.ReadInt();
			if (text2 == null)
			{
				text2 = text;
			}
			RegionAttachment regionAttachment = attachmentLoader.NewRegionAttachment(skin, text, text2);
			if (regionAttachment == null)
			{
				return null;
			}
			regionAttachment.Path = text2;
			regionAttachment.x = num4 * num;
			regionAttachment.y = num5 * num;
			regionAttachment.scaleX = scaleX;
			regionAttachment.scaleY = scaleY;
			regionAttachment.rotation = rotation;
			regionAttachment.width = num6 * num;
			regionAttachment.height = num7 * num;
			regionAttachment.r = (float)((num8 & 0xFF000000u) >> 24) / 255f;
			regionAttachment.g = (float)((num8 & 0xFF0000) >> 16) / 255f;
			regionAttachment.b = (float)((num8 & 0xFF00) >> 8) / 255f;
			regionAttachment.a = (float)(num8 & 0xFF) / 255f;
			regionAttachment.UpdateOffset();
			return regionAttachment;
		}
		case AttachmentType.Boundingbox:
		{
			int num21 = input.ReadInt(optimizePositive: true);
			Vertices vertices4 = ReadVertices(input, num21);
			if (nonessential)
			{
				input.ReadInt();
			}
			BoundingBoxAttachment boundingBoxAttachment = attachmentLoader.NewBoundingBoxAttachment(skin, text);
			if (boundingBoxAttachment == null)
			{
				return null;
			}
			boundingBoxAttachment.worldVerticesLength = num21 << 1;
			boundingBoxAttachment.vertices = vertices4.vertices;
			boundingBoxAttachment.bones = vertices4.bones;
			return boundingBoxAttachment;
		}
		case AttachmentType.Mesh:
		{
			string text3 = input.ReadStringRef();
			int num9 = input.ReadInt();
			int num10 = input.ReadInt(optimizePositive: true);
			float[] regionUVs = ReadFloatArray(input, num10 << 1, 1f);
			int[] triangles = ReadShortArray(input);
			Vertices vertices2 = ReadVertices(input, num10);
			int num11 = input.ReadInt(optimizePositive: true);
			int[] edges = null;
			float num12 = 0f;
			float num13 = 0f;
			if (nonessential)
			{
				edges = ReadShortArray(input);
				num12 = input.ReadFloat();
				num13 = input.ReadFloat();
			}
			if (text3 == null)
			{
				text3 = text;
			}
			MeshAttachment meshAttachment = attachmentLoader.NewMeshAttachment(skin, text, text3);
			if (meshAttachment == null)
			{
				return null;
			}
			meshAttachment.Path = text3;
			meshAttachment.r = (float)((num9 & 0xFF000000u) >> 24) / 255f;
			meshAttachment.g = (float)((num9 & 0xFF0000) >> 16) / 255f;
			meshAttachment.b = (float)((num9 & 0xFF00) >> 8) / 255f;
			meshAttachment.a = (float)(num9 & 0xFF) / 255f;
			meshAttachment.bones = vertices2.bones;
			meshAttachment.vertices = vertices2.vertices;
			meshAttachment.WorldVerticesLength = num10 << 1;
			meshAttachment.triangles = triangles;
			meshAttachment.regionUVs = regionUVs;
			meshAttachment.UpdateUVs();
			meshAttachment.HullLength = num11 << 1;
			if (nonessential)
			{
				meshAttachment.Edges = edges;
				meshAttachment.Width = num12 * num;
				meshAttachment.Height = num13 * num;
			}
			return meshAttachment;
		}
		case AttachmentType.Linkedmesh:
		{
			string text4 = input.ReadStringRef();
			int num14 = input.ReadInt();
			string skin2 = input.ReadStringRef();
			string parent = input.ReadStringRef();
			bool inheritDeform = input.ReadBoolean();
			float num15 = 0f;
			float num16 = 0f;
			if (nonessential)
			{
				num15 = input.ReadFloat();
				num16 = input.ReadFloat();
			}
			if (text4 == null)
			{
				text4 = text;
			}
			MeshAttachment meshAttachment2 = attachmentLoader.NewMeshAttachment(skin, text, text4);
			if (meshAttachment2 == null)
			{
				return null;
			}
			meshAttachment2.Path = text4;
			meshAttachment2.r = (float)((num14 & 0xFF000000u) >> 24) / 255f;
			meshAttachment2.g = (float)((num14 & 0xFF0000) >> 16) / 255f;
			meshAttachment2.b = (float)((num14 & 0xFF00) >> 8) / 255f;
			meshAttachment2.a = (float)(num14 & 0xFF) / 255f;
			if (nonessential)
			{
				meshAttachment2.Width = num15 * num;
				meshAttachment2.Height = num16 * num;
			}
			linkedMeshes.Add(new LinkedMesh(meshAttachment2, skin2, slotIndex, parent, inheritDeform));
			return meshAttachment2;
		}
		case AttachmentType.Path:
		{
			bool closed = input.ReadBoolean();
			bool constantSpeed = input.ReadBoolean();
			int num17 = input.ReadInt(optimizePositive: true);
			Vertices vertices3 = ReadVertices(input, num17);
			float[] array = new float[num17 / 3];
			int i = 0;
			for (int num18 = array.Length; i < num18; i++)
			{
				array[i] = input.ReadFloat() * num;
			}
			if (nonessential)
			{
				input.ReadInt();
			}
			PathAttachment pathAttachment = attachmentLoader.NewPathAttachment(skin, text);
			if (pathAttachment == null)
			{
				return null;
			}
			pathAttachment.closed = closed;
			pathAttachment.constantSpeed = constantSpeed;
			pathAttachment.worldVerticesLength = num17 << 1;
			pathAttachment.vertices = vertices3.vertices;
			pathAttachment.bones = vertices3.bones;
			pathAttachment.lengths = array;
			return pathAttachment;
		}
		case AttachmentType.Point:
		{
			float rotation2 = input.ReadFloat();
			float num19 = input.ReadFloat();
			float num20 = input.ReadFloat();
			if (nonessential)
			{
				input.ReadInt();
			}
			PointAttachment pointAttachment = attachmentLoader.NewPointAttachment(skin, text);
			if (pointAttachment == null)
			{
				return null;
			}
			pointAttachment.x = num19 * num;
			pointAttachment.y = num20 * num;
			pointAttachment.rotation = rotation2;
			return pointAttachment;
		}
		case AttachmentType.Clipping:
		{
			int num2 = input.ReadInt(optimizePositive: true);
			int num3 = input.ReadInt(optimizePositive: true);
			Vertices vertices = ReadVertices(input, num3);
			if (nonessential)
			{
				input.ReadInt();
			}
			ClippingAttachment clippingAttachment = attachmentLoader.NewClippingAttachment(skin, text);
			if (clippingAttachment == null)
			{
				return null;
			}
			clippingAttachment.EndSlot = skeletonData.slots.Items[num2];
			clippingAttachment.worldVerticesLength = num3 << 1;
			clippingAttachment.vertices = vertices.vertices;
			clippingAttachment.bones = vertices.bones;
			return clippingAttachment;
		}
		default:
			return null;
		}
	}

	private Vertices ReadVertices(SkeletonInput input, int vertexCount)
	{
		float num = scale;
		int num2 = vertexCount << 1;
		Vertices vertices = new Vertices();
		if (!input.ReadBoolean())
		{
			vertices.vertices = ReadFloatArray(input, num2, num);
			return vertices;
		}
		ExposedList<float> exposedList = new ExposedList<float>(num2 * 3 * 3);
		ExposedList<int> exposedList2 = new ExposedList<int>(num2 * 3);
		for (int i = 0; i < vertexCount; i++)
		{
			int num3 = input.ReadInt(optimizePositive: true);
			exposedList2.Add(num3);
			for (int j = 0; j < num3; j++)
			{
				exposedList2.Add(input.ReadInt(optimizePositive: true));
				exposedList.Add(input.ReadFloat() * num);
				exposedList.Add(input.ReadFloat() * num);
				exposedList.Add(input.ReadFloat());
			}
		}
		vertices.vertices = exposedList.ToArray();
		vertices.bones = exposedList2.ToArray();
		return vertices;
	}

	private float[] ReadFloatArray(SkeletonInput input, int n, float scale)
	{
		float[] array = new float[n];
		if (scale == 1f)
		{
			for (int i = 0; i < n; i++)
			{
				array[i] = input.ReadFloat();
			}
		}
		else
		{
			for (int j = 0; j < n; j++)
			{
				array[j] = input.ReadFloat() * scale;
			}
		}
		return array;
	}

	private int[] ReadShortArray(SkeletonInput input)
	{
		int num = input.ReadInt(optimizePositive: true);
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (input.ReadByte() << 8) | input.ReadByte();
		}
		return array;
	}

	private Animation ReadAnimation(string name, SkeletonInput input, SkeletonData skeletonData)
	{
		ExposedList<Timeline> exposedList = new ExposedList<Timeline>(input.ReadInt(optimizePositive: true));
		float num = scale;
		int i = 0;
		for (int num2 = input.ReadInt(optimizePositive: true); i < num2; i++)
		{
			int slotIndex = input.ReadInt(optimizePositive: true);
			int j = 0;
			for (int num3 = input.ReadInt(optimizePositive: true); j < num3; j++)
			{
				int num4 = input.ReadByte();
				int num5 = input.ReadInt(optimizePositive: true);
				int num6 = num5 - 1;
				switch (num4)
				{
				case 0:
				{
					AttachmentTimeline attachmentTimeline = new AttachmentTimeline(num5, slotIndex);
					for (int k = 0; k < num5; k++)
					{
						attachmentTimeline.SetFrame(k, input.ReadFloat(), input.ReadStringRef());
					}
					exposedList.Add(attachmentTimeline);
					break;
				}
				case 1:
				{
					RGBATimeline rGBATimeline = new RGBATimeline(num5, input.ReadInt(optimizePositive: true), slotIndex);
					float num31 = input.ReadFloat();
					float num32 = (float)input.Read() / 255f;
					float num33 = (float)input.Read() / 255f;
					float num34 = (float)input.Read() / 255f;
					float num35 = (float)input.Read() / 255f;
					int num36 = 0;
					int num37 = 0;
					while (true)
					{
						rGBATimeline.SetFrame(num36, num31, num32, num33, num34, num35);
						if (num36 == num6)
						{
							break;
						}
						float num38 = input.ReadFloat();
						float num39 = (float)input.Read() / 255f;
						float num40 = (float)input.Read() / 255f;
						float num41 = (float)input.Read() / 255f;
						float num42 = (float)input.Read() / 255f;
						switch (input.ReadByte())
						{
						case 1:
							rGBATimeline.SetStepped(num36);
							break;
						case 2:
							SetBezier(input, rGBATimeline, num37++, num36, 0, num31, num38, num32, num39, 1f);
							SetBezier(input, rGBATimeline, num37++, num36, 1, num31, num38, num33, num40, 1f);
							SetBezier(input, rGBATimeline, num37++, num36, 2, num31, num38, num34, num41, 1f);
							SetBezier(input, rGBATimeline, num37++, num36, 3, num31, num38, num35, num42, 1f);
							break;
						}
						num31 = num38;
						num32 = num39;
						num33 = num40;
						num34 = num41;
						num35 = num42;
						num36++;
					}
					exposedList.Add(rGBATimeline);
					break;
				}
				case 2:
				{
					RGBTimeline rGBTimeline = new RGBTimeline(num5, input.ReadInt(optimizePositive: true), slotIndex);
					float num59 = input.ReadFloat();
					float num60 = (float)input.Read() / 255f;
					float num61 = (float)input.Read() / 255f;
					float num62 = (float)input.Read() / 255f;
					int num63 = 0;
					int num64 = 0;
					while (true)
					{
						rGBTimeline.SetFrame(num63, num59, num60, num61, num62);
						if (num63 == num6)
						{
							break;
						}
						float num65 = input.ReadFloat();
						float num66 = (float)input.Read() / 255f;
						float num67 = (float)input.Read() / 255f;
						float num68 = (float)input.Read() / 255f;
						switch (input.ReadByte())
						{
						case 1:
							rGBTimeline.SetStepped(num63);
							break;
						case 2:
							SetBezier(input, rGBTimeline, num64++, num63, 0, num59, num65, num60, num66, 1f);
							SetBezier(input, rGBTimeline, num64++, num63, 1, num59, num65, num61, num67, 1f);
							SetBezier(input, rGBTimeline, num64++, num63, 2, num59, num65, num62, num68, 1f);
							break;
						}
						num59 = num65;
						num60 = num66;
						num61 = num67;
						num62 = num68;
						num63++;
					}
					exposedList.Add(rGBTimeline);
					break;
				}
				case 3:
				{
					RGBA2Timeline rGBA2Timeline = new RGBA2Timeline(num5, input.ReadInt(optimizePositive: true), slotIndex);
					float num13 = input.ReadFloat();
					float num14 = (float)input.Read() / 255f;
					float num15 = (float)input.Read() / 255f;
					float num16 = (float)input.Read() / 255f;
					float num17 = (float)input.Read() / 255f;
					float num18 = (float)input.Read() / 255f;
					float num19 = (float)input.Read() / 255f;
					float num20 = (float)input.Read() / 255f;
					int num21 = 0;
					int num22 = 0;
					while (true)
					{
						rGBA2Timeline.SetFrame(num21, num13, num14, num15, num16, num17, num18, num19, num20);
						if (num21 == num6)
						{
							break;
						}
						float num23 = input.ReadFloat();
						float num24 = (float)input.Read() / 255f;
						float num25 = (float)input.Read() / 255f;
						float num26 = (float)input.Read() / 255f;
						float num27 = (float)input.Read() / 255f;
						float num28 = (float)input.Read() / 255f;
						float num29 = (float)input.Read() / 255f;
						float num30 = (float)input.Read() / 255f;
						switch (input.ReadByte())
						{
						case 1:
							rGBA2Timeline.SetStepped(num21);
							break;
						case 2:
							SetBezier(input, rGBA2Timeline, num22++, num21, 0, num13, num23, num14, num24, 1f);
							SetBezier(input, rGBA2Timeline, num22++, num21, 1, num13, num23, num15, num25, 1f);
							SetBezier(input, rGBA2Timeline, num22++, num21, 2, num13, num23, num16, num26, 1f);
							SetBezier(input, rGBA2Timeline, num22++, num21, 3, num13, num23, num17, num27, 1f);
							SetBezier(input, rGBA2Timeline, num22++, num21, 4, num13, num23, num18, num28, 1f);
							SetBezier(input, rGBA2Timeline, num22++, num21, 5, num13, num23, num19, num29, 1f);
							SetBezier(input, rGBA2Timeline, num22++, num21, 6, num13, num23, num20, num30, 1f);
							break;
						}
						num13 = num23;
						num14 = num24;
						num15 = num25;
						num16 = num26;
						num17 = num27;
						num18 = num28;
						num19 = num29;
						num20 = num30;
						num21++;
					}
					exposedList.Add(rGBA2Timeline);
					break;
				}
				case 4:
				{
					RGB2Timeline rGB2Timeline = new RGB2Timeline(num5, input.ReadInt(optimizePositive: true), slotIndex);
					float num43 = input.ReadFloat();
					float num44 = (float)input.Read() / 255f;
					float num45 = (float)input.Read() / 255f;
					float num46 = (float)input.Read() / 255f;
					float num47 = (float)input.Read() / 255f;
					float num48 = (float)input.Read() / 255f;
					float num49 = (float)input.Read() / 255f;
					int num50 = 0;
					int num51 = 0;
					while (true)
					{
						rGB2Timeline.SetFrame(num50, num43, num44, num45, num46, num47, num48, num49);
						if (num50 == num6)
						{
							break;
						}
						float num52 = input.ReadFloat();
						float num53 = (float)input.Read() / 255f;
						float num54 = (float)input.Read() / 255f;
						float num55 = (float)input.Read() / 255f;
						float num56 = (float)input.Read() / 255f;
						float num57 = (float)input.Read() / 255f;
						float num58 = (float)input.Read() / 255f;
						switch (input.ReadByte())
						{
						case 1:
							rGB2Timeline.SetStepped(num50);
							break;
						case 2:
							SetBezier(input, rGB2Timeline, num51++, num50, 0, num43, num52, num44, num53, 1f);
							SetBezier(input, rGB2Timeline, num51++, num50, 1, num43, num52, num45, num54, 1f);
							SetBezier(input, rGB2Timeline, num51++, num50, 2, num43, num52, num46, num55, 1f);
							SetBezier(input, rGB2Timeline, num51++, num50, 3, num43, num52, num47, num56, 1f);
							SetBezier(input, rGB2Timeline, num51++, num50, 4, num43, num52, num48, num57, 1f);
							SetBezier(input, rGB2Timeline, num51++, num50, 5, num43, num52, num49, num58, 1f);
							break;
						}
						num43 = num52;
						num44 = num53;
						num45 = num54;
						num46 = num55;
						num47 = num56;
						num48 = num57;
						num49 = num58;
						num50++;
					}
					exposedList.Add(rGB2Timeline);
					break;
				}
				case 5:
				{
					AlphaTimeline alphaTimeline = new AlphaTimeline(num5, input.ReadInt(optimizePositive: true), slotIndex);
					float num7 = input.ReadFloat();
					float num8 = (float)input.Read() / 255f;
					int num9 = 0;
					int num10 = 0;
					while (true)
					{
						alphaTimeline.SetFrame(num9, num7, num8);
						if (num9 == num6)
						{
							break;
						}
						float num11 = input.ReadFloat();
						float num12 = (float)input.Read() / 255f;
						switch (input.ReadByte())
						{
						case 1:
							alphaTimeline.SetStepped(num9);
							break;
						case 2:
							SetBezier(input, alphaTimeline, num10++, num9, 0, num7, num11, num8, num12, 1f);
							break;
						}
						num7 = num11;
						num8 = num12;
						num9++;
					}
					exposedList.Add(alphaTimeline);
					break;
				}
				}
			}
		}
		int l = 0;
		for (int num69 = input.ReadInt(optimizePositive: true); l < num69; l++)
		{
			int boneIndex = input.ReadInt(optimizePositive: true);
			int m = 0;
			for (int num70 = input.ReadInt(optimizePositive: true); m < num70; m++)
			{
				int num71 = input.ReadByte();
				int frameCount = input.ReadInt(optimizePositive: true);
				int bezierCount = input.ReadInt(optimizePositive: true);
				switch (num71)
				{
				case 0:
					exposedList.Add(ReadTimeline(input, new RotateTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				case 1:
					exposedList.Add(ReadTimeline(input, new TranslateTimeline(frameCount, bezierCount, boneIndex), num));
					break;
				case 2:
					exposedList.Add(ReadTimeline(input, new TranslateXTimeline(frameCount, bezierCount, boneIndex), num));
					break;
				case 3:
					exposedList.Add(ReadTimeline(input, new TranslateYTimeline(frameCount, bezierCount, boneIndex), num));
					break;
				case 4:
					exposedList.Add(ReadTimeline(input, new ScaleTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				case 5:
					exposedList.Add(ReadTimeline(input, new ScaleXTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				case 6:
					exposedList.Add(ReadTimeline(input, new ScaleYTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				case 7:
					exposedList.Add(ReadTimeline(input, new ShearTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				case 8:
					exposedList.Add(ReadTimeline(input, new ShearXTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				case 9:
					exposedList.Add(ReadTimeline(input, new ShearYTimeline(frameCount, bezierCount, boneIndex), 1f));
					break;
				}
			}
		}
		int n = 0;
		for (int num72 = input.ReadInt(optimizePositive: true); n < num72; n++)
		{
			int ikConstraintIndex = input.ReadInt(optimizePositive: true);
			int num73 = input.ReadInt(optimizePositive: true);
			int num74 = num73 - 1;
			IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(num73, input.ReadInt(optimizePositive: true), ikConstraintIndex);
			float num75 = input.ReadFloat();
			float num76 = input.ReadFloat();
			float num77 = input.ReadFloat() * num;
			int num78 = 0;
			int num79 = 0;
			while (true)
			{
				ikConstraintTimeline.SetFrame(num78, num75, num76, num77, input.ReadSByte(), input.ReadBoolean(), input.ReadBoolean());
				if (num78 == num74)
				{
					break;
				}
				float num80 = input.ReadFloat();
				float num81 = input.ReadFloat();
				float num82 = input.ReadFloat() * num;
				switch (input.ReadByte())
				{
				case 1:
					ikConstraintTimeline.SetStepped(num78);
					break;
				case 2:
					SetBezier(input, ikConstraintTimeline, num79++, num78, 0, num75, num80, num76, num81, 1f);
					SetBezier(input, ikConstraintTimeline, num79++, num78, 1, num75, num80, num77, num82, num);
					break;
				}
				num75 = num80;
				num76 = num81;
				num77 = num82;
				num78++;
			}
			exposedList.Add(ikConstraintTimeline);
		}
		int num83 = 0;
		for (int num84 = input.ReadInt(optimizePositive: true); num83 < num84; num83++)
		{
			int transformConstraintIndex = input.ReadInt(optimizePositive: true);
			int num85 = input.ReadInt(optimizePositive: true);
			int num86 = num85 - 1;
			TransformConstraintTimeline transformConstraintTimeline = new TransformConstraintTimeline(num85, input.ReadInt(optimizePositive: true), transformConstraintIndex);
			float num87 = input.ReadFloat();
			float num88 = input.ReadFloat();
			float num89 = input.ReadFloat();
			float num90 = input.ReadFloat();
			float num91 = input.ReadFloat();
			float num92 = input.ReadFloat();
			float num93 = input.ReadFloat();
			int num94 = 0;
			int num95 = 0;
			while (true)
			{
				transformConstraintTimeline.SetFrame(num94, num87, num88, num89, num90, num91, num92, num93);
				if (num94 == num86)
				{
					break;
				}
				float num96 = input.ReadFloat();
				float num97 = input.ReadFloat();
				float num98 = input.ReadFloat();
				float num99 = input.ReadFloat();
				float num100 = input.ReadFloat();
				float num101 = input.ReadFloat();
				float num102 = input.ReadFloat();
				switch (input.ReadByte())
				{
				case 1:
					transformConstraintTimeline.SetStepped(num94);
					break;
				case 2:
					SetBezier(input, transformConstraintTimeline, num95++, num94, 0, num87, num96, num88, num97, 1f);
					SetBezier(input, transformConstraintTimeline, num95++, num94, 1, num87, num96, num89, num98, 1f);
					SetBezier(input, transformConstraintTimeline, num95++, num94, 2, num87, num96, num90, num99, 1f);
					SetBezier(input, transformConstraintTimeline, num95++, num94, 3, num87, num96, num91, num100, 1f);
					SetBezier(input, transformConstraintTimeline, num95++, num94, 4, num87, num96, num92, num101, 1f);
					SetBezier(input, transformConstraintTimeline, num95++, num94, 5, num87, num96, num93, num102, 1f);
					break;
				}
				num87 = num96;
				num88 = num97;
				num89 = num98;
				num90 = num99;
				num91 = num100;
				num92 = num101;
				num93 = num102;
				num94++;
			}
			exposedList.Add(transformConstraintTimeline);
		}
		int num103 = 0;
		for (int num104 = input.ReadInt(optimizePositive: true); num103 < num104; num103++)
		{
			int num105 = input.ReadInt(optimizePositive: true);
			PathConstraintData pathConstraintData = skeletonData.pathConstraints.Items[num105];
			int num106 = 0;
			for (int num107 = input.ReadInt(optimizePositive: true); num106 < num107; num106++)
			{
				switch (input.ReadByte())
				{
				case 0:
					exposedList.Add(ReadTimeline(input, new PathConstraintPositionTimeline(input.ReadInt(optimizePositive: true), input.ReadInt(optimizePositive: true), num105), (pathConstraintData.positionMode == PositionMode.Fixed) ? num : 1f));
					break;
				case 1:
					exposedList.Add(ReadTimeline(input, new PathConstraintSpacingTimeline(input.ReadInt(optimizePositive: true), input.ReadInt(optimizePositive: true), num105), (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed) ? num : 1f));
					break;
				case 2:
				{
					PathConstraintMixTimeline pathConstraintMixTimeline = new PathConstraintMixTimeline(input.ReadInt(optimizePositive: true), input.ReadInt(optimizePositive: true), num105);
					float num108 = input.ReadFloat();
					float num109 = input.ReadFloat();
					float num110 = input.ReadFloat();
					float num111 = input.ReadFloat();
					int num112 = 0;
					int num113 = 0;
					int num114 = pathConstraintMixTimeline.FrameCount - 1;
					while (true)
					{
						pathConstraintMixTimeline.SetFrame(num112, num108, num109, num110, num111);
						if (num112 == num114)
						{
							break;
						}
						float num115 = input.ReadFloat();
						float num116 = input.ReadFloat();
						float num117 = input.ReadFloat();
						float num118 = input.ReadFloat();
						switch (input.ReadByte())
						{
						case 1:
							pathConstraintMixTimeline.SetStepped(num112);
							break;
						case 2:
							SetBezier(input, pathConstraintMixTimeline, num113++, num112, 0, num108, num115, num109, num116, 1f);
							SetBezier(input, pathConstraintMixTimeline, num113++, num112, 1, num108, num115, num110, num117, 1f);
							SetBezier(input, pathConstraintMixTimeline, num113++, num112, 2, num108, num115, num111, num118, 1f);
							break;
						}
						num108 = num115;
						num109 = num116;
						num110 = num117;
						num111 = num118;
						num112++;
					}
					exposedList.Add(pathConstraintMixTimeline);
					break;
				}
				}
			}
		}
		int num119 = 0;
		for (int num120 = input.ReadInt(optimizePositive: true); num119 < num120; num119++)
		{
			Skin skin = skeletonData.skins.Items[input.ReadInt(optimizePositive: true)];
			int num121 = 0;
			for (int num122 = input.ReadInt(optimizePositive: true); num121 < num122; num121++)
			{
				int slotIndex2 = input.ReadInt(optimizePositive: true);
				int num123 = 0;
				for (int num124 = input.ReadInt(optimizePositive: true); num123 < num124; num123++)
				{
					string text = input.ReadStringRef();
					VertexAttachment vertexAttachment = (VertexAttachment)skin.GetAttachment(slotIndex2, text);
					if (vertexAttachment == null)
					{
						throw new SerializationException("Vertex attachment not found: " + text);
					}
					bool flag = vertexAttachment.Bones != null;
					float[] vertices = vertexAttachment.Vertices;
					int num125 = (flag ? (vertices.Length / 3 << 1) : vertices.Length);
					int num126 = input.ReadInt(optimizePositive: true);
					int num127 = num126 - 1;
					DeformTimeline deformTimeline = new DeformTimeline(num126, input.ReadInt(optimizePositive: true), slotIndex2, vertexAttachment);
					float num128 = input.ReadFloat();
					int num129 = 0;
					int num130 = 0;
					while (true)
					{
						int num131 = input.ReadInt(optimizePositive: true);
						float[] array;
						if (num131 == 0)
						{
							array = (flag ? new float[num125] : vertices);
						}
						else
						{
							array = new float[num125];
							int num132 = input.ReadInt(optimizePositive: true);
							num131 += num132;
							if (num == 1f)
							{
								for (int num133 = num132; num133 < num131; num133++)
								{
									array[num133] = input.ReadFloat();
								}
							}
							else
							{
								for (int num134 = num132; num134 < num131; num134++)
								{
									array[num134] = input.ReadFloat() * num;
								}
							}
							if (!flag)
							{
								int num135 = 0;
								for (int num136 = array.Length; num135 < num136; num135++)
								{
									array[num135] += vertices[num135];
								}
							}
						}
						deformTimeline.SetFrame(num129, num128, array);
						if (num129 == num127)
						{
							break;
						}
						float num137 = input.ReadFloat();
						switch (input.ReadByte())
						{
						case 1:
							deformTimeline.SetStepped(num129);
							break;
						case 2:
							SetBezier(input, deformTimeline, num130++, num129, 0, num128, num137, 0f, 1f, 1f);
							break;
						}
						num128 = num137;
						num129++;
					}
					exposedList.Add(deformTimeline);
				}
			}
		}
		int num138 = input.ReadInt(optimizePositive: true);
		if (num138 > 0)
		{
			DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(num138);
			int count = skeletonData.slots.Count;
			for (int num139 = 0; num139 < num138; num139++)
			{
				float time = input.ReadFloat();
				int num140 = input.ReadInt(optimizePositive: true);
				int[] array2 = new int[count];
				for (int num141 = count - 1; num141 >= 0; num141--)
				{
					array2[num141] = -1;
				}
				int[] array3 = new int[count - num140];
				int num142 = 0;
				int num143 = 0;
				for (int num144 = 0; num144 < num140; num144++)
				{
					int num145 = input.ReadInt(optimizePositive: true);
					while (num142 != num145)
					{
						array3[num143++] = num142++;
					}
					array2[num142 + input.ReadInt(optimizePositive: true)] = num142++;
				}
				while (num142 < count)
				{
					array3[num143++] = num142++;
				}
				for (int num146 = count - 1; num146 >= 0; num146--)
				{
					if (array2[num146] == -1)
					{
						array2[num146] = array3[--num143];
					}
				}
				drawOrderTimeline.SetFrame(num139, time, array2);
			}
			exposedList.Add(drawOrderTimeline);
		}
		int num147 = input.ReadInt(optimizePositive: true);
		if (num147 > 0)
		{
			EventTimeline eventTimeline = new EventTimeline(num147);
			for (int num148 = 0; num148 < num147; num148++)
			{
				float time2 = input.ReadFloat();
				EventData eventData = skeletonData.events.Items[input.ReadInt(optimizePositive: true)];
				Event obj = new Event(time2, eventData);
				obj.intValue = input.ReadInt(optimizePositive: false);
				obj.floatValue = input.ReadFloat();
				obj.stringValue = (input.ReadBoolean() ? input.ReadString() : eventData.String);
				if (obj.Data.AudioPath != null)
				{
					obj.volume = input.ReadFloat();
					obj.balance = input.ReadFloat();
				}
				eventTimeline.SetFrame(num148, obj);
			}
			exposedList.Add(eventTimeline);
		}
		float num149 = 0f;
		Timeline[] items = exposedList.Items;
		int num150 = 0;
		for (int count2 = exposedList.Count; num150 < count2; num150++)
		{
			num149 = Math.Max(num149, items[num150].Duration);
		}
		return new Animation(name, exposedList, num149);
	}

	private Timeline ReadTimeline(SkeletonInput input, CurveTimeline1 timeline, float scale)
	{
		float num = input.ReadFloat();
		float num2 = input.ReadFloat() * scale;
		int num3 = 0;
		int num4 = 0;
		int num5 = timeline.FrameCount - 1;
		while (true)
		{
			timeline.SetFrame(num3, num, num2);
			if (num3 == num5)
			{
				break;
			}
			float num6 = input.ReadFloat();
			float num7 = input.ReadFloat() * scale;
			switch (input.ReadByte())
			{
			case 1:
				timeline.SetStepped(num3);
				break;
			case 2:
				SetBezier(input, timeline, num4++, num3, 0, num, num6, num2, num7, scale);
				break;
			}
			num = num6;
			num2 = num7;
			num3++;
		}
		return timeline;
	}

	private Timeline ReadTimeline(SkeletonInput input, CurveTimeline2 timeline, float scale)
	{
		float num = input.ReadFloat();
		float value = input.ReadFloat() * scale;
		float num2 = input.ReadFloat() * scale;
		int num3 = 0;
		int num4 = 0;
		int num5 = timeline.FrameCount - 1;
		while (true)
		{
			timeline.SetFrame(num3, num, value, num2);
			if (num3 == num5)
			{
				break;
			}
			float num6 = input.ReadFloat();
			float num7 = input.ReadFloat() * scale;
			float num8 = input.ReadFloat() * scale;
			switch (input.ReadByte())
			{
			case 1:
				timeline.SetStepped(num3);
				break;
			case 2:
				SetBezier(input, timeline, num4++, num3, 0, num, num6, value, num7, scale);
				SetBezier(input, timeline, num4++, num3, 1, num, num6, num2, num8, scale);
				break;
			}
			num = num6;
			value = num7;
			num2 = num8;
			num3++;
		}
		return timeline;
	}

	private void SetBezier(SkeletonInput input, CurveTimeline timeline, int bezier, int frame, int value, float time1, float time2, float value1, float value2, float scale)
	{
		timeline.SetBezier(bezier, frame, value, time1, value1, input.ReadFloat(), input.ReadFloat() * scale, input.ReadFloat(), input.ReadFloat() * scale, time2, value2);
	}
}
