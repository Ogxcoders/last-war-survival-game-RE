using System.Runtime.InteropServices;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class DataUIRenderMessage
{
	public struct UIResult : IRender
	{
		public bool Win;

		public int Result;

		public int SelfPlayerID;

		public EPlayerID WinPlayerID;

		public long BattleTimeMills;

		public string ValidationStr;

		public int[] FireCount;

		public bool HeadShot;

		public UIRenderType Type => UIRenderType.UIResult;
	}

	public struct UIShowInfo : IRender
	{
		public int GunBulletCount;

		public int GunBulletMax;

		public float GunReloadMaxCD;

		public float GunReloadCurCD;

		public EGameType GameType;

		public int PlayerID;

		public int[] CurHp;

		public int[] MaxHp;

		public UIRenderType Type => UIRenderType.UIShowInfo;
	}

	public struct UIPlayerBind : IRender
	{
		public DataUIPlayerController Controller;

		public UIRenderType Type => UIRenderType.UIPlayerBind;
	}

	public struct UIEffect : IRender
	{
		public bool IsMe;

		public int EffectType;

		public Vector3 Position;

		public Quaternion Rotation;

		public Transform Parent;

		public UIRenderType Type => UIRenderType.UIEffect;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct UIZhunXin : IRender
	{
		public UIRenderType Type => UIRenderType.UIZhunXin;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct UICameraShake : IRender
	{
		public UIRenderType Type => UIRenderType.UICameraShake;
	}

	public struct UIEntityAction : IRender
	{
		public bool IsMe;

		public float FireCdTime;

		public int ActionType;

		public int Result;

		public DataUIEntityController Controller;

		public UIRenderType Type => UIRenderType.UIEntityAction;
	}

	public struct UINetWork : IRender
	{
		public bool SuccOrFair;

		public UIRenderType Type => UIRenderType.UINetWork;
	}

	public struct UIWait : IRender
	{
		public int PlayerID;

		public UIRenderType Type => UIRenderType.UIWait;
	}
}
