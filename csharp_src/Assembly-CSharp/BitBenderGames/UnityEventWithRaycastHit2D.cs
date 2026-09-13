using System;
using UnityEngine;
using UnityEngine.Events;

namespace BitBenderGames;

[Serializable]
public class UnityEventWithRaycastHit2D : UnityEvent<RaycastHit2D>
{
}
