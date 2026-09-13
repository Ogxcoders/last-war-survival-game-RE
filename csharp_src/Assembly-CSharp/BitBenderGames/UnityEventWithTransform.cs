using System;
using UnityEngine;
using UnityEngine.Events;

namespace BitBenderGames;

[Serializable]
public class UnityEventWithTransform : UnityEvent<Transform>
{
}
