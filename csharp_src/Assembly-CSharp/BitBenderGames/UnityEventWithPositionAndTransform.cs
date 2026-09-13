using System;
using UnityEngine;
using UnityEngine.Events;

namespace BitBenderGames;

[Serializable]
public class UnityEventWithPositionAndTransform : UnityEvent<Vector3, Transform>
{
}
