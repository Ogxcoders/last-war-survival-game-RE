using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGoodsFly : MonoBehaviour
{
	public bool isRun;

	public Vector3 targetPos = Vector3.zero;

	public float minSize = -100f;

	public float maxSize = 100f;

	public bool isReset;

	public Vector3 oldPos = Vector3.zero;

	public float moveTime = 1f;

	private static float _pointCount = 5f;

	private static int SEGMENT_COUNT = 20;

	public AnimationCurve firstCurve;

	public AnimationCurve secondCurve;

	public AnimationCurve thirdCurve;

	public Vector3 controlPointOffset = new Vector3(0f, 300f, 0f);

	private int SearchFlyType = 999;

	private const string coinRootPath = "UIResource/UIMain/safeArea/topLayer/ResourceBar/{0}/root/resourceIcon";

	private const string goldRootPath = "UIResource/UIMain/safeArea/topLayer/ResourceBar/goldObj/goldIcon";

	private const string resourceRootPath = "UIResource/UIMain/safeArea/showObj/GoodsIcon";

	private const string powerRootPath = "UIResource/UIMain/safeArea/showObj/PowerIcon";

	private const string energyRootPath = "UIResource/UIMain/safeArea/topLayer/ResourceBar/UIEnergySlider/Icon";

	private const string searchPath = "UIResource/UIMain/safeArea/leftLayer/playerObj/SearchBtn/SearchIcon";

	public Image img;

	public Text txt;

	private Transform targetRoot;

	private static List<TrailRenderer> _listTrail = new List<TrailRenderer>(4);

	private static Vector3[] _paths = new Vector3[SEGMENT_COUNT];

	private WaitForSeconds tt1 = new WaitForSeconds(0.5f);

	public float middleValue = 0.5f;

	private void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		ClearTrailRenderPoint();
	}

	private void ClearTrailRenderPoint()
	{
		if (base.gameObject != null)
		{
			_listTrail.Clear();
			base.gameObject.GetComponentsInChildren(includeInactive: false, _listTrail);
			for (int i = 0; i < _listTrail.Count; i++)
			{
				_listTrail[i].Clear();
			}
		}
	}

	private void Update()
	{
		if (isRun)
		{
			oldPos = base.transform.position;
			targetPos = GameObject.Find("Garbage_3444").transform.position;
			isRun = false;
		}
		if (isReset)
		{
			base.transform.position = oldPos;
			isReset = false;
		}
	}

	public void DoAnimForLua(float minRange, float maxRange, int rewardType, string pic, int num, Vector3 destPos, Action onComplete, bool isOnlyDisperse)
	{
		DoAnim(minRange, maxRange, rewardType, pic, num, destPos, onComplete, isOnlyDisperse);
	}

	public void DoAnim(float minRange, float maxRange, int rewardType, string pic, int num, Vector3 destPos, Action onComplete, bool isOnlyDisperse)
	{
		if (txt != null)
		{
			txt.text = $"+{num}";
		}
		DoAnim(minRange, maxRange, rewardType, pic, destPos, onComplete, isOnlyDisperse);
	}

	public void DoAnimBox(float minRange, float maxRange, Vector3 startPos, Vector3 destPos, Action onComplete)
	{
		float num = UnityEngine.Random.Range(-3, 3);
		float num2 = UnityEngine.Random.Range(0, 3);
		float num3 = UnityEngine.Random.Range(-3, 3);
		startPos = new Vector3(startPos.x + num, startPos.y + num2, startPos.z + num3);
		Vector3 vector = Vector3.Cross(startPos, destPos);
		Vector3 zero = Vector3.zero;
		Vector3[] path = Bezier2Path(controlPos: (!(vector.y > 0f)) ? ((startPos + destPos) * middleValue - controlPointOffset) : ((startPos + destPos) * middleValue + controlPointOffset), startPos: startPos, endPos: destPos);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOMove(startPos, 0.2f)).SetEase(firstCurve);
		sequence.Append(base.transform.DOPath(path, moveTime)).SetEase(secondCurve);
		sequence.OnKill(delegate
		{
			onComplete?.Invoke();
			DynamicFPSConfig.FreeHighFPSLockerForGameObject(base.gameObject);
		});
		DynamicFPSConfig.AcquireHighFPSLockerGameObject(base.gameObject);
	}

	public void DoAnim(float minRange, float maxRange, Vector3 destPos, Vector3 startPos, Action onComplete)
	{
		Vector3 normalized = (new Vector3(destPos.x, startPos.y, destPos.z) - startPos).normalized;
		float num = UnityEngine.Random.Range(0, 3);
		float num2 = UnityEngine.Random.Range(0, 3);
		float num3 = UnityEngine.Random.Range(0, 3);
		startPos = new Vector3(startPos.x + normalized.x * num, startPos.y + normalized.y * num2, startPos.z + normalized.z * num3);
		Vector3 vector = Vector3.Cross(startPos, destPos);
		Vector3 zero = Vector3.zero;
		Vector3[] path = Bezier2Path(controlPos: (!(vector.y > 0f)) ? ((startPos + destPos) * middleValue - controlPointOffset) : ((startPos + destPos) * middleValue + controlPointOffset), startPos: startPos, endPos: destPos);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOMove(startPos, 0.2f)).SetEase(firstCurve);
		sequence.Insert(0.2f, base.transform.DOPath(path, moveTime)).SetEase(secondCurve);
		sequence.onKill = delegate
		{
			onComplete();
			DynamicFPSConfig.FreeHighFPSLockerForGameObject(base.gameObject);
		};
		DynamicFPSConfig.AcquireHighFPSLockerGameObject(base.gameObject);
	}

	private IEnumerator DelayComplete(Action onComplete)
	{
		yield return tt1;
		onComplete();
	}

	public void DoAnim(float minRange, float maxRange, int rewardType, string pic, Vector3 destPos, Action onComplete, bool isOnlyDisperse)
	{
		_ = string.Empty;
		if (rewardType == 20)
		{
			targetRoot = GameEntry.UIContainer.Find($"UIResource/UIMain/safeArea/topLayer/ResourceBar/{14}/root/resourceIcon");
		}
		else if (rewardType == 0)
		{
			targetRoot = GameEntry.UIContainer.Find($"UIResource/UIMain/safeArea/topLayer/ResourceBar/{0}/root/resourceIcon");
		}
		else if (rewardType == 1)
		{
			targetRoot = GameEntry.UIContainer.Find($"UIResource/UIMain/safeArea/topLayer/ResourceBar/{1}/root/resourceIcon");
		}
		else if (rewardType == 21)
		{
			targetRoot = GameEntry.UIContainer.Find($"UIResource/UIMain/safeArea/topLayer/ResourceBar/{12}/root/resourceIcon");
		}
		else if (rewardType == 19)
		{
			targetRoot = GameEntry.UIContainer.Find($"UIResource/UIMain/safeArea/topLayer/ResourceBar/{11}/root/resourceIcon");
		}
		else if (rewardType == 15)
		{
			targetRoot = GameEntry.UIContainer.Find("UIResource/UIMain/safeArea/topLayer/ResourceBar/goldObj/goldIcon");
		}
		else if (rewardType == 9)
		{
			targetRoot = GameEntry.UIContainer.Find("UIResource/UIMain/safeArea/showObj/PowerIcon");
		}
		else if (rewardType == 30)
		{
			targetRoot = GameEntry.UIContainer.Find("UIResource/UIMain/safeArea/topLayer/ResourceBar/UIEnergySlider/Icon");
		}
		else if (rewardType == SearchFlyType)
		{
			targetRoot = GameEntry.UIContainer.Find("UIResource/UIMain/safeArea/leftLayer/playerObj/SearchBtn/SearchIcon");
		}
		else
		{
			targetRoot = GameEntry.UIContainer.Find("UIResource/UIMain/safeArea/showObj/GoodsIcon");
		}
		if (destPos == Vector3.zero && targetRoot != null)
		{
			destPos = targetRoot.transform.position;
		}
		if (img != null)
		{
			img.LoadSprite(pic);
			img.color = new Color(1f, 1f, 1f, 1f);
		}
		Vector3 vector = base.transform.position + new Vector3(UnityEngine.Random.Range(minRange, maxRange), UnityEngine.Random.Range(minRange, maxRange), 0f);
		Vector3 vector2 = Vector3.Cross(vector, destPos);
		Vector3 zero = Vector3.zero;
		zero = ((!(vector2.y > 0f)) ? ((vector + destPos) * middleValue - controlPointOffset) : ((vector + destPos) * middleValue + controlPointOffset));
		Vector3[] path = Bezier2Path(vector, zero, destPos);
		Sequence sequence = DOTween.Sequence();
		if (rewardType == 34)
		{
			sequence.Append(base.transform.DOMove(destPos, 1f)).SetEase(firstCurve);
		}
		else if (isOnlyDisperse)
		{
			sequence.Append(base.transform.DOMove(vector, 1.4f)).SetEase(firstCurve);
		}
		else
		{
			sequence.Append(base.transform.DOMove(vector, 0.2f)).SetEase(firstCurve);
			sequence.Insert(0.2f, base.transform.DOPath(path, moveTime)).SetEase(secondCurve);
		}
		sequence.onKill = delegate
		{
			GameEntry.Event.Fire(EventId.RewardItemAdd, rewardType);
			onComplete();
			DynamicFPSConfig.FreeHighFPSLockerForGameObject(base.gameObject);
		};
		DynamicFPSConfig.AcquireHighFPSLockerGameObject(base.gameObject);
	}

	public void DoAnimWithoutLogic(float minRange, float maxRange, string pic, Vector3 destPos, Action onComplete, float flyTime1, float flyTime2)
	{
		if (img != null)
		{
			img.LoadSprite(pic);
			img.color = new Color(1f, 1f, 1f, 1f);
		}
		Vector3 vector = base.transform.position + new Vector3(UnityEngine.Random.Range(minRange, maxRange), UnityEngine.Random.Range(minRange, maxRange), 0f);
		Vector3 vector2 = Vector3.Cross(vector, destPos);
		Vector3 zero = Vector3.zero;
		zero = ((!(vector2.y > 0f)) ? ((vector + destPos) * middleValue - controlPointOffset) : ((vector + destPos) * middleValue + controlPointOffset));
		Vector3[] path = Bezier2Path(vector, zero, destPos);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOMove(vector, flyTime1)).SetEase(firstCurve);
		sequence.Insert(flyTime1, base.transform.DOPath(path, flyTime2)).SetEase(secondCurve);
		sequence.onKill = delegate
		{
			onComplete();
			DynamicFPSConfig.FreeHighFPSLockerForGameObject(base.gameObject);
		};
		DynamicFPSConfig.AcquireHighFPSLockerGameObject(base.gameObject);
	}

	public void DoParabolaAnim(Vector3 destPos, Vector3 startPos, Action onComplete)
	{
		Vector3 vector = Vector3.Cross(startPos, destPos);
		Vector3 zero = Vector3.zero;
		zero = ((!(vector.y > 0f)) ? ((startPos + destPos) * middleValue - controlPointOffset) : ((startPos + destPos) * middleValue + controlPointOffset));
		Vector3[] path = Bezier2Path(startPos, zero, destPos);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOMove(startPos, 0.2f)).SetEase(firstCurve);
		sequence.Insert(0.2f, base.transform.DOPath(path, moveTime)).SetEase(secondCurve);
		sequence.onKill = delegate
		{
			onComplete();
			DynamicFPSConfig.FreeHighFPSLockerForGameObject(base.gameObject);
		};
		DynamicFPSConfig.AcquireHighFPSLockerGameObject(base.gameObject);
	}

	public void DoParabolaAnimLocal(Vector3 destPos, Vector3 startPos, Action onComplete)
	{
		Vector3 vector = Vector3.Cross(startPos, destPos);
		Vector3 zero = Vector3.zero;
		zero = ((!(vector.y > 0f)) ? ((startPos + destPos) * middleValue - controlPointOffset) : ((startPos + destPos) * middleValue + controlPointOffset));
		Vector3[] path = Bezier2Path(startPos, zero, destPos);
		Sequence sequence = DOTween.Sequence();
		sequence.Append(base.transform.DOLocalMove(startPos, 0.2f)).SetEase(firstCurve);
		sequence.Insert(0.2f, base.transform.DOLocalPath(path, moveTime)).SetEase(secondCurve);
		sequence.onKill = delegate
		{
			onComplete();
			DynamicFPSConfig.FreeHighFPSLockerForGameObject(base.gameObject);
		};
		DynamicFPSConfig.AcquireHighFPSLockerGameObject(base.gameObject);
	}

	private Vector3[] Bezier2Path(Vector3 startPos, Vector3 controlPos, Vector3 endPos)
	{
		for (int i = 1; i <= SEGMENT_COUNT; i++)
		{
			float t = (float)i / (float)SEGMENT_COUNT;
			Vector3 vector = CalculateCubicBezierPointfor2C(t, startPos, controlPos, endPos);
			_paths[i - 1] = vector;
		}
		return _paths;
	}

	public static Vector3 Bezier2(Vector3 startPos, Vector3 controlPos, Vector3 endPos, float t)
	{
		return (1f - t) * (1f - t) * startPos + 2f * t * (1f - t) * controlPos + t * t * endPos;
	}

	public static Vector3 Bezier3(Vector3 startPos, Vector3 controlPos1, Vector3 controlPos2, Vector3 endPos, float t)
	{
		float num = 1f - t;
		return num * num * num * startPos + 3f * t * num * num * controlPos1 + 3f * t * t * num * controlPos2 + t * t * t * endPos;
	}

	private Vector3 CalculateCubicBezierPointfor2C(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float num2 = t * t;
		return num * num * p0 + 2f * num * t * p1 + num2 * p2;
	}
}
