using Joker;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class UIS5GameMain : MonoBehaviour, IBindUI
{
	public TextMeshProUGUIEx TextMagCount;

	public TextMeshProUGUIEx TextMagCD;

	public GameObject GameEnd;

	public Transform GameRoot;

	public DataUIAdapt DataUIAdapt;

	public Camera UICamera;

	public Transform HudRoot;

	public UIS5GamePlayerHud[] PlayerHuds;

	private float leftTime;

	private bool updateFinish;

	private int GameType = -1;

	private void Start()
	{
		for (int i = 0; i < PlayerHuds.Length; i++)
		{
			PlayerHuds[i].Init(i);
		}
	}

	private void OnDestroy()
	{
		DataUIAdapt?.RemoveUISyncEvent(UISyncEvent);
		StopAllCoroutines();
	}

	public void BindAdapt(DataUIAdapt dataUIAdapt)
	{
		DataUIAdapt = dataUIAdapt;
		DataUIAdapt.AddUISyncEvent(UISyncEvent);
	}

	public Transform GetGameRoot()
	{
		return GameRoot;
	}

	private void RefreshPlayerHud(DataUIRenderMessage.UIShowInfo uiShowInfo)
	{
		HudRoot.gameObject.SetActive(uiShowInfo.GameType == EGameType.PvpClient);
		UIS5GamePlayerHud[] playerHuds = PlayerHuds;
		for (int i = 0; i < playerHuds.Length; i++)
		{
			playerHuds[i].Refresh(uiShowInfo);
		}
	}

	private void UISyncEvent(IRender render)
	{
		if (render is DataUIRenderMessage.UIShowInfo uiShowInfo)
		{
			GameType = (int)uiShowInfo.GameType;
			Log.Debug($"[BiuBiu]:DataUIRenderMessage.UIShowInfo {uiShowInfo.GunReloadCurCD} {uiShowInfo.GunReloadMaxCD}");
			leftTime = uiShowInfo.GunReloadCurCD;
			updateFinish = false;
			Update();
			TextMagCount.SetText($"{uiShowInfo.GunBulletCount}/{uiShowInfo.GunBulletMax}");
			if (GameType == 3)
			{
				RefreshPlayerHud(uiShowInfo);
			}
		}
		if (render is DataUIRenderMessage.UIResult uIResult)
		{
			GameEnd.SetActive(value: true);
			GameEnd.transform.Find("Win").gameObject.SetActive(uIResult.Win);
			GameEnd.transform.Find("Lose").gameObject.SetActive(!uIResult.Win);
		}
		if (GameType == 3 && render is DataUIRenderMessage.UIPlayerBind uiPlayerBind)
		{
			int playerID = uiPlayerBind.Controller.PlayerID;
			PlayerHuds[playerID].BindController(uiPlayerBind, UICamera);
		}
	}

	private void Update()
	{
		if (!updateFinish && leftTime > 0f)
		{
			TextMagCD.SetText(leftTime.ToString("F2"));
			leftTime -= Time.deltaTime;
		}
		else
		{
			updateFinish = true;
			TextMagCD.SetText("0");
		}
	}

	public void OnClickNextBtn()
	{
		GameEnd.SetActive(value: false);
	}

	public void OnClickCloseBtn()
	{
		GameEnd.SetActive(value: false);
	}
}
