using System;
using System.Collections.Generic;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Joints;

public abstract class Joint
{
	public Body BodyA;

	public Body BodyB;

	public readonly bool CollideConnected;

	public JointEdge EdgeA;

	public JointEdge EdgeB;

	public int Index;

	public bool IslandFlag;

	public LinkedListNode<Joint> Node;

	public object UserData;

	public bool IsEnabled
	{
		get
		{
			if (BodyA.IsEnabled)
			{
				return BodyB.IsEnabled;
			}
			return false;
		}
	}

	public bool IsCollideConnected => CollideConnected;

	public JointType JointType { get; }

	internal Joint(JointDef def)
	{
		JointType = def.JointType;
		BodyA = def.BodyA;
		BodyB = def.BodyB;
		Index = 0;
		CollideConnected = def.CollideConnected;
		IslandFlag = false;
		UserData = def.UserData;
		EdgeA = default(JointEdge);
		EdgeB = default(JointEdge);
	}

	public abstract FVector2 GetAnchorA();

	public abstract FVector2 GetAnchorB();

	public abstract FVector2 GetReactionForce(FP inv_dt);

	public abstract FP GetReactionTorque(FP inv_dt);

	public virtual void Dump()
	{
		DumpLogger.Log("// Dump is not supported for this joint type.\n");
	}

	public virtual void ShiftOrigin(in FVector2 newOrigin)
	{
	}

	public virtual void Draw(IDraw draw)
	{
		Transform transform = BodyA.GetTransform();
		Transform transform2 = BodyB.GetTransform();
		FVector2 p = transform.Position;
		FVector2 p2 = transform2.Position;
		FVector2 p3 = GetAnchorA();
		FVector2 p4 = GetAnchorB();
		Color color = Color.FromArgb(0.5f, 0.8f, 0.8f);
		switch (JointType)
		{
		case JointType.DistanceJoint:
			draw.DrawSegment(in p3, in p4, in color);
			break;
		case JointType.PulleyJoint:
		{
			PulleyJoint obj = (PulleyJoint)this;
			FVector2 p5 = obj.GetGroundAnchorA();
			FVector2 p6 = obj.GetGroundAnchorB();
			draw.DrawSegment(in p5, in p3, in color);
			draw.DrawSegment(in p6, in p4, in color);
			draw.DrawSegment(in p5, in p6, in color);
			break;
		}
		case JointType.MouseJoint:
		{
			Color color2 = Color.FromArgb(0f, 1f, 0f);
			draw.DrawPoint(in p3, 4f, in color2);
			draw.DrawPoint(in p4, 4f, in color2);
			draw.DrawSegment(in p3, in p4, Color.FromArgb(0.8f, 0.8f, 0.8f));
			break;
		}
		default:
			draw.DrawSegment(in p, in p3, in color);
			draw.DrawSegment(in p3, in p4, in color);
			draw.DrawSegment(in p2, in p4, in color);
			break;
		}
	}

	internal abstract void InitVelocityConstraints(in SolverData data);

	internal abstract void SolveVelocityConstraints(in SolverData data);

	internal abstract bool SolvePositionConstraints(in SolverData data);

	internal static Joint Create(JointDef jointDef)
	{
		if (jointDef != null)
		{
			if (jointDef is DistanceJointDef def)
			{
				return new DistanceJoint(def);
			}
			if (jointDef is WheelJointDef def2)
			{
				return new WheelJoint(def2);
			}
			if (jointDef is MouseJointDef def3)
			{
				return new MouseJoint(def3);
			}
			if (jointDef is WeldJointDef def4)
			{
				return new WeldJoint(def4);
			}
			if (jointDef is PulleyJointDef def5)
			{
				return new PulleyJoint(def5);
			}
			if (jointDef is RevoluteJointDef def6)
			{
				return new RevoluteJoint(def6);
			}
			if (jointDef is FrictionJointDef def7)
			{
				return new FrictionJoint(def7);
			}
			if (jointDef is GearJointDef def8)
			{
				return new GearJoint(def8);
			}
			if (jointDef is MotorJointDef def9)
			{
				return new MotorJoint(def9);
			}
			if (jointDef is PrismaticJointDef def10)
			{
				return new PrismaticJoint(def10);
			}
		}
		throw new ArgumentOutOfRangeException("JointType");
	}
}
