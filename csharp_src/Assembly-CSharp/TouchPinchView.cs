using UnityEngine;

public class TouchPinchView : MonoBehaviour
{
	[SerializeField]
	public float ZoomMax = 9f;

	[SerializeField]
	public float ZoomMin = 0.44f;

	[SerializeField]
	public float ZoomBest = 0.44f;

	[SerializeField]
	public float ZoomSpeed = 0.02f;

	private float curZoomRatio = 1f;

	private const float mouseWheelSpeed = -5f;

	private int mainFingerId = -1;

	private int subFingerId = -1;

	private bool mobilePlatform;

	private float screenDpi;

	private float screenDpc;

	private void Awake()
	{
		CalcScreenDpi();
		mobilePlatform = Application.isMobilePlatform;
	}

	private void OnEnable()
	{
		mainFingerId = -1;
		subFingerId = -1;
		curZoomRatio = ZoomBest;
		base.transform.localScale = new Vector3(ZoomBest, ZoomBest, ZoomBest);
	}

	private void OnDisable()
	{
		mainFingerId = -1;
		subFingerId = -1;
	}

	private void Update()
	{
		if (mobilePlatform)
		{
			UpdateMobile();
		}
		else
		{
			UpdateMouse();
		}
	}

	private void OnTouchPinch(float speedscr, float speed)
	{
		int touchCount = Input.touchCount;
		if (base.transform != null && touchCount < 3)
		{
			float num = Mathf.Clamp(Mathf.InverseLerp(ZoomMin, ZoomMax, curZoomRatio), 0.1f, 1f);
			curZoomRatio += speed * ZoomSpeed * num;
			curZoomRatio = Mathf.Clamp(curZoomRatio, ZoomMin, ZoomMax);
			base.transform.localScale = new Vector3(curZoomRatio, curZoomRatio, curZoomRatio);
		}
	}

	private void CalcScreenDpi()
	{
		screenDpi = Screen.dpi;
		if (screenDpi == 0f)
		{
			screenDpi = 96f;
		}
		screenDpc = screenDpi / 2.54f;
	}

	private void UpdateMobile()
	{
		int touchCount = Input.touchCount;
		if (touchCount == 0)
		{
			mainFingerId = -1;
			subFingerId = -1;
			return;
		}
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			int fingerId = touch.fingerId;
			if (fingerId >= 2)
			{
				continue;
			}
			if (touch.phase == TouchPhase.Began)
			{
				if (fingerId == 0)
				{
					mainFingerId = fingerId;
				}
				else
				{
					subFingerId = fingerId;
				}
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				if (mainFingerId < 0 || subFingerId < 0)
				{
					continue;
				}
				Vector2 vector = Vector2.zero;
				Vector2 vector2 = Vector2.zero;
				Vector2 vector3 = Vector2.zero;
				Vector2 vector4 = Vector2.zero;
				int num = 0;
				for (int j = 0; j < touchCount; j++)
				{
					Touch touch2 = Input.GetTouch(j);
					if (mainFingerId == touch2.fingerId)
					{
						vector = touch2.position;
						vector3 = touch2.deltaPosition;
						num++;
					}
					else if (subFingerId == touch2.fingerId)
					{
						vector2 = touch2.position;
						vector4 = touch2.deltaPosition;
						num++;
					}
				}
				if (num == 2)
				{
					float num2 = Vector2.Distance(vector, vector2);
					float num3 = Vector2.Distance(vector - vector3, vector2 - vector4);
					float num4 = num2 - num3;
					float speed = num4 / screenDpc / Time.deltaTime;
					float speedscr = num4 / (float)(Screen.width + Screen.height) * 0.5f / Time.deltaTime;
					OnTouchPinch(speedscr, speed);
				}
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				if (fingerId == 0)
				{
					mainFingerId = -1;
					subFingerId = -1;
				}
				else
				{
					subFingerId = -1;
				}
			}
			else if (touch.phase == TouchPhase.Canceled)
			{
				if (fingerId == 0)
				{
					mainFingerId = -1;
					subFingerId = -1;
				}
				else
				{
					subFingerId = -1;
				}
			}
		}
	}

	private void UpdateMouse()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (!Mathf.Approximately(axis, 0f))
		{
			axis *= -5f;
			float speed = axis / Time.deltaTime;
			float speedscr = axis / (float)(Screen.width + Screen.height) * 0.5f / Time.deltaTime;
			OnTouchPinch(speedscr, speed);
		}
	}
}
