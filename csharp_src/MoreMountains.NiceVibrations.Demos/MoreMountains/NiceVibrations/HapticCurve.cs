using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class HapticCurve : MonoBehaviour
{
	[Range(0f, 1f)]
	public float Intensity = 1f;

	[Range(0f, 1f)]
	public float Sharpness;

	public int PointsCount = 50;

	public float AmplitudeFactor = 3f;

	public int Period = 4;

	public RectTransform StartPoint;

	public RectTransform EndPoint;

	[Header("Movement")]
	public bool Move;

	public float MovementSpeed = 1f;

	protected LineRenderer _targetLineRenderer;

	protected List<Vector3> Points;

	protected Canvas _canvas;

	protected Camera _camera;

	protected Vector3 _startPosition;

	protected Vector3 _endPosition;

	protected Vector3 _workPoint;

	protected virtual void Awake()
	{
		Initialization();
	}

	protected virtual void Initialization()
	{
		Points = new List<Vector3>();
		_canvas = base.gameObject.GetComponentInParent<Canvas>();
		_targetLineRenderer = base.gameObject.GetComponent<LineRenderer>();
		_camera = _canvas.worldCamera;
		DrawCurve();
	}

	protected virtual void DrawCurve()
	{
		_startPosition = StartPoint.transform.position;
		_startPosition.z -= 0.1f;
		_endPosition = EndPoint.transform.position;
		_endPosition.z -= 0.1f;
		Points.Clear();
		for (int i = 0; i < PointsCount; i++)
		{
			float num = NiceVibrationsDemoHelpers.Remap(i, 0f, PointsCount, 0f, 1f);
			float value = MMSignal.GetValue(num, MMSignal.SignalType.Sine, 1f, AmplitudeFactor, Period, 0f);
			float value2 = MMSignal.GetValue(num, MMSignal.SignalType.Triangle, 1f, AmplitudeFactor, Period, 0f);
			if (Move)
			{
				value = MMSignal.GetValue(num + Time.time * MovementSpeed, MMSignal.SignalType.Sine, 1f, AmplitudeFactor, Period, 0f);
				value2 = MMSignal.GetValue(num + Time.time * MovementSpeed, MMSignal.SignalType.Triangle, 1f, AmplitudeFactor, Period, 0f);
			}
			float num2 = Mathf.Lerp(value, value2, Sharpness);
			_workPoint.x = Mathf.Lerp(_startPosition.x, _endPosition.x, num);
			_workPoint.y = num2 * Intensity + _startPosition.y;
			_workPoint.z = _startPosition.z;
			Points.Add(_workPoint);
		}
		_targetLineRenderer.positionCount = PointsCount;
		_targetLineRenderer.SetPositions(Points.ToArray());
	}

	protected virtual void Update()
	{
		UpdateCurve(Intensity, Sharpness);
	}

	public virtual void UpdateCurve(float intensity, float sharpness)
	{
		Intensity = intensity;
		Sharpness = sharpness;
		DrawCurve();
	}
}
