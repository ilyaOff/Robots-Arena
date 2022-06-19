using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatorToTarget : Navigator, ITargetFollowing
{
    public UnityEngine.Transform Target { get; private set; }

    public override Vector3 Direction()
    {
        return Target.position - transform.position;
    }

    public bool TryChangeTarget(UnityEngine.Transform target)
    {
        if (target is null)
        {
            Debug.LogError("Navigator->target");
            return false;
        }

        Target = target;

        return true;
    }
}
