using System;

namespace UnityEngine.UIElements.UIR;

internal class RenderChainCommand : PoolItem
{
	internal VisualElement owner;

	internal RenderChainCommand prev;

	internal RenderChainCommand next;

	internal bool closing;

	internal CommandType type;

	internal State state;

	internal MeshHandle mesh;

	internal int indexOffset;

	internal int indexCount;

	internal Action callback;

	internal void Reset()
	{
		owner = null;
		prev = (next = null);
		closing = false;
		type = CommandType.Draw;
		state = default(State);
		mesh = null;
		indexOffset = (indexCount = 0);
		callback = null;
	}

	internal void ExecuteNonDrawMesh(DrawParams drawParams, bool straightY, float pixelsPerPoint, ref Exception immediateException)
	{
		switch (type)
		{
		case CommandType.Immediate:
		{
			if (immediateException != null)
			{
				break;
			}
			bool flag = drawParams.scissor.Count > 1;
			if (flag)
			{
				Utility.DisableScissor();
			}
			Utility.ProfileImmediateRendererBegin();
			try
			{
				using (new GUIClip.ParentClipScope(owner.worldTransform, owner.worldClip))
				{
					callback();
				}
			}
			catch (Exception ex)
			{
				immediateException = ex;
			}
			GL.modelview = drawParams.view.Peek().transform;
			GL.LoadProjectionMatrix(drawParams.projection);
			Utility.ProfileImmediateRendererEnd();
			if (flag)
			{
				Utility.SetScissorRect(RectPointsToPixelsAndFlipYAxis(drawParams.scissor.Peek(), drawParams.viewport, pixelsPerPoint));
			}
			break;
		}
		case CommandType.PushView:
		{
			VisualElement parent = owner.hierarchy.parent;
			Vector4 clipRect = ((parent == null) ? DrawParams.k_UnlimitedRect.ToVector4() : RectToScreenSpace(parent.worldClip, drawParams.projection, straightY));
			ViewTransform item = new ViewTransform
			{
				transform = owner.worldTransform,
				clipRect = clipRect
			};
			drawParams.view.Push(item);
			GL.modelview = item.transform;
			break;
		}
		case CommandType.PopView:
			drawParams.view.Pop();
			GL.modelview = drawParams.view.Peek().transform;
			break;
		case CommandType.PushScissor:
		{
			Rect rect2 = CombineScissorRects(owner.worldClip, drawParams.scissor.Peek());
			drawParams.scissor.Push(rect2);
			Utility.SetScissorRect(RectPointsToPixelsAndFlipYAxis(rect2, drawParams.viewport, pixelsPerPoint));
			break;
		}
		case CommandType.PopScissor:
		{
			drawParams.scissor.Pop();
			Rect rect = drawParams.scissor.Peek();
			if (rect.x == DrawParams.k_UnlimitedRect.x)
			{
				Utility.DisableScissor();
			}
			else
			{
				Utility.SetScissorRect(RectPointsToPixelsAndFlipYAxis(rect, drawParams.viewport, pixelsPerPoint));
			}
			break;
		}
		}
	}

	private static Vector4 RectToScreenSpace(Rect rc, Matrix4x4 projection, bool straightY)
	{
		RectInt activeViewport = Utility.GetActiveViewport();
		Vector3 vector = projection.MultiplyPoint(new Vector3(rc.xMin, rc.yMin, 0.995f));
		Vector3 vector2 = projection.MultiplyPoint(new Vector3(rc.xMax, rc.yMax, 0.995f));
		float num = (straightY ? 0.5f : (-0.5f));
		float a = (vector.x * 0.5f + 0.5f) * (float)activeViewport.width;
		float b = (vector2.x * 0.5f + 0.5f) * (float)activeViewport.width;
		float a2 = (vector.y * num + 0.5f) * (float)activeViewport.height;
		float b2 = (vector2.y * num + 0.5f) * (float)activeViewport.height;
		return new Vector4(Mathf.Min(a, b), Mathf.Min(a2, b2), Mathf.Max(a, b), Mathf.Max(a2, b2));
	}

	private static Rect CombineScissorRects(Rect r0, Rect r1)
	{
		Rect result = new Rect(0f, 0f, 0f, 0f);
		result.x = Math.Max(r0.x, r1.x);
		result.y = Math.Max(r0.y, r1.y);
		result.xMax = Math.Max(result.x, Math.Min(r0.xMax, r1.xMax));
		result.yMax = Math.Max(result.y, Math.Min(r0.yMax, r1.yMax));
		return result;
	}

	private static RectInt RectPointsToPixelsAndFlipYAxis(Rect rect, Rect viewport, float pixelsPerPoint)
	{
		RectInt result = new RectInt(0, 0, 0, 0);
		result.x = Mathf.RoundToInt(rect.x * pixelsPerPoint);
		result.y = Mathf.RoundToInt((viewport.height - rect.yMax) * pixelsPerPoint);
		result.width = Mathf.RoundToInt(rect.width * pixelsPerPoint);
		result.height = Mathf.RoundToInt(rect.height * pixelsPerPoint);
		return result;
	}
}
