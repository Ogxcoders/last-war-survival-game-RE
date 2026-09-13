namespace Spine;

public class BoundingBoxAttachment : VertexAttachment
{
	public BoundingBoxAttachment(string name)
		: base(name)
	{
	}

	public override Attachment Copy()
	{
		BoundingBoxAttachment boundingBoxAttachment = new BoundingBoxAttachment(base.Name);
		CopyTo(boundingBoxAttachment);
		return boundingBoxAttachment;
	}
}
