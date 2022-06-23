using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegActionController : LegController
{
    [SerializeField] private LegAction moving;   
    [SerializeField] private bool[] inverse;
    [SerializeField] private float[] shift;
    [SerializeField] private int[] powerLegNumber;
    [SerializeField] private float powerForward = 0.001f;
    private float timer = 0;
    protected override void Moving()
    {
        timer += Time.fixedDeltaTime;
        if (timer > moving.MaxTime)
            timer = 0;


        for (int i = 0; i < legs.Count; i++)
        {
            float reverseTime = inverse[i] ?  moving.MaxTime/2 : 0 ;
            float time = timer + moving.MaxTime * shift[i];
            float deltaForward = moving.MaxTime*powerForward * (powerLegNumber[i]);
            legs[i].NormalizeVerticalAngle = CalculateAngle(moving.vertical, time);
            legs[i].NormalizeHipAngle = (1-deltaForward)*CalculateAngle(moving.hip, reverseTime + time);
            legs[i].NormalizeKneeAngle =(1+ deltaForward)*CalculateAngle(moving.knee, reverseTime + time);
        }
    }

    private float CalculateAngle(AnimationCurve curve, float time)
    {
        return curve.Evaluate(time/ moving.MaxTime);
    }
}
