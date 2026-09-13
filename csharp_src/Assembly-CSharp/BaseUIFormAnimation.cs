using GameKit.Base;
using UnityEngine;
using UnityEngine.Events;

public class BaseUIFormAnimation : MonoBehaviour
{
	private const float CloseCurveMaxTime = 0.17f;

	private float closeCurveTime;

	private Vector3AnimationCurve closeScaleCurve = new Vector3AnimationCurve();

	private AnimationCurve closeAlphaCurve = new AnimationCurve();

	private CanvasGroup group;

	private const float OpenCurveMaxTime = 0.27f;

	private float openCurveTime;

	private Vector3AnimationCurve openScaleCurve = new Vector3AnimationCurve();

	private AnimationCurve openAlphaCurve = new AnimationCurve();

	public event UnityAction OnCloseEnd;

	private void Awake()
	{
		AddOpenKeyFrame();
		AddCloseKeyFrame();
		group = base.gameObject.GetOrAddComponent<CanvasGroup>();
	}

	private void OnEnable()
	{
		openCurveTime = 0.27f;
		closeCurveTime = 0.17f;
	}

	private void Update()
	{
		UpdateOpenAnim();
		UpdateCloseAnim();
	}

	private void AddCloseKeyFrame()
	{
		closeScaleCurve.AddKey(0f, Vector3.one);
		closeScaleCurve.AddKey(0.17f, new Vector3(1.03f, 1.03f, 1.03f));
		closeAlphaCurve.AddKey(0f, 1f);
		closeAlphaCurve.AddKey(0.17f, 0f);
	}

	private void UpdateCloseAnim()
	{
		if (!(closeCurveTime >= 0.17f))
		{
			closeCurveTime += Time.deltaTime;
			if (closeCurveTime > 0.17f)
			{
				closeCurveTime = 0.17f;
			}
			base.transform.localScale = closeScaleCurve.Evaluate(closeCurveTime);
			group.alpha = closeAlphaCurve.Evaluate(closeCurveTime);
			if (closeCurveTime >= 0.17f && this.OnCloseEnd != null)
			{
				this.OnCloseEnd();
			}
		}
	}

	public void PlayCloseAnim()
	{
		closeCurveTime = 0f;
	}

	private void AddOpenKeyFrame()
	{
		openScaleCurve.AddKey(0f, new Vector3(1.02f, 1.02f, 1.02f));
		openScaleCurve.AddKey(0.13f, new Vector3(1.03f, 1.03f, 1.03f));
		openScaleCurve.AddKey(0.27f, Vector3.one);
		openAlphaCurve.AddKey(0f, 0.5f);
		openAlphaCurve.AddKey(0.13f, 1f);
		openAlphaCurve.AddKey(0.27f, 1f);
	}

	private void UpdateOpenAnim()
	{
		if (!(openCurveTime >= 0.27f))
		{
			openCurveTime += Time.deltaTime;
			if (openCurveTime > 0.27f)
			{
				openCurveTime = 0.27f;
			}
			base.transform.localScale = openScaleCurve.Evaluate(openCurveTime);
			group.alpha = openAlphaCurve.Evaluate(openCurveTime);
		}
	}

	public void PlayOpenAnim()
	{
		openCurveTime = 0f;
	}
}
