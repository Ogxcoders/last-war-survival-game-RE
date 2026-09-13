using System;
using System.Collections.Generic;
using GameKit.Base;
using Leopotam.EcsLite;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public static class DataUIRenderLogic
{
	public struct RenderContext
	{
		public GameObject go_effect_hit;

		public GameObject go_effect_tanshe;

		public GameObject go_effect_tanshe_other;

		public GameObject go_effec_z_x;

		public GameObject go_effec_explode;

		public GameObject go_effect_impact;

		public GameObject go_effect_headshot;

		public Camera Camera;

		public Action<GameObject> Callack;

		public EcsWorld World;
	}

	private static Dictionary<Type, Action<IRender, RenderContext>> RendHandle = new Dictionary<Type, Action<IRender, RenderContext>>
	{
		{
			typeof(DataUIRenderMessage.UIEffect),
			UIEffectHandle
		},
		{
			typeof(DataUIRenderMessage.UIZhunXin),
			UIZhunXinHandle
		},
		{
			typeof(DataUIRenderMessage.UIEntityAction),
			UIEntityActionHandle
		},
		{
			typeof(DataUIRenderMessage.UICameraShake),
			UICameraShakeHandle
		}
	};

	public static void Render(IRender render, RenderContext context)
	{
		if (render != null)
		{
			RendHandle.TryGetValue(render.GetType(), out var value);
			if (value != null)
			{
				value?.Invoke(render, context);
			}
		}
	}

	private static void UIEffectHandle(IRender render, RenderContext context)
	{
		DataUIRenderMessage.UIEffect uIEffect = ((render is DataUIRenderMessage.UIEffect) ? ((DataUIRenderMessage.UIEffect)(object)render) : default(DataUIRenderMessage.UIEffect));
		GameObject prefab = null;
		if (uIEffect.EffectType == 2)
		{
			prefab = context.go_effect_hit;
			UICameraShakeHandle(render, context);
		}
		else if (uIEffect.EffectType == 1)
		{
			prefab = ((!uIEffect.IsMe) ? context.go_effect_tanshe_other : context.go_effect_tanshe);
			DataUISound.PlayerSound(5100010);
		}
		else if (uIEffect.EffectType == 3)
		{
			prefab = context.go_effec_explode;
		}
		else if (uIEffect.EffectType == 4)
		{
			prefab = context.go_effect_impact;
		}
		else if (uIEffect.EffectType == 5)
		{
			prefab = context.go_effect_headshot;
		}
		GameObject go = null;
		go = prefab.GameObjectSpawn();
		go.SetActive(value: true);
		go.transform.SetParent(uIEffect.Parent);
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = uIEffect.Position;
		go.transform.rotation = uIEffect.Rotation;
		GameEntry.Timer.RegisterTimer(2f, delegate
		{
			if (go != null)
			{
				go.GameObjectRecycle();
			}
		});
	}

	private static void UIEntityActionHandle(IRender render, RenderContext context)
	{
		DataUIRenderMessage.UIEntityAction entityAction = ((render is DataUIRenderMessage.UIEntityAction) ? ((DataUIRenderMessage.UIEntityAction)(object)render) : default(DataUIRenderMessage.UIEntityAction));
		if (entityAction.ActionType == 3)
		{
			entityAction.Controller?.GameEnd(entityAction, context);
		}
		if (entityAction.ActionType == 2)
		{
			entityAction.Controller?.Fire(entityAction, context);
		}
		if (entityAction.ActionType == 1)
		{
			entityAction.Controller?.Die();
		}
		if (entityAction.ActionType == 4)
		{
			entityAction.Controller?.Reload(context);
		}
	}

	private static void UIZhunXinHandle(IRender render, RenderContext context)
	{
		Vector3 mousePosition = Input.mousePosition;
		Vector3 position = context.Camera.ScreenToWorldPoint(mousePosition);
		context.go_effec_z_x.transform.position = position;
		if (!context.go_effec_z_x.gameObject.activeSelf)
		{
			context.go_effec_z_x.gameObject.SetActive(value: true);
		}
	}

	private static void UICameraShakeHandle(IRender render, RenderContext context)
	{
		context.Camera.GetOrAddComponent<DataUICameraShake>().TriggerShake();
	}
}
