using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class Box2DTriggerAdapt : IBox2DTrigger
{
	private EcsWorld _world;

	private SharedRuntime _shared;

	private S5Game _game;

	public Box2DTriggerAdapt(EcsWorld world, SharedRuntime shared, S5Game game)
	{
		_world = world;
		_game = game;
		_shared = shared;
	}

	public void OnCollider(Contact contact)
	{
		if (!contact.IsEnabled)
		{
			return;
		}
		_world.GetShared<GameSharedEnv>().FrameSyncIsNeeded = true;
		S5Game.BodyLogic bodyLogic = contact.FixtureA.Body.UserData as S5Game.BodyLogic;
		S5Game.BodyLogic bodyLogic2 = contact.FixtureB.Body.UserData as S5Game.BodyLogic;
		S5Game.S5GameColliderLayer s5GameColliderLayer = bodyLogic.Layer | bodyLogic2.Layer;
		if (!s5GameColliderLayer.HasFlag(S5Game.S5GameColliderLayer.Environment) || s5GameColliderLayer.HasFlag(S5Game.S5GameColliderLayer.Bullet))
		{
			contact.GetWorldManifold(out var worldManifold);
			int senderFixtureType = 1;
			int targetFixtureType = 1;
			if (contact.FixtureA.UserData != null && contact.FixtureA.UserData is IFixtureLogic fixtureLogic)
			{
				senderFixtureType = fixtureLogic.IType;
			}
			if (contact.FixtureB.UserData != null && contact.FixtureB.UserData is IFixtureLogic fixtureLogic2)
			{
				targetFixtureType = fixtureLogic2.IType;
			}
			FuncEvent.Broadcast(ref FuncEvent.GetComponentEventManager(_world), new EventPhysicCollection(bodyLogic.EntityId, bodyLogic2.EntityId, worldManifold, bodyLogic.ILayer, bodyLogic2.ILayer, senderFixtureType, targetFixtureType));
		}
	}

	public void EndCollider(Contact contact)
	{
	}

	public void OnSolve(Contact contact)
	{
	}
}
