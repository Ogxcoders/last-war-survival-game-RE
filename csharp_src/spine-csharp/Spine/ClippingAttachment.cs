namespace Spine;

public class ClippingAttachment : VertexAttachment
{
	internal SlotData endSlot;

	public SlotData EndSlot
	{
		get
		{
			return endSlot;
		}
		set
		{
			endSlot = value;
		}
	}

	public ClippingAttachment(string name)
		: base(name)
	{
	}

	public override Attachment Copy()
	{
		ClippingAttachment clippingAttachment = new ClippingAttachment(base.Name);
		CopyTo(clippingAttachment);
		clippingAttachment.endSlot = endSlot;
		return clippingAttachment;
	}
}
