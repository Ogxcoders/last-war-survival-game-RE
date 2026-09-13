using System;

namespace Leopotam.EcsLite;

public interface IEcsPoolDelegate
{
	Type DelegateType { get; }
}
