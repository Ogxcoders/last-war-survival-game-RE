using System;
using System.Collections.Generic;
using System.IO;

namespace Spine;

public class SkeletonJson : SkeletonLoader
{
	public SkeletonJson(AttachmentLoader attachmentLoader)
		: base(attachmentLoader)
	{
	}

	public SkeletonJson(params Atlas[] atlasArray)
		: base(atlasArray)
	{
	}

	public override SkeletonData ReadSkeletonData(string path)
	{
		using StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
		SkeletonData skeletonData = ReadSkeletonData(reader);
		skeletonData.name = Path.GetFileNameWithoutExtension(path);
		return skeletonData;
	}

	public SkeletonData ReadSkeletonData(TextReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader", "reader cannot be null.");
		}
		float num = scale;
		SkeletonData skeletonData = new SkeletonData();
		if (!(Json.Deserialize(reader) is Dictionary<string, object> dictionary))
		{
			throw new Exception("Invalid JSON.");
		}
		if (dictionary.ContainsKey("skeleton"))
		{
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)dictionary["skeleton"];
			skeletonData.hash = (string)dictionary2["hash"];
			skeletonData.version = (string)dictionary2["spine"];
			skeletonData.x = GetFloat(dictionary2, "x", 0f);
			skeletonData.y = GetFloat(dictionary2, "y", 0f);
			skeletonData.width = GetFloat(dictionary2, "width", 0f);
			skeletonData.height = GetFloat(dictionary2, "height", 0f);
			skeletonData.fps = GetFloat(dictionary2, "fps", 30f);
			skeletonData.imagesPath = GetString(dictionary2, "images", null);
			skeletonData.audioPath = GetString(dictionary2, "audio", null);
		}
		if (dictionary.ContainsKey("bones"))
		{
			foreach (Dictionary<string, object> item in (List<object>)dictionary["bones"])
			{
				BoneData boneData = null;
				if (item.ContainsKey("parent"))
				{
					boneData = skeletonData.FindBone((string)item["parent"]);
					if (boneData == null)
					{
						throw new Exception("Parent bone not found: " + item["parent"]);
					}
				}
				BoneData boneData2 = new BoneData(skeletonData.Bones.Count, (string)item["name"], boneData);
				boneData2.length = GetFloat(item, "length", 0f) * num;
				boneData2.x = GetFloat(item, "x", 0f) * num;
				boneData2.y = GetFloat(item, "y", 0f) * num;
				boneData2.rotation = GetFloat(item, "rotation", 0f);
				boneData2.scaleX = GetFloat(item, "scaleX", 1f);
				boneData2.scaleY = GetFloat(item, "scaleY", 1f);
				boneData2.shearX = GetFloat(item, "shearX", 0f);
				boneData2.shearY = GetFloat(item, "shearY", 0f);
				string value = GetString(item, "transform", TransformMode.Normal.ToString());
				boneData2.transformMode = (TransformMode)Enum.Parse(typeof(TransformMode), value, ignoreCase: true);
				boneData2.skinRequired = GetBoolean(item, "skin", defaultValue: false);
				skeletonData.bones.Add(boneData2);
			}
		}
		if (dictionary.ContainsKey("slots"))
		{
			foreach (Dictionary<string, object> item2 in (List<object>)dictionary["slots"])
			{
				string name = (string)item2["name"];
				string text = (string)item2["bone"];
				BoneData boneData3 = skeletonData.FindBone(text);
				if (boneData3 == null)
				{
					throw new Exception("Slot bone not found: " + text);
				}
				SlotData slotData = new SlotData(skeletonData.Slots.Count, name, boneData3);
				if (item2.ContainsKey("color"))
				{
					string hexString = (string)item2["color"];
					slotData.r = ToColor(hexString, 0);
					slotData.g = ToColor(hexString, 1);
					slotData.b = ToColor(hexString, 2);
					slotData.a = ToColor(hexString, 3);
				}
				if (item2.ContainsKey("dark"))
				{
					string hexString2 = (string)item2["dark"];
					slotData.r2 = ToColor(hexString2, 0, 6);
					slotData.g2 = ToColor(hexString2, 1, 6);
					slotData.b2 = ToColor(hexString2, 2, 6);
					slotData.hasSecondColor = true;
				}
				slotData.attachmentName = GetString(item2, "attachment", null);
				if (item2.ContainsKey("blend"))
				{
					slotData.blendMode = (BlendMode)Enum.Parse(typeof(BlendMode), (string)item2["blend"], ignoreCase: true);
				}
				else
				{
					slotData.blendMode = BlendMode.Normal;
				}
				skeletonData.slots.Add(slotData);
			}
		}
		if (dictionary.ContainsKey("ik"))
		{
			foreach (Dictionary<string, object> item3 in (List<object>)dictionary["ik"])
			{
				IkConstraintData ikConstraintData = new IkConstraintData((string)item3["name"]);
				ikConstraintData.order = GetInt(item3, "order", 0);
				ikConstraintData.skinRequired = GetBoolean(item3, "skin", defaultValue: false);
				if (item3.ContainsKey("bones"))
				{
					foreach (string item4 in (List<object>)item3["bones"])
					{
						BoneData boneData4 = skeletonData.FindBone(item4);
						if (boneData4 == null)
						{
							throw new Exception("IK bone not found: " + item4);
						}
						ikConstraintData.bones.Add(boneData4);
					}
				}
				string text3 = (string)item3["target"];
				ikConstraintData.target = skeletonData.FindBone(text3);
				if (ikConstraintData.target == null)
				{
					throw new Exception("IK target bone not found: " + text3);
				}
				ikConstraintData.mix = GetFloat(item3, "mix", 1f);
				ikConstraintData.softness = GetFloat(item3, "softness", 0f) * num;
				ikConstraintData.bendDirection = (GetBoolean(item3, "bendPositive", defaultValue: true) ? 1 : (-1));
				ikConstraintData.compress = GetBoolean(item3, "compress", defaultValue: false);
				ikConstraintData.stretch = GetBoolean(item3, "stretch", defaultValue: false);
				ikConstraintData.uniform = GetBoolean(item3, "uniform", defaultValue: false);
				skeletonData.ikConstraints.Add(ikConstraintData);
			}
		}
		if (dictionary.ContainsKey("transform"))
		{
			foreach (Dictionary<string, object> item5 in (List<object>)dictionary["transform"])
			{
				TransformConstraintData transformConstraintData = new TransformConstraintData((string)item5["name"]);
				transformConstraintData.order = GetInt(item5, "order", 0);
				transformConstraintData.skinRequired = GetBoolean(item5, "skin", defaultValue: false);
				if (item5.ContainsKey("bones"))
				{
					foreach (string item6 in (List<object>)item5["bones"])
					{
						BoneData boneData5 = skeletonData.FindBone(item6);
						if (boneData5 == null)
						{
							throw new Exception("Transform constraint bone not found: " + item6);
						}
						transformConstraintData.bones.Add(boneData5);
					}
				}
				string text5 = (string)item5["target"];
				transformConstraintData.target = skeletonData.FindBone(text5);
				if (transformConstraintData.target == null)
				{
					throw new Exception("Transform constraint target bone not found: " + text5);
				}
				transformConstraintData.local = GetBoolean(item5, "local", defaultValue: false);
				transformConstraintData.relative = GetBoolean(item5, "relative", defaultValue: false);
				transformConstraintData.offsetRotation = GetFloat(item5, "rotation", 0f);
				transformConstraintData.offsetX = GetFloat(item5, "x", 0f) * num;
				transformConstraintData.offsetY = GetFloat(item5, "y", 0f) * num;
				transformConstraintData.offsetScaleX = GetFloat(item5, "scaleX", 0f);
				transformConstraintData.offsetScaleY = GetFloat(item5, "scaleY", 0f);
				transformConstraintData.offsetShearY = GetFloat(item5, "shearY", 0f);
				transformConstraintData.mixRotate = GetFloat(item5, "mixRotate", 1f);
				transformConstraintData.mixX = GetFloat(item5, "mixX", 1f);
				transformConstraintData.mixY = GetFloat(item5, "mixY", transformConstraintData.mixX);
				transformConstraintData.mixScaleX = GetFloat(item5, "mixScaleX", 1f);
				transformConstraintData.mixScaleY = GetFloat(item5, "mixScaleY", transformConstraintData.mixScaleX);
				transformConstraintData.mixShearY = GetFloat(item5, "mixShearY", 1f);
				skeletonData.transformConstraints.Add(transformConstraintData);
			}
		}
		if (dictionary.ContainsKey("path"))
		{
			foreach (Dictionary<string, object> item7 in (List<object>)dictionary["path"])
			{
				PathConstraintData pathConstraintData = new PathConstraintData((string)item7["name"]);
				pathConstraintData.order = GetInt(item7, "order", 0);
				pathConstraintData.skinRequired = GetBoolean(item7, "skin", defaultValue: false);
				if (item7.ContainsKey("bones"))
				{
					foreach (string item8 in (List<object>)item7["bones"])
					{
						BoneData boneData6 = skeletonData.FindBone(item8);
						if (boneData6 == null)
						{
							throw new Exception("Path bone not found: " + item8);
						}
						pathConstraintData.bones.Add(boneData6);
					}
				}
				string text7 = (string)item7["target"];
				pathConstraintData.target = skeletonData.FindSlot(text7);
				if (pathConstraintData.target == null)
				{
					throw new Exception("Path target slot not found: " + text7);
				}
				pathConstraintData.positionMode = (PositionMode)Enum.Parse(typeof(PositionMode), GetString(item7, "positionMode", "percent"), ignoreCase: true);
				pathConstraintData.spacingMode = (SpacingMode)Enum.Parse(typeof(SpacingMode), GetString(item7, "spacingMode", "length"), ignoreCase: true);
				pathConstraintData.rotateMode = (RotateMode)Enum.Parse(typeof(RotateMode), GetString(item7, "rotateMode", "tangent"), ignoreCase: true);
				pathConstraintData.offsetRotation = GetFloat(item7, "rotation", 0f);
				pathConstraintData.position = GetFloat(item7, "position", 0f);
				if (pathConstraintData.positionMode == PositionMode.Fixed)
				{
					pathConstraintData.position *= num;
				}
				pathConstraintData.spacing = GetFloat(item7, "spacing", 0f);
				if (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed)
				{
					pathConstraintData.spacing *= num;
				}
				pathConstraintData.mixRotate = GetFloat(item7, "mixRotate", 1f);
				pathConstraintData.mixX = GetFloat(item7, "mixX", 1f);
				pathConstraintData.mixY = GetFloat(item7, "mixY", pathConstraintData.mixX);
				skeletonData.pathConstraints.Add(pathConstraintData);
			}
		}
		if (dictionary.ContainsKey("skins"))
		{
			foreach (Dictionary<string, object> item9 in (List<object>)dictionary["skins"])
			{
				Skin skin = new Skin((string)item9["name"]);
				if (item9.ContainsKey("bones"))
				{
					foreach (string item10 in (List<object>)item9["bones"])
					{
						BoneData boneData7 = skeletonData.FindBone(item10);
						if (boneData7 == null)
						{
							throw new Exception("Skin bone not found: " + item10);
						}
						skin.bones.Add(boneData7);
					}
				}
				skin.bones.TrimExcess();
				if (item9.ContainsKey("ik"))
				{
					foreach (string item11 in (List<object>)item9["ik"])
					{
						IkConstraintData ikConstraintData2 = skeletonData.FindIkConstraint(item11);
						if (ikConstraintData2 == null)
						{
							throw new Exception("Skin IK constraint not found: " + item11);
						}
						skin.constraints.Add(ikConstraintData2);
					}
				}
				if (item9.ContainsKey("transform"))
				{
					foreach (string item12 in (List<object>)item9["transform"])
					{
						TransformConstraintData transformConstraintData2 = skeletonData.FindTransformConstraint(item12);
						if (transformConstraintData2 == null)
						{
							throw new Exception("Skin transform constraint not found: " + item12);
						}
						skin.constraints.Add(transformConstraintData2);
					}
				}
				if (item9.ContainsKey("path"))
				{
					foreach (string item13 in (List<object>)item9["path"])
					{
						PathConstraintData pathConstraintData2 = skeletonData.FindPathConstraint(item13);
						if (pathConstraintData2 == null)
						{
							throw new Exception("Skin path constraint not found: " + item13);
						}
						skin.constraints.Add(pathConstraintData2);
					}
				}
				skin.constraints.TrimExcess();
				if (item9.ContainsKey("attachments"))
				{
					foreach (KeyValuePair<string, object> item14 in (Dictionary<string, object>)item9["attachments"])
					{
						int slotIndex = FindSlotIndex(skeletonData, item14.Key);
						foreach (KeyValuePair<string, object> item15 in (Dictionary<string, object>)item14.Value)
						{
							try
							{
								Attachment attachment = ReadAttachment((Dictionary<string, object>)item15.Value, skin, slotIndex, item15.Key, skeletonData);
								if (attachment != null)
								{
									skin.SetAttachment(slotIndex, item15.Key, attachment);
								}
							}
							catch (Exception innerException)
							{
								throw new Exception("Error reading attachment: " + item15.Key + ", skin: " + skin, innerException);
							}
						}
					}
				}
				skeletonData.skins.Add(skin);
				if (skin.name == "default")
				{
					skeletonData.defaultSkin = skin;
				}
			}
		}
		int i = 0;
		for (int count = linkedMeshes.Count; i < count; i++)
		{
			LinkedMesh linkedMesh = linkedMeshes[i];
			Attachment attachment2 = (((linkedMesh.skin == null) ? skeletonData.defaultSkin : skeletonData.FindSkin(linkedMesh.skin)) ?? throw new Exception("Slot not found: " + linkedMesh.skin)).GetAttachment(linkedMesh.slotIndex, linkedMesh.parent);
			if (attachment2 == null)
			{
				throw new Exception("Parent mesh not found: " + linkedMesh.parent);
			}
			linkedMesh.mesh.DeformAttachment = (linkedMesh.inheritDeform ? ((VertexAttachment)attachment2) : linkedMesh.mesh);
			linkedMesh.mesh.ParentMesh = (MeshAttachment)attachment2;
			linkedMesh.mesh.UpdateUVs();
		}
		linkedMeshes.Clear();
		if (dictionary.ContainsKey("events"))
		{
			foreach (KeyValuePair<string, object> item16 in (Dictionary<string, object>)dictionary["events"])
			{
				Dictionary<string, object> map = (Dictionary<string, object>)item16.Value;
				EventData eventData = new EventData(item16.Key);
				eventData.Int = GetInt(map, "int", 0);
				eventData.Float = GetFloat(map, "float", 0f);
				eventData.String = GetString(map, "string", string.Empty);
				eventData.AudioPath = GetString(map, "audio", null);
				if (eventData.AudioPath != null)
				{
					eventData.Volume = GetFloat(map, "volume", 1f);
					eventData.Balance = GetFloat(map, "balance", 0f);
				}
				skeletonData.events.Add(eventData);
			}
		}
		if (dictionary.ContainsKey("animations"))
		{
			foreach (KeyValuePair<string, object> item17 in (Dictionary<string, object>)dictionary["animations"])
			{
				try
				{
					ReadAnimation((Dictionary<string, object>)item17.Value, item17.Key, skeletonData);
				}
				catch (Exception ex)
				{
					throw new Exception("Error reading animation: " + item17.Key + "\n" + ex.Message, ex);
				}
			}
		}
		skeletonData.bones.TrimExcess();
		skeletonData.slots.TrimExcess();
		skeletonData.skins.TrimExcess();
		skeletonData.events.TrimExcess();
		skeletonData.animations.TrimExcess();
		skeletonData.ikConstraints.TrimExcess();
		return skeletonData;
	}

	private Attachment ReadAttachment(Dictionary<string, object> map, Skin skin, int slotIndex, string name, SkeletonData skeletonData)
	{
		float num = scale;
		name = GetString(map, "name", name);
		string value = GetString(map, "type", "region");
		AttachmentType attachmentType = (AttachmentType)Enum.Parse(typeof(AttachmentType), value, ignoreCase: true);
		string path = GetString(map, "path", name);
		switch (attachmentType)
		{
		case AttachmentType.Region:
		{
			RegionAttachment regionAttachment = attachmentLoader.NewRegionAttachment(skin, name, path);
			if (regionAttachment == null)
			{
				return null;
			}
			regionAttachment.Path = path;
			regionAttachment.x = GetFloat(map, "x", 0f) * num;
			regionAttachment.y = GetFloat(map, "y", 0f) * num;
			regionAttachment.scaleX = GetFloat(map, "scaleX", 1f);
			regionAttachment.scaleY = GetFloat(map, "scaleY", 1f);
			regionAttachment.rotation = GetFloat(map, "rotation", 0f);
			regionAttachment.width = GetFloat(map, "width", 32f) * num;
			regionAttachment.height = GetFloat(map, "height", 32f) * num;
			if (map.ContainsKey("color"))
			{
				string hexString2 = (string)map["color"];
				regionAttachment.r = ToColor(hexString2, 0);
				regionAttachment.g = ToColor(hexString2, 1);
				regionAttachment.b = ToColor(hexString2, 2);
				regionAttachment.a = ToColor(hexString2, 3);
			}
			regionAttachment.UpdateOffset();
			return regionAttachment;
		}
		case AttachmentType.Boundingbox:
		{
			BoundingBoxAttachment boundingBoxAttachment = attachmentLoader.NewBoundingBoxAttachment(skin, name);
			if (boundingBoxAttachment == null)
			{
				return null;
			}
			ReadVertices(map, boundingBoxAttachment, GetInt(map, "vertexCount", 0) << 1);
			return boundingBoxAttachment;
		}
		case AttachmentType.Mesh:
		case AttachmentType.Linkedmesh:
		{
			MeshAttachment meshAttachment = attachmentLoader.NewMeshAttachment(skin, name, path);
			if (meshAttachment == null)
			{
				return null;
			}
			meshAttachment.Path = path;
			if (map.ContainsKey("color"))
			{
				string hexString = (string)map["color"];
				meshAttachment.r = ToColor(hexString, 0);
				meshAttachment.g = ToColor(hexString, 1);
				meshAttachment.b = ToColor(hexString, 2);
				meshAttachment.a = ToColor(hexString, 3);
			}
			meshAttachment.Width = GetFloat(map, "width", 0f) * num;
			meshAttachment.Height = GetFloat(map, "height", 0f) * num;
			string text2 = GetString(map, "parent", null);
			if (text2 != null)
			{
				linkedMeshes.Add(new LinkedMesh(meshAttachment, GetString(map, "skin", null), slotIndex, text2, GetBoolean(map, "deform", defaultValue: true)));
				return meshAttachment;
			}
			float[] floatArray = GetFloatArray(map, "uvs", 1f);
			ReadVertices(map, meshAttachment, floatArray.Length);
			meshAttachment.triangles = GetIntArray(map, "triangles");
			meshAttachment.regionUVs = floatArray;
			meshAttachment.UpdateUVs();
			if (map.ContainsKey("hull"))
			{
				meshAttachment.HullLength = GetInt(map, "hull", 0) << 1;
			}
			if (map.ContainsKey("edges"))
			{
				meshAttachment.Edges = GetIntArray(map, "edges");
			}
			return meshAttachment;
		}
		case AttachmentType.Path:
		{
			PathAttachment pathAttachment = attachmentLoader.NewPathAttachment(skin, name);
			if (pathAttachment == null)
			{
				return null;
			}
			pathAttachment.closed = GetBoolean(map, "closed", defaultValue: false);
			pathAttachment.constantSpeed = GetBoolean(map, "constantSpeed", defaultValue: true);
			int num2 = GetInt(map, "vertexCount", 0);
			ReadVertices(map, pathAttachment, num2 << 1);
			pathAttachment.lengths = GetFloatArray(map, "lengths", num);
			return pathAttachment;
		}
		case AttachmentType.Point:
		{
			PointAttachment pointAttachment = attachmentLoader.NewPointAttachment(skin, name);
			if (pointAttachment == null)
			{
				return null;
			}
			pointAttachment.x = GetFloat(map, "x", 0f) * num;
			pointAttachment.y = GetFloat(map, "y", 0f) * num;
			pointAttachment.rotation = GetFloat(map, "rotation", 0f);
			return pointAttachment;
		}
		case AttachmentType.Clipping:
		{
			ClippingAttachment clippingAttachment = attachmentLoader.NewClippingAttachment(skin, name);
			if (clippingAttachment == null)
			{
				return null;
			}
			string text = GetString(map, "end", null);
			if (text != null)
			{
				SlotData slotData = skeletonData.FindSlot(text);
				if (slotData == null)
				{
					throw new Exception("Clipping end slot not found: " + text);
				}
				clippingAttachment.EndSlot = slotData;
			}
			ReadVertices(map, clippingAttachment, GetInt(map, "vertexCount", 0) << 1);
			return clippingAttachment;
		}
		default:
			return null;
		}
	}

	private void ReadVertices(Dictionary<string, object> map, VertexAttachment attachment, int verticesLength)
	{
		attachment.WorldVerticesLength = verticesLength;
		float[] floatArray = GetFloatArray(map, "vertices", 1f);
		float num = base.Scale;
		if (verticesLength == floatArray.Length)
		{
			if (num != 1f)
			{
				for (int i = 0; i < floatArray.Length; i++)
				{
					floatArray[i] *= num;
				}
			}
			attachment.vertices = floatArray;
			return;
		}
		ExposedList<float> exposedList = new ExposedList<float>(verticesLength * 3 * 3);
		ExposedList<int> exposedList2 = new ExposedList<int>(verticesLength * 3);
		int j = 0;
		int num2 = floatArray.Length;
		while (j < num2)
		{
			int num3 = (int)floatArray[j++];
			exposedList2.Add(num3);
			for (int num4 = j + (num3 << 2); j < num4; j += 4)
			{
				exposedList2.Add((int)floatArray[j]);
				exposedList.Add(floatArray[j + 1] * base.Scale);
				exposedList.Add(floatArray[j + 2] * base.Scale);
				exposedList.Add(floatArray[j + 3]);
			}
		}
		attachment.bones = exposedList2.ToArray();
		attachment.vertices = exposedList.ToArray();
	}

	private int FindSlotIndex(SkeletonData skeletonData, string slotName)
	{
		SlotData[] items = skeletonData.slots.Items;
		int i = 0;
		for (int count = skeletonData.slots.Count; i < count; i++)
		{
			if (items[i].name == slotName)
			{
				return i;
			}
		}
		throw new Exception("Slot not found: " + slotName);
	}

	private void ReadAnimation(Dictionary<string, object> map, string name, SkeletonData skeletonData)
	{
		float num = scale;
		ExposedList<Timeline> exposedList = new ExposedList<Timeline>();
		if (map.ContainsKey("slots"))
		{
			foreach (KeyValuePair<string, object> item3 in (Dictionary<string, object>)map["slots"])
			{
				string key = item3.Key;
				int slotIndex = FindSlotIndex(skeletonData, key);
				foreach (KeyValuePair<string, object> item4 in (Dictionary<string, object>)item3.Value)
				{
					List<object> list = (List<object>)item4.Value;
					int count = list.Count;
					if (count == 0)
					{
						continue;
					}
					string key2 = item4.Key;
					switch (key2)
					{
					case "attachment":
					{
						AttachmentTimeline attachmentTimeline = new AttachmentTimeline(count, slotIndex);
						int num58 = 0;
						foreach (Dictionary<string, object> item5 in list)
						{
							attachmentTimeline.SetFrame(num58++, GetFloat(item5, "time", 0f), (string)item5["name"]);
						}
						exposedList.Add(attachmentTimeline);
						break;
					}
					case "rgba":
					{
						RGBATimeline rGBATimeline = new RGBATimeline(count, count << 2, slotIndex);
						List<object>.Enumerator enumerator6 = list.GetEnumerator();
						enumerator6.MoveNext();
						Dictionary<string, object> dictionary4 = (Dictionary<string, object>)enumerator6.Current;
						float num46 = GetFloat(dictionary4, "time", 0f);
						string hexString11 = (string)dictionary4["color"];
						float num47 = ToColor(hexString11, 0);
						float num48 = ToColor(hexString11, 1);
						float num49 = ToColor(hexString11, 2);
						float num50 = ToColor(hexString11, 3);
						int num51 = 0;
						int num52 = 0;
						while (true)
						{
							rGBATimeline.SetFrame(num51, num46, num47, num48, num49, num50);
							if (!enumerator6.MoveNext())
							{
								break;
							}
							Dictionary<string, object> obj4 = (Dictionary<string, object>)enumerator6.Current;
							float num53 = GetFloat(obj4, "time", 0f);
							string hexString12 = (string)obj4["color"];
							float num54 = ToColor(hexString12, 0);
							float num55 = ToColor(hexString12, 1);
							float num56 = ToColor(hexString12, 2);
							float num57 = ToColor(hexString12, 3);
							if (dictionary4.ContainsKey("curve"))
							{
								object curve4 = dictionary4["curve"];
								num52 = ReadCurve(curve4, rGBATimeline, num52, num51, 0, num46, num53, num47, num54, 1f);
								num52 = ReadCurve(curve4, rGBATimeline, num52, num51, 1, num46, num53, num48, num55, 1f);
								num52 = ReadCurve(curve4, rGBATimeline, num52, num51, 2, num46, num53, num49, num56, 1f);
								num52 = ReadCurve(curve4, rGBATimeline, num52, num51, 3, num46, num53, num50, num57, 1f);
							}
							num46 = num53;
							num47 = num54;
							num48 = num55;
							num49 = num56;
							num50 = num57;
							dictionary4 = obj4;
							num51++;
						}
						rGBATimeline.Shrink(num52);
						exposedList.Add(rGBATimeline);
						break;
					}
					case "rgb":
					{
						RGBTimeline rGBTimeline = new RGBTimeline(count, count * 3, slotIndex);
						List<object>.Enumerator enumerator4 = list.GetEnumerator();
						enumerator4.MoveNext();
						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)enumerator4.Current;
						float num18 = GetFloat(dictionary2, "time", 0f);
						string hexString5 = (string)dictionary2["color"];
						float num19 = ToColor(hexString5, 0, 6);
						float num20 = ToColor(hexString5, 1, 6);
						float num21 = ToColor(hexString5, 2, 6);
						int num22 = 0;
						int num23 = 0;
						while (true)
						{
							rGBTimeline.SetFrame(num22, num18, num19, num20, num21);
							if (!enumerator4.MoveNext())
							{
								break;
							}
							Dictionary<string, object> obj2 = (Dictionary<string, object>)enumerator4.Current;
							float num24 = GetFloat(obj2, "time", 0f);
							string hexString6 = (string)obj2["color"];
							float num25 = ToColor(hexString6, 0, 6);
							float num26 = ToColor(hexString6, 1, 6);
							float num27 = ToColor(hexString6, 2, 6);
							if (dictionary2.ContainsKey("curve"))
							{
								object curve2 = dictionary2["curve"];
								num23 = ReadCurve(curve2, rGBTimeline, num23, num22, 0, num18, num24, num19, num25, 1f);
								num23 = ReadCurve(curve2, rGBTimeline, num23, num22, 1, num18, num24, num20, num26, 1f);
								num23 = ReadCurve(curve2, rGBTimeline, num23, num22, 2, num18, num24, num21, num27, 1f);
							}
							num18 = num24;
							num19 = num25;
							num20 = num26;
							num21 = num27;
							dictionary2 = obj2;
							num22++;
						}
						rGBTimeline.Shrink(num23);
						exposedList.Add(rGBTimeline);
						break;
					}
					case "alpha":
					{
						List<object>.Enumerator keyMapEnumerator = list.GetEnumerator();
						keyMapEnumerator.MoveNext();
						exposedList.Add(ReadTimeline(ref keyMapEnumerator, new AlphaTimeline(count, count, slotIndex), 0f, 1f));
						break;
					}
					case "rgba2":
					{
						RGBA2Timeline rGBA2Timeline = new RGBA2Timeline(count, count * 7, slotIndex);
						List<object>.Enumerator enumerator5 = list.GetEnumerator();
						enumerator5.MoveNext();
						Dictionary<string, object> dictionary3 = (Dictionary<string, object>)enumerator5.Current;
						float num28 = GetFloat(dictionary3, "time", 0f);
						string hexString7 = (string)dictionary3["light"];
						float num29 = ToColor(hexString7, 0);
						float num30 = ToColor(hexString7, 1);
						float num31 = ToColor(hexString7, 2);
						float num32 = ToColor(hexString7, 3);
						string hexString8 = (string)dictionary3["dark"];
						float num33 = ToColor(hexString8, 0, 6);
						float num34 = ToColor(hexString8, 1, 6);
						float num35 = ToColor(hexString8, 2, 6);
						int num36 = 0;
						int num37 = 0;
						while (true)
						{
							rGBA2Timeline.SetFrame(num36, num28, num29, num30, num31, num32, num33, num34, num35);
							if (!enumerator5.MoveNext())
							{
								break;
							}
							Dictionary<string, object> obj3 = (Dictionary<string, object>)enumerator5.Current;
							float num38 = GetFloat(obj3, "time", 0f);
							string hexString9 = (string)obj3["light"];
							float num39 = ToColor(hexString9, 0);
							float num40 = ToColor(hexString9, 1);
							float num41 = ToColor(hexString9, 2);
							float num42 = ToColor(hexString9, 3);
							string hexString10 = (string)obj3["dark"];
							float num43 = ToColor(hexString10, 0, 6);
							float num44 = ToColor(hexString10, 1, 6);
							float num45 = ToColor(hexString10, 2, 6);
							if (dictionary3.ContainsKey("curve"))
							{
								object curve3 = dictionary3["curve"];
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 0, num28, num38, num29, num39, 1f);
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 1, num28, num38, num30, num40, 1f);
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 2, num28, num38, num31, num41, 1f);
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 3, num28, num38, num32, num42, 1f);
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 4, num28, num38, num33, num43, 1f);
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 5, num28, num38, num34, num44, 1f);
								num37 = ReadCurve(curve3, rGBA2Timeline, num37, num36, 6, num28, num38, num35, num45, 1f);
							}
							num28 = num38;
							num29 = num39;
							num30 = num40;
							num31 = num41;
							num32 = num42;
							num33 = num43;
							num34 = num44;
							num35 = num45;
							dictionary3 = obj3;
							num36++;
						}
						rGBA2Timeline.Shrink(num37);
						exposedList.Add(rGBA2Timeline);
						break;
					}
					case "rgb2":
					{
						RGB2Timeline rGB2Timeline = new RGB2Timeline(count, count * 6, slotIndex);
						List<object>.Enumerator enumerator3 = list.GetEnumerator();
						enumerator3.MoveNext();
						Dictionary<string, object> dictionary = (Dictionary<string, object>)enumerator3.Current;
						float num2 = GetFloat(dictionary, "time", 0f);
						string hexString = (string)dictionary["light"];
						float num3 = ToColor(hexString, 0, 6);
						float num4 = ToColor(hexString, 1, 6);
						float num5 = ToColor(hexString, 2, 6);
						string hexString2 = (string)dictionary["dark"];
						float num6 = ToColor(hexString2, 0, 6);
						float num7 = ToColor(hexString2, 1, 6);
						float num8 = ToColor(hexString2, 2, 6);
						int num9 = 0;
						int num10 = 0;
						while (true)
						{
							rGB2Timeline.SetFrame(num9, num2, num3, num4, num5, num6, num7, num8);
							if (!enumerator3.MoveNext())
							{
								break;
							}
							Dictionary<string, object> obj = (Dictionary<string, object>)enumerator3.Current;
							float num11 = GetFloat(obj, "time", 0f);
							string hexString3 = (string)obj["light"];
							float num12 = ToColor(hexString3, 0, 6);
							float num13 = ToColor(hexString3, 1, 6);
							float num14 = ToColor(hexString3, 2, 6);
							string hexString4 = (string)obj["dark"];
							float num15 = ToColor(hexString4, 0, 6);
							float num16 = ToColor(hexString4, 1, 6);
							float num17 = ToColor(hexString4, 2, 6);
							if (dictionary.ContainsKey("curve"))
							{
								object curve = dictionary["curve"];
								num10 = ReadCurve(curve, rGB2Timeline, num10, num9, 0, num2, num11, num3, num12, 1f);
								num10 = ReadCurve(curve, rGB2Timeline, num10, num9, 1, num2, num11, num4, num13, 1f);
								num10 = ReadCurve(curve, rGB2Timeline, num10, num9, 2, num2, num11, num5, num14, 1f);
								num10 = ReadCurve(curve, rGB2Timeline, num10, num9, 3, num2, num11, num6, num15, 1f);
								num10 = ReadCurve(curve, rGB2Timeline, num10, num9, 4, num2, num11, num7, num16, 1f);
								num10 = ReadCurve(curve, rGB2Timeline, num10, num9, 5, num2, num11, num8, num17, 1f);
							}
							num2 = num11;
							num3 = num12;
							num4 = num13;
							num5 = num14;
							num6 = num15;
							num7 = num16;
							num8 = num17;
							dictionary = obj;
							num9++;
						}
						rGB2Timeline.Shrink(num10);
						exposedList.Add(rGB2Timeline);
						break;
					}
					default:
						throw new Exception("Invalid timeline type for a slot: " + key2 + " (" + key + ")");
					}
				}
			}
		}
		if (map.ContainsKey("bones"))
		{
			foreach (KeyValuePair<string, object> item6 in (Dictionary<string, object>)map["bones"])
			{
				string key3 = item6.Key;
				int num59 = -1;
				BoneData[] items = skeletonData.bones.Items;
				int i = 0;
				for (int count2 = skeletonData.bones.Count; i < count2; i++)
				{
					if (items[i].name == key3)
					{
						num59 = i;
						break;
					}
				}
				if (num59 == -1)
				{
					throw new Exception("Bone not found: " + key3);
				}
				foreach (KeyValuePair<string, object> item7 in (Dictionary<string, object>)item6.Value)
				{
					List<object> list2 = (List<object>)item7.Value;
					List<object>.Enumerator keyMapEnumerator2 = list2.GetEnumerator();
					if (keyMapEnumerator2.MoveNext())
					{
						int count3 = list2.Count;
						string key4 = item7.Key;
						switch (key4)
						{
						case "rotate":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new RotateTimeline(count3, count3, num59), 0f, 1f));
							continue;
						case "translate":
						{
							TranslateTimeline timeline3 = new TranslateTimeline(count3, count3 << 1, num59);
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, timeline3, "x", "y", 0f, num));
							continue;
						}
						case "translatex":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new TranslateXTimeline(count3, count3, num59), 0f, num));
							continue;
						case "translatey":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new TranslateYTimeline(count3, count3, num59), 0f, num));
							continue;
						case "scale":
						{
							ScaleTimeline timeline2 = new ScaleTimeline(count3, count3 << 1, num59);
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, timeline2, "x", "y", 1f, 1f));
							continue;
						}
						case "scalex":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new ScaleXTimeline(count3, count3, num59), 1f, 1f));
							continue;
						case "scaley":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new ScaleYTimeline(count3, count3, num59), 1f, 1f));
							continue;
						case "shear":
						{
							ShearTimeline timeline = new ShearTimeline(count3, count3 << 1, num59);
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, timeline, "x", "y", 0f, 1f));
							continue;
						}
						case "shearx":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new ShearXTimeline(count3, count3, num59), 0f, 1f));
							continue;
						case "sheary":
							exposedList.Add(ReadTimeline(ref keyMapEnumerator2, new ShearYTimeline(count3, count3, num59), 0f, 1f));
							continue;
						}
						throw new Exception("Invalid timeline type for a bone: " + key4 + " (" + key3 + ")");
					}
				}
			}
		}
		if (map.ContainsKey("ik"))
		{
			foreach (KeyValuePair<string, object> item8 in (Dictionary<string, object>)map["ik"])
			{
				List<object> list3 = (List<object>)item8.Value;
				List<object>.Enumerator enumerator8 = list3.GetEnumerator();
				if (!enumerator8.MoveNext())
				{
					continue;
				}
				Dictionary<string, object> dictionary6 = (Dictionary<string, object>)enumerator8.Current;
				IkConstraintData item = skeletonData.FindIkConstraint(item8.Key);
				IkConstraintTimeline ikConstraintTimeline = new IkConstraintTimeline(list3.Count, list3.Count << 1, skeletonData.IkConstraints.IndexOf(item));
				float num60 = GetFloat(dictionary6, "time", 0f);
				float num61 = GetFloat(dictionary6, "mix", 1f);
				float num62 = GetFloat(dictionary6, "softness", 0f) * num;
				int num63 = 0;
				int num64 = 0;
				while (true)
				{
					ikConstraintTimeline.SetFrame(num63, num60, num61, num62, GetBoolean(dictionary6, "bendPositive", defaultValue: true) ? 1 : (-1), GetBoolean(dictionary6, "compress", defaultValue: false), GetBoolean(dictionary6, "stretch", defaultValue: false));
					if (!enumerator8.MoveNext())
					{
						break;
					}
					Dictionary<string, object> obj5 = (Dictionary<string, object>)enumerator8.Current;
					float num65 = GetFloat(obj5, "time", 0f);
					float num66 = GetFloat(obj5, "mix", 1f);
					float num67 = GetFloat(obj5, "softness", 0f) * num;
					if (dictionary6.ContainsKey("curve"))
					{
						object curve5 = dictionary6["curve"];
						num64 = ReadCurve(curve5, ikConstraintTimeline, num64, num63, 0, num60, num65, num61, num66, 1f);
						num64 = ReadCurve(curve5, ikConstraintTimeline, num64, num63, 1, num60, num65, num62, num67, num);
					}
					num60 = num65;
					num61 = num66;
					num62 = num67;
					dictionary6 = obj5;
					num63++;
				}
				ikConstraintTimeline.Shrink(num64);
				exposedList.Add(ikConstraintTimeline);
			}
		}
		if (map.ContainsKey("transform"))
		{
			foreach (KeyValuePair<string, object> item9 in (Dictionary<string, object>)map["transform"])
			{
				List<object> list4 = (List<object>)item9.Value;
				List<object>.Enumerator enumerator9 = list4.GetEnumerator();
				if (!enumerator9.MoveNext())
				{
					continue;
				}
				Dictionary<string, object> dictionary7 = (Dictionary<string, object>)enumerator9.Current;
				TransformConstraintData item2 = skeletonData.FindTransformConstraint(item9.Key);
				TransformConstraintTimeline transformConstraintTimeline = new TransformConstraintTimeline(list4.Count, list4.Count * 6, skeletonData.TransformConstraints.IndexOf(item2));
				float num68 = GetFloat(dictionary7, "time", 0f);
				float num69 = GetFloat(dictionary7, "mixRotate", 1f);
				float num70 = GetFloat(dictionary7, "mixShearY", 1f);
				float num71 = GetFloat(dictionary7, "mixX", 1f);
				float num72 = GetFloat(dictionary7, "mixY", num71);
				float num73 = GetFloat(dictionary7, "mixScaleX", 1f);
				float num74 = GetFloat(dictionary7, "mixScaleY", num73);
				int num75 = 0;
				int num76 = 0;
				while (true)
				{
					transformConstraintTimeline.SetFrame(num75, num68, num69, num71, num72, num73, num74, num70);
					if (!enumerator9.MoveNext())
					{
						break;
					}
					Dictionary<string, object> obj6 = (Dictionary<string, object>)enumerator9.Current;
					float num77 = GetFloat(obj6, "time", 0f);
					float num78 = GetFloat(obj6, "mixRotate", 1f);
					float value = GetFloat(obj6, "mixShearY", 1f);
					float num79 = GetFloat(obj6, "mixX", 1f);
					float num80 = GetFloat(obj6, "mixY", num79);
					float num81 = GetFloat(obj6, "mixScaleX", 1f);
					float num82 = GetFloat(obj6, "mixScaleY", num81);
					if (dictionary7.ContainsKey("curve"))
					{
						object curve6 = dictionary7["curve"];
						num76 = ReadCurve(curve6, transformConstraintTimeline, num76, num75, 0, num68, num77, num69, num78, 1f);
						num76 = ReadCurve(curve6, transformConstraintTimeline, num76, num75, 1, num68, num77, num71, num79, 1f);
						num76 = ReadCurve(curve6, transformConstraintTimeline, num76, num75, 2, num68, num77, num72, num80, 1f);
						num76 = ReadCurve(curve6, transformConstraintTimeline, num76, num75, 3, num68, num77, num73, num81, 1f);
						num76 = ReadCurve(curve6, transformConstraintTimeline, num76, num75, 4, num68, num77, num74, num82, 1f);
						num76 = ReadCurve(curve6, transformConstraintTimeline, num76, num75, 5, num68, num77, num70, value, 1f);
					}
					num68 = num77;
					num69 = num78;
					num71 = num79;
					num72 = num80;
					num73 = num81;
					num74 = num82;
					num73 = num81;
					dictionary7 = obj6;
					num75++;
				}
				transformConstraintTimeline.Shrink(num76);
				exposedList.Add(transformConstraintTimeline);
			}
		}
		if (map.ContainsKey("path"))
		{
			foreach (KeyValuePair<string, object> item10 in (Dictionary<string, object>)map["path"])
			{
				PathConstraintData pathConstraintData = skeletonData.FindPathConstraint(item10.Key);
				if (pathConstraintData == null)
				{
					throw new Exception("Path constraint not found: " + item10.Key);
				}
				int pathConstraintIndex = skeletonData.pathConstraints.IndexOf(pathConstraintData);
				foreach (KeyValuePair<string, object> item11 in (Dictionary<string, object>)item10.Value)
				{
					List<object> list5 = (List<object>)item11.Value;
					List<object>.Enumerator keyMapEnumerator3 = list5.GetEnumerator();
					if (!keyMapEnumerator3.MoveNext())
					{
						continue;
					}
					int count4 = list5.Count;
					switch (item11.Key)
					{
					case "position":
					{
						CurveTimeline1 timeline5 = new PathConstraintPositionTimeline(count4, count4, pathConstraintIndex);
						exposedList.Add(ReadTimeline(ref keyMapEnumerator3, timeline5, 0f, (pathConstraintData.positionMode == PositionMode.Fixed) ? num : 1f));
						break;
					}
					case "spacing":
					{
						CurveTimeline1 timeline4 = new PathConstraintSpacingTimeline(count4, count4, pathConstraintIndex);
						exposedList.Add(ReadTimeline(ref keyMapEnumerator3, timeline4, 0f, (pathConstraintData.spacingMode == SpacingMode.Length || pathConstraintData.spacingMode == SpacingMode.Fixed) ? num : 1f));
						break;
					}
					case "mix":
					{
						PathConstraintMixTimeline pathConstraintMixTimeline = new PathConstraintMixTimeline(count4, count4 * 3, pathConstraintIndex);
						Dictionary<string, object> dictionary8 = (Dictionary<string, object>)keyMapEnumerator3.Current;
						float num83 = GetFloat(dictionary8, "time", 0f);
						float num84 = GetFloat(dictionary8, "mixRotate", 1f);
						float num85 = GetFloat(dictionary8, "mixX", 1f);
						float num86 = GetFloat(dictionary8, "mixY", num85);
						int num87 = 0;
						int num88 = 0;
						while (true)
						{
							pathConstraintMixTimeline.SetFrame(num87, num83, num84, num85, num86);
							if (!keyMapEnumerator3.MoveNext())
							{
								break;
							}
							Dictionary<string, object> obj7 = (Dictionary<string, object>)keyMapEnumerator3.Current;
							float num89 = GetFloat(obj7, "time", 0f);
							float num90 = GetFloat(obj7, "mixRotate", 1f);
							float num91 = GetFloat(obj7, "mixX", 1f);
							float num92 = GetFloat(obj7, "mixY", num91);
							if (dictionary8.ContainsKey("curve"))
							{
								object curve7 = dictionary8["curve"];
								num88 = ReadCurve(curve7, pathConstraintMixTimeline, num88, num87, 0, num83, num89, num84, num90, 1f);
								num88 = ReadCurve(curve7, pathConstraintMixTimeline, num88, num87, 1, num83, num89, num85, num91, 1f);
								num88 = ReadCurve(curve7, pathConstraintMixTimeline, num88, num87, 2, num83, num89, num86, num92, 1f);
							}
							num83 = num89;
							num84 = num90;
							num85 = num91;
							num86 = num92;
							dictionary8 = obj7;
							num87++;
						}
						pathConstraintMixTimeline.Shrink(num88);
						exposedList.Add(pathConstraintMixTimeline);
						break;
					}
					}
				}
			}
		}
		if (map.ContainsKey("deform"))
		{
			foreach (KeyValuePair<string, object> item12 in (Dictionary<string, object>)map["deform"])
			{
				Skin skin = skeletonData.FindSkin(item12.Key);
				foreach (KeyValuePair<string, object> item13 in (Dictionary<string, object>)item12.Value)
				{
					int slotIndex2 = FindSlotIndex(skeletonData, item13.Key);
					foreach (KeyValuePair<string, object> item14 in (Dictionary<string, object>)item13.Value)
					{
						List<object> list6 = (List<object>)item14.Value;
						List<object>.Enumerator enumerator11 = list6.GetEnumerator();
						if (!enumerator11.MoveNext())
						{
							continue;
						}
						Dictionary<string, object> dictionary9 = (Dictionary<string, object>)enumerator11.Current;
						VertexAttachment vertexAttachment = (VertexAttachment)skin.GetAttachment(slotIndex2, item14.Key);
						if (vertexAttachment == null)
						{
							throw new Exception("Deform attachment not found: " + item14.Key);
						}
						bool flag = vertexAttachment.bones != null;
						float[] vertices = vertexAttachment.vertices;
						int num93 = (flag ? (vertices.Length / 3 << 1) : vertices.Length);
						DeformTimeline deformTimeline = new DeformTimeline(list6.Count, list6.Count, slotIndex2, vertexAttachment);
						float num94 = GetFloat(dictionary9, "time", 0f);
						int num95 = 0;
						int num96 = 0;
						while (true)
						{
							float[] array;
							if (!dictionary9.ContainsKey("vertices"))
							{
								array = (flag ? new float[num93] : vertices);
							}
							else
							{
								array = new float[num93];
								int num97 = GetInt(dictionary9, "offset", 0);
								float[] floatArray = GetFloatArray(dictionary9, "vertices", 1f);
								Array.Copy(floatArray, 0, array, num97, floatArray.Length);
								if (num != 1f)
								{
									int j = num97;
									for (int num98 = j + floatArray.Length; j < num98; j++)
									{
										array[j] *= num;
									}
								}
								if (!flag)
								{
									for (int k = 0; k < num93; k++)
									{
										array[k] += vertices[k];
									}
								}
							}
							deformTimeline.SetFrame(num95, num94, array);
							if (!enumerator11.MoveNext())
							{
								break;
							}
							Dictionary<string, object> obj8 = (Dictionary<string, object>)enumerator11.Current;
							float num99 = GetFloat(obj8, "time", 0f);
							if (dictionary9.ContainsKey("curve"))
							{
								num96 = ReadCurve(dictionary9["curve"], deformTimeline, num96, num95, 0, num94, num99, 0f, 1f, 1f);
							}
							num94 = num99;
							dictionary9 = obj8;
							num95++;
						}
						deformTimeline.Shrink(num96);
						exposedList.Add(deformTimeline);
					}
				}
			}
		}
		if (map.ContainsKey("drawOrder"))
		{
			List<object> obj9 = (List<object>)map["drawOrder"];
			DrawOrderTimeline drawOrderTimeline = new DrawOrderTimeline(obj9.Count);
			int count5 = skeletonData.slots.Count;
			int num100 = 0;
			foreach (Dictionary<string, object> item15 in obj9)
			{
				int[] array2 = null;
				if (item15.ContainsKey("offsets"))
				{
					array2 = new int[count5];
					for (int num101 = count5 - 1; num101 >= 0; num101--)
					{
						array2[num101] = -1;
					}
					List<object> list7 = (List<object>)item15["offsets"];
					int[] array3 = new int[count5 - list7.Count];
					int num102 = 0;
					int num103 = 0;
					foreach (Dictionary<string, object> item16 in list7)
					{
						int num104 = FindSlotIndex(skeletonData, (string)item16["slot"]);
						while (num102 != num104)
						{
							array3[num103++] = num102++;
						}
						int num105 = num102 + (int)(float)item16["offset"];
						array2[num105] = num102++;
					}
					while (num102 < count5)
					{
						array3[num103++] = num102++;
					}
					for (int num106 = count5 - 1; num106 >= 0; num106--)
					{
						if (array2[num106] == -1)
						{
							array2[num106] = array3[--num103];
						}
					}
				}
				drawOrderTimeline.SetFrame(num100, GetFloat(item15, "time", 0f), array2);
				num100++;
			}
			exposedList.Add(drawOrderTimeline);
		}
		if (map.ContainsKey("events"))
		{
			List<object> obj10 = (List<object>)map["events"];
			EventTimeline eventTimeline = new EventTimeline(obj10.Count);
			int num107 = 0;
			foreach (Dictionary<string, object> item17 in obj10)
			{
				EventData eventData = skeletonData.FindEvent((string)item17["name"]);
				if (eventData == null)
				{
					throw new Exception("Event not found: " + item17["name"]);
				}
				Event obj11 = new Event(GetFloat(item17, "time", 0f), eventData)
				{
					intValue = GetInt(item17, "int", eventData.Int),
					floatValue = GetFloat(item17, "float", eventData.Float),
					stringValue = GetString(item17, "string", eventData.String)
				};
				if (obj11.data.AudioPath != null)
				{
					obj11.volume = GetFloat(item17, "volume", eventData.Volume);
					obj11.balance = GetFloat(item17, "balance", eventData.Balance);
				}
				eventTimeline.SetFrame(num107, obj11);
				num107++;
			}
			exposedList.Add(eventTimeline);
		}
		exposedList.TrimExcess();
		float num108 = 0f;
		Timeline[] items2 = exposedList.Items;
		int l = 0;
		for (int count6 = exposedList.Count; l < count6; l++)
		{
			num108 = Math.Max(num108, items2[l].Duration);
		}
		skeletonData.animations.Add(new Animation(name, exposedList, num108));
	}

	private static Timeline ReadTimeline(ref List<object>.Enumerator keyMapEnumerator, CurveTimeline1 timeline, float defaultValue, float scale)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)keyMapEnumerator.Current;
		float num = GetFloat(dictionary, "time", 0f);
		float num2 = GetFloat(dictionary, "value", defaultValue) * scale;
		int num3 = 0;
		int num4 = 0;
		while (true)
		{
			timeline.SetFrame(num3, num, num2);
			if (!keyMapEnumerator.MoveNext())
			{
				break;
			}
			Dictionary<string, object> obj = (Dictionary<string, object>)keyMapEnumerator.Current;
			float num5 = GetFloat(obj, "time", 0f);
			float num6 = GetFloat(obj, "value", defaultValue) * scale;
			if (dictionary.ContainsKey("curve"))
			{
				num4 = ReadCurve(dictionary["curve"], timeline, num4, num3, 0, num, num5, num2, num6, scale);
			}
			num = num5;
			num2 = num6;
			dictionary = obj;
			num3++;
		}
		timeline.Shrink(num4);
		return timeline;
	}

	private static Timeline ReadTimeline(ref List<object>.Enumerator keyMapEnumerator, CurveTimeline2 timeline, string name1, string name2, float defaultValue, float scale)
	{
		Dictionary<string, object> dictionary = (Dictionary<string, object>)keyMapEnumerator.Current;
		float num = GetFloat(dictionary, "time", 0f);
		float value = GetFloat(dictionary, name1, defaultValue) * scale;
		float num2 = GetFloat(dictionary, name2, defaultValue) * scale;
		int num3 = 0;
		int num4 = 0;
		while (true)
		{
			timeline.SetFrame(num3, num, value, num2);
			if (!keyMapEnumerator.MoveNext())
			{
				break;
			}
			Dictionary<string, object> obj = (Dictionary<string, object>)keyMapEnumerator.Current;
			float num5 = GetFloat(obj, "time", 0f);
			float num6 = GetFloat(obj, name1, defaultValue) * scale;
			float num7 = GetFloat(obj, name2, defaultValue) * scale;
			if (dictionary.ContainsKey("curve"))
			{
				object curve = dictionary["curve"];
				num4 = ReadCurve(curve, timeline, num4, num3, 0, num, num5, value, num6, scale);
				num4 = ReadCurve(curve, timeline, num4, num3, 1, num, num5, num2, num7, scale);
			}
			num = num5;
			value = num6;
			num2 = num7;
			dictionary = obj;
			num3++;
		}
		timeline.Shrink(num4);
		return timeline;
	}

	private static int ReadCurve(object curve, CurveTimeline timeline, int bezier, int frame, int value, float time1, float time2, float value1, float value2, float scale)
	{
		if (curve is string text)
		{
			if (text == "stepped")
			{
				timeline.SetStepped(frame);
			}
			return bezier;
		}
		List<object> obj = (List<object>)curve;
		int num = value << 2;
		float cx = (float)obj[num];
		float cy = (float)obj[num + 1] * scale;
		float cx2 = (float)obj[num + 2];
		float cy2 = (float)obj[num + 3] * scale;
		SetBezier(timeline, frame, value, bezier, time1, value1, cx, cy, cx2, cy2, time2, value2);
		return bezier + 1;
	}

	private static void SetBezier(CurveTimeline timeline, int frame, int value, int bezier, float time1, float value1, float cx1, float cy1, float cx2, float cy2, float time2, float value2)
	{
		timeline.SetBezier(bezier, frame, value, time1, value1, cx1, cy1, cx2, cy2, time2, value2);
	}

	private static float[] GetFloatArray(Dictionary<string, object> map, string name, float scale)
	{
		List<object> list = (List<object>)map[name];
		float[] array = new float[list.Count];
		if (scale == 1f)
		{
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				array[i] = (float)list[i];
			}
		}
		else
		{
			int j = 0;
			for (int count2 = list.Count; j < count2; j++)
			{
				array[j] = (float)list[j] * scale;
			}
		}
		return array;
	}

	private static int[] GetIntArray(Dictionary<string, object> map, string name)
	{
		List<object> list = (List<object>)map[name];
		int[] array = new int[list.Count];
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			array[i] = (int)(float)list[i];
		}
		return array;
	}

	private static float GetFloat(Dictionary<string, object> map, string name, float defaultValue)
	{
		if (!map.ContainsKey(name))
		{
			return defaultValue;
		}
		return (float)map[name];
	}

	private static int GetInt(Dictionary<string, object> map, string name, int defaultValue)
	{
		if (!map.ContainsKey(name))
		{
			return defaultValue;
		}
		return (int)(float)map[name];
	}

	private static bool GetBoolean(Dictionary<string, object> map, string name, bool defaultValue)
	{
		if (!map.ContainsKey(name))
		{
			return defaultValue;
		}
		return (bool)map[name];
	}

	private static string GetString(Dictionary<string, object> map, string name, string defaultValue)
	{
		if (!map.ContainsKey(name))
		{
			return defaultValue;
		}
		return (string)map[name];
	}

	private static float ToColor(string hexString, int colorIndex, int expectedLength = 8)
	{
		if (hexString.Length != expectedLength)
		{
			throw new ArgumentException("Color hexidecimal length must be " + expectedLength + ", recieved: " + hexString, "hexString");
		}
		return (float)Convert.ToInt32(hexString.Substring(colorIndex * 2, 2), 16) / 255f;
	}
}
