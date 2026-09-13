using UnityEngine;

namespace VolumetricFogAndMist2;

[CreateAssetMenu(menuName = "Volumetric Fog \u008b& Mist/Fog Profile", fileName = "VolumetricFogProfile", order = 1001)]
public class VolumetricFogProfile : ScriptableObject
{
	[Header("Rendering")]
	[Range(1f, 16f)]
	public int raymarchQuality = 6;

	[Range(0f, 2f)]
	public float dithering = 1f;

	[Range(0f, 2f)]
	public float jittering = 0.25f;

	[Tooltip("The render queue for this renderer. By default, all transparent objects use a render queue of 3000. Use a lower value to render before all transparent objects.")]
	public int renderQueue = 3100;

	[Tooltip("Optional sorting layer Id (number) for this renderer. By default 0. Usually used to control the order with other transparent renderers, like Sprite Renderer.")]
	public int sortingLayerID;

	[Tooltip("Optional sorting order for this renderer. Used to control the order with other transparent renderers, like Sprite Renderer.")]
	public int sortingOrder;

	[Header("Density")]
	public Texture2D noiseTexture;

	[Range(0f, 3f)]
	public float noiseStrength = 1f;

	public float noiseScale = 15f;

	public float noiseFinalMultiplier = 1f;

	public float density = 1f;

	[Header("Boundary")]
	public VolumetricFogShape shape;

	[Range(0f, 1f)]
	public float border = 0.05f;

	public float verticalOffset;

	[Tooltip("When enabled, makes fog appear at certain distance from a camera")]
	public float distance;

	[Range(0f, 1f)]
	public float distanceFallOff;

	[Header("Colors")]
	public float intencity = 1f;

	public Color albedo = new Color32(227, 227, 227, byte.MaxValue);

	public float brightness = 1f;

	[Range(0f, 2f)]
	public float deepObscurance = 1f;

	public Color specularColor = new Color(1f, 1f, 0.8f, 1f);

	[Range(0f, 1f)]
	public float specularThreshold = 0.637f;

	[Range(0f, 1f)]
	public float specularIntensity = 0.428f;

	[Header("Animation")]
	public float turbulence = 0.73f;

	public Vector3 windDirection = new Vector3(0.02f, 0f, 0f);

	[Header("Directional Light")]
	[Range(0f, 64f)]
	public float lightDiffusionPower = 32f;

	[Range(0f, 1f)]
	public float lightDiffusionIntensity = 0.4f;

	public bool receiveShadows;

	[Range(0f, 1f)]
	public float shadowIntensity = 0.5f;

	public float _InvFade = 0.1f;

	public Texture2D fogMask;

	public Texture2D fogNormal;

	public float normalScale;

	public float uvScale;

	public float fogNormalSpeed;

	public Texture2D disturbanceTexture;

	public float disturbanceSpeed;

	public float disturbanceScale;

	public float disturbanceIntencity;

	public Color mixColor = Color.white;

	public event OnSettingsChanged onSettingsChanged;

	private void OnEnable()
	{
		if (noiseTexture == null)
		{
			noiseTexture = Resources.Load<Texture2D>("Textures/NoiseTex256");
		}
	}

	private void OnValidate()
	{
		distance = Mathf.Max(0f, distance);
		density = Mathf.Max(0f, density);
		noiseScale = Mathf.Max(0.1f, noiseScale);
		if (this.onSettingsChanged != null)
		{
			this.onSettingsChanged();
		}
	}
}
