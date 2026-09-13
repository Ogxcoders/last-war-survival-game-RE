using System.Text;
using UnityEngine;

public class CityAttachmentBuildPointObject : WorldPointObject
{
	private int buildId;

	private int pointId;

	private long uuid;

	private SuperTextMesh theRewardBubbleText;

	private SpriteRenderer theRewardBubbleIcon;

	private SimpleAnimation theRewardBubbleAnim;

	private InstanceRequest theBuildModel;

	private InstanceRequest theBubble;

	private int theCost;

	private SpriteRenderer theBuildBloodSlider;

	private SuperTextMesh theBuildBloodPosText;

	private SuperTextMesh theBuildBloodOwnerName;

	public CityAttachmentBuildPointObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	public override void CreateGameObject()
	{
		base.CreateGameObject();
		DestroyEffect();
		CityAttachmentBuildPointInfo info = world.GetPointInfo(pointIndex) as CityAttachmentBuildPointInfo;
		if (info == null)
		{
			return;
		}
		pointId = info.mainIndex;
		serverId = info.serverId;
		uuid = info.uuid;
		buildId = info.mBuildData.BuildId;
		AddOldObject();
		if (instance != null)
		{
			return;
		}
		string param = $"CityAttachmentSlotId,{pointId},{serverId}";
		int num = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", param);
		string text = $"Assets/Main/Prefabs/AllianceBuilding/CityAttachment/Slot{num}.prefab";
		if (text.IsNullOrEmpty())
		{
			return;
		}
		if (theBuildModel != null)
		{
			theBuildModel.Destroy();
			theBuildModel = null;
		}
		if (theBubble != null)
		{
			theBubble.Destroy();
			theBubble = null;
		}
		instance = GameEntry.Resource.InstantiateAsync(text);
		instance.completed += delegate
		{
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = base.WorldPosition;
				gameObject.SetActive(isVisible);
				string tabName = "season_builders_alliance_list";
				string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, buildId, "model");
				theBuildModel = WorldPointObject.AsyncLoad(templateData, gameObject.transform.Find("Model"));
				UpdateName(info);
				SetAutoAdjustLod();
				UpdateGameObject();
			}
		};
	}

	private void OnBubbleClick()
	{
		int selfServerId = GameEntry.Data.Player.GetSelfServerId();
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (selfServerId != curServerId)
		{
			UIUtils.ShowTips("season_tips143", 3f);
			return;
		}
		if (!GameEntry.Data.Player.GetData(uuid + "_record_cab").IsNullOrEmpty())
		{
			UIUtils.ShowTips("season_builders_alliance_tips_22", 3f);
			return;
		}
		GameEntry.Lua.Call("SFSNetwork.SendMessage", "city.attachment.rec.bubble", uuid);
		if (theRewardBubbleIcon != null)
		{
			theRewardBubbleIcon.LoadSprite("Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_giftbag_gray.png");
		}
		if (theRewardBubbleAnim != null)
		{
			theRewardBubbleAnim.Play("EnterBubble");
		}
	}

	private void TryShowBuildInfo(CityAttachmentBuildPointInfo info)
	{
		if (gameObject == null)
		{
			return;
		}
		Transform transform = gameObject.transform.Find("Model/Upgrade");
		int state = info.mBuildData.State;
		if (transform != null)
		{
			transform.gameObject.SetActive(state == 0);
		}
		int RewardNum = info.mBuildData.RewardNum;
		if (state != 1 || RewardNum == 0)
		{
			DestroyEffect();
		}
		else
		{
			if (RewardNum <= 0)
			{
				return;
			}
			string tabName = "season_builders_alliance_list";
			string RewardCount = GameEntry.ConfigCache.GetTemplateData(tabName, buildId, "bubble_reward_count");
			if (theRewardBubbleText != null)
			{
				if (RewardCount.IsNullOrEmpty())
				{
					theRewardBubbleText.text = RewardNum.ToString();
				}
				else
				{
					theRewardBubbleText.text = RewardNum + "/" + RewardCount;
				}
			}
			else
			{
				if (theBubble != null)
				{
					return;
				}
				string prefabPath = "Assets/Main/Prefabs/UI/State/BuildStateIconCAB.prefab";
				Transform parent = gameObject.transform.Find("Model");
				theBubble = WorldPointObject.AsyncLoad(prefabPath, parent, new Vector3(0f, 3f, 0f), new Vector3(1f, 1f, 1f), Quaternion.identity, delegate(InstanceRequest m)
				{
					GameObject gameObject = m.gameObject;
					TouchObjectEventTrigger componentInChildren = gameObject.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
					if (componentInChildren != null)
					{
						componentInChildren.onPointerClick = OnBubbleClick;
					}
					SpriteRenderer component = gameObject.transform.Find("Go/Bg/icon").GetComponent<SpriteRenderer>();
					string data = GameEntry.Data.Player.GetData(uuid + "_record_cab");
					int privateInt = GameEntry.Setting.GetPrivateInt("CityAttachmentBubbleCount", 5);
					SimpleAnimation component2 = gameObject.transform.Find("Go").GetComponent<SimpleAnimation>();
					if (data.IsNullOrEmpty() && privateInt > 0)
					{
						component.LoadSprite("Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_giftbag.png");
						if (component2 != null)
						{
							component2.Play("EnterBubble");
							component2.PlayQueued("NormalBubble");
						}
					}
					else
					{
						component.LoadSprite("Assets/Main/Sprites/UI/LWCommon/Sprite/Common_icon_giftbag_gray.png");
						if (component2 != null)
						{
							component2.Play("EnterBubble");
						}
					}
					theRewardBubbleIcon = component;
					theRewardBubbleAnim = component2;
					theRewardBubbleText = gameObject.transform.Find("Go/Bg/time").GetComponent<SuperTextMesh>();
					if (theRewardBubbleText != null)
					{
						if (RewardCount.IsNullOrEmpty())
						{
							theRewardBubbleText.text = RewardNum.ToString();
						}
						else
						{
							theRewardBubbleText.text = RewardNum + "/" + RewardCount;
						}
					}
				});
			}
		}
	}

	private void UpdateBloodTips(CityAttachmentBuildPointInfo info)
	{
		if (gameObject == null)
		{
			return;
		}
		Transform transform = gameObject.transform.Find("Model/BuildBloodTip");
		if (!(transform != null))
		{
			return;
		}
		if (info.mBuildData.State == 0)
		{
			if (theBuildBloodSlider == null)
			{
				theBuildBloodSlider = gameObject.transform.Find("Model/BuildBloodTip/PosGo/Bg/Slider").GetComponent<SpriteRenderer>();
				theBuildBloodPosText = gameObject.transform.Find("Model/BuildBloodTip/PosGo/Bg/PosText").GetComponent<SuperTextMesh>();
				theBuildBloodOwnerName = gameObject.transform.Find("Model/BuildBloodTip/OwnerNode/OwnerName").GetComponent<SuperTextMesh>();
			}
			if (theBuildBloodSlider != null)
			{
				transform.gameObject.SetActive(value: true);
				theBuildBloodOwnerName.text = "[" + info.mBuildData.AlAbbr + "]" + info.mBuildData.AlName;
				if (theCost == 0)
				{
					string tabName = "season_builders_alliance_list";
					int num = GameEntry.ConfigCache.GetTemplateData(tabName, info.mBuildData.BuildId, "cost").ToInt();
					if (num > 0)
					{
						theCost = num;
					}
				}
				long curExp = info.mBuildData.CurExp;
				if (curExp == 0L || theCost == 0)
				{
					theBuildBloodPosText.text = "0%";
					theBuildBloodSlider.size = new Vector2(0f, 1f);
				}
				else if (curExp == theCost)
				{
					theBuildBloodPosText.text = "100%";
					theBuildBloodSlider.size = new Vector2(7.8f, 1f);
				}
				else
				{
					double num2 = (float)curExp * 100f / (float)theCost;
					theBuildBloodPosText.text = num2.ToString("F2") + "%";
					theBuildBloodSlider.size = new Vector2(7.8f * (float)curExp / (float)theCost, 1f);
				}
			}
			else
			{
				transform.gameObject.SetActive(value: false);
			}
		}
		else
		{
			transform.gameObject.SetActive(value: false);
		}
	}

	private void UpdateName(CityAttachmentBuildPointInfo info)
	{
		bool flag = GameEntry.Data.Player.GetAllianceId() == info.mBuildData.AllianceId;
		UIWorldLabel[] componentsInChildren = gameObject.GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
		string templateData = GameEntry.ConfigCache.GetTemplateData("season_builders_alliance_list", info.mBuildData.BuildId, "name");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[" + info.mBuildData.AlAbbr + "]");
		if (!templateData.IsNullOrEmpty())
		{
			stringBuilder.Append(GameEntry.Localization.GetString(templateData));
		}
		UIWorldLabel[] array = componentsInChildren;
		foreach (UIWorldLabel obj in array)
		{
			obj.gameObject.SetActive(value: true);
			obj.SetNameBgSkin();
			obj.SetName(stringBuilder.ToString(), flag ? GameDefines.CityLabelColorType.Blue : GameDefines.CityLabelColorType.White);
		}
	}

	public override void Destroy()
	{
		if (theBuildModel != null)
		{
			theBuildModel.Destroy();
			theBuildModel = null;
		}
		DestroyEffect();
		base.Destroy();
	}

	public override void UpdateGameObject()
	{
		base.UpdateGameObject();
		if (!(world.GetPointInfo(pointIndex) is CityAttachmentBuildPointInfo cityAttachmentBuildPointInfo))
		{
			return;
		}
		serverId = cityAttachmentBuildPointInfo.serverId;
		if (cityAttachmentBuildPointInfo.mBuildData.RewardNum <= 0)
		{
			DestroyEffect();
		}
		else if (theRewardBubbleText != null)
		{
			string tabName = "season_builders_alliance_list";
			string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, buildId, "bubble_reward_count");
			if (templateData.IsNullOrEmpty())
			{
				theRewardBubbleText.text = cityAttachmentBuildPointInfo.mBuildData.RewardNum.ToString();
			}
			else
			{
				theRewardBubbleText.text = cityAttachmentBuildPointInfo.mBuildData.RewardNum + "/" + templateData;
			}
		}
		TryShowBuildInfo(cityAttachmentBuildPointInfo);
		UpdateBloodTips(cityAttachmentBuildPointInfo);
	}

	private void DestroyEffect()
	{
		theRewardBubbleText = null;
		theRewardBubbleIcon = null;
		theRewardBubbleAnim = null;
		if (theBubble != null)
		{
			theBubble.Destroy();
			theBubble = null;
		}
	}
}
