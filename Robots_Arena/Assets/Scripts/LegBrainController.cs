using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegBrainController : LegController, INeuralNetworkAgent
{
    public NeuralNetwork Brain { get; private set; }
    private float[] inputBrain;

    public void NewBrain(NeuralNetwork brain)
    {
        Brain = brain;
        inputBrain = new float[brain.Inputs];

        InitialPosition();
    
    }
    protected override void Moving()
    {
       
        float[] calculateAngle = CalculateBrain();
        for (int i = 0; i<legs.Count; i++)
        {
            legs[i].NormalizeVerticalAngle += CalculateAngle(calculateAngle[3 * i]);
            legs[i].NormalizeHipAngle += CalculateAngle(calculateAngle[3 * i + 1]);
            legs[i].NormalizeKneeAngle += CalculateAngle(calculateAngle[3 * i + 2]);
        }
    }
    private float CalculateAngle(float output)
    {
        float threshold = 0.25f;
        int result = Mathf.RoundToInt(output / threshold);

        return output * Time.fixedDeltaTime;
    }

    private float[] CalculateBrain()
    {
        int shift = 0;
        int legParametrs = 10;
        for (int i = 0; i < legs.Count; i++)
        {
            int k = legParametrs * i;
            inputBrain[k ] = legs[i].InGround ? 1 : -1;
            PushJointPosition(k + 1 , legs[i].Vertical);
            PushJointPosition(k+ 4, legs[i].Hip);
            PushJointPosition(k + 7, legs[i].Knee);
        }

        shift = legParametrs * legs.Count;

        Vector3 direction = _navigator.Direction();
        inputBrain[0 + shift] = direction.magnitude;

        direction = direction.normalized;
        inputBrain[1 + shift] = direction.x;
        inputBrain[2 + shift] = direction.y;
        inputBrain[3 + shift] = direction.z;
        

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 directionProject = Vector3.ProjectOnPlane(direction, Vector3.up);
        inputBrain[4 + shift] = Vector3.Angle(forward, directionProject);

        directionProject = Vector3.ProjectOnPlane(direction, transform.right);
        inputBrain[5 + shift] = Vector3.Angle(transform.up, directionProject);


        return Brain.CalculeteOutput(inputBrain);
    }
    
    private void PushJointPosition(int shift, Vector3 jointPosition)
    {
        inputBrain[shift] = jointPosition.x;
        inputBrain[shift + 1] = jointPosition.y;
        inputBrain[shift + 2] = jointPosition.z;
    }

}
