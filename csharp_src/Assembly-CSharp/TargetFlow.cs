using UnityEngine;

public class TargetFlow : MonoBehaviour
{
	public GameObject drawPoint;

	public LineRenderer render;

	public Transform target;

	public float zOffset;

	public float xOffset;

	public Color lineColor = Color.green;

	public float maxYoffset = 5f;

	public float minYoffset = 2f;

	private bool toggleShowLine = true;

	private Transform tran;

	public Transform _a;

	public Transform _b;

	private bool doAnim;

	private float startTime;

	private float continueTime;

	private void Awake()
	{
		tran = base.transform;
	}

	private void Start()
	{
		DoFlow();
	}

	private void DoFlow()
	{
		if (target == null)
		{
			if (toggleShowLine)
			{
				render.gameObject.SetActive(value: false);
				toggleShowLine = false;
			}
			return;
		}
		Transform parent = target.parent;
		float num = 0f;
		float num2 = parent.rotation.eulerAngles.y % 180f;
		num = ((!(parent.rotation.eulerAngles.y < 180f)) ? Mathf.Lerp(minYoffset, maxYoffset, num2 / 180f) : Mathf.Lerp(maxYoffset, minYoffset, num2 / 180f));
		if (!toggleShowLine)
		{
			render.gameObject.SetActive(value: true);
			toggleShowLine = true;
		}
		render.startColor = lineColor;
		render.endColor = lineColor;
		render.SetPosition(0, drawPoint.transform.position);
		render.SetPosition(1, target.parent.transform.position);
		float num3 = zOffset;
		float num4 = parent.localRotation.eulerAngles.y;
		if (num4 > 180f)
		{
			num4 -= 360f;
		}
		num3 = ((num4 >= -15f && num4 <= 15f) ? (2.1f * zOffset) : (((!(num4 >= 15f) || !(num4 <= 45f)) && (!(num4 >= -45f) || !(num4 <= -15f))) ? zOffset : (1.3f * zOffset)));
		target.transform.localPosition = new Vector3(xOffset, num, num3);
		tran.position = target.transform.position;
		if (_a != null && _b != null)
		{
			_a.position = _b.position;
		}
	}

	public void DoFlowAnim(float deltaTime)
	{
		if (target == null)
		{
			if (toggleShowLine)
			{
				render.gameObject.SetActive(value: false);
				toggleShowLine = false;
			}
		}
		else if (tran == null)
		{
			if (toggleShowLine)
			{
				render.gameObject.SetActive(value: false);
				toggleShowLine = false;
			}
		}
		else
		{
			continueTime = deltaTime;
			startTime = 0f;
			doAnim = true;
			SetRotation(0f);
		}
	}

	private void SetRotation(float percent)
	{
		if (target == null)
		{
			if (toggleShowLine)
			{
				render.gameObject.SetActive(value: false);
				toggleShowLine = false;
			}
			return;
		}
		Transform parent = target.parent;
		float num = 0f;
		float num2 = parent.rotation.eulerAngles.y % 180f;
		num = ((!(parent.rotation.eulerAngles.y < 180f)) ? Mathf.Lerp(minYoffset, maxYoffset, num2 / 180f) : Mathf.Lerp(maxYoffset, minYoffset, num2 / 180f));
		if (!toggleShowLine)
		{
			render.gameObject.SetActive(value: true);
			toggleShowLine = true;
		}
		render.startColor = lineColor;
		render.endColor = lineColor;
		render.SetPosition(0, drawPoint.transform.position);
		render.SetPosition(1, target.parent.transform.position);
		float num3 = zOffset;
		float num4 = parent.localRotation.eulerAngles.y;
		if (num4 > 180f)
		{
			num4 -= 360f;
		}
		num3 = ((num4 >= -15f && num4 <= 15f) ? (2.1f * zOffset) : (((!(num4 >= 15f) || !(num4 <= 45f)) && (!(num4 >= -45f) || !(num4 <= -15f))) ? zOffset : (1.3f * zOffset)));
		target.transform.localPosition = new Vector3(xOffset, num, num3 * percent);
		tran.position = target.transform.position;
		if (_a != null && _b != null)
		{
			_a.position = _b.position;
		}
	}

	public void Update()
	{
		if (doAnim)
		{
			startTime += Time.deltaTime;
			if (startTime > continueTime)
			{
				doAnim = false;
				startTime = 0f;
				SetRotation(1f);
			}
			else
			{
				SetRotation(startTime / continueTime);
			}
		}
		else
		{
			DoFlow();
		}
	}
}
