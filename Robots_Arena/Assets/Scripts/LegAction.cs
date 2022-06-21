using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAction : MonoBehaviour
{
    [SerializeField] public AnimationCurve vertical;
    [SerializeField] public AnimationCurve hip;
    [SerializeField] public AnimationCurve knee;
    [SerializeField] public float MaxTime = 1f;
}
