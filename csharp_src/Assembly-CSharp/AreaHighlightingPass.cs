using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AreaHighlightingPass : ScriptableRenderPass
{
	private bool isDebug;

	private int worldSize;

	private Vector3 centerPos;

	private Material mat;

	private float intensity;

	private Color inSideColor;

	private Color addSideColor;

	private Color darkColor;

	private Texture2D texture;

	private float brightness;

	private float saturation;

	private float contrast;

	private float _diffuseScale;

	private float _diffusePower;

	private float _cutThreshold;

	private RenderTargetIdentifier source;

	private RenderTargetHandle destination;

	private const string m_ProfilerTag = "AreaScreenPass";

	private RenderTargetHandle m_temporaryColorTexture01;

	private readonly int _IntenSity = Shader.PropertyToID("_LuminosityAmount");

	private readonly int _FogOfWar = Shader.PropertyToID("_FogOfWar");

	private readonly int m_Params = Shader.PropertyToID("_Params");

	private readonly int m_CamPos = Shader.PropertyToID("_CamPos");

	private readonly int m_InverseMVP = Shader.PropertyToID("_InverseMVP");

	private readonly int m_InColor = Shader.PropertyToID("inColor");

	private readonly int m_addSideColor = Shader.PropertyToID("addSideColor");

	private readonly int m_brightness = Shader.PropertyToID("_brightness");

	private readonly int m_saturation = Shader.PropertyToID("_saturation");

	private readonly int m_contrast = Shader.PropertyToID("_contrast");

	private readonly int m_diffusePower = Shader.PropertyToID("_diffusePower");

	private readonly int m_diffuseScale = Shader.PropertyToID("_diffuseScale");

	private readonly int m_cutThreshold = Shader.PropertyToID("_cutThreshold");

	private readonly int m_darkColor = Shader.PropertyToID("_darkColor");

	private static readonly int afterPostProceessID = Shader.PropertyToID("_AfterPostProcessTexture");

	private static RenderTargetIdentifier afterRenderIndentifier = new RenderTargetIdentifier(afterPostProceessID);

	private Transform cameraTransform;

	private Camera camera;

	private AreaHightingSystem areaInstance;

	public void Init()
	{
	}

	public void Setup(RenderTargetIdentifier source, RenderTargetHandle destination)
	{
		this.source = source;
		this.destination = destination;
	}

	public AreaHighlightingPass(int worldSize, Vector3 centerPos, Material mat, float intensity, Color inSideColor, float brightness, float saturation, float contrast, RenderPassEvent passEvent, float diffusePower, float diffuseScale, float _cutThreshold, Color addSideColor, bool isDebug, Color darkColor)
	{
		this.darkColor = darkColor;
		this.mat = mat;
		this.addSideColor = addSideColor;
		_diffusePower = diffusePower;
		_diffuseScale = diffuseScale;
		this.intensity = intensity;
		this.inSideColor = inSideColor;
		base.renderPassEvent = passEvent;
		this.brightness = brightness;
		this.saturation = saturation;
		this.contrast = contrast;
		this.worldSize = worldSize;
		this.centerPos = centerPos;
		this._cutThreshold = _cutThreshold;
		this.isDebug = isDebug;
		m_temporaryColorTexture01.Init("_temporaryColorTexture1");
	}

	public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
	{
		base.Configure(cmd, cameraTextureDescriptor);
	}

	public override void FrameCleanup(CommandBuffer cmd)
	{
		base.FrameCleanup(cmd);
	}

	public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
	{
		ref CameraData cameraData = ref renderingData.cameraData;
		if (renderingData.cameraData.camera.cullingMask == 1 << LayerMask.NameToLayer("UIObject3D") || renderingData.cameraData.camera.cullingMask == 1 << LayerMask.NameToLayer("GuideScene") || renderingData.cameraData.camera.cullingMask == 1 << LayerMask.NameToLayer("UI"))
		{
			return;
		}
		int num = (1 << LayerMask.NameToLayer("UIObject3D")) + (1 << LayerMask.NameToLayer("Grab"));
		if (renderingData.cameraData.camera.cullingMask == num || cameraData.isSceneViewCamera)
		{
			return;
		}
		if (camera == null)
		{
			camera = Camera.main;
		}
		if (areaInstance == null)
		{
			areaInstance = AreaHightingSystem.instance;
		}
		if (!(areaInstance == null))
		{
			Vector4 value = camera.transform.position;
			cameraTransform = camera.transform;
			Matrix4x4 identity = Matrix4x4.identity;
			float fieldOfView = camera.fieldOfView;
			float nearClipPlane = camera.nearClipPlane;
			float aspect = camera.aspect;
			float num2 = nearClipPlane * Mathf.Tan(fieldOfView * 0.5f * (MathF.PI / 180f));
			Vector3 vector = cameraTransform.right * num2 * aspect;
			Vector3 vector2 = cameraTransform.up * num2;
			Vector3 vector3 = cameraTransform.forward * nearClipPlane + vector2 - vector;
			float num3 = vector3.magnitude / nearClipPlane;
			vector3.Normalize();
			vector3 *= num3;
			Vector3 vector4 = cameraTransform.forward * nearClipPlane + vector + vector2;
			vector4.Normalize();
			vector4 *= num3;
			Vector3 vector5 = cameraTransform.forward * nearClipPlane - vector2 - vector;
			vector5.Normalize();
			vector5 *= num3;
			Vector3 vector6 = cameraTransform.forward * nearClipPlane + vector - vector2;
			vector6.Normalize();
			vector6 *= num3;
			identity.SetRow(0, vector5);
			identity.SetRow(1, vector6);
			identity.SetRow(2, vector4);
			identity.SetRow(3, vector3);
			float num4 = 1f / (float)worldSize;
			float num5 = centerPos.x - (float)worldSize * 0.5f;
			float num6 = centerPos.z - (float)worldSize * 0.5f;
			Vector4 value2 = new Vector4((0f - num5) * num4, (0f - num6) * num4, num4, 1f);
			CommandBuffer commandBuffer = CommandBufferPool.Get("AreaScreenPass");
			if (isDebug)
			{
				commandBuffer.SetGlobalVector(m_Params, value2);
				commandBuffer.SetGlobalTexture(_FogOfWar, areaInstance.texture1);
				commandBuffer.SetGlobalFloat(m_diffusePower, _diffusePower);
				commandBuffer.SetGlobalFloat(m_diffuseScale, _diffuseScale);
				commandBuffer.SetGlobalColor(m_InColor, new Color(0.08f, 0.13f, 0.3f, 1f));
				commandBuffer.SetGlobalColor(m_addSideColor, addSideColor);
				commandBuffer.SetGlobalFloat(m_cutThreshold, _cutThreshold);
				commandBuffer.SetGlobalColor(m_darkColor, darkColor);
			}
			else
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				commandBuffer.GetTemporaryRT(m_temporaryColorTexture01.id, cameraTargetDescriptor, FilterMode.Bilinear);
				mat.SetVector(m_Params, value2);
				mat.SetTexture(_FogOfWar, areaInstance.texture1);
				mat.SetMatrix(m_InverseMVP, identity);
				mat.SetVector(m_CamPos, value);
				mat.SetMatrix(m_InverseMVP, identity);
				mat.SetColor(m_InColor, new Color(0.08f, 0.13f, 0.3f, 1f));
				mat.SetFloat(m_brightness, brightness);
				mat.SetFloat(m_contrast, contrast);
				mat.SetFloat(m_saturation, saturation);
				commandBuffer.Blit(cameraData.postProcessEnabled ? afterRenderIndentifier : source, m_temporaryColorTexture01.Identifier(), mat);
				commandBuffer.Blit(m_temporaryColorTexture01.Identifier(), cameraData.postProcessEnabled ? afterRenderIndentifier : source);
				commandBuffer.ReleaseTemporaryRT(m_temporaryColorTexture01.id);
			}
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}
	}
}
