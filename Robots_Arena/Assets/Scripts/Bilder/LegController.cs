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
    private bool usedBrain = false;

    private void Awake()
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
        usedBrain = true;

        body.isKinematic = false;

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

    public void InitialPosition()
    {
        usedBrain = false;
        body.isKinematic = true;
        for (int i = 0; i < legs.Count; i++)
        {
            legs[i].Restart();
            /*
            legs[i].NormalizeVerticalAngle = 0.5f;
            legs[i].NormalizeHipAngle = 0.9f;
            legs[i].NormalizeKneeAngle = 0.01f;
            */
        }
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
        Moving();
    }

    private void Moving()
    {
        if (!usedBrain) return;

        calculateAngle = CalculateBrain();
        for (int i = 0; i < legs.Count; i++)
        {
            legs[i].NormalizeVerticalAngle += CalculateAngle(calculateAngle[3 * i]);
            legs[i].NormalizeHipAngle += CalculateAngle(calculateAngle[3 * i + 1]);
            legs[i].NormalizeKneeAngle += CalculateAngle(calculateAngle[3 * i + 2]);
        }
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
        int shift = 0;
        for (int i = 0; i < legs.Count; i++)
        {
            inputBrain[3 * i] = legs[i].NormalizeVerticalAngle;
            inputBrain[3 * i + 1] = legs[i].NormalizeHipAngle;
            inputBrain[3 * i + 2] = legs[i].NormalizeKneeAngle;
        }

        shift = 3 * legs.Count;

        Vector3 direction = Vector3.zero;
        if (target != null)
            direction = (target.position - transform.position);
        
        inputBrain[0 + shift] = direction.x;
        inputBrain[1 + shift] = direction.y;
        inputBrain[2 + shift] = direction.z;
        inputBrain[3 + shift] = direction.magnitude;

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        Vector3 directionProject = Vector3.ProjectOnPlane(direction, Vector3.up);
        inputBrain[4 + shift] = Vector3.Angle(forward, directionProject);

        directionProject = Vector3.ProjectOnPlane(direction, transform.right);
        inputBrain[5 + shift] = Vector3.Angle(transform.up, directionProject);
                

        return brain.CalculeteOutput(inputBrain);
    }
}
