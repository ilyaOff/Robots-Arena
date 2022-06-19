using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public virtual Vector3 Direction()
    {
        return transform.forward;
    }
}
