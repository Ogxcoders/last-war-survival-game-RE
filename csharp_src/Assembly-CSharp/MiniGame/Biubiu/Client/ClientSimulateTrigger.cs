using Box2DSharp.Common;
using Box2DSharp.Dynamics.Contacts;
using Box2DSharp.Foreign;
using Leopotam.EcsLite;

namespace MiniGame.Biubiu.Client;

public class ClientSimulateTrigger : IBox2DTrigger
{
	private EcsWorld _world;

	private SharedRuntime _shared;

	private S5Game _game;

	private CollectLayerEachOtherCondition _bulletCondition;

	private IAction _bulletAction;

	public ClientSimulateTrigger(EcsWorld world, SharedRuntime shared, S5Game game)
	{
		_world = world;
		_game = game;
		_shared = shared;
		_bulletCondition = new CollectLayerEachOtherCondition
		{
			ALayer = S5Game.S5GameColliderLayer.Bullet,
			BLayer = (S5Game.S5GameColliderLayer.Environment | S5Game.S5GameColliderLayer.Obstacle | S5Game.S5GameColliderLayer.WoodBarrel | S5Game.S5GameColliderLayer.Toggle)
		};
		_bulletAction = new EffectAction
		{
			EEffectType = EffectAction.EffectType.BulletWall
		};
	}

	public void OnCollider(Contact contact)
	{
		if (!contact.IsEnabled)
		{
			return;
		}
		S5Game.BodyLogic bodyLogic = contact.FixtureA.Body.UserData as S5Game.BodyLogic;
		S5Game.BodyLogic bodyLogic2 = contact.FixtureB.Body.UserData as S5Game.BodyLogic;
		S5Game.S5GameColliderLayer s5GameColliderLayer = bodyLogic.Layer | bodyLogic2.Layer;
		if (s5GameColliderLayer.HasFlag(S5Game.S5GameColliderLayer.Environment) && !s5GameColliderLayer.HasFlag(S5Game.S5GameColliderLayer.Bullet))
		{
			return;
		}
		if (bodyLogic.Layer == S5Game.S5GameColliderLayer.Bullet && bodyLogic2.Layer == S5Game.S5GameColliderLayer.Bullet)
		{
			contact.FixtureA.Body.SetLinearVelocity(FVector2.Zero);
			contact.FixtureB.Body.SetLinearVelocity(FVector2.Zero);
			return;
		}
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
		EventPhysicCollection eventPhysicCollection = new EventPhysicCollection(bodyLogic.EntityId, bodyLogic2.EntityId, worldManifold, bodyLogic.ILayer, bodyLogic2.ILayer, senderFixtureType, targetFixtureType);
		if (CollectLayerEachOtherCondition.Check(_world, -1, null, _bulletCondition, eventPhysicCollection))
		{
			FuncTCClient.DoEffectAction(_world, -1, _bulletAction, eventPhysicCollection);
		}
	}

	public void EndCollider(Contact contact)
	{
	}

	public void OnSolve(Contact contact)
	{
	}
}
