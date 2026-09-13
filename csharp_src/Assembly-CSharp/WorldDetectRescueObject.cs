using UnityEngine;

public class WorldDetectRescueObject : WorldSampleObject
{
	private GameObject firstMan;

	private GameObject secondMan;

	private float firstManHideTime = 0.6f;

	private float secondManHideTime = 1.2f;

	private float firstShowBoomEffectTime = 3f;

	private float secondShowBoomEffectTime = 3.5f;

	private float updateTime;

	protected InstanceRequest boomEffectObject;

	private const string boomEffectPath = "Assets/_Art/Effect/prefab/scene/Common/VFX_baozha.prefab";

	private bool needShowFirstBoomEffect;

	private bool needShowSecondBoomEffect;

	public WorldDetectRescueObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
		collectEffectPath = "Assets/_Art/Effect/prefab/scene/Common/VFX_jianlaji.prefab";
		needShowFirstBoomEffect = true;
		needShowSecondBoomEffect = true;
		needShowCollectEffect = false;
	}

	public override void Destroy()
	{
		HideBoomParticle();
		base.Destroy();
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (IsCollect)
		{
			updateTime += deltaTime;
			if (firstMan != null && firstMan.activeSelf && updateTime > firstManHideTime)
			{
				firstMan.SetActive(value: false);
			}
			if (secondMan != null && secondMan.activeSelf && updateTime > secondManHideTime)
			{
				secondMan.SetActive(value: false);
			}
			if (updateTime > secondShowBoomEffectTime && needShowSecondBoomEffect)
			{
				needShowSecondBoomEffect = false;
				HideBoomParticle();
				ShowBoomParticle();
			}
			if (updateTime > firstShowBoomEffectTime && needShowFirstBoomEffect)
			{
				needShowFirstBoomEffect = false;
				HideBoomParticle();
				ShowBoomParticle();
			}
		}
	}

	protected override void DoWhenMarchInfoChange(object userData)
	{
		base.DoWhenMarchInfoChange(userData);
		if (detectEventInst != null && detectEventInst.gameObject != null)
		{
			detectEventInst.gameObject.SetActive(value: false);
		}
	}

	public override void AsyncCompleteCallBack(InstanceRequest instance)
	{
		base.AsyncCompleteCallBack(instance);
		if (model != null)
		{
			Transform transform = model.transform.Find("normalType/A_Hero_bubing02_1");
			Transform transform2 = model.transform.Find("normalType/A_Hero_bubing02_2");
			if (transform != null && transform2 != null)
			{
				firstMan = model.transform.Find("normalType/A_Hero_bubing02_1").gameObject;
				secondMan = model.transform.Find("normalType/A_Hero_bubing02_2").gameObject;
				firstMan.SetActive(value: true);
				secondMan.SetActive(value: true);
				updateTime = 0f;
			}
		}
	}

	protected override bool NeedShowTime()
	{
		return false;
	}

	private void ShowBoomParticle()
	{
		if (boomEffectObject != null)
		{
			if (boomEffectObject.gameObject != null)
			{
				boomEffectObject.gameObject.SetActive(value: true);
			}
			return;
		}
		boomEffectObject = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/Common/VFX_baozha.prefab");
		boomEffectObject.completed += delegate
		{
			GameObject gameObject = boomEffectObject.gameObject;
			gameObject.SetActive(value: true);
			Transform transform = base.gameObject.transform;
			if (transform != null)
			{
				gameObject.transform.SetParent(transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			}
		};
	}

	private void HideBoomParticle()
	{
		if (boomEffectObject != null)
		{
			boomEffectObject.Destroy();
			boomEffectObject = null;
		}
	}
}
