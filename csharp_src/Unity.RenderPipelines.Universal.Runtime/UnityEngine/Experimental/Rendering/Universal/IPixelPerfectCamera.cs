namespace UnityEngine.Experimental.Rendering.Universal;

internal interface IPixelPerfectCamera
{
	int assetsPPU { get; set; }

	int refResolutionX { get; set; }

	int refResolutionY { get; set; }

	bool upscaleRT { get; set; }

	bool pixelSnapping { get; set; }

	bool cropFrameX { get; set; }

	bool cropFrameY { get; set; }

	bool stretchFill { get; set; }
}
