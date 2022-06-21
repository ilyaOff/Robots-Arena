using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegActionController : LegController
{
    [SerializeField] private LegAction moving;
    [SerializeField] private float[] shift;
    private float timer = 0;
    protected override void Moving()
    {
        timer += Time.fixedDeltaTime;
        if (timer > moving.MaxTime)
            timer = 0;


        for (int i = 0; i < legs.Count; i++)
        {
            float k = i < legs.Count / 2 ? 0 : moving.MaxTime/2;
            legs[i].NormalizeVerticalAngle = CalculateAngle(moving.vertical, timer + shift[i]);
            legs[i].NormalizeHipAngle = CalculateAngle(moving.hip, k+timer + shift[i]);
            legs[i].NormalizeKneeAngle = CalculateAngle(moving.knee, k +timer + shift[i]);
        }
    }

    private float CalculateAngle(AnimationCurve curve, float time)
    {
        return curve.Evaluate(time/ moving.MaxTime);
    }
}
