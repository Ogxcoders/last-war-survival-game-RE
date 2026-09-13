using System;
using UnityEngine;

public class BottleWaterCtrl : MonoBehaviour
{
	public GameObject water;

	private Material waterMat;

	public float xOffsetMax = 1f;

	public float zOffsetMax = 1f;

	private float xOffset;

	private float zOffset;

	private Vector3 lastPos;

	private Vector3 lastRota;

	private float maxPosOffset = 0.05f;

	private float maxAngleOffset = 30f;

	private float waveX;

	private float waveZ;

	private float waveSpeed = 10f;

	private void Start()
	{
		if (water != null)
		{
			Renderer component = water.GetComponent<Renderer>();
			if (component != null)
			{
				waterMat = component.material;
			}
		}
		SetCurrentTransform();
	}

	private void Update()
	{
		CheckDown();
		CheckShake();
		SetWaterWave();
	}

	private void CheckDown()
	{
		if (!(waterMat == null))
		{
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			float value = Mathf.Abs(Mathf.Sin(Mathf.Max(eulerAngles.x, eulerAngles.z) * (MathF.PI / 180f)));
			waterMat.SetFloat("_down", value);
		}
	}

	private void CheckShake()
	{
		if (!(water == null))
		{
			Vector3 position = water.transform.position;
			Vector3 eulerAngles = water.transform.rotation.eulerAngles;
			Vector3 vector = position - lastPos;
			Vector3 vector2 = eulerAngles - lastRota;
			float val = vector.x / maxPosOffset * -1f + vector.y / maxPosOffset * 0.5f + vector2.z / maxAngleOffset + vector2.y / maxAngleOffset * 0.5f;
			float val2 = vector.z / maxPosOffset * -1f + vector.y / maxPosOffset * 0.5f + vector2.x / maxAngleOffset + vector2.y / maxAngleOffset * 0.5f;
			AddXOffset(val);
			AddZOffset(val2);
			SetCurrentTransform();
		}
	}

	private void AddXOffset(float val)
	{
		if (Mathf.Abs(val) != 0f)
		{
			xOffset += val;
			if (xOffset < -1f)
			{
				xOffset = -1f;
			}
			if (xOffset > 1f)
			{
				xOffset = 1f;
			}
		}
	}

	private void AddZOffset(float val)
	{
		if (Mathf.Abs(val) != 0f)
		{
			zOffset += val;
			if (zOffset < -1f)
			{
				zOffset = -1f;
			}
			if (zOffset > 1f)
			{
				zOffset = 1f;
			}
		}
	}

	private void SetCurrentTransform()
	{
		if (!(water == null))
		{
			lastPos = water.transform.position;
			lastRota = water.transform.rotation.eulerAngles;
		}
	}

	private void SetWaterWave()
	{
		if (xOffset != 0f)
		{
			waveX += xOffset * Time.deltaTime * waveSpeed;
			if (xOffset > 0f)
			{
				if (waveX >= xOffset)
				{
					waveX = xOffset;
					xOffset *= -0.8f;
				}
			}
			else if (waveX <= xOffset)
			{
				waveX = xOffset;
				xOffset *= -0.8f;
			}
			if (Mathf.Abs(xOffset) < 0.01f)
			{
				xOffset = 0f;
				waveX = 0f;
			}
			if ((bool)waterMat)
			{
				waterMat.SetFloat("_waveX", waveX * xOffsetMax);
			}
		}
		if (zOffset == 0f)
		{
			return;
		}
		waveZ += zOffset * Time.deltaTime * waveSpeed;
		if (zOffset > 0f)
		{
			if (waveZ >= zOffset)
			{
				waveZ = zOffset;
				zOffset *= -0.8f;
			}
		}
		else if (waveZ <= zOffset)
		{
			waveZ = zOffset;
			zOffset *= -0.8f;
		}
		if (Mathf.Abs(zOffset) < 0.01f)
		{
			zOffset = 0f;
			waveZ = 0f;
		}
		if ((bool)waterMat)
		{
			waterMat.SetFloat("_waveZ", waveZ * zOffsetMax);
		}
	}
}
