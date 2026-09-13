using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BitBenderGames;
using DG.Tweening;
using Framework.Utils.UnityEx;
using GameFramework.Localization;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;
using UnityEngine.UI;
using XLua.CSObjectWrap;
using XLua.LuaDLL;

namespace XLua;

public class ObjectTranslator
{
	internal enum LOGLEVEL
	{
		NO,
		INFO,
		WARN,
		ERROR
	}

	public delegate void PushCSObject(IntPtr L, object obj);

	public delegate object GetCSObject(IntPtr L, int idx);

	public delegate void UpdateCSObject(IntPtr L, int idx, object obj);

	public delegate void GetFunc<T>(IntPtr L, int idx, out T val);

	private class IniterAdderFrameworkUtilsUnityExTouchInfo
	{
		static IniterAdderFrameworkUtilsUnityExTouchInfo()
		{
			LuaEnv.AddIniter(Init);
		}

		private static void Init(LuaEnv luaenv, ObjectTranslator translator)
		{
			translator.RegisterPushAndGetAndUpdate<TouchInfo>(translator.PushFrameworkUtilsUnityExTouchInfo, translator.Get, translator.UpdateFrameworkUtilsUnityExTouchInfo);
			translator.RegisterPushAndGetAndUpdate<Vector2>(translator.PushUnityEngineVector2, translator.Get, translator.UpdateUnityEngineVector2);
			translator.RegisterPushAndGetAndUpdate<Vector3>(translator.PushUnityEngineVector3, translator.Get, translator.UpdateUnityEngineVector3);
			translator.RegisterPushAndGetAndUpdate<Vector4>(translator.PushUnityEngineVector4, translator.Get, translator.UpdateUnityEngineVector4);
			translator.RegisterPushAndGetAndUpdate<Color>(translator.PushUnityEngineColor, translator.Get, translator.UpdateUnityEngineColor);
			translator.RegisterPushAndGetAndUpdate<Quaternion>(translator.PushUnityEngineQuaternion, translator.Get, translator.UpdateUnityEngineQuaternion);
			translator.RegisterPushAndGetAndUpdate<Ray>(translator.PushUnityEngineRay, translator.Get, translator.UpdateUnityEngineRay);
			translator.RegisterPushAndGetAndUpdate<Bounds>(translator.PushUnityEngineBounds, translator.Get, translator.UpdateUnityEngineBounds);
			translator.RegisterPushAndGetAndUpdate<Ray2D>(translator.PushUnityEngineRay2D, translator.Get, translator.UpdateUnityEngineRay2D);
			translator.RegisterPushAndGetAndUpdate<BindingFlags>(translator.PushSystemReflectionBindingFlags, translator.Get, translator.UpdateSystemReflectionBindingFlags);
			translator.RegisterPushAndGetAndUpdate<KeyCode>(translator.PushUnityEngineKeyCode, translator.Get, translator.UpdateUnityEngineKeyCode);
			translator.RegisterPushAndGetAndUpdate<Camera.GateFitMode>(translator.PushUnityEngineCameraGateFitMode, translator.Get, translator.UpdateUnityEngineCameraGateFitMode);
			translator.RegisterPushAndGetAndUpdate<Camera.FieldOfViewAxis>(translator.PushUnityEngineCameraFieldOfViewAxis, translator.Get, translator.UpdateUnityEngineCameraFieldOfViewAxis);
			translator.RegisterPushAndGetAndUpdate<Camera.StereoscopicEye>(translator.PushUnityEngineCameraStereoscopicEye, translator.Get, translator.UpdateUnityEngineCameraStereoscopicEye);
			translator.RegisterPushAndGetAndUpdate<Camera.MonoOrStereoscopicEye>(translator.PushUnityEngineCameraMonoOrStereoscopicEye, translator.Get, translator.UpdateUnityEngineCameraMonoOrStereoscopicEye);
			translator.RegisterPushAndGetAndUpdate<Ease>(translator.PushDGTweeningEase, translator.Get, translator.UpdateDGTweeningEase);
			translator.RegisterPushAndGetAndUpdate<DOTweenAnimation.AnimationType>(translator.PushDGTweeningDOTweenAnimationAnimationType, translator.Get, translator.UpdateDGTweeningDOTweenAnimationAnimationType);
			translator.RegisterPushAndGetAndUpdate<DOTweenAnimation.TargetType>(translator.PushDGTweeningDOTweenAnimationTargetType, translator.Get, translator.UpdateDGTweeningDOTweenAnimationTargetType);
			translator.RegisterPushAndGetAndUpdate<TextMeshProUGUIEx.HorizontalAlignmentOptions>(translator.PushTextMeshProUGUIExHorizontalAlignmentOptions, translator.Get, translator.UpdateTextMeshProUGUIExHorizontalAlignmentOptions);
			translator.RegisterPushAndGetAndUpdate<TextMeshProUGUIEx.VerticalAlignmentOptions>(translator.PushTextMeshProUGUIExVerticalAlignmentOptions, translator.Get, translator.UpdateTextMeshProUGUIExVerticalAlignmentOptions);
			translator.RegisterPushAndGetAndUpdate<RectTransform.Edge>(translator.PushUnityEngineRectTransformEdge, translator.Get, translator.UpdateUnityEngineRectTransformEdge);
			translator.RegisterPushAndGetAndUpdate<RectTransform.Axis>(translator.PushUnityEngineRectTransformAxis, translator.Get, translator.UpdateUnityEngineRectTransformAxis);
			translator.RegisterPushAndGetAndUpdate<PointerEventData.InputButton>(translator.PushUnityEngineEventSystemsPointerEventDataInputButton, translator.Get, translator.UpdateUnityEngineEventSystemsPointerEventDataInputButton);
			translator.RegisterPushAndGetAndUpdate<PointerEventData.FramePressState>(translator.PushUnityEngineEventSystemsPointerEventDataFramePressState, translator.Get, translator.UpdateUnityEngineEventSystemsPointerEventDataFramePressState);
			translator.RegisterPushAndGetAndUpdate<RenderTextureFormat>(translator.PushUnityEngineRenderTextureFormat, translator.Get, translator.UpdateUnityEngineRenderTextureFormat);
			translator.RegisterPushAndGetAndUpdate<Space>(translator.PushUnityEngineSpace, translator.Get, translator.UpdateUnityEngineSpace);
			translator.RegisterPushAndGetAndUpdate<QueryTriggerInteraction>(translator.PushUnityEngineQueryTriggerInteraction, translator.Get, translator.UpdateUnityEngineQueryTriggerInteraction);
			translator.RegisterPushAndGetAndUpdate<AntialiasingMode>(translator.PushUnityEngineRenderingUniversalAntialiasingMode, translator.Get, translator.UpdateUnityEngineRenderingUniversalAntialiasingMode);
			translator.RegisterPushAndGetAndUpdate<Selectable.Transition>(translator.PushUnityEngineUISelectableTransition, translator.Get, translator.UpdateUnityEngineUISelectableTransition);
			translator.RegisterPushAndGetAndUpdate<InputField.ContentType>(translator.PushUnityEngineUIInputFieldContentType, translator.Get, translator.UpdateUnityEngineUIInputFieldContentType);
			translator.RegisterPushAndGetAndUpdate<InputField.InputType>(translator.PushUnityEngineUIInputFieldInputType, translator.Get, translator.UpdateUnityEngineUIInputFieldInputType);
			translator.RegisterPushAndGetAndUpdate<InputField.CharacterValidation>(translator.PushUnityEngineUIInputFieldCharacterValidation, translator.Get, translator.UpdateUnityEngineUIInputFieldCharacterValidation);
			translator.RegisterPushAndGetAndUpdate<InputField.LineType>(translator.PushUnityEngineUIInputFieldLineType, translator.Get, translator.UpdateUnityEngineUIInputFieldLineType);
			translator.RegisterPushAndGetAndUpdate<Image.Type>(translator.PushUnityEngineUIImageType, translator.Get, translator.UpdateUnityEngineUIImageType);
			translator.RegisterPushAndGetAndUpdate<Image.FillMethod>(translator.PushUnityEngineUIImageFillMethod, translator.Get, translator.UpdateUnityEngineUIImageFillMethod);
			translator.RegisterPushAndGetAndUpdate<Image.OriginHorizontal>(translator.PushUnityEngineUIImageOriginHorizontal, translator.Get, translator.UpdateUnityEngineUIImageOriginHorizontal);
			translator.RegisterPushAndGetAndUpdate<Image.OriginVertical>(translator.PushUnityEngineUIImageOriginVertical, translator.Get, translator.UpdateUnityEngineUIImageOriginVertical);
			translator.RegisterPushAndGetAndUpdate<Image.Origin90>(translator.PushUnityEngineUIImageOrigin90, translator.Get, translator.UpdateUnityEngineUIImageOrigin90);
			translator.RegisterPushAndGetAndUpdate<Image.Origin180>(translator.PushUnityEngineUIImageOrigin180, translator.Get, translator.UpdateUnityEngineUIImageOrigin180);
			translator.RegisterPushAndGetAndUpdate<Image.Origin360>(translator.PushUnityEngineUIImageOrigin360, translator.Get, translator.UpdateUnityEngineUIImageOrigin360);
			translator.RegisterPushAndGetAndUpdate<ScrollRect.MovementType>(translator.PushUnityEngineUIScrollRectMovementType, translator.Get, translator.UpdateUnityEngineUIScrollRectMovementType);
			translator.RegisterPushAndGetAndUpdate<ScrollRect.ScrollbarVisibility>(translator.PushUnityEngineUIScrollRectScrollbarVisibility, translator.Get, translator.UpdateUnityEngineUIScrollRectScrollbarVisibility);
			translator.RegisterPushAndGetAndUpdate<Slider.Direction>(translator.PushUnityEngineUISliderDirection, translator.Get, translator.UpdateUnityEngineUISliderDirection);
			translator.RegisterPushAndGetAndUpdate<Toggle.ToggleTransition>(translator.PushUnityEngineUIToggleToggleTransition, translator.Get, translator.UpdateUnityEngineUIToggleToggleTransition);
			translator.RegisterPushAndGetAndUpdate<GridLayoutGroup.Corner>(translator.PushUnityEngineUIGridLayoutGroupCorner, translator.Get, translator.UpdateUnityEngineUIGridLayoutGroupCorner);
			translator.RegisterPushAndGetAndUpdate<GridLayoutGroup.Axis>(translator.PushUnityEngineUIGridLayoutGroupAxis, translator.Get, translator.UpdateUnityEngineUIGridLayoutGroupAxis);
			translator.RegisterPushAndGetAndUpdate<GridLayoutGroup.Constraint>(translator.PushUnityEngineUIGridLayoutGroupConstraint, translator.Get, translator.UpdateUnityEngineUIGridLayoutGroupConstraint);
			translator.RegisterPushAndGetAndUpdate<ContentSizeFitter.FitMode>(translator.PushUnityEngineUIContentSizeFitterFitMode, translator.Get, translator.UpdateUnityEngineUIContentSizeFitterFitMode);
			translator.RegisterPushAndGetAndUpdate<SuperTextMesh.Alignment>(translator.PushSuperTextMeshAlignment, translator.Get, translator.UpdateSuperTextMeshAlignment);
			translator.RegisterPushAndGetAndUpdate<AnimatorCullingMode>(translator.PushUnityEngineAnimatorCullingMode, translator.Get, translator.UpdateUnityEngineAnimatorCullingMode);
			translator.RegisterPushAndGetAndUpdate<TextAnchor>(translator.PushUnityEngineTextAnchor, translator.Get, translator.UpdateUnityEngineTextAnchor);
			translator.RegisterPushAndGetAndUpdate<ScrollView.MovementType>(translator.PushScrollViewMovementType, translator.Get, translator.UpdateScrollViewMovementType);
			translator.RegisterPushAndGetAndUpdate<ScrollView.ScrollbarVisibility>(translator.PushScrollViewScrollbarVisibility, translator.Get, translator.UpdateScrollViewScrollbarVisibility);
			translator.RegisterPushAndGetAndUpdate<ScrollView.ScrollViewLayoutType>(translator.PushScrollViewScrollViewLayoutType, translator.Get, translator.UpdateScrollViewScrollViewLayoutType);
			translator.RegisterPushAndGetAndUpdate<TouchPhase>(translator.PushUnityEngineTouchPhase, translator.Get, translator.UpdateUnityEngineTouchPhase);
			translator.RegisterPushAndGetAndUpdate<MobileTouchCamera.State>(translator.PushBitBenderGamesMobileTouchCameraState, translator.Get, translator.UpdateBitBenderGamesMobileTouchCameraState);
			translator.RegisterPushAndGetAndUpdate<Language>(translator.PushGameFrameworkLocalizationLanguage, translator.Get, translator.UpdateGameFrameworkLocalizationLanguage);
			translator.RegisterPushAndGetAndUpdate<GameDefines.CityLabelColorType>(translator.PushGameDefinesCityLabelColorType, translator.Get, translator.UpdateGameDefinesCityLabelColorType);
			translator.RegisterPushAndGetAndUpdate<GameDefines.BuildConnectRoadDirection>(translator.PushGameDefinesBuildConnectRoadDirection, translator.Get, translator.UpdateGameDefinesBuildConnectRoadDirection);
			translator.RegisterPushAndGetAndUpdate<GameDefines.DirectionType>(translator.PushGameDefinesDirectionType, translator.Get, translator.UpdateGameDefinesDirectionType);
			translator.RegisterPushAndGetAndUpdate<ResourceManager.PreloadType>(translator.PushResourceManagerPreloadType, translator.Get, translator.UpdateResourceManagerPreloadType);
			translator.RegisterPushAndGetAndUpdate<LODType>(translator.PushLODType, translator.Get, translator.UpdateLODType);
			translator.RegisterPushAndGetAndUpdate<SceneManager.SceneID>(translator.PushSceneManagerSceneID, translator.Get, translator.UpdateSceneManagerSceneID);
			translator.RegisterPushAndGetAndUpdate<CityBuilding.BuildSceneType>(translator.PushCityBuildingBuildSceneType, translator.Get, translator.UpdateCityBuildingBuildSceneType);
			translator.RegisterPushAndGetAndUpdate<ModelManager.ModelObjectType>(translator.PushModelManagerModelObjectType, translator.Get, translator.UpdateModelManagerModelObjectType);
			translator.RegisterPushAndGetAndUpdate<FakeModelManager.TempRoadType>(translator.PushFakeModelManagerTempRoadType, translator.Get, translator.UpdateFakeModelManagerTempRoadType);
			translator.RegisterPushAndGetAndUpdate<WorldMarchDataManager.BattleWordType>(translator.PushWorldMarchDataManagerBattleWordType, translator.Get, translator.UpdateWorldMarchDataManagerBattleWordType);
			translator.RegisterPushAndGetAndUpdate<NewQueueState>(translator.PushNewQueueState, translator.Get, translator.UpdateNewQueueState);
			translator.RegisterPushAndGetAndUpdate<ResourceType>(translator.PushResourceType, translator.Get, translator.UpdateResourceType);
			translator.RegisterPushAndGetAndUpdate<BuildingState>(translator.PushBuildingState, translator.Get, translator.UpdateBuildingState);
			translator.RegisterPushAndGetAndUpdate<PlaceBuildType>(translator.PushPlaceBuildType, translator.Get, translator.UpdatePlaceBuildType);
			translator.RegisterPushAndGetAndUpdate<MarchStatus>(translator.PushMarchStatus, translator.Get, translator.UpdateMarchStatus);
			translator.RegisterPushAndGetAndUpdate<ClipCaps>(translator.PushUnityEngineTimelineClipCaps, translator.Get, translator.UpdateUnityEngineTimelineClipCaps);
			translator.RegisterPushAndGetAndUpdate<TextAlignmentOptions>(translator.PushTMProTextAlignmentOptions, translator.Get, translator.UpdateTMProTextAlignmentOptions);
			translator.RegisterPushAndGetAndUpdate<TMP_InputField.ContentType>(translator.PushTMProTMP_InputFieldContentType, translator.Get, translator.UpdateTMProTMP_InputFieldContentType);
			translator.RegisterPushAndGetAndUpdate<TMP_InputField.InputType>(translator.PushTMProTMP_InputFieldInputType, translator.Get, translator.UpdateTMProTMP_InputFieldInputType);
			translator.RegisterPushAndGetAndUpdate<TMP_InputField.CharacterValidation>(translator.PushTMProTMP_InputFieldCharacterValidation, translator.Get, translator.UpdateTMProTMP_InputFieldCharacterValidation);
			translator.RegisterPushAndGetAndUpdate<TMP_InputField.LineType>(translator.PushTMProTMP_InputFieldLineType, translator.Get, translator.UpdateTMProTMP_InputFieldLineType);
			translator.RegisterPushAndGetAndUpdate<TMP_InputFieldEx.HorizontalAlignmentOptions>(translator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions, translator.Get, translator.UpdateTMProTMP_InputFieldExHorizontalAlignmentOptions);
			translator.RegisterPushAndGetAndUpdate<InstanceRequest.State>(translator.PushInstanceRequestState, translator.Get, translator.UpdateInstanceRequestState);
			translator.RegisterPushAndGetAndUpdate<NewMarchType>(translator.PushNewMarchType, translator.Get, translator.UpdateNewMarchType);
			translator.RegisterPushAndGetAndUpdate<WorldPointType>(translator.PushWorldPointType, translator.Get, translator.UpdateWorldPointType);
			translator.RegisterPushAndGetAndUpdate<ListItemArrangeType>(translator.PushSuperScrollViewListItemArrangeType, translator.Get, translator.UpdateSuperScrollViewListItemArrangeType);
			translator.RegisterPushAndGetAndUpdate<URLGroupType>(translator.PushURLGroupType, translator.Get, translator.UpdateURLGroupType);
			translator.RegisterPushAndGetAndUpdate<MeteoriteWorldEffectPlayer.FragmentData.FragmentType>(translator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType, translator.Get, translator.UpdateMeteoriteWorldEffectPlayerFragmentDataFragmentType);
			translator.RegisterPushAndGetAndUpdate<WorldMeteoritePoint.MeteoritePointState>(translator.PushWorldMeteoritePointMeteoritePointState, translator.Get, translator.UpdateWorldMeteoritePointMeteoritePointState);
			translator.RegisterPushAndGetAndUpdate<BattleColliderUtils.ColliderType>(translator.PushBattleColliderUtilsColliderType, translator.Get, translator.UpdateBattleColliderUtilsColliderType);
			translator.RegisterPushAndGetAndUpdate<ViewSkinProPropertyRecorder.CodeType>(translator.PushViewSkinProPropertyRecorderCodeType, translator.Get, translator.UpdateViewSkinProPropertyRecorderCodeType);
			translator.RegisterPushAndGetAndUpdate<FOWSystem.LOSChecks>(translator.PushFOWSystemLOSChecks, translator.Get, translator.UpdateFOWSystemLOSChecks);
			translator.RegisterPushAndGetAndUpdate<FOWSystem.State>(translator.PushFOWSystemState, translator.Get, translator.UpdateFOWSystemState);
			translator.RegisterPushAndGetAndUpdate<ObjectPoolTag>(translator.PushObjectPoolTag, translator.Get, translator.UpdateObjectPoolTag);
			translator.RegisterPushAndGetAndUpdate<ObjectPoolTagGroup>(translator.PushObjectPoolTagGroup, translator.Get, translator.UpdateObjectPoolTagGroup);
			translator.RegisterPushAndGetAndUpdate<DeviceLevel>(translator.PushDeviceLevel, translator.Get, translator.UpdateDeviceLevel);
			translator.RegisterPushAndGetAndUpdate<PlayerType>(translator.PushPlayerType, translator.Get, translator.UpdatePlayerType);
		}
	}

	internal MethodWrapsCache methodWrapsCache;

	internal ObjectCheckers objectCheckers;

	internal ObjectCasters objectCasters;

	internal readonly ObjectPool objects = new ObjectPool();

	internal readonly Dictionary<object, int> reverseMap = new Dictionary<object, int>(new ReferenceEqualsComparer());

	internal LuaEnv luaEnv;

	internal StaticLuaCallbacks metaFunctions;

	internal List<Assembly> assemblies;

	private lua_CSFunction importTypeFunction;

	private lua_CSFunction loadAssemblyFunction;

	private lua_CSFunction castFunction;

	private readonly Dictionary<Type, Action<IntPtr>> delayWrap = new Dictionary<Type, Action<IntPtr>>();

	private readonly Dictionary<Type, Func<int, LuaEnv, LuaBase>> interfaceBridgeCreators = new Dictionary<Type, Func<int, LuaEnv, LuaBase>>();

	private readonly Dictionary<Type, Type> aliasCfg = new Dictionary<Type, Type>();

	private Dictionary<Type, bool> loaded_types = new Dictionary<Type, bool>();

	public int cacheRef;

	private MethodInfo[] genericAction;

	private MethodInfo[] genericFunc;

	private Dictionary<Type, Func<DelegateBridgeBase, Delegate>> delegateCreatorCache = new Dictionary<Type, Func<DelegateBridgeBase, Delegate>>();

	private Dictionary<int, WeakReference> delegate_bridges = new Dictionary<int, WeakReference>();

	private int common_array_meta = -1;

	private int common_delegate_meta = -1;

	private int enumerable_pairs_func = -1;

	private Dictionary<Type, int> typeIdMap = new Dictionary<Type, int>();

	private Dictionary<int, Type> typeMap = new Dictionary<int, Type>();

	private HashSet<Type> privateAccessibleFlags = new HashSet<Type>();

	private Dictionary<object, int> enumMap = new Dictionary<object, int>();

	private List<lua_CSFunction> fix_cs_functions = new List<lua_CSFunction>();

	private Dictionary<Type, PushCSObject> custom_push_funcs = new Dictionary<Type, PushCSObject>();

	private Dictionary<Type, GetCSObject> custom_get_funcs = new Dictionary<Type, GetCSObject>();

	private Dictionary<Type, UpdateCSObject> custom_update_funcs = new Dictionary<Type, UpdateCSObject>();

	private Dictionary<Type, Delegate> push_func_with_type;

	private Dictionary<Type, Delegate> get_func_with_type;

	private int decimal_type_id = -1;

	private static IniterAdderFrameworkUtilsUnityExTouchInfo s_IniterAdderFrameworkUtilsUnityExTouchInfo_dumb_obj = new IniterAdderFrameworkUtilsUnityExTouchInfo();

	private int FrameworkUtilsUnityExTouchInfo_TypeID = -1;

	private int UnityEngineVector2_TypeID = -1;

	private int UnityEngineVector3_TypeID = -1;

	private int UnityEngineVector4_TypeID = -1;

	private int UnityEngineColor_TypeID = -1;

	private int UnityEngineQuaternion_TypeID = -1;

	private int UnityEngineRay_TypeID = -1;

	private int UnityEngineBounds_TypeID = -1;

	private int UnityEngineRay2D_TypeID = -1;

	private int SystemReflectionBindingFlags_TypeID = -1;

	private int SystemReflectionBindingFlags_EnumRef = -1;

	private int UnityEngineKeyCode_TypeID = -1;

	private int UnityEngineKeyCode_EnumRef = -1;

	private int UnityEngineCameraGateFitMode_TypeID = -1;

	private int UnityEngineCameraGateFitMode_EnumRef = -1;

	private int UnityEngineCameraFieldOfViewAxis_TypeID = -1;

	private int UnityEngineCameraFieldOfViewAxis_EnumRef = -1;

	private int UnityEngineCameraStereoscopicEye_TypeID = -1;

	private int UnityEngineCameraStereoscopicEye_EnumRef = -1;

	private int UnityEngineCameraMonoOrStereoscopicEye_TypeID = -1;

	private int UnityEngineCameraMonoOrStereoscopicEye_EnumRef = -1;

	private int DGTweeningEase_TypeID = -1;

	private int DGTweeningEase_EnumRef = -1;

	private int DGTweeningDOTweenAnimationAnimationType_TypeID = -1;

	private int DGTweeningDOTweenAnimationAnimationType_EnumRef = -1;

	private int DGTweeningDOTweenAnimationTargetType_TypeID = -1;

	private int DGTweeningDOTweenAnimationTargetType_EnumRef = -1;

	private int TextMeshProUGUIExHorizontalAlignmentOptions_TypeID = -1;

	private int TextMeshProUGUIExHorizontalAlignmentOptions_EnumRef = -1;

	private int TextMeshProUGUIExVerticalAlignmentOptions_TypeID = -1;

	private int TextMeshProUGUIExVerticalAlignmentOptions_EnumRef = -1;

	private int UnityEngineRectTransformEdge_TypeID = -1;

	private int UnityEngineRectTransformEdge_EnumRef = -1;

	private int UnityEngineRectTransformAxis_TypeID = -1;

	private int UnityEngineRectTransformAxis_EnumRef = -1;

	private int UnityEngineEventSystemsPointerEventDataInputButton_TypeID = -1;

	private int UnityEngineEventSystemsPointerEventDataInputButton_EnumRef = -1;

	private int UnityEngineEventSystemsPointerEventDataFramePressState_TypeID = -1;

	private int UnityEngineEventSystemsPointerEventDataFramePressState_EnumRef = -1;

	private int UnityEngineRenderTextureFormat_TypeID = -1;

	private int UnityEngineRenderTextureFormat_EnumRef = -1;

	private int UnityEngineSpace_TypeID = -1;

	private int UnityEngineSpace_EnumRef = -1;

	private int UnityEngineQueryTriggerInteraction_TypeID = -1;

	private int UnityEngineQueryTriggerInteraction_EnumRef = -1;

	private int UnityEngineRenderingUniversalAntialiasingMode_TypeID = -1;

	private int UnityEngineRenderingUniversalAntialiasingMode_EnumRef = -1;

	private int UnityEngineUISelectableTransition_TypeID = -1;

	private int UnityEngineUISelectableTransition_EnumRef = -1;

	private int UnityEngineUIInputFieldContentType_TypeID = -1;

	private int UnityEngineUIInputFieldContentType_EnumRef = -1;

	private int UnityEngineUIInputFieldInputType_TypeID = -1;

	private int UnityEngineUIInputFieldInputType_EnumRef = -1;

	private int UnityEngineUIInputFieldCharacterValidation_TypeID = -1;

	private int UnityEngineUIInputFieldCharacterValidation_EnumRef = -1;

	private int UnityEngineUIInputFieldLineType_TypeID = -1;

	private int UnityEngineUIInputFieldLineType_EnumRef = -1;

	private int UnityEngineUIImageType_TypeID = -1;

	private int UnityEngineUIImageType_EnumRef = -1;

	private int UnityEngineUIImageFillMethod_TypeID = -1;

	private int UnityEngineUIImageFillMethod_EnumRef = -1;

	private int UnityEngineUIImageOriginHorizontal_TypeID = -1;

	private int UnityEngineUIImageOriginHorizontal_EnumRef = -1;

	private int UnityEngineUIImageOriginVertical_TypeID = -1;

	private int UnityEngineUIImageOriginVertical_EnumRef = -1;

	private int UnityEngineUIImageOrigin90_TypeID = -1;

	private int UnityEngineUIImageOrigin90_EnumRef = -1;

	private int UnityEngineUIImageOrigin180_TypeID = -1;

	private int UnityEngineUIImageOrigin180_EnumRef = -1;

	private int UnityEngineUIImageOrigin360_TypeID = -1;

	private int UnityEngineUIImageOrigin360_EnumRef = -1;

	private int UnityEngineUIScrollRectMovementType_TypeID = -1;

	private int UnityEngineUIScrollRectMovementType_EnumRef = -1;

	private int UnityEngineUIScrollRectScrollbarVisibility_TypeID = -1;

	private int UnityEngineUIScrollRectScrollbarVisibility_EnumRef = -1;

	private int UnityEngineUISliderDirection_TypeID = -1;

	private int UnityEngineUISliderDirection_EnumRef = -1;

	private int UnityEngineUIToggleToggleTransition_TypeID = -1;

	private int UnityEngineUIToggleToggleTransition_EnumRef = -1;

	private int UnityEngineUIGridLayoutGroupCorner_TypeID = -1;

	private int UnityEngineUIGridLayoutGroupCorner_EnumRef = -1;

	private int UnityEngineUIGridLayoutGroupAxis_TypeID = -1;

	private int UnityEngineUIGridLayoutGroupAxis_EnumRef = -1;

	private int UnityEngineUIGridLayoutGroupConstraint_TypeID = -1;

	private int UnityEngineUIGridLayoutGroupConstraint_EnumRef = -1;

	private int UnityEngineUIContentSizeFitterFitMode_TypeID = -1;

	private int UnityEngineUIContentSizeFitterFitMode_EnumRef = -1;

	private int SuperTextMeshAlignment_TypeID = -1;

	private int SuperTextMeshAlignment_EnumRef = -1;

	private int UnityEngineAnimatorCullingMode_TypeID = -1;

	private int UnityEngineAnimatorCullingMode_EnumRef = -1;

	private int UnityEngineTextAnchor_TypeID = -1;

	private int UnityEngineTextAnchor_EnumRef = -1;

	private int ScrollViewMovementType_TypeID = -1;

	private int ScrollViewMovementType_EnumRef = -1;

	private int ScrollViewScrollbarVisibility_TypeID = -1;

	private int ScrollViewScrollbarVisibility_EnumRef = -1;

	private int ScrollViewScrollViewLayoutType_TypeID = -1;

	private int ScrollViewScrollViewLayoutType_EnumRef = -1;

	private int UnityEngineTouchPhase_TypeID = -1;

	private int UnityEngineTouchPhase_EnumRef = -1;

	private int BitBenderGamesMobileTouchCameraState_TypeID = -1;

	private int BitBenderGamesMobileTouchCameraState_EnumRef = -1;

	private int GameFrameworkLocalizationLanguage_TypeID = -1;

	private int GameFrameworkLocalizationLanguage_EnumRef = -1;

	private int GameDefinesCityLabelColorType_TypeID = -1;

	private int GameDefinesCityLabelColorType_EnumRef = -1;

	private int GameDefinesBuildConnectRoadDirection_TypeID = -1;

	private int GameDefinesBuildConnectRoadDirection_EnumRef = -1;

	private int GameDefinesDirectionType_TypeID = -1;

	private int GameDefinesDirectionType_EnumRef = -1;

	private int ResourceManagerPreloadType_TypeID = -1;

	private int ResourceManagerPreloadType_EnumRef = -1;

	private int LODType_TypeID = -1;

	private int LODType_EnumRef = -1;

	private int SceneManagerSceneID_TypeID = -1;

	private int SceneManagerSceneID_EnumRef = -1;

	private int CityBuildingBuildSceneType_TypeID = -1;

	private int CityBuildingBuildSceneType_EnumRef = -1;

	private int ModelManagerModelObjectType_TypeID = -1;

	private int ModelManagerModelObjectType_EnumRef = -1;

	private int FakeModelManagerTempRoadType_TypeID = -1;

	private int FakeModelManagerTempRoadType_EnumRef = -1;

	private int WorldMarchDataManagerBattleWordType_TypeID = -1;

	private int WorldMarchDataManagerBattleWordType_EnumRef = -1;

	private int NewQueueState_TypeID = -1;

	private int NewQueueState_EnumRef = -1;

	private int ResourceType_TypeID = -1;

	private int ResourceType_EnumRef = -1;

	private int BuildingState_TypeID = -1;

	private int BuildingState_EnumRef = -1;

	private int PlaceBuildType_TypeID = -1;

	private int PlaceBuildType_EnumRef = -1;

	private int MarchStatus_TypeID = -1;

	private int MarchStatus_EnumRef = -1;

	private int UnityEngineTimelineClipCaps_TypeID = -1;

	private int UnityEngineTimelineClipCaps_EnumRef = -1;

	private int TMProTextAlignmentOptions_TypeID = -1;

	private int TMProTextAlignmentOptions_EnumRef = -1;

	private int TMProTMP_InputFieldContentType_TypeID = -1;

	private int TMProTMP_InputFieldContentType_EnumRef = -1;

	private int TMProTMP_InputFieldInputType_TypeID = -1;

	private int TMProTMP_InputFieldInputType_EnumRef = -1;

	private int TMProTMP_InputFieldCharacterValidation_TypeID = -1;

	private int TMProTMP_InputFieldCharacterValidation_EnumRef = -1;

	private int TMProTMP_InputFieldLineType_TypeID = -1;

	private int TMProTMP_InputFieldLineType_EnumRef = -1;

	private int TMProTMP_InputFieldExHorizontalAlignmentOptions_TypeID = -1;

	private int TMProTMP_InputFieldExHorizontalAlignmentOptions_EnumRef = -1;

	private int InstanceRequestState_TypeID = -1;

	private int InstanceRequestState_EnumRef = -1;

	private int NewMarchType_TypeID = -1;

	private int NewMarchType_EnumRef = -1;

	private int WorldPointType_TypeID = -1;

	private int WorldPointType_EnumRef = -1;

	private int SuperScrollViewListItemArrangeType_TypeID = -1;

	private int SuperScrollViewListItemArrangeType_EnumRef = -1;

	private int URLGroupType_TypeID = -1;

	private int URLGroupType_EnumRef = -1;

	private int MeteoriteWorldEffectPlayerFragmentDataFragmentType_TypeID = -1;

	private int MeteoriteWorldEffectPlayerFragmentDataFragmentType_EnumRef = -1;

	private int WorldMeteoritePointMeteoritePointState_TypeID = -1;

	private int WorldMeteoritePointMeteoritePointState_EnumRef = -1;

	private int BattleColliderUtilsColliderType_TypeID = -1;

	private int BattleColliderUtilsColliderType_EnumRef = -1;

	private int ViewSkinProPropertyRecorderCodeType_TypeID = -1;

	private int ViewSkinProPropertyRecorderCodeType_EnumRef = -1;

	private int FOWSystemLOSChecks_TypeID = -1;

	private int FOWSystemLOSChecks_EnumRef = -1;

	private int FOWSystemState_TypeID = -1;

	private int FOWSystemState_EnumRef = -1;

	private int ObjectPoolTag_TypeID = -1;

	private int ObjectPoolTag_EnumRef = -1;

	private int ObjectPoolTagGroup_TypeID = -1;

	private int ObjectPoolTagGroup_EnumRef = -1;

	private int DeviceLevel_TypeID = -1;

	private int DeviceLevel_EnumRef = -1;

	private int PlayerType_TypeID = -1;

	private int PlayerType_EnumRef = -1;

	private static XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua_Gen_Initer_Register__();

	private static IniterAdderFrameworkUtilsUnityExTouchInfo IniterAdderFrameworkUtilsUnityExTouchInfo_dumb_obj => s_IniterAdderFrameworkUtilsUnityExTouchInfo_dumb_obj;

	private static XLua_Gen_Initer_Register__ gen_reg_dumb_obj => s_gen_reg_dumb_obj;

	public void DelayWrapLoader(Type type, Action<IntPtr> loader)
	{
		delayWrap[type] = loader;
	}

	public void AddInterfaceBridgeCreator(Type type, Func<int, LuaEnv, LuaBase> creator)
	{
		interfaceBridgeCreators.Add(type, creator);
	}

	public bool TryDelayWrapLoader(IntPtr L, Type type)
	{
		if (loaded_types.ContainsKey(type))
		{
			return true;
		}
		loaded_types.Add(type, value: true);
		Lua.luaL_newmetatable(L, type.FullName);
		Lua.lua_pop(L, 1);
		int num = Lua.lua_gettop(L);
		if (delayWrap.TryGetValue(type, out var value))
		{
			delayWrap.Remove(type);
			value(L);
		}
		else
		{
			Utils.ReflectionWrap(L, type, privateAccessibleFlags.Contains(type));
		}
		if (num != Lua.lua_gettop(L))
		{
			throw new Exception("top change, before:" + num + ", after:" + Lua.lua_gettop(L));
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public);
		foreach (Type type2 in nestedTypes)
		{
			if (!type2.IsGenericTypeDefinition())
			{
				GetTypeId(L, type2);
			}
		}
		return true;
	}

	public void Alias(Type type, string alias)
	{
		Type type2 = FindType(alias);
		if (type2 == null)
		{
			throw new ArgumentException("Can not find " + alias);
		}
		aliasCfg[type2] = type;
	}

	private void addAssemblieByName(IEnumerable<Assembly> assemblies_usorted, string name)
	{
		foreach (Assembly item in assemblies_usorted)
		{
			if (item.FullName.StartsWith(name) && !assemblies.Contains(item))
			{
				assemblies.Add(item);
				break;
			}
		}
	}

	public ObjectTranslator(LuaEnv luaenv, IntPtr L)
	{
		assemblies = new List<Assembly>();
		assemblies.Add(Assembly.GetExecutingAssembly());
		Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
		addAssemblieByName(array, "mscorlib,");
		addAssemblieByName(array, "System,");
		addAssemblieByName(array, "System.Core,");
		Assembly[] array2 = array;
		foreach (Assembly item in array2)
		{
			if (!assemblies.Contains(item))
			{
				assemblies.Add(item);
			}
		}
		luaEnv = luaenv;
		objectCasters = new ObjectCasters(this);
		objectCheckers = new ObjectCheckers(this);
		methodWrapsCache = new MethodWrapsCache(this, objectCheckers, objectCasters);
		metaFunctions = new StaticLuaCallbacks();
		importTypeFunction = StaticLuaCallbacks.ImportType;
		loadAssemblyFunction = StaticLuaCallbacks.LoadAssembly;
		castFunction = StaticLuaCallbacks.Cast;
		Lua.lua_newtable(L);
		Lua.lua_newtable(L);
		Lua.xlua_pushasciistring(L, "__mode");
		Lua.xlua_pushasciistring(L, "v");
		Lua.lua_rawset(L, -3);
		Lua.lua_setmetatable(L, -2);
		cacheRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
		initCSharpCallLua();
	}

	private void initCSharpCallLua()
	{
	}

	private Func<DelegateBridgeBase, Delegate> getCreatorUsingGeneric(DelegateBridgeBase bridge, Type delegateType, MethodInfo delegateMethod)
	{
		Func<DelegateBridgeBase, Delegate> func = null;
		if (genericAction == null)
		{
			MethodInfo[] methods = typeof(DelegateBridge).GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			genericAction = (from m in methods
				where m.Name == "Action"
				orderby m.GetParameters().Length
				select m).ToArray();
			genericFunc = (from m in methods
				where m.Name == "Func"
				orderby m.GetParameters().Length
				select m).ToArray();
		}
		if (genericAction.Length != 5 || genericFunc.Length != 5)
		{
			return null;
		}
		ParameterInfo[] parameters = delegateMethod.GetParameters();
		if ((delegateMethod.ReturnType.IsValueType() && delegateMethod.ReturnType != typeof(void)) || parameters.Length > 4)
		{
			func = (DelegateBridgeBase x) => (Delegate)null;
		}
		else
		{
			ParameterInfo[] array = parameters;
			foreach (ParameterInfo parameterInfo in array)
			{
				if (parameterInfo.ParameterType.IsValueType() || parameterInfo.IsOut || parameterInfo.ParameterType.IsByRef)
				{
					func = (DelegateBridgeBase x) => (Delegate)null;
					break;
				}
			}
			if (func == null)
			{
				IEnumerable<Type> enumerable = parameters.Select((ParameterInfo pinfo) => pinfo.ParameterType);
				MethodInfo genericMethodInfo = null;
				if (delegateMethod.ReturnType == typeof(void))
				{
					genericMethodInfo = genericAction[parameters.Length];
				}
				else
				{
					genericMethodInfo = genericFunc[parameters.Length];
					enumerable = enumerable.Concat(new Type[1] { delegateMethod.ReturnType });
				}
				if (genericMethodInfo.IsGenericMethodDefinition)
				{
					MethodInfo methodInfo = genericMethodInfo.MakeGenericMethod(enumerable.ToArray());
					func = (DelegateBridgeBase o) => Delegate.CreateDelegate(delegateType, o, methodInfo);
				}
				else
				{
					func = (DelegateBridgeBase o) => Delegate.CreateDelegate(delegateType, o, genericMethodInfo);
				}
			}
		}
		return func;
	}

	private Delegate getDelegate(DelegateBridgeBase bridge, Type delegateType)
	{
		Delegate delegateByType = bridge.GetDelegateByType(delegateType);
		if ((object)delegateByType != null)
		{
			return delegateByType;
		}
		if (delegateType == typeof(Delegate) || delegateType == typeof(MulticastDelegate))
		{
			return null;
		}
		if (!delegateCreatorCache.TryGetValue(delegateType, out var value))
		{
			MethodInfo method = delegateType.GetMethod("Invoke");
			MethodInfo[] array = (from m in bridge.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
				where !m.IsGenericMethodDefinition && (m.Name.StartsWith("__Gen_Delegate_Imp") || m.Name == "Action")
				select m).ToArray();
			for (int num = 0; num < array.Length; num++)
			{
				if (!array[num].IsConstructor && Utils.IsParamsMatch(method, array[num]))
				{
					MethodInfo foundMethod = array[num];
					value = (DelegateBridgeBase o) => Delegate.CreateDelegate(delegateType, o, foundMethod);
					break;
				}
			}
			if (value == null)
			{
				value = getCreatorUsingGeneric(bridge, delegateType, method);
			}
			delegateCreatorCache.Add(delegateType, value);
		}
		delegateByType = value(bridge);
		if ((object)delegateByType != null)
		{
			return delegateByType;
		}
		throw new InvalidCastException("This type must add to CSharpCallLua: " + delegateType.GetFriendlyName());
	}

	public object CreateDelegateBridge(IntPtr L, Type delegateType, int idx)
	{
		int r;
		return _CreateDelegateBridge(L, delegateType, idx, out r);
	}

	private object _CreateDelegateBridge(IntPtr L, Type delegateType, int idx, out int r)
	{
		Lua.lua_pushvalue(L, idx);
		Lua.lua_rawget(L, LuaIndexes.LUA_REGISTRYINDEX);
		if (!Lua.lua_isnil(L, -1))
		{
			int num = Lua.xlua_tointeger(L, -1);
			Lua.lua_pop(L, 1);
			if (delegate_bridges[num].IsAlive)
			{
				r = num;
				if (delegateType == null)
				{
					return delegate_bridges[num].Target;
				}
				DelegateBridgeBase delegateBridgeBase = delegate_bridges[num].Target as DelegateBridgeBase;
				if (delegateBridgeBase.TryGetDelegate(delegateType, out var value))
				{
					return value;
				}
				value = getDelegate(delegateBridgeBase, delegateType);
				delegateBridgeBase.AddDelegate(delegateType, value);
				return value;
			}
		}
		else
		{
			Lua.lua_pop(L, 1);
		}
		Lua.lua_pushvalue(L, idx);
		int num2 = (r = Lua.luaL_ref(L));
		Lua.lua_pushvalue(L, idx);
		Lua.lua_pushnumber(L, num2);
		Lua.lua_rawset(L, LuaIndexes.LUA_REGISTRYINDEX);
		DelegateBridgeBase delegateBridgeBase2;
		try
		{
			delegateBridgeBase2 = new DelegateBridge(num2, luaEnv);
		}
		catch (Exception ex)
		{
			Lua.lua_pushvalue(L, idx);
			Lua.lua_pushnil(L);
			Lua.lua_rawset(L, LuaIndexes.LUA_REGISTRYINDEX);
			Lua.lua_pushnil(L);
			Lua.xlua_rawseti(L, LuaIndexes.LUA_REGISTRYINDEX, num2);
			throw ex;
		}
		if (delegateType == null)
		{
			delegate_bridges[num2] = new WeakReference(delegateBridgeBase2);
			return delegateBridgeBase2;
		}
		try
		{
			Delegate obj = getDelegate(delegateBridgeBase2, delegateType);
			delegateBridgeBase2.AddDelegate(delegateType, obj);
			delegate_bridges[num2] = new WeakReference(delegateBridgeBase2);
			return obj;
		}
		catch (Exception ex2)
		{
			delegateBridgeBase2.Dispose();
			throw ex2;
		}
	}

	public bool AllDelegateBridgeReleased()
	{
		foreach (KeyValuePair<int, WeakReference> delegate_bridge in delegate_bridges)
		{
			if (delegate_bridge.Value.IsAlive)
			{
				return false;
			}
		}
		return true;
	}

	public void ForceClearAllDelegateBridge()
	{
		List<DelegateBridgeBase> list = new List<DelegateBridgeBase>();
		foreach (KeyValuePair<int, WeakReference> delegate_bridge in delegate_bridges)
		{
			if (delegate_bridge.Value.IsAlive && delegate_bridge.Value.Target is DelegateBridgeBase delegateBridgeBase)
			{
				delegateBridgeBase.MarkDisposed();
				list.Add(delegateBridgeBase);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Dispose();
		}
		delegate_bridges?.Clear();
	}

	public int GetDelegateBridgeCount()
	{
		return delegate_bridges.Count;
	}

	public void ReleaseLuaBase(IntPtr L, int reference, bool is_delegate)
	{
		if (is_delegate)
		{
			Lua.xlua_rawgeti(L, LuaIndexes.LUA_REGISTRYINDEX, reference);
			if (Lua.lua_isnil(L, -1))
			{
				Lua.lua_pop(L, 1);
			}
			else
			{
				Lua.lua_pushvalue(L, -1);
				Lua.lua_rawget(L, LuaIndexes.LUA_REGISTRYINDEX);
				if (Lua.lua_type(L, -1) == LuaTypes.LUA_TNUMBER && Lua.xlua_tointeger(L, -1) == reference)
				{
					Lua.lua_pop(L, 1);
					Lua.lua_pushnil(L);
					Lua.lua_rawset(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				else
				{
					Lua.lua_pop(L, 2);
				}
			}
			Lua.lua_unref(L, reference);
			delegate_bridges.Remove(reference);
		}
		else
		{
			Lua.lua_unref(L, reference);
		}
	}

	public object CreateInterfaceBridge(IntPtr L, Type interfaceType, int idx)
	{
		if (!interfaceBridgeCreators.TryGetValue(interfaceType, out var value))
		{
			throw new InvalidCastException("This type must add to CSharpCallLua: " + interfaceType);
		}
		Lua.lua_pushvalue(L, idx);
		return value(Lua.luaL_ref(L), luaEnv);
	}

	public void CreateArrayMetatable(IntPtr L)
	{
		Utils.BeginObjectRegister(null, L, this, 0, 0, 1, 0, common_array_meta);
		Utils.RegisterFunc(L, -2, "Length", StaticLuaCallbacks.ArrayLength);
		Utils.EndObjectRegister(null, L, this, null, null, typeof(Array), StaticLuaCallbacks.ArrayIndexer, StaticLuaCallbacks.ArrayNewIndexer);
	}

	public void CreateDelegateMetatable(IntPtr L)
	{
		Utils.BeginObjectRegister(null, L, this, 3, 0, 0, 0, common_delegate_meta);
		Utils.RegisterFunc(L, -4, "__call", StaticLuaCallbacks.DelegateCall);
		Utils.RegisterFunc(L, -4, "__add", StaticLuaCallbacks.DelegateCombine);
		Utils.RegisterFunc(L, -4, "__sub", StaticLuaCallbacks.DelegateRemove);
		Utils.EndObjectRegister(null, L, this, null, null, typeof(MulticastDelegate), null, null);
	}

	internal void CreateEnumerablePairs(IntPtr L)
	{
		LuaFunction obj = luaEnv.DoString("\n                return function(obj)\n                    local isKeyValuePair\n                    local function lua_iter(cs_iter, k)\n                        if cs_iter:MoveNext() then\n                            local current = cs_iter.Current\n                            if isKeyValuePair == nil then\n                                if type(current) == 'userdata' then\n                                    local t = current:GetType()\n                                    isKeyValuePair = t.Name == 'KeyValuePair`2' and t.Namespace == 'System.Collections.Generic'\n                                 else\n                                    isKeyValuePair = false\n                                 end\n                                 --print(current, isKeyValuePair)\n                            end\n                            if isKeyValuePair then\n                                return current.Key, current.Value\n                            else\n                                return k + 1, current\n                            end\n                        end\n                    end\n                    return lua_iter, obj:GetEnumerator(), -1\n                end\n            ")[0] as LuaFunction;
		obj.push(L);
		enumerable_pairs_func = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
		obj.Dispose();
	}

	public void OpenLib(IntPtr L)
	{
		if (Lua.xlua_getglobal(L, "xlua") != 0)
		{
			throw new Exception("call xlua_getglobal fail!" + Lua.lua_tostring(L, -1));
		}
		Lua.xlua_pushasciistring(L, "import_type");
		Lua.lua_pushstdcallcfunction(L, importTypeFunction);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "import_generic_type");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.ImportGenericType);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "cast");
		Lua.lua_pushstdcallcfunction(L, castFunction);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "load_assembly");
		Lua.lua_pushstdcallcfunction(L, loadAssemblyFunction);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "access");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.XLuaAccess);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "private_accessible");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.XLuaPrivateAccessible);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "metatable_operation");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.XLuaMetatableOperation);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "tofunction");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.ToFunction);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "get_generic_method");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.GetGenericMethod);
		Lua.lua_rawset(L, -3);
		Lua.xlua_pushasciistring(L, "release");
		Lua.lua_pushstdcallcfunction(L, StaticLuaCallbacks.ReleaseCsObject);
		Lua.lua_rawset(L, -3);
		Lua.lua_pop(L, 1);
		Lua.lua_createtable(L, 1, 4);
		common_array_meta = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.lua_createtable(L, 1, 4);
		common_delegate_meta = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
	}

	internal void createFunctionMetatable(IntPtr L)
	{
		Lua.lua_newtable(L);
		Lua.xlua_pushasciistring(L, "__gc");
		Lua.lua_pushstdcallcfunction(L, metaFunctions.GcMeta);
		Lua.lua_rawset(L, -3);
		Lua.lua_pushlightuserdata(L, Lua.xlua_tag());
		Lua.lua_pushnumber(L, 1.0);
		Lua.lua_rawset(L, -3);
		Lua.lua_pushvalue(L, -1);
		int num = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
		Lua.lua_pushnumber(L, num);
		Lua.xlua_rawseti(L, -2, 1L);
		Lua.lua_pop(L, 1);
		typeIdMap.Add(typeof(lua_CSFunction), num);
	}

	internal Type FindType(string className, bool isQualifiedName = false)
	{
		foreach (Assembly assembly in assemblies)
		{
			Type type = assembly.GetType(className);
			if (type != null)
			{
				return type;
			}
		}
		int num = className.IndexOf('[');
		if (num > 0 && !isQualifiedName)
		{
			string text = className.Substring(0, num + 1);
			string[] array = className.Substring(num + 1, className.Length - text.Length - 1).Split(new char[1] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				Type type2 = FindType(array[i].Trim());
				if (type2 == null)
				{
					return null;
				}
				if (i != 0)
				{
					text += ", ";
				}
				text = text + "[" + type2.AssemblyQualifiedName + "]";
			}
			text += "]";
			return FindType(text, isQualifiedName: true);
		}
		return null;
	}

	private bool hasMethod(Type type, string methodName)
	{
		MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		for (int i = 0; i < methods.Length; i++)
		{
			if (methods[i].Name == methodName)
			{
				return true;
			}
		}
		return false;
	}

	internal void collectObject(int obj_index_to_collect)
	{
		if (!objects.TryGetValue(obj_index_to_collect, out var obj))
		{
			return;
		}
		objects.Remove(obj_index_to_collect);
		if (obj == null)
		{
			return;
		}
		bool flag = obj.GetType().IsEnum();
		if ((flag ? enumMap.TryGetValue(obj, out var value) : reverseMap.TryGetValue(obj, out value)) && value == obj_index_to_collect)
		{
			if (flag)
			{
				enumMap.Remove(obj);
			}
			else
			{
				reverseMap.Remove(obj);
			}
		}
	}

	private int addObject(object obj, bool is_valuetype, bool is_enum)
	{
		int num = objects.Add(obj);
		if (is_enum)
		{
			enumMap[obj] = num;
		}
		else if (!is_valuetype)
		{
			reverseMap[obj] = num;
		}
		return num;
	}

	internal object GetObject(IntPtr L, int index)
	{
		return objectCasters.GetCaster(typeof(object))(L, index, null);
	}

	public Type GetTypeOf(IntPtr L, int idx)
	{
		Type value = null;
		int num = Lua.xlua_gettypeid(L, idx);
		if (num != -1)
		{
			typeMap.TryGetValue(num, out value);
		}
		return value;
	}

	public bool Assignable<T>(IntPtr L, int index)
	{
		return Assignable(L, index, typeof(T));
	}

	public bool Assignable(IntPtr L, int index, Type type)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			int num = Lua.xlua_tocsobj_safe(L, index);
			if (num != -1 && objects.TryGetValue(num, out var obj))
			{
				if (obj is RawObject rawObject)
				{
					obj = rawObject.Target;
				}
				if (obj == null)
				{
					return !type.IsValueType();
				}
				return type.IsAssignableFrom(obj.GetType());
			}
			int num2 = Lua.xlua_gettypeid(L, index);
			if (num2 != -1 && typeMap.TryGetValue(num2, out var value))
			{
				return type.IsAssignableFrom(value);
			}
		}
		return objectCheckers.GetChecker(type)(L, index);
	}

	public object GetObject(IntPtr L, int index, Type type)
	{
		int num = Lua.xlua_tocsobj_safe(L, index);
		if (num != -1)
		{
			object obj = objects.Get(num);
			if (obj is RawObject rawObject)
			{
				return rawObject.Target;
			}
			return obj;
		}
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			int num2 = Lua.xlua_gettypeid(L, index);
			if (num2 != -1 && num2 == decimal_type_id)
			{
				Get(L, index, out decimal val);
				return val;
			}
			if (num2 != -1 && typeMap.TryGetValue(num2, out var value) && type.IsAssignableFrom(value) && custom_get_funcs.TryGetValue(type, out var value2))
			{
				return value2(L, index);
			}
		}
		return objectCasters.GetCaster(type)(L, index, null);
	}

	public void Get<T>(IntPtr L, int index, out T v)
	{
		if (tryGetGetFuncByType<Func<IntPtr, int, T>>(typeof(T), out var func))
		{
			v = func(L, index);
		}
		else
		{
			v = (T)GetObject(L, index, typeof(T));
		}
	}

	public void PushByType<T>(IntPtr L, T v)
	{
		if (tryGetPushFuncByType<Action<IntPtr, T>>(typeof(T), out var func))
		{
			func(L, v);
		}
		else
		{
			PushAny(L, v);
		}
	}

	public T[] GetParams<T>(IntPtr L, int index)
	{
		T[] array = new T[Math.Max(Lua.lua_gettop(L) - index + 1, 0)];
		for (int i = 0; i < array.Length; i++)
		{
			Get(L, index + i, out array[i]);
		}
		return array;
	}

	public Array GetParams(IntPtr L, int index, Type type)
	{
		Array array = Array.CreateInstance(type, Math.Max(Lua.lua_gettop(L) - index + 1, 0));
		for (int i = 0; i < array.Length; i++)
		{
			array.SetValue(GetObject(L, index + i, type), i);
		}
		return array;
	}

	public T GetDelegate<T>(IntPtr L, int index) where T : class
	{
		if (Lua.lua_isfunction(L, index))
		{
			return CreateDelegateBridge(L, typeof(T), index) as T;
		}
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			return (T)SafeGetCSObj(L, index);
		}
		return null;
	}

	public int GetTypeId(IntPtr L, Type type)
	{
		bool is_first;
		return getTypeId(L, type, out is_first);
	}

	public void PrivateAccessible(IntPtr L, Type type)
	{
		if (!privateAccessibleFlags.Contains(type))
		{
			privateAccessibleFlags.Add(type);
			if (typeIdMap.ContainsKey(type))
			{
				Utils.MakePrivateAccessible(L, type);
			}
		}
	}

	internal int getTypeId(IntPtr L, Type type, out bool is_first, LOGLEVEL log_level = LOGLEVEL.WARN)
	{
		is_first = false;
		if (!typeIdMap.TryGetValue(type, out var value))
		{
			if (type.IsArray)
			{
				if (common_array_meta == -1)
				{
					throw new Exception("Fatal Exception! Array Metatable not inited!");
				}
				return common_array_meta;
			}
			if (typeof(MulticastDelegate).IsAssignableFrom(type))
			{
				if (common_delegate_meta == -1)
				{
					throw new Exception("Fatal Exception! Delegate Metatable not inited!");
				}
				TryDelayWrapLoader(L, type);
				return common_delegate_meta;
			}
			is_first = true;
			Type value2 = null;
			aliasCfg.TryGetValue(type, out value2);
			Lua.luaL_getmetatable(L, (value2 == null) ? type.FullName : value2.FullName);
			if (Lua.lua_isnil(L, -1))
			{
				Lua.lua_pop(L, 1);
				if (!TryDelayWrapLoader(L, (value2 == null) ? type : value2))
				{
					throw new Exception("Fatal: can not load metatable of type:" + type);
				}
				Lua.luaL_getmetatable(L, (value2 == null) ? type.FullName : value2.FullName);
			}
			if (typeIdMap.TryGetValue(type, out value))
			{
				Lua.lua_pop(L, 1);
			}
			else
			{
				if (type.IsEnum())
				{
					Lua.xlua_pushasciistring(L, "__band");
					Lua.lua_pushstdcallcfunction(L, metaFunctions.EnumAndMeta);
					Lua.lua_rawset(L, -3);
					Lua.xlua_pushasciistring(L, "__bor");
					Lua.lua_pushstdcallcfunction(L, metaFunctions.EnumOrMeta);
					Lua.lua_rawset(L, -3);
				}
				if (typeof(IEnumerable).IsAssignableFrom(type))
				{
					Lua.xlua_pushasciistring(L, "__pairs");
					Lua.lua_getref(L, enumerable_pairs_func);
					Lua.lua_rawset(L, -3);
				}
				Lua.lua_pushvalue(L, -1);
				value = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				Lua.lua_pushnumber(L, value);
				Lua.xlua_rawseti(L, -2, 1L);
				Lua.lua_pop(L, 1);
				if (type.IsValueType())
				{
					typeMap.Add(value, type);
				}
				typeIdMap.Add(type, value);
			}
		}
		return value;
	}

	private void pushPrimitive(IntPtr L, object o)
	{
		if (o is sbyte || o is byte || o is short || o is ushort || o is int)
		{
			int value = Convert.ToInt32(o);
			Lua.xlua_pushinteger(L, value);
			return;
		}
		if (o is uint)
		{
			Lua.xlua_pushuint(L, (uint)o);
			return;
		}
		if (o is float || o is double)
		{
			double number = Convert.ToDouble(o);
			Lua.lua_pushnumber(L, number);
			return;
		}
		if (o is IntPtr)
		{
			Lua.lua_pushlightuserdata(L, (IntPtr)o);
			return;
		}
		if (o is char)
		{
			Lua.xlua_pushinteger(L, (char)o);
			return;
		}
		if (o is long)
		{
			Lua.lua_pushint64(L, Convert.ToInt64(o));
			return;
		}
		if (o is ulong)
		{
			Lua.lua_pushuint64(L, Convert.ToUInt64(o));
			return;
		}
		if (o is bool value2)
		{
			Lua.lua_pushboolean(L, value2);
			return;
		}
		throw new Exception("No support type " + o.GetType());
	}

	public void PushAny(IntPtr L, object o)
	{
		if (o == null)
		{
			Lua.lua_pushnil(L);
			return;
		}
		Type type = o.GetType();
		if (type.IsPrimitive())
		{
			pushPrimitive(L, o);
		}
		else if (o is string)
		{
			Lua.lua_pushstring(L, o as string);
		}
		else if (type == typeof(byte[]))
		{
			Lua.lua_pushstring(L, o as byte[]);
		}
		else if (o is decimal)
		{
			PushDecimal(L, (decimal)o);
		}
		else if (o is LuaBase)
		{
			((LuaBase)o).push(L);
		}
		else if (o is lua_CSFunction)
		{
			Push(L, o as lua_CSFunction);
		}
		else if (o is ValueType)
		{
			if (custom_push_funcs.TryGetValue(o.GetType(), out var value))
			{
				value(L, o);
			}
			else
			{
				Push(L, o);
			}
		}
		else
		{
			Push(L, o);
		}
	}

	public int TranslateToEnumToTop(IntPtr L, Type type, int idx)
	{
		object obj = null;
		switch (Lua.lua_type(L, idx))
		{
		case LuaTypes.LUA_TNUMBER:
		{
			int value2 = (int)Lua.lua_tonumber(L, idx);
			obj = Enum.ToObject(type, value2);
			break;
		}
		case LuaTypes.LUA_TSTRING:
		{
			string value = Lua.lua_tostring(L, idx);
			obj = Enum.Parse(type, value);
			break;
		}
		default:
			return Lua.luaL_error(L, "#1 argument must be a integer or a string");
		}
		PushAny(L, obj);
		return 1;
	}

	public void Push(IntPtr L, lua_CSFunction o)
	{
		if (Utils.IsStaticPInvokeCSFunction(o))
		{
			Lua.lua_pushstdcallcfunction(L, o);
			return;
		}
		Push(L, (object)o);
		Lua.lua_pushstdcallcfunction(L, metaFunctions.StaticCSFunctionWraper, 1);
	}

	public void Push(IntPtr L, LuaBase o)
	{
		if (o == null)
		{
			Lua.lua_pushnil(L);
		}
		else
		{
			o.push(L);
		}
	}

	public void Push(IntPtr L, object o)
	{
		if (o == null)
		{
			Lua.lua_pushnil(L);
			return;
		}
		int value = -1;
		Type type = o.GetType();
		bool isEnum = type.IsEnum;
		bool isValueType = type.IsValueType;
		bool flag = !isValueType || isEnum;
		if (!flag || !(isEnum ? enumMap.TryGetValue(o, out value) : reverseMap.TryGetValue(o, out value)) || Lua.xlua_tryget_cachedud(L, value, cacheRef) != 1)
		{
			bool is_first;
			int typeId = getTypeId(L, type, out is_first);
			if (!(is_first && flag) || !(isEnum ? enumMap.TryGetValue(o, out value) : reverseMap.TryGetValue(o, out value)) || Lua.xlua_tryget_cachedud(L, value, cacheRef) != 1)
			{
				value = addObject(o, isValueType, isEnum);
				Lua.xlua_pushcsobj(L, value, typeId, flag, cacheRef);
			}
		}
	}

	public void PushObject(IntPtr L, object o, int type_id)
	{
		if (o == null)
		{
			Lua.lua_pushnil(L);
			return;
		}
		int value = -1;
		if (!reverseMap.TryGetValue(o, out value) || Lua.xlua_tryget_cachedud(L, value, cacheRef) != 1)
		{
			value = addObject(o, is_valuetype: false, is_enum: false);
			Lua.xlua_pushcsobj(L, value, type_id, need_cache: true, cacheRef);
		}
	}

	public void Update(IntPtr L, int index, object obj)
	{
		int num = Lua.xlua_tocsobj_fast(L, index);
		if (num != -1)
		{
			objects.Replace(num, obj);
			return;
		}
		if (custom_update_funcs.TryGetValue(obj.GetType(), out var value))
		{
			value(L, index, obj);
			return;
		}
		throw new Exception(string.Concat("can not update [", obj, "]"));
	}

	private object getCsObj(IntPtr L, int index, int udata)
	{
		if (udata == -1)
		{
			if (Lua.lua_type(L, index) != LuaTypes.LUA_TUSERDATA)
			{
				return null;
			}
			Type typeOf = GetTypeOf(L, index);
			if (typeOf == typeof(decimal))
			{
				Get(L, index, out decimal val);
				return val;
			}
			if (typeOf != null && custom_get_funcs.TryGetValue(typeOf, out var value))
			{
				return value(L, index);
			}
			return null;
		}
		if (objects.TryGetValue(udata, out var obj))
		{
			return obj;
		}
		return null;
	}

	internal object SafeGetCSObj(IntPtr L, int index)
	{
		return getCsObj(L, index, Lua.xlua_tocsobj_safe(L, index));
	}

	internal object FastGetCSObj(IntPtr L, int index)
	{
		return getCsObj(L, index, Lua.xlua_tocsobj_fast(L, index));
	}

	internal void ReleaseCSObj(IntPtr L, int index)
	{
		int num = Lua.xlua_tocsobj_safe(L, index);
		if (num != -1)
		{
			object obj = objects.Replace(num, null);
			if (obj != null && reverseMap.ContainsKey(obj))
			{
				reverseMap.Remove(obj);
			}
		}
	}

	internal lua_CSFunction GetFixCSFunction(int index)
	{
		return fix_cs_functions[index];
	}

	internal void PushFixCSFunction(IntPtr L, lua_CSFunction func)
	{
		if (func == null)
		{
			Lua.lua_pushnil(L);
			return;
		}
		Lua.xlua_pushinteger(L, fix_cs_functions.Count);
		fix_cs_functions.Add(func);
		Lua.lua_pushstdcallcfunction(L, metaFunctions.FixCSFunctionWraper, 1);
	}

	internal object[] popValues(IntPtr L, int oldTop)
	{
		int num = Lua.lua_gettop(L);
		if (oldTop == num)
		{
			return null;
		}
		ArrayList arrayList = new ArrayList();
		for (int i = oldTop + 1; i <= num; i++)
		{
			arrayList.Add(GetObject(L, i));
		}
		Lua.lua_settop(L, oldTop);
		return arrayList.ToArray();
	}

	internal object[] popValues(IntPtr L, int oldTop, Type[] popTypes)
	{
		int num = Lua.lua_gettop(L);
		if (oldTop == num)
		{
			return null;
		}
		ArrayList arrayList = new ArrayList();
		int num2 = ((popTypes[0] == typeof(void)) ? 1 : 0);
		for (int i = oldTop + 1; i <= num; i++)
		{
			arrayList.Add(GetObject(L, i, popTypes[num2]));
			num2++;
		}
		Lua.lua_settop(L, oldTop);
		return arrayList.ToArray();
	}

	private void registerCustomOp(Type type, PushCSObject push, GetCSObject get, UpdateCSObject update)
	{
		if (push != null)
		{
			custom_push_funcs.Add(type, push);
		}
		if (get != null)
		{
			custom_get_funcs.Add(type, get);
		}
		if (update != null)
		{
			custom_update_funcs.Add(type, update);
		}
	}

	public bool HasCustomOp(Type type)
	{
		return custom_push_funcs.ContainsKey(type);
	}

	private bool tryGetPushFuncByType<T>(Type type, out T func) where T : class
	{
		if (push_func_with_type == null)
		{
			push_func_with_type = new Dictionary<Type, Delegate>
			{
				{
					typeof(int),
					new Action<IntPtr, int>(Lua.xlua_pushinteger)
				},
				{
					typeof(double),
					new Action<IntPtr, double>(Lua.lua_pushnumber)
				},
				{
					typeof(string),
					new Action<IntPtr, string>(Lua.lua_pushstring)
				},
				{
					typeof(byte[]),
					new Action<IntPtr, byte[]>(Lua.lua_pushstring)
				},
				{
					typeof(bool),
					new Action<IntPtr, bool>(Lua.lua_pushboolean)
				},
				{
					typeof(long),
					new Action<IntPtr, long>(Lua.lua_pushint64)
				},
				{
					typeof(ulong),
					new Action<IntPtr, ulong>(Lua.lua_pushuint64)
				},
				{
					typeof(IntPtr),
					new Action<IntPtr, IntPtr>(Lua.lua_pushlightuserdata)
				},
				{
					typeof(decimal),
					new Action<IntPtr, decimal>(PushDecimal)
				},
				{
					typeof(byte),
					(Action<IntPtr, byte>)delegate(IntPtr L, byte v)
					{
						Lua.xlua_pushinteger(L, v);
					}
				},
				{
					typeof(sbyte),
					(Action<IntPtr, sbyte>)delegate(IntPtr L, sbyte v)
					{
						Lua.xlua_pushinteger(L, v);
					}
				},
				{
					typeof(char),
					(Action<IntPtr, char>)delegate(IntPtr L, char v)
					{
						Lua.xlua_pushinteger(L, v);
					}
				},
				{
					typeof(short),
					(Action<IntPtr, short>)delegate(IntPtr L, short v)
					{
						Lua.xlua_pushinteger(L, v);
					}
				},
				{
					typeof(ushort),
					(Action<IntPtr, ushort>)delegate(IntPtr L, ushort v)
					{
						Lua.xlua_pushinteger(L, v);
					}
				},
				{
					typeof(uint),
					new Action<IntPtr, uint>(Lua.xlua_pushuint)
				},
				{
					typeof(float),
					(Action<IntPtr, float>)delegate(IntPtr L, float v)
					{
						Lua.lua_pushnumber(L, v);
					}
				}
			};
		}
		if (push_func_with_type.TryGetValue(type, out var value))
		{
			func = value as T;
			return true;
		}
		func = null;
		return false;
	}

	private bool tryGetGetFuncByType<T>(Type type, out T func) where T : class
	{
		if (get_func_with_type == null)
		{
			get_func_with_type = new Dictionary<Type, Delegate>
			{
				{
					typeof(int),
					new Func<IntPtr, int, int>(Lua.xlua_tointeger)
				},
				{
					typeof(double),
					new Func<IntPtr, int, double>(Lua.lua_tonumber)
				},
				{
					typeof(string),
					new Func<IntPtr, int, string>(Lua.lua_tostring)
				},
				{
					typeof(byte[]),
					new Func<IntPtr, int, byte[]>(Lua.lua_tobytes)
				},
				{
					typeof(bool),
					new Func<IntPtr, int, bool>(Lua.lua_toboolean)
				},
				{
					typeof(long),
					new Func<IntPtr, int, long>(Lua.lua_toint64)
				},
				{
					typeof(ulong),
					new Func<IntPtr, int, ulong>(Lua.lua_touint64)
				},
				{
					typeof(IntPtr),
					new Func<IntPtr, int, IntPtr>(Lua.lua_touserdata)
				},
				{
					typeof(decimal),
					(Func<IntPtr, int, decimal>)delegate(IntPtr L, int idx)
					{
						Get(L, idx, out decimal val);
						return val;
					}
				},
				{
					typeof(byte),
					(Func<IntPtr, int, byte>)((IntPtr L, int idx) => (byte)Lua.xlua_tointeger(L, idx))
				},
				{
					typeof(sbyte),
					(Func<IntPtr, int, sbyte>)((IntPtr L, int idx) => (sbyte)Lua.xlua_tointeger(L, idx))
				},
				{
					typeof(char),
					(Func<IntPtr, int, char>)((IntPtr L, int idx) => (char)Lua.xlua_tointeger(L, idx))
				},
				{
					typeof(short),
					(Func<IntPtr, int, short>)((IntPtr L, int idx) => (short)Lua.xlua_tointeger(L, idx))
				},
				{
					typeof(ushort),
					(Func<IntPtr, int, ushort>)((IntPtr L, int idx) => (ushort)Lua.xlua_tointeger(L, idx))
				},
				{
					typeof(uint),
					new Func<IntPtr, int, uint>(Lua.xlua_touint)
				},
				{
					typeof(float),
					(Func<IntPtr, int, float>)((IntPtr L, int idx) => (float)Lua.lua_tonumber(L, idx))
				}
			};
		}
		if (get_func_with_type.TryGetValue(type, out var value))
		{
			func = value as T;
			return true;
		}
		func = null;
		return false;
	}

	public void RegisterPushAndGetAndUpdate<T>(Action<IntPtr, T> push, GetFunc<T> get, Action<IntPtr, int, T> update)
	{
		Type typeFromHandle = typeof(T);
		if (tryGetPushFuncByType<Action<IntPtr, T>>(typeFromHandle, out var _) || tryGetGetFuncByType<Func<IntPtr, int, T>>(typeFromHandle, out var _))
		{
			throw new InvalidOperationException(string.Concat("push or get of ", typeFromHandle, " has register!"));
		}
		push_func_with_type.Add(typeFromHandle, push);
		get_func_with_type.Add(typeFromHandle, (Func<IntPtr, int, T>)delegate(IntPtr L, int idx)
		{
			get(L, idx, out var val);
			return val;
		});
		registerCustomOp(typeFromHandle, delegate(IntPtr L, object obj)
		{
			push(L, (T)obj);
		}, delegate(IntPtr L, int idx)
		{
			get(L, idx, out var val);
			return val;
		}, delegate(IntPtr L, int idx, object obj)
		{
			update(L, idx, (T)obj);
		});
	}

	public void RegisterCaster<T>(GetFunc<T> get)
	{
		objectCasters.AddCaster(typeof(T), delegate(IntPtr L, int idx, object o)
		{
			get(L, idx, out var val);
			return val;
		});
	}

	public void PushDecimal(IntPtr L, decimal val)
	{
		if (decimal_type_id == -1)
		{
			decimal_type_id = getTypeId(L, typeof(decimal), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 16u, decimal_type_id), 0, val))
		{
			throw new Exception("pack fail for decimal ,value=" + val);
		}
	}

	public bool IsDecimal(IntPtr L, int index)
	{
		if (decimal_type_id == -1)
		{
			return false;
		}
		return Lua.xlua_gettypeid(L, index) == decimal_type_id;
	}

	public decimal GetDecimal(IntPtr L, int index)
	{
		Get(L, index, out decimal val);
		return val;
	}

	public void Get(IntPtr L, int index, out decimal val)
	{
		LuaTypes luaTypes = Lua.lua_type(L, index);
		switch (luaTypes)
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != decimal_type_id)
			{
				throw new Exception("invalid userdata for decimal!");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack decimal fail!");
			}
			break;
		case LuaTypes.LUA_TNUMBER:
			if (Lua.lua_isint64(L, index))
			{
				val = Lua.lua_toint64(L, index);
			}
			else
			{
				val = (decimal)Lua.lua_tonumber(L, index);
			}
			break;
		default:
			throw new Exception("invalid lua value for decimal, LuaType=" + luaTypes);
		}
	}

	public bool AddPushByteFunc<T>(Type t, Action<IntPtr, T> func)
	{
		if (push_func_with_type != null)
		{
			push_func_with_type[t] = func;
			return true;
		}
		return false;
	}

	public bool AddCustomFunc(Type t, PushCSObject func)
	{
		if (custom_push_funcs != null)
		{
			custom_push_funcs[t] = func;
			return true;
		}
		return false;
	}

	public void PushByType(IntPtr L, int v)
	{
		Lua.xlua_pushinteger(L, v);
	}

	public void PushByType(IntPtr L, double v)
	{
		Lua.lua_pushnumber(L, v);
	}

	public void PushByType(IntPtr L, string v)
	{
		Lua.lua_pushstring(L, v);
	}

	public void PushByType(IntPtr L, byte[] v)
	{
		Lua.lua_pushstring(L, v);
	}

	public void PushByType(IntPtr L, bool v)
	{
		Lua.lua_pushboolean(L, v);
	}

	public void PushByType(IntPtr L, long v)
	{
		Lua.lua_pushint64(L, v);
	}

	public void PushByType(IntPtr L, ulong v)
	{
		Lua.lua_pushuint64(L, v);
	}

	public void PushByType(IntPtr L, uint v)
	{
		Lua.xlua_pushuint(L, v);
	}

	public void PushByType(IntPtr L, float v)
	{
		Lua.lua_pushnumber(L, v);
	}

	public void PushByType(IntPtr L, XLuaManager.ByteString v)
	{
		Lua.xlua_pushlstring(L, v.str, v.str_len);
	}

	public void Get(IntPtr L, int index, out int v)
	{
		v = Lua.xlua_tointeger(L, index);
	}

	public void Get(IntPtr L, int index, out double v)
	{
		v = Lua.lua_tonumber(L, index);
	}

	public void Get(IntPtr L, int index, out string v)
	{
		v = Lua.lua_tostring(L, index);
	}

	public void Get(IntPtr L, int index, out byte[] v)
	{
		v = Lua.lua_tobytes(L, index);
	}

	public void Get(IntPtr L, int index, out bool v)
	{
		v = Lua.lua_toboolean(L, index);
	}

	public void Get(IntPtr L, int index, out long v)
	{
		v = Lua.lua_toint64(L, index);
	}

	public void Get(IntPtr L, int index, out ulong v)
	{
		v = Lua.lua_touint64(L, index);
	}

	public void Get(IntPtr L, int index, out uint v)
	{
		v = Lua.xlua_touint(L, index);
	}

	public void Get(IntPtr L, int index, out float v)
	{
		v = (float)Lua.lua_tonumber(L, index);
	}

	public void PushFrameworkUtilsUnityExTouchInfo(IntPtr L, TouchInfo val)
	{
		if (FrameworkUtilsUnityExTouchInfo_TypeID == -1)
		{
			FrameworkUtilsUnityExTouchInfo_TypeID = getTypeId(L, typeof(TouchInfo), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 20u, FrameworkUtilsUnityExTouchInfo_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for Framework.Utils.UnityEx.TouchInfo ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out TouchInfo val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != FrameworkUtilsUnityExTouchInfo_TypeID)
			{
				throw new Exception("invalid userdata for Framework.Utils.UnityEx.TouchInfo");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for Framework.Utils.UnityEx.TouchInfo");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (TouchInfo)objectCasters.GetCaster(typeof(TouchInfo))(L, index, null);
			break;
		}
	}

	public void UpdateFrameworkUtilsUnityExTouchInfo(IntPtr L, int index, TouchInfo val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FrameworkUtilsUnityExTouchInfo_TypeID)
			{
				throw new Exception("invalid userdata for Framework.Utils.UnityEx.TouchInfo");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for Framework.Utils.UnityEx.TouchInfo ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineVector2(IntPtr L, Vector2 val)
	{
		if (UnityEngineVector2_TypeID == -1)
		{
			UnityEngineVector2_TypeID = getTypeId(L, typeof(Vector2), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 8u, UnityEngineVector2_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Vector2 ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Vector2 val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Vector2");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Vector2");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Vector2)objectCasters.GetCaster(typeof(Vector2))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineVector2(IntPtr L, int index, Vector2 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Vector2");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Vector2 ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineVector3(IntPtr L, Vector3 val)
	{
		if (UnityEngineVector3_TypeID == -1)
		{
			UnityEngineVector3_TypeID = getTypeId(L, typeof(Vector3), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 12u, UnityEngineVector3_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Vector3 ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Vector3 val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Vector3");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Vector3");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Vector3)objectCasters.GetCaster(typeof(Vector3))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineVector3(IntPtr L, int index, Vector3 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Vector3");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Vector3 ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineVector4(IntPtr L, Vector4 val)
	{
		if (UnityEngineVector4_TypeID == -1)
		{
			UnityEngineVector4_TypeID = getTypeId(L, typeof(Vector4), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 16u, UnityEngineVector4_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Vector4 ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Vector4 val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Vector4");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Vector4");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Vector4)objectCasters.GetCaster(typeof(Vector4))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineVector4(IntPtr L, int index, Vector4 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Vector4");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Vector4 ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineColor(IntPtr L, Color val)
	{
		if (UnityEngineColor_TypeID == -1)
		{
			UnityEngineColor_TypeID = getTypeId(L, typeof(Color), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 16u, UnityEngineColor_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Color ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Color val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Color");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Color");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Color)objectCasters.GetCaster(typeof(Color))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineColor(IntPtr L, int index, Color val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Color");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Color ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineQuaternion(IntPtr L, Quaternion val)
	{
		if (UnityEngineQuaternion_TypeID == -1)
		{
			UnityEngineQuaternion_TypeID = getTypeId(L, typeof(Quaternion), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 16u, UnityEngineQuaternion_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Quaternion ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Quaternion val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Quaternion");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Quaternion");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Quaternion)objectCasters.GetCaster(typeof(Quaternion))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineQuaternion(IntPtr L, int index, Quaternion val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Quaternion");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Quaternion ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineRay(IntPtr L, Ray val)
	{
		if (UnityEngineRay_TypeID == -1)
		{
			UnityEngineRay_TypeID = getTypeId(L, typeof(Ray), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 24u, UnityEngineRay_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Ray ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Ray val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Ray");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Ray");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Ray)objectCasters.GetCaster(typeof(Ray))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineRay(IntPtr L, int index, Ray val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Ray");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Ray ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineBounds(IntPtr L, Bounds val)
	{
		if (UnityEngineBounds_TypeID == -1)
		{
			UnityEngineBounds_TypeID = getTypeId(L, typeof(Bounds), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 24u, UnityEngineBounds_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Bounds ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Bounds val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Bounds");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Bounds");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Bounds)objectCasters.GetCaster(typeof(Bounds))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineBounds(IntPtr L, int index, Bounds val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Bounds");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Bounds ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineRay2D(IntPtr L, Ray2D val)
	{
		if (UnityEngineRay2D_TypeID == -1)
		{
			UnityEngineRay2D_TypeID = getTypeId(L, typeof(Ray2D), out var _);
		}
		if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 16u, UnityEngineRay2D_TypeID), 0, val))
		{
			throw new Exception("pack fail fail for UnityEngine.Ray2D ,value=" + val);
		}
	}

	public void Get(IntPtr L, int index, out Ray2D val)
	{
		switch (Lua.lua_type(L, index))
		{
		case LuaTypes.LUA_TUSERDATA:
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Ray2D");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out val))
			{
				throw new Exception("unpack fail for UnityEngine.Ray2D");
			}
			break;
		case LuaTypes.LUA_TTABLE:
			CopyByValue.UnPack(this, L, index, out val);
			break;
		default:
			val = (Ray2D)objectCasters.GetCaster(typeof(Ray2D))(L, index, null);
			break;
		}
	}

	public void UpdateUnityEngineRay2D(IntPtr L, int index, Ray2D val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Ray2D");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, val))
			{
				throw new Exception("pack fail for UnityEngine.Ray2D ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushSystemReflectionBindingFlags(IntPtr L, BindingFlags val)
	{
		if (SystemReflectionBindingFlags_TypeID == -1)
		{
			SystemReflectionBindingFlags_TypeID = getTypeId(L, typeof(BindingFlags), out var _);
			if (SystemReflectionBindingFlags_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(BindingFlags));
				SystemReflectionBindingFlags_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, SystemReflectionBindingFlags_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, SystemReflectionBindingFlags_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for System.Reflection.BindingFlags ,value=" + val);
			}
			Lua.lua_getref(L, SystemReflectionBindingFlags_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out BindingFlags val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SystemReflectionBindingFlags_TypeID)
			{
				throw new Exception("invalid userdata for System.Reflection.BindingFlags");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for System.Reflection.BindingFlags");
			}
			val = (BindingFlags)field;
		}
		else
		{
			val = (BindingFlags)objectCasters.GetCaster(typeof(BindingFlags))(L, index, null);
		}
	}

	public void UpdateSystemReflectionBindingFlags(IntPtr L, int index, BindingFlags val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SystemReflectionBindingFlags_TypeID)
			{
				throw new Exception("invalid userdata for System.Reflection.BindingFlags");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for System.Reflection.BindingFlags ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineKeyCode(IntPtr L, KeyCode val)
	{
		if (UnityEngineKeyCode_TypeID == -1)
		{
			UnityEngineKeyCode_TypeID = getTypeId(L, typeof(KeyCode), out var _);
			if (UnityEngineKeyCode_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(KeyCode));
				UnityEngineKeyCode_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineKeyCode_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineKeyCode_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.KeyCode ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineKeyCode_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out KeyCode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineKeyCode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.KeyCode");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.KeyCode");
			}
			val = (KeyCode)field;
		}
		else
		{
			val = (KeyCode)objectCasters.GetCaster(typeof(KeyCode))(L, index, null);
		}
	}

	public void UpdateUnityEngineKeyCode(IntPtr L, int index, KeyCode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineKeyCode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.KeyCode");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.KeyCode ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineCameraGateFitMode(IntPtr L, Camera.GateFitMode val)
	{
		if (UnityEngineCameraGateFitMode_TypeID == -1)
		{
			UnityEngineCameraGateFitMode_TypeID = getTypeId(L, typeof(Camera.GateFitMode), out var _);
			if (UnityEngineCameraGateFitMode_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Camera.GateFitMode));
				UnityEngineCameraGateFitMode_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraGateFitMode_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineCameraGateFitMode_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Camera.GateFitMode ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineCameraGateFitMode_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Camera.GateFitMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraGateFitMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.GateFitMode");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Camera.GateFitMode");
			}
			val = (Camera.GateFitMode)field;
		}
		else
		{
			val = (Camera.GateFitMode)objectCasters.GetCaster(typeof(Camera.GateFitMode))(L, index, null);
		}
	}

	public void UpdateUnityEngineCameraGateFitMode(IntPtr L, int index, Camera.GateFitMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraGateFitMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.GateFitMode");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Camera.GateFitMode ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineCameraFieldOfViewAxis(IntPtr L, Camera.FieldOfViewAxis val)
	{
		if (UnityEngineCameraFieldOfViewAxis_TypeID == -1)
		{
			UnityEngineCameraFieldOfViewAxis_TypeID = getTypeId(L, typeof(Camera.FieldOfViewAxis), out var _);
			if (UnityEngineCameraFieldOfViewAxis_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Camera.FieldOfViewAxis));
				UnityEngineCameraFieldOfViewAxis_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraFieldOfViewAxis_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineCameraFieldOfViewAxis_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Camera.FieldOfViewAxis ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineCameraFieldOfViewAxis_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Camera.FieldOfViewAxis val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraFieldOfViewAxis_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.FieldOfViewAxis");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Camera.FieldOfViewAxis");
			}
			val = (Camera.FieldOfViewAxis)field;
		}
		else
		{
			val = (Camera.FieldOfViewAxis)objectCasters.GetCaster(typeof(Camera.FieldOfViewAxis))(L, index, null);
		}
	}

	public void UpdateUnityEngineCameraFieldOfViewAxis(IntPtr L, int index, Camera.FieldOfViewAxis val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraFieldOfViewAxis_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.FieldOfViewAxis");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Camera.FieldOfViewAxis ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineCameraStereoscopicEye(IntPtr L, Camera.StereoscopicEye val)
	{
		if (UnityEngineCameraStereoscopicEye_TypeID == -1)
		{
			UnityEngineCameraStereoscopicEye_TypeID = getTypeId(L, typeof(Camera.StereoscopicEye), out var _);
			if (UnityEngineCameraStereoscopicEye_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Camera.StereoscopicEye));
				UnityEngineCameraStereoscopicEye_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraStereoscopicEye_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineCameraStereoscopicEye_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Camera.StereoscopicEye ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineCameraStereoscopicEye_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Camera.StereoscopicEye val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraStereoscopicEye_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.StereoscopicEye");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Camera.StereoscopicEye");
			}
			val = (Camera.StereoscopicEye)field;
		}
		else
		{
			val = (Camera.StereoscopicEye)objectCasters.GetCaster(typeof(Camera.StereoscopicEye))(L, index, null);
		}
	}

	public void UpdateUnityEngineCameraStereoscopicEye(IntPtr L, int index, Camera.StereoscopicEye val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraStereoscopicEye_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.StereoscopicEye");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Camera.StereoscopicEye ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineCameraMonoOrStereoscopicEye(IntPtr L, Camera.MonoOrStereoscopicEye val)
	{
		if (UnityEngineCameraMonoOrStereoscopicEye_TypeID == -1)
		{
			UnityEngineCameraMonoOrStereoscopicEye_TypeID = getTypeId(L, typeof(Camera.MonoOrStereoscopicEye), out var _);
			if (UnityEngineCameraMonoOrStereoscopicEye_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Camera.MonoOrStereoscopicEye));
				UnityEngineCameraMonoOrStereoscopicEye_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineCameraMonoOrStereoscopicEye_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineCameraMonoOrStereoscopicEye_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Camera.MonoOrStereoscopicEye ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineCameraMonoOrStereoscopicEye_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Camera.MonoOrStereoscopicEye val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraMonoOrStereoscopicEye_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.MonoOrStereoscopicEye");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Camera.MonoOrStereoscopicEye");
			}
			val = (Camera.MonoOrStereoscopicEye)field;
		}
		else
		{
			val = (Camera.MonoOrStereoscopicEye)objectCasters.GetCaster(typeof(Camera.MonoOrStereoscopicEye))(L, index, null);
		}
	}

	public void UpdateUnityEngineCameraMonoOrStereoscopicEye(IntPtr L, int index, Camera.MonoOrStereoscopicEye val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineCameraMonoOrStereoscopicEye_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Camera.MonoOrStereoscopicEye");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Camera.MonoOrStereoscopicEye ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushDGTweeningEase(IntPtr L, Ease val)
	{
		if (DGTweeningEase_TypeID == -1)
		{
			DGTweeningEase_TypeID = getTypeId(L, typeof(Ease), out var _);
			if (DGTweeningEase_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Ease));
				DGTweeningEase_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, DGTweeningEase_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, DGTweeningEase_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for DG.Tweening.Ease ,value=" + val);
			}
			Lua.lua_getref(L, DGTweeningEase_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Ease val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DGTweeningEase_TypeID)
			{
				throw new Exception("invalid userdata for DG.Tweening.Ease");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for DG.Tweening.Ease");
			}
			val = (Ease)field;
		}
		else
		{
			val = (Ease)objectCasters.GetCaster(typeof(Ease))(L, index, null);
		}
	}

	public void UpdateDGTweeningEase(IntPtr L, int index, Ease val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DGTweeningEase_TypeID)
			{
				throw new Exception("invalid userdata for DG.Tweening.Ease");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for DG.Tweening.Ease ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushDGTweeningDOTweenAnimationAnimationType(IntPtr L, DOTweenAnimation.AnimationType val)
	{
		if (DGTweeningDOTweenAnimationAnimationType_TypeID == -1)
		{
			DGTweeningDOTweenAnimationAnimationType_TypeID = getTypeId(L, typeof(DOTweenAnimation.AnimationType), out var _);
			if (DGTweeningDOTweenAnimationAnimationType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(DOTweenAnimation.AnimationType));
				DGTweeningDOTweenAnimationAnimationType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, DGTweeningDOTweenAnimationAnimationType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, DGTweeningDOTweenAnimationAnimationType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for DG.Tweening.DOTweenAnimation.AnimationType ,value=" + val);
			}
			Lua.lua_getref(L, DGTweeningDOTweenAnimationAnimationType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out DOTweenAnimation.AnimationType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DGTweeningDOTweenAnimationAnimationType_TypeID)
			{
				throw new Exception("invalid userdata for DG.Tweening.DOTweenAnimation.AnimationType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for DG.Tweening.DOTweenAnimation.AnimationType");
			}
			val = (DOTweenAnimation.AnimationType)field;
		}
		else
		{
			val = (DOTweenAnimation.AnimationType)objectCasters.GetCaster(typeof(DOTweenAnimation.AnimationType))(L, index, null);
		}
	}

	public void UpdateDGTweeningDOTweenAnimationAnimationType(IntPtr L, int index, DOTweenAnimation.AnimationType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DGTweeningDOTweenAnimationAnimationType_TypeID)
			{
				throw new Exception("invalid userdata for DG.Tweening.DOTweenAnimation.AnimationType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for DG.Tweening.DOTweenAnimation.AnimationType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushDGTweeningDOTweenAnimationTargetType(IntPtr L, DOTweenAnimation.TargetType val)
	{
		if (DGTweeningDOTweenAnimationTargetType_TypeID == -1)
		{
			DGTweeningDOTweenAnimationTargetType_TypeID = getTypeId(L, typeof(DOTweenAnimation.TargetType), out var _);
			if (DGTweeningDOTweenAnimationTargetType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(DOTweenAnimation.TargetType));
				DGTweeningDOTweenAnimationTargetType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, DGTweeningDOTweenAnimationTargetType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, DGTweeningDOTweenAnimationTargetType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for DG.Tweening.DOTweenAnimation.TargetType ,value=" + val);
			}
			Lua.lua_getref(L, DGTweeningDOTweenAnimationTargetType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out DOTweenAnimation.TargetType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DGTweeningDOTweenAnimationTargetType_TypeID)
			{
				throw new Exception("invalid userdata for DG.Tweening.DOTweenAnimation.TargetType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for DG.Tweening.DOTweenAnimation.TargetType");
			}
			val = (DOTweenAnimation.TargetType)field;
		}
		else
		{
			val = (DOTweenAnimation.TargetType)objectCasters.GetCaster(typeof(DOTweenAnimation.TargetType))(L, index, null);
		}
	}

	public void UpdateDGTweeningDOTweenAnimationTargetType(IntPtr L, int index, DOTweenAnimation.TargetType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DGTweeningDOTweenAnimationTargetType_TypeID)
			{
				throw new Exception("invalid userdata for DG.Tweening.DOTweenAnimation.TargetType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for DG.Tweening.DOTweenAnimation.TargetType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTextMeshProUGUIExHorizontalAlignmentOptions(IntPtr L, TextMeshProUGUIEx.HorizontalAlignmentOptions val)
	{
		if (TextMeshProUGUIExHorizontalAlignmentOptions_TypeID == -1)
		{
			TextMeshProUGUIExHorizontalAlignmentOptions_TypeID = getTypeId(L, typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions), out var _);
			if (TextMeshProUGUIExHorizontalAlignmentOptions_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions));
				TextMeshProUGUIExHorizontalAlignmentOptions_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TextMeshProUGUIExHorizontalAlignmentOptions_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TextMeshProUGUIExHorizontalAlignmentOptions_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TextMeshProUGUIEx.HorizontalAlignmentOptions ,value=" + val);
			}
			Lua.lua_getref(L, TextMeshProUGUIExHorizontalAlignmentOptions_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TextMeshProUGUIEx.HorizontalAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TextMeshProUGUIExHorizontalAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TextMeshProUGUIEx.HorizontalAlignmentOptions");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TextMeshProUGUIEx.HorizontalAlignmentOptions");
			}
			val = (TextMeshProUGUIEx.HorizontalAlignmentOptions)field;
		}
		else
		{
			val = (TextMeshProUGUIEx.HorizontalAlignmentOptions)objectCasters.GetCaster(typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions))(L, index, null);
		}
	}

	public void UpdateTextMeshProUGUIExHorizontalAlignmentOptions(IntPtr L, int index, TextMeshProUGUIEx.HorizontalAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TextMeshProUGUIExHorizontalAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TextMeshProUGUIEx.HorizontalAlignmentOptions");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TextMeshProUGUIEx.HorizontalAlignmentOptions ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTextMeshProUGUIExVerticalAlignmentOptions(IntPtr L, TextMeshProUGUIEx.VerticalAlignmentOptions val)
	{
		if (TextMeshProUGUIExVerticalAlignmentOptions_TypeID == -1)
		{
			TextMeshProUGUIExVerticalAlignmentOptions_TypeID = getTypeId(L, typeof(TextMeshProUGUIEx.VerticalAlignmentOptions), out var _);
			if (TextMeshProUGUIExVerticalAlignmentOptions_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TextMeshProUGUIEx.VerticalAlignmentOptions));
				TextMeshProUGUIExVerticalAlignmentOptions_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TextMeshProUGUIExVerticalAlignmentOptions_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TextMeshProUGUIExVerticalAlignmentOptions_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TextMeshProUGUIEx.VerticalAlignmentOptions ,value=" + val);
			}
			Lua.lua_getref(L, TextMeshProUGUIExVerticalAlignmentOptions_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TextMeshProUGUIEx.VerticalAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TextMeshProUGUIExVerticalAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TextMeshProUGUIEx.VerticalAlignmentOptions");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TextMeshProUGUIEx.VerticalAlignmentOptions");
			}
			val = (TextMeshProUGUIEx.VerticalAlignmentOptions)field;
		}
		else
		{
			val = (TextMeshProUGUIEx.VerticalAlignmentOptions)objectCasters.GetCaster(typeof(TextMeshProUGUIEx.VerticalAlignmentOptions))(L, index, null);
		}
	}

	public void UpdateTextMeshProUGUIExVerticalAlignmentOptions(IntPtr L, int index, TextMeshProUGUIEx.VerticalAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TextMeshProUGUIExVerticalAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TextMeshProUGUIEx.VerticalAlignmentOptions");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TextMeshProUGUIEx.VerticalAlignmentOptions ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineRectTransformEdge(IntPtr L, RectTransform.Edge val)
	{
		if (UnityEngineRectTransformEdge_TypeID == -1)
		{
			UnityEngineRectTransformEdge_TypeID = getTypeId(L, typeof(RectTransform.Edge), out var _);
			if (UnityEngineRectTransformEdge_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(RectTransform.Edge));
				UnityEngineRectTransformEdge_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineRectTransformEdge_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineRectTransformEdge_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.RectTransform.Edge ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineRectTransformEdge_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out RectTransform.Edge val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRectTransformEdge_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.RectTransform.Edge");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.RectTransform.Edge");
			}
			val = (RectTransform.Edge)field;
		}
		else
		{
			val = (RectTransform.Edge)objectCasters.GetCaster(typeof(RectTransform.Edge))(L, index, null);
		}
	}

	public void UpdateUnityEngineRectTransformEdge(IntPtr L, int index, RectTransform.Edge val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRectTransformEdge_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.RectTransform.Edge");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.RectTransform.Edge ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineRectTransformAxis(IntPtr L, RectTransform.Axis val)
	{
		if (UnityEngineRectTransformAxis_TypeID == -1)
		{
			UnityEngineRectTransformAxis_TypeID = getTypeId(L, typeof(RectTransform.Axis), out var _);
			if (UnityEngineRectTransformAxis_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(RectTransform.Axis));
				UnityEngineRectTransformAxis_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineRectTransformAxis_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineRectTransformAxis_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.RectTransform.Axis ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineRectTransformAxis_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out RectTransform.Axis val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRectTransformAxis_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.RectTransform.Axis");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.RectTransform.Axis");
			}
			val = (RectTransform.Axis)field;
		}
		else
		{
			val = (RectTransform.Axis)objectCasters.GetCaster(typeof(RectTransform.Axis))(L, index, null);
		}
	}

	public void UpdateUnityEngineRectTransformAxis(IntPtr L, int index, RectTransform.Axis val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRectTransformAxis_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.RectTransform.Axis");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.RectTransform.Axis ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineEventSystemsPointerEventDataInputButton(IntPtr L, PointerEventData.InputButton val)
	{
		if (UnityEngineEventSystemsPointerEventDataInputButton_TypeID == -1)
		{
			UnityEngineEventSystemsPointerEventDataInputButton_TypeID = getTypeId(L, typeof(PointerEventData.InputButton), out var _);
			if (UnityEngineEventSystemsPointerEventDataInputButton_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(PointerEventData.InputButton));
				UnityEngineEventSystemsPointerEventDataInputButton_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineEventSystemsPointerEventDataInputButton_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineEventSystemsPointerEventDataInputButton_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.EventSystems.PointerEventData.InputButton ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineEventSystemsPointerEventDataInputButton_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out PointerEventData.InputButton val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineEventSystemsPointerEventDataInputButton_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.EventSystems.PointerEventData.InputButton");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.EventSystems.PointerEventData.InputButton");
			}
			val = (PointerEventData.InputButton)field;
		}
		else
		{
			val = (PointerEventData.InputButton)objectCasters.GetCaster(typeof(PointerEventData.InputButton))(L, index, null);
		}
	}

	public void UpdateUnityEngineEventSystemsPointerEventDataInputButton(IntPtr L, int index, PointerEventData.InputButton val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineEventSystemsPointerEventDataInputButton_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.EventSystems.PointerEventData.InputButton");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.EventSystems.PointerEventData.InputButton ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineEventSystemsPointerEventDataFramePressState(IntPtr L, PointerEventData.FramePressState val)
	{
		if (UnityEngineEventSystemsPointerEventDataFramePressState_TypeID == -1)
		{
			UnityEngineEventSystemsPointerEventDataFramePressState_TypeID = getTypeId(L, typeof(PointerEventData.FramePressState), out var _);
			if (UnityEngineEventSystemsPointerEventDataFramePressState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(PointerEventData.FramePressState));
				UnityEngineEventSystemsPointerEventDataFramePressState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineEventSystemsPointerEventDataFramePressState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineEventSystemsPointerEventDataFramePressState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.EventSystems.PointerEventData.FramePressState ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineEventSystemsPointerEventDataFramePressState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out PointerEventData.FramePressState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineEventSystemsPointerEventDataFramePressState_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.EventSystems.PointerEventData.FramePressState");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.EventSystems.PointerEventData.FramePressState");
			}
			val = (PointerEventData.FramePressState)field;
		}
		else
		{
			val = (PointerEventData.FramePressState)objectCasters.GetCaster(typeof(PointerEventData.FramePressState))(L, index, null);
		}
	}

	public void UpdateUnityEngineEventSystemsPointerEventDataFramePressState(IntPtr L, int index, PointerEventData.FramePressState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineEventSystemsPointerEventDataFramePressState_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.EventSystems.PointerEventData.FramePressState");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.EventSystems.PointerEventData.FramePressState ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineRenderTextureFormat(IntPtr L, RenderTextureFormat val)
	{
		if (UnityEngineRenderTextureFormat_TypeID == -1)
		{
			UnityEngineRenderTextureFormat_TypeID = getTypeId(L, typeof(RenderTextureFormat), out var _);
			if (UnityEngineRenderTextureFormat_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(RenderTextureFormat));
				UnityEngineRenderTextureFormat_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineRenderTextureFormat_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineRenderTextureFormat_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.RenderTextureFormat ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineRenderTextureFormat_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out RenderTextureFormat val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRenderTextureFormat_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.RenderTextureFormat");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.RenderTextureFormat");
			}
			val = (RenderTextureFormat)field;
		}
		else
		{
			val = (RenderTextureFormat)objectCasters.GetCaster(typeof(RenderTextureFormat))(L, index, null);
		}
	}

	public void UpdateUnityEngineRenderTextureFormat(IntPtr L, int index, RenderTextureFormat val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRenderTextureFormat_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.RenderTextureFormat");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.RenderTextureFormat ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineSpace(IntPtr L, Space val)
	{
		if (UnityEngineSpace_TypeID == -1)
		{
			UnityEngineSpace_TypeID = getTypeId(L, typeof(Space), out var _);
			if (UnityEngineSpace_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Space));
				UnityEngineSpace_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineSpace_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineSpace_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Space ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineSpace_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Space val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineSpace_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Space");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Space");
			}
			val = (Space)field;
		}
		else
		{
			val = (Space)objectCasters.GetCaster(typeof(Space))(L, index, null);
		}
	}

	public void UpdateUnityEngineSpace(IntPtr L, int index, Space val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineSpace_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Space");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Space ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineQueryTriggerInteraction(IntPtr L, QueryTriggerInteraction val)
	{
		if (UnityEngineQueryTriggerInteraction_TypeID == -1)
		{
			UnityEngineQueryTriggerInteraction_TypeID = getTypeId(L, typeof(QueryTriggerInteraction), out var _);
			if (UnityEngineQueryTriggerInteraction_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(QueryTriggerInteraction));
				UnityEngineQueryTriggerInteraction_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineQueryTriggerInteraction_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineQueryTriggerInteraction_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.QueryTriggerInteraction ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineQueryTriggerInteraction_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out QueryTriggerInteraction val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineQueryTriggerInteraction_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.QueryTriggerInteraction");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.QueryTriggerInteraction");
			}
			val = (QueryTriggerInteraction)field;
		}
		else
		{
			val = (QueryTriggerInteraction)objectCasters.GetCaster(typeof(QueryTriggerInteraction))(L, index, null);
		}
	}

	public void UpdateUnityEngineQueryTriggerInteraction(IntPtr L, int index, QueryTriggerInteraction val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineQueryTriggerInteraction_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.QueryTriggerInteraction");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.QueryTriggerInteraction ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineRenderingUniversalAntialiasingMode(IntPtr L, AntialiasingMode val)
	{
		if (UnityEngineRenderingUniversalAntialiasingMode_TypeID == -1)
		{
			UnityEngineRenderingUniversalAntialiasingMode_TypeID = getTypeId(L, typeof(AntialiasingMode), out var _);
			if (UnityEngineRenderingUniversalAntialiasingMode_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(AntialiasingMode));
				UnityEngineRenderingUniversalAntialiasingMode_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineRenderingUniversalAntialiasingMode_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineRenderingUniversalAntialiasingMode_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Rendering.Universal.AntialiasingMode ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineRenderingUniversalAntialiasingMode_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out AntialiasingMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRenderingUniversalAntialiasingMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Rendering.Universal.AntialiasingMode");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Rendering.Universal.AntialiasingMode");
			}
			val = (AntialiasingMode)field;
		}
		else
		{
			val = (AntialiasingMode)objectCasters.GetCaster(typeof(AntialiasingMode))(L, index, null);
		}
	}

	public void UpdateUnityEngineRenderingUniversalAntialiasingMode(IntPtr L, int index, AntialiasingMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineRenderingUniversalAntialiasingMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Rendering.Universal.AntialiasingMode");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Rendering.Universal.AntialiasingMode ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUISelectableTransition(IntPtr L, Selectable.Transition val)
	{
		if (UnityEngineUISelectableTransition_TypeID == -1)
		{
			UnityEngineUISelectableTransition_TypeID = getTypeId(L, typeof(Selectable.Transition), out var _);
			if (UnityEngineUISelectableTransition_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Selectable.Transition));
				UnityEngineUISelectableTransition_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUISelectableTransition_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUISelectableTransition_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Selectable.Transition ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUISelectableTransition_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Selectable.Transition val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUISelectableTransition_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Selectable.Transition");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Selectable.Transition");
			}
			val = (Selectable.Transition)field;
		}
		else
		{
			val = (Selectable.Transition)objectCasters.GetCaster(typeof(Selectable.Transition))(L, index, null);
		}
	}

	public void UpdateUnityEngineUISelectableTransition(IntPtr L, int index, Selectable.Transition val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUISelectableTransition_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Selectable.Transition");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Selectable.Transition ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIInputFieldContentType(IntPtr L, InputField.ContentType val)
	{
		if (UnityEngineUIInputFieldContentType_TypeID == -1)
		{
			UnityEngineUIInputFieldContentType_TypeID = getTypeId(L, typeof(InputField.ContentType), out var _);
			if (UnityEngineUIInputFieldContentType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(InputField.ContentType));
				UnityEngineUIInputFieldContentType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldContentType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIInputFieldContentType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.InputField.ContentType ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIInputFieldContentType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out InputField.ContentType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldContentType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.ContentType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.InputField.ContentType");
			}
			val = (InputField.ContentType)field;
		}
		else
		{
			val = (InputField.ContentType)objectCasters.GetCaster(typeof(InputField.ContentType))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIInputFieldContentType(IntPtr L, int index, InputField.ContentType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldContentType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.ContentType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.InputField.ContentType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIInputFieldInputType(IntPtr L, InputField.InputType val)
	{
		if (UnityEngineUIInputFieldInputType_TypeID == -1)
		{
			UnityEngineUIInputFieldInputType_TypeID = getTypeId(L, typeof(InputField.InputType), out var _);
			if (UnityEngineUIInputFieldInputType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(InputField.InputType));
				UnityEngineUIInputFieldInputType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldInputType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIInputFieldInputType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.InputField.InputType ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIInputFieldInputType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out InputField.InputType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldInputType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.InputType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.InputField.InputType");
			}
			val = (InputField.InputType)field;
		}
		else
		{
			val = (InputField.InputType)objectCasters.GetCaster(typeof(InputField.InputType))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIInputFieldInputType(IntPtr L, int index, InputField.InputType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldInputType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.InputType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.InputField.InputType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIInputFieldCharacterValidation(IntPtr L, InputField.CharacterValidation val)
	{
		if (UnityEngineUIInputFieldCharacterValidation_TypeID == -1)
		{
			UnityEngineUIInputFieldCharacterValidation_TypeID = getTypeId(L, typeof(InputField.CharacterValidation), out var _);
			if (UnityEngineUIInputFieldCharacterValidation_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(InputField.CharacterValidation));
				UnityEngineUIInputFieldCharacterValidation_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldCharacterValidation_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIInputFieldCharacterValidation_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.InputField.CharacterValidation ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIInputFieldCharacterValidation_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out InputField.CharacterValidation val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldCharacterValidation_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.CharacterValidation");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.InputField.CharacterValidation");
			}
			val = (InputField.CharacterValidation)field;
		}
		else
		{
			val = (InputField.CharacterValidation)objectCasters.GetCaster(typeof(InputField.CharacterValidation))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIInputFieldCharacterValidation(IntPtr L, int index, InputField.CharacterValidation val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldCharacterValidation_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.CharacterValidation");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.InputField.CharacterValidation ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIInputFieldLineType(IntPtr L, InputField.LineType val)
	{
		if (UnityEngineUIInputFieldLineType_TypeID == -1)
		{
			UnityEngineUIInputFieldLineType_TypeID = getTypeId(L, typeof(InputField.LineType), out var _);
			if (UnityEngineUIInputFieldLineType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(InputField.LineType));
				UnityEngineUIInputFieldLineType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIInputFieldLineType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIInputFieldLineType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.InputField.LineType ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIInputFieldLineType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out InputField.LineType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldLineType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.LineType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.InputField.LineType");
			}
			val = (InputField.LineType)field;
		}
		else
		{
			val = (InputField.LineType)objectCasters.GetCaster(typeof(InputField.LineType))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIInputFieldLineType(IntPtr L, int index, InputField.LineType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIInputFieldLineType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.InputField.LineType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.InputField.LineType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageType(IntPtr L, Image.Type val)
	{
		if (UnityEngineUIImageType_TypeID == -1)
		{
			UnityEngineUIImageType_TypeID = getTypeId(L, typeof(Image.Type), out var _);
			if (UnityEngineUIImageType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.Type));
				UnityEngineUIImageType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.Type ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.Type val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Type");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.Type");
			}
			val = (Image.Type)field;
		}
		else
		{
			val = (Image.Type)objectCasters.GetCaster(typeof(Image.Type))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageType(IntPtr L, int index, Image.Type val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Type");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.Type ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageFillMethod(IntPtr L, Image.FillMethod val)
	{
		if (UnityEngineUIImageFillMethod_TypeID == -1)
		{
			UnityEngineUIImageFillMethod_TypeID = getTypeId(L, typeof(Image.FillMethod), out var _);
			if (UnityEngineUIImageFillMethod_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.FillMethod));
				UnityEngineUIImageFillMethod_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageFillMethod_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageFillMethod_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.FillMethod ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageFillMethod_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.FillMethod val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageFillMethod_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.FillMethod");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.FillMethod");
			}
			val = (Image.FillMethod)field;
		}
		else
		{
			val = (Image.FillMethod)objectCasters.GetCaster(typeof(Image.FillMethod))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageFillMethod(IntPtr L, int index, Image.FillMethod val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageFillMethod_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.FillMethod");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.FillMethod ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageOriginHorizontal(IntPtr L, Image.OriginHorizontal val)
	{
		if (UnityEngineUIImageOriginHorizontal_TypeID == -1)
		{
			UnityEngineUIImageOriginHorizontal_TypeID = getTypeId(L, typeof(Image.OriginHorizontal), out var _);
			if (UnityEngineUIImageOriginHorizontal_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.OriginHorizontal));
				UnityEngineUIImageOriginHorizontal_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOriginHorizontal_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageOriginHorizontal_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.OriginHorizontal ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageOriginHorizontal_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.OriginHorizontal val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOriginHorizontal_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginHorizontal");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.OriginHorizontal");
			}
			val = (Image.OriginHorizontal)field;
		}
		else
		{
			val = (Image.OriginHorizontal)objectCasters.GetCaster(typeof(Image.OriginHorizontal))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageOriginHorizontal(IntPtr L, int index, Image.OriginHorizontal val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOriginHorizontal_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginHorizontal");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.OriginHorizontal ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageOriginVertical(IntPtr L, Image.OriginVertical val)
	{
		if (UnityEngineUIImageOriginVertical_TypeID == -1)
		{
			UnityEngineUIImageOriginVertical_TypeID = getTypeId(L, typeof(Image.OriginVertical), out var _);
			if (UnityEngineUIImageOriginVertical_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.OriginVertical));
				UnityEngineUIImageOriginVertical_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOriginVertical_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageOriginVertical_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.OriginVertical ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageOriginVertical_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.OriginVertical val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOriginVertical_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginVertical");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.OriginVertical");
			}
			val = (Image.OriginVertical)field;
		}
		else
		{
			val = (Image.OriginVertical)objectCasters.GetCaster(typeof(Image.OriginVertical))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageOriginVertical(IntPtr L, int index, Image.OriginVertical val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOriginVertical_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.OriginVertical");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.OriginVertical ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageOrigin90(IntPtr L, Image.Origin90 val)
	{
		if (UnityEngineUIImageOrigin90_TypeID == -1)
		{
			UnityEngineUIImageOrigin90_TypeID = getTypeId(L, typeof(Image.Origin90), out var _);
			if (UnityEngineUIImageOrigin90_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.Origin90));
				UnityEngineUIImageOrigin90_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOrigin90_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageOrigin90_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.Origin90 ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageOrigin90_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.Origin90 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin90_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin90");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.Origin90");
			}
			val = (Image.Origin90)field;
		}
		else
		{
			val = (Image.Origin90)objectCasters.GetCaster(typeof(Image.Origin90))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageOrigin90(IntPtr L, int index, Image.Origin90 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin90_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin90");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.Origin90 ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageOrigin180(IntPtr L, Image.Origin180 val)
	{
		if (UnityEngineUIImageOrigin180_TypeID == -1)
		{
			UnityEngineUIImageOrigin180_TypeID = getTypeId(L, typeof(Image.Origin180), out var _);
			if (UnityEngineUIImageOrigin180_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.Origin180));
				UnityEngineUIImageOrigin180_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOrigin180_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageOrigin180_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.Origin180 ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageOrigin180_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.Origin180 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin180_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin180");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.Origin180");
			}
			val = (Image.Origin180)field;
		}
		else
		{
			val = (Image.Origin180)objectCasters.GetCaster(typeof(Image.Origin180))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageOrigin180(IntPtr L, int index, Image.Origin180 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin180_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin180");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.Origin180 ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIImageOrigin360(IntPtr L, Image.Origin360 val)
	{
		if (UnityEngineUIImageOrigin360_TypeID == -1)
		{
			UnityEngineUIImageOrigin360_TypeID = getTypeId(L, typeof(Image.Origin360), out var _);
			if (UnityEngineUIImageOrigin360_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Image.Origin360));
				UnityEngineUIImageOrigin360_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIImageOrigin360_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIImageOrigin360_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Image.Origin360 ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIImageOrigin360_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Image.Origin360 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin360_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin360");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Image.Origin360");
			}
			val = (Image.Origin360)field;
		}
		else
		{
			val = (Image.Origin360)objectCasters.GetCaster(typeof(Image.Origin360))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIImageOrigin360(IntPtr L, int index, Image.Origin360 val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIImageOrigin360_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Image.Origin360");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Image.Origin360 ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIScrollRectMovementType(IntPtr L, ScrollRect.MovementType val)
	{
		if (UnityEngineUIScrollRectMovementType_TypeID == -1)
		{
			UnityEngineUIScrollRectMovementType_TypeID = getTypeId(L, typeof(ScrollRect.MovementType), out var _);
			if (UnityEngineUIScrollRectMovementType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ScrollRect.MovementType));
				UnityEngineUIScrollRectMovementType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIScrollRectMovementType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIScrollRectMovementType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.ScrollRect.MovementType ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIScrollRectMovementType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ScrollRect.MovementType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIScrollRectMovementType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.MovementType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.ScrollRect.MovementType");
			}
			val = (ScrollRect.MovementType)field;
		}
		else
		{
			val = (ScrollRect.MovementType)objectCasters.GetCaster(typeof(ScrollRect.MovementType))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIScrollRectMovementType(IntPtr L, int index, ScrollRect.MovementType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIScrollRectMovementType_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.MovementType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.ScrollRect.MovementType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIScrollRectScrollbarVisibility(IntPtr L, ScrollRect.ScrollbarVisibility val)
	{
		if (UnityEngineUIScrollRectScrollbarVisibility_TypeID == -1)
		{
			UnityEngineUIScrollRectScrollbarVisibility_TypeID = getTypeId(L, typeof(ScrollRect.ScrollbarVisibility), out var _);
			if (UnityEngineUIScrollRectScrollbarVisibility_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ScrollRect.ScrollbarVisibility));
				UnityEngineUIScrollRectScrollbarVisibility_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIScrollRectScrollbarVisibility_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIScrollRectScrollbarVisibility_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.ScrollRect.ScrollbarVisibility ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIScrollRectScrollbarVisibility_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ScrollRect.ScrollbarVisibility val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIScrollRectScrollbarVisibility_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.ScrollbarVisibility");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.ScrollRect.ScrollbarVisibility");
			}
			val = (ScrollRect.ScrollbarVisibility)field;
		}
		else
		{
			val = (ScrollRect.ScrollbarVisibility)objectCasters.GetCaster(typeof(ScrollRect.ScrollbarVisibility))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIScrollRectScrollbarVisibility(IntPtr L, int index, ScrollRect.ScrollbarVisibility val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIScrollRectScrollbarVisibility_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.ScrollRect.ScrollbarVisibility");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.ScrollRect.ScrollbarVisibility ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUISliderDirection(IntPtr L, Slider.Direction val)
	{
		if (UnityEngineUISliderDirection_TypeID == -1)
		{
			UnityEngineUISliderDirection_TypeID = getTypeId(L, typeof(Slider.Direction), out var _);
			if (UnityEngineUISliderDirection_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Slider.Direction));
				UnityEngineUISliderDirection_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUISliderDirection_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUISliderDirection_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Slider.Direction ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUISliderDirection_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Slider.Direction val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUISliderDirection_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Slider.Direction");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Slider.Direction");
			}
			val = (Slider.Direction)field;
		}
		else
		{
			val = (Slider.Direction)objectCasters.GetCaster(typeof(Slider.Direction))(L, index, null);
		}
	}

	public void UpdateUnityEngineUISliderDirection(IntPtr L, int index, Slider.Direction val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUISliderDirection_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Slider.Direction");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Slider.Direction ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIToggleToggleTransition(IntPtr L, Toggle.ToggleTransition val)
	{
		if (UnityEngineUIToggleToggleTransition_TypeID == -1)
		{
			UnityEngineUIToggleToggleTransition_TypeID = getTypeId(L, typeof(Toggle.ToggleTransition), out var _);
			if (UnityEngineUIToggleToggleTransition_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Toggle.ToggleTransition));
				UnityEngineUIToggleToggleTransition_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIToggleToggleTransition_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIToggleToggleTransition_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.Toggle.ToggleTransition ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIToggleToggleTransition_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Toggle.ToggleTransition val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIToggleToggleTransition_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Toggle.ToggleTransition");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.Toggle.ToggleTransition");
			}
			val = (Toggle.ToggleTransition)field;
		}
		else
		{
			val = (Toggle.ToggleTransition)objectCasters.GetCaster(typeof(Toggle.ToggleTransition))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIToggleToggleTransition(IntPtr L, int index, Toggle.ToggleTransition val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIToggleToggleTransition_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.Toggle.ToggleTransition");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.Toggle.ToggleTransition ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIGridLayoutGroupCorner(IntPtr L, GridLayoutGroup.Corner val)
	{
		if (UnityEngineUIGridLayoutGroupCorner_TypeID == -1)
		{
			UnityEngineUIGridLayoutGroupCorner_TypeID = getTypeId(L, typeof(GridLayoutGroup.Corner), out var _);
			if (UnityEngineUIGridLayoutGroupCorner_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(GridLayoutGroup.Corner));
				UnityEngineUIGridLayoutGroupCorner_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIGridLayoutGroupCorner_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIGridLayoutGroupCorner_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.GridLayoutGroup.Corner ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIGridLayoutGroupCorner_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out GridLayoutGroup.Corner val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIGridLayoutGroupCorner_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.GridLayoutGroup.Corner");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.GridLayoutGroup.Corner");
			}
			val = (GridLayoutGroup.Corner)field;
		}
		else
		{
			val = (GridLayoutGroup.Corner)objectCasters.GetCaster(typeof(GridLayoutGroup.Corner))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIGridLayoutGroupCorner(IntPtr L, int index, GridLayoutGroup.Corner val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIGridLayoutGroupCorner_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.GridLayoutGroup.Corner");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.GridLayoutGroup.Corner ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIGridLayoutGroupAxis(IntPtr L, GridLayoutGroup.Axis val)
	{
		if (UnityEngineUIGridLayoutGroupAxis_TypeID == -1)
		{
			UnityEngineUIGridLayoutGroupAxis_TypeID = getTypeId(L, typeof(GridLayoutGroup.Axis), out var _);
			if (UnityEngineUIGridLayoutGroupAxis_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(GridLayoutGroup.Axis));
				UnityEngineUIGridLayoutGroupAxis_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIGridLayoutGroupAxis_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIGridLayoutGroupAxis_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.GridLayoutGroup.Axis ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIGridLayoutGroupAxis_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out GridLayoutGroup.Axis val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIGridLayoutGroupAxis_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.GridLayoutGroup.Axis");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.GridLayoutGroup.Axis");
			}
			val = (GridLayoutGroup.Axis)field;
		}
		else
		{
			val = (GridLayoutGroup.Axis)objectCasters.GetCaster(typeof(GridLayoutGroup.Axis))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIGridLayoutGroupAxis(IntPtr L, int index, GridLayoutGroup.Axis val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIGridLayoutGroupAxis_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.GridLayoutGroup.Axis");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.GridLayoutGroup.Axis ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIGridLayoutGroupConstraint(IntPtr L, GridLayoutGroup.Constraint val)
	{
		if (UnityEngineUIGridLayoutGroupConstraint_TypeID == -1)
		{
			UnityEngineUIGridLayoutGroupConstraint_TypeID = getTypeId(L, typeof(GridLayoutGroup.Constraint), out var _);
			if (UnityEngineUIGridLayoutGroupConstraint_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(GridLayoutGroup.Constraint));
				UnityEngineUIGridLayoutGroupConstraint_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIGridLayoutGroupConstraint_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIGridLayoutGroupConstraint_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.GridLayoutGroup.Constraint ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIGridLayoutGroupConstraint_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out GridLayoutGroup.Constraint val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIGridLayoutGroupConstraint_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.GridLayoutGroup.Constraint");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.GridLayoutGroup.Constraint");
			}
			val = (GridLayoutGroup.Constraint)field;
		}
		else
		{
			val = (GridLayoutGroup.Constraint)objectCasters.GetCaster(typeof(GridLayoutGroup.Constraint))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIGridLayoutGroupConstraint(IntPtr L, int index, GridLayoutGroup.Constraint val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIGridLayoutGroupConstraint_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.GridLayoutGroup.Constraint");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.GridLayoutGroup.Constraint ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineUIContentSizeFitterFitMode(IntPtr L, ContentSizeFitter.FitMode val)
	{
		if (UnityEngineUIContentSizeFitterFitMode_TypeID == -1)
		{
			UnityEngineUIContentSizeFitterFitMode_TypeID = getTypeId(L, typeof(ContentSizeFitter.FitMode), out var _);
			if (UnityEngineUIContentSizeFitterFitMode_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ContentSizeFitter.FitMode));
				UnityEngineUIContentSizeFitterFitMode_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineUIContentSizeFitterFitMode_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineUIContentSizeFitterFitMode_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.UI.ContentSizeFitter.FitMode ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineUIContentSizeFitterFitMode_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ContentSizeFitter.FitMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIContentSizeFitterFitMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.ContentSizeFitter.FitMode");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.UI.ContentSizeFitter.FitMode");
			}
			val = (ContentSizeFitter.FitMode)field;
		}
		else
		{
			val = (ContentSizeFitter.FitMode)objectCasters.GetCaster(typeof(ContentSizeFitter.FitMode))(L, index, null);
		}
	}

	public void UpdateUnityEngineUIContentSizeFitterFitMode(IntPtr L, int index, ContentSizeFitter.FitMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineUIContentSizeFitterFitMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.UI.ContentSizeFitter.FitMode");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.UI.ContentSizeFitter.FitMode ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushSuperTextMeshAlignment(IntPtr L, SuperTextMesh.Alignment val)
	{
		if (SuperTextMeshAlignment_TypeID == -1)
		{
			SuperTextMeshAlignment_TypeID = getTypeId(L, typeof(SuperTextMesh.Alignment), out var _);
			if (SuperTextMeshAlignment_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(SuperTextMesh.Alignment));
				SuperTextMeshAlignment_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, SuperTextMeshAlignment_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, SuperTextMeshAlignment_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for SuperTextMesh.Alignment ,value=" + val);
			}
			Lua.lua_getref(L, SuperTextMeshAlignment_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out SuperTextMesh.Alignment val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SuperTextMeshAlignment_TypeID)
			{
				throw new Exception("invalid userdata for SuperTextMesh.Alignment");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for SuperTextMesh.Alignment");
			}
			val = (SuperTextMesh.Alignment)field;
		}
		else
		{
			val = (SuperTextMesh.Alignment)objectCasters.GetCaster(typeof(SuperTextMesh.Alignment))(L, index, null);
		}
	}

	public void UpdateSuperTextMeshAlignment(IntPtr L, int index, SuperTextMesh.Alignment val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SuperTextMeshAlignment_TypeID)
			{
				throw new Exception("invalid userdata for SuperTextMesh.Alignment");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for SuperTextMesh.Alignment ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineAnimatorCullingMode(IntPtr L, AnimatorCullingMode val)
	{
		if (UnityEngineAnimatorCullingMode_TypeID == -1)
		{
			UnityEngineAnimatorCullingMode_TypeID = getTypeId(L, typeof(AnimatorCullingMode), out var _);
			if (UnityEngineAnimatorCullingMode_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(AnimatorCullingMode));
				UnityEngineAnimatorCullingMode_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineAnimatorCullingMode_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineAnimatorCullingMode_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.AnimatorCullingMode ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineAnimatorCullingMode_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out AnimatorCullingMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineAnimatorCullingMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.AnimatorCullingMode");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.AnimatorCullingMode");
			}
			val = (AnimatorCullingMode)field;
		}
		else
		{
			val = (AnimatorCullingMode)objectCasters.GetCaster(typeof(AnimatorCullingMode))(L, index, null);
		}
	}

	public void UpdateUnityEngineAnimatorCullingMode(IntPtr L, int index, AnimatorCullingMode val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineAnimatorCullingMode_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.AnimatorCullingMode");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.AnimatorCullingMode ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineTextAnchor(IntPtr L, TextAnchor val)
	{
		if (UnityEngineTextAnchor_TypeID == -1)
		{
			UnityEngineTextAnchor_TypeID = getTypeId(L, typeof(TextAnchor), out var _);
			if (UnityEngineTextAnchor_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TextAnchor));
				UnityEngineTextAnchor_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineTextAnchor_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineTextAnchor_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.TextAnchor ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineTextAnchor_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TextAnchor val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineTextAnchor_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.TextAnchor");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.TextAnchor");
			}
			val = (TextAnchor)field;
		}
		else
		{
			val = (TextAnchor)objectCasters.GetCaster(typeof(TextAnchor))(L, index, null);
		}
	}

	public void UpdateUnityEngineTextAnchor(IntPtr L, int index, TextAnchor val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineTextAnchor_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.TextAnchor");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.TextAnchor ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushScrollViewMovementType(IntPtr L, ScrollView.MovementType val)
	{
		if (ScrollViewMovementType_TypeID == -1)
		{
			ScrollViewMovementType_TypeID = getTypeId(L, typeof(ScrollView.MovementType), out var _);
			if (ScrollViewMovementType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ScrollView.MovementType));
				ScrollViewMovementType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ScrollViewMovementType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ScrollViewMovementType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ScrollView.MovementType ,value=" + val);
			}
			Lua.lua_getref(L, ScrollViewMovementType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ScrollView.MovementType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ScrollViewMovementType_TypeID)
			{
				throw new Exception("invalid userdata for ScrollView.MovementType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ScrollView.MovementType");
			}
			val = (ScrollView.MovementType)field;
		}
		else
		{
			val = (ScrollView.MovementType)objectCasters.GetCaster(typeof(ScrollView.MovementType))(L, index, null);
		}
	}

	public void UpdateScrollViewMovementType(IntPtr L, int index, ScrollView.MovementType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ScrollViewMovementType_TypeID)
			{
				throw new Exception("invalid userdata for ScrollView.MovementType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ScrollView.MovementType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushScrollViewScrollbarVisibility(IntPtr L, ScrollView.ScrollbarVisibility val)
	{
		if (ScrollViewScrollbarVisibility_TypeID == -1)
		{
			ScrollViewScrollbarVisibility_TypeID = getTypeId(L, typeof(ScrollView.ScrollbarVisibility), out var _);
			if (ScrollViewScrollbarVisibility_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ScrollView.ScrollbarVisibility));
				ScrollViewScrollbarVisibility_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ScrollViewScrollbarVisibility_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ScrollViewScrollbarVisibility_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ScrollView.ScrollbarVisibility ,value=" + val);
			}
			Lua.lua_getref(L, ScrollViewScrollbarVisibility_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ScrollView.ScrollbarVisibility val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ScrollViewScrollbarVisibility_TypeID)
			{
				throw new Exception("invalid userdata for ScrollView.ScrollbarVisibility");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ScrollView.ScrollbarVisibility");
			}
			val = (ScrollView.ScrollbarVisibility)field;
		}
		else
		{
			val = (ScrollView.ScrollbarVisibility)objectCasters.GetCaster(typeof(ScrollView.ScrollbarVisibility))(L, index, null);
		}
	}

	public void UpdateScrollViewScrollbarVisibility(IntPtr L, int index, ScrollView.ScrollbarVisibility val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ScrollViewScrollbarVisibility_TypeID)
			{
				throw new Exception("invalid userdata for ScrollView.ScrollbarVisibility");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ScrollView.ScrollbarVisibility ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushScrollViewScrollViewLayoutType(IntPtr L, ScrollView.ScrollViewLayoutType val)
	{
		if (ScrollViewScrollViewLayoutType_TypeID == -1)
		{
			ScrollViewScrollViewLayoutType_TypeID = getTypeId(L, typeof(ScrollView.ScrollViewLayoutType), out var _);
			if (ScrollViewScrollViewLayoutType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ScrollView.ScrollViewLayoutType));
				ScrollViewScrollViewLayoutType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ScrollViewScrollViewLayoutType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ScrollViewScrollViewLayoutType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ScrollView.ScrollViewLayoutType ,value=" + val);
			}
			Lua.lua_getref(L, ScrollViewScrollViewLayoutType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ScrollView.ScrollViewLayoutType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ScrollViewScrollViewLayoutType_TypeID)
			{
				throw new Exception("invalid userdata for ScrollView.ScrollViewLayoutType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ScrollView.ScrollViewLayoutType");
			}
			val = (ScrollView.ScrollViewLayoutType)field;
		}
		else
		{
			val = (ScrollView.ScrollViewLayoutType)objectCasters.GetCaster(typeof(ScrollView.ScrollViewLayoutType))(L, index, null);
		}
	}

	public void UpdateScrollViewScrollViewLayoutType(IntPtr L, int index, ScrollView.ScrollViewLayoutType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ScrollViewScrollViewLayoutType_TypeID)
			{
				throw new Exception("invalid userdata for ScrollView.ScrollViewLayoutType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ScrollView.ScrollViewLayoutType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineTouchPhase(IntPtr L, TouchPhase val)
	{
		if (UnityEngineTouchPhase_TypeID == -1)
		{
			UnityEngineTouchPhase_TypeID = getTypeId(L, typeof(TouchPhase), out var _);
			if (UnityEngineTouchPhase_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TouchPhase));
				UnityEngineTouchPhase_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineTouchPhase_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineTouchPhase_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.TouchPhase ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineTouchPhase_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TouchPhase val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineTouchPhase_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.TouchPhase");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.TouchPhase");
			}
			val = (TouchPhase)field;
		}
		else
		{
			val = (TouchPhase)objectCasters.GetCaster(typeof(TouchPhase))(L, index, null);
		}
	}

	public void UpdateUnityEngineTouchPhase(IntPtr L, int index, TouchPhase val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineTouchPhase_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.TouchPhase");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.TouchPhase ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushBitBenderGamesMobileTouchCameraState(IntPtr L, MobileTouchCamera.State val)
	{
		if (BitBenderGamesMobileTouchCameraState_TypeID == -1)
		{
			BitBenderGamesMobileTouchCameraState_TypeID = getTypeId(L, typeof(MobileTouchCamera.State), out var _);
			if (BitBenderGamesMobileTouchCameraState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(MobileTouchCamera.State));
				BitBenderGamesMobileTouchCameraState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, BitBenderGamesMobileTouchCameraState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, BitBenderGamesMobileTouchCameraState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for BitBenderGames.MobileTouchCamera.State ,value=" + val);
			}
			Lua.lua_getref(L, BitBenderGamesMobileTouchCameraState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out MobileTouchCamera.State val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != BitBenderGamesMobileTouchCameraState_TypeID)
			{
				throw new Exception("invalid userdata for BitBenderGames.MobileTouchCamera.State");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for BitBenderGames.MobileTouchCamera.State");
			}
			val = (MobileTouchCamera.State)field;
		}
		else
		{
			val = (MobileTouchCamera.State)objectCasters.GetCaster(typeof(MobileTouchCamera.State))(L, index, null);
		}
	}

	public void UpdateBitBenderGamesMobileTouchCameraState(IntPtr L, int index, MobileTouchCamera.State val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != BitBenderGamesMobileTouchCameraState_TypeID)
			{
				throw new Exception("invalid userdata for BitBenderGames.MobileTouchCamera.State");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for BitBenderGames.MobileTouchCamera.State ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushGameFrameworkLocalizationLanguage(IntPtr L, Language val)
	{
		if (GameFrameworkLocalizationLanguage_TypeID == -1)
		{
			GameFrameworkLocalizationLanguage_TypeID = getTypeId(L, typeof(Language), out var _);
			if (GameFrameworkLocalizationLanguage_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(Language));
				GameFrameworkLocalizationLanguage_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, GameFrameworkLocalizationLanguage_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, GameFrameworkLocalizationLanguage_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for GameFramework.Localization.Language ,value=" + val);
			}
			Lua.lua_getref(L, GameFrameworkLocalizationLanguage_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out Language val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameFrameworkLocalizationLanguage_TypeID)
			{
				throw new Exception("invalid userdata for GameFramework.Localization.Language");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for GameFramework.Localization.Language");
			}
			val = (Language)field;
		}
		else
		{
			val = (Language)objectCasters.GetCaster(typeof(Language))(L, index, null);
		}
	}

	public void UpdateGameFrameworkLocalizationLanguage(IntPtr L, int index, Language val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameFrameworkLocalizationLanguage_TypeID)
			{
				throw new Exception("invalid userdata for GameFramework.Localization.Language");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for GameFramework.Localization.Language ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushGameDefinesCityLabelColorType(IntPtr L, GameDefines.CityLabelColorType val)
	{
		if (GameDefinesCityLabelColorType_TypeID == -1)
		{
			GameDefinesCityLabelColorType_TypeID = getTypeId(L, typeof(GameDefines.CityLabelColorType), out var _);
			if (GameDefinesCityLabelColorType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(GameDefines.CityLabelColorType));
				GameDefinesCityLabelColorType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, GameDefinesCityLabelColorType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, GameDefinesCityLabelColorType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for GameDefines.CityLabelColorType ,value=" + val);
			}
			Lua.lua_getref(L, GameDefinesCityLabelColorType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out GameDefines.CityLabelColorType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameDefinesCityLabelColorType_TypeID)
			{
				throw new Exception("invalid userdata for GameDefines.CityLabelColorType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for GameDefines.CityLabelColorType");
			}
			val = (GameDefines.CityLabelColorType)field;
		}
		else
		{
			val = (GameDefines.CityLabelColorType)objectCasters.GetCaster(typeof(GameDefines.CityLabelColorType))(L, index, null);
		}
	}

	public void UpdateGameDefinesCityLabelColorType(IntPtr L, int index, GameDefines.CityLabelColorType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameDefinesCityLabelColorType_TypeID)
			{
				throw new Exception("invalid userdata for GameDefines.CityLabelColorType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for GameDefines.CityLabelColorType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushGameDefinesBuildConnectRoadDirection(IntPtr L, GameDefines.BuildConnectRoadDirection val)
	{
		if (GameDefinesBuildConnectRoadDirection_TypeID == -1)
		{
			GameDefinesBuildConnectRoadDirection_TypeID = getTypeId(L, typeof(GameDefines.BuildConnectRoadDirection), out var _);
			if (GameDefinesBuildConnectRoadDirection_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(GameDefines.BuildConnectRoadDirection));
				GameDefinesBuildConnectRoadDirection_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, GameDefinesBuildConnectRoadDirection_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, GameDefinesBuildConnectRoadDirection_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for GameDefines.BuildConnectRoadDirection ,value=" + val);
			}
			Lua.lua_getref(L, GameDefinesBuildConnectRoadDirection_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out GameDefines.BuildConnectRoadDirection val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameDefinesBuildConnectRoadDirection_TypeID)
			{
				throw new Exception("invalid userdata for GameDefines.BuildConnectRoadDirection");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for GameDefines.BuildConnectRoadDirection");
			}
			val = (GameDefines.BuildConnectRoadDirection)field;
		}
		else
		{
			val = (GameDefines.BuildConnectRoadDirection)objectCasters.GetCaster(typeof(GameDefines.BuildConnectRoadDirection))(L, index, null);
		}
	}

	public void UpdateGameDefinesBuildConnectRoadDirection(IntPtr L, int index, GameDefines.BuildConnectRoadDirection val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameDefinesBuildConnectRoadDirection_TypeID)
			{
				throw new Exception("invalid userdata for GameDefines.BuildConnectRoadDirection");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for GameDefines.BuildConnectRoadDirection ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushGameDefinesDirectionType(IntPtr L, GameDefines.DirectionType val)
	{
		if (GameDefinesDirectionType_TypeID == -1)
		{
			GameDefinesDirectionType_TypeID = getTypeId(L, typeof(GameDefines.DirectionType), out var _);
			if (GameDefinesDirectionType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(GameDefines.DirectionType));
				GameDefinesDirectionType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, GameDefinesDirectionType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, GameDefinesDirectionType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for GameDefines.DirectionType ,value=" + val);
			}
			Lua.lua_getref(L, GameDefinesDirectionType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out GameDefines.DirectionType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameDefinesDirectionType_TypeID)
			{
				throw new Exception("invalid userdata for GameDefines.DirectionType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for GameDefines.DirectionType");
			}
			val = (GameDefines.DirectionType)field;
		}
		else
		{
			val = (GameDefines.DirectionType)objectCasters.GetCaster(typeof(GameDefines.DirectionType))(L, index, null);
		}
	}

	public void UpdateGameDefinesDirectionType(IntPtr L, int index, GameDefines.DirectionType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != GameDefinesDirectionType_TypeID)
			{
				throw new Exception("invalid userdata for GameDefines.DirectionType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for GameDefines.DirectionType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushResourceManagerPreloadType(IntPtr L, ResourceManager.PreloadType val)
	{
		if (ResourceManagerPreloadType_TypeID == -1)
		{
			ResourceManagerPreloadType_TypeID = getTypeId(L, typeof(ResourceManager.PreloadType), out var _);
			if (ResourceManagerPreloadType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ResourceManager.PreloadType));
				ResourceManagerPreloadType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ResourceManagerPreloadType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ResourceManagerPreloadType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ResourceManager.PreloadType ,value=" + val);
			}
			Lua.lua_getref(L, ResourceManagerPreloadType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ResourceManager.PreloadType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ResourceManagerPreloadType_TypeID)
			{
				throw new Exception("invalid userdata for ResourceManager.PreloadType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ResourceManager.PreloadType");
			}
			val = (ResourceManager.PreloadType)field;
		}
		else
		{
			val = (ResourceManager.PreloadType)objectCasters.GetCaster(typeof(ResourceManager.PreloadType))(L, index, null);
		}
	}

	public void UpdateResourceManagerPreloadType(IntPtr L, int index, ResourceManager.PreloadType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ResourceManagerPreloadType_TypeID)
			{
				throw new Exception("invalid userdata for ResourceManager.PreloadType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ResourceManager.PreloadType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushLODType(IntPtr L, LODType val)
	{
		if (LODType_TypeID == -1)
		{
			LODType_TypeID = getTypeId(L, typeof(LODType), out var _);
			if (LODType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(LODType));
				LODType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, LODType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, LODType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for LODType ,value=" + val);
			}
			Lua.lua_getref(L, LODType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out LODType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != LODType_TypeID)
			{
				throw new Exception("invalid userdata for LODType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for LODType");
			}
			val = (LODType)field;
		}
		else
		{
			val = (LODType)objectCasters.GetCaster(typeof(LODType))(L, index, null);
		}
	}

	public void UpdateLODType(IntPtr L, int index, LODType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != LODType_TypeID)
			{
				throw new Exception("invalid userdata for LODType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for LODType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushSceneManagerSceneID(IntPtr L, SceneManager.SceneID val)
	{
		if (SceneManagerSceneID_TypeID == -1)
		{
			SceneManagerSceneID_TypeID = getTypeId(L, typeof(SceneManager.SceneID), out var _);
			if (SceneManagerSceneID_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(SceneManager.SceneID));
				SceneManagerSceneID_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, SceneManagerSceneID_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, SceneManagerSceneID_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for SceneManager.SceneID ,value=" + val);
			}
			Lua.lua_getref(L, SceneManagerSceneID_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out SceneManager.SceneID val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SceneManagerSceneID_TypeID)
			{
				throw new Exception("invalid userdata for SceneManager.SceneID");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for SceneManager.SceneID");
			}
			val = (SceneManager.SceneID)field;
		}
		else
		{
			val = (SceneManager.SceneID)objectCasters.GetCaster(typeof(SceneManager.SceneID))(L, index, null);
		}
	}

	public void UpdateSceneManagerSceneID(IntPtr L, int index, SceneManager.SceneID val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SceneManagerSceneID_TypeID)
			{
				throw new Exception("invalid userdata for SceneManager.SceneID");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for SceneManager.SceneID ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushCityBuildingBuildSceneType(IntPtr L, CityBuilding.BuildSceneType val)
	{
		if (CityBuildingBuildSceneType_TypeID == -1)
		{
			CityBuildingBuildSceneType_TypeID = getTypeId(L, typeof(CityBuilding.BuildSceneType), out var _);
			if (CityBuildingBuildSceneType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(CityBuilding.BuildSceneType));
				CityBuildingBuildSceneType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, CityBuildingBuildSceneType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, CityBuildingBuildSceneType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for CityBuilding.BuildSceneType ,value=" + val);
			}
			Lua.lua_getref(L, CityBuildingBuildSceneType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out CityBuilding.BuildSceneType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != CityBuildingBuildSceneType_TypeID)
			{
				throw new Exception("invalid userdata for CityBuilding.BuildSceneType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for CityBuilding.BuildSceneType");
			}
			val = (CityBuilding.BuildSceneType)field;
		}
		else
		{
			val = (CityBuilding.BuildSceneType)objectCasters.GetCaster(typeof(CityBuilding.BuildSceneType))(L, index, null);
		}
	}

	public void UpdateCityBuildingBuildSceneType(IntPtr L, int index, CityBuilding.BuildSceneType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != CityBuildingBuildSceneType_TypeID)
			{
				throw new Exception("invalid userdata for CityBuilding.BuildSceneType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for CityBuilding.BuildSceneType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushModelManagerModelObjectType(IntPtr L, ModelManager.ModelObjectType val)
	{
		if (ModelManagerModelObjectType_TypeID == -1)
		{
			ModelManagerModelObjectType_TypeID = getTypeId(L, typeof(ModelManager.ModelObjectType), out var _);
			if (ModelManagerModelObjectType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ModelManager.ModelObjectType));
				ModelManagerModelObjectType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ModelManagerModelObjectType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ModelManagerModelObjectType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ModelManager.ModelObjectType ,value=" + val);
			}
			Lua.lua_getref(L, ModelManagerModelObjectType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ModelManager.ModelObjectType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ModelManagerModelObjectType_TypeID)
			{
				throw new Exception("invalid userdata for ModelManager.ModelObjectType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ModelManager.ModelObjectType");
			}
			val = (ModelManager.ModelObjectType)field;
		}
		else
		{
			val = (ModelManager.ModelObjectType)objectCasters.GetCaster(typeof(ModelManager.ModelObjectType))(L, index, null);
		}
	}

	public void UpdateModelManagerModelObjectType(IntPtr L, int index, ModelManager.ModelObjectType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ModelManagerModelObjectType_TypeID)
			{
				throw new Exception("invalid userdata for ModelManager.ModelObjectType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ModelManager.ModelObjectType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushFakeModelManagerTempRoadType(IntPtr L, FakeModelManager.TempRoadType val)
	{
		if (FakeModelManagerTempRoadType_TypeID == -1)
		{
			FakeModelManagerTempRoadType_TypeID = getTypeId(L, typeof(FakeModelManager.TempRoadType), out var _);
			if (FakeModelManagerTempRoadType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(FakeModelManager.TempRoadType));
				FakeModelManagerTempRoadType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, FakeModelManagerTempRoadType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, FakeModelManagerTempRoadType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for FakeModelManager.TempRoadType ,value=" + val);
			}
			Lua.lua_getref(L, FakeModelManagerTempRoadType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out FakeModelManager.TempRoadType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FakeModelManagerTempRoadType_TypeID)
			{
				throw new Exception("invalid userdata for FakeModelManager.TempRoadType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for FakeModelManager.TempRoadType");
			}
			val = (FakeModelManager.TempRoadType)field;
		}
		else
		{
			val = (FakeModelManager.TempRoadType)objectCasters.GetCaster(typeof(FakeModelManager.TempRoadType))(L, index, null);
		}
	}

	public void UpdateFakeModelManagerTempRoadType(IntPtr L, int index, FakeModelManager.TempRoadType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FakeModelManagerTempRoadType_TypeID)
			{
				throw new Exception("invalid userdata for FakeModelManager.TempRoadType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for FakeModelManager.TempRoadType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushWorldMarchDataManagerBattleWordType(IntPtr L, WorldMarchDataManager.BattleWordType val)
	{
		if (WorldMarchDataManagerBattleWordType_TypeID == -1)
		{
			WorldMarchDataManagerBattleWordType_TypeID = getTypeId(L, typeof(WorldMarchDataManager.BattleWordType), out var _);
			if (WorldMarchDataManagerBattleWordType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(WorldMarchDataManager.BattleWordType));
				WorldMarchDataManagerBattleWordType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, WorldMarchDataManagerBattleWordType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, WorldMarchDataManagerBattleWordType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for WorldMarchDataManager.BattleWordType ,value=" + val);
			}
			Lua.lua_getref(L, WorldMarchDataManagerBattleWordType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out WorldMarchDataManager.BattleWordType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != WorldMarchDataManagerBattleWordType_TypeID)
			{
				throw new Exception("invalid userdata for WorldMarchDataManager.BattleWordType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for WorldMarchDataManager.BattleWordType");
			}
			val = (WorldMarchDataManager.BattleWordType)field;
		}
		else
		{
			val = (WorldMarchDataManager.BattleWordType)objectCasters.GetCaster(typeof(WorldMarchDataManager.BattleWordType))(L, index, null);
		}
	}

	public void UpdateWorldMarchDataManagerBattleWordType(IntPtr L, int index, WorldMarchDataManager.BattleWordType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != WorldMarchDataManagerBattleWordType_TypeID)
			{
				throw new Exception("invalid userdata for WorldMarchDataManager.BattleWordType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for WorldMarchDataManager.BattleWordType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushNewQueueState(IntPtr L, NewQueueState val)
	{
		if (NewQueueState_TypeID == -1)
		{
			NewQueueState_TypeID = getTypeId(L, typeof(NewQueueState), out var _);
			if (NewQueueState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(NewQueueState));
				NewQueueState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, NewQueueState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, NewQueueState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for NewQueueState ,value=" + val);
			}
			Lua.lua_getref(L, NewQueueState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out NewQueueState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != NewQueueState_TypeID)
			{
				throw new Exception("invalid userdata for NewQueueState");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for NewQueueState");
			}
			val = (NewQueueState)field;
		}
		else
		{
			val = (NewQueueState)objectCasters.GetCaster(typeof(NewQueueState))(L, index, null);
		}
	}

	public void UpdateNewQueueState(IntPtr L, int index, NewQueueState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != NewQueueState_TypeID)
			{
				throw new Exception("invalid userdata for NewQueueState");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for NewQueueState ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushResourceType(IntPtr L, ResourceType val)
	{
		if (ResourceType_TypeID == -1)
		{
			ResourceType_TypeID = getTypeId(L, typeof(ResourceType), out var _);
			if (ResourceType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ResourceType));
				ResourceType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ResourceType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ResourceType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ResourceType ,value=" + val);
			}
			Lua.lua_getref(L, ResourceType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ResourceType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ResourceType_TypeID)
			{
				throw new Exception("invalid userdata for ResourceType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ResourceType");
			}
			val = (ResourceType)field;
		}
		else
		{
			val = (ResourceType)objectCasters.GetCaster(typeof(ResourceType))(L, index, null);
		}
	}

	public void UpdateResourceType(IntPtr L, int index, ResourceType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ResourceType_TypeID)
			{
				throw new Exception("invalid userdata for ResourceType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ResourceType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushBuildingState(IntPtr L, BuildingState val)
	{
		if (BuildingState_TypeID == -1)
		{
			BuildingState_TypeID = getTypeId(L, typeof(BuildingState), out var _);
			if (BuildingState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(BuildingState));
				BuildingState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, BuildingState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, BuildingState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for BuildingState ,value=" + val);
			}
			Lua.lua_getref(L, BuildingState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out BuildingState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != BuildingState_TypeID)
			{
				throw new Exception("invalid userdata for BuildingState");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for BuildingState");
			}
			val = (BuildingState)field;
		}
		else
		{
			val = (BuildingState)objectCasters.GetCaster(typeof(BuildingState))(L, index, null);
		}
	}

	public void UpdateBuildingState(IntPtr L, int index, BuildingState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != BuildingState_TypeID)
			{
				throw new Exception("invalid userdata for BuildingState");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for BuildingState ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushPlaceBuildType(IntPtr L, PlaceBuildType val)
	{
		if (PlaceBuildType_TypeID == -1)
		{
			PlaceBuildType_TypeID = getTypeId(L, typeof(PlaceBuildType), out var _);
			if (PlaceBuildType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(PlaceBuildType));
				PlaceBuildType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, PlaceBuildType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, PlaceBuildType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for PlaceBuildType ,value=" + val);
			}
			Lua.lua_getref(L, PlaceBuildType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out PlaceBuildType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != PlaceBuildType_TypeID)
			{
				throw new Exception("invalid userdata for PlaceBuildType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for PlaceBuildType");
			}
			val = (PlaceBuildType)field;
		}
		else
		{
			val = (PlaceBuildType)objectCasters.GetCaster(typeof(PlaceBuildType))(L, index, null);
		}
	}

	public void UpdatePlaceBuildType(IntPtr L, int index, PlaceBuildType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != PlaceBuildType_TypeID)
			{
				throw new Exception("invalid userdata for PlaceBuildType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for PlaceBuildType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushMarchStatus(IntPtr L, MarchStatus val)
	{
		if (MarchStatus_TypeID == -1)
		{
			MarchStatus_TypeID = getTypeId(L, typeof(MarchStatus), out var _);
			if (MarchStatus_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(MarchStatus));
				MarchStatus_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, MarchStatus_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, MarchStatus_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for MarchStatus ,value=" + val);
			}
			Lua.lua_getref(L, MarchStatus_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out MarchStatus val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != MarchStatus_TypeID)
			{
				throw new Exception("invalid userdata for MarchStatus");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for MarchStatus");
			}
			val = (MarchStatus)field;
		}
		else
		{
			val = (MarchStatus)objectCasters.GetCaster(typeof(MarchStatus))(L, index, null);
		}
	}

	public void UpdateMarchStatus(IntPtr L, int index, MarchStatus val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != MarchStatus_TypeID)
			{
				throw new Exception("invalid userdata for MarchStatus");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for MarchStatus ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushUnityEngineTimelineClipCaps(IntPtr L, ClipCaps val)
	{
		if (UnityEngineTimelineClipCaps_TypeID == -1)
		{
			UnityEngineTimelineClipCaps_TypeID = getTypeId(L, typeof(ClipCaps), out var _);
			if (UnityEngineTimelineClipCaps_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ClipCaps));
				UnityEngineTimelineClipCaps_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, UnityEngineTimelineClipCaps_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, UnityEngineTimelineClipCaps_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for UnityEngine.Timeline.ClipCaps ,value=" + val);
			}
			Lua.lua_getref(L, UnityEngineTimelineClipCaps_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ClipCaps val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineTimelineClipCaps_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Timeline.ClipCaps");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for UnityEngine.Timeline.ClipCaps");
			}
			val = (ClipCaps)field;
		}
		else
		{
			val = (ClipCaps)objectCasters.GetCaster(typeof(ClipCaps))(L, index, null);
		}
	}

	public void UpdateUnityEngineTimelineClipCaps(IntPtr L, int index, ClipCaps val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != UnityEngineTimelineClipCaps_TypeID)
			{
				throw new Exception("invalid userdata for UnityEngine.Timeline.ClipCaps");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for UnityEngine.Timeline.ClipCaps ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTMProTextAlignmentOptions(IntPtr L, TextAlignmentOptions val)
	{
		if (TMProTextAlignmentOptions_TypeID == -1)
		{
			TMProTextAlignmentOptions_TypeID = getTypeId(L, typeof(TextAlignmentOptions), out var _);
			if (TMProTextAlignmentOptions_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TextAlignmentOptions));
				TMProTextAlignmentOptions_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TMProTextAlignmentOptions_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TMProTextAlignmentOptions_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TMPro.TextAlignmentOptions ,value=" + val);
			}
			Lua.lua_getref(L, TMProTextAlignmentOptions_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TextAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTextAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TextAlignmentOptions");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TMPro.TextAlignmentOptions");
			}
			val = (TextAlignmentOptions)field;
		}
		else
		{
			val = (TextAlignmentOptions)objectCasters.GetCaster(typeof(TextAlignmentOptions))(L, index, null);
		}
	}

	public void UpdateTMProTextAlignmentOptions(IntPtr L, int index, TextAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTextAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TextAlignmentOptions");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TMPro.TextAlignmentOptions ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTMProTMP_InputFieldContentType(IntPtr L, TMP_InputField.ContentType val)
	{
		if (TMProTMP_InputFieldContentType_TypeID == -1)
		{
			TMProTMP_InputFieldContentType_TypeID = getTypeId(L, typeof(TMP_InputField.ContentType), out var _);
			if (TMProTMP_InputFieldContentType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TMP_InputField.ContentType));
				TMProTMP_InputFieldContentType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TMProTMP_InputFieldContentType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TMProTMP_InputFieldContentType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TMPro.TMP_InputField.ContentType ,value=" + val);
			}
			Lua.lua_getref(L, TMProTMP_InputFieldContentType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TMP_InputField.ContentType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldContentType_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.ContentType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TMPro.TMP_InputField.ContentType");
			}
			val = (TMP_InputField.ContentType)field;
		}
		else
		{
			val = (TMP_InputField.ContentType)objectCasters.GetCaster(typeof(TMP_InputField.ContentType))(L, index, null);
		}
	}

	public void UpdateTMProTMP_InputFieldContentType(IntPtr L, int index, TMP_InputField.ContentType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldContentType_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.ContentType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TMPro.TMP_InputField.ContentType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTMProTMP_InputFieldInputType(IntPtr L, TMP_InputField.InputType val)
	{
		if (TMProTMP_InputFieldInputType_TypeID == -1)
		{
			TMProTMP_InputFieldInputType_TypeID = getTypeId(L, typeof(TMP_InputField.InputType), out var _);
			if (TMProTMP_InputFieldInputType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TMP_InputField.InputType));
				TMProTMP_InputFieldInputType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TMProTMP_InputFieldInputType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TMProTMP_InputFieldInputType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TMPro.TMP_InputField.InputType ,value=" + val);
			}
			Lua.lua_getref(L, TMProTMP_InputFieldInputType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TMP_InputField.InputType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldInputType_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.InputType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TMPro.TMP_InputField.InputType");
			}
			val = (TMP_InputField.InputType)field;
		}
		else
		{
			val = (TMP_InputField.InputType)objectCasters.GetCaster(typeof(TMP_InputField.InputType))(L, index, null);
		}
	}

	public void UpdateTMProTMP_InputFieldInputType(IntPtr L, int index, TMP_InputField.InputType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldInputType_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.InputType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TMPro.TMP_InputField.InputType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTMProTMP_InputFieldCharacterValidation(IntPtr L, TMP_InputField.CharacterValidation val)
	{
		if (TMProTMP_InputFieldCharacterValidation_TypeID == -1)
		{
			TMProTMP_InputFieldCharacterValidation_TypeID = getTypeId(L, typeof(TMP_InputField.CharacterValidation), out var _);
			if (TMProTMP_InputFieldCharacterValidation_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TMP_InputField.CharacterValidation));
				TMProTMP_InputFieldCharacterValidation_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TMProTMP_InputFieldCharacterValidation_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TMProTMP_InputFieldCharacterValidation_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TMPro.TMP_InputField.CharacterValidation ,value=" + val);
			}
			Lua.lua_getref(L, TMProTMP_InputFieldCharacterValidation_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TMP_InputField.CharacterValidation val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldCharacterValidation_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.CharacterValidation");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TMPro.TMP_InputField.CharacterValidation");
			}
			val = (TMP_InputField.CharacterValidation)field;
		}
		else
		{
			val = (TMP_InputField.CharacterValidation)objectCasters.GetCaster(typeof(TMP_InputField.CharacterValidation))(L, index, null);
		}
	}

	public void UpdateTMProTMP_InputFieldCharacterValidation(IntPtr L, int index, TMP_InputField.CharacterValidation val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldCharacterValidation_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.CharacterValidation");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TMPro.TMP_InputField.CharacterValidation ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTMProTMP_InputFieldLineType(IntPtr L, TMP_InputField.LineType val)
	{
		if (TMProTMP_InputFieldLineType_TypeID == -1)
		{
			TMProTMP_InputFieldLineType_TypeID = getTypeId(L, typeof(TMP_InputField.LineType), out var _);
			if (TMProTMP_InputFieldLineType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TMP_InputField.LineType));
				TMProTMP_InputFieldLineType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TMProTMP_InputFieldLineType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TMProTMP_InputFieldLineType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TMPro.TMP_InputField.LineType ,value=" + val);
			}
			Lua.lua_getref(L, TMProTMP_InputFieldLineType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TMP_InputField.LineType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldLineType_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.LineType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TMPro.TMP_InputField.LineType");
			}
			val = (TMP_InputField.LineType)field;
		}
		else
		{
			val = (TMP_InputField.LineType)objectCasters.GetCaster(typeof(TMP_InputField.LineType))(L, index, null);
		}
	}

	public void UpdateTMProTMP_InputFieldLineType(IntPtr L, int index, TMP_InputField.LineType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldLineType_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputField.LineType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TMPro.TMP_InputField.LineType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushTMProTMP_InputFieldExHorizontalAlignmentOptions(IntPtr L, TMP_InputFieldEx.HorizontalAlignmentOptions val)
	{
		if (TMProTMP_InputFieldExHorizontalAlignmentOptions_TypeID == -1)
		{
			TMProTMP_InputFieldExHorizontalAlignmentOptions_TypeID = getTypeId(L, typeof(TMP_InputFieldEx.HorizontalAlignmentOptions), out var _);
			if (TMProTMP_InputFieldExHorizontalAlignmentOptions_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(TMP_InputFieldEx.HorizontalAlignmentOptions));
				TMProTMP_InputFieldExHorizontalAlignmentOptions_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, TMProTMP_InputFieldExHorizontalAlignmentOptions_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, TMProTMP_InputFieldExHorizontalAlignmentOptions_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions ,value=" + val);
			}
			Lua.lua_getref(L, TMProTMP_InputFieldExHorizontalAlignmentOptions_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out TMP_InputFieldEx.HorizontalAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldExHorizontalAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions");
			}
			val = (TMP_InputFieldEx.HorizontalAlignmentOptions)field;
		}
		else
		{
			val = (TMP_InputFieldEx.HorizontalAlignmentOptions)objectCasters.GetCaster(typeof(TMP_InputFieldEx.HorizontalAlignmentOptions))(L, index, null);
		}
	}

	public void UpdateTMProTMP_InputFieldExHorizontalAlignmentOptions(IntPtr L, int index, TMP_InputFieldEx.HorizontalAlignmentOptions val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != TMProTMP_InputFieldExHorizontalAlignmentOptions_TypeID)
			{
				throw new Exception("invalid userdata for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for TMPro.TMP_InputFieldEx.HorizontalAlignmentOptions ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushInstanceRequestState(IntPtr L, InstanceRequest.State val)
	{
		if (InstanceRequestState_TypeID == -1)
		{
			InstanceRequestState_TypeID = getTypeId(L, typeof(InstanceRequest.State), out var _);
			if (InstanceRequestState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(InstanceRequest.State));
				InstanceRequestState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, InstanceRequestState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, InstanceRequestState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for InstanceRequest.State ,value=" + val);
			}
			Lua.lua_getref(L, InstanceRequestState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out InstanceRequest.State val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != InstanceRequestState_TypeID)
			{
				throw new Exception("invalid userdata for InstanceRequest.State");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for InstanceRequest.State");
			}
			val = (InstanceRequest.State)field;
		}
		else
		{
			val = (InstanceRequest.State)objectCasters.GetCaster(typeof(InstanceRequest.State))(L, index, null);
		}
	}

	public void UpdateInstanceRequestState(IntPtr L, int index, InstanceRequest.State val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != InstanceRequestState_TypeID)
			{
				throw new Exception("invalid userdata for InstanceRequest.State");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for InstanceRequest.State ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushNewMarchType(IntPtr L, NewMarchType val)
	{
		if (NewMarchType_TypeID == -1)
		{
			NewMarchType_TypeID = getTypeId(L, typeof(NewMarchType), out var _);
			if (NewMarchType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(NewMarchType));
				NewMarchType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, NewMarchType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, NewMarchType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for NewMarchType ,value=" + val);
			}
			Lua.lua_getref(L, NewMarchType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out NewMarchType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != NewMarchType_TypeID)
			{
				throw new Exception("invalid userdata for NewMarchType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for NewMarchType");
			}
			val = (NewMarchType)field;
		}
		else
		{
			val = (NewMarchType)objectCasters.GetCaster(typeof(NewMarchType))(L, index, null);
		}
	}

	public void UpdateNewMarchType(IntPtr L, int index, NewMarchType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != NewMarchType_TypeID)
			{
				throw new Exception("invalid userdata for NewMarchType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for NewMarchType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushWorldPointType(IntPtr L, WorldPointType val)
	{
		if (WorldPointType_TypeID == -1)
		{
			WorldPointType_TypeID = getTypeId(L, typeof(WorldPointType), out var _);
			if (WorldPointType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(WorldPointType));
				WorldPointType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, WorldPointType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, WorldPointType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for WorldPointType ,value=" + val);
			}
			Lua.lua_getref(L, WorldPointType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out WorldPointType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != WorldPointType_TypeID)
			{
				throw new Exception("invalid userdata for WorldPointType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for WorldPointType");
			}
			val = (WorldPointType)field;
		}
		else
		{
			val = (WorldPointType)objectCasters.GetCaster(typeof(WorldPointType))(L, index, null);
		}
	}

	public void UpdateWorldPointType(IntPtr L, int index, WorldPointType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != WorldPointType_TypeID)
			{
				throw new Exception("invalid userdata for WorldPointType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for WorldPointType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushSuperScrollViewListItemArrangeType(IntPtr L, ListItemArrangeType val)
	{
		if (SuperScrollViewListItemArrangeType_TypeID == -1)
		{
			SuperScrollViewListItemArrangeType_TypeID = getTypeId(L, typeof(ListItemArrangeType), out var _);
			if (SuperScrollViewListItemArrangeType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ListItemArrangeType));
				SuperScrollViewListItemArrangeType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, SuperScrollViewListItemArrangeType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, SuperScrollViewListItemArrangeType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for SuperScrollView.ListItemArrangeType ,value=" + val);
			}
			Lua.lua_getref(L, SuperScrollViewListItemArrangeType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ListItemArrangeType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SuperScrollViewListItemArrangeType_TypeID)
			{
				throw new Exception("invalid userdata for SuperScrollView.ListItemArrangeType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for SuperScrollView.ListItemArrangeType");
			}
			val = (ListItemArrangeType)field;
		}
		else
		{
			val = (ListItemArrangeType)objectCasters.GetCaster(typeof(ListItemArrangeType))(L, index, null);
		}
	}

	public void UpdateSuperScrollViewListItemArrangeType(IntPtr L, int index, ListItemArrangeType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != SuperScrollViewListItemArrangeType_TypeID)
			{
				throw new Exception("invalid userdata for SuperScrollView.ListItemArrangeType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for SuperScrollView.ListItemArrangeType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushURLGroupType(IntPtr L, URLGroupType val)
	{
		if (URLGroupType_TypeID == -1)
		{
			URLGroupType_TypeID = getTypeId(L, typeof(URLGroupType), out var _);
			if (URLGroupType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(URLGroupType));
				URLGroupType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, URLGroupType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, URLGroupType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for URLGroupType ,value=" + val);
			}
			Lua.lua_getref(L, URLGroupType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out URLGroupType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != URLGroupType_TypeID)
			{
				throw new Exception("invalid userdata for URLGroupType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for URLGroupType");
			}
			val = (URLGroupType)field;
		}
		else
		{
			val = (URLGroupType)objectCasters.GetCaster(typeof(URLGroupType))(L, index, null);
		}
	}

	public void UpdateURLGroupType(IntPtr L, int index, URLGroupType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != URLGroupType_TypeID)
			{
				throw new Exception("invalid userdata for URLGroupType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for URLGroupType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(IntPtr L, MeteoriteWorldEffectPlayer.FragmentData.FragmentType val)
	{
		if (MeteoriteWorldEffectPlayerFragmentDataFragmentType_TypeID == -1)
		{
			MeteoriteWorldEffectPlayerFragmentDataFragmentType_TypeID = getTypeId(L, typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType), out var _);
			if (MeteoriteWorldEffectPlayerFragmentDataFragmentType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType));
				MeteoriteWorldEffectPlayerFragmentDataFragmentType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, MeteoriteWorldEffectPlayerFragmentDataFragmentType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, MeteoriteWorldEffectPlayerFragmentDataFragmentType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for MeteoriteWorldEffectPlayer.FragmentData.FragmentType ,value=" + val);
			}
			Lua.lua_getref(L, MeteoriteWorldEffectPlayerFragmentDataFragmentType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out MeteoriteWorldEffectPlayer.FragmentData.FragmentType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != MeteoriteWorldEffectPlayerFragmentDataFragmentType_TypeID)
			{
				throw new Exception("invalid userdata for MeteoriteWorldEffectPlayer.FragmentData.FragmentType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for MeteoriteWorldEffectPlayer.FragmentData.FragmentType");
			}
			val = (MeteoriteWorldEffectPlayer.FragmentData.FragmentType)field;
		}
		else
		{
			val = (MeteoriteWorldEffectPlayer.FragmentData.FragmentType)objectCasters.GetCaster(typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType))(L, index, null);
		}
	}

	public void UpdateMeteoriteWorldEffectPlayerFragmentDataFragmentType(IntPtr L, int index, MeteoriteWorldEffectPlayer.FragmentData.FragmentType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != MeteoriteWorldEffectPlayerFragmentDataFragmentType_TypeID)
			{
				throw new Exception("invalid userdata for MeteoriteWorldEffectPlayer.FragmentData.FragmentType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for MeteoriteWorldEffectPlayer.FragmentData.FragmentType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushWorldMeteoritePointMeteoritePointState(IntPtr L, WorldMeteoritePoint.MeteoritePointState val)
	{
		if (WorldMeteoritePointMeteoritePointState_TypeID == -1)
		{
			WorldMeteoritePointMeteoritePointState_TypeID = getTypeId(L, typeof(WorldMeteoritePoint.MeteoritePointState), out var _);
			if (WorldMeteoritePointMeteoritePointState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(WorldMeteoritePoint.MeteoritePointState));
				WorldMeteoritePointMeteoritePointState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, WorldMeteoritePointMeteoritePointState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, WorldMeteoritePointMeteoritePointState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for WorldMeteoritePoint.MeteoritePointState ,value=" + val);
			}
			Lua.lua_getref(L, WorldMeteoritePointMeteoritePointState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out WorldMeteoritePoint.MeteoritePointState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != WorldMeteoritePointMeteoritePointState_TypeID)
			{
				throw new Exception("invalid userdata for WorldMeteoritePoint.MeteoritePointState");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for WorldMeteoritePoint.MeteoritePointState");
			}
			val = (WorldMeteoritePoint.MeteoritePointState)field;
		}
		else
		{
			val = (WorldMeteoritePoint.MeteoritePointState)objectCasters.GetCaster(typeof(WorldMeteoritePoint.MeteoritePointState))(L, index, null);
		}
	}

	public void UpdateWorldMeteoritePointMeteoritePointState(IntPtr L, int index, WorldMeteoritePoint.MeteoritePointState val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != WorldMeteoritePointMeteoritePointState_TypeID)
			{
				throw new Exception("invalid userdata for WorldMeteoritePoint.MeteoritePointState");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for WorldMeteoritePoint.MeteoritePointState ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushBattleColliderUtilsColliderType(IntPtr L, BattleColliderUtils.ColliderType val)
	{
		if (BattleColliderUtilsColliderType_TypeID == -1)
		{
			BattleColliderUtilsColliderType_TypeID = getTypeId(L, typeof(BattleColliderUtils.ColliderType), out var _);
			if (BattleColliderUtilsColliderType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(BattleColliderUtils.ColliderType));
				BattleColliderUtilsColliderType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, BattleColliderUtilsColliderType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, BattleColliderUtilsColliderType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for BattleColliderUtils.ColliderType ,value=" + val);
			}
			Lua.lua_getref(L, BattleColliderUtilsColliderType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out BattleColliderUtils.ColliderType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != BattleColliderUtilsColliderType_TypeID)
			{
				throw new Exception("invalid userdata for BattleColliderUtils.ColliderType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for BattleColliderUtils.ColliderType");
			}
			val = (BattleColliderUtils.ColliderType)field;
		}
		else
		{
			val = (BattleColliderUtils.ColliderType)objectCasters.GetCaster(typeof(BattleColliderUtils.ColliderType))(L, index, null);
		}
	}

	public void UpdateBattleColliderUtilsColliderType(IntPtr L, int index, BattleColliderUtils.ColliderType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != BattleColliderUtilsColliderType_TypeID)
			{
				throw new Exception("invalid userdata for BattleColliderUtils.ColliderType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for BattleColliderUtils.ColliderType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushViewSkinProPropertyRecorderCodeType(IntPtr L, ViewSkinProPropertyRecorder.CodeType val)
	{
		if (ViewSkinProPropertyRecorderCodeType_TypeID == -1)
		{
			ViewSkinProPropertyRecorderCodeType_TypeID = getTypeId(L, typeof(ViewSkinProPropertyRecorder.CodeType), out var _);
			if (ViewSkinProPropertyRecorderCodeType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ViewSkinProPropertyRecorder.CodeType));
				ViewSkinProPropertyRecorderCodeType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ViewSkinProPropertyRecorderCodeType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ViewSkinProPropertyRecorderCodeType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ViewSkinProPropertyRecorder.CodeType ,value=" + val);
			}
			Lua.lua_getref(L, ViewSkinProPropertyRecorderCodeType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ViewSkinProPropertyRecorder.CodeType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ViewSkinProPropertyRecorderCodeType_TypeID)
			{
				throw new Exception("invalid userdata for ViewSkinProPropertyRecorder.CodeType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ViewSkinProPropertyRecorder.CodeType");
			}
			val = (ViewSkinProPropertyRecorder.CodeType)field;
		}
		else
		{
			val = (ViewSkinProPropertyRecorder.CodeType)objectCasters.GetCaster(typeof(ViewSkinProPropertyRecorder.CodeType))(L, index, null);
		}
	}

	public void UpdateViewSkinProPropertyRecorderCodeType(IntPtr L, int index, ViewSkinProPropertyRecorder.CodeType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ViewSkinProPropertyRecorderCodeType_TypeID)
			{
				throw new Exception("invalid userdata for ViewSkinProPropertyRecorder.CodeType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ViewSkinProPropertyRecorder.CodeType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushFOWSystemLOSChecks(IntPtr L, FOWSystem.LOSChecks val)
	{
		if (FOWSystemLOSChecks_TypeID == -1)
		{
			FOWSystemLOSChecks_TypeID = getTypeId(L, typeof(FOWSystem.LOSChecks), out var _);
			if (FOWSystemLOSChecks_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(FOWSystem.LOSChecks));
				FOWSystemLOSChecks_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, FOWSystemLOSChecks_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, FOWSystemLOSChecks_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for FOWSystem.LOSChecks ,value=" + val);
			}
			Lua.lua_getref(L, FOWSystemLOSChecks_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out FOWSystem.LOSChecks val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FOWSystemLOSChecks_TypeID)
			{
				throw new Exception("invalid userdata for FOWSystem.LOSChecks");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for FOWSystem.LOSChecks");
			}
			val = (FOWSystem.LOSChecks)field;
		}
		else
		{
			val = (FOWSystem.LOSChecks)objectCasters.GetCaster(typeof(FOWSystem.LOSChecks))(L, index, null);
		}
	}

	public void UpdateFOWSystemLOSChecks(IntPtr L, int index, FOWSystem.LOSChecks val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FOWSystemLOSChecks_TypeID)
			{
				throw new Exception("invalid userdata for FOWSystem.LOSChecks");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for FOWSystem.LOSChecks ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushFOWSystemState(IntPtr L, FOWSystem.State val)
	{
		if (FOWSystemState_TypeID == -1)
		{
			FOWSystemState_TypeID = getTypeId(L, typeof(FOWSystem.State), out var _);
			if (FOWSystemState_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(FOWSystem.State));
				FOWSystemState_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, FOWSystemState_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, FOWSystemState_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for FOWSystem.State ,value=" + val);
			}
			Lua.lua_getref(L, FOWSystemState_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out FOWSystem.State val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FOWSystemState_TypeID)
			{
				throw new Exception("invalid userdata for FOWSystem.State");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for FOWSystem.State");
			}
			val = (FOWSystem.State)field;
		}
		else
		{
			val = (FOWSystem.State)objectCasters.GetCaster(typeof(FOWSystem.State))(L, index, null);
		}
	}

	public void UpdateFOWSystemState(IntPtr L, int index, FOWSystem.State val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != FOWSystemState_TypeID)
			{
				throw new Exception("invalid userdata for FOWSystem.State");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for FOWSystem.State ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushObjectPoolTag(IntPtr L, ObjectPoolTag val)
	{
		if (ObjectPoolTag_TypeID == -1)
		{
			ObjectPoolTag_TypeID = getTypeId(L, typeof(ObjectPoolTag), out var _);
			if (ObjectPoolTag_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ObjectPoolTag));
				ObjectPoolTag_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ObjectPoolTag_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ObjectPoolTag_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ObjectPoolTag ,value=" + val);
			}
			Lua.lua_getref(L, ObjectPoolTag_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ObjectPoolTag val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ObjectPoolTag_TypeID)
			{
				throw new Exception("invalid userdata for ObjectPoolTag");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ObjectPoolTag");
			}
			val = (ObjectPoolTag)field;
		}
		else
		{
			val = (ObjectPoolTag)objectCasters.GetCaster(typeof(ObjectPoolTag))(L, index, null);
		}
	}

	public void UpdateObjectPoolTag(IntPtr L, int index, ObjectPoolTag val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ObjectPoolTag_TypeID)
			{
				throw new Exception("invalid userdata for ObjectPoolTag");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ObjectPoolTag ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushObjectPoolTagGroup(IntPtr L, ObjectPoolTagGroup val)
	{
		if (ObjectPoolTagGroup_TypeID == -1)
		{
			ObjectPoolTagGroup_TypeID = getTypeId(L, typeof(ObjectPoolTagGroup), out var _);
			if (ObjectPoolTagGroup_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(ObjectPoolTagGroup));
				ObjectPoolTagGroup_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, ObjectPoolTagGroup_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, ObjectPoolTagGroup_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for ObjectPoolTagGroup ,value=" + val);
			}
			Lua.lua_getref(L, ObjectPoolTagGroup_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out ObjectPoolTagGroup val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ObjectPoolTagGroup_TypeID)
			{
				throw new Exception("invalid userdata for ObjectPoolTagGroup");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for ObjectPoolTagGroup");
			}
			val = (ObjectPoolTagGroup)field;
		}
		else
		{
			val = (ObjectPoolTagGroup)objectCasters.GetCaster(typeof(ObjectPoolTagGroup))(L, index, null);
		}
	}

	public void UpdateObjectPoolTagGroup(IntPtr L, int index, ObjectPoolTagGroup val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != ObjectPoolTagGroup_TypeID)
			{
				throw new Exception("invalid userdata for ObjectPoolTagGroup");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for ObjectPoolTagGroup ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushDeviceLevel(IntPtr L, DeviceLevel val)
	{
		if (DeviceLevel_TypeID == -1)
		{
			DeviceLevel_TypeID = getTypeId(L, typeof(DeviceLevel), out var _);
			if (DeviceLevel_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(DeviceLevel));
				DeviceLevel_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, DeviceLevel_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, DeviceLevel_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for DeviceLevel ,value=" + val);
			}
			Lua.lua_getref(L, DeviceLevel_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out DeviceLevel val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DeviceLevel_TypeID)
			{
				throw new Exception("invalid userdata for DeviceLevel");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for DeviceLevel");
			}
			val = (DeviceLevel)field;
		}
		else
		{
			val = (DeviceLevel)objectCasters.GetCaster(typeof(DeviceLevel))(L, index, null);
		}
	}

	public void UpdateDeviceLevel(IntPtr L, int index, DeviceLevel val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != DeviceLevel_TypeID)
			{
				throw new Exception("invalid userdata for DeviceLevel");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for DeviceLevel ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}

	public void PushPlayerType(IntPtr L, PlayerType val)
	{
		if (PlayerType_TypeID == -1)
		{
			PlayerType_TypeID = getTypeId(L, typeof(PlayerType), out var _);
			if (PlayerType_EnumRef == -1)
			{
				Utils.LoadCSTable(L, typeof(PlayerType));
				PlayerType_EnumRef = Lua.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		if (Lua.xlua_tryget_cachedud(L, (int)val, PlayerType_EnumRef) != 1)
		{
			if (!CopyByValue.Pack(Lua.xlua_pushstruct(L, 4u, PlayerType_TypeID), 0, (int)val))
			{
				throw new Exception("pack fail fail for PlayerType ,value=" + val);
			}
			Lua.lua_getref(L, PlayerType_EnumRef);
			Lua.lua_pushvalue(L, -2);
			Lua.xlua_rawseti(L, -2, (long)val);
			Lua.lua_pop(L, 1);
		}
	}

	public void Get(IntPtr L, int index, out PlayerType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != PlayerType_TypeID)
			{
				throw new Exception("invalid userdata for PlayerType");
			}
			if (!CopyByValue.UnPack(Lua.lua_touserdata(L, index), 0, out int field))
			{
				throw new Exception("unpack fail for PlayerType");
			}
			val = (PlayerType)field;
		}
		else
		{
			val = (PlayerType)objectCasters.GetCaster(typeof(PlayerType))(L, index, null);
		}
	}

	public void UpdatePlayerType(IntPtr L, int index, PlayerType val)
	{
		if (Lua.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
		{
			if (Lua.xlua_gettypeid(L, index) != PlayerType_TypeID)
			{
				throw new Exception("invalid userdata for PlayerType");
			}
			if (!CopyByValue.Pack(Lua.lua_touserdata(L, index), 0, (int)val))
			{
				throw new Exception("pack fail for PlayerType ,value=" + val);
			}
			return;
		}
		throw new Exception("try to update a data with lua type:" + Lua.lua_type(L, index));
	}
}
