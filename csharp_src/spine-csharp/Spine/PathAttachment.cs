using System;

namespace Spine;

public class PathAttachment : VertexAttachment
{
	internal float[] lengths;

	internal bool closed;

	internal bool constantSpeed;

	public float[] Lengths
	{
		get
		{
			return lengths;
		}
		set
		{
			lengths = value;
		}
	}

	public bool Closed
	{
		get
		{
			return closed;
		}
		set
		{
			closed = value;
		}
	}

	public bool ConstantSpeed
	{
		get
		{
			return constantSpeed;
		}
		set
		{
			constantSpeed = value;
		}
	}

	public PathAttachment(string name)
		: base(name)
	{
	}

	public override Attachment Copy()
	{
		PathAttachment pathAttachment = new PathAttachment(base.Name);
		CopyTo(pathAttachment);
		pathAttachment.lengths = new float[lengths.Length];
		Array.Copy(lengths, 0, pathAttachment.lengths, 0, lengths.Length);
		pathAttachment.closed = closed;
		pathAttachment.constantSpeed = constantSpeed;
		return pathAttachment;
	}
}
