using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HeightFogPass : ScriptableRenderPass
{
	private static readonly string k_RenderTag = "HeightFogPass Effects";

	private static readonly int MainTexId = Shader.PropertyToID("_MainTex");

	private static readonly int TempTargetId = Shader.PropertyToID("_TempTarget");

	private static readonly int _FogPosY = Shader.PropertyToID("_FogPosY");

	private static readonly int _FogDisappearHeight = Shader.PropertyToID("_FogDisappearHeight");

	private static readonly int FogIntensity = Shader.PropertyToID("_FogIntensity");

	private static readonly int FogNoiseTexId = Shader.PropertyToID("_NoiseTex");

	private static readonly int FogXSpeedId = Shader.PropertyToID("_FogXSpeed");

	private static readonly int FogYSpeedId = Shader.PropertyToID("_FogYSpeed");

	private static readonly int NoiseAmountID = Shader.PropertyToID("_NoiseAmount");

	private static readonly int _FogTex0 = Shader.PropertyToID("_FogTex0");

	private static readonly int _FogTex1 = Shader.PropertyToID("_FogTex1");

	private static readonly int m_InverseMVP = Shader.PropertyToID("_InverseMVP");

	private static readonly int m_centerPos = Shader.PropertyToID("_CenterPos");

	private static readonly int m_noiseScaleID = Shader.PropertyToID("NoiseScale");

	private static readonly int FogNormalID = Shader.PropertyToID("_FogNormalTex");

	private static readonly int FogNormalScaleID = Shader.PropertyToID("_FogNormalScale");

	private static readonly int m_normalScale = Shader.PropertyToID("_NormalScale");

	private static readonly int m_CamPos = Shader.PropertyToID("_CamPos");

	private static readonly int m_Params = Shader.PropertyToID("_Params");

	private static readonly int unexploredColor = Shader.PropertyToID("unexploredColor");

	private static readonly int exploredColor = Shader.PropertyToID("exploredColor");

	private static readonly int afterPostProceessID = Shader.PropertyToID("_AfterPostProcessTexture");

	private static RenderTargetIdentifier afterRenderIndentifier = new RenderTargetIdentifier(afterPostProceessID);

	private Transform cameraTransform;

	private HeightFogSettings heightFogSetting;

	private Camera camera;

	private static Material material;

	private RenderTargetIdentifier currentTarget;

	public HeightFogPass(RenderPassEvent evt)
	{
		base.renderPassEvent = evt;
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		if (material == null)
		{
			Debug.LogError("Material not created.");
			return;
		}
		CommandBuffer commandBuffer = CommandBufferPool.Get(k_RenderTag);
		Render(commandBuffer, ref renderingData);
		context.ExecuteCommandBuffer(commandBuffer);
		CommandBufferPool.Release(commandBuffer);
	}

	public void Setup(in RenderTargetIdentifier currentTarget, HeightFogSettings heightFogSetting)
	{
		this.currentTarget = currentTarget;
		this.heightFogSetting = heightFogSetting;
		if (heightFogSetting.fogShader == null)
		{
			Debug.LogError("HeightFog Shader not found.");
		}
		else if (material == null)
		{
			material = CoreUtils.CreateEngineMaterial(heightFogSetting.fogShader);
		}
	}

	private void Render(CommandBuffer cmd, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		if (cameraData.isSceneViewCamera || renderingData.cameraData.camera.cullingMask == 1 << LayerMask.NameToLayer("Hud3D") || renderingData.cameraData.camera.cullingMask == 1 << LayerMask.NameToLayer("UIObject3D") || renderingData.cameraData.camera.cullingMask == 1 << LayerMask.NameToLayer("GuideScene"))
		{
			return;
		}
		int num = (1 << LayerMask.NameToLayer("UIObject3D")) + (1 << LayerMask.NameToLayer("Grab"));
		if (renderingData.cameraData.camera.cullingMask == num)
		{
			return;
		}
		if (camera == null)
		{
			camera = Camera.main;
		}
		if (camera == null)
		{
			return;
		}
		RenderTargetIdentifier renderTargetIdentifier = currentTarget;
		int tempTargetId = TempTargetId;
		FOWSystem fowSystem = heightFogSetting.fowSystem;
		if (fowSystem == null)
		{
			return;
		}
		int scaledPixelWidth = cameraData.camera.scaledPixelWidth;
		int scaledPixelHeight = cameraData.camera.scaledPixelHeight;
		material.SetFloat(_FogPosY, heightFogSetting._FogPosY);
		material.SetFloat(_FogDisappearHeight, heightFogSetting._FogDisappearHeight);
		material.SetFloat(FogIntensity, heightFogSetting.FogIntensity);
		material.SetFloat(FogXSpeedId, heightFogSetting.FogXSpeed);
		material.SetFloat(FogYSpeedId, heightFogSetting.FogYSpeed);
		material.SetFloat(NoiseAmountID, heightFogSetting.NoiseAmount);
		material.SetTexture(FogNoiseTexId, heightFogSetting.noise2D);
		material.SetFloat(m_normalScale, heightFogSetting.NormalScale);
		material.SetTexture(FogNormalID, heightFogSetting.fogNormal);
		material.SetColor(exploredColor, heightFogSetting.exploredColor);
		material.SetColor(unexploredColor, heightFogSetting.unexploredColor);
		Vector4 value = camera.transform.position;
		cameraTransform = camera.transform;
		if (QualitySettings.antiAliasing > 0)
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer)
			{
				value.w = 1f;
			}
		}
		float num2 = 1f / (float)fowSystem.worldSize;
		Transform transform = fowSystem.transform;
		float num3 = transform.position.x - (float)fowSystem.worldSize * 0.5f;
		float num4 = transform.position.z - (float)fowSystem.worldSize * 0.5f;
		Vector4 value2 = new Vector4((0f - num3) * num2, (0f - num4) * num2, num2, fowSystem.blendFactor);
		Matrix4x4 identity = Matrix4x4.identity;
		float fieldOfView = camera.fieldOfView;
		float nearClipPlane = camera.nearClipPlane;
		float aspect = camera.aspect;
		float num5 = nearClipPlane * Mathf.Tan(fieldOfView * 0.5f * (MathF.PI / 180f));
		Vector3 vector = cameraTransform.right * num5 * aspect;
		Vector3 vector2 = cameraTransform.up * num5;
		Vector3 vector3 = cameraTransform.forward * nearClipPlane + vector2 - vector;
		float num6 = vector3.magnitude / nearClipPlane;
		vector3.Normalize();
		vector3 *= num6;
		Vector3 vector4 = cameraTransform.forward * nearClipPlane + vector + vector2;
		vector4.Normalize();
		vector4 *= num6;
		Vector3 vector5 = cameraTransform.forward * nearClipPlane - vector2 - vector;
		vector5.Normalize();
		vector5 *= num6;
		Vector3 vector6 = cameraTransform.forward * nearClipPlane + vector - vector2;
		vector6.Normalize();
		vector6 *= num6;
		identity.SetRow(0, vector5);
		identity.SetRow(1, vector6);
		identity.SetRow(2, vector4);
		identity.SetRow(3, vector3);
		material.SetMatrix(m_InverseMVP, identity);
		material.SetVector(m_CamPos, value);
		material.SetVector(m_Params, value2);
		material.SetMatrix(m_InverseMVP, identity);
		material.SetTexture(_FogTex0, heightFogSetting.fowSystem.texture0);
		material.SetTexture(_FogTex1, heightFogSetting.fowSystem.texture1);
		float renderScale = fowSystem.GetRenderScale();
		scaledPixelWidth = (int)((float)scaledPixelWidth * renderScale);
		scaledPixelHeight = (int)((float)scaledPixelHeight * renderScale);
		int pass = 0;
		RenderTextureFormat format = RenderTextureFormat.RGB111110Float;
		if (!SystemInfo.SupportsRenderTextureFormat(format))
		{
			format = RenderTextureFormat.DefaultHDR;
		}
		cmd.GetTemporaryRT(tempTargetId, scaledPixelWidth / 2, scaledPixelHeight / 2, 0, FilterMode.Point, format);
		cmd.Blit(renderTargetIdentifier, tempTargetId);
		cmd.Blit(null, cameraData.postProcessEnabled ? afterRenderIndentifier : renderTargetIdentifier, material, pass);
		cmd.ReleaseTemporaryRT(tempTargetId);
	}
}
