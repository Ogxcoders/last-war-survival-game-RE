using System;
using System.Collections.Generic;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Box2DSharp.Dynamics.Contacts;
using Leopotam.EcsLite;

namespace Box2DSharp.Foreign;

public class S5Game : Box2DGame
{
	public class BodyLogic : IBodyLogic
	{
		public S5GameColliderLayer Layer
		{
			get
			{
				return (S5GameColliderLayer)ILayer;
			}
			set
			{
				ILayer = (int)value;
			}
		}
	}

	public class PlatformLogic : BodyLogic, IPlatformLogic
	{
		public List<int> OnPlatformIDs { get; set; }

		public new virtual IBodyLogic Clone()
		{
			return new PlatformLogic
			{
				ID = ID,
				ILayer = ILayer,
				OwnerID = OwnerID,
				EntityId = EcsPackedEntity.Invalid,
				CanOnPlatform = CanOnPlatform,
				Die = Die,
				OnPlatformIDs = ((OnPlatformIDs != null) ? new List<int>(OnPlatformIDs) : null)
			};
		}
	}

	[Flags]
	public enum S5GameColliderLayer
	{
		None = 0,
		Player = 1,
		Enemy = 2,
		Bullet = 4,
		Environment = 8,
		Obstacle = 0x10,
		WoodBarrel = 0x20,
		Bomb = 0x40,
		Toggle = 0x80,
		BulletObstacle = 0x100
	}

	[Flags]
	public enum FixtureType
	{
		None = 1,
		Head = 2,
		Body = 4
	}

	private S5GameSettings S5GameSettings => Settings as S5GameSettings;

	public Body CreatWall(FVector2[][] vector2, BodyType bodyType)
	{
		BodyDef def = new BodyDef
		{
			BodyType = bodyType,
			UserData = new BodyLogic
			{
				Layer = S5GameColliderLayer.Environment,
				CanOnPlatform = false
			}
		};
		Body body = World.CreateBody(in def);
		for (int i = 0; i < vector2.Length; i++)
		{
			ChainShape chainShape = new ChainShape();
			FVector2[] array = vector2[i];
			chainShape.CreateLoop(array, array.Length);
			body.CreateFixture(new FixtureDef
			{
				Density = 10,
				Friction = 1,
				Restitution = 0f,
				RestitutionThreshold = 0.01f,
				Shape = chainShape,
				Filter = new Filter
				{
					CategoryBits = 8,
					MaskBits = 119
				}
			});
		}
		return body;
	}

	public Body FireBullet(int ownerID, FP radius, FVector2 position, FVector2 velocity)
	{
		CircleShape circleShape = new CircleShape();
		circleShape.Radius = radius;
		FixtureDef def = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 1f,
			RestitutionThreshold = 0.01f,
			Shape = circleShape,
			Filter = new Filter
			{
				GroupIndex = -4,
				CategoryBits = 4,
				MaskBits = 251
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = BodyType.DynamicBody,
			GravityScale = 0,
			Position = position,
			LinearVelocity = velocity,
			Bullet = true,
			UserData = new BodyLogic
			{
				Layer = S5GameColliderLayer.Bullet,
				OwnerID = ownerID,
				CanOnPlatform = false
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body FirePvpBullet(int ownerID, FP radius, FVector2 position, FVector2 velocity, FP baseRadius, FVector2 basePos)
	{
		CircleShape circleShape = new CircleShape();
		circleShape.Radius = radius;
		FixtureDef def = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 1f,
			RestitutionThreshold = 0.01f,
			Shape = circleShape,
			Filter = new Filter
			{
				GroupIndex = -4,
				CategoryBits = 4,
				MaskBits = 251
			}
		};
		CircleShape circleShape2 = new CircleShape();
		circleShape2.Radius = baseRadius;
		circleShape2.Position = basePos;
		FixtureDef def2 = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 1f,
			RestitutionThreshold = 0.01f,
			Shape = circleShape2,
			Filter = new Filter
			{
				GroupIndex = -256,
				CategoryBits = 256,
				MaskBits = 256
			}
		};
		BodyDef def3 = new BodyDef
		{
			BodyType = BodyType.DynamicBody,
			GravityScale = 0,
			Position = position,
			LinearVelocity = velocity,
			Bullet = true,
			FixedRotation = true,
			UserData = new BodyLogic
			{
				Layer = S5GameColliderLayer.Bullet,
				OwnerID = ownerID,
				CanOnPlatform = false
			}
		};
		Body body = World.CreateBody(in def3);
		body.CreateFixture(def);
		body.CreateFixture(def2);
		return body;
	}

	public Body CreatePlayer(int ownerID, Shape headShape, Shape bodyShape, BodyType bodyType)
	{
		FixtureDef def = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 0,
			Shape = headShape,
			UserData = new IFixtureLogic
			{
				IType = 2
			},
			Filter = new Filter
			{
				CategoryBits = 1,
				MaskBits = 120
			}
		};
		FixtureDef def2 = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 0,
			Shape = bodyShape,
			UserData = new IFixtureLogic
			{
				IType = 4
			},
			Filter = new Filter
			{
				CategoryBits = 1,
				MaskBits = 120
			}
		};
		BodyDef def3 = new BodyDef
		{
			BodyType = bodyType,
			FixedRotation = true,
			Position = FVector2.Zero,
			GravityScale = 1,
			UserData = new BodyLogic
			{
				Layer = S5GameColliderLayer.Player,
				OwnerID = ownerID,
				CanOnPlatform = true
			}
		};
		Body body = World.CreateBody(in def3);
		body.CreateFixture(def);
		body.CreateFixture(def2);
		return body;
	}

	public Body CreateEnemy(int ownerID, Shape headShape, Shape bodyShape, BodyType bodyType)
	{
		FixtureDef def = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 0,
			Shape = headShape,
			UserData = new IFixtureLogic
			{
				IType = 2
			},
			Filter = new Filter
			{
				CategoryBits = 2,
				MaskBits = 124
			}
		};
		FixtureDef def2 = new FixtureDef
		{
			Density = 1,
			Friction = 0,
			Restitution = 0,
			Shape = bodyShape,
			UserData = new IFixtureLogic
			{
				IType = 4
			},
			Filter = new Filter
			{
				CategoryBits = 2,
				MaskBits = 124
			}
		};
		BodyDef def3 = new BodyDef
		{
			BodyType = bodyType,
			Position = FVector2.Zero,
			FixedRotation = true,
			GravityScale = 1,
			UserData = new BodyLogic
			{
				Layer = S5GameColliderLayer.Enemy,
				OwnerID = ownerID,
				CanOnPlatform = true
			}
		};
		Body body = World.CreateBody(in def3);
		body.CreateFixture(def);
		body.CreateFixture(def2);
		return body;
	}

	public Body CreateObstacle(FVector2 center, FVector2 size, FVector2 position, BodyType bodyType)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.SetAsBox(size.X, size.Y, in center, 0);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				CategoryBits = 16,
				MaskBits = 103
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = position,
			GravityScale = 0,
			UserData = new PlatformLogic
			{
				ILayer = 16,
				CanOnPlatform = false
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body CreateStaticObstacle(FVector2 center, FVector2 size, FVector2 position, BodyType bodyType)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.SetAsBox(size.X, size.Y, in center, 0);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				CategoryBits = 16,
				MaskBits = 103
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = position,
			GravityScale = 0,
			UserData = new PlatformLogic
			{
				ILayer = 16,
				CanOnPlatform = false
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body CreateStaticObstacle(FVector2[] points, BodyType bodyType, FVector2 position)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.Set(points, points.Length);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				CategoryBits = 16,
				MaskBits = 103
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = position,
			GravityScale = 0,
			UserData = new PlatformLogic
			{
				ILayer = 16,
				CanOnPlatform = false
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body CreateWoodBarrel(FVector2 center, FVector2 size, FVector2 position, BodyType bodyType)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.SetAsBox(size.X, size.Y, in center, 0);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				GroupIndex = 32,
				CategoryBits = 32,
				MaskBits = 95
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = position,
			FixedRotation = true,
			GravityScale = 1,
			UserData = new BodyLogic
			{
				ILayer = 32,
				CanOnPlatform = true
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body CreateBomb(FVector2 center, FVector2 size, FVector2 position, BodyType bodyType)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.SetAsBox(size.X, size.Y, in center, 0);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				GroupIndex = 64,
				CategoryBits = 64,
				MaskBits = 63
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = position,
			FixedRotation = true,
			GravityScale = 1,
			UserData = new BodyLogic
			{
				ILayer = 64,
				CanOnPlatform = true
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body CreateBombNew(FVector2[] points, BodyType bodyType)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.Set(points, points.Length);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				GroupIndex = 64,
				CategoryBits = 64,
				MaskBits = 63
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = FVector2.Zero,
			FixedRotation = true,
			GravityScale = 1,
			UserData = new BodyLogic
			{
				ILayer = 32,
				CanOnPlatform = true
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public Body CreateToggle(FVector2 center, FVector2 size, FVector2 position, FP angle, BodyType bodyType)
	{
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.SetAsBox(size.X, size.Y, in center, 0);
		FixtureDef def = new FixtureDef
		{
			Density = 50,
			Friction = 1,
			Restitution = 0,
			Shape = polygonShape,
			IsSensor = false,
			Filter = new Filter
			{
				CategoryBits = 128,
				MaskBits = 4
			}
		};
		BodyDef def2 = new BodyDef
		{
			BodyType = bodyType,
			Position = position,
			FixedRotation = true,
			GravityScale = 0,
			UserData = new BodyLogic
			{
				ILayer = 128
			}
		};
		Body body = World.CreateBody(in def2);
		body.CreateFixture(def);
		return body;
	}

	public override void BeginContact(Contact contact)
	{
		base.BeginContact(contact);
		Box2DTrigger?.OnCollider(contact);
	}

	public override void EndContact(Contact contact)
	{
		base.EndContact(contact);
		Box2DTrigger?.EndCollider(contact);
	}

	public override void PreSolve(Contact contact, in Manifold oldManifold)
	{
		base.PreSolve(contact, in oldManifold);
		IBodyLogic bodyLogic = contact.FixtureA.Body.UserData as IBodyLogic;
		IBodyLogic bodyLogic2 = contact.FixtureB.Body.UserData as IBodyLogic;
		if (bodyLogic != null && bodyLogic2 != null && ((bodyLogic.ILayer == 4 && bodyLogic2.ILayer == 1) || (bodyLogic2.ILayer == 4 && bodyLogic.ILayer == 1)))
		{
			contact.SetEnabled(flag: false);
		}
		Box2DTrigger?.OnSolve(contact);
	}

	public override bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
	{
		IBodyLogic bodyLogic = fixtureA.Body.UserData as IBodyLogic;
		IBodyLogic bodyLogic2 = fixtureB.Body.UserData as IBodyLogic;
		if (bodyLogic != null && bodyLogic2 != null)
		{
			if ((bodyLogic.Die || bodyLogic2.Die) && (bodyLogic.ILayer == 4 || bodyLogic2.ILayer == 4))
			{
				return false;
			}
			if (bodyLogic.ILayer == 1 && bodyLogic2.ILayer == 1)
			{
				return true;
			}
			if (bodyLogic.ILayer == 2 && bodyLogic2.ILayer == 2)
			{
				return true;
			}
			S5GameColliderLayer s5GameColliderLayer = (S5GameColliderLayer)(bodyLogic.ILayer | bodyLogic2.ILayer);
			if (s5GameColliderLayer.HasFlag(S5GameColliderLayer.Player) && s5GameColliderLayer.HasFlag(S5GameColliderLayer.Enemy))
			{
				return true;
			}
		}
		return base.ShouldCollide(fixtureA, fixtureB);
	}
}
