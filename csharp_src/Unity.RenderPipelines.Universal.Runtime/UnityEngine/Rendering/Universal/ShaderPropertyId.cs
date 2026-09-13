namespace UnityEngine.Rendering.Universal;

internal static class ShaderPropertyId
{
	public static readonly int scaledScreenParams = Shader.PropertyToID("_ScaledScreenParams");

	public static readonly int worldSpaceCameraPos = Shader.PropertyToID("_WorldSpaceCameraPos");

	public static readonly int screenParams = Shader.PropertyToID("_ScreenParams");

	public static readonly int projectionParams = Shader.PropertyToID("_ProjectionParams");

	public static readonly int zBufferParams = Shader.PropertyToID("_ZBufferParams");

	public static readonly int orthoParams = Shader.PropertyToID("unity_OrthoParams");

	public static readonly int viewMatrix = Shader.PropertyToID("unity_MatrixV");

	public static readonly int projectionMatrix = Shader.PropertyToID("glstate_matrix_projection");

	public static readonly int viewAndProjectionMatrix = Shader.PropertyToID("unity_MatrixVP");

	public static readonly int inverseViewMatrix = Shader.PropertyToID("unity_MatrixInvV");

	public static readonly int inverseViewAndProjectionMatrix = Shader.PropertyToID("unity_MatrixInvVP");

	public static readonly int cameraProjectionMatrix = Shader.PropertyToID("unity_CameraProjection");

	public static readonly int inverseCameraProjectionMatrix = Shader.PropertyToID("unity_CameraInvProjection");

	public static readonly int worldToCameraMatrix = Shader.PropertyToID("unity_WorldToCamera");

	public static readonly int cameraToWorldMatrix = Shader.PropertyToID("unity_CameraToWorld");
}
