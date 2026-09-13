using System;
using FibMatrix.Rendering;
using Joker;
using Leopotam.EcsLite;
using MiniGame.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MiniGame.Biubiu.Client;

public class GameObjectHolder : IResourceHolder
{
	public struct InstanceExtra
	{
		public static InstanceExtra Default = new InstanceExtra
		{
			Position = Vector3.zero,
			Rotation = Vector3.zero,
			Scale = Vector3.one,
			Active = true,
			BodyPosition = Vector3.zero,
			BodyVelocity = Vector3.zero
		};

		public bool Active;

		public Vector3 Position;

		public Vector3 Rotation;

		public Vector3 Scale;

		public Vector3 BodyPosition;

		public Vector3 BodyVelocity;
	}

	private IResourceHolder ResourceHolder;

	private EnumClient.AssetType AssetType;

	private DataResourceLoaderEnv LoaderEnv;

	private InstanceExtra Extra;

	private bool WithFinish;

	private bool HasWithData;

	private EcsPackedEntityWithWorld PackEntityWithWorld;

	private bool IsWithWorld;

	public object Data { get; set; }

	public bool IsDone => ResourceHolder.IsDone;

	public bool IsError => ResourceHolder.IsError;

	public bool IsValid => ResourceHolder.IsValid;

	public float Progress => ResourceHolder.Progress;

	public string ErrorMessage => ResourceHolder.ErrorMessage;

	private bool CanWith
	{
		get
		{
			if (Data != null)
			{
				return HasWithData;
			}
			return false;
		}
	}

	public Action<IResourceHolder> OnDone { get; set; }

	public GameObjectHolder(IResourceHolder resourceHolder, DataResourceLoaderEnv loaderEnv, EnumClient.AssetType assetType)
	{
		ResourceHolder = resourceHolder;
		AssetType = assetType;
		LoaderEnv = loaderEnv;
		WithFinish = false;
		HasWithData = false;
		resourceHolder.OnDone = (Action<IResourceHolder>)Delegate.Combine(resourceHolder.OnDone, (Action<IResourceHolder>)delegate(IResourceHolder holder)
		{
			GameObject gameObject = null;
			if (!IsError && IsValid)
			{
				gameObject = UnityEngine.Object.Instantiate(holder.Data as GameObject);
				if (gameObject != null)
				{
					Log.Debug("[BiuBiu]:GameObjectHolder  Instantiate " + gameObject.gameObject.name);
				}
			}
			Data = gameObject;
			ExecuteWith();
		});
	}

	public T As<T>()
	{
		return (T)Data;
	}

	public void With(InstanceExtra extra)
	{
		Extra = extra;
		HasWithData = true;
		ExecuteWith();
	}

	public void WithWorld(EcsPackedEntityWithWorld ecsPackedEntityWithWorld)
	{
		PackEntityWithWorld = ecsPackedEntityWithWorld;
		HasWithData = true;
		IsWithWorld = true;
		ExecuteWith();
	}

	public void Dispose()
	{
		OnDone = null;
		if (Data != null)
		{
			UnityEngine.Object.Destroy(Data as GameObject);
			Data = null;
		}
		if (ResourceHolder != null)
		{
			ResourceHolder.Dispose();
			ResourceHolder = null;
		}
		WithFinish = false;
		HasWithData = false;
		LoaderEnv = null;
		IsWithWorld = false;
	}

	private void BuildInstanceExtra()
	{
		InstanceExtra extra = InstanceExtra.Default;
		if (PackEntityWithWorld.Unpack(out var world, out var entity))
		{
			float z = 0f;
			EcsPool<ComponentRotation> pool = world.GetPool<ComponentRotation>();
			if (pool.Has(entity))
			{
				z = pool.Get(entity).Rotation.AsFloat;
			}
			Vector3 position = Vector3.zero;
			EcsPool<ComponentPosition> pool2 = world.GetPool<ComponentPosition>();
			if (pool2.Has(entity))
			{
				position = pool2.Get(entity).Position.ToUnityVector3();
			}
			Vector3 bodyPosition = Vector3.zero;
			Vector3 bodyVelocity = Vector3.zero;
			EcsPool<ComponentPhysics> pool3 = world.GetPool<ComponentPhysics>();
			if (pool3.Has(entity))
			{
				ref ComponentPhysics reference = ref pool3.Get(entity);
				bodyPosition = reference.Body.GetPosition().ToUnityVector3();
				bodyVelocity = reference.Body.LinearVelocity.ToUnityVector3();
			}
			bool active = true;
			EcsPool<ComponentActivityChanged> pool4 = world.GetPool<ComponentActivityChanged>();
			if (pool4.Has(entity))
			{
				active = pool4.Get(entity).IsActive;
			}
			extra = new InstanceExtra
			{
				Position = position,
				Rotation = new Vector3(0f, 0f, z),
				Scale = Vector3.one,
				BodyPosition = bodyPosition,
				BodyVelocity = bodyVelocity,
				Active = active
			};
		}
		Extra = extra;
	}

	private void ExecuteWith()
	{
		if (!WithFinish && CanWith)
		{
			WithFinish = true;
			if (IsWithWorld)
			{
				BuildInstanceExtra();
			}
			GameObject gameObject = As<GameObject>();
			if (AssetType == EnumClient.AssetType.Map)
			{
				gameObject.transform.SetParent(LoaderEnv.TileMask);
				gameObject.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
				gameObject.GetOrAddComponent<UnityTileOutLine>().SetToGenMesh();
			}
			else if (AssetType == EnumClient.AssetType.Character)
			{
				gameObject.transform.SetParent(LoaderEnv.PlayerRoot);
				gameObject.GetComponentInChildren<DataUIPlayerController>()?.BindLoaderEnv(LoaderEnv);
			}
			else if (AssetType == EnumClient.AssetType.Env || AssetType == EnumClient.AssetType.UI)
			{
				gameObject.transform.SetParent(LoaderEnv.GameRoot);
			}
			else
			{
				gameObject.transform.SetParent(LoaderEnv.DynamicRoot);
			}
			if (AssetType == EnumClient.AssetType.Bullet)
			{
				float num = 1f / LoaderEnv.SizeToUnit;
				Quaternion quaternion = Quaternion.LookRotation(Extra.BodyVelocity, Vector3.up);
				gameObject.transform.localRotation = quaternion * Quaternion.Euler(new Vector3(0f, -90f, 0f));
				gameObject.transform.localScale = Extra.Scale;
				gameObject.transform.localPosition = Extra.BodyPosition * num;
			}
			else if (AssetType == EnumClient.AssetType.Map)
			{
				gameObject.transform.localRotation = Quaternion.Euler(Extra.Rotation);
				gameObject.transform.localScale = Vector3.one * 0.01f;
				gameObject.transform.localPosition = Extra.Position;
			}
			else
			{
				gameObject.transform.localRotation = Quaternion.Euler(Extra.Rotation);
				gameObject.transform.localScale = Extra.Scale;
				gameObject.transform.localPosition = Extra.Position;
			}
			gameObject.SetActive(Extra.Active);
			OnDone?.Invoke(this);
		}
	}
}
