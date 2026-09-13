using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Dynamics.Joints;

namespace Box2DSharp.Foreign;

public abstract class Box2DGame : IContactListener, IContactFilter, IDestructionListener, IDisposable
{
	protected GameSettings Settings;

	public World World;

	public IBox2DTrigger Box2DTrigger;

	private List<Body> DelBodys;

	public void BindTrigger(IBox2DTrigger trigger)
	{
		Box2DTrigger = trigger;
	}

	public void Build(GameSettings settings)
	{
		Settings = settings;
		Regex.Replace(GetType().Name, "(\\B[A-Z])", " $1");
		World = new World(in Settings.Gravity);
		World.SetContactListener(this);
		World.SetContactFilter(this);
		World.DestructionListener = this;
		DelBodys = new List<Body>();
	}

	public void Dispose()
	{
		ClearBody();
		World?.Dispose();
		World = null;
	}

	public void Step(FP fp)
	{
		World.AllowSleep = Settings.EnableSleep;
		World.WarmStarting = Settings.EnableWarmStarting;
		World.ContinuousPhysics = Settings.EnableContinuous;
		World.SubStepping = Settings.EnableSubStepping;
		PreStep();
		PlatformStep();
		World.Step(fp, Settings.VelocityIterations, Settings.PositionIterations);
		PostStep();
	}

	private Body GetBodyByID(int Id)
	{
		if (World.BodyList == null)
		{
			return null;
		}
		foreach (Body body in World.BodyList)
		{
			if (body.UserData is IBodyLogic bodyLogic && bodyLogic.ID == Id)
			{
				return body;
			}
		}
		return null;
	}

	private void PlatformStep()
	{
		if (World.BodyList == null)
		{
			return;
		}
		foreach (Body body in World.BodyList)
		{
			if (!(body.UserData is IPlatformLogic { OnPlatformIDs: not null } platformLogic) || platformLogic.OnPlatformIDs.Count <= 0)
			{
				continue;
			}
			foreach (int onPlatformID in platformLogic.OnPlatformIDs)
			{
				GetBodyByID(onPlatformID)?.SetLinearVelocity(body.LinearVelocity);
			}
		}
	}

	protected virtual void PreStep()
	{
	}

	protected virtual void PostStep()
	{
		ClearBody();
	}

	public virtual void BeginContact(Contact contact)
	{
	}

	public virtual void EndContact(Contact contact)
	{
		IBodyLogic bodyLogic = contact.FixtureA.Body.UserData as IBodyLogic;
		IBodyLogic bodyLogic2 = contact.FixtureB.Body.UserData as IBodyLogic;
		if (bodyLogic is IPlatformLogic || bodyLogic2 is IPlatformLogic)
		{
			IPlatformLogic platformLogic = (IPlatformLogic)((bodyLogic is IPlatformLogic) ? bodyLogic : bodyLogic2);
			IBodyLogic bodyLogic3 = ((bodyLogic is IPlatformLogic) ? bodyLogic2 : bodyLogic);
			if (bodyLogic3 != null && bodyLogic3.CanOnPlatform && platformLogic.OnPlatformIDs != null && platformLogic.OnPlatformIDs.Contains(bodyLogic3.ID))
			{
				platformLogic.OnPlatformIDs.Remove(bodyLogic3.ID);
			}
		}
	}

	public virtual void PreSolve(Contact contact, in Manifold oldManifold)
	{
		IBodyLogic bodyLogic = contact.FixtureA.Body.UserData as IBodyLogic;
		IBodyLogic bodyLogic2 = contact.FixtureB.Body.UserData as IBodyLogic;
		if (!(bodyLogic is IPlatformLogic) && !(bodyLogic2 is IPlatformLogic))
		{
			return;
		}
		IPlatformLogic platformLogic = (IPlatformLogic)((bodyLogic is IPlatformLogic) ? bodyLogic : bodyLogic2);
		IBodyLogic bodyLogic3 = ((bodyLogic is IPlatformLogic) ? bodyLogic2 : bodyLogic);
		if (bodyLogic3 == null || !bodyLogic3.CanOnPlatform)
		{
			return;
		}
		if (platformLogic.OnPlatformIDs == null)
		{
			platformLogic.OnPlatformIDs = new List<int>();
		}
		bool num = bodyLogic is IPlatformLogic;
		contact.GetWorldManifold(out var worldManifold);
		bool flag = false;
		if ((!num) ? (worldManifold.Normal.Y.AsFloat <= -0.9f) : (worldManifold.Normal.Y.AsFloat >= 0.9f))
		{
			if (!platformLogic.OnPlatformIDs.Contains(bodyLogic3.ID))
			{
				platformLogic.OnPlatformIDs.Add(bodyLogic3.ID);
			}
		}
		else if (platformLogic.OnPlatformIDs.Contains(bodyLogic3.ID))
		{
			platformLogic.OnPlatformIDs.Remove(bodyLogic3.ID);
		}
	}

	public virtual void PostSolve(Contact contact, in ContactImpulse impulse)
	{
	}

	public virtual bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
	{
		IBodyLogic bodyLogic = fixtureA.Body.UserData as IBodyLogic;
		IBodyLogic bodyLogic2 = fixtureB.Body.UserData as IBodyLogic;
		if (bodyLogic == null || bodyLogic2 == null)
		{
			return true;
		}
		if (bodyLogic.OwnerID > 0 && bodyLogic2.OwnerID > 0 && bodyLogic.OwnerID == bodyLogic2.OwnerID)
		{
			return false;
		}
		return DefaultContactFilter.DoShouldCollide(fixtureA, fixtureB);
	}

	protected virtual void CanColliderAction(Fixture fixtureA, Fixture fixtureB)
	{
	}

	public virtual void SayGoodbye(Joint joint)
	{
	}

	public virtual void SayGoodbye(Fixture fixture)
	{
	}

	public void ClearBody()
	{
		if (DelBodys.Count > 0)
		{
			for (int num = DelBodys.Count - 1; num >= 0; num--)
			{
				World.DestroyBody(DelBodys[num]);
			}
			DelBodys.Clear();
		}
	}

	public void DestroyBody(Body body)
	{
		if (!DelBodys.Contains(body))
		{
			DelBodys.Add(body);
		}
	}
}
