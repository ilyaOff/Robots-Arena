using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFollowing
{
    UnityEngine.Transform Target { get; }
    bool TryChangeTarget(UnityEngine.Transform target);
}
