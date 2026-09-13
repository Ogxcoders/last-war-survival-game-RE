using UnityEngine;

public class S4MonsterEffList : MonoBehaviour
{
	private enum State
	{
		None,
		AfraidLight,
		Dark,
		BloodNight
	}

	[SerializeField]
	private GameObject[] darkEffects;

	[SerializeField]
	private GameObject[] bloodNightEffects;

	private WorldMarch marchInfo;

	private WorldTroop troop;

	private WorldScene world;

	private bool subscribed;

	private State curState;

	private void ShowBloodNightEffect(bool isShow = true)
	{
		GameObject[] array = bloodNightEffects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i]?.SetActive(isShow);
		}
	}

	private void ShowDarkEffect(bool isShow = true)
	{
		GameObject[] array = darkEffects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i]?.SetActive(isShow);
		}
	}

	public void Init(WorldTroop troop, WorldScene world, WorldMarch marchInfo)
	{
		switch (world.GetBloodyNightState())
		{
		case BloodyNightState.None:
			curState = State.AfraidLight;
			break;
		case BloodyNightState.Bloody:
			curState = State.BloodNight;
			ShowBloodNightEffect();
			ShowDarkEffect();
			break;
		case BloodyNightState.Silent:
			if (darkEffects.Length != 0 && !LightDataManager.GetInstance().IsLightUpInPointId(marchInfo.startPos))
			{
				curState = State.Dark;
				ShowDarkEffect();
			}
			else
			{
				curState = State.AfraidLight;
			}
			break;
		}
		this.marchInfo = marchInfo;
		this.world = world;
		this.troop = troop;
		if (darkEffects.Length != 0 && !subscribed)
		{
			GameEntry.Event.Subscribe(EventId.PushLightChangeInWhiteNight, OnPushLightChangeInWhiteNight);
			subscribed = true;
		}
	}

	public void Dispose()
	{
		ShowBloodNightEffect(isShow: false);
		ShowDarkEffect(isShow: false);
		marchInfo = null;
		world = null;
		troop = null;
		if (subscribed)
		{
			GameEntry.Event.Unsubscribe(EventId.PushLightChangeInWhiteNight, OnPushLightChangeInWhiteNight);
			subscribed = false;
		}
	}

	private void OnPushLightChangeInWhiteNight(object param)
	{
		if (!(param is int num))
		{
			return;
		}
		int num2 = num / 10000000;
		int index = num % 10000000;
		Vector2Int vector2Int = world.IndexToTilePos(index);
		int startPos = marchInfo.startPos;
		Vector2Int vector2Int2 = world.IndexToTilePos(startPos);
		if (vector2Int.x - num2 <= vector2Int2.x && vector2Int.x + num2 >= vector2Int2.x && vector2Int.y - num2 <= vector2Int2.y && vector2Int.y + num2 >= vector2Int2.y)
		{
			if (LightDataManager.GetInstance().IsLightUpInPointId(marchInfo.startPos))
			{
				curState = State.AfraidLight;
				ShowDarkEffect(isShow: false);
				troop.TryPlayS4IdleAnim("idle02");
			}
			else
			{
				curState = State.Dark;
				ShowDarkEffect();
				troop.TryPlayS4IdleAnim("idle");
			}
		}
	}

	public void ShowAfraidLight()
	{
		if (curState != State.AfraidLight)
		{
			curState = State.AfraidLight;
			ShowDarkEffect(isShow: false);
			troop.TryPlayS4IdleAnim("idle02");
		}
	}
}
