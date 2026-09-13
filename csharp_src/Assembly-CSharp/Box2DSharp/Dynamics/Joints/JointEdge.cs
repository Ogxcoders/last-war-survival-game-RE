using System;
using System.Collections.Generic;

namespace Box2DSharp.Dynamics.Joints;

public struct JointEdge : IDisposable
{
	public Body Other;

	public Joint Joint;

	public LinkedListNode<JointEdge> Node;

	public void Dispose()
	{
		Other = null;
		Joint = null;
		Node = null;
	}
}
