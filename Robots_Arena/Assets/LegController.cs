using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LegController : MonoBehaviour
{
    [SerializeField] private List<Leg> legs;
    public int CountLegs => legs.Count;

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

        //NewBrain();
    }

    public void NewBrain(NeuralNetwork brain)
    {
        this.brain = brain; 
        inputBrain = new float[brain.Inputs];
        calculateAngle = new float[brain.Outputs];
    }

    public bool TryChangeTarget(Transform newTarget)
    { 
        if(newTarget != null)
        {
            target = newTarget;
            return true;
        }

        return false;
    }
    
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.O))
        {
            NewBrain();
        }*/
            
    }
    
    private void FixedUpdate()
    {
        calculateAngle = CalculateBrain();
        for (int i = 0; i < legs.Count; i++)
        {
            /*
            legs[i].NormalizeVerticalAngle = Mathf.Sin(Time.time * calculateAngle[3 * i]);
            legs[i].NormalizeHipAngle = Mathf.Sin(Time.time * calculateAngle[3 * i + 1]);
            legs[i].NormalizeKneeAngle = Mathf.Sin(Time.time * calculateAngle[3 * i + 2]);
            */
            /* 
            legs[i].NormalizeVerticalAngle = (calculateAngle[3 * i]*2-1)*Mathf.Sin(Time.time );
            legs[i].NormalizeHipAngle = (calculateAngle[3 * i+1] * 2 - 1) * Mathf.Sin(Time.time);
            legs[i].NormalizeKneeAngle = (calculateAngle[3 * i+2] * 2 - 1) * Mathf.Sin(Time.time);
            */

            legs[i].NormalizeVerticalAngle += CalculateAngle(calculateAngle[3 * i]);
            legs[i].NormalizeHipAngle += CalculateAngle(calculateAngle[3 * i+1]);
            legs[i].NormalizeKneeAngle += CalculateAngle(calculateAngle[3 * i+2]);

        }
        //Debug.Log(calculateAngle[0] + " " + Mathf.Sin(Time.time * calculateAngle[0]));
    }

    private float CalculateAngle(float output)
    {
        float threshold = 0.25f;
        float result = 0f;
        if (Mathf.Abs(output) > threshold*3)
            result = 3;
        else if (Mathf.Abs(output) > threshold*2)
            result = 2;
        else if (Mathf.Abs(output) > threshold )
            result = 1;

        return  result*Time.fixedDeltaTime*Mathf.Sign(output);        
    }

    private float[] CalculateBrain()
    {
        Vector3 direction = Vector3.zero;
        if(target != null)
         direction = (target.position - transform.position);
           

        for (int i = 0; i < legs.Count; i++)
        {
            inputBrain[3 * i] = legs[i].NormalizeVerticalAngle;
            inputBrain[3 * i + 1] = legs[i].NormalizeHipAngle;
            inputBrain[3 * i + 2] = legs[i].NormalizeKneeAngle;
        }
        int shift = 3 * legs.Count;
        inputBrain[0+ shift] = direction.x;
        inputBrain[1+shift] = direction.y;
        inputBrain[2+ shift] = direction.z;
        inputBrain[3 + shift] = direction.magnitude;

        return brain.CalculeteOutput(inputBrain);
    }
}
