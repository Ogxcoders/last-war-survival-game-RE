using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using GameKit.Base;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua;

internal class InternalGlobals
{
	internal delegate bool TryArrayGet(Type type, IntPtr L, ObjectTranslator translator, object obj, int index);

	internal delegate bool TryArraySet(Type type, IntPtr L, ObjectTranslator translator, object obj, int array_idx, int obj_idx);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE0(CanvasGroup target, float endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE1(Graphic target, Color endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE2(Graphic target, float endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE3(Image target, Color endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE4(Image target, float endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE5(Image target, float endValue, float duration);

	private delegate Sequence __GEN_DELEGATE6(Image target, UnityEngine.Gradient gradient, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE7(LayoutElement target, Vector2 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE8(LayoutElement target, Vector2 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE9(LayoutElement target, Vector2 endValue, float duration, bool snapping);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE10(Outline target, Color endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE11(Outline target, float endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE12(Outline target, Vector2 endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE13(RectTransform target, Vector2 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE14(RectTransform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE15(RectTransform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE16(RectTransform target, Vector3 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE17(RectTransform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE18(RectTransform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE19(RectTransform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE20(RectTransform target, Vector2 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE21(RectTransform target, Vector2 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE22(RectTransform target, Vector2 endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE23(RectTransform target, float endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE24(RectTransform target, float endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE25(RectTransform target, Vector2 endValue, float duration, bool snapping);

	private delegate Tweener __GEN_DELEGATE26(RectTransform target, Vector2 punch, float duration, int vibrato, float elasticity, bool snapping);

	private delegate Tweener __GEN_DELEGATE27(RectTransform target, float duration, float strength, int vibrato, float randomness, bool snapping, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE28(RectTransform target, float duration, Vector2 strength, int vibrato, float randomness, bool snapping, bool fadeOut);

	private delegate Sequence __GEN_DELEGATE29(RectTransform target, Vector2 endValue, float jumpPower, int numJumps, float duration, bool snapping);

	private delegate Tweener __GEN_DELEGATE30(ScrollRect target, Vector2 endValue, float duration, bool snapping);

	private delegate Tweener __GEN_DELEGATE31(ScrollRect target, float endValue, float duration, bool snapping);

	private delegate Tweener __GEN_DELEGATE32(ScrollRect target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE33(Slider target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE34(Text target, Color endValue, float duration);

	private delegate TweenerCore<int, int, NoOptions> __GEN_DELEGATE35(Text target, int fromValue, int endValue, float duration, bool addThousandsSeparator, CultureInfo culture);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE36(Text target, float endValue, float duration);

	private delegate TweenerCore<string, string, StringOptions> __GEN_DELEGATE37(Text target, string endValue, float duration, bool richTextEnabled, ScrambleMode scrambleMode, string scrambleChars);

	private delegate Tweener __GEN_DELEGATE38(Graphic target, Color endValue, float duration);

	private delegate Tweener __GEN_DELEGATE39(Image target, Color endValue, float duration);

	private delegate Tweener __GEN_DELEGATE40(Text target, Color endValue, float duration);

	private delegate void __GEN_DELEGATE41(Tween t);

	private delegate void __GEN_DELEGATE42(Tween t, bool withCallbacks);

	private delegate void __GEN_DELEGATE43(Tween t);

	private delegate void __GEN_DELEGATE44(Tween t);

	private delegate void __GEN_DELEGATE45(Tween t, float to, bool andPlay);

	private delegate void __GEN_DELEGATE46(Tween t, bool complete);

	private delegate Tween __GEN_DELEGATE47(Tween t);

	private delegate Tween __GEN_DELEGATE48(Tween t);

	private delegate void __GEN_DELEGATE49(Tween t);

	private delegate void __GEN_DELEGATE50(Tween t);

	private delegate void __GEN_DELEGATE51(Tween t, bool includeDelay, float changeDelayTo);

	private delegate void __GEN_DELEGATE52(Tween t, bool includeDelay);

	private delegate void __GEN_DELEGATE53(Tween t);

	private delegate void __GEN_DELEGATE54(Tween t);

	private delegate void __GEN_DELEGATE55(Tween t, int waypointIndex, bool andPlay);

	private delegate YieldInstruction __GEN_DELEGATE56(Tween t);

	private delegate YieldInstruction __GEN_DELEGATE57(Tween t);

	private delegate YieldInstruction __GEN_DELEGATE58(Tween t);

	private delegate YieldInstruction __GEN_DELEGATE59(Tween t, int elapsedLoops);

	private delegate YieldInstruction __GEN_DELEGATE60(Tween t, float position);

	private delegate Coroutine __GEN_DELEGATE61(Tween t);

	private delegate int __GEN_DELEGATE62(Tween t);

	private delegate float __GEN_DELEGATE63(Tween t);

	private delegate float __GEN_DELEGATE64(Tween t);

	private delegate float __GEN_DELEGATE65(Tween t, bool includeLoops);

	private delegate float __GEN_DELEGATE66(Tween t, bool includeLoops);

	private delegate float __GEN_DELEGATE67(Tween t, bool includeLoops);

	private delegate float __GEN_DELEGATE68(Tween t);

	private delegate bool __GEN_DELEGATE69(Tween t);

	private delegate bool __GEN_DELEGATE70(Tween t);

	private delegate bool __GEN_DELEGATE71(Tween t);

	private delegate bool __GEN_DELEGATE72(Tween t);

	private delegate bool __GEN_DELEGATE73(Tween t);

	private delegate int __GEN_DELEGATE74(Tween t);

	private delegate Vector3 __GEN_DELEGATE75(Tween t, float pathPercentage);

	private delegate Vector3[] __GEN_DELEGATE76(Tween t, int subdivisionsXSegment);

	private delegate float __GEN_DELEGATE77(Tween t);

	private delegate Tween __GEN_DELEGATE78(Tween t);

	private delegate Tween __GEN_DELEGATE79(Tween t, bool autoKillOnCompletion);

	private delegate Tween __GEN_DELEGATE80(Tween t, object objectId);

	private delegate Tween __GEN_DELEGATE81(Tween t, string stringId);

	private delegate Tween __GEN_DELEGATE82(Tween t, int intId);

	private delegate Tween __GEN_DELEGATE83(Tween t, GameObject gameObject);

	private delegate Tween __GEN_DELEGATE84(Tween t, GameObject gameObject, LinkBehaviour behaviour);

	private delegate Tween __GEN_DELEGATE85(Tween t, object target);

	private delegate Tween __GEN_DELEGATE86(Tween t, int loops);

	private delegate Tween __GEN_DELEGATE87(Tween t, int loops, LoopType loopType);

	private delegate Tween __GEN_DELEGATE88(Tween t, Ease ease);

	private delegate Tween __GEN_DELEGATE89(Tween t, Ease ease, float overshoot);

	private delegate Tween __GEN_DELEGATE90(Tween t, Ease ease, float amplitude, float period);

	private delegate Tween __GEN_DELEGATE91(Tween t, AnimationCurve animCurve);

	private delegate Tween __GEN_DELEGATE92(Tween t, EaseFunction customEase);

	private delegate Tween __GEN_DELEGATE93(Tween t);

	private delegate Tween __GEN_DELEGATE94(Tween t, bool recyclable);

	private delegate Tween __GEN_DELEGATE95(Tween t, bool isIndependentUpdate);

	private delegate Tween __GEN_DELEGATE96(Tween t, UpdateType updateType);

	private delegate Tween __GEN_DELEGATE97(Tween t, UpdateType updateType, bool isIndependentUpdate);

	private delegate Tween __GEN_DELEGATE98(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE99(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE100(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE101(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE102(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE103(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE104(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE105(Tween t, TweenCallback action);

	private delegate Tween __GEN_DELEGATE106(Tween t, TweenCallback<int> action);

	private delegate Tween __GEN_DELEGATE107(Tween t, Tween asTween);

	private delegate Tween __GEN_DELEGATE108(Tween t, TweenParams tweenParams);

	private delegate Sequence __GEN_DELEGATE109(Sequence s, Tween t);

	private delegate Sequence __GEN_DELEGATE110(Sequence s, Tween t);

	private delegate Sequence __GEN_DELEGATE111(Sequence s, Tween t);

	private delegate Sequence __GEN_DELEGATE112(Sequence s, float atPosition, Tween t);

	private delegate Sequence __GEN_DELEGATE113(Sequence s, float interval);

	private delegate Sequence __GEN_DELEGATE114(Sequence s, float interval);

	private delegate Sequence __GEN_DELEGATE115(Sequence s, TweenCallback callback);

	private delegate Sequence __GEN_DELEGATE116(Sequence s, TweenCallback callback);

	private delegate Sequence __GEN_DELEGATE117(Sequence s, float atPosition, TweenCallback callback);

	private delegate Tweener __GEN_DELEGATE118(Tweener t);

	private delegate Tweener __GEN_DELEGATE119(Tweener t, bool isRelative);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE120(TweenerCore<Color, Color, ColorOptions> t, float fromAlphaValue, bool setImmediately, bool isRelative);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE121(TweenerCore<Vector3, Vector3, VectorOptions> t, float fromValue, bool setImmediately, bool isRelative);

	private delegate Tween __GEN_DELEGATE122(Tween t, float delay);

	private delegate Tween __GEN_DELEGATE123(Tween t, float delay, bool asPrependedIntervalIfSequence);

	private delegate Tween __GEN_DELEGATE124(Tween t);

	private delegate Tween __GEN_DELEGATE125(Tween t, bool isRelative);

	private delegate Tween __GEN_DELEGATE126(Tween t);

	private delegate Tween __GEN_DELEGATE127(Tween t, bool isSpeedBased);

	private delegate Tweener __GEN_DELEGATE128(TweenerCore<float, float, FloatOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE129(TweenerCore<Vector2, Vector2, VectorOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE130(TweenerCore<Vector2, Vector2, VectorOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate Tweener __GEN_DELEGATE131(TweenerCore<Vector3, Vector3, VectorOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE132(TweenerCore<Vector3, Vector3, VectorOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate Tweener __GEN_DELEGATE133(TweenerCore<Vector4, Vector4, VectorOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE134(TweenerCore<Vector4, Vector4, VectorOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate Tweener __GEN_DELEGATE135(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool useShortest360Route);

	private delegate Tweener __GEN_DELEGATE136(TweenerCore<Color, Color, ColorOptions> t, bool alphaOnly);

	private delegate Tweener __GEN_DELEGATE137(TweenerCore<Rect, Rect, RectOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE138(TweenerCore<string, string, StringOptions> t, bool richTextEnabled, ScrambleMode scrambleMode, string scrambleChars);

	private delegate Tweener __GEN_DELEGATE139(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE140(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE141(TweenerCore<Vector3, Path, PathOptions> t, AxisConstraint lockPosition, AxisConstraint lockRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE142(TweenerCore<Vector3, Path, PathOptions> t, bool closePath, AxisConstraint lockPosition, AxisConstraint lockRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE143(TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, Vector3? forwardDirection, Vector3? up);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE144(TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, bool stableZRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE145(TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, Vector3? forwardDirection, Vector3? up);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE146(TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, bool stableZRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE147(TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, Vector3? forwardDirection, Vector3? up);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE148(TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, bool stableZRotation);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE149(Camera target, float endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE150(Camera target, Color endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE151(Camera target, float endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE152(Camera target, float endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE153(Camera target, float endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE154(Camera target, float endValue, float duration);

	private delegate TweenerCore<Rect, Rect, RectOptions> __GEN_DELEGATE155(Camera target, Rect endValue, float duration);

	private delegate TweenerCore<Rect, Rect, RectOptions> __GEN_DELEGATE156(Camera target, Rect endValue, float duration);

	private delegate Tweener __GEN_DELEGATE157(Camera target, float duration, float strength, int vibrato, float randomness, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE158(Camera target, float duration, Vector3 strength, int vibrato, float randomness, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE159(Camera target, float duration, float strength, int vibrato, float randomness, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE160(Camera target, float duration, Vector3 strength, int vibrato, float randomness, bool fadeOut);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE161(Light target, Color endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE162(Light target, float endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE163(Light target, float endValue, float duration);

	private delegate Tweener __GEN_DELEGATE164(LineRenderer target, Color2 startValue, Color2 endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE165(Material target, Color endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE166(Material target, Color endValue, string property, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE167(Material target, Color endValue, int propertyID, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE168(Material target, float endValue, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE169(Material target, float endValue, string property, float duration);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE170(Material target, float endValue, int propertyID, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE171(Material target, float endValue, string property, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE172(Material target, float endValue, int propertyID, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE173(Material target, Vector2 endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE174(Material target, Vector2 endValue, string property, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE175(Material target, Vector2 endValue, float duration);

	private delegate TweenerCore<Vector2, Vector2, VectorOptions> __GEN_DELEGATE176(Material target, Vector2 endValue, string property, float duration);

	private delegate TweenerCore<Vector4, Vector4, VectorOptions> __GEN_DELEGATE177(Material target, Vector4 endValue, string property, float duration);

	private delegate TweenerCore<Vector4, Vector4, VectorOptions> __GEN_DELEGATE178(Material target, Vector4 endValue, int propertyID, float duration);

	private delegate Tweener __GEN_DELEGATE179(TrailRenderer target, float toStartWidth, float toEndWidth, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE180(TrailRenderer target, float endValue, float duration);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE181(Transform target, Vector3 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE182(Transform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE183(Transform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE184(Transform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE185(Transform target, Vector3 endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE186(Transform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE187(Transform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE188(Transform target, float endValue, float duration, bool snapping);

	private delegate TweenerCore<Quaternion, Vector3, QuaternionOptions> __GEN_DELEGATE189(Transform target, Vector3 endValue, float duration, RotateMode mode);

	private delegate TweenerCore<Quaternion, Quaternion, NoOptions> __GEN_DELEGATE190(Transform target, Quaternion endValue, float duration);

	private delegate TweenerCore<Quaternion, Vector3, QuaternionOptions> __GEN_DELEGATE191(Transform target, Vector3 endValue, float duration, RotateMode mode);

	private delegate TweenerCore<Quaternion, Quaternion, NoOptions> __GEN_DELEGATE192(Transform target, Quaternion endValue, float duration);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE193(Transform target, Vector3 endValue, float duration);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE194(Transform target, float endValue, float duration);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE195(Transform target, float endValue, float duration);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE196(Transform target, float endValue, float duration);

	private delegate TweenerCore<Vector3, Vector3, VectorOptions> __GEN_DELEGATE197(Transform target, float endValue, float duration);

	private delegate Tweener __GEN_DELEGATE198(Transform target, Vector3 towards, float duration, AxisConstraint axisConstraint, Vector3? up);

	private delegate Tweener __GEN_DELEGATE199(Transform target, Vector3 punch, float duration, int vibrato, float elasticity, bool snapping);

	private delegate Tweener __GEN_DELEGATE200(Transform target, Vector3 punch, float duration, int vibrato, float elasticity);

	private delegate Tweener __GEN_DELEGATE201(Transform target, Vector3 punch, float duration, int vibrato, float elasticity);

	private delegate Tweener __GEN_DELEGATE202(Transform target, float duration, float strength, int vibrato, float randomness, bool snapping, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE203(Transform target, float duration, Vector3 strength, int vibrato, float randomness, bool snapping, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE204(Transform target, float duration, float strength, int vibrato, float randomness, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE205(Transform target, float duration, Vector3 strength, int vibrato, float randomness, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE206(Transform target, float duration, float strength, int vibrato, float randomness, bool fadeOut);

	private delegate Tweener __GEN_DELEGATE207(Transform target, float duration, Vector3 strength, int vibrato, float randomness, bool fadeOut);

	private delegate Sequence __GEN_DELEGATE208(Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping);

	private delegate Sequence __GEN_DELEGATE209(Transform target, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE210(Transform target, Vector3[] path, float duration, PathType pathType, PathMode pathMode, int resolution, Color? gizmoColor);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE211(Transform target, Vector3[] path, float duration, PathType pathType, PathMode pathMode, int resolution, Color? gizmoColor);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE212(Transform target, Path path, float duration, PathMode pathMode);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE213(Transform target, Path path, float duration, PathMode pathMode);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE214(Tween target, float endValue, float duration);

	private delegate Tweener __GEN_DELEGATE215(Light target, Color endValue, float duration);

	private delegate Tweener __GEN_DELEGATE216(Material target, Color endValue, float duration);

	private delegate Tweener __GEN_DELEGATE217(Material target, Color endValue, string property, float duration);

	private delegate Tweener __GEN_DELEGATE218(Material target, Color endValue, int propertyID, float duration);

	private delegate Tweener __GEN_DELEGATE219(Transform target, Vector3 byValue, float duration, bool snapping);

	private delegate Tweener __GEN_DELEGATE220(Transform target, Vector3 byValue, float duration, bool snapping);

	private delegate Tweener __GEN_DELEGATE221(Transform target, Vector3 byValue, float duration, RotateMode mode);

	private delegate Tweener __GEN_DELEGATE222(Transform target, Vector3 byValue, float duration, RotateMode mode);

	private delegate Tweener __GEN_DELEGATE223(Transform target, Vector3 punch, float duration, int vibrato, float elasticity);

	private delegate Tweener __GEN_DELEGATE224(Transform target, Vector3 byValue, float duration);

	private delegate int __GEN_DELEGATE225(Component target, bool withCallbacks);

	private delegate int __GEN_DELEGATE226(Material target, bool withCallbacks);

	private delegate int __GEN_DELEGATE227(Component target, bool complete);

	private delegate int __GEN_DELEGATE228(Material target, bool complete);

	private delegate int __GEN_DELEGATE229(Component target);

	private delegate int __GEN_DELEGATE230(Material target);

	private delegate int __GEN_DELEGATE231(Component target, float to, bool andPlay);

	private delegate int __GEN_DELEGATE232(Material target, float to, bool andPlay);

	private delegate int __GEN_DELEGATE233(Component target);

	private delegate int __GEN_DELEGATE234(Material target);

	private delegate int __GEN_DELEGATE235(Component target);

	private delegate int __GEN_DELEGATE236(Material target);

	private delegate int __GEN_DELEGATE237(Component target);

	private delegate int __GEN_DELEGATE238(Material target);

	private delegate int __GEN_DELEGATE239(Component target);

	private delegate int __GEN_DELEGATE240(Material target);

	private delegate int __GEN_DELEGATE241(Component target, bool includeDelay);

	private delegate int __GEN_DELEGATE242(Material target, bool includeDelay);

	private delegate int __GEN_DELEGATE243(Component target, bool includeDelay);

	private delegate int __GEN_DELEGATE244(Material target, bool includeDelay);

	private delegate int __GEN_DELEGATE245(Component target);

	private delegate int __GEN_DELEGATE246(Material target);

	private delegate int __GEN_DELEGATE247(Component target);

	private delegate int __GEN_DELEGATE248(Material target);

	private delegate Tween __GEN_DELEGATE249(Tween t, SpecialStartupMode mode);

	private delegate bool __GEN_DELEGATE250(UnityEngine.Object o);

	private delegate void __GEN_DELEGATE251(GameObject prefab);

	private delegate GameObject __GEN_DELEGATE252(GameObject prefab);

	private delegate GameObject __GEN_DELEGATE253(GameObject prefab, Transform parent);

	private delegate void __GEN_DELEGATE254(GameObject obj);

	private delegate void __GEN_DELEGATE255(GameObject prefab);

	private delegate void __GEN_DELEGATE256(GameObject prefab);

	private delegate int __GEN_DELEGATE257(GameObject prefab);

	private delegate GameObject __GEN_DELEGATE258(GameObject obj);

	private delegate Component __GEN_DELEGATE259(Component comp, Type type, bool set_enable);

	private delegate Component __GEN_DELEGATE260(GameObject go, Type type, bool set_enable);

	private delegate void __GEN_DELEGATE261(ScrollRect scrollRect);

	private delegate void __GEN_DELEGATE262(ScrollView scrollView);

	private delegate Transform __GEN_DELEGATE263(Transform parent, string childName, Vector3 position, Vector3 roll);

	private delegate Transform __GEN_DELEGATE264(Transform parent, string childName, Vector3 position, Quaternion rotation);

	private delegate Transform __GEN_DELEGATE265(Transform parent, string childName);

	private delegate List<string> __GEN_DELEGATE266(string str, char splitChar);

	private delegate List<int> __GEN_DELEGATE267(string str, char splitChar);

	private delegate int __GEN_DELEGATE268(string str);

	private delegate int __GEN_DELEGATE269(object obj);

	private delegate int __GEN_DELEGATE270(ReadOnlySpan<char> str);

	private delegate float __GEN_DELEGATE271(string str);

	private delegate float __GEN_DELEGATE272(ReadOnlySpan<char> str);

	private delegate ulong __GEN_DELEGATE273(ReadOnlySpan<char> str);

	private delegate float __GEN_DELEGATE274(object obj);

	private delegate long __GEN_DELEGATE275(string str);

	private delegate GameObject __GEN_DELEGATE276(GameObject go);

	private delegate void __GEN_DELEGATE277(GameObject go);

	private delegate bool __GEN_DELEGATE278(GameObject gameObject);

	private delegate void __GEN_DELEGATE279(GameObject gameObject, int layer);

	private delegate Vector2 __GEN_DELEGATE280(Vector3 vector3);

	private delegate Vector3 __GEN_DELEGATE281(Vector2 vector2);

	private delegate void __GEN_DELEGATE282(GameObject obj);

	private delegate void __GEN_DELEGATE283(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE284(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE285(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE286(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE287(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE288(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE289(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE290(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE291(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE292(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE293(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE294(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE295(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE296(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE297(Transform transform, float newValue);

	private delegate void __GEN_DELEGATE298(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE299(Transform transform, float deltaValue);

	private delegate void __GEN_DELEGATE300(Transform transform, float deltaValue);

	private delegate Transform __GEN_DELEGATE301(Transform parent, string name, int maxFirstLevelChildren, int maxDepth);

	private delegate Component __GEN_DELEGATE302(Transform tran, string path, Type type);

	private delegate Component __GEN_DELEGATE303(Transform tran);

	private delegate Component __GEN_DELEGATE304(GameObject obj);

	private delegate Component __GEN_DELEGATE305(Transform tran);

	private delegate Component __GEN_DELEGATE306(GameObject obj);

	private delegate Component __GEN_DELEGATE307(Transform tran);

	private delegate Component __GEN_DELEGATE308(GameObject obj);

	private delegate Component __GEN_DELEGATE309(Transform tran);

	private delegate Component __GEN_DELEGATE310(GameObject obj);

	private delegate void __GEN_DELEGATE311(Text text, long leftMilliSecond);

	private delegate bool __GEN_DELEGATE312(SimpleAnimation ani, int stateNameToId);

	private delegate void __GEN_DELEGATE313(SimpleAnimation simpleAni, int stateNameToId);

	private delegate void __GEN_DELEGATE314(Animator ani, int stateNameToId, int layerIdx, float normalizedTime);

	private delegate void __GEN_DELEGATE315(Animator ani, int triggerNameToId);

	private delegate Transform __GEN_DELEGATE316(Transform tran, int pathToId);

	private delegate void __GEN_DELEGATE317(Text obj);

	private delegate void __GEN_DELEGATE318(TextMeshProUGUI obj);

	private delegate void __GEN_DELEGATE319(InputField obj);

	private delegate void __GEN_DELEGATE320(TMP_InputField obj);

	private delegate void __GEN_DELEGATE321(SuperTextMesh obj);

	private delegate void __GEN_DELEGATE322(UnityEventBase ev);

	private delegate void __GEN_DELEGATE323(Tween tween, TweenCallback action);

	private delegate void __GEN_DELEGATE324(Image image, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE325(Image image, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE326(Image image, string spritePath, Action<Sprite> completeCallback, string defaultSprite);

	private delegate void __GEN_DELEGATE327(CircleImage image, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE328(SpriteRenderer spriteRenderer, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE329(SpriteRenderer spriteRenderer, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE330(SpriteRenderer spriteRenderer, string spritePath, Action<Sprite> completeCallback, string defaultSprite);

	private delegate void __GEN_DELEGATE331(CircleMesh circleMesh, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE332(CircleMeshInstanced circleMesh, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE333(SpriteMeshRenderer meshRenderer, string spritePath, string defaultSprite, bool isAsync);

	private delegate void __GEN_DELEGATE334(RawImage image, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE335(RawImage image, string spritePath, string defaultSprite);

	private delegate void __GEN_DELEGATE336(RawImage image, string spritePath, Action<Texture> completeCallback, string defaultSprite);

	private delegate void __GEN_DELEGATE337(GameObject obj, bool active);

	private delegate SpriteMeshRenderer __GEN_DELEGATE338(SpriteRenderer spriteRenderer);

	private delegate void __GEN_DELEGATE339(Image image, float alpha);

	private delegate void __GEN_DELEGATE340(ScrollRect scrollRect, float ratio);

	private delegate float __GEN_DELEGATE341(ScrollRect scrollRect);

	private delegate bool __GEN_DELEGATE342(string str);

	private delegate bool __GEN_DELEGATE343(string value);

	private delegate string __GEN_DELEGATE344(string str);

	private delegate string __GEN_DELEGATE345(string path, char separator);

	private delegate string __GEN_DELEGATE346(string path, char separator);

	private delegate int __GEN_DELEGATE347(string sourceStr);

	private delegate string __GEN_DELEGATE348(string sourceStr, int len);

	private delegate void __GEN_DELEGATE349(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE350(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE351(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE352(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE353(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE354(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE355(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE356(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE357(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE358(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE359(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE360(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE361(RectTransform rt, float x, float y);

	private delegate void __GEN_DELEGATE362(RectTransform rt, float x);

	private delegate void __GEN_DELEGATE363(RectTransform rt, float y);

	private delegate void __GEN_DELEGATE364(RectTransform rt, out float x, out float y);

	private delegate void __GEN_DELEGATE365(RectTransform rt, out float x);

	private delegate void __GEN_DELEGATE366(RectTransform rt, out float y);

	private delegate void __GEN_DELEGATE367(RectTransform rt, out float minX, out float maxX);

	private delegate void __GEN_DELEGATE368(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE369(Transform t, float x);

	private delegate void __GEN_DELEGATE370(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE371(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE372(Transform rt, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE373(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE374(Transform rt, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE375(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE376(Transform rt, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE377(Transform rt, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE378(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE379(Transform rt, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE380(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE381(Transform rt, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE382(Transform t, out float x, out float y, out float z, out float w);

	private delegate void __GEN_DELEGATE383(Transform t, float x, float y, float z, float w);

	private delegate void __GEN_DELEGATE384(Transform t, out float x, out float y, out float z, out float w);

	private delegate void __GEN_DELEGATE385(Transform t, float x, float y, float z, float w);

	private delegate void __GEN_DELEGATE386(Transform t, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE387(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE388(Transform t, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE389(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE390(Transform t, out float x, out float y, out float z);

	private delegate void __GEN_DELEGATE391(Transform t, float x, float y, float z);

	private delegate void __GEN_DELEGATE392(Transform t);

	private delegate void __GEN_DELEGATE393(Graphic graphic, float r, float g, float b, float a);

	private delegate void __GEN_DELEGATE394(Graphic graphic, float r);

	private delegate void __GEN_DELEGATE395(Graphic graphic, float g);

	private delegate void __GEN_DELEGATE396(Graphic graphic, float b);

	private delegate void __GEN_DELEGATE397(Graphic graphic, float a);

	private delegate void __GEN_DELEGATE398(Graphic graphic, out float r, out float g, out float b, out float a);

	private delegate void __GEN_DELEGATE399(Graphic graphic, out float r);

	private delegate void __GEN_DELEGATE400(Graphic graphic, out float g);

	private delegate void __GEN_DELEGATE401(Graphic graphic, out float b);

	private delegate void __GEN_DELEGATE402(Graphic graphic, out float a);

	private delegate void __GEN_DELEGATE403(SpriteRenderer r, float x, float y);

	private delegate void __GEN_DELEGATE404(SpriteRenderer r, out float x, out float y);

	private delegate void __GEN_DELEGATE405(SpriteRenderer sr, float r, float g, float b, float a);

	private delegate void __GEN_DELEGATE406(SpriteRenderer sr, float r);

	private delegate void __GEN_DELEGATE407(SpriteRenderer sr, float g);

	private delegate void __GEN_DELEGATE408(SpriteRenderer sr, float b);

	private delegate void __GEN_DELEGATE409(SpriteRenderer sr, float a);

	private delegate void __GEN_DELEGATE410(SpriteMeshRenderer sr, float a);

	private delegate void __GEN_DELEGATE411(SpriteRenderer sr, out float r, out float g, out float b, out float a);

	private delegate void __GEN_DELEGATE412(SpriteRenderer sr, out float r);

	private delegate void __GEN_DELEGATE413(SpriteRenderer sr, out float g);

	private delegate void __GEN_DELEGATE414(SpriteRenderer sr, out float b);

	private delegate void __GEN_DELEGATE415(SpriteRenderer sr, out float a);

	private delegate void __GEN_DELEGATE416(Shadow shadow, float r, float g, float b, float a);

	private delegate void __GEN_DELEGATE417(TextMeshProUGUI text, string value);

	private delegate void __GEN_DELEGATE418(TextMeshProUGUI text, string value);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE419(TweenerCore<Color, Color, ColorOptions> t, float fromAlphaValue, bool setImmediately, bool isRelative);

	private delegate Tweener __GEN_DELEGATE420(TweenerCore<float, float, FloatOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE421(TweenerCore<Vector2, Vector2, VectorOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE422(TweenerCore<Vector2, Vector2, VectorOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate Tweener __GEN_DELEGATE423(TweenerCore<Vector4, Vector4, VectorOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE424(TweenerCore<Vector4, Vector4, VectorOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate Tweener __GEN_DELEGATE425(TweenerCore<Quaternion, Vector3, QuaternionOptions> t, bool useShortest360Route);

	private delegate Tweener __GEN_DELEGATE426(TweenerCore<Color, Color, ColorOptions> t, bool alphaOnly);

	private delegate Tweener __GEN_DELEGATE427(TweenerCore<Rect, Rect, RectOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE428(TweenerCore<string, string, StringOptions> t, bool richTextEnabled, ScrambleMode scrambleMode, string scrambleChars);

	private delegate Tweener __GEN_DELEGATE429(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, bool snapping);

	private delegate Tweener __GEN_DELEGATE430(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> t, AxisConstraint axisConstraint, bool snapping);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE431(TweenerCore<Vector3, Path, PathOptions> t, AxisConstraint lockPosition, AxisConstraint lockRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE432(TweenerCore<Vector3, Path, PathOptions> t, bool closePath, AxisConstraint lockPosition, AxisConstraint lockRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE433(TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, Vector3? forwardDirection, Vector3? up);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE434(TweenerCore<Vector3, Path, PathOptions> t, Vector3 lookAtPosition, bool stableZRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE435(TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, Vector3? forwardDirection, Vector3? up);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE436(TweenerCore<Vector3, Path, PathOptions> t, Transform lookAtTransform, bool stableZRotation);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE437(TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, Vector3? forwardDirection, Vector3? up);

	private delegate TweenerCore<Vector3, Path, PathOptions> __GEN_DELEGATE438(TweenerCore<Vector3, Path, PathOptions> t, float lookAhead, bool stableZRotation);

	private delegate TweenerCore<Color, Color, ColorOptions> __GEN_DELEGATE439(Light target, Color endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE440(Light target, float endValue, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE441(Light target, float endValue, float duration);

	private delegate Tweener __GEN_DELEGATE442(TrailRenderer target, float toStartWidth, float toEndWidth, float duration);

	private delegate TweenerCore<float, float, FloatOptions> __GEN_DELEGATE443(TrailRenderer target, float endValue, float duration);

	private delegate Tweener __GEN_DELEGATE444(Light target, Color endValue, float duration);

	private delegate List<string> __GEN_DELEGATE445(string str, char splitChar);

	private delegate List<int> __GEN_DELEGATE446(string str, char splitChar);

	private delegate int __GEN_DELEGATE447(string str);

	private delegate int __GEN_DELEGATE448(ReadOnlySpan<char> str);

	private delegate float __GEN_DELEGATE449(string str);

	private delegate float __GEN_DELEGATE450(ReadOnlySpan<char> str);

	private delegate ulong __GEN_DELEGATE451(ReadOnlySpan<char> str);

	private delegate long __GEN_DELEGATE452(string str);

	private delegate void __GEN_DELEGATE453(CircleMeshInstanced circleMesh, string spritePath, string defaultSprite);

	private delegate bool __GEN_DELEGATE454(string str);

	private delegate bool __GEN_DELEGATE455(string value);

	private delegate string __GEN_DELEGATE456(string str);

	private delegate string __GEN_DELEGATE457(string path, char separator);

	private delegate string __GEN_DELEGATE458(string path, char separator);

	private delegate int __GEN_DELEGATE459(string sourceStr);

	private delegate string __GEN_DELEGATE460(string sourceStr, int len);

	internal static byte[] strBuff;

	internal static volatile TryArrayGet genTryArrayGetPtr;

	internal static volatile TryArraySet genTryArraySetPtr;

	internal static volatile ObjectTranslatorPool objectTranslatorPool;

	internal static volatile int LUA_REGISTRYINDEX;

	internal static volatile Dictionary<string, string> supportOp;

	internal static volatile Dictionary<Type, IEnumerable<MethodInfo>> extensionMethodMap;

	internal static volatile lua_CSFunction LazyReflectionWrap;

	static InternalGlobals()
	{
		strBuff = new byte[256];
		genTryArrayGetPtr = null;
		genTryArraySetPtr = null;
		objectTranslatorPool = new ObjectTranslatorPool();
		LUA_REGISTRYINDEX = -10000;
		supportOp = new Dictionary<string, string>
		{
			{ "op_Addition", "__add" },
			{ "op_Subtraction", "__sub" },
			{ "op_Multiply", "__mul" },
			{ "op_Division", "__div" },
			{ "op_Equality", "__eq" },
			{ "op_UnaryNegation", "__unm" },
			{ "op_LessThan", "__lt" },
			{ "op_LessThanOrEqual", "__le" },
			{ "op_Modulus", "__mod" },
			{ "op_BitwiseAnd", "__band" },
			{ "op_BitwiseOr", "__bor" },
			{ "op_ExclusiveOr", "__bxor" },
			{ "op_OnesComplement", "__bnot" },
			{ "op_LeftShift", "__shl" },
			{ "op_RightShift", "__shr" }
		};
		extensionMethodMap = null;
		LazyReflectionWrap = Utils.LazyReflectionCall;
		extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>
		{
			{
				typeof(CanvasGroup),
				new List<MethodInfo> { new __GEN_DELEGATE0(DOTweenModuleUI.DOFade).Method }
			},
			{
				typeof(Graphic),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE1(DOTweenModuleUI.DOColor).Method,
					new __GEN_DELEGATE2(DOTweenModuleUI.DOFade).Method,
					new __GEN_DELEGATE38(DOTweenModuleUI.DOBlendableColor).Method,
					new __GEN_DELEGATE393(xLuaOptiUtils.Set_color).Method,
					new __GEN_DELEGATE394(xLuaOptiUtils.Set_color_r).Method,
					new __GEN_DELEGATE395(xLuaOptiUtils.Set_color_g).Method,
					new __GEN_DELEGATE396(xLuaOptiUtils.Set_color_b).Method,
					new __GEN_DELEGATE397(xLuaOptiUtils.Set_color_a).Method,
					new __GEN_DELEGATE398(xLuaOptiUtils.Get_color).Method,
					new __GEN_DELEGATE399(xLuaOptiUtils.Get_color_r).Method,
					new __GEN_DELEGATE400(xLuaOptiUtils.Get_color_g).Method,
					new __GEN_DELEGATE401(xLuaOptiUtils.Get_color_b).Method,
					new __GEN_DELEGATE402(xLuaOptiUtils.Get_color_a).Method
				}
			},
			{
				typeof(Image),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE3(DOTweenModuleUI.DOColor).Method,
					new __GEN_DELEGATE4(DOTweenModuleUI.DOFade).Method,
					new __GEN_DELEGATE5(DOTweenModuleUI.DOFillAmount).Method,
					new __GEN_DELEGATE6(DOTweenModuleUI.DOGradientColor).Method,
					new __GEN_DELEGATE39(DOTweenModuleUI.DOBlendableColor).Method,
					new __GEN_DELEGATE324(UnityUIExtension.LoadSprite).Method,
					new __GEN_DELEGATE325(UnityUIExtension.LoadSpriteAsync).Method,
					new __GEN_DELEGATE326(UnityUIExtension.LoadSpriteAsync).Method,
					new __GEN_DELEGATE339(UnityUIExtension.SetAlpha).Method
				}
			},
			{
				typeof(LayoutElement),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE7(DOTweenModuleUI.DOFlexibleSize).Method,
					new __GEN_DELEGATE8(DOTweenModuleUI.DOMinSize).Method,
					new __GEN_DELEGATE9(DOTweenModuleUI.DOPreferredSize).Method
				}
			},
			{
				typeof(Outline),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE10(DOTweenModuleUI.DOColor).Method,
					new __GEN_DELEGATE11(DOTweenModuleUI.DOFade).Method,
					new __GEN_DELEGATE12(DOTweenModuleUI.DOScale).Method
				}
			},
			{
				typeof(RectTransform),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE13(DOTweenModuleUI.DOAnchorPos).Method,
					new __GEN_DELEGATE14(DOTweenModuleUI.DOAnchorPosX).Method,
					new __GEN_DELEGATE15(DOTweenModuleUI.DOAnchorPosY).Method,
					new __GEN_DELEGATE16(DOTweenModuleUI.DOAnchorPos3D).Method,
					new __GEN_DELEGATE17(DOTweenModuleUI.DOAnchorPos3DX).Method,
					new __GEN_DELEGATE18(DOTweenModuleUI.DOAnchorPos3DY).Method,
					new __GEN_DELEGATE19(DOTweenModuleUI.DOAnchorPos3DZ).Method,
					new __GEN_DELEGATE20(DOTweenModuleUI.DOAnchorMax).Method,
					new __GEN_DELEGATE21(DOTweenModuleUI.DOAnchorMin).Method,
					new __GEN_DELEGATE22(DOTweenModuleUI.DOPivot).Method,
					new __GEN_DELEGATE23(DOTweenModuleUI.DOPivotX).Method,
					new __GEN_DELEGATE24(DOTweenModuleUI.DOPivotY).Method,
					new __GEN_DELEGATE25(DOTweenModuleUI.DOSizeDelta).Method,
					new __GEN_DELEGATE26(DOTweenModuleUI.DOPunchAnchorPos).Method,
					new __GEN_DELEGATE27(DOTweenModuleUI.DOShakeAnchorPos).Method,
					new __GEN_DELEGATE28(DOTweenModuleUI.DOShakeAnchorPos).Method,
					new __GEN_DELEGATE29(DOTweenModuleUI.DOJumpAnchorPos).Method,
					new __GEN_DELEGATE349(xLuaOptiUtils.Set_offsetMax).Method,
					new __GEN_DELEGATE350(xLuaOptiUtils.Get_offsetMax).Method,
					new __GEN_DELEGATE351(xLuaOptiUtils.Set_offsetMin).Method,
					new __GEN_DELEGATE352(xLuaOptiUtils.Get_offsetMin).Method,
					new __GEN_DELEGATE353(xLuaOptiUtils.Set_anchorMin).Method,
					new __GEN_DELEGATE354(xLuaOptiUtils.Get_anchorMin).Method,
					new __GEN_DELEGATE355(xLuaOptiUtils.Set_anchorMax).Method,
					new __GEN_DELEGATE356(xLuaOptiUtils.Get_anchorMax).Method,
					new __GEN_DELEGATE357(xLuaOptiUtils.Set_anchoredPosition).Method,
					new __GEN_DELEGATE358(xLuaOptiUtils.Get_anchoredPosition).Method,
					new __GEN_DELEGATE359(xLuaOptiUtils.Set_pivot).Method,
					new __GEN_DELEGATE360(xLuaOptiUtils.Get_pivot).Method,
					new __GEN_DELEGATE361(xLuaOptiUtils.Set_sizeDelta).Method,
					new __GEN_DELEGATE362(xLuaOptiUtils.Set_sizeDelta_x).Method,
					new __GEN_DELEGATE363(xLuaOptiUtils.Set_sizeDelta_y).Method,
					new __GEN_DELEGATE364(xLuaOptiUtils.Get_sizeDelta).Method,
					new __GEN_DELEGATE365(xLuaOptiUtils.Get_sizeDelta_x).Method,
					new __GEN_DELEGATE366(xLuaOptiUtils.Get_sizeDelta_y).Method,
					new __GEN_DELEGATE367(xLuaOptiUtils.Get_worldCorners_x).Method
				}
			},
			{
				typeof(ScrollRect),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE30(DOTweenModuleUI.DONormalizedPos).Method,
					new __GEN_DELEGATE31(DOTweenModuleUI.DOHorizontalNormalizedPos).Method,
					new __GEN_DELEGATE32(DOTweenModuleUI.DOVerticalNormalizedPos).Method,
					new __GEN_DELEGATE261(ComponentExtensions.ScrollRect_EndDrag).Method,
					new __GEN_DELEGATE340(UnityUIExtension.SetHorizontalNormalizedPosition).Method,
					new __GEN_DELEGATE341(UnityUIExtension.GetHorizontalNormalizedPosition).Method
				}
			},
			{
				typeof(Slider),
				new List<MethodInfo> { new __GEN_DELEGATE33(DOTweenModuleUI.DOValue).Method }
			},
			{
				typeof(Text),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE34(DOTweenModuleUI.DOColor).Method,
					new __GEN_DELEGATE35(DOTweenModuleUI.DOCounter).Method,
					new __GEN_DELEGATE36(DOTweenModuleUI.DOFade).Method,
					new __GEN_DELEGATE37(DOTweenModuleUI.DOText).Method,
					new __GEN_DELEGATE40(DOTweenModuleUI.DOBlendableColor).Method,
					new __GEN_DELEGATE311(UnityExtension.SetTimeStamp).Method,
					new __GEN_DELEGATE317(UnityExtension.SetLocalText).Method
				}
			},
			{
				typeof(Tween),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE41(TweenExtensions.Complete).Method,
					new __GEN_DELEGATE42(TweenExtensions.Complete).Method,
					new __GEN_DELEGATE43(TweenExtensions.Flip).Method,
					new __GEN_DELEGATE44(TweenExtensions.ForceInit).Method,
					new __GEN_DELEGATE45(TweenExtensions.Goto).Method,
					new __GEN_DELEGATE46(TweenExtensions.Kill).Method,
					new __GEN_DELEGATE47(TweenExtensions.Pause).Method,
					new __GEN_DELEGATE48(TweenExtensions.Play).Method,
					new __GEN_DELEGATE49(TweenExtensions.PlayBackwards).Method,
					new __GEN_DELEGATE50(TweenExtensions.PlayForward).Method,
					new __GEN_DELEGATE51(TweenExtensions.Restart).Method,
					new __GEN_DELEGATE52(TweenExtensions.Rewind).Method,
					new __GEN_DELEGATE53(TweenExtensions.SmoothRewind).Method,
					new __GEN_DELEGATE54(TweenExtensions.TogglePause).Method,
					new __GEN_DELEGATE55(TweenExtensions.GotoWaypoint).Method,
					new __GEN_DELEGATE56(TweenExtensions.WaitForCompletion).Method,
					new __GEN_DELEGATE57(TweenExtensions.WaitForRewind).Method,
					new __GEN_DELEGATE58(TweenExtensions.WaitForKill).Method,
					new __GEN_DELEGATE59(TweenExtensions.WaitForElapsedLoops).Method,
					new __GEN_DELEGATE60(TweenExtensions.WaitForPosition).Method,
					new __GEN_DELEGATE61(TweenExtensions.WaitForStart).Method,
					new __GEN_DELEGATE62(TweenExtensions.CompletedLoops).Method,
					new __GEN_DELEGATE63(TweenExtensions.Delay).Method,
					new __GEN_DELEGATE64(TweenExtensions.ElapsedDelay).Method,
					new __GEN_DELEGATE65(TweenExtensions.Duration).Method,
					new __GEN_DELEGATE66(TweenExtensions.Elapsed).Method,
					new __GEN_DELEGATE67(TweenExtensions.ElapsedPercentage).Method,
					new __GEN_DELEGATE68(TweenExtensions.ElapsedDirectionalPercentage).Method,
					new __GEN_DELEGATE69(TweenExtensions.IsActive).Method,
					new __GEN_DELEGATE70(TweenExtensions.IsBackwards).Method,
					new __GEN_DELEGATE71(TweenExtensions.IsComplete).Method,
					new __GEN_DELEGATE72(TweenExtensions.IsInitialized).Method,
					new __GEN_DELEGATE73(TweenExtensions.IsPlaying).Method,
					new __GEN_DELEGATE74(TweenExtensions.Loops).Method,
					new __GEN_DELEGATE75(TweenExtensions.PathGetPoint).Method,
					new __GEN_DELEGATE76(TweenExtensions.PathGetDrawPoints).Method,
					new __GEN_DELEGATE77(TweenExtensions.PathLength).Method,
					new __GEN_DELEGATE78(TweenSettingsExtensions.SetAutoKill).Method,
					new __GEN_DELEGATE79(TweenSettingsExtensions.SetAutoKill).Method,
					new __GEN_DELEGATE80(TweenSettingsExtensions.SetId).Method,
					new __GEN_DELEGATE81(TweenSettingsExtensions.SetId).Method,
					new __GEN_DELEGATE82(TweenSettingsExtensions.SetId).Method,
					new __GEN_DELEGATE83(TweenSettingsExtensions.SetLink).Method,
					new __GEN_DELEGATE84(TweenSettingsExtensions.SetLink).Method,
					new __GEN_DELEGATE85(TweenSettingsExtensions.SetTarget).Method,
					new __GEN_DELEGATE86(TweenSettingsExtensions.SetLoops).Method,
					new __GEN_DELEGATE87(TweenSettingsExtensions.SetLoops).Method,
					new __GEN_DELEGATE88(TweenSettingsExtensions.SetEase).Method,
					new __GEN_DELEGATE89(TweenSettingsExtensions.SetEase).Method,
					new __GEN_DELEGATE90(TweenSettingsExtensions.SetEase).Method,
					new __GEN_DELEGATE91(TweenSettingsExtensions.SetEase).Method,
					new __GEN_DELEGATE92(TweenSettingsExtensions.SetEase).Method,
					new __GEN_DELEGATE93(TweenSettingsExtensions.SetRecyclable).Method,
					new __GEN_DELEGATE94(TweenSettingsExtensions.SetRecyclable).Method,
					new __GEN_DELEGATE95(TweenSettingsExtensions.SetUpdate).Method,
					new __GEN_DELEGATE96(TweenSettingsExtensions.SetUpdate).Method,
					new __GEN_DELEGATE97(TweenSettingsExtensions.SetUpdate).Method,
					new __GEN_DELEGATE98(TweenSettingsExtensions.OnStart).Method,
					new __GEN_DELEGATE99(TweenSettingsExtensions.OnPlay).Method,
					new __GEN_DELEGATE100(TweenSettingsExtensions.OnPause).Method,
					new __GEN_DELEGATE101(TweenSettingsExtensions.OnRewind).Method,
					new __GEN_DELEGATE102(TweenSettingsExtensions.OnUpdate).Method,
					new __GEN_DELEGATE103(TweenSettingsExtensions.OnStepComplete).Method,
					new __GEN_DELEGATE104(TweenSettingsExtensions.OnComplete).Method,
					new __GEN_DELEGATE105(TweenSettingsExtensions.OnKill).Method,
					new __GEN_DELEGATE106(TweenSettingsExtensions.OnWaypointChange).Method,
					new __GEN_DELEGATE107(TweenSettingsExtensions.SetAs).Method,
					new __GEN_DELEGATE108(TweenSettingsExtensions.SetAs).Method,
					new __GEN_DELEGATE122(TweenSettingsExtensions.SetDelay).Method,
					new __GEN_DELEGATE123(TweenSettingsExtensions.SetDelay).Method,
					new __GEN_DELEGATE124(TweenSettingsExtensions.SetRelative).Method,
					new __GEN_DELEGATE125(TweenSettingsExtensions.SetRelative).Method,
					new __GEN_DELEGATE126(TweenSettingsExtensions.SetSpeedBased).Method,
					new __GEN_DELEGATE127(TweenSettingsExtensions.SetSpeedBased).Method,
					new __GEN_DELEGATE214(ShortcutExtensions.DOTimeScale).Method,
					new __GEN_DELEGATE249(Extensions.SetSpecialStartupMode).Method,
					new __GEN_DELEGATE323(UnityUIExtension.ScriptOnComplete).Method
				}
			},
			{
				typeof(Sequence),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE109(TweenSettingsExtensions.Append).Method,
					new __GEN_DELEGATE110(TweenSettingsExtensions.Prepend).Method,
					new __GEN_DELEGATE111(TweenSettingsExtensions.Join).Method,
					new __GEN_DELEGATE112(TweenSettingsExtensions.Insert).Method,
					new __GEN_DELEGATE113(TweenSettingsExtensions.AppendInterval).Method,
					new __GEN_DELEGATE114(TweenSettingsExtensions.PrependInterval).Method,
					new __GEN_DELEGATE115(TweenSettingsExtensions.AppendCallback).Method,
					new __GEN_DELEGATE116(TweenSettingsExtensions.PrependCallback).Method,
					new __GEN_DELEGATE117(TweenSettingsExtensions.InsertCallback).Method
				}
			},
			{
				typeof(Tweener),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE118(TweenSettingsExtensions.From).Method,
					new __GEN_DELEGATE119(TweenSettingsExtensions.From).Method
				}
			},
			{
				typeof(TweenerCore<Color, Color, ColorOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE120(TweenSettingsExtensions.From).Method,
					new __GEN_DELEGATE136(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE419(TweenSettingsExtensions.From).Method,
					new __GEN_DELEGATE426(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Vector3, Vector3, VectorOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE121(TweenSettingsExtensions.From).Method,
					new __GEN_DELEGATE131(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE132(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<float, float, FloatOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE128(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE420(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Vector2, Vector2, VectorOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE129(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE130(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE421(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE422(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Vector4, Vector4, VectorOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE133(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE134(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE423(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE424(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Quaternion, Vector3, QuaternionOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE135(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE425(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Rect, Rect, RectOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE137(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE427(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<string, string, StringOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE138(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE428(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE139(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE140(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE429(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE430(TweenSettingsExtensions.SetOptions).Method
				}
			},
			{
				typeof(TweenerCore<Vector3, Path, PathOptions>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE141(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE142(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE143(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE144(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE145(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE146(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE147(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE148(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE431(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE432(TweenSettingsExtensions.SetOptions).Method,
					new __GEN_DELEGATE433(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE434(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE435(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE436(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE437(TweenSettingsExtensions.SetLookAt).Method,
					new __GEN_DELEGATE438(TweenSettingsExtensions.SetLookAt).Method
				}
			},
			{
				typeof(Camera),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE149(ShortcutExtensions.DOAspect).Method,
					new __GEN_DELEGATE150(ShortcutExtensions.DOColor).Method,
					new __GEN_DELEGATE151(ShortcutExtensions.DOFarClipPlane).Method,
					new __GEN_DELEGATE152(ShortcutExtensions.DOFieldOfView).Method,
					new __GEN_DELEGATE153(ShortcutExtensions.DONearClipPlane).Method,
					new __GEN_DELEGATE154(ShortcutExtensions.DOOrthoSize).Method,
					new __GEN_DELEGATE155(ShortcutExtensions.DOPixelRect).Method,
					new __GEN_DELEGATE156(ShortcutExtensions.DORect).Method,
					new __GEN_DELEGATE157(ShortcutExtensions.DOShakePosition).Method,
					new __GEN_DELEGATE158(ShortcutExtensions.DOShakePosition).Method,
					new __GEN_DELEGATE159(ShortcutExtensions.DOShakeRotation).Method,
					new __GEN_DELEGATE160(ShortcutExtensions.DOShakeRotation).Method
				}
			},
			{
				typeof(Light),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE161(ShortcutExtensions.DOColor).Method,
					new __GEN_DELEGATE162(ShortcutExtensions.DOIntensity).Method,
					new __GEN_DELEGATE163(ShortcutExtensions.DOShadowStrength).Method,
					new __GEN_DELEGATE215(ShortcutExtensions.DOBlendableColor).Method,
					new __GEN_DELEGATE439(ShortcutExtensions.DOColor).Method,
					new __GEN_DELEGATE440(ShortcutExtensions.DOIntensity).Method,
					new __GEN_DELEGATE441(ShortcutExtensions.DOShadowStrength).Method,
					new __GEN_DELEGATE444(ShortcutExtensions.DOBlendableColor).Method
				}
			},
			{
				typeof(LineRenderer),
				new List<MethodInfo> { new __GEN_DELEGATE164(ShortcutExtensions.DOColor).Method }
			},
			{
				typeof(Material),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE165(ShortcutExtensions.DOColor).Method,
					new __GEN_DELEGATE166(ShortcutExtensions.DOColor).Method,
					new __GEN_DELEGATE167(ShortcutExtensions.DOColor).Method,
					new __GEN_DELEGATE168(ShortcutExtensions.DOFade).Method,
					new __GEN_DELEGATE169(ShortcutExtensions.DOFade).Method,
					new __GEN_DELEGATE170(ShortcutExtensions.DOFade).Method,
					new __GEN_DELEGATE171(ShortcutExtensions.DOFloat).Method,
					new __GEN_DELEGATE172(ShortcutExtensions.DOFloat).Method,
					new __GEN_DELEGATE173(ShortcutExtensions.DOOffset).Method,
					new __GEN_DELEGATE174(ShortcutExtensions.DOOffset).Method,
					new __GEN_DELEGATE175(ShortcutExtensions.DOTiling).Method,
					new __GEN_DELEGATE176(ShortcutExtensions.DOTiling).Method,
					new __GEN_DELEGATE177(ShortcutExtensions.DOVector).Method,
					new __GEN_DELEGATE178(ShortcutExtensions.DOVector).Method,
					new __GEN_DELEGATE216(ShortcutExtensions.DOBlendableColor).Method,
					new __GEN_DELEGATE217(ShortcutExtensions.DOBlendableColor).Method,
					new __GEN_DELEGATE218(ShortcutExtensions.DOBlendableColor).Method,
					new __GEN_DELEGATE226(ShortcutExtensions.DOComplete).Method,
					new __GEN_DELEGATE228(ShortcutExtensions.DOKill).Method,
					new __GEN_DELEGATE230(ShortcutExtensions.DOFlip).Method,
					new __GEN_DELEGATE232(ShortcutExtensions.DOGoto).Method,
					new __GEN_DELEGATE234(ShortcutExtensions.DOPause).Method,
					new __GEN_DELEGATE236(ShortcutExtensions.DOPlay).Method,
					new __GEN_DELEGATE238(ShortcutExtensions.DOPlayBackwards).Method,
					new __GEN_DELEGATE240(ShortcutExtensions.DOPlayForward).Method,
					new __GEN_DELEGATE242(ShortcutExtensions.DORestart).Method,
					new __GEN_DELEGATE244(ShortcutExtensions.DORewind).Method,
					new __GEN_DELEGATE246(ShortcutExtensions.DOSmoothRewind).Method,
					new __GEN_DELEGATE248(ShortcutExtensions.DOTogglePause).Method
				}
			},
			{
				typeof(TrailRenderer),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE179(ShortcutExtensions.DOResize).Method,
					new __GEN_DELEGATE180(ShortcutExtensions.DOTime).Method,
					new __GEN_DELEGATE442(ShortcutExtensions.DOResize).Method,
					new __GEN_DELEGATE443(ShortcutExtensions.DOTime).Method
				}
			},
			{
				typeof(Transform),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE181(ShortcutExtensions.DOMove).Method,
					new __GEN_DELEGATE182(ShortcutExtensions.DOMoveX).Method,
					new __GEN_DELEGATE183(ShortcutExtensions.DOMoveY).Method,
					new __GEN_DELEGATE184(ShortcutExtensions.DOMoveZ).Method,
					new __GEN_DELEGATE185(ShortcutExtensions.DOLocalMove).Method,
					new __GEN_DELEGATE186(ShortcutExtensions.DOLocalMoveX).Method,
					new __GEN_DELEGATE187(ShortcutExtensions.DOLocalMoveY).Method,
					new __GEN_DELEGATE188(ShortcutExtensions.DOLocalMoveZ).Method,
					new __GEN_DELEGATE189(ShortcutExtensions.DORotate).Method,
					new __GEN_DELEGATE190(ShortcutExtensions.DORotateQuaternion).Method,
					new __GEN_DELEGATE191(ShortcutExtensions.DOLocalRotate).Method,
					new __GEN_DELEGATE192(ShortcutExtensions.DOLocalRotateQuaternion).Method,
					new __GEN_DELEGATE193(ShortcutExtensions.DOScale).Method,
					new __GEN_DELEGATE194(ShortcutExtensions.DOScale).Method,
					new __GEN_DELEGATE195(ShortcutExtensions.DOScaleX).Method,
					new __GEN_DELEGATE196(ShortcutExtensions.DOScaleY).Method,
					new __GEN_DELEGATE197(ShortcutExtensions.DOScaleZ).Method,
					new __GEN_DELEGATE198(ShortcutExtensions.DOLookAt).Method,
					new __GEN_DELEGATE199(ShortcutExtensions.DOPunchPosition).Method,
					new __GEN_DELEGATE200(ShortcutExtensions.DOPunchScale).Method,
					new __GEN_DELEGATE201(ShortcutExtensions.DOPunchRotation).Method,
					new __GEN_DELEGATE202(ShortcutExtensions.DOShakePosition).Method,
					new __GEN_DELEGATE203(ShortcutExtensions.DOShakePosition).Method,
					new __GEN_DELEGATE204(ShortcutExtensions.DOShakeRotation).Method,
					new __GEN_DELEGATE205(ShortcutExtensions.DOShakeRotation).Method,
					new __GEN_DELEGATE206(ShortcutExtensions.DOShakeScale).Method,
					new __GEN_DELEGATE207(ShortcutExtensions.DOShakeScale).Method,
					new __GEN_DELEGATE208(ShortcutExtensions.DOJump).Method,
					new __GEN_DELEGATE209(ShortcutExtensions.DOLocalJump).Method,
					new __GEN_DELEGATE210(ShortcutExtensions.DOPath).Method,
					new __GEN_DELEGATE211(ShortcutExtensions.DOLocalPath).Method,
					new __GEN_DELEGATE212(ShortcutExtensions.DOPath).Method,
					new __GEN_DELEGATE213(ShortcutExtensions.DOLocalPath).Method,
					new __GEN_DELEGATE219(ShortcutExtensions.DOBlendableMoveBy).Method,
					new __GEN_DELEGATE220(ShortcutExtensions.DOBlendableLocalMoveBy).Method,
					new __GEN_DELEGATE221(ShortcutExtensions.DOBlendableRotateBy).Method,
					new __GEN_DELEGATE222(ShortcutExtensions.DOBlendableLocalRotateBy).Method,
					new __GEN_DELEGATE223(ShortcutExtensions.DOBlendablePunchRotation).Method,
					new __GEN_DELEGATE224(ShortcutExtensions.DOBlendableScaleBy).Method,
					new __GEN_DELEGATE263(TransformExtentions.GetOrAddTransform).Method,
					new __GEN_DELEGATE264(TransformExtentions.GetOrAddTransform).Method,
					new __GEN_DELEGATE265(TransformExtentions.GetOrAddTransform).Method,
					new __GEN_DELEGATE283(UnityExtensionBase.SetPositionX).Method,
					new __GEN_DELEGATE284(UnityExtensionBase.SetPositionY).Method,
					new __GEN_DELEGATE285(UnityExtensionBase.SetPositionZ).Method,
					new __GEN_DELEGATE286(UnityExtensionBase.AddPositionX).Method,
					new __GEN_DELEGATE287(UnityExtensionBase.AddPositionY).Method,
					new __GEN_DELEGATE288(UnityExtensionBase.AddPositionZ).Method,
					new __GEN_DELEGATE289(UnityExtensionBase.SetLocalPositionX).Method,
					new __GEN_DELEGATE290(UnityExtensionBase.SetLocalPositionY).Method,
					new __GEN_DELEGATE291(UnityExtensionBase.SetLocalPositionZ).Method,
					new __GEN_DELEGATE292(UnityExtensionBase.AddLocalPositionX).Method,
					new __GEN_DELEGATE293(UnityExtensionBase.AddLocalPositionY).Method,
					new __GEN_DELEGATE294(UnityExtensionBase.AddLocalPositionZ).Method,
					new __GEN_DELEGATE295(UnityExtensionBase.SetLocalScaleX).Method,
					new __GEN_DELEGATE296(UnityExtensionBase.SetLocalScaleY).Method,
					new __GEN_DELEGATE297(UnityExtensionBase.SetLocalScaleZ).Method,
					new __GEN_DELEGATE298(UnityExtensionBase.AddLocalScaleX).Method,
					new __GEN_DELEGATE299(UnityExtensionBase.AddLocalScaleY).Method,
					new __GEN_DELEGATE300(UnityExtensionBase.AddLocalScaleZ).Method,
					new __GEN_DELEGATE301(UnityExtensionBase.FindChildByName).Method,
					new __GEN_DELEGATE302(UnityExtensionBase.FindAndGetComponent).Method,
					new __GEN_DELEGATE303(UnityExtensionBase.GetComponent_RectTransform).Method,
					new __GEN_DELEGATE305(UnityExtensionBase.GetComponent_Text).Method,
					new __GEN_DELEGATE307(UnityExtensionBase.GetComponent_Image).Method,
					new __GEN_DELEGATE309(UnityExtensionBase.GetComponent_Button).Method,
					new __GEN_DELEGATE316(UnityExtension.FindId).Method,
					new __GEN_DELEGATE368(xLuaOptiUtils.Set_position).Method,
					new __GEN_DELEGATE369(xLuaOptiUtils.Set_positionX).Method,
					new __GEN_DELEGATE370(xLuaOptiUtils.Set_positionY).Method,
					new __GEN_DELEGATE371(xLuaOptiUtils.Set_positionZ).Method,
					new __GEN_DELEGATE372(xLuaOptiUtils.Get_position).Method,
					new __GEN_DELEGATE373(xLuaOptiUtils.Set_localPosition).Method,
					new __GEN_DELEGATE374(xLuaOptiUtils.Get_localPosition).Method,
					new __GEN_DELEGATE375(xLuaOptiUtils.Set_localScale).Method,
					new __GEN_DELEGATE376(xLuaOptiUtils.Get_localScale).Method,
					new __GEN_DELEGATE377(xLuaOptiUtils.Get_lossyScale).Method,
					new __GEN_DELEGATE378(xLuaOptiUtils.Set_eulerAngles).Method,
					new __GEN_DELEGATE379(xLuaOptiUtils.Get_eulerAngles).Method,
					new __GEN_DELEGATE380(xLuaOptiUtils.Set_localEulerAngles).Method,
					new __GEN_DELEGATE381(xLuaOptiUtils.Get_localEulerAngles).Method,
					new __GEN_DELEGATE382(xLuaOptiUtils.Get_rotation).Method,
					new __GEN_DELEGATE383(xLuaOptiUtils.Set_rotation).Method,
					new __GEN_DELEGATE384(xLuaOptiUtils.Get_localRotation).Method,
					new __GEN_DELEGATE385(xLuaOptiUtils.Set_localRotation).Method,
					new __GEN_DELEGATE386(xLuaOptiUtils.Get_forward).Method,
					new __GEN_DELEGATE387(xLuaOptiUtils.Set_forward).Method,
					new __GEN_DELEGATE388(xLuaOptiUtils.Get_right).Method,
					new __GEN_DELEGATE389(xLuaOptiUtils.Set_right).Method,
					new __GEN_DELEGATE390(xLuaOptiUtils.Get_up).Method,
					new __GEN_DELEGATE391(xLuaOptiUtils.Set_up).Method,
					new __GEN_DELEGATE392(xLuaOptiUtils.Reset).Method
				}
			},
			{
				typeof(Component),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE225(ShortcutExtensions.DOComplete).Method,
					new __GEN_DELEGATE227(ShortcutExtensions.DOKill).Method,
					new __GEN_DELEGATE229(ShortcutExtensions.DOFlip).Method,
					new __GEN_DELEGATE231(ShortcutExtensions.DOGoto).Method,
					new __GEN_DELEGATE233(ShortcutExtensions.DOPause).Method,
					new __GEN_DELEGATE235(ShortcutExtensions.DOPlay).Method,
					new __GEN_DELEGATE237(ShortcutExtensions.DOPlayBackwards).Method,
					new __GEN_DELEGATE239(ShortcutExtensions.DOPlayForward).Method,
					new __GEN_DELEGATE241(ShortcutExtensions.DORestart).Method,
					new __GEN_DELEGATE243(ShortcutExtensions.DORewind).Method,
					new __GEN_DELEGATE245(ShortcutExtensions.DOSmoothRewind).Method,
					new __GEN_DELEGATE247(ShortcutExtensions.DOTogglePause).Method,
					new __GEN_DELEGATE259(ComponentExtensions.GetOrAddComponent).Method
				}
			},
			{
				typeof(UnityEngine.Object),
				new List<MethodInfo> { new __GEN_DELEGATE250(UnityEngineObjectExtention.IsNull).Method }
			},
			{
				typeof(GameObject),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE251(UnityEngineGameObjectExtention.GameObjectCreatePool).Method,
					new __GEN_DELEGATE252(UnityEngineGameObjectExtention.GameObjectSpawn).Method,
					new __GEN_DELEGATE253(UnityEngineGameObjectExtention.GameObjectSpawn).Method,
					new __GEN_DELEGATE254(UnityEngineGameObjectExtention.GameObjectRecycle).Method,
					new __GEN_DELEGATE255(UnityEngineGameObjectExtention.GameObjectRecycleAll).Method,
					new __GEN_DELEGATE256(UnityEngineGameObjectExtention.GameObjectDestroyAll).Method,
					new __GEN_DELEGATE257(UnityEngineGameObjectExtention.CountPooled).Method,
					new __GEN_DELEGATE258(UnityEngineGameObjectExtention.GetRootParent).Method,
					new __GEN_DELEGATE260(ComponentExtensions.GetOrAddComponent).Method,
					new __GEN_DELEGATE276(UnityExtensionBase.Instantiate).Method,
					new __GEN_DELEGATE277(UnityExtensionBase.Destroy).Method,
					new __GEN_DELEGATE278(UnityExtensionBase.InScene).Method,
					new __GEN_DELEGATE279(UnityExtensionBase.SetLayerRecursively).Method,
					new __GEN_DELEGATE282(UnityExtensionBase.DestroyEx).Method,
					new __GEN_DELEGATE304(UnityExtensionBase.GetComponent_RectTransform).Method,
					new __GEN_DELEGATE306(UnityExtensionBase.GetComponent_Text).Method,
					new __GEN_DELEGATE308(UnityExtensionBase.GetComponent_Image).Method,
					new __GEN_DELEGATE310(UnityExtensionBase.GetComponent_Button).Method,
					new __GEN_DELEGATE337(UnityUIExtension.TryActive).Method
				}
			},
			{
				typeof(ScrollView),
				new List<MethodInfo> { new __GEN_DELEGATE262(ComponentExtensions.ScrollView_EndDrag).Method }
			},
			{
				typeof(string),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE266(UnityExtensionBase.ToStrList).Method,
					new __GEN_DELEGATE267(UnityExtensionBase.ToIntList).Method,
					new __GEN_DELEGATE268(UnityExtensionBase.ToInt).Method,
					new __GEN_DELEGATE271(UnityExtensionBase.ToFloat).Method,
					new __GEN_DELEGATE275(UnityExtensionBase.ToLong).Method,
					new __GEN_DELEGATE342(StringUtilsExt.IsNullOrEmpty).Method,
					new __GEN_DELEGATE343(StringUtilsExt.IsInt).Method,
					new __GEN_DELEGATE344(StringUtilsExt.FixNewLine).Method,
					new __GEN_DELEGATE345(StringUtilsExt.GetFileNameNoExtension).Method,
					new __GEN_DELEGATE346(StringUtilsExt.GetFileName).Method,
					new __GEN_DELEGATE347(StringUtilsExt.GetLengthByChar).Method,
					new __GEN_DELEGATE348(StringUtilsExt.SubstringEx).Method,
					new __GEN_DELEGATE445(UnityExtensionBase.ToStrList).Method,
					new __GEN_DELEGATE446(UnityExtensionBase.ToIntList).Method,
					new __GEN_DELEGATE447(UnityExtensionBase.ToInt).Method,
					new __GEN_DELEGATE449(UnityExtensionBase.ToFloat).Method,
					new __GEN_DELEGATE452(UnityExtensionBase.ToLong).Method,
					new __GEN_DELEGATE454(StringUtilsExt.IsNullOrEmpty).Method,
					new __GEN_DELEGATE455(StringUtilsExt.IsInt).Method,
					new __GEN_DELEGATE456(StringUtilsExt.FixNewLine).Method,
					new __GEN_DELEGATE457(StringUtilsExt.GetFileNameNoExtension).Method,
					new __GEN_DELEGATE458(StringUtilsExt.GetFileName).Method,
					new __GEN_DELEGATE459(StringUtilsExt.GetLengthByChar).Method,
					new __GEN_DELEGATE460(StringUtilsExt.SubstringEx).Method
				}
			},
			{
				typeof(object),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE269(UnityExtensionBase.ToInt).Method,
					new __GEN_DELEGATE274(UnityExtensionBase.ToFloat).Method
				}
			},
			{
				typeof(ReadOnlySpan<char>),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE270(UnityExtensionBase.ToInt).Method,
					new __GEN_DELEGATE272(UnityExtensionBase.ToFloat).Method,
					new __GEN_DELEGATE273(UnityExtensionBase.ToULong).Method,
					new __GEN_DELEGATE448(UnityExtensionBase.ToInt).Method,
					new __GEN_DELEGATE450(UnityExtensionBase.ToFloat).Method,
					new __GEN_DELEGATE451(UnityExtensionBase.ToULong).Method
				}
			},
			{
				typeof(Vector3),
				new List<MethodInfo> { new __GEN_DELEGATE280(UnityExtensionBase.ToVector2).Method }
			},
			{
				typeof(Vector2),
				new List<MethodInfo> { new __GEN_DELEGATE281(UnityExtensionBase.ToVector3).Method }
			},
			{
				typeof(SimpleAnimation),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE312(UnityExtension.PlayId).Method,
					new __GEN_DELEGATE313(UnityExtension.PlayQueuedId).Method
				}
			},
			{
				typeof(Animator),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE314(UnityExtension.PlayId).Method,
					new __GEN_DELEGATE315(UnityExtension.SetTriggerId).Method
				}
			},
			{
				typeof(TextMeshProUGUI),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE318(UnityExtension.SetLocalText).Method,
					new __GEN_DELEGATE417(xLuaOptiUtils.Native_SetText).Method,
					new __GEN_DELEGATE418(xLuaOptiUtils.SetText_NotNative).Method
				}
			},
			{
				typeof(InputField),
				new List<MethodInfo> { new __GEN_DELEGATE319(UnityExtension.SetLocalText).Method }
			},
			{
				typeof(TMP_InputField),
				new List<MethodInfo> { new __GEN_DELEGATE320(UnityExtension.SetLocalText).Method }
			},
			{
				typeof(SuperTextMesh),
				new List<MethodInfo> { new __GEN_DELEGATE321(UnityExtension.SetLocalText).Method }
			},
			{
				typeof(UnityEventBase),
				new List<MethodInfo> { new __GEN_DELEGATE322(UnityEventEx.Clear).Method }
			},
			{
				typeof(CircleImage),
				new List<MethodInfo> { new __GEN_DELEGATE327(UnityUIExtension.LoadSprite).Method }
			},
			{
				typeof(SpriteRenderer),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE328(UnityUIExtension.LoadSprite).Method,
					new __GEN_DELEGATE329(UnityUIExtension.LoadSpriteAsync).Method,
					new __GEN_DELEGATE330(UnityUIExtension.LoadSpriteAsync).Method,
					new __GEN_DELEGATE338(UnityUIExtension.ConvertToSpriteMeshRender).Method,
					new __GEN_DELEGATE403(xLuaOptiUtils.Set_size).Method,
					new __GEN_DELEGATE404(xLuaOptiUtils.Get_size).Method,
					new __GEN_DELEGATE405(xLuaOptiUtils.Set_color).Method,
					new __GEN_DELEGATE406(xLuaOptiUtils.Set_color_r).Method,
					new __GEN_DELEGATE407(xLuaOptiUtils.Set_color_g).Method,
					new __GEN_DELEGATE408(xLuaOptiUtils.Set_color_b).Method,
					new __GEN_DELEGATE409(xLuaOptiUtils.Set_color_a).Method,
					new __GEN_DELEGATE411(xLuaOptiUtils.Get_color).Method,
					new __GEN_DELEGATE412(xLuaOptiUtils.Get_color_r).Method,
					new __GEN_DELEGATE413(xLuaOptiUtils.Get_color_g).Method,
					new __GEN_DELEGATE414(xLuaOptiUtils.Get_color_b).Method,
					new __GEN_DELEGATE415(xLuaOptiUtils.Get_color_a).Method
				}
			},
			{
				typeof(CircleMesh),
				new List<MethodInfo> { new __GEN_DELEGATE331(UnityUIExtension.LoadSprite).Method }
			},
			{
				typeof(CircleMeshInstanced),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE332(UnityUIExtension.LoadSprite).Method,
					new __GEN_DELEGATE453(UnityUIExtension.LoadSprite).Method
				}
			},
			{
				typeof(SpriteMeshRenderer),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE333(UnityUIExtension.LoadSprite).Method,
					new __GEN_DELEGATE410(xLuaOptiUtils.Set_color_a).Method
				}
			},
			{
				typeof(RawImage),
				new List<MethodInfo>
				{
					new __GEN_DELEGATE334(UnityUIExtension.LoadSprite).Method,
					new __GEN_DELEGATE335(UnityUIExtension.LoadSpriteAsync).Method,
					new __GEN_DELEGATE336(UnityUIExtension.LoadSpriteAsync).Method
				}
			},
			{
				typeof(Shadow),
				new List<MethodInfo> { new __GEN_DELEGATE416(xLuaOptiUtils.Set_color).Method }
			}
		};
		genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
		genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
	}
}
