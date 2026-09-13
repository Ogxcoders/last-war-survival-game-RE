using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.Biubiu.Client;

public class UIS5GamePlayerHud : MonoBehaviour
{
	public Image CD;

	public GameObject GoCD;

	public UIS5GamePlayerHpItemView hpView;

	private int PlayerID;

	private DataUIPlayerController PlayerController;

	private float CurCD;

	private float MaxCD;

	private Camera UICamera;

	private Camera BiuBiuCamera;

	private RectTransform rectTransform;

	public void Init(int index)
	{
		PlayerID = index;
		rectTransform = GetComponent<RectTransform>();
	}

	public void Refresh(DataUIRenderMessage.UIShowInfo uiShowInfo)
	{
		bool flag = uiShowInfo.PlayerID == PlayerID;
		GoCD.gameObject.SetActive(flag);
		if (flag)
		{
			CurCD = uiShowInfo.GunReloadCurCD;
			MaxCD = uiShowInfo.GunReloadMaxCD;
		}
		int curHp = uiShowInfo.CurHp[PlayerID];
		int maxHp = uiShowInfo.MaxHp[PlayerID];
		hpView.Refresh(flag, curHp, maxHp);
	}

	private void Update()
	{
		CurCD -= Time.deltaTime;
		CurCD = Mathf.Clamp(CurCD, 0f, MaxCD);
		CD.fillAmount = 1f - CurCD / MaxCD;
		if (PlayerController != null)
		{
			Vector3 vector = BiuBiuCamera.WorldToScreenPoint(PlayerController.UIPos.position);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent.GetComponent<RectTransform>(), vector, UICamera, out var localPoint);
			rectTransform.anchoredPosition = localPoint;
		}
	}

	public void BindController(DataUIRenderMessage.UIPlayerBind uiPlayerBind, Camera uiCamera)
	{
		PlayerController = uiPlayerBind.Controller;
		UICamera = uiCamera;
		BiuBiuCamera = PlayerController.Camera;
	}
}
