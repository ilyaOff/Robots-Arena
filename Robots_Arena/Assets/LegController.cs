using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LegController : MonoBehaviour
{
    [SerializeField] private List<Leg> legs;

    private Rigidbody body;

    private NeuralNetwork brain;
    [SerializeField] private Transform target;
    private float[] inputBrain;
    private float[] calculateAngle;
    private void Start()
    {
        body = this.GetComponent<Rigidbody>();
        legs = this.gameObject.GetComponentsInChildren<Leg>().ToList();
        if (legs is null || legs.Count == 0)
        {
            Debug.LogError("Legs is empty!");
            Destroy(this);
            return;
        }

        foreach (Leg leg in legs)
        {
            leg.AttachToBody(body);
        }

        NewBrain();
    }

    private void NewBrain()
    {
        int inputLayer = 3//dimension of Target
                + legs.Count * 3;//angle of leg                
        int outputLayer = +legs.Count * 3;//angle of leg
                
        brain = new NeuralNetwork(new int[]
                    {
                        inputLayer, outputLayer
                    });
        inputBrain = new float[inputLayer];
        calculateAngle = new float[outputLayer];
    }

    private void FixedUpdate()
    {
        calculateAngle = CalculateBrain();
        for (int i = 0; i < legs.Count; i++)
        {
            legs[i].NormalizeVerticalAngle = calculateAngle[3*i];
            legs[i].NormalizeHipAngle = calculateAngle[3*i+1];            
            legs[i].NormalizeKneeAngle = calculateAngle[3*i+2];
        }
    }

    private float[] CalculateBrain()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        inputBrain[0] = direction.x;
        inputBrain[1] = direction.y;
        inputBrain[2] = direction.z;

        for (int i = 0; i < legs.Count; i++)
        {
            inputBrain[3 * i + 3] = legs[i].NormalizeVerticalAngle;
            inputBrain[3 * i + 4] = legs[i].NormalizeHipAngle;
            inputBrain[3 * i + 5] = legs[i].NormalizeKneeAngle;
        }

        return brain.CalculeteOutput(inputBrain);
    }
}
