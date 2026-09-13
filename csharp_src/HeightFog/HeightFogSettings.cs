using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class HeightFogSettings
{
	public bool EnableEffect = true;

	public float _FogDisappearHeight;

	public float _FogPosY;

	public float FogIntensity;

	public Color unexploredColor = new Color(0.05f, 0.05f, 0.05f, 1f);

	public Color exploredColor = new Color(0.2f, 0.2f, 0.2f, 1f);

	public Texture2D noise2D;

	public Texture2D fogTex;

	public Texture2D fogNormal;

	public LayerMask opaqueLayerMask;

	public LayerMask transparentLayerMask;

	public float FogXSpeed;

	public float FogYSpeed;

	public float NoiseAmount;

	public float NormalScale = 1f;

	public FOWSystem fowSystem;

	public Shader fogShader;

	public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
}
