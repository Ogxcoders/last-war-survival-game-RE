using System;

namespace UnityEngine.Rendering.Universal;

[Serializable]
public class PostProcessData : ScriptableObject
{
	[Serializable]
	[ReloadGroup]
	public sealed class ShaderResources
	{
		[Reload("Shaders/PostProcessing/StopNaN.shader", ReloadAttribute.Package.Root)]
		public Shader stopNanPS;

		[Reload("Shaders/PostProcessing/SubpixelMorphologicalAntialiasing.shader", ReloadAttribute.Package.Root)]
		public Shader subpixelMorphologicalAntialiasingPS;

		[Reload("Shaders/PostProcessing/TiltShift.shader", ReloadAttribute.Package.Root)]
		public Shader tiltShiftPS;

		[Reload("Shaders/PostProcessing/GaussianDepthOfField.shader", ReloadAttribute.Package.Root)]
		public Shader gaussianDepthOfFieldPS;

		[Reload("Shaders/PostProcessing/BokehDepthOfField.shader", ReloadAttribute.Package.Root)]
		public Shader bokehDepthOfFieldPS;

		[Reload("Shaders/PostProcessing/CameraMotionBlur.shader", ReloadAttribute.Package.Root)]
		public Shader cameraMotionBlurPS;

		[Reload("Shaders/PostProcessing/PaniniProjection.shader", ReloadAttribute.Package.Root)]
		public Shader paniniProjectionPS;

		[Reload("Shaders/PostProcessing/LutBuilderLdr.shader", ReloadAttribute.Package.Root)]
		public Shader lutBuilderLdrPS;

		[Reload("Shaders/PostProcessing/LutBuilderHdr.shader", ReloadAttribute.Package.Root)]
		public Shader lutBuilderHdrPS;

		[Reload("Shaders/PostProcessing/Bloom.shader", ReloadAttribute.Package.Root)]
		public Shader bloomPS;

		[Reload("Shaders/PostProcessing/UberPost.shader", ReloadAttribute.Package.Root)]
		public Shader uberPostPS;

		[Reload("Shaders/PostProcessing/FinalPost.shader", ReloadAttribute.Package.Root)]
		public Shader finalPostPassPS;
	}

	[Serializable]
	[ReloadGroup]
	public sealed class TextureResources
	{
		public Texture2D[] blueNoise16LTex;

		public Texture2D[] filmGrainTex;

		public Texture2D smaaAreaTex;

		public Texture2D smaaSearchTex;
	}

	public ShaderResources shaders;

	public TextureResources textures;
}
