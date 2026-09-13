using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.Biubiu.Client;

public class DataResourceLoaderEnv
{
	public GameBiuBiuPlayerBase Player;

	public float EditorCameraOrthographicSize = 5.4f;

	public readonly Transform GameRoot;

	public const float ScalePhysicsToLogic = 100f;

	public float SizeToUnit = 0.75f;

	public float Physics2GfxScale;

	private float OrthographicSizeScale;

	public Camera Camera { get; set; }

	public GameObject GameGo { get; private set; }

	public Transform MapRoot { get; private set; }

	public Transform TileMask { get; private set; }

	public Transform PlayerRoot { get; private set; }

	public GameObject WifiGo { get; private set; }

	public Transform EffectRoot { get; private set; }

	public Transform DynamicRoot { get; private set; }

	public GameObject GoEffectHit { get; private set; }

	public GameObject GoEffectTanshe { get; private set; }

	public GameObject GoEffectTansheOther { get; private set; }

	public GameObject GoEffectZX { get; private set; }

	public GameObject GoEffectExplode { get; private set; }

	public GameObject GoEffectImpact { get; private set; }

	public GameObject GoEffectHeadShot { get; private set; }

	public float Gfx2LogicScale { get; private set; }

	public float Logic2GfxScale => 1f / Gfx2LogicScale;

	public DataResourceLoaderEnv(Transform gameRoot, float orthographicSize = 1f)
	{
		GameRoot = gameRoot;
		OrthographicSizeScale = orthographicSize;
	}

	public void SetGameGO(GameObject go)
	{
		GameGo = go;
		MapRoot = GameGo.transform.Find("MapRoot");
		TileMask = MapRoot.transform.Find("TileMask");
		PlayerRoot = GameGo.transform.Find("PlayerRoot");
		EffectRoot = GameGo.transform.Find("EffectRoot");
		Camera = GameGo.transform.Find("Camera").GetComponent<Camera>();
		WifiGo = GameGo.transform.Find("WifiCircleBg").gameObject;
		GoEffectZX = GameGo.transform.Find("EffectRoot/Go_Effec_ZX").gameObject;
		GoEffectTanshe = GameGo.transform.Find("EffectRoot/Go_Effect_Tanshe").gameObject;
		GoEffectTansheOther = GameGo.transform.Find("EffectRoot/Go_Effect_Tanshe_Other").gameObject;
		GoEffectHit = GameGo.transform.Find("EffectRoot/Go_Effect_Hit").gameObject;
		GoEffectExplode = GameGo.transform.Find("EffectRoot/Go_Effect_Explode").gameObject;
		GoEffectImpact = GameGo.transform.Find("EffectRoot/Eff_S5_GoldRush_Impact").gameObject;
		GoEffectHeadShot = GameGo.transform.Find("EffectRoot/Eff_S5_GoldRush_headshot").gameObject;
		float referencePixelsPerUnit = Object.FindObjectOfType<CanvasScaler>().referencePixelsPerUnit;
		PlayerRoot.localScale = new Vector3(referencePixelsPerUnit, referencePixelsPerUnit, referencePixelsPerUnit);
		DynamicRoot = GameGo.transform.Find("DynamicRoot");
		DynamicRoot.localScale = new Vector3(referencePixelsPerUnit, referencePixelsPerUnit, referencePixelsPerUnit);
		Gfx2LogicScale = (GameRoot.transform.parent.lossyScale * referencePixelsPerUnit).x;
		Physics2GfxScale = Logic2GfxScale * referencePixelsPerUnit * OrthographicSizeScale;
		WifiGo.transform.localScale = 0.75f * Gfx2LogicScale / SizeToUnit * Vector3.one;
	}

	public Vector3 CalWorldToRootLocalPos(Vector3 pos)
	{
		return GameGo.transform.InverseTransformPoint(pos);
	}

	public Vector3 CalWorldToPlayerLocalPos(Vector3 pos)
	{
		return PlayerRoot.transform.InverseTransformPoint(pos);
	}

	public Vector3 CalWorldToDynamicLocalPos(Vector3 pos)
	{
		return DynamicRoot.transform.InverseTransformPoint(pos);
	}
}
