using System;

namespace Spine.Unity;

public class SpineAttachment : SpineAttributeBase
{
	public struct Hierarchy
	{
		public string skin;

		public string slot;

		public string name;

		public Hierarchy(string fullPath)
		{
			string[] array = fullPath.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				skin = "";
				slot = "";
				name = "";
				return;
			}
			if (array.Length < 2)
			{
				throw new Exception("Cannot generate Attachment Hierarchy from string! Not enough components! [" + fullPath + "]");
			}
			skin = array[0];
			slot = array[1];
			name = "";
			for (int i = 2; i < array.Length; i++)
			{
				name += array[i];
			}
		}
	}

	public bool returnAttachmentPath;

	public bool currentSkinOnly;

	public bool placeholdersOnly;

	public string skinField = "";

	public string slotField = "";

	public SpineAttachment(bool currentSkinOnly = true, bool returnAttachmentPath = false, bool placeholdersOnly = false, string slotField = "", string dataField = "", string skinField = "", bool includeNone = true, bool fallbackToTextField = false)
	{
		this.currentSkinOnly = currentSkinOnly;
		this.returnAttachmentPath = returnAttachmentPath;
		this.placeholdersOnly = placeholdersOnly;
		this.slotField = slotField;
		base.dataField = dataField;
		this.skinField = skinField;
		base.includeNone = includeNone;
		base.fallbackToTextField = fallbackToTextField;
	}

	public static Hierarchy GetHierarchy(string fullPath)
	{
		return new Hierarchy(fullPath);
	}

	public static Attachment GetAttachment(string attachmentPath, SkeletonData skeletonData)
	{
		Hierarchy hierarchy = GetHierarchy(attachmentPath);
		if (string.IsNullOrEmpty(hierarchy.name))
		{
			return null;
		}
		SlotData slotData = skeletonData.FindSlot(hierarchy.slot);
		if (slotData == null)
		{
			return null;
		}
		return skeletonData.FindSkin(hierarchy.skin).GetAttachment(slotData.Index, hierarchy.name);
	}

	public static Attachment GetAttachment(string attachmentPath, SkeletonDataAsset skeletonDataAsset)
	{
		return GetAttachment(attachmentPath, skeletonDataAsset.GetSkeletonData(quiet: true));
	}
}
