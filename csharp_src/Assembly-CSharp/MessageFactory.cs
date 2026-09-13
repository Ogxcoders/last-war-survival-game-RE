using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameFramework;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using UnityEngine;

public class MessageFactory
{
	private static MessageFactory _instance;

	private static Dictionary<Type, BaseMessage> mHandlers;

	public float asyncDelayTime;

	private string _asyncDelayCmd;

	public HashSet<string> asyncDelayCmdSet;

	private static readonly Dictionary<string, Type> supportMessageTypes = new Dictionary<string, Type>
	{
		{
			"bind.gaid",
			typeof(UserBindGaidMessage)
		},
		{
			"build.main.city",
			typeof(BuildMainCityMessage)
		},
		{
			"user.modify.nickName.google",
			typeof(ChangePFdisplayName)
		},
		{
			"check.device.change",
			typeof(CheckDeviceChangeMessage)
		},
		{
			"cross.world.mv",
			typeof(CrossWorldMoveMessage)
		},
		{
			"change.user.parseid",
			typeof(FcmTokenMessage)
		},
		{
			"get.al.points",
			typeof(GetALPointsMessage)
		},
		{
			"get.server.list",
			typeof(GetServerListMessage)
		},
		{
			"world.get.new",
			typeof(GetViewLevelWorldInfoMessage)
		},
		{
			"praise.receive",
			typeof(FSTaskCommand)
		},
		{
			"push.popup.5star",
			typeof(Popup5starPush)
		},
		{
			"hot.spot.base.info",
			typeof(HotSpotBaseInfoMessage)
		},
		{
			"hot.spot.may.effect.me",
			typeof(HotSpotMayEffectMeMessage)
		},
		{
			"init.before",
			typeof(InitBeforeMessage)
		},
		{
			"init.after",
			typeof(InitAfterMessage)
		},
		{
			"init.error",
			typeof(InitErrorMessage)
		},
		{
			"init",
			typeof(InitMessage)
		},
		{
			"login.ext",
			typeof(LoginExtMessage)
		},
		{
			"login.init",
			typeof(LoginInitCommand)
		},
		{
			"login",
			typeof(LoginMessage)
		},
		{
			"login.push.shumei.exception.level",
			typeof(LoginPushShumeiExceptionLevelMessage)
		},
		{
			"logout",
			typeof(LogoutMessage)
		},
		{
			"push.aiHelp.status",
			typeof(PushAIHelpConversationStatus)
		},
		{
			"push.blood.queen.gunner.attack",
			typeof(PushBloodQueenGunnerAttackMessage)
		},
		{
			"push.hot.spot.event.del",
			typeof(PushHotSpotEventDelMessage)
		},
		{
			"push.hot.spot.patch",
			typeof(PushHotSpotPatchMessage)
		},
		{
			"push.lua.env",
			typeof(PushLuaEnvMessage)
		},
		{
			"push.monster.invasion.boss.progress",
			typeof(PushMonsterInvasionBossProgressMessage)
		},
		{
			"push.monster.invasion.summon",
			typeof(PushMonsterInvasionSummon)
		},
		{
			"push.world.point.update",
			typeof(PushPointInfoUpdate)
		},
		{
			"push.record",
			typeof(PushRecordMessage)
		},
		{
			"push.sandworm.delete",
			typeof(PushSandWormDeleteMessage)
		},
		{
			"push.sandworm.update",
			typeof(PushSandWormUpdateMessage)
		},
		{
			"push.thermal.conductor.info",
			typeof(PushThermalConductorInfoMessage)
		},
		{
			"push.update.light.data",
			typeof(PushUpdateLightDataMessage)
		},
		{
			"push.update.world.assistance.Info",
			typeof(PushUpdateWorldAssistanceInfoMessage)
		},
		{
			"push.user.off",
			typeof(PushUserOffMessage)
		},
		{
			"push.wolf.status.change",
			typeof(PushWolfStatusChangeMessage)
		},
		{
			"push.update.green.points",
			typeof(PushWorldAreaGreenUpdate)
		},
		{
			"push.battle.finish",
			typeof(PushWorldBattleFinishMessage)
		},
		{
			"push.battle.round.info",
			typeof(PushWorldBattleUpdateMessage)
		},
		{
			"push.world.desert.update",
			typeof(PushWorldDesertUpdate)
		},
		{
			"push.world.get.block",
			typeof(PushWorldGetBlock)
		},
		{
			"push.world.kill.skin",
			typeof(PushWorldKillSkinMessage)
		},
		{
			"push.world.land.update",
			typeof(PushWorldLandUpdate)
		},
		{
			"push.world.march.new",
			typeof(PushWorldMarchMessage)
		},
		{
			"push.world.march.del",
			typeof(PushWorldMarchDelMessage)
		},
		{
			"push.world.march.return.new",
			typeof(PushWorldMarchRetrunMessage)
		},
		{
			"push.city.be.move",
			typeof(PushWorldBeMoveMessage)
		},
		{
			"push.world.user.crash",
			typeof(PushWorldUserCrashMessage)
		},
		{
			"push.world.march.world.get.new",
			typeof(PushWorldMarchWorldGet)
		},
		{
			"push.world.obj.state.change",
			typeof(PushWorldObjStateChange)
		},
		{
			"push.world.trigger.del",
			typeof(PushWorldTriggerDelMessage)
		},
		{
			"push.world.trigger.update",
			typeof(PushWorldTriggerUpdateMessage)
		},
		{
			"season.get.zone.train.activity.info",
			typeof(SeasonGetZoneTrainActivityInfoMessage)
		},
		{
			"season.hunter.refresh.shadow.info",
			typeof(SeasonHunterRefreshShadowInfoMessage)
		},
		{
			"shumei.request",
			typeof(ShumeiSendDeviceIdMessage)
		},
		{
			"thermal.conductor.info",
			typeof(ThermalConductorInfoMessage)
		},
		{
			"user.clean.post",
			typeof(UserCleanPostMessage)
		},
		{
			"user.tower.change.listen",
			typeof(UserTowerChangeListenMessage)
		},
		{
			"world.get.block",
			typeof(WorldGetBlockMessage)
		},
		{
			"user.leave.world",
			typeof(WorldLeaveCrossServerMessage)
		},
		{
			"world.march.formation.new",
			typeof(WorldMarchFormationMessage)
		},
		{
			"train.send",
			typeof(WorldMarchTrainSendMessage)
		},
		{
			"train.batch.send",
			typeof(WorldMarchTrainListSendMessage)
		},
		{
			"world.march.change",
			typeof(WorldMarchFormationChangeMessage)
		},
		{
			"world.march.speed.up",
			typeof(WorldMarchFormationRapidMessage)
		},
		{
			"world.get.march.infos",
			typeof(WorldGetRectMarchInfosMessage)
		},
		{
			"create.zendesk.token",
			typeof(ZendeskJWTTokenMessage)
		}
	};

	public static MessageFactory Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MessageFactory();
			}
			return _instance;
		}
	}

	public string asyncDelayCmd
	{
		get
		{
			return _asyncDelayCmd;
		}
		set
		{
			_asyncDelayCmd = value;
			if (string.IsNullOrWhiteSpace(_asyncDelayCmd))
			{
				asyncDelayCmdSet = null;
				return;
			}
			asyncDelayCmdSet = new HashSet<string>(_asyncDelayCmd.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries));
		}
	}

	public void InitMessageHandlers()
	{
		mHandlers = new Dictionary<Type, BaseMessage>();
	}

	public void DispatchResponse(BaseEvent e)
	{
		string cmd = (string)e.Params["cmd"];
		SFSObject so = e.Params["params"] as SFSObject;
		DispatchResponse(cmd, so);
	}

	public static BaseMessage GetMessageByCmd(string cmd)
	{
		if (supportMessageTypes.TryGetValue(cmd, out var value))
		{
			return GetMessage(value);
		}
		return null;
	}

	public static T GetMessage<T>() where T : BaseMessage
	{
		Type typeFromHandle = typeof(T);
		if (mHandlers == null)
		{
			Log.Error("Message Factory Error, Call GetMessage When not init");
			return null;
		}
		if (mHandlers.TryGetValue(typeFromHandle, out var value))
		{
			return (T)value;
		}
		return (T)GetMessage(typeFromHandle);
	}

	public static BaseMessage GetMessage(Type type)
	{
		if (mHandlers == null)
		{
			Log.Error("Message Factory Error, Call GetMessage When not init");
			return null;
		}
		if (!mHandlers.TryGetValue(type, out var value))
		{
			value = type.Assembly.CreateInstance(type.FullName) as BaseMessage;
			if (value != null)
			{
				mHandlers[type] = value;
			}
		}
		return value;
	}

	public static bool ContainsMessage(Type type)
	{
		if (mHandlers == null)
		{
			Log.Error("Message Factory Error, Call ContainsMessage When not init");
			return false;
		}
		return mHandlers.ContainsKey(type);
	}

	public void DispatchResponse(string cmd, SFSObject so)
	{
		if (GMSwitch.IsGM && GMSwitch.GetBool("DebugLogProtocolMsg"))
		{
			if (cmd.Equals("world.get.new"))
			{
				Log.Warning($"[Msg][Receive]<color=green>extension res <{cmd}> |</color>");
			}
			else
			{
				string arg = so.ToJson();
				Log.Warning($"[Msg][Receive]<color=green>extension res <{cmd}> |</color> {arg}");
			}
		}
		try
		{
			if (so.ContainsKey("_id"))
			{
				int fuid = so.GetInt("_id");
				int serverTime = so.TryGetInt("_time");
				GameEntry.Network.getFutureManager().onServerMsgCome(fuid, serverTime);
			}
			if (CommonUtils.IsDebug())
			{
				if (asyncDelayTime > 0f && (asyncDelayCmdSet == null || asyncDelayCmdSet.Count == 0 || asyncDelayCmdSet.Contains(cmd)))
				{
					YieldUtils.DelayActionWithOutContext(delegate
					{
						BaseMessage messageByCmd3 = GetMessageByCmd(cmd);
						if (messageByCmd3 != null)
						{
							messageByCmd3.Handle(so);
						}
						else
						{
							GameEntry.Lua.DispatchResponse(cmd, so.ToLuaTable(GameEntry.Lua.Env));
						}
					}, asyncDelayTime);
				}
				else
				{
					BaseMessage messageByCmd = GetMessageByCmd(cmd);
					if (messageByCmd != null)
					{
						messageByCmd.Handle(so);
					}
					else
					{
						GameEntry.Lua.DispatchResponse(cmd, so.ToLuaTable(GameEntry.Lua.Env));
					}
				}
			}
			else
			{
				BaseMessage messageByCmd2 = GetMessageByCmd(cmd);
				if (messageByCmd2 != null)
				{
					messageByCmd2.Handle(so);
				}
				else
				{
					GameEntry.Lua.DispatchResponse(cmd, so.ToLuaTable(GameEntry.Lua.Env));
				}
			}
		}
		catch (Exception arg2)
		{
			Log.Error("process msg {0} error, {1}", cmd, arg2);
		}
	}

	public bool OnLogin(BaseEvent e)
	{
		SFSObject sFSObject = e.Params["data"] as SFSObject;
		if (sFSObject == null)
		{
			sFSObject = e.Params["errorMessage"] as SFSObject;
		}
		if (sFSObject == null)
		{
			Log.Error("Login failed");
			return false;
		}
		GetMessageByCmd("login")?.Handle(sFSObject);
		return true;
	}

	private void CheckMessageRegistConfig()
	{
		Type[] array = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => from t in a.GetTypes()
			where t.BaseType == typeof(BaseMessage)
			select t).ToArray();
		foreach (Type type in array)
		{
			object obj = type.Assembly.CreateInstance(type.FullName);
			if (obj is BaseMessage baseMessage && !(obj is LoginCrossServerMessage) && !(obj is WorldCrossServerMessage))
			{
				string msgId = baseMessage.GetMsgId();
				if (!supportMessageTypes.ContainsKey(msgId))
				{
					Debug.LogError("MsgId为" + msgId + "的" + type.Name + "未注册，请点击\"Tools/Constant代码生成/生成C#BaseMessage注册文件\"进行注册");
				}
				else if (supportMessageTypes[msgId].Name != type.Name)
				{
					Debug.LogError("有MsgId同为" + msgId + "的" + supportMessageTypes[msgId].Name + "和" + type.Name + "，请修改，然后点击\"Tools/Constant代码生成/生成C#BaseMessage注册文件\"刷新注册文件");
				}
			}
		}
	}
}
